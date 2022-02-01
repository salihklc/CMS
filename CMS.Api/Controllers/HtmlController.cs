using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HtmlController : ControllerBase
    {

        // GET api/values/5
        [Route("google5db7cbf518a2e064.html", Order = 0)]
        public ActionResult<string> GetDomainVerification()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "google-site-verification: google5db7cbf518a2e064.html"
            };
        }

    }
}