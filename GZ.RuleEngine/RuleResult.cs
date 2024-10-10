using System.Collections.Generic;

namespace GZ.RuleEngine
{


    public class RuleResult
    {
        public bool IsSuccess { get; set; }
        public string RuleId { get; set; }
        public string RuleName { get; set; }
        public string Expression { get; set; }
        public string Message { get; set; }

        public List<RuleResult> ResultDetails { get; set; }

    }
}