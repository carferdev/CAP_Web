using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Polimerida_CAP.Services.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Configuraciones> Configuraciones { get; set; }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<Dispositivo> Dispositivo { get; set; }

    public virtual DbSet<Empleado> Empleado { get; set; }

    public virtual DbSet<Festivo> Festivo { get; set; }

    public virtual DbSet<Grupo> Grupo { get; set; }

    public virtual DbSet<Incidencias> Incidencias { get; set; }

    public virtual DbSet<Incidenciaxempleado> Incidenciaxempleado { get; set; }

    public virtual DbSet<Puesto> Puesto { get; set; }

    public virtual DbSet<Punches> Punches { get; set; }

    public virtual DbSet<Subgrupo> Subgrupo { get; set; }

    public virtual DbSet<Turno> Turno { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Configuraciones>(entity =>
        {
            entity.HasKey(e => e.Idconfiguraciones).HasName("PRIMARY");

            entity.ToTable("configuraciones");

            entity.Property(e => e.Idconfiguraciones)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idconfiguraciones");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.Valor)
                .HasMaxLength(45)
                .HasDefaultValueSql("''")
                .HasColumnName("valor");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Iddepartamento).HasName("PRIMARY");

            entity.ToTable("departamento");

            entity.Property(e => e.Iddepartamento)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("iddepartamento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("reg_status");
        });

        modelBuilder.Entity<Dispositivo>(entity =>
        {
            entity.HasKey(e => e.Iddispositivo).HasName("PRIMARY");

            entity.ToTable("dispositivo");

            entity.Property(e => e.Iddispositivo)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("iddispositivo");
            entity.Property(e => e.Clase)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("clase");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.Division)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("division");
            entity.Property(e => e.Ip)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("ip");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("reg_status");
            entity.Property(e => e.Tipo)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Idempleado).HasName("PRIMARY");

            entity.ToTable("empleado");

            entity.HasIndex(e => e.Idsubgrupo, "empleado_subgrupo_idx");

            entity.Property(e => e.Idempleado)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idempleado");
            entity.Property(e => e.Apellidomaterno)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("apellidomaterno");
            entity.Property(e => e.Apellidopaterno)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("apellidopaterno");
            entity.Property(e => e.Credencial)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("credencial");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Fechabaja)
                .HasDefaultValueSql("'2001-01-01 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("fechabaja");
            entity.Property(e => e.Fechaingreso)
                .HasDefaultValueSql("'2001-01-01 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("fechaingreso");
            entity.Property(e => e.Iddepartamento)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("iddepartamento");
            entity.Property(e => e.Iddispositivo)
                .HasColumnType("int(11)")
                .HasColumnName("iddispositivo");
            entity.Property(e => e.Idpuesto)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idpuesto");
            entity.Property(e => e.Idsubgrupo)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("idsubgrupo");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Nombreimagen)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasColumnName("nombreimagen");
            entity.Property(e => e.Primernombre)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("primernombre");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("reg_status");
            entity.Property(e => e.Segundonombre)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("segundonombre");
            entity.Property(e => e.Sexo)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("sexo");
           

            entity.HasOne(d => d.IdsubgrupoNavigation).WithMany(p => p.Empleado)
                .HasForeignKey(d => d.Idsubgrupo)
                .HasConstraintName("empleado_subgrupo");

            entity.HasOne(d => d.IddepartamentoNavigation).WithMany()
                .HasForeignKey(d => d.Iddepartamento)
                .HasConstraintName("empleado_departamento");

            entity.HasOne(d => d.IdpuestoNavigation).WithMany()
                .HasForeignKey(d => d.Idpuesto)
                .HasConstraintName("empleado_puesto");

            entity.HasOne(d => d.IddispositivoNavigation).WithMany()
                .HasForeignKey(d => d.Iddispositivo)
                .HasConstraintName("empleado_dispositivo");
        });

        modelBuilder.Entity<Festivo>(entity =>
        {
            entity.HasKey(e => e.Idfestivo).HasName("PRIMARY");

            entity.ToTable("festivo");

            entity.Property(e => e.Idfestivo)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idfestivo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(2)
                .HasDefaultValueSql("'A'")
                .HasColumnName("reg_status");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Idgrupo).HasName("PRIMARY");

            entity.ToTable("grupo");

            entity.Property(e => e.Idgrupo)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idgrupo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("nombre");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("reg_status");
        });

        modelBuilder.Entity<Incidencias>(entity =>
        {
            entity.HasKey(e => e.Idincidencia).HasName("PRIMARY");

            entity.ToTable("incidencias");

            entity.Property(e => e.Idincidencia)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idincidencia");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(2)
                .HasDefaultValueSql("'A'")
                .HasColumnName("reg_status");
        });

        modelBuilder.Entity<Incidenciaxempleado>(entity =>
        {
            entity.HasKey(e => e.Idincidenciaxempleado).HasName("PRIMARY");

            entity.ToTable("incidenciaxempleado");

            entity.Property(e => e.Idincidenciaxempleado)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idincidenciaxempleado");
            entity.Property(e => e.Fechafin)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("fechafin");
            entity.Property(e => e.Fechainicio)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("fechainicio");
            entity.Property(e => e.Idempleado)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idempleado");
            entity.Property(e => e.Idincidencia)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idincidencia");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(5)
                .HasDefaultValueSql("'A'")
                .HasColumnName("reg_status");

            entity.HasOne(d => d.IdempleadoNavigation).WithMany()
                .HasForeignKey(d => d.Idempleado)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdincidenciaNavigation).WithMany()
                .HasForeignKey(d => d.Idincidencia)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Puesto>(entity =>
        {
            entity.HasKey(e => e.Idpuesto).HasName("PRIMARY");

            entity.ToTable("puesto");

            entity.Property(e => e.Idpuesto)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idpuesto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .HasDefaultValueSql("''")
                .HasColumnName("descripcion");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("reg_status");
        });

        modelBuilder.Entity<Punches>(entity =>
        {
            entity.HasKey(e => e.Idpunches).HasName("PRIMARY");

            entity.ToTable("punches");

            entity.HasIndex(e => e.Softkey, "softkey");

            entity.Property(e => e.Idpunches)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idpunches");
            entity.Property(e => e.Fechapunch)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("fechapunch");
            entity.Property(e => e.Iddispositivo)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("iddispositivo");
            entity.Property(e => e.Idempleado)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idempleado");
            entity.Property(e => e.Idgrupo)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idgrupo");
            entity.Property(e => e.Idturno)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idturno");
            entity.Property(e => e.Nombredispositivo)
                .HasMaxLength(45)
                .HasDefaultValueSql("''")
                .HasColumnName("nombredispositivo");
            entity.Property(e => e.Penalizacion)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("penalizacion");
            entity.Property(e => e.Regtimestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("regtimestamp");
            entity.Property(e => e.Softkey)
                .HasMaxLength(2)
                .HasDefaultValueSql("'IN'")
                .IsFixedLength()
                .HasColumnName("softkey");
        });

        modelBuilder.Entity<Subgrupo>(entity =>
        {
            entity.HasKey(e => e.Idsubgrupo).HasName("PRIMARY");

            entity.ToTable("subgrupo");

            entity.HasIndex(e => e.Idsubgrupo, "idsubgrupo_UNIQUE").IsUnique();

            entity.Property(e => e.Idsubgrupo)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("idsubgrupo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
            entity.Property(e => e.Idgrupo)
                .HasColumnType("int(11)")
                .HasColumnName("idgrupo");
            entity.Property(e => e.Idturno)
                .HasColumnType("int(11)")
                .HasColumnName("idturno");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'")
                .IsFixedLength()
                .HasColumnName("reg_status");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.Idturno).HasName("PRIMARY");

            entity.ToTable("turno");

            entity.Property(e => e.Idturno)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idturno");
            entity.Property(e => e.Entradacomida)
                .HasColumnType("datetime")
                .HasColumnName("entradacomida");
            entity.Property(e => e.Horaentrada)
                .HasDefaultValueSql("'2001-01-01 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("horaentrada");
            entity.Property(e => e.Horasalida)
                .HasDefaultValueSql("'2001-01-01 00:00:00'")
                .HasColumnType("datetime")
                .HasColumnName("horasalida");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasDefaultValueSql("''")
                .HasColumnName("nombre");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(2)
                .HasDefaultValueSql("'A'")
                .HasColumnName("reg_status");
            entity.Property(e => e.Salidacomida)
                .HasColumnType("datetime")
                .HasColumnName("salidacomida");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Idusuario)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("idusuario");
            entity.Property(e => e.Password)
                .HasColumnType("text")
                .HasColumnName("password");
            entity.Property(e => e.RegStatus)
                .HasMaxLength(2)
                .HasDefaultValueSql("'A'")
                .HasColumnName("reg_status");
            entity.Property(e => e.Usuario1)
                .HasColumnType("text")
                .HasColumnName("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
