using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group17_MVC.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Tên đăng nhập không được bỏ trống.")]
        [Display(Name ="Tên đăng nhập!")]
        public string MaNguoiDung {  get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string MatKhau { get; set; }
    }
}