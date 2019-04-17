using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PinkEducationPlagin.Tasks.Task_1
{
    public class UpdateClientPlaginInSandbox : Plugin
    {
        public UpdateClientPlaginInSandbox()
            : base(typeof(UpdateClientPlaginInSandbox))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Update",
                    "new_pinktest",
                    UpdatePlugin));
        }

        protected void UpdatePlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            IOrganizationService service = localContext.OrganizationService;
            EntityReference contactRef = target.GetAttributeValue<EntityReference>("new_contactid");

            if (target.Contains("new_contactid") && contactRef != null)
            {
                string[] srt = { "emailaddress1" };
                Entity email = service.Retrieve(contactRef.LogicalName, contactRef.Id, new ColumnSet(srt));
                Entity someNewEnt = new Entity(target.LogicalName, target.Id);

                if (email.Contains("emailaddress1") && email["emailaddress1"] != null)
                {
                    someNewEnt["new_text2"] = "Customer meets the requirements";
                    service.Update(someNewEnt);
                }
                else
                {
                    //someNewEnt["new_text2"] = "Email address is not filled !!!";
                    //service.Update(someNewEnt);
                    throw new Exception("Email address is not filled !!!");
                }
            }
        }
    
    }
}
