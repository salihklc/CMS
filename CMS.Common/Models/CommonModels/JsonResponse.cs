using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Models.CommonModels
{
    public class JsonResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ReturnUrl { get; set; }
        public string ExtraData { get; set; }
    }
}
