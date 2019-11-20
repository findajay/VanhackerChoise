using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VanhackRecruitAPI.Processor;
using VanhackRecruitAPI.Modeling;

namespace VanhackRecruitAPI.FunctionEndpoints
{
    public static class GetFilteredJobs
    {
        [FunctionName("GetFilteredJobs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetFilteredJobs")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var  requestEntity = JsonConvert.DeserializeObject<CandidateRequestEntity>(requestBody);

            if (requestEntity == null)
            {
                return new BadRequestObjectResult("Please pass expected input in the request body");
            }
                var response = ExcelProcessing.ProcessJobsExcelFile(requestEntity,log);

                return response != null
                    ? (ActionResult)new OkObjectResult(response)
                    : new BadRequestObjectResult("Please pass expected input in the request body");
            }
        }
    }
