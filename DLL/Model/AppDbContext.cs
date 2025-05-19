using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DLL.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diem> Diems { get; set; }

        public virtual DbSet<LopGiaoVien> LopGiaoViens { get; set; }

        public virtual DbSet<LopHoc> LopHocs { get; set; }

        public virtual DbSet<LopMon> LopMons { get; set; }

        public virtual DbSet<MonHoc> MonHocs { get; set; }

        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Data Source=LUONGNDDPH50650\\SQLEXPRESS;Initial Catalog=QuanLyLopHoc4;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;MultipleActiveResultSets=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__diem__3213E83FAD09625C");

                entity.ToTable("diem");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdMonHoc).HasColumnName("id_mon_hoc");
                entity.Property(e => e.IdNguoiDung).HasColumnName("id_nguoi_dung");
                entity.Property(e => e.MaDiem)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('DH'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", true)
                    .HasColumnName("ma_diem");
                entity.Property(e => e.SoDiem)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("so_diem");
                entity.Property(e => e.TenDiem)
                    .HasMaxLength(100)
                    .HasColumnName("ten_diem");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");

                entity.HasOne(d => d.IdMonHocNavigation).WithMany(p => p.Diems)
                    .HasForeignKey(d => d.IdMonHoc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__diem__id_mon_hoc__5070F446");

                entity.HasOne(d => d.IdNguoiDungNavigation).WithMany(p => p.Diems)
                    .HasForeignKey(d => d.IdNguoiDung)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__diem__id_nguoi_d__5165187F");
            });

            modelBuilder.Entity<LopGiaoVien>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__lop_giao__3213E83F3FDD8286");

                entity.ToTable("lop_giao_vien");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdLopHoc).HasColumnName("id_lop_hoc");
                entity.Property(e => e.IdNguoiDung).HasColumnName("id_nguoi_dung");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");

                entity.HasOne(d => d.IdLopHocNavigation).WithMany(p => p.LopGiaoViens)
                    .HasForeignKey(d => d.IdLopHoc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__lop_giao___id_lo__5535A963");

                entity.HasOne(d => d.IdNguoiDungNavigation).WithMany(p => p.LopGiaoViens)
                    .HasForeignKey(d => d.IdNguoiDung)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__lop_giao___id_ng__5441852A");
            });

            modelBuilder.Entity<LopHoc>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__lop_hoc__3213E83FE4ECF793");

                entity.ToTable("lop_hoc");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.KhoiLop).HasColumnName("khoi_lop");
                entity.Property(e => e.MaLop)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('LH'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", true)
                    .HasColumnName("ma_lop");
                entity.Property(e => e.TenLop)
                    .HasMaxLength(100)
                    .HasColumnName("ten_lop");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");
            });

            modelBuilder.Entity<LopMon>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__lop_mon__3213E83F1B5F446A");

                entity.ToTable("lop_mon");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.IdLop).HasColumnName("id_lop");
                entity.Property(e => e.IdMonHoc).HasColumnName("id_mon_hoc");
                entity.Property(e => e.IdNguoiDung).HasColumnName("id_nguoi_dung");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");

                entity.HasOne(d => d.IdLopNavigation).WithMany(p => p.LopMons)
                    .HasForeignKey(d => d.IdLop)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__lop_mon__id_lop__5812160E");

                entity.HasOne(d => d.IdMonHocNavigation).WithMany(p => p.LopMons)
                    .HasForeignKey(d => d.IdMonHoc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__lop_mon__id_mon___59063A47");

                entity.HasOne(d => d.IdNguoiDungNavigation).WithMany(p => p.LopMons)
                    .HasForeignKey(d => d.IdNguoiDung)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__lop_mon__id_nguo__59FA5E80");
            });

            modelBuilder.Entity<MonHoc>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__mon_hoc__3213E83F3D8D09B4");

                entity.ToTable("mon_hoc");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.MaMon)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('MH'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", true)
                    .HasColumnName("ma_mon");
                entity.Property(e => e.TenMon)
                    .HasMaxLength(100)
                    .HasColumnName("ten_mon");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__nguoi_du__3213E83FA9AD549F");

                entity.ToTable("nguoi_dung");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DiaChi)
                    .HasMaxLength(255)
                    .HasColumnName("dia_chi");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.GioiTinh).HasColumnName("gioi_tinh");
                entity.Property(e => e.IdLopHoc).HasColumnName("id_lop_hoc");
                entity.Property(e => e.MaNguoiDung)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('ND'+right('0000'+CONVERT([varchar](4),[ID]),(4)))", true)
                    .HasColumnName("ma_nguoi_dung");
                entity.Property(e => e.MatKhau)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mat_khau");
                entity.Property(e => e.NgaySinh).HasColumnName("ngay_sinh");
                entity.Property(e => e.NhanXet)
                    .HasMaxLength(255)
                    .HasColumnName("nhan_xet");
                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("so_dien_thoai");
                entity.Property(e => e.TenLinkAnh)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ten_link_anh");
                entity.Property(e => e.TenNguoiDung)
                    .HasMaxLength(100)
                    .HasColumnName("ten_nguoi_dung");
                entity.Property(e => e.TrangThai).HasColumnName("trang_thai");
                entity.Property(e => e.VaiTro).HasColumnName("vai_tro");

                entity.HasOne(d => d.IdLopHocNavigation).WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.IdLopHoc)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__nguoi_dun__id_lo__4D94879B");
            });

        }

    }
}
