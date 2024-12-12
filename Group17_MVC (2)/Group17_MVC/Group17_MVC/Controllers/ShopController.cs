using Group17_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class ShopController : Controller
    {
        public readonly Nhom17_WedBanSachEntities db;

        public ShopController()
        {
            db = new Nhom17_WedBanSachEntities();
        }

        public ActionResult Index(string id)
        {

            // Lấy danh sách tất cả sách
            var sach = db.Saches.AsQueryable();

            // Lọc theo mã thể loại nếu tham số id có giá trị
            if (!string.IsNullOrEmpty(id))
            {
                sach = sach.Where(p => p.MaTheLoai == id);
            }

            // Chuyển dữ liệu thành ViewModel
            var result = sach.Select(p => new ShopViewModel
            {
                MaSach = p.MaSach,
                TenSach = p.TenSach,
                DonGia = (double)p.Gia,
                Hinh = p.URLAnhBia ?? "", // Nếu không có URL ảnh, đặt chuỗi rỗng
                MoTa = p.MoTa,
                TenTheLoai = p.TheLoai.TenTheLoai
            }).ToList();

            // Trả về View với dữ liệu
            return View(result);

        }
        public ActionResult Search(string query)
        {
            // Lấy danh sách tất cả sách
            var sach = db.Saches.AsQueryable();

            // Nếu tham số query có giá trị, tìm kiếm sách theo tên
            if (!string.IsNullOrEmpty(query))
            {
                sach = sach.Where(p => p.TenSach.Contains(query));
            }

            // Chuyển dữ liệu thành ViewModel
            var result = sach.Select(p => new ShopViewModel
            {
                MaSach = p.MaSach,
                TenSach = p.TenSach,
                DonGia = (double)p.Gia,
                Hinh = p.URLAnhBia ?? "", // Nếu không có URL ảnh, đặt chuỗi rỗng
                MoTa = p.MoTa,
                TenTheLoai = p.TheLoai.TenTheLoai
            }).ToList();

            // Trả về View với dữ liệu
            return View(result);
        }


        public PartialViewResult ShopItem(IEnumerable<Group17_MVC.ViewModel.ShopViewModel> model)
        {
            return PartialView("ShopItem", model);
        }

        public ActionResult Detail(string id)
        {
            var product = db.Saches
                .Where(p => p.MaSach == id)
                .Select(p => new ChiTietHangHoaVM
                {
                    MaSach = p.MaSach,
                    TenSach = p.TenSach,
                    TenTheLoai = p.TheLoai.TenTheLoai,
                    Hinh = p.URLAnhBia,
                    DonGia = (double)p.Gia,
                    MoTa = p.MoTa,
                    SoLuong = 1 // Mặc định số lượng là 1
                })
                .SingleOrDefault();

            if (product == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return RedirectToAction("NotFound", "Home");
            }

            return View(product);
        }
    }
}