using System;
using System.Collections.Generic;
using System.Text;

namespace VanhackRecruitAPI.Modeling
{
    public class CandidateRequestEntity
    {
        public List<string> skills { get; set; }
        public string position { get; set; }
        public int experience { get; set; }
        public EnglishProficiency english { get; set; }
    }
  
    public enum EnglishProficiency
    {
        None,
        Native,
        Fluent,
        Advanced,
        Medium

    }
}
