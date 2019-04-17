using System;
using Microsoft.Xrm.Sdk;

namespace PinkEducationPlagin.Tasks.Task_1
{
    public class CreatePlaginInSandbox : Plugin
    {
        public CreatePlaginInSandbox()
            : base(typeof(CreatePlaginInSandbox))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Create",
                    "new_pinktest",
                    CreatePlugin));
        }

        protected void CreatePlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            string name = target.GetAttributeValue<string>("new_name");
            Random rand = new Random();
            target["new_name"] = name + rand.Next(1 , 100000).ToString();
        }
    }
}
