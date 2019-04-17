using System;
using System.Collections;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using PinkEducationPlagin.Repository;
using PinkEducationPlagin.Tasks.Task_5;
using PinkEducationPlagin.Tasks.Task_5.CrmContext;

namespace PinkEducationPlagin
{
    public class ServiceClass
    {
        private IOrganizationService _service;
        private const string Entity1 = "new_pinktest";
        private const string Entity2 = "new_pinktest2";
        private const string relationshipEntityName = "new_new_pinktest_new_pinktest2";
        public ServiceClass(IOrganizationService service)
        {
            _service = service;
        }

        public void MainMethod()
        {
            ContactRepository contactRepository = new ContactRepository(_service);
          
            EntityCollection contacts = contactRepository.GetContactsWithPhone(new Guid("{5E23A40E-2043-E911-8115-00155D05FA01}"));          
            if (contacts != null)
            {
                Console.WriteLine(contacts.Entities.Count);
            }
            else
            {
                Console.WriteLine("No contacts");
            }
            Console.ReadLine();
        }
        public void MainMethod2()
        {
            QueryExpDeactivePinkTestRepository deactiveRepository = new QueryExpDeactivePinkTestRepository(_service);
            //колекция  Entity1
            EntityCollection collection1 = deactiveRepository.DeactiveEntityPinkTest(Entity1, Entity2, relationshipEntityName);
            //колекция  Entity2
            EntityCollection collection2 = deactiveRepository.DeactiveEntityPinkTest(Entity2, Entity1, relationshipEntityName);

            if (collection1.Entities.Count > 0 && collection2.Entities.Count > 0)
            {
                Console.WriteLine(collection1.Entities.Count);
                foreach (Entity entity1 in collection1.Entities)
                {               
                    foreach (var entity2 in collection2.Entities)
                    {
                        if (entity1.GetAttributeValue<EntityReference>("new_lookuppinktest2") != null &&
                             entity1.GetAttributeValue<EntityReference>("new_accountid") != null &&
                             entity2.GetAttributeValue<EntityReference>("new_accountid") != null &&
                             entity2.GetAttributeValue<EntityReference>("new_pinktestentityid") != null)
                        {
                            if (entity1.GetAttributeValue<EntityReference>("new_lookuppinktest2").Id == entity2.Id &&
                                entity1.GetAttributeValue<EntityReference>("new_accountid").Id == 
                                    entity2.GetAttributeValue<EntityReference>("new_accountid").Id &&
                                    entity1.Id == entity2.GetAttributeValue<EntityReference>("new_pinktestentityid").Id)
                            {
                                Console.WriteLine(entity1.Attributes["new_name"]+" Inactived");
                                deactiveRepository.changeEntityStatus(entity1.Id, entity1.LogicalName);
                            }
                        }                     
                    }
                }
            }
            else
            {
                Console.WriteLine("No Entity");
            }
               Console.ReadLine();
        }

        public void MainMethod3(Guid piMainId, OrganizationServiceProxy _serviceProxy)
        {
            _serviceProxy.EnableProxyTypes();
            using (CrmServiceContext _context = new CrmServiceContext(_serviceProxy))
            {             
                var query_where = (from d in _context.new_PinkTestSet
                          where d.Id == piMainId
                          select d);


            foreach (var a in query_where)
            {
                Console.WriteLine(a.new_name);
            }
                 Console.ReadLine();
            }
        }

        public void MainMethodRelationShip(OrganizationServiceProxy _serviceProxy)
        {
            _serviceProxy.EnableProxyTypes();
            using (CrmServiceContext _context = new CrmServiceContext(_serviceProxy))
            {
                var query_where = (from p1 in _context.new_PinkTestSet
                                   join relP1 in _context.new_new_pinktest_new_pinktest2Set on p1.Id equals relP1.new_pinktestid
                                   join p2 in _context.new_pinktest2Set on relP1.new_pinktest2id equals p2.Id
                                   where !p1.new_LookupPinkTest2.Equals(null) || !p2.new_accountid.Equals(null) //&&
                                  // p1.new_LookupPinkTest2.Id == p2.Id && 
                                   //p1.new_accountid.Id == p2.new_accountid.Id &&
                                  // p1.Id == p2.new_pinktestentityid.Id
                                   select p1).ToList();
                //select new
                //{
                //    contact_name = c.FullName,
                //    account_name = a.Name
                //};

                foreach (var c in query_where)
                {
                    Console.WriteLine(c.new_name);
                }
                Console.ReadLine();
            }
        }
    }
}