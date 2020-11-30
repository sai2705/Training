using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace AUthenticationRequest
{
    public static class Authentication
    {
        [FunctionName("Authentication")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (req.Method == HttpMethods.Post)
            {
                log.LogInformation($"The method requested is success");

            }
            else if (req.Method == HttpMethods.Delete)
            {
                log.LogInformation($"The method requested is success");

            }
            else
            {
                log.LogInformation($"({req.Method}) is not valid");
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
