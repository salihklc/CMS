using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using CMS.TaskManager.Business.Database;
using CMS.TaskManager.Business.Database.EntityModels;
using CMS.TaskManager.Business.Logic;
using CMS.TaskManager.Business.Logic.Models;

namespace CMS.TaskManager {
    public class Worker : BackgroundService {
        static readonly object DbLock = new object ();

        private readonly ILogger<Worker> _logger;
        private readonly IOptions<ApplicationConfig> _settings;
        private Timer _timerForWatcher;

        private Timer _timerForQueueRunner;

        public Worker (ILogger<Worker> logger, IOptions<ApplicationConfig> settings, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _settings = settings;

        }
        public IServiceProvider _serviceProvider { get; }

        public override Task StartAsync (CancellationToken cancellationToken) {

            //TaskWatcher("");
            //RunQue("");
            int WatcherTimer = 60;
            int QueueTimer = 30;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine ("Application Starting...");

            if (_settings.Value != null && _settings.Value.WatchTimer > 0) {
                WatcherTimer = _settings.Value.WatchTimer;
                QueueTimer = _settings.Value.QueueReaderTimer;
            }

            try {
                TaskManagerSettingsModel settings;

                using (var scope = _serviceProvider.CreateScope ()) {

                    var _context =
                        scope.ServiceProvider
                        .GetRequiredService<CmsDbContext> ();

                    settings = _context.TaskManagerSettings.FirstOrDefault();
                }

                WatcherTimer = settings.WatcherTimer;
                QueueTimer = settings.QueueTimer;

            } catch (Exception ex) {
                _logger.LogError (ex, "TaskManager Settings Okunamadı");
            }

            _timerForWatcher = new Timer (TaskWatcher, null, TimeSpan.Zero, TimeSpan.FromSeconds(WatcherTimer));

            _timerForQueueRunner = new Timer (RunQue, null, TimeSpan.Zero, TimeSpan.FromSeconds(QueueTimer));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Taskları okuyup que'ya dolduracak olan function
        /// </summary>
        /// <param name="state"></param>
        private void TaskWatcher (object state) {
            try {

                using (var scope = _serviceProvider.CreateScope ()) {

                    var _context =
                        scope.ServiceProvider
                        .GetRequiredService<CmsDbContext> ();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine ("Tasks Watching...");

                    var taskToAddQueue = _context.Workers.Where (w =>
                        w.StartDate <= DateTime.Now &&
                        w.ExpireDate >= DateTime.Now &&
                        w.EndDate >= DateTime.Now &&
                        w.LastRunDate.AddSeconds (w.ScheduleTime) <= DateTime.Now &&

                        DateTime.Now.TimeOfDay >= w.StartTimeOfDay &&
                        DateTime.Now.TimeOfDay <= w.EndTimeOfDay

                    ).ToList ();

                    List<TaskQueue> queueList = new List<TaskQueue> ();
                    foreach (var item in taskToAddQueue) {
                        Console.WriteLine ($"Tasks Found {item.JobIdx}");

                        int dayOfWeek = Convert.ToInt32( Math.Pow(2,((int) DateTime.Now.DayOfWeek)));
                        //     bitwise 127 max  &   
                        //    ( DAY OF WEEK     &   DB Day OF WEEK )                     == DAY OF WEEK
                        if ((item.DayOfWeek     &  dayOfWeek ) == dayOfWeek ) //dayofweek 0'dan başlıyor DB'de 1 den başlayarak tuttuk içinde varsa 👍
                        {
                            Console.WriteLine("Qeueu added to list");
                            queueList.Add (
                                new TaskQueue () {
                                    InsertDate = DateTime.Now,
                                        InsertUserIdx = -1,
                                        Status = 0,
                                        WorkerIdx = item.Idx,
                                        UpdateDate = DateTime.Now,
                                        UpdateUserIdx = -1
                                });
                        }

                    }

                    

                    _context.Queues.AddRange(queueList);

                    taskToAddQueue.ForEach(e => {

                        e.LastRunDate = DateTime.Now;

                    });
                    _context.Workers.UpdateRange(taskToAddQueue);

                    _context.SaveChanges();

                    Console.WriteLine("Task added to queue");
                }

            } catch (Exception ex) {
                Console.WriteLine ($"Tasks EXCEPTION {ex.Message}");
                _logger.LogError (ex, "TASK WATCHER OKUYAMADI !! ");
            }
        }

        /// <summary>
        /// Que'dan okuyup request atacak olan function
        /// </summary>
        /// <param name="state"></param>
        private void RunQue (object state) {
            try {
                using (var scope = _serviceProvider.CreateScope ()) {
                    var _context =
                        scope.ServiceProvider
                        .GetRequiredService<CmsDbContext> ();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine ($"Queue Running");

                    TaskQueue queuePopElement = null;
                    TaskWorkers worker = null;
                    TaskJobs job = null;

                    #region read queu and lock for running task
                    lock (DbLock) {

                        queuePopElement = _context.Queues.Where (f => f.Status == 0).FirstOrDefault ();

                        if (queuePopElement != null) {
                            
                            queuePopElement.Status = 1;

                            if (queuePopElement.Idx > 0) {
                                Console.WriteLine ($"Queue item found {queuePopElement.Idx}");

                                _context.Queues.Update (queuePopElement);
                                _context.SaveChanges ();
                            }
                        }

                    }
                    #endregion

                    if (queuePopElement != null) {
                        #region read worker data for send request 
                        lock (DbLock) {
                            worker = _context.Workers.Where (f => f.Idx == queuePopElement.WorkerIdx).FirstOrDefault ();

                            if (worker != null && worker.Idx > 0) {
                                job = _context.Jobs.Where (f => f.Idx == worker.JobIdx).FirstOrDefault ();
                            }

                        }
                        #endregion

                        #region call rest save response
                        if (queuePopElement != null && worker != null && job != null) {
                            var result = RestCaller.SendRequest (job.Method, job.Url, job.RequiredParams, worker.Params);

                            var newWorkerHistory = new TaskWorkersHistories () {
                                InsertDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                InsertUserIdx = -1,
                                UpdateUserIdx = -1,
                                WorkerIdx = queuePopElement.WorkerIdx,
                                Params = worker.Params,
                                WorkerData = JsonConvert.SerializeObject (worker),
                                JobData = job.RequiredParams,
                                Status = (result.HttpStatus == "200" ? 1 : 0),
                                Result = JsonConvert.SerializeObject (result),
                                RunDate = DateTime.Now
                            };

                            lock (DbLock) {
                                _context.WorkersHistories.Add (newWorkerHistory);
                                _context.SaveChanges ();
                            }

                        }
                        #endregion

                        #region remove queue
                        lock (DbLock) {
                            _context.Remove (queuePopElement);
                            _context.SaveChanges ();
                        }
                        #endregion
                    } else {
                        //No element on queue
                    }
                }
            } catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine ($"Run QUeue Hata {ex.Message}");

                _logger.LogError (ex, "Queue Okunamadı !!!");
            }

        }

        protected override Task ExecuteAsync (CancellationToken stoppingToken) {
            throw new NotImplementedException ();
        }
    }
}