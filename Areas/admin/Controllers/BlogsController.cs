using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;

namespace WebDemo.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogsController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: admin/Blogs
        public ActionResult Index(long? id = null)
        {
            getCategory();
            return View();
        }

        public void getCategory(long? selectedId = null)
        {
            ViewBag.Category = new SelectList(db.BlogCategories.Where(x => x.hide == false).OrderBy(x => x.order), "id", "name", selectedId);
        }

        public ActionResult getBlog(long? id)
        {
            if (id == null)
            {
                return PartialView(db.Blogs.OrderBy(x => x.order).ToList());
            }
            return PartialView(db.Blogs.Where(x => x.categoryid == id).OrderBy(x => x.order).ToList());
        }

        // GET: admin/Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            if (blogs == null)
            {
                return HttpNotFound();
            }

            ViewBag.name = db.BlogCategories.Find(blogs.categoryid).name;

            return View(blogs);
        }

        // GET: admin/Blogs/Create
        public ActionResult Create()
        {
            getCategory();
            return View();
        }

        // POST: admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "name,detail,description,comments,dateupload,meta,hide,order,datebegin,img,categoryid,author")] Blogs blogs, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        //filename = Guid.NewGuid().ToString() + img.FileName;
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/Uploads/img/blog/"), filename);
                        img.SaveAs(path);
                        blogs.img = filename; //Lưu ý
                    }
                    else
                    {
                        blogs.img = "logo.png";
                    }
                    blogs.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    blogs.order = getMaxOrder((int)blogs.categoryid) + 1;
                    db.Blogs.Add(blogs);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", "Blogs", new { id = blogs.categoryid });
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(blogs);
        }

        public int getMaxOrder(int categoryId)
        {
            return db.Blogs.Where(x => x.categoryid == categoryId).Count();
        }

        // GET: admin/Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            if (blogs == null)
            {
                return HttpNotFound();
            }
            getCategory(blogs.categoryid);
            return View(blogs);
        }

        // POST: admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,detail,description,comments,dateupload,meta,hide,order,datebegin,img,categoryid,author")] Blogs blogs, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                Blogs temp = db.Blogs.Find(blogs.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        //filename = Guid.NewGuid().ToString() + img.FileName;
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/Uploads/img/blog"), filename);
                        img.SaveAs(path);
                        temp.img = filename; //Lưu ý
                    }
                    // temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());                   
                    temp.name = blogs.name;
                    temp.description = blogs.description;
                    temp.meta = blogs.meta;
                    temp.hide = blogs.hide;
                    temp.order = blogs.order;
                    temp.categoryid = blogs.categoryid;
                    temp.detail = blogs.detail;
                    temp.comments = blogs.comments;
                    temp.author = blogs.author;
                    
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", "Blogs", new { id = blogs.categoryid });
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(blogs);
        }

        // GET: admin/Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blogs = db.Blogs.Find(id);
            if (blogs == null)
            {
                return HttpNotFound();
            }
            return View(blogs);
        }

        // POST: admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blogs blogs = db.Blogs.Find(id);
            db.Blogs.Remove(blogs);
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
