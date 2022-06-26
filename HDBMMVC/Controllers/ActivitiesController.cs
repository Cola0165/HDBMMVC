using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HDBMMVC.Models;
using Microsoft.AspNet.Identity;
using QXHMVC.Models;

namespace HDBMMVC.Controllers
{
    public class ActivitiesController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Activities
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyIndex()
        {
            string userid = User.Identity.GetUserId().ToString();
            return View(db.Activities.Where(r => r.UserId == userid).ToList());
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            return View(db.Activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null || activity.UserId != User.Identity.GetUserId().ToString())
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            
            //
            return View();
        }

        // POST: Activities/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Major,Sno,Phone,Email")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.UserId = User.Identity.GetUserId().ToString();
                activity.CreateId= Guid.NewGuid().ToString();
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("CreateSuccess", new { CreateId = activity.CreateId });
            }

            return View(activity);
        }

        public ActionResult CreateSuccess(string CreateId)
        {
            ViewBag.createid = CreateId;
            return View();
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null || activity.UserId != User.Identity.GetUserId().ToString())
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Major,Sno,Phone,Email")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null || activity.UserId != User.Identity.GetUserId().ToString())
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
