
namespace GZ.RuleEngine
{
    public static class RuleResultExtention
    {
        /// <summary>
        /// 策略不存在
        /// </summary>
        /// <param name="ruleResult"></param>
        /// <returns></returns>
        public static RuleResult RuleWorkflowNotFound(this RuleResult ruleResult)
        {
            ruleResult = new RuleResult { IsSuccess = false, Message = "没有找到对应策略" };
            return ruleResult;
        }
        /// <summary>
        /// 策略下没有找到规则
        /// </summary>
        /// <param name="ruleResult"></param>
        /// <returns></returns>
        public static RuleResult RuleInWorkflowNotFound(this RuleResult ruleResult)
        {
            ruleResult = new RuleResult { IsSuccess = false, Message = "策略下没有找到规则" };
            return ruleResult;
        }
        /// <summary>
        /// 规则下没有找到规则
        /// </summary>
        /// <param name="ruleResult"></param>
        /// <returns></returns>
        public static RuleResult RuleinRuleNotFound(this RuleResult ruleResult)
        {
            ruleResult = new RuleResult { IsSuccess = false, Message = "规则下没有找到规则" };
            return ruleResult;
        }
        /// <summary>
        /// 规则表达式有误
        /// </summary>
        /// <param name="ruleResult"></param>
        /// <returns></returns>
        public static RuleResult RuleExpressError(this RuleResult ruleResult)
        {
            ruleResult = new RuleResult { IsSuccess = false, Message = "规则表达式有误" };
            return ruleResult;
        }
    }
}
