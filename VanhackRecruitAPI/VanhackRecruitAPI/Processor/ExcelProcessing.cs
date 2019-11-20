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
        private static string AvailableCandidateCsvPath = Environment.GetEnvironmentVariable("AvailableCandidatesBlob");
        private static string AvailableJobsCsvPath = Environment.GetEnvironmentVariable("AvailableJobsBlob");

        /// <summary>
        /// Get Candidate Excel from blob storage and process
        /// </summary>
        /// <param name="CandidateProfile"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static SearchResponse ProcessCandidatesExcelFile(CandidateRequestEntity CandidateProfile, ILogger log)
        {
            List<string> CandRes = new List<string>();
            string[] requestedSkills = new string[3];
            requestedSkills = GetRequestedSkills(requestedSkills, CandidateProfile.skills);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(AvailableCandidateCsvPath);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            try
            {
                using CsvReader csv =
                new CsvReader(new StreamReader(resp.GetResponseStream()), true);
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    if ((string.IsNullOrEmpty(CandidateProfile.position) || csv[2].ToLower().Contains(CandidateProfile.position.ToLower())) && (CandidateProfile.experience == 0 || int.Parse(csv[3]) == CandidateProfile.experience) && (CandidateProfile.english == 0 || int.Parse(csv[4]) == Convert.ToInt32(CandidateProfile.english)))
                    {
                        if (requestedSkills == null || (requestedSkills != null && CheckForSkills(csv[1], requestedSkills)))
                        {
                            CandRes.Add(csv[0]);
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
        /// Get jobs Excel from blob storage and process
        /// </summary>
        /// <param name="CandidateProfile"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static SearchResponse ProcessJobsExcelFile(CandidateRequestEntity CandidateProfile, ILogger log)
        {
            List<string> JobsRes = new List<string>();
            string[] requestedSkills = new string[3];

            requestedSkills = GetRequestedSkills(requestedSkills, CandidateProfile.skills);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(AvailableJobsCsvPath);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            try
            {
                using CsvReader csv =
                new CsvReader(new StreamReader(resp.GetResponseStream()), true);
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    if ((string.IsNullOrEmpty(CandidateProfile.position) || csv[2].ToLower().Contains(CandidateProfile.position.ToLower())) || (requestedSkills != null && CheckForSkills(csv[3], requestedSkills)))
                    {
                        JobsRes.Add(csv[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
                return new SearchResponse();
            }
            return new SearchResponse { BestMatched = JobsRes };
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

        /// <summary>
        /// To get request skills array from alexa
        /// </summary>
        /// <param name="requestedSkills"></param>
        /// <param name="availableSkills"></param>
        /// <returns></returns>
        private static string[] GetRequestedSkills(string[] requestedSkills, string availableSkills)
        {
            if (!string.IsNullOrEmpty(availableSkills))
            {
                if (availableSkills.Contains(" "))
                {
                    requestedSkills = availableSkills.Split(" ");
                }
                else if (availableSkills.Contains(","))
                {
                    requestedSkills = availableSkills.Split(",");
                }
                return requestedSkills;
            }
            return null;
        }


    }
}
