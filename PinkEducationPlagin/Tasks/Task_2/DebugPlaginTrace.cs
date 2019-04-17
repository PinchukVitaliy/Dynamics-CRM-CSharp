using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PinkEducationPlagin.Tasks.Task_2
{
    public class DebugPlaginTrace : Plugin
    {
        public DebugPlaginTrace()
          : base(typeof(DebugPlaginTrace))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Update",
                    "new_pinktest",
                    TraceThePlugin));
        }

        protected void TraceThePlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            //Settings > Administration > System Settings. На вкладке «Customization»
            //найдите раскрывающееся меню «Enable logging to plug-in trace log» и выберите один из доступных параметров.
            localContext.Trace("Start Start");
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            localContext.Trace(target.LogicalName);
            bool boolField = target.GetAttributeValue<bool>("new_floatfield");
            if (boolField == true)
            {
                target["new_floatfield"] = "Test";
            }
            else
            {
                localContext.Trace("EXEPTION Pink");
                throw new InvalidPluginExecutionException("тест");
            }
            localContext.Trace("End");
        }
    }
}
