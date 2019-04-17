
using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PinkEducationPlagin.Tasks.Task_6
{
    public class ActionPlugin : Plugin
    {
        public ActionPlugin()
            : base(typeof(ActionPlugin))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "new_PinkActionSecond",
                   null,
                    ActionProcesses));
        }

        protected void ActionProcesses(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("InputColectinEntity"))
            {
                throw new InvalidPluginExecutionException("No ID list received");
            }
            

            IOrganizationService service = localContext.OrganizationService;
            string inputEntitys = (string)localContext.PluginExecutionContext.InputParameters["InputColectinEntity"];

            Entity en = new Entity("new_pinktest", new Guid(inputEntitys));
            string[] srt = { "new_lookuppinktest2" };
            Entity refMain = service.Retrieve(en.LogicalName, en.Id, new ColumnSet(srt));
            EntityReference r = (EntityReference)refMain["new_lookuppinktest2"];
            deactiveEntity(r.Id, r.LogicalName, service);
            //en["new_stepprevious"] = r.Id.ToString();//.ToUpper();
            //service.Update(en);

        }
        public void deactiveEntity(Guid userId, string entityName, IOrganizationService service)
        {
            SetStateRequest setStateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference(entityName, userId),
                State = new OptionSetValue(1),
                Status = new OptionSetValue(-1),
            };
            service.Execute(setStateRequest);
        }
    }
}


