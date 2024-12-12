using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group17_MVC.ViewModel
{
    public class OrderSuccessViewModel
    {
        public int MaDonHang { get; set; }      // Mã đơn hàng
        public DateTime NgayDat { get; set; }   // Ngày đặt hàng
        public double ThanhTien { get; set; }   // Tổng tiền của đơn hàng
        public string HoTenKhachHang { get; set; } // Họ tên khách hàng
        public string DiaChiGiao { get; set; }  // Địa chỉ giao hàng
        public List<CartItem> CartItems { get; internal set; }
    }
}