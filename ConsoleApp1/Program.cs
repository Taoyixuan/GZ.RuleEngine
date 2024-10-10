using GZ.RuleEngine;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Testrulemodel2 testrulemodel = new Testrulemodel2();
            RuleManage ruleManage = new RuleManage();
            List<RuleInfo> rules = new List<RuleInfo>();
            rules.Add(new RuleInfo()
            {
                Id = "11",
                ExpressionType = ExpressionType.Lamdba,
                Message = "haha",
                RuleName = "aaa",
                Expression = "name!=\"aa\"",
                Operator = RuleOperator.Current,


            });
            rules.Add(new RuleInfo()
            {
                Id = "12",
                ExpressionType = ExpressionType.Lamdba,
                Message = "aaaa",
                RuleName = "bbb",
                Expression = "2==2",
                Operator = RuleOperator.Current

            });
            RuleWorkflow ruleWorkflow = new RuleWorkflow()
            {
                Id = "1",
                WorkflowName = "1",
                Rules = rules
            };
            ruleManage.AddWorkflow(ruleWorkflow);
            testrulemodel.name = "aa";
            List<Testrulemodel2> testrulemodel2s = new List<Testrulemodel2>();
            testrulemodel2s.Add(testrulemodel);
            var aa = ruleManage.ExecuteRule("1", testrulemodel);
            var checkresult = "成功";
            if (!aa.IsSuccess)
                checkresult = "失败"; 
        }
    } 
}
