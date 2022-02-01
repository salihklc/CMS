using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CMS.Api.StartupHelpers;

namespace CMS.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProgramRunnerExtension.BuilWebHost(args);
        }
    }
}
