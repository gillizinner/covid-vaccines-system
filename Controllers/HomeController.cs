using CovidVaccinationSystem.App_Start;
using CovidVaccinationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CovidVaccinationSystem.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            memberModel Model = new memberModel();
            List<memberBE> memberList = Model.memberList;
            return View(memberList);
        }
        public ActionResult Details(int id) 
        {
            memberModel Model = new memberModel();
            //memberBE member = Model.memberList.Find(item => item.ID == id);
            //return View(member);
            return View();
        }
        public ActionResult Create() //show the form
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(int id, FormCollection collection) //saves changes in db
        {
            memberModel Model = new memberModel();
            try
            {
                //memberBE member = Model.memberList.Find(item => item.ID==id);
                //memberBE member = Model.memberList.Find(item => item.ID == id);
                //member.firstName = collection["firstName"]; //saves value of spef field from collection in the matching field
                //member.firstName = collection["lastName"];
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id) //show the form
        {
            memberModel Model = new memberModel();
            //memberBE member = Model.memberList.Find(item => item.ID==id);
            //return View(member);
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) //saves changes in db
        {
            memberModel Model = new memberModel();
            try
            {
                //memberBE member = Model.memberList.Find(item => item.ID==id);
                //memberBE member = Model.memberList.Find(item => item.ID == id);
                //member.firstName = collection["firstName"]; //saves value of spef field from collection in the matching field
                //member.firstName = collection["lastName"];
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}