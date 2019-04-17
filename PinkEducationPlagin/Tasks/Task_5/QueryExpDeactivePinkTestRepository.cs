using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PinkEducationPlagin.Repository;

namespace PinkEducationPlagin.Tasks.Task_5
{
    class QueryExpDeactivePinkTestRepository
    {
        private IOrganizationService _service;
        public QueryExpDeactivePinkTestRepository(IOrganizationService service)
        {
            _service = service;
        }

        public EntityCollection DeactiveEntityPinkTest(string Entity1, string Entity2, string relationshipEntityName)
        {
            // N-N example
            //var query = new QueryExpression(Entity1)
            //{
            //    ColumnSet = new ColumnSet(true),
            //    //Criteria = new FilterExpression(LogicalOperator.And) 
            //    //{
            //    //    Conditions =
            //    //    {
            //    //        new ConditionExpression()
            //    //    }
            //    //},
            //    LinkEntities =
            //    {
            //        new LinkEntity(Entity1, relationshipEntityName, Entity1+"id", Entity1+"id", JoinOperator.Inner)
            //            // делаем линк на промежуточную таблицу
            //      {
            //          LinkEntities =
            //          {
            //              new LinkEntity(relationshipEntityName, Entity2, Entity2+"id", Entity2+"id" ,JoinOperator.Inner)
            //              {
            //                      LinkCriteria = new FilterExpression(LogicalOperator.And)
            //                      {
            //                            Conditions =
            //                         {
            //                            new ConditionExpression()
            //                         }
            //                      }
            //              }
            //           }
            //        }
            //    }
            //};
            //return _service.RetrieveMultiple(query);

            QueryExpression query = new QueryExpression(Entity1);

            query.ColumnSet = new ColumnSet(true);
            query.Distinct = true;

            LinkEntity linkEntity1 = new LinkEntity(Entity1, relationshipEntityName, Entity1 + "id", Entity1 + "id", 
                JoinOperator.LeftOuter);
            LinkEntity linkEntity2 = new LinkEntity(relationshipEntityName, Entity2, Entity2 + "id", Entity2 + "id", 
                JoinOperator.LeftOuter);



            linkEntity1.LinkEntities.Add(linkEntity2);

            query.LinkEntities.Add(linkEntity1);

            query.Criteria = new FilterExpression();
            query.Criteria.FilterOperator = LogicalOperator.And;
            FilterExpression filter1 = query.Criteria.AddFilter(LogicalOperator.And);
            filter1.Conditions.Add(new ConditionExpression("createdon", ConditionOperator.LastXMonths, 2));
            //filter1.Conditions.Add(new ConditionExpression("new_accountid", ConditionOperator.NotNull));
            query.Criteria.Filters.Add(filter1);

            return _service.RetrieveMultiple(query);
        }
        public void changeEntityStatus(Guid userId,string entityName)
        {
            SetStateRequest setStateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference(entityName, userId),
                State = new OptionSetValue(1),
                Status = new OptionSetValue(-1),
            };
            _service.Execute(setStateRequest);
        }
    }
}
