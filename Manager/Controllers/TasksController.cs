using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Manager.Models;

namespace Manager.Controllers
{
    public class TasksController : Controller
    {
        private pentonixEntities1 db = new pentonixEntities1();

        // GET: Tasks
       
        public ActionResult Index()
        {
            return View(db.Tasks.ToList());
        }

        // GET: Tasks/Details/5
   
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
       
        public ActionResult Create()
        {
            dynamic dy = new ExpandoObject();
            var Fname = db.Users.ToList();
            List<string> list = new List<string>();
            foreach (var s in Fname)
            {
                list.Add(s.FirstName);
            }

            ViewBag.NameList = new SelectList(list, "Name");

            Task model = new Task();
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketNo,Task1,Name,TaskStatus,PlanedEffort,Date,Comment")] Task task)
        {
            if (ModelState.IsValid)
            {

                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }





        // GET: Tasks/Delete/5
      
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
