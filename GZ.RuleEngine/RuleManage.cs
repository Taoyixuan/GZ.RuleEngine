 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using GZ.RuleEngine;
using RunBowLibaray.Util.Extention;
using ExpressionType = GZ.RuleEngine.ExpressionType;

namespace GZ.RuleEngine
{

    public class RuleManage
    {
        private List<RuleWorkflow> ruleWorkflows = new List<RuleWorkflow>();

        public void AddWorkflow(RuleWorkflow ruleWorkflow)
        {
            ruleWorkflows.Add(ruleWorkflow);
        }

        public void RemoveWorkflow(string ruleWorkflowId)
        {
            ruleWorkflows.RemoveAll((RuleWorkflow m) => m.Id == ruleWorkflowId);
        }

        public RuleResult ExecuteRule(string ruleWorkflowId, object obj)
        {
            RuleResult ruleResult = new RuleResult();
            List<RuleResult> resultdetails = new List<RuleResult>();
            if (ruleWorkflows.Count <= 0 || !ruleWorkflows.Exists((RuleWorkflow m) => m.Id == ruleWorkflowId))
            {
                return ruleResult.RuleWorkflowNotFound();
            }
            RuleWorkflow workflow = ruleWorkflows.First((RuleWorkflow m) => m.Id == ruleWorkflowId);
            if (workflow.Rules == null || workflow.Rules.Count == 0)
            {
                return ruleResult.RuleInWorkflowNotFound();
            }
            bool IsStop = false;
            workflow.Rules.ForEach(delegate (RuleInfo m)
            {
                if (!IsStop)
                {
                    RuleResult ruleResult2 = RunRuleGroup(obj, m);
                    resultdetails.Add(ruleResult2);
                    switch (workflow.Operator)
                    {
                        case RuleOperator.Or:
                            if (ruleResult2.IsSuccess)
                            {
                                IsStop = true;
                            }

                            break;
                        case RuleOperator.IsFalse:
                            if (!ruleResult2.IsSuccess)
                            {
                                IsStop = true;
                            }

                            break;
                    }
                }
            });
            ruleResult = ((!resultdetails.Exists((RuleResult m) => !m.IsSuccess)) ? resultdetails.Last().DeepClone() : resultdetails.First((RuleResult n) => !n.IsSuccess).DeepClone());
            ruleResult.ResultDetails = resultdetails;
            return ruleResult;
        }

        public void LoadFormJson(string DirectoryPath)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                new Exception("没有找到文件夹");
            }

            FileInfo[] files = Directory.CreateDirectory(DirectoryPath).GetFiles();
            FileInfo[] array = files;
            foreach (FileInfo fileInfo in array)
            {
                if (fileInfo.Extension.ToLower() == ".json")
                {
                    string jsonStr = string.Empty;
                    using (StreamReader streamReader = File.OpenText(fileInfo.FullName))
                    {
                        jsonStr = streamReader.ReadToEnd();
                    }

                    RuleWorkflow ruleWorkflow = (RuleWorkflow)jsonStr.ToObject(typeof(RuleWorkflow));
                    AddWorkflow(ruleWorkflow);
                }
            }
        }

        private RuleResult RunRuleGroup(object obj, RuleInfo rule)
        {
            RuleResult ruleResult = new RuleResult();
            List<RuleResult> list = new List<RuleResult>();
            if (rule.Operator == RuleOperator.Current)
            {
                list.Add(RunRule(obj, rule));
            }
            else
            {
                if (rule.Rules == null || rule.Rules.Count == 0)
                { return ruleResult.RuleinRuleNotFound(); }
                list.AddRange(RunRules(obj, rule.Rules, rule.Operator));
            }

            ruleResult = ((!list.Exists((RuleResult m) => !m.IsSuccess)) ? list.Last().DeepClone() : list.First((RuleResult n) => !n.IsSuccess).DeepClone());
            ruleResult.ResultDetails = list;
            return ruleResult;
        }

        private RuleResult RunRule(object obj, RuleInfo rule)
        {
            RuleResult ruleResult = new RuleResult();
            bool flag = false;
            flag = ((rule.ExpressionType != ExpressionType.Action) ? ExecLamdba(obj, rule.Expression) : RunAction(rule.RuleAction, obj));
            ruleResult.IsSuccess = flag;
            ruleResult.RuleId = rule.Id;
            ruleResult.RuleName = rule.RuleName;
            ruleResult.Expression = rule.Expression;
            if (!ruleResult.IsSuccess)
            {
                ruleResult.Message = rule.Message;
            }

            return ruleResult;
        }

        private bool RunAction(Func<object, bool> func, object obj)
        {
            return func?.Invoke(obj) ?? false;
        }

        private bool ExecLamdba(object obj, string expression)
        {
            Type type = obj.GetType();
            ParameterExpression parameterExpression = Expression.Parameter(type, "data");
            LambdaExpression lambdaExpression = DynamicExpressionParser.ParseLambda(new ParameterExpression[1] { parameterExpression }, typeof(bool), expression);
            object obj2 = lambdaExpression.Compile().DynamicInvoke(obj);
            return (bool)obj2;
        }

        private List<RuleResult> RunRules(object obj, List<RuleInfo> rules, RuleOperator ruleOperator)
        {
            List<RuleResult> resultDetails = new List<RuleResult>();
            bool IsStop = false;
            rules.ForEach(delegate (RuleInfo m)
            {
                if (!IsStop)
                {
                    RuleResult ruleResult = RunRule(obj, m);
                    resultDetails.Add(ruleResult);
                    switch (ruleOperator)
                    {
                        case RuleOperator.Or:
                            if (ruleResult.IsSuccess)
                            {
                                IsStop = true;
                            }
                            break;
                        case RuleOperator.IsFalse:
                            if (!ruleResult.IsSuccess)
                            {
                                IsStop = true;
                            }
                            break;
                    }
                }
            });
            return resultDetails;
        }
    }
}