//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Group17_MVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class DanhSachYeuThich
    {
        public int MaYeuThich { get; set; }
        public string MaNguoiDung { get; set; }
        public string MaSach { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual Sach Sach { get; set; }
    }
}