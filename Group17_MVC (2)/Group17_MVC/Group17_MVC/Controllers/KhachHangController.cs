using Group17_MVC.ViewModel;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace Group17_MVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Nhom17_WedBanSachEntities db;

        public KhachHangController()
        {
            db = new Nhom17_WedBanSachEntities();
        }
     

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Chuyển đổi hash thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp tên đăng nhập
                if (db.NguoiDungs.Any(u => u.MaNguoiDung == model.MaNguoiDung))
                {
                    ModelState.AddModelError("MaNguoiDung", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }

                // Kiểm tra trùng lặp email
                if (db.NguoiDungs.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                // Kiểm tra mật khẩu và xác nhận mật khẩu
                if (model.MatKhau != model.XacNhanMatKhau)
                {
                    ModelState.AddModelError("XacNhanMatKhau", "Mật khẩu và xác nhận mật khẩu không khớp.");
                    return View(model);
                }

                var nguoiDung = new NguoiDung
                {
                    MaNguoiDung = model.MaNguoiDung,
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    Email = model.Email,
                    SoDienThoai = model.SoDienThoai,
                    MatKhau = HashPassword(model.MatKhau)  // Mã hóa mật khẩu
                };

                try
                {
                    // Thêm người dùng vào cơ sở dữ liệu
                    db.NguoiDungs.Add(nguoiDung);  // Sửa lại từ "n" thành "nguoiDung"
                    db.SaveChanges();  // Lưu vào cơ sở dữ liệu
                    TempData["Success"] = "Đăng ký thành công!";
                    return RedirectToAction("Index", "Home");  // Điều hướng đến trang chủ
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có sự cố khi lưu vào cơ sở dữ liệu
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }

            // Nếu ModelState không hợp lệ, trả lại view với thông báo lỗi
            return View(model);
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            // Ghi nhớ URL gốc nếu nó tồn tại
            string returnUrl = Request.UrlReferrer?.ToString();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Session["ReturnUrl"] = returnUrl;
            }

            return View();
        }

        public ActionResult DangNhap(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == model.MaNguoiDung);

                if (user == null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập không tồn tại.");
                    return View(model);
                }

                if (user.MatKhau != model.MatKhau)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                    return View(model);
                }
                

                if (user.VaiTro == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                // Đăng nhập thành công
                Session["UserMaNguoiDung"] = user.MaNguoiDung;
                Session["UserName"] = user.HoTen;
                
                // Kiểm tra nếu có URL gốc để chuyển hướng
                if (Session["ReturnUrl"] != null)
                {
                    string returnUrl = Session["ReturnUrl"].ToString();
                    Session["ReturnUrl"] = null; // Xóa URL sau khi dùng
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home"); // Mặc định quay lại trang chủ
            }

            return View(model);
        }



        public ActionResult Logout()
        {
            // Xóa thông tin người dùng khỏi Session
            Session.Clear();

            // Chuyển hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }






    }
}
