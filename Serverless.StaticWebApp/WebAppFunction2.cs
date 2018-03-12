using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Serverless.StaticWebApp
{
    public static class WebAppFunction2
    {
        [FunctionName("WebAppFunction2")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "WebApp/{page}")]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                var filePath = BaseFunction.GetFilePath(req);
                var fileContent = File.ReadAllText(filePath);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(fileContent, Encoding.UTF8, BaseFunction.GetMediaType(filePath))
                };
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //Should return error page 500.html
                return null;
            }
        }
    }
}