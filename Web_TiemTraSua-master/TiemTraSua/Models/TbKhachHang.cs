using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiemTraSua.Models;

public partial class TbKhachHang
{
    public Guid Id { get; set; }

    [StringLength(50, ErrorMessage = "Tên khách hàng không được vượt quá 50 ký tự.")]
    public string TenKhachHang { get; set; } = null!;

    [StringLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 ký tự.")]
    public string SdtkhachHang { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public virtual ICollection<TbHoaDonBan> TbHoaDonBans { get; set; } = new List<TbHoaDonBan>();
}
