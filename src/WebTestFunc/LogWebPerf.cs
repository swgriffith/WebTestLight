using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Configuration;

namespace WebTestFunc
{
    public static class LogWebPerf
    {
        //private static string eventHubName = ConfigurationManager.AppSettings["eventHubName"]; //"webtestlighthub";
        private static string connectionString = ConfigurationManager.AppSettings["connectionString"]; //"Endpoint=sb://webtestlight.servicebus.windows.net/;SharedAccessKeyName=sender;SharedAccessKey=qjB2Lwli+RMOSszvNie9ntBUWTNlTcYwRNnZqmeD7t0=;EntityPath=webtestlighthub";

        [FunctionName("LogWebPerf")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                // parse query parameters
                data respData = new data();
                respData.timestamp = DateTime.Now;
                respData.region = req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => string.Compare(q.Key, "region", true) == 0)
                    .Value;
                respData.resptime = Convert.ToDecimal(req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => string.Compare(q.Key, "resptime", true) == 0)
                    .Value);

                string output = JsonConvert.SerializeObject(respData);
                log.Info(output);
                var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);

                eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(output)));

                return req.CreateResponse(HttpStatusCode.OK, output);
            }
            catch (System.Exception ex)
            {
                log.Info(ex.InnerException.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.ToString());
            }

        }
    }


    class data
    {
        public DateTime timestamp { get; set; }
        public string region { get; set; }
        public decimal resptime { get; set; }
    }
}
