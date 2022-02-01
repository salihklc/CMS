using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;

namespace CMS.Infrastructure.Integrations.GoogleIntegration
{
    public class GMailIntegration
    {
        GmailService service;
        static string[] Scopes = {
            GmailService.Scope.GmailReadonly,
            GmailService.Scope.GmailModify,
            GmailService.Scope.MailGoogleCom
        };

        public GMailIntegration(string GmailCredentialPath,string AppEmailName,string User, string ApplicationName)
        {
            // Create Gmail API service.
            // var credential = GoogleCredential.FromFile(GmailCredentialPath);
            var certificate = new X509Certificate2(GmailCredentialPath, "notasecret", X509KeyStorageFlags.Exportable);

            ServiceAccountCredential credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(AppEmailName)
                {
                    Scopes = Scopes,
                    User = User
                }.FromCertificate(certificate)
            );

            service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

        }

        public ListHistoryResponse GetHistoryById(ulong historyId, string nextPageToken = "")
        {
            var historyReq = service.Users.History.List("me");

            historyReq.StartHistoryId = historyId;
            historyReq.PageToken = nextPageToken;

            historyReq.HistoryTypes = UsersResource.HistoryResource.ListRequest.HistoryTypesEnum.MessageAdded;

            return historyReq.Execute();
        }

        public Message GetMessageById(string messageId)
        {
            var messageReq = service.Users.Messages.Get("me", messageId);
            return messageReq.Execute();
        }

        public WatchResponse SubscribeGmail(string TopicName, List<string> Labels)
        {
            var watchReq = new WatchRequest();

            watchReq.LabelIds = Labels;

            watchReq.LabelFilterAction = "include";

            watchReq.TopicName = TopicName;

            var resp = service.Users.Watch(watchReq, "me").Execute();
            return resp;
            // Log.log(resp);
        }
        public string StopSubscribeGmail()
        {

            var resp = service.Users.Stop("me").Execute();

            return resp;
        }


        public Message SendMail(Message message, string userId)
        {
            var sendMailResponse = service.Users.Messages.Send(message, userId).Execute();

            return sendMailResponse;
        }

        public MessagePartBody GetAttachmentById(string messageId, string attachmentId)
        {
            var attachmentReq = service.Users.Messages.Attachments.Get("me", messageId, attachmentId);
            return attachmentReq.Execute();
        }


        public ListLabelsResponse GetLabels()
        {
            var historyReq = service.Users.Labels.List("me");

            var labels = historyReq.Execute();

            return labels;
        }

    }
}