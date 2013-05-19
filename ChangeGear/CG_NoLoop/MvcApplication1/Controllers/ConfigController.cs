using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class ConfigController : Controller
    {
        //
        // GET: /Config/

        public string myFile = @"C:\Program Files (x86)\SunView Software\ChangeGear\Server\EmailCnctr\CGEmailCnCtr.exe.config";

        //ActionResult is a variable class that allows for polymorphism (might return one of a variety of types)
        public ActionResult Index()
        {
            //Load the XML document as an object
            XDocument doc = XDocument.Load(myFile);

            //search the key/value pair that matches our parameters, and get the first result
            var messageFilter =
              (from node in doc.Descendants("add") //descendants are the node names
               where node.Attribute("key").Value.Equals("messageFilter") //attributes are the attributes... how about that?
               select node.Attribute("value").Value).FirstOrDefault();

            //because the filters are seprated in the key's value by semicolons, split the result at each semicolon
            string[] currentFilters = messageFilter.Split(';');

            //return the result as a JSON object
            //uses "AllowGet" because by default, the MVC framework does not allow you to respond to an HTTP GET request with a JSON payload
            //http://haacked.com/archive/2009/06/25/json-hijacking.aspx
            return Json(currentFilters, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost] says that only an http post can call this function
        //Allows us to overload Index
        [HttpPost]
        public ActionResult Index(string newFilter)
        {
            //Load the XML document as an object
            XDocument doc = XDocument.Load(myFile);

            //search the key/value pair that matches our parameters, and get the first result
            var messageFilter =
              (from node in doc.Descendants("add") //descendants are the node names
               where node.Attribute("key").Value.Equals("messageFilter") //attributes are the attributes... how about that?
               select node).FirstOrDefault();

            //pass the value of the messageFilter key to a string
            string filters = messageFilter.Attribute("value").Value;

            //append the user's input to the filter string
            filters += String.Concat(';', newFilter);

            //set the value of "value" to the new string
            messageFilter.Attribute("value").SetValue(filters);

            //overwrite the document
            doc.Save(myFile);

            //split the new list of filters to a new array
            string[] currentFilters = filters.Split(';');

            //return the result as a JSON object
            //uses "AllowGet" because by default, the MVC framework does not allow you to respond to an HTTP GET request with a JSON payload
            //http://haacked.com/archive/2009/06/25/json-hijacking.aspx
            return Json(currentFilters, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Config/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Config/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Config/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Config/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Config/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Config/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Config/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
