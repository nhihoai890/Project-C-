using Group17_MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class ThongKeController : Controller
    {
        private Nhom17_WedBanSachEntities db = new Nhom17_WedBanSachEntities();

        public ActionResult Index(string searchQuery)
        {
            int tongnguoidung = db.NguoiDungs.Count();
            ViewBag.Tonguser = tongnguoidung;
            int tongdonhang = db.DonHangs.Count();
            ViewBag.Tongdh = tongdonhang;
            int sachchuadat = db.Saches.Where(s => !s.ChiTietDonHangs.Any()).Count();
            ViewBag.SachChuaDat = sachchuadat;

            var listsach = db.Saches
                .Where(s => !s.ChiTietDonHangs.Any())
                .Select(s => new SachViewModel
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach
                });

            if (!string.IsNullOrEmpty(searchQuery))
            {
                listsach = listsach.Where(s => s.MaSach.Contains(searchQuery));
            }

            return View(listsach.ToList());
        }

    }

    public class SachViewModel
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
    }
}