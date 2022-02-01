
using Microsoft.Extensions.Options;
using CMS.Common.Interfaces;
using CMS.Common.Models.CommonModels;
using CMS.Core.Entities;
using CMS.Infrastructure.Integrations.GoogleIntegration;
using System.Linq;
using CMS.Common.AppConstants;
using System.Text;
using System;
using System.Collections.Generic;
using Google.Apis.Gmail.v1.Data;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;

namespace CMS.Infrastructure.TicketIntegration.Integrators
{
    public class GmailTicketIntegration : AbstractTicketIntegration
    {
        public readonly GMailIntegration _gmailInteration;
        private readonly ILogger<GmailTicketIntegration> _logger;
        public GmailTicketIntegration(IntegratorSettings settings, TypeOfTicketIntegration type, ILogger<GmailTicketIntegration> logger) : base(type)
        {
            _logger = logger;

            _logger.LogInformation("GmailTicketIntegration {path}", settings.GmailCredential.CredentialPath);

            using (var stream = new FileStream(settings.GmailCredential.CredentialPath,
                                                           FileMode.Open,
                                                           FileAccess.Read))
            {
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    string content = sr.ReadToEnd();
                    _logger.LogInformation("GmailTicketIntegration FILE DATA {data}", content);
                }

            }

            _gmailInteration = new GMailIntegration(
                settings.GmailCredential.CredentialPath,
                settings.GmailCredential.CredentialApplicationEmail,
                settings.GmailCredential.CredentialUser,
                settings.GmailCredential.ApplicationName);

        }
        public override List<MailTicket> GetTicketById(ulong Id)
        {
            List<MailTicket> returnMailList = new List<MailTicket>();

            var allHistoryFromTheId = GetHistories(Id, "");

            _logger.LogInformation("ghistory : {history}", JsonConvert.SerializeObject(allHistoryFromTheId));

            if (allHistoryFromTheId != null && allHistoryFromTheId.Count > 0)
            {

                /*
                    BURADA ŞÖYLE Bİ' BUG OLUAŞACAKTIR. ADAM BİZİM SİSTEMDEN RESPONSE ATARSA O DA MESSAGE ADDED'A GELİCEK 
                    VE DUPLİCATE KAYIT ATACAK.
                    BUNU DÜŞÜNECEZ.

                    Solved SENT olmayan labelleri al dedik
                */
                foreach (var item in allHistoryFromTheId)
                {
                    var messageId = item.MessagesAdded.FirstOrDefault().Message.Id;

                    var message = _gmailInteration.GetMessageById(messageId);
                    if (message != null && message.HistoryId > 0 && !message.LabelIds.Contains("SENT"))
                    {

                        _logger.LogInformation("getMessage : {message}", JsonConvert.SerializeObject(message));

                        MailTicket data = new MailTicket()
                        {
                            HistoryId = message.HistoryId ?? 0,
                            IntegrationName = IntegrationName,
                            IntegrationType = IntegrationType,
                            ThreadId = message.ThreadId,
                            ShortMessage = message.Snippet,
                            MessageId = message.Id
                        };


                        data.Headers = JsonConvert.SerializeObject(message.Payload.Headers.Select(f =>
                            new HeaderKeyValue { Key = f.Name, Value = f.Value }
                        ).ToList());

                        foreach (var headerItem in message.Payload.Headers)
                        {
                            switch (headerItem.Name)
                            {
                                case "Subject":
                                    data.Subject = headerItem.Value;
                                    break;
                                case "From":
                                    data.From = headerItem.Value;
                                    break;
                                case "Errors-To":
                                    data.ErrorsTo = headerItem.Value;
                                    break;
                                case "To":
                                    data.Delivered = headerItem.Value;
                                    break;
                                case "Delivered-To":
                                    data.DeliveredTo = headerItem.Value;
                                    break;
                                case "Date":
                                    try
                                    {
                                        data.ReceiveDate = Convert.ToDateTime(headerItem.Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        data.ReceiveDate = DateTime.Now;
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }


                        if (message.Payload.MimeType == "multipart/alternative")
                        {
                            foreach (var part in message.Payload.Parts)
                            {
                                var formattedData = part.Body.Data.RFC2822Decode();
                                // .Replace('-', '+');
                                //formattedData = formattedData.Replace('_', '/');

                                if (part.MimeType == "text/plain")
                                {
                                    data.MessageBodyText = formattedData;//Encoding.UTF8.GetString(Convert.FromBase64String(formattedData));
                                }

                                if (part.MimeType == "text/html")
                                {
                                    data.MessageBodyHtml = formattedData; //Encoding.UTF8.GetString(Convert.FromBase64String(formattedData));
                                }
                            }
                        }

                        if (message.Payload.MimeType == "multipart/mixed")
                        {
                            var AttachmentList = new List<Attachment>();

                            foreach (var part in message.Payload.Parts)
                            {

                                //Buna bir defa girer attachment olan mail'in ilk elemanı mesajı içerir.
                                if (part.MimeType == "multipart/alternative")
                                {
                                    foreach (var partOfPart in part.Parts)
                                    {
                                        var formattedData = partOfPart.Body.Data.RFC2822Decode();

                                        if (partOfPart.MimeType == "text/plain")
                                        {
                                            data.MessageBodyText = formattedData;//Encoding.UTF8.GetString(Convert.FromBase64String(formattedData));
                                        }

                                        if (partOfPart.MimeType == "text/html")
                                        {
                                            data.MessageBodyHtml = formattedData; //Encoding.UTF8.GetString(Convert.FromBase64String(formattedData));
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(part.Filename))
                                {
                                    var attachment = new Attachment();

                                    var atthId = part.Body.AttachmentId;
                                    var attachmentBody = _gmailInteration.GetAttachmentById(messageId, atthId);

                                    attachment.FileName = part.Filename;

                                    /* //attachmentBody i�erisinden data almak!
                                    foreach (var header in part.Headers)
                                    {
                                        switch (header.Name)
                                        {
                                            case "Content-Type":
                                                //Gerekli olursa alınabilir
                                                break;
                                            case "Content-Description":
                                                attachment.FileName = part.Filename;
                                                break;
                                            case "Content-Disposition":
                                                //Gerekli olursa alınabilir
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    */
                                    attachment.Data = attachmentBody.Data;

                                    var attachmentBase64 = attachmentBody.Data.Replace('-', '+').Replace('_', '/');

                                    attachment.Data = attachmentBase64;
                                    attachment.Size = attachmentBody.Size ?? 0;
                                    attachment.ByteData = Convert.FromBase64String(attachmentBase64);

                                    AttachmentList.Add(attachment);

                                }
                            }

                            data.Attechments = AttachmentList;
                        }

                        try
                        {

                            _logger.LogInformation("returnMailList add data : {data}", JsonConvert.SerializeObject(data));

                        }
                        catch (Exception)
                        {
                        }

                        returnMailList.Add(data);
                    }
                }

                return returnMailList;
            }
            else
            {
                _logger.LogInformation("ghistory Not found");
                return null;
            }

        }

        private List<History> GetHistories(ulong Id, string PageToken)
        {
            var ghistory = _gmailInteration.GetHistoryById(Id, PageToken);

            List<History> returnHistoryList = new List<History>(ghistory.History);

            if (!string.IsNullOrEmpty(ghistory.NextPageToken))
            {
                returnHistoryList.AddRange(GetHistories(Id, ghistory.NextPageToken));
            }

            return returnHistoryList;
        }

        public override void SendTicketReplyById(SendMailModel mailModel)
        {

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(mailModel.Headers.From);

            foreach (var to in mailModel.ToRecipients)
            {
                mailMessage.To.Add(to);
            }

            mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
            mailMessage.HeadersEncoding = Encoding.GetEncoding("ISO-8859-1");
            mailMessage.Headers.Add("Content-Type", "content=text/html; charset=\"UTF-8\"");

            if (!string.IsNullOrEmpty(mailModel.Headers.References))
                mailMessage.Headers.Add("References", mailModel.Headers.References);

            if (!string.IsNullOrEmpty(mailModel.Headers.InReplyTo))
                mailMessage.Headers.Add("In-Reply-To", mailModel.Headers.InReplyTo);

            mailMessage.ReplyToList.Add(mailModel.Headers.From);
            mailMessage.Subject = mailModel.Subject;
            mailMessage.IsBodyHtml = !string.IsNullOrEmpty(mailModel.BodyHtml);
            mailMessage.Body = mailMessage.IsBodyHtml ? mailModel.BodyHtml : mailModel.BodyText;

            //if (mailModel.Attachments != null && mailModel.Attachments.Count() > 0)
            //{
            //    foreach (var attachment in mailModel.Attachments)
            //    {

            //        System.Net.Mail.Attachment atth = new System.Net.Mail.Attachment()
            //        {
            //            Name = attachment.FileName,

            //        };
            //        mailMessage.Attachments.Add(atth);
            //    }
            //}


            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            _logger.LogInformation("mimeMessage to send to user : {mime}", mimeMessage);

            Message message = new Message()
            {
                ThreadId = mailModel.ThreadId,
                Raw = mimeMessage.ToString().Rfc2822Encode(),
                Snippet = mailModel.Summary
            };

            _gmailInteration.SendMail(message, mailModel.UserId);

            //TODO devam edecek.
        }

        public override WatchResponseModel WatchMail(WatchRequestModel requestModel)
        {
            WatchResponseModel responseModel = new WatchResponseModel();

            try
            {
                object response = null;
                if (requestModel.IsWatchRequest)
                {
                    response = _gmailInteration.SubscribeGmail(requestModel.TopicName, requestModel.LabelList);

                    var tempData = response as WatchResponse;

                    responseModel.HistoryId = tempData.HistoryId ?? 0;
                    responseModel.ExpireDate = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(tempData.Expiration ?? 0);
                }
                else
                {
                    response = _gmailInteration.StopSubscribeGmail();
                }

                _logger.LogWarning("watcmailRunned {request} and response {resp}",
                 JsonConvert.SerializeObject(requestModel), JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GmailTicketIntegration {hata}", ex.Message);
            }

            return responseModel;
        }
    }
}