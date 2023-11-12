using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonalInfoesController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: admin/PersonalInfoes
        public ActionResult Index()
        {
            return View(db.PersonalInfo.ToList());
        }

        // GET: admin/PersonalInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            if (personalInfo == null)
            {
                return HttpNotFound();
            }
            return View(personalInfo);
        }

        // GET: admin/PersonalInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/PersonalInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,phone,address,opentime,email,meta,hide,order,datebegin")] PersonalInfo personalInfo)
        {
            if (ModelState.IsValid)
            {
                db.PersonalInfo.Add(personalInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personalInfo);
        }

        // GET: admin/PersonalInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            if (personalInfo == null)
            {
                return HttpNotFound();
            }
            return View(personalInfo);
        }

        // POST: admin/PersonalInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,phone,address,opentime,email,meta,hide,order,datebegin")] PersonalInfo personalInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personalInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personalInfo);
        }

        // GET: admin/PersonalInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            if (personalInfo == null)
            {
                return HttpNotFound();
            }
            return View(personalInfo);
        }

        // POST: admin/PersonalInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            db.PersonalInfo.Remove(personalInfo);
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
