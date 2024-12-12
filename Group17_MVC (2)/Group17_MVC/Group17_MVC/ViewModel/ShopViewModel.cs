using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group17_MVC.ViewModel
{
    public class ShopViewModel
    {
        internal int? SoLuongTon;

        public String MaSach { get; set; }
        public String TenSach { get; set; }
        public String TacGia { get; set; }
        public String Hinh { get; set; }
        public double DonGia { get; set; }
        public String MoTa { get; set; }
        public String TenTheLoai { get; set; }



    }

    public class ChiTietHangHoaVM
    {
        public String MaSach { get; set; }
        public String TenSach { get; set; }
        public String TacGia { get; set; }
        public String Hinh { get; set; }
        public double DonGia { get; set; }
        public String MoTa { get; set; }
        public String TenTheLoai { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongTon {  get; set; }
    }
}