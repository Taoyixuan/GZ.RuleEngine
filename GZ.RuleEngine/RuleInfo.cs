
using System;
using System.Collections.Generic;

namespace GZ.RuleEngine
{

    public enum RuleOperator
    {
        Current,
        All,
        IsFalse,
        Or
    }
    public enum ExpressionType
    {
        Lamdba,
        Action
    }


    public class RuleInfo
    {
        private List<RuleInfo> ruleInfos = new List<RuleInfo>();


        /// <summary>
        /// ID
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>

        public string RuleName { get; set; }

        /// <summary>
        /// 规则类型（0-lamdba 1-action)
        /// </summary>

        public ExpressionType ExpressionType { get; set; } = ExpressionType.Lamdba;

        /// <summary>
        /// 规则表达式
        /// </summary>

        public string Expression { get; set; }

        /// <summary>
        /// 规则运行逻辑 (仅 ruleinfo下的rules不为空时，才可以选择非‘Current’ 类型)
        /// </summary>

        public RuleOperator Operator { get; set; } = RuleOperator.Current;

        /// <summary>
        /// 规则结果描述
        /// </summary>

        public string Message { get; set; }


        public Func<object, bool> RuleAction { get; set; }


        public Action<object, bool> OnCompleted { get; set; }


        public List<RuleInfo> Rules
        {
            get
            {
                return ruleInfos;
            }
            set
            {
                ruleInfos = value;
            }
        }

        public RuleInfo()
        {
            Operator = RuleOperator.Current;
        }

        public RuleInfo(RuleOperator ruleOperator)
        {
            Operator = ruleOperator;
        }

        public RuleInfo(string expression)
        {
            Expression = expression;
        }

        public RuleInfo(RuleOperator ruleOperator, List<RuleInfo> rules)
        {
            Operator = ruleOperator;
            ruleInfos = rules;
        }

        public void AddRule(RuleInfo ruleInfo)
        {
            ruleInfos.Add(ruleInfo);
        }
    }
}