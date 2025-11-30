using TiemTraSua.Models;

namespace TiemTraSua.Repository
{
    public interface INhomSpRepository
    {
        TbNhomSanPham Add(TbNhomSanPham nhomSp);
        TbNhomSanPham Update(TbNhomSanPham nhomSp);
        TbNhomSanPham Delete(String maNhomSp);
        TbNhomSanPham GetAllNhomSp(String maNhomSp);
        IEnumerable<TbNhomSanPham> GetAllNhomSp();
    }
}
