using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group17_MVC.ViewModel
{
    public class Sach
    {
        public int MaSach { get; set; }  // Mã sách
        public string TenSach { get; set; }  // Tên sách
        public string TacGia { get; set; }  // Tác giả
        public string URLAnhBia { get; set; }  // Đường dẫn ảnh bìa
        public double Gia { get; set; }  // Giá sách

        // Mối quan hệ giữa Sach và ChiTietDonHang (1-N)
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }

}