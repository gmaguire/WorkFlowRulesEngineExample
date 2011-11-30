using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Workflow.Activities.Rules;
using System.Workflow.Activities.Rules.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;

namespace WorkflowRulesEngine
{
    public class RulesEngine<T>
    {
        private readonly string _rulesFilePath;

        public RulesEngine()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filename = typeof(T).Name + ".rules";
            _rulesFilePath = Path.Combine(path ?? string.Empty, filename);
        }

        public RuleSet LaunchRulesDialog(RuleSet ruleSet)
        {
            using (var ruleSetDialog = new RuleSetDialog(typeof(T), null, ruleSet))
            {
                if (ruleSetDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveRuleSetToFile(ruleSetDialog.RuleSet);
                }

                return ruleSetDialog.RuleSet;
            }
        }

        public T ProcessRuleSet(T objectToProcess, RuleSet ruleSet)
        {
            var validation = new RuleValidation(typeof(T), null);
            var execution = new RuleExecution(validation, objectToProcess);

            ruleSet.Execute(execution);

            return objectToProcess;
        }

        public void SaveRuleSetToFile(RuleSet ruleSet)
        {
            var serializer = new WorkflowMarkupSerializer();

            if (File.Exists(_rulesFilePath))
            {
                File.Delete(_rulesFilePath);
            }

            using (var rulesWriter = XmlWriter.Create(_rulesFilePath))
            {
                serializer.Serialize(rulesWriter, ruleSet);
            }
        }

        public RuleSet LoadRuleSetFromFile()
        {
            RuleSet ruleSet;

            if (!File.Exists(_rulesFilePath))
            {
                return new RuleSet();
            }

            using (var rulesReader = new XmlTextReader(_rulesFilePath))
            {
                var serializer = new WorkflowMarkupSerializer();
                ruleSet = (RuleSet)serializer.Deserialize(rulesReader);
            }

            return ruleSet;
        }
    }
}