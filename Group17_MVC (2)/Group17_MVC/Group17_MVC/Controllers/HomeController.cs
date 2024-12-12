using Group17_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Nhom17_WedBanSachEntities db = new Nhom17_WedBanSachEntities();
        public ActionResult Index()
        {
           

            return View();

        }

       public ActionResult GetBookByAll(string[] maTheLoai)
        {
            // Nếu không có mã thể loại nào được chọn, mặc định chọn TL001 và TL002
            if (maTheLoai == null || maTheLoai.Length == 0)
            {
                maTheLoai = new[] { "TL008", "TL009", "TL003", "TL005" }; // Thiết lập mặc định
            }

            // Lấy danh sách sách theo các mã thể loại
            var books = db.Saches.Where(s => maTheLoai.Contains(s.MaTheLoai)).ToList();

            // Chuyển đổi danh sách sách thành danh sách ShopViewModel
            var shopViewModel = books.Select(book => new ShopViewModel
            {
                MaSach = book.MaSach,
                TacGia = book.TacGia,
                TenSach = book.TenSach,
                DonGia = (double)book.Gia,
                Hinh = book.URLAnhBia,
                TenTheLoai = book.TheLoai.TenTheLoai,
            }).ToList();

            return PartialView("_BooksPartial3", shopViewModel);
        }
       


        public ActionResult GetBooksByCategory(string maTheLoai = "TL002")
        {
            // Lấy danh sách sách theo mã thể loại
            var books = db.Saches.Where(s => s.MaTheLoai == maTheLoai).ToList();

            // Kiểm tra xem có sách trong danh sách không
            if (books == null || !books.Any())
            {
                // Trả về thông báo nếu không có sách
                return Content("Không có sách nào trong thể loại này");
            }

            // Chuyển đổi sách thành ShopViewModel
            var shopViewModel = books.Select(book => new ShopViewModel
            {MaSach = book.MaSach,
            TacGia= book.TacGia,
                TenSach = book.TenSach,
                DonGia = (double)book.Gia,
                Hinh = book.URLAnhBia,
                TenTheLoai = book.TheLoai.TenTheLoai,
            }).ToList();

            return PartialView("_BooksPartial", shopViewModel); // Trả về PartialView với danh sách sách
        }
        public ActionResult GetBookByTrend(string maTheLoai = "TL003")
        {
            // Lấy danh sách sách theo mã thể loại
            var books = db.Saches.Where(s => s.MaTheLoai == maTheLoai).ToList();

            // Kiểm tra xem có sách trong danh sách không
            if (books == null || !books.Any())
            {
                // Trả về thông báo nếu không có sách
                return Content("Không có sách nào trong thể loại này");
            }

            // Chuyển đổi sách thành ShopViewModel
            var shopViewModel = books.Select(book => new ShopViewModel
            {
                MaSach = book.MaSach,   
                TenSach = book.TenSach,
                DonGia = (double)book.Gia,
                Hinh = book.URLAnhBia,
                TenTheLoai = book.TheLoai.TenTheLoai,
            }).ToList();

            return PartialView("_BooksPartial1", shopViewModel); // Trả về PartialView với danh sách sách
        }

        public ActionResult GetBookBySale(string maTheLoai = "TL006")
        {
            var books = db.Saches.Where(s => s.MaTheLoai == maTheLoai).ToList();

            // Kiểm tra xem có sách trong danh sách không
            if (books == null || !books.Any())
            {
                // Trả về thông báo nếu không có sách
                return Content("Không có sách nào trong thể loại này");
            }

            // Chuyển đổi sách thành ShopViewModel
            var shopViewModel = books.Select(book => new ShopViewModel
            { MaSach = book.MaSach,
                TacGia = book.TacGia,
                TenSach = book.TenSach,
                DonGia = (double)book.Gia,
                Hinh = book.URLAnhBia,
                TenTheLoai = book.TheLoai.TenTheLoai,
            }).ToList();

            return PartialView("_BooksPartial2", shopViewModel); // Trả về PartialView với danh sách sách
        }
      



        public ActionResult Detail(string id)
        {
            var product = db.Saches
                .Where(p => p.MaSach == id)
                .Select(p => new ChiTietHangHoaVM
                {
                    MaSach = p.MaSach,
                    TacGia = p.TacGia,
                    TenTheLoai = p.TheLoai.TenTheLoai,
                    TenSach = p.TenSach,
                    Hinh = p.URLAnhBia,
                    DonGia = (double)p.Gia,
                    MoTa = p.MoTa,
                    SoLuong = 1 // Mặc định số lượng là 1
                })
                .SingleOrDefault();

            if (product == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return RedirectToAction("Index", "Home");
            }

            return View(product);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
      public ActionResult NotFound() {
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}