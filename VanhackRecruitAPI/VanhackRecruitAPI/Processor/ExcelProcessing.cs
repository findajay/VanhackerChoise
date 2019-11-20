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
        
        /// <summary>
        /// Get Excel from blob storage and process
        /// </summary>
        /// <param name="CandidateProfile"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static SearchResponse GetExcelFile(CandidateRequestEntity CandidateProfile, ILogger log)
        {
            List<string> CandRes = new List<string>();
            string[] requestedSkills = new string[3];
            if (CandidateProfile.skills.Contains(" "))
            {
                requestedSkills = CandidateProfile.skills.Split(" ");
            }
            else if (CandidateProfile.skills.Contains(","))
            {
                requestedSkills = CandidateProfile.skills.Split(",");
            }
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
                        if (csv[2].ToLower().Contains(CandidateProfile.position.ToLower()) && int.Parse(csv[3]) == CandidateProfile.experience && int.Parse(csv[4]) == Convert.ToInt32(CandidateProfile.english))
                        {
                            if (CheckForSkills(csv[1], requestedSkills))
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


        /// <summary>
        /// check for all requested skills are matching or not
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="skillsRequested"></param>
        /// <returns></returns>
        private static bool CheckForSkills(string skillRow, string[] skillsRequested)
        {
            foreach (string s in skillsRequested)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (!skillRow.ToLower().Contains(Convert.ToString(s).Trim().ToLower()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


    }
}
