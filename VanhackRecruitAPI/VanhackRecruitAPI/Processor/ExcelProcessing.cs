using LumenWorks.Framework.IO.Csv;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using VanhackRecruitAPI.Modeling;

namespace VanhackRecruitAPI.Processor
{
    public static class ExcelProcessing
    {
        private static string AvailableCandidateCsvPath = Environment.GetEnvironmentVariable("AvailableCandidatesBlob");// @"";
        public static SearchResponse getExcelFile(CandidateRequestEntity CandidateProfile, ILogger log)
        {
            List<string> CandRes = new List<string>();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(AvailableCandidateCsvPath);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            try
            {
                using (CsvReader csv =
                new CsvReader(new StreamReader(resp.GetResponseStream()), true))
                {
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        if (csv[1].Contains(CandidateProfile.skills[0]) && csv[1].Contains(CandidateProfile.skills?[1]) && csv[1].Contains(CandidateProfile.skills?[2]))
                        {
                            if (csv[2].Contains(CandidateProfile.position) && int.Parse(csv[3]) == CandidateProfile.experience && int.Parse(csv[4]) == Convert.ToInt32(CandidateProfile.english))
                            {
                                CandRes.Add(csv[0]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
                return new SearchResponse();
            }
            return new SearchResponse { BestMatched = CandRes };
        }
    }
}
