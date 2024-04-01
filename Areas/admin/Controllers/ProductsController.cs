using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using System.Data.Entity.Validation;
using System.IO;
using WebDemo.Helper;

namespace WebDemo.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ShopOnlineEntities db = new ShopOnlineEntities();

        // GET: admin/Products
        public ActionResult Index(long? id = null)
        {
            getCategory(id);
            return View();
        }

        public void getCategory(long? selectedId = null)
        {
            ViewBag.Category = new SelectList(db.ListCategories.Where(x => x.hide == false).OrderBy(x =>x.order), "id", "name", selectedId);
        }

        public ActionResult getProduct(long? id)
        {
            if(id == null)
            {
                return PartialView(db.Products.OrderBy(x => x.order).ToList());
            }
            return PartialView(db.Products.Where(x => x.categoryid == id).OrderBy(x => x.order).ToList());
        }
        
        // GET: admin/Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.name = db.ListCategories.Find(products.categoryid).name;

            return View(products);
        }

        // GET: admin/Products/Create
        public ActionResult Create()
        {
            getCategory();
            return View();
        }

        // POST: admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]

        public ActionResult Create([Bind(Include = "name,price,img,description,meta,hide,order,datebegin,categoryid,latest_product,top_product,review_product, availability, shipping_day, weight, detail_description")] Products products, HttpPostedFileBase img)
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
                        path = Path.Combine(Server.MapPath("~/Uploads/img/product"), filename);
                        img.SaveAs(path);
                        products.img = filename; //Lưu ý
                    }
                    else
                    {
                        products.img = "logo.png";
                    }
                    products.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    products.order = getMaxOrder((int)products.categoryid) + 1;
                    db.Products.Add(products);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", "Products", new { id = products.categoryid });
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
            return View(products);
        }

        public int getMaxOrder(int categoryId)
        {
            return db.Products.Where(x => x.categoryid == categoryId).Count();
        }

        // GET: admin/Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            getCategory(products.categoryid);
            return View(products);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id, name,price,img,description,meta,hide,order,datebegin,categoryid,latest_product,top_product,review_product, availability, shipping_day, weight, detail_description")] Products products, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                Products temp = db.Products.Find(products.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        //filename = Guid.NewGuid().ToString() + img.FileName;
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/Uploads/img/product"), filename);
                        img.SaveAs(path);
                        temp.img = filename; //Lưu ý
                    }
                    // temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());                   
                    temp.name = products.name;
                    temp.price = products.price;
                    temp.description = products.description;
                    temp.meta = products.meta;
                    temp.hide = products.hide;
                    temp.order = products.order;
                    temp.categoryid = products.categoryid;
                    temp.latest_product = products.latest_product;
                    temp.top_product = products.top_product;
                    temp.review_product = products.review_product;
                    temp.availability = products.availability;
                    temp.shipping_day = products.shipping_day;
                    temp.weight = products.weight;
                    temp.detail_description = products.detail_description;
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", "Products", new { id = products.categoryid });
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

            return View(products);
        }

        // GET: admin/Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Products products = await db.Products.FindAsync(id);
            db.Products.Remove(products);
            await db.SaveChangesAsync();
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
