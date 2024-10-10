


using System.Collections.Generic;

namespace GZ.RuleEngine
{


    public class RuleWorkflow
    {
        /// <summary>
        /// ID
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// 策略名称
        /// </summary>

        public string WorkflowName { get; set; }

        /// <summary>
        /// 规则运行逻辑
        /// </summary> 
        public RuleOperator Operator { get; set; } = RuleOperator.All;

        public List<RuleInfo> Rules { get; set; }

    }
}