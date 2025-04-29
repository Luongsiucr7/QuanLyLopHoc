using System;
using System.Collections.Generic;

namespace DLL.Model;

public partial class MonHoc
{
    public int Id { get; set; }

    public string MaMon { get; set; } = null!;

    public string TenMon { get; set; } = null!;

    public int TrangThai { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<LopMon> LopMons { get; set; } = new List<LopMon>();
}
