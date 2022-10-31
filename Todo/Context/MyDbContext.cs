using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeptTb> DeptTbs { get; set; }

    public virtual DbSet<EmpTb> EmpTbs { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer(
//             "Server=localhost,1437;Database=DB_EMP_DEPT;Trusted_Connection=false;TrustServerCertificate=true;uid=sa;pwd=Ver7CompleXPW");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_PRC_CI_AS");

        modelBuilder.Entity<DeptTb>(entity =>
        {
            entity.HasKey(e => e.Deptno).HasName("PK__DEPT_TB__E0EB08D74D57FDB1");

            entity.ToTable("DEPT_TB");

            entity.Property(e => e.Deptno).HasColumnName("DEPTNO");
            entity.Property(e => e.Dname)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("DNAME");
            entity.Property(e => e.Loc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("LOC");
        });

        modelBuilder.Entity<EmpTb>(entity =>
        {
            entity.HasKey(e => e.Empno).HasName("PK__EMP_TB__14CCF2EEC1853756");

            entity.ToTable("EMP_TB");

            entity.Property(e => e.Empno).HasColumnName("EMPNO");
            entity.Property(e => e.Deptno).HasColumnName("DEPTNO");
            entity.Property(e => e.Ename)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("ename");
            entity.Property(e => e.Hiredate)
                .HasColumnType("date")
                .HasColumnName("HIREDATE");
            entity.Property(e => e.Job)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("job");

            entity.HasOne(d => d.DeptnoNavigation).WithMany(p => p.EmpTbs)
                .HasForeignKey(d => d.Deptno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EMP_TB__DEPTNO__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}