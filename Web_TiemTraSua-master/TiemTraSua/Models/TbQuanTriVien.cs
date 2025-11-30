using System;
using System.Collections.Generic;

namespace TiemTraSua.Models;

public partial class TbQuanTriVien
{
    public int Id { get; set; }

    public string TenNguoiDung { get; set; } = null!;

    public string MatKhau { get; set; } = null!;
}
