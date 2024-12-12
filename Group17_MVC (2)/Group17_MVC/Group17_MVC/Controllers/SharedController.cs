using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class SharedController : Controller
    {
        private readonly Nhom17_WedBanSachEntities db;

        public SharedController() { 
            db= new Nhom17_WedBanSachEntities();
        }
        public SharedController(Nhom17_WedBanSachEntities context) => db = context;


        public ActionResult MenuLoai()
        {
            var data = db.Saches
                .GroupBy(s => new { s.MaTheLoai, s.TheLoai.TenTheLoai }) // Nhóm theo thể loại
                .Select(g => new ViewModel.MenuLoaiVM
                {
                    MaTheLoai = g.Key.MaTheLoai,
                    TenTheLoai = g.Key.TenTheLoai,
                    SoLuong = g.Count(), // Đếm số lượng sách
                })
                .ToList();

            System.Diagnostics.Debug.WriteLine("Dữ liệu truyền vào Partial View: " + data.Count);
            return PartialView("~/Views/Shop/MenuLoai.cshtml", data);
        }

    }
}