using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using CMS.TaskManager.Business.Logic.Models;

namespace CMS.TaskManager.Business.Logic
{
    public class RestCaller
    {

        public RestCaller(ILogger<RestCaller> logger)
        {
        }

        public static RestCallResponse SendRequest(string Method, string Url, string RequiredParams, string Params)
        {
            var result = new RestCallResponse();
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                try
                {
                    
                    switch (Method.ToLower())
                    {
                        case "post":
                            byte[] postData = Encoding.UTF8.GetBytes(Params);
                            response = client.UploadData(Url,postData);
                            break;
                        case "get":
                            var jObj = (JObject)JsonConvert.DeserializeObject(Params);

                            var query = String.Join("&",
                                            jObj.Children().Cast<JProperty>()
                                            .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));

                            response = client.DownloadData($"{Url}?{query}");
                            break;
                    }
                }
                catch (WebException ex)
                {
                    result.HttpStatus = ex.Status.ToString();
                    Log.Error(ex, ex.Message);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }

                if (response != null)
                {
                    result.ResultData = Encoding.UTF8.GetString(response);
                    result.HttpStatus = "200";
                }
            }

            return result;
        }
    }
}
