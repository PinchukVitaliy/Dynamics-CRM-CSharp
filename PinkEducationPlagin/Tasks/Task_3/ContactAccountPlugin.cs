using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PinkEducationPlagin.Tasks.Task_3
{
    public class ContactAccountPlugin : Plugin
    {
        public ContactAccountPlugin()
           : base(typeof(ContactAccountPlugin))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Update",
                    "new_pinktest",
                    UpdateCAPlugin));
        }

        protected void UpdateCAPlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            if (!localContext.PluginExecutionContext.PreEntityImages.Contains("PreImage"))
            {
                throw new InvalidPluginExecutionException("Post image 'PostImage' is missing. Check step registration.");
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            Entity pretImage = localContext.PluginExecutionContext.PreEntityImages["PreImage"];

            IOrganizationService service = localContext.OrganizationService;


            if (target.Contains("new_accountid") && target["new_accountid"] != null)
            {
                EntityReference accounttRef = target.GetAttributeValue<EntityReference>("new_accountid");

                string[] srt = { "primarycontactid" };
                Entity newContact = service.Retrieve(accounttRef.LogicalName, accounttRef.Id, new ColumnSet(srt));
                if (newContact.Contains("primarycontactid") && newContact["primarycontactid"] != null)
                {
                    target["new_contactid"] = newContact["primarycontactid"];
                    target["new_text2"] = "success Contact";
                }
                else
                {
                    target["new_contactid"] = null;
                    target["new_text2"] = "No Contact";
                }
            }
            else
            if (target.Contains("new_contactid") && target["new_contactid"] != null)
            {
                EntityReference contactRef = target.GetAttributeValue<EntityReference>("new_contactid");
                string[] srt = { "parentcustomerid" };
                Entity newAccount = service.Retrieve(contactRef.LogicalName, contactRef.Id, new ColumnSet(srt));
                if (newAccount.Contains("parentcustomerid") && newAccount["parentcustomerid"] != null)
                {
                    target["new_accountid"] = newAccount["parentcustomerid"];
                    target["new_text2"] = "success Account";
                }
                else
                {
                    target["new_accountid"] = null;
                    target["new_text2"] = "No Account";
                }
            }
            else
            {
                target["new_text2"] = "";
            }

        }
        //проверка еслть в target поле лукапа, если нет берем с pretImage и возвращаем EntityReference(лукапа)
        public EntityReference OutLookup(Entity target, Entity pretImage, string fieldName)
        {
            EntityReference entity = null;
            if (target.Contains(fieldName))
            {
                entity = target.GetAttributeValue<EntityReference>(fieldName);
            }
            else
            {
                entity = pretImage.GetAttributeValue<EntityReference>(fieldName);
            }
            return entity;
        }
    }  
}

