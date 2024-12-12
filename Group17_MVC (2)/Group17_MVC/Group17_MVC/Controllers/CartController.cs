using Group17_MVC.Helpers;
using Group17_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group17_MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Nhom17_WedBanSachEntities db = new Nhom17_WedBanSachEntities();

        public ActionResult Index()
        {
            // Lấy giỏ hàng từ Session
            var cart = SessionHelper.GetCart();

            // Nếu giỏ hàng trống
            if (cart == null || !cart.Any())
            {
                TempData["Message"] = "Giỏ hàng của bạn hiện tại chưa có sản phẩm.";
            }

           

            return View(cart); // Trả về View giỏ hàng với dữ liệu

        }


        [HttpPost]
        public ActionResult AddToCart(string maSach)
        {
            // Kiểm tra nếu người dùng chưa đăng nhập
            if (Session["UserMaNguoiDung"] == null)
            {
                // Lưu URL của trang hiện tại để quay lại sau khi đăng nhập
                Session["ReturnUrl"] = Request.UrlReferrer?.ToString();
                return RedirectToAction("DangNhap", "KhachHang");
            }

            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.MaSach == maSach);
            if (existingItem != null)
            {
                existingItem.SoLuong += 1;
            }
            else
            {
                var sach = db.Saches.Find(maSach);
                if (sach != null)
                {
                    cart.Add(new ViewModel.CartItem
                    {
                        MaSach = sach.MaSach,
                        TenSach = sach.TenSach,
                        Hinh = sach.URLAnhBia,
                        Gia = (double)sach.Gia,
                        SoLuong = 1
                    });
                }
            }

            SessionHelper.SetCart(cart);
            return RedirectToAction("Index", "Cart");
        }

        


        [HttpPost]
        public ActionResult RemoveFromCart(string maSach)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if(cart != null)
            {
                var itemToRemove = cart.FirstOrDefault(x => x.MaSach == maSach);
                if(itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                }
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index", "Cart");
        }
       
       
        [HttpPost]
        public ActionResult UpdateCart(string maSach, string action, int quantity)
        {
            var cart = SessionHelper.GetCart();

            var existingItem = cart.FirstOrDefault(c => c.MaSach == maSach);
            if (existingItem != null)
            {
                if (action == "increase")
                {
                    existingItem.SoLuong += 1;
                }
                else if (action == "decrease")
                {
                    existingItem.SoLuong -= 1;
                    if (existingItem.SoLuong <= 0)
                    {
                        cart.Remove(existingItem); // Xóa sản phẩm nếu số lượng <= 0
                    }
                }
            }

            SessionHelper.SetCart(cart);
            return RedirectToAction("Index");
        }
        public ActionResult CartIcon()
        {
            var cart = SessionHelper.GetCart();
            int totalQuantity = cart.Sum(item => item.SoLuong);
            return PartialView("_CartIcon", totalQuantity);
        }

        public ActionResult Checkout(CheckoutViewModel model)
        {

            // Kiểm tra xem giỏ hàng có sản phẩm hay không
            var cartItems = Session["Cart"] as List<CartItem>;
            if (cartItems == null || cartItems.Count == 0)
            {
                ModelState.AddModelError("", "Giỏ hàng của bạn không có sản phẩm.");
            }
            ;
            if (ModelState.IsValid)
            {
                // Lấy giỏ hàng từ session
                _ = Session["Cart"] as List<CartItem>;

                TempData["BookNames"] = cartItems.Select(item => item.TenSach).ToList();
                // Sau khi thanh toán thành công, chuyển hướng đến trang OrderSuccess
                Session["Cart"] = null; // Xóa giỏ hàng sau khi thanh toán thành công

                return RedirectToAction("OrderSuccess");
            }
            model.CartItems = cartItems ?? new List<CartItem>(); // Tránh null cho CartItems

            return View(model);


        }


        

        public ActionResult OrderSuccess()
        {
            
            return View();
        }


    }
}