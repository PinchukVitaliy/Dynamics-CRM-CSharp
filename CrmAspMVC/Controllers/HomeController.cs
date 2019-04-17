using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Activities.Presentation.Annotations;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CrmAspMVC.Controllers
{
    public class HomeController : Controller
    {
        private static OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
        private static IOrganizationService service = (OrganizationServiceProxy)serviceProxy;

        static string str ="";

        //http://localhost:50055/Home/Index?invoceId={6301F8F2-2843-E911-8115-00155D05FA01}
        public ActionResult Index(Guid? invoceId) // {6301F8F2-2843-E911-8115-00155D05FA01} oil entity
        {
            if (invoceId.HasValue)
            {                        
                try
                {
                    Entity invoce = service.Retrieve("invoice", invoceId.Value, new ColumnSet(true));
                    ViewBag.Name = "Name Entity Invoice: "+ invoce.GetAttributeValue<string>("name");
                    Money moneyInvoice = invoce.GetAttributeValue<Money>("totalamount");
                    ViewBag.Total ="Total Amount: " + "$"+moneyInvoice.Value;
                    
                }
                catch (Exception ex)
                {
                    ViewBag.Messge = "There is no Invoce with ID: " + invoceId;
                }
            }
            else
            {
                ViewBag.Messge = "Not defined guid entities Invoce";
            }

            return View();
        }


        public string Result()
        {
            return "Hello";
        }


        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Records = GetAllRecords("new_name");
            ViewBag.CheckBoxList = str;
            return View();
        }

        [HttpPost]
        public ActionResult About(FormCollection collection, IEnumerable<HttpPostedFileBase> uploads)
        {
            string checkResp = "No selected CheckBoxes";
            if (!string.IsNullOrEmpty(collection["guid"]))
            {             
                foreach (var elem in collection["guid"].Split(','))
                {
                    //str += elem; 
                    Attach(uploads, elem);
                }
                str = "files attached successfully";
            }
            else
            {
                str = checkResp;
            }
            return RedirectToAction("/About");
        }

        public ActionResult Contact(FormCollection collection)
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public Dictionary<Guid, string> GetAllRecords(string fieldName)
        {
            // так выбирать все записи нельзя! для реальных задач берем записи постранично
            var query = new QueryExpression("new_pinktest")
            {
                ColumnSet = new ColumnSet(fieldName),
            };
            var res = service.RetrieveMultiple(query).Entities;
            return res.ToDictionary(t => t.Id, t => t.GetAttributeValue<string>(fieldName));
        }

        public void Attach(IEnumerable<HttpPostedFileBase> uploads, string guid)
        {
            string filename = "";
            foreach (var file in uploads)
            {
                if (file == null)
                    continue;
                filename = Path.GetFileName(file.FileName);   
                byte[] filebytes = new byte[file.ContentLength];
                AttachFile(guid, filebytes, filename);
            }
        }

        public void AttachFile(string guid, byte[] filebytes, string fileName)
        {
            //string strMessage =”this is a demo”;
            //byte[] filename = Encoding.ASCII.GetBytes(strMessage);
            string encodedData = System.Convert.ToBase64String(filebytes);
            Entity Annotation = new Entity("annotation");
            Annotation.Attributes["objectid"] = new EntityReference("new_pinktest", new Guid(guid));
            //Annotation.Attributes["objecttypecode"] = “EntityNAME”;
            Annotation.Attributes["subject"] = "Demo Pink";
            Annotation.Attributes["documentbody"] = encodedData;
            Annotation.Attributes["mimetype"] = @"text/plain";
            Annotation.Attributes["notetext"] = "Sample attachment Pink File";
            Annotation.Attributes["filename"] = fileName;
            service.Create(Annotation);
        }


    }
}