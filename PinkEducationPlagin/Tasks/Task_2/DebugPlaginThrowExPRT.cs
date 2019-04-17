using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PinkEducationPlagin.Tasks.Task_2
{
    public class DebugPlaginThrowExPRT : Plugin
    {
        public DebugPlaginThrowExPRT()
           : base(typeof(DebugPlaginThrowExPRT))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Update",
                    "new_pinktest",
                    ThrowThePlugin));
        }

        protected void ThrowThePlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];

            bool boolField = target.GetAttributeValue<bool>("new_floatfield");
            if(boolField == true)
            {
                target["new_floatfield"] = "Test";
            }
            else
            {
                throw new InvalidPluginExecutionException("тест");
            }
            
        }
    }
}
