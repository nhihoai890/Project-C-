using Group17_MVC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class QuanlinguoidungController : Controller
    {
        Nhom17_WedBanSachEntities db = new Nhom17_WedBanSachEntities();
        private static readonly HashSet<string> lockedAccounts = new HashSet<string>();

        public static HashSet<string> GetLockedAccounts()
        {
            return lockedAccounts;
        }

        public ActionResult LockAccount(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                lockedAccounts.Add(id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UnlockAccount(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                lockedAccounts.Remove(id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index(string searchString)
        {
            var nguoidung = string.IsNullOrEmpty(searchString)
                ? db.NguoiDungs.ToList()
                : db.NguoiDungs.Where(d =>
                    d.MaNguoiDung.Contains(searchString) ||
                    d.HoTen.Contains(searchString) ||
                    d.Email.Contains(searchString) ||
                    d.SoDienThoai.Contains(searchString)).ToList();
            ViewBag.LockedAccounts = lockedAccounts;
            ViewBag.SearchString = searchString;
            return View(nguoidung);
        }

        public ActionResult Edit(string id)
        {
            var item = db.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var roles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "KhachHang", Value = "KhachHang" }
            };
            ViewBag.VaiTroList = roles;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var user = db.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == model.MaNguoiDung);
                if (user != null)
                {
                    user.VaiTro = model.VaiTro;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public ActionResult Add()
        {
            
            var roles = new List<SelectListItem>
    {
        new SelectListItem { Text = "Admin", Value = "Admin" },
        new SelectListItem { Text = "KhachHang", Value = "KhachHang" }
    };
            ViewBag.VaiTroList = roles;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                db.NguoiDungs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}