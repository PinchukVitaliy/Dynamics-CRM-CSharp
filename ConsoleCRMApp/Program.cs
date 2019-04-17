using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using PinkEducationPlagin;
using PinkEducationPlagin.Tasks.Task_5;

namespace ConsoleCRMApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            //Entity someEnt = service.Retrieve("new_pinktest", new Guid("B10F2561-CD53-E911-8117-00155D05FA01"), new ColumnSet(true));
            ServiceClass serviceClass = new ServiceClass(service);
            //serviceClass.MainMethod();

            //serviceClass.MainMethod2(); //деактивировация 

            //Обращение к црм с использование контекста.  
            //serviceClass.MainMethod3(new Guid("{B10F2561-CD53-E911-8117-00155D05FA01}"), serviceProxy);
            // N-N example
            //serviceClass.MainMethodRelationShip(serviceProxy);

            string var = null;
        }
    }
}
