using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group17_MVC.ViewModel
{
    public class RegisterVM
    {
        [Key]
        [Display(Name = "Tên Đăng Nhập")]
        [Required(ErrorMessage = "*")]
        public string MaNguoiDung { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Họ và Tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string XacNhanMatKhau { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Số Điện Thoại")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; }
    }

}