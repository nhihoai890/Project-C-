using Group17_MVC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class QuanlisanphamController : Controller
    {
        // GET: Admin/Quanlisanpham
        private Nhom17_WedBanSachEntities db = new Nhom17_WedBanSachEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            using (var context = new Nhom17_WedBanSachEntities())
            {
                var theLoaiList = context.TheLoais
                                     .Select(tl => new SelectListItem
                                     {
                                         Value = tl.MaTheLoai.ToString(),
                                         Text = tl.MaTheLoai.ToString()
                                     }).ToList();

                ViewBag.TheLoaiList = theLoaiList;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Sach model)
        {
            if (ModelState.IsValid)
            {
                model.NgayTao = DateTime.Now;
                db.Saches.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);

        }
        public ActionResult Edit(string id)
        {
            
            var categories = db.TheLoais.ToList();
            ViewBag.TheLoaiList = new SelectList(categories, "MaTheLoai", "TenTheLoai");
            var item = db.Saches.FirstOrDefault(s => s.MaSach == id);

            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sach model)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var categories = db.TheLoais.ToList();
            ViewBag.TheLoaiList = new SelectList(categories, "MaTheLoai", "TenTheLoai");

            return View(model);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.Saches.FirstOrDefault(s => s.MaSach == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var item = db.Saches.FirstOrDefault(s => s.MaSach == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            db.Saches.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}