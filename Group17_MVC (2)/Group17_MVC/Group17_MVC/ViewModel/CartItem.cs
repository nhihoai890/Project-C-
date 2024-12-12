using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc.Routing.Constraints;

namespace Group17_MVC.ViewModel
{
    public class CartItem
    {
        public String MaSach {  get; set; }
        public String TenSach { get; set; }
        public String Hinh { get; set; }
        public double Gia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => SoLuong * Gia;
    }

    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string DiaChi { get; set; }

        public List<CartItem> CartItems { get; set; }
        public double ThanhTien { get; internal set; }
    }

}