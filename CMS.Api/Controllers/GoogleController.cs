using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;

namespace CMS.Api.Controllers
{
    public class GoogleController : BaseApiController
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<GoogleController> _logger;

        public GoogleController(ILogger<GoogleController> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            var data = (await _ticketService.GetTicketLabels()).Select(s => s.Name_TR).ToList();
            return data;
        }

        public async Task<IActionResult> SendDeneme(SendMailModel model)
        {
            try
            {
                _logger.LogInformation("mail için geldi");
                var ticketCreateResponse = await _ticketService.SendTicketReply(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mail exp.");

                return BadRequest(ex.Message);
            }

            //Log.Information(JsonConvert.SerializeObject(Request.Headers));
            return Ok(1);
        }


        public async Task<IActionResult> GetDeneme(ulong id)
        {
            try
            {
                GmailPushDataModel ff = new GmailPushDataModel()
                {
                    HistoryId = id
                };

                _logger.LogInformation("ticket için geldi");
                var ticketCreateResponse = _ticketService.SyncTicketFromGmail(ff);

                if (ticketCreateResponse.Exception != null && !string.IsNullOrEmpty(ticketCreateResponse.Exception.Message))
                {
                    _logger.LogError(ticketCreateResponse.Exception, "Ticket oluşturulamadı. Hata kodu {code}", ticketCreateResponse.Result);
                    throw ticketCreateResponse.Exception;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Push Notification okunamadı!");

                return BadRequest(ex.Message);
            }

            //Log.Information(JsonConvert.SerializeObject(Request.Headers));
            return Ok(1);
        }

        [HttpPost]
        public async Task<IActionResult> PubSubActionAsync([FromBody]PubSub pubSub)
        {
            int pushResp = 0;
            try
            {
                var decryptedData = Encoding.UTF8.GetString(Convert.FromBase64String(pubSub.Message.Data));

                var gData = JsonConvert.DeserializeObject<GmailPushDataModel>(decryptedData);

                pushResp = await _ticketService.SyncTicketFromGmail(gData);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Push Notification okunamadı!");

                return BadRequest(ex.Message);
            }

            //Log.Information(JsonConvert.SerializeObject(Request.Headers));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> WatchMail(string TopicName = "")
        {
            try
            {

                if (string.IsNullOrEmpty(TopicName))
                {
                    TopicName = "projects/my-cms-topic/topics/talep-cms-push";
                }

                var ff = await _ticketService.WatchMail(new WatchRequestModel()
                {
                    IsWatchRequest = true,
                    LabelList = new List<string> { "INBOX" },
                    TopicName = TopicName
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Hata aldık {pff}", ex.Message);
                return BadRequest("Mail Watched");
            }

            return Ok("Mail Watched");
        }

        [HttpGet]
        public async Task<IActionResult> StopWatchMail()
        {

            var ff = await _ticketService.WatchMail(new WatchRequestModel() { IsWatchRequest = false });

            return Ok("Mail stop watched");
        }

    }
}
