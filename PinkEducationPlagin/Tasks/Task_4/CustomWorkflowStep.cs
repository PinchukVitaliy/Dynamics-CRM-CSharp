using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace PinkEducationPlagin.Tasks.Task_4
{
    public class CustomWorkflowStep : CodeActivity
    {
        #region Workflow parameters

        [RequiredArgument]
        [Input("GuidLookup")]
        [ReferenceTarget("account")]
        public InArgument<EntityReference> IdParam { get; set; }

        [RequiredArgument]
        [Output("OuterString")]
        public OutArgument<string> OutParam { get; set; }

        //[RequiredArgument]
        //[Input("IdToAdd")]
        //public InArgument<string> IdParam { get; set; }

        //[RequiredArgument]
        //[Input("IntToAdd")]
        //public InArgument<int> IntParam { get; set; }

        #endregion
    protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (IdParam.Get<EntityReference>(executionContext) != null)
            {
                EntityReference refAcc = IdParam.Get<EntityReference>(executionContext);
                string[] srt = { "emailaddress1" };
                Entity email = service.Retrieve(refAcc.LogicalName, refAcc.Id, new ColumnSet(srt));
                if(email.GetAttributeValue<string>("emailaddress1") != null)
                {
                    string name = email.GetAttributeValue<string>("emailaddress1");
                    OutParam.Set(executionContext, name);
                }
                else
                {
                    OutParam.Set(executionContext, "Account ->("+ refAcc.Name+")<- email is missing");
                }

            }
            else
            {
                OutParam.Set(executionContext, "There is no account in this entity");
            }
        }
    }
}
