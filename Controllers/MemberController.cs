using CovidVaccinationSystem.App_Start;
using CovidVaccinationSystem.Models;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Linq;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace CovidVaccinationSystem.Controllers
{
    public class MemberController : Controller
    {
        MongoContext _mongoContext;
        public MemberController()
        {
            _mongoContext = new MongoContext();
        }
        // GET: Member
        public ActionResult Index()
        {
            var membersList = _mongoContext._database.GetCollection<memberBE>("Members").FindAll().ToList();
            return View(membersList);
        }

        // GET: Member/Details/5
        public ActionResult Details(string id)
        {
            var memberID = Query<memberBE>.EQ(item => item.ID, new ObjectId(id)); //filtering collection by matching id's
            var memberDetails = _mongoContext._database.GetCollection<memberBE>("Members").FindOne(memberID); //finds the spef member from collection
            return View(memberDetails);
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(memberBE newMember)
        {
            try
            {
                //set dates so one day isn't substraced
                memberBE newEditedMember = setDatesFormat(newMember);
                for (int i = 0; i < newMember.vaccinesArray.Length; i++) //beacuse of date picker for vac dates(does'nt exist in edit)
                {
                    if (DateTime.Compare(newMember.vaccinesArray[i].dateOfVaccine, new DateTime()) != 0)
                    {
                        newMember.vaccinesArray[i].dateOfVaccine = newMember.vaccinesArray[i].dateOfVaccine.ToUniversalTime().AddDays(1);
                    }
                }

                //getting collection from db
                var memberCollection = _mongoContext._database.GetCollection<BsonDocument>("Members"); 

                //checking for input validations

               
                //checking for duplicates
                var query = Query.EQ("idOfMember", newMember.idOfMember); //query that asks who has that id
                var count = memberCollection.FindAs<memberBE>(query).Count(); //how many members in collection match the query
                if (count != 0)
                {
                    //TempData["Message"] = "Member ALready Exists";
                    ViewBag.Error = "Member with that id already exists";
                    return View("Create", newMember);
                }
                //wrong date input 
                if (Validations(newMember)==false)
                {
                    return View("Create", newMember);
                }
                //no mistake - insert to db
                var result = memberCollection.Insert(newMember);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Edit/5
        public ActionResult Edit(string id)
        {
            var memberCollection = _mongoContext._database.GetCollection<memberBE>("Members");

            var memberDetailscount = memberCollection.FindAs<memberBE>(Query.EQ("_id", new ObjectId(id))).Count(); //filtering collection by matching id's

            if (memberDetailscount > 0) //if spef member exists in collection
            {
                var memberbjectid = Query<memberBE>.EQ(p => p.ID, new ObjectId(id)); //check what that does

                var memberDetails = _mongoContext._database.GetCollection<memberBE>("Members").FindOne(memberbjectid);

                return View(memberDetails);
            }
            return RedirectToAction("Index");
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, memberBE editedMember)
        {
            try
            {
                //set dates so one day isn't substraced
                memberBE newEditedMember = setDatesFormat(editedMember);

                editedMember.ID = new ObjectId(id);
                //Mongo Query  
                var memberObjectId = Query<memberBE>.EQ(item => item.ID, new ObjectId(id));
                // Document Collections  
                var memberCollection = _mongoContext._database.GetCollection<memberBE>("Members");

                //beacuase id of member is editable - we have to keep it in the new edited member
                var wantedToEditMember = _mongoContext._database.GetCollection<memberBE>("Members").FindOne(memberObjectId);
                editedMember.idOfMember=wantedToEditMember.idOfMember;

                //checking for input validations

                //wrong input
                if (Validations(editedMember) == false)
                {
                    return View("Edit", editedMember);
                }

                //no mistake - update in db
                // Document Update which need Id and Data to Update  
                var result = memberCollection.Update(memberObjectId, Update.Replace(editedMember), UpdateFlags.None);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Member/Delete/5
        public ActionResult Delete(string id)
        {
            var memberObjectId = Query<memberBE>.EQ(item => item.ID, new ObjectId(id));
            var memberDetails = _mongoContext._database.GetCollection<memberBE>("Members").FindOne(memberObjectId);
            return View(memberDetails);
        }

        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, memberBE memberToDelete)
        {
            try
            {
                //Mongo Query  
                var memberObjectId = Query<memberBE>.EQ(item => item.ID, new ObjectId(id));
                // Document Collections  
                var memberCollection = _mongoContext._database.GetCollection<memberBE>("Members");
                // Document Delete which need ObjectId to Delete Data   
                var result = memberCollection.Remove(memberObjectId, RemoveFlags.Single);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //makes input validations such wrong dates
        public bool Validations(memberBE member)
        {
            
            //checking for date mistakes

            //not a normal age
            if (DateTime.Compare(member.dateOfBirth, new DateTime()) != 0) //checking if there is date of birth value
            {
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - member.dateOfBirth.Year;
                // Go back to the year in which the person was born in case of a leap year
                if (member.dateOfBirth.Date > today.AddYears(-age)) age--;
                if (age < 0 || age > 120) //checking if age is normal
                {
                    ViewBag.Error = "Wrong date of birth";
                    return false;
                }
            }

            // checking for unreasonable order - birth after recovery etc
            List<DateTime> datesWantedOrder = new List<DateTime>(); //list ordered properly
            if (DateTime.Compare(member.dateOfBirth, new DateTime()) != 0) datesWantedOrder.Add(member.dateOfBirth);
            if (DateTime.Compare(member.dateOfPositiveResult, new DateTime()) != 0) datesWantedOrder.Add(member.dateOfPositiveResult);
            if (DateTime.Compare(member.dateOfRecovery, new DateTime()) != 0) datesWantedOrder.Add(member.dateOfRecovery);
            List<DateTime> daysActualOrder = datesWantedOrder.ToList();
            datesWantedOrder.Sort(); //actual ordered list
            for (int i = 0; i < datesWantedOrder.Count(); i++)
            {
                if (datesWantedOrder[i] != daysActualOrder[i])
                {
                    ViewBag.Error = "Wrong Dates order";
                    return false;
                }
            }
            
            // checking for unreasonable Vaccines dates order - first after third etc
            List<DateTime> vaccinesDatesWantedOrder = new List<DateTime>(); //list ordered properly
            for (int i = 0; i < 4; i++)
            {
                if (DateTime.Compare(member.vaccinesArray[i].dateOfVaccine, new DateTime()) != 0) vaccinesDatesWantedOrder.Add(member.vaccinesArray[i].dateOfVaccine);
            }
            List<DateTime> vaccinesDatesActualOrder = vaccinesDatesWantedOrder.ToList();
            vaccinesDatesActualOrder.Sort(); //actual ordered list
            for (int i = 0; i < vaccinesDatesActualOrder.Count(); i++)
            {
                if (vaccinesDatesWantedOrder[i] != vaccinesDatesActualOrder[i])
                {
                    ViewBag.Error = "Wrong Dates order";
                    return false;
                }
            }
            
            //check first vaccine date isn't after birth
            if (DateTime.Compare(member.dateOfBirth, new DateTime()) != 0 && DateTime.Compare(member.vaccinesArray[0].dateOfVaccine, new DateTime()) != 0)
            {
                if (DateTime.Compare(member.dateOfBirth, member.vaccinesArray[0].dateOfVaccine) > 0)
                {
                    ViewBag.Error = "Wrong Dates order - first vaccine before birth";
                    return false;
                }
            }
            return true;
        }
        public memberBE setDatesFormat(memberBE member)
        {
            if (DateTime.Compare(member.dateOfBirth, new DateTime())!=0)
            {
                member.dateOfBirth = member.dateOfBirth.ToUniversalTime().AddDays(1);
            }
            if (DateTime.Compare(member.dateOfPositiveResult, new DateTime()) != 0)
            {
                member.dateOfPositiveResult = member.dateOfPositiveResult.ToUniversalTime().AddDays(1);
            }
            if (DateTime.Compare(member.dateOfRecovery, new DateTime()) != 0)
            {
                member.dateOfRecovery = member.dateOfRecovery.ToUniversalTime().AddDays(1);
            }
            
            return member; 
        }
    }
}
