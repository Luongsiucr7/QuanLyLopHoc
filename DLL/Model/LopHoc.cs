using System;
using System.Collections.Generic;

namespace DLL.Model;

public partial class LopHoc
{
    public int Id { get; set; }

    public string MaLop { get; set; } = null!;

    public string TenLop { get; set; } = null!;

    public int KhoiLop { get; set; }

    public int TrangThai { get; set; }

    public virtual ICollection<LopGiaoVien> LopGiaoViens { get; set; } = new List<LopGiaoVien>();

    public virtual ICollection<LopMon> LopMons { get; set; } = new List<LopMon>();

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
