
using System.Collections.Generic;

namespace CMS.Common.Models.CommonModels
{

    public class GmailCredential
    {
        public string CredentialPath { get; set; }
        public string ApplicationName { get; set; }
        public string CredentialApplicationEmail { get; set; }
        public string CredentialUser { get; set; }
    }

    public class OutlookCredential
    {

    }

    public class IntegratorSettings
    {
        public GmailCredential GmailCredential { get; set; }
        public OutlookCredential OutlookCredential { get; set; }
    }

    public class FoldersSettings
    {
        public string Uploads { get; set; }
        public string Example { get; set; }
        public string Attachments { get; set; }
        
    }

}