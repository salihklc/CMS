
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Common.AppConstants;

namespace CMS.Common.Models.CommonModels
{
    public class MailTicket
    {
        public string MessageId { get; set; }
        public string ThreadId { get; set; }
        public ulong HistoryId { get; set; }
        public string ShortMessage { get; set; }
        public string MessageBodyText { get; set; }
        public string MessageBodyHtml { get; set; }

        public string Subject { get; set; }
        public string From { get; set; } // mail ve adı
        public string ErrorsTo { get; set; } // sadece maili
        public string Delivered { get; set; } // adı ve maili
        public string DeliveredTo { get; set; } //Mail adresi

        public DateTime SendDate { get; set; }
        public DateTime ReceiveDate { get; set; }

        public string Headers { get; set; }

        public List<Attachment> Attechments { get; set; }
        public TypeOfTicketIntegration IntegrationType { get; set; }
        public string IntegrationName { get; set; }
    }

    public class Attachment
    {
        public string FileName { get; set; }
        public string Data { get; set; } //base64
        public int Size { get; set; }
        public byte[] ByteData { get; set; }
        public string Detail { get; set; }
    }
    public class GmailPushDataModel
    {
        public string Email { get; set; }
        public ulong HistoryId { get; set; }
    }

    public class GmailPushMessage
    {
        public string Data { get; set; } // base64 data
        public string MessageId { get; set; }
        public string Message_id { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime Publish_time { get; set; }
    }
    public class PubSub
    {
        public string Subscription { get; set; }
        public GmailPushMessage Message { get; set; }
        public string id { get; set; }

    }

    public class SendMailModel
    {
        public string ThreadId { get; set; }
        public string UserId { get; set; } = "me";
        public string Summary { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string BodyText { get; set; }

        public List<string> ToRecipients { get; set; }

        public RFC2822Header Headers { get; set; }

        public List<Attachment> Attachments { get; set; }

    }

    public class WatchResponseModel
    {
        public ulong HistoryId { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class WatchRequestModel
    {
        public bool IsWatchRequest { get; set; }
        public string TopicName { get; set; }
        public List<String> LabelList { get; set; }
    }

    public class RFC2822Header
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public string ErrorsTo { get; set; }
        public string To { get; set; }
        public string DeliveredTo { get; set; }
        public string Date { get; set; }

        public string References { get; set; }
        public string InReplyTo { get; set; }

        public Dictionary<string, string> ToDictionary()
        {

            Dictionary<string, string> tmp = new Dictionary<string, string>()
            {
                {"Subject",   this.Subject },
                {"From",   this.From },
                {"Errors-To",   this.ErrorsTo },
                {"To",   this.To },
                {"Delivered-To",   this.DeliveredTo },
                {"Date",   this.Date },
                {"References",   this.References },
                {"In-Reply-To",   this.InReplyTo }
            };

            return tmp;
        }

    }

    public static class Rfc2822Base64Extension
    {
        public static string Rfc2822Encode(this string mesageToEncode)
        {
            var temp64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(mesageToEncode));

            temp64String = temp64String.Replace('+', '-').Replace('/', '_').Replace("=", "");;

            return temp64String;
        }

        public static string RFC2822Decode(this string messageToDecode)
        {
            var formattedDataNormalBase64 = messageToDecode.Replace('-', '+').Replace('_', '/');

            var tempByteString = Convert.FromBase64String(formattedDataNormalBase64);

            return Encoding.UTF8.GetString(tempByteString);
        }

        public static string ToBase64ToRfc2822Base64(this string base64)
        {
            return base64.Replace('+', '-').Replace('_', '/');
        }
    }

    public class HeaderKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}