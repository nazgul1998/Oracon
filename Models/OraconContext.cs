using Microsoft.EntityFrameworkCore;
using Oracon.Models.Entidades;
using Oracon.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Models
{
    public class OraconContext:DbContext
    {
        public OraconContext()
        {
        }

        public OraconContext(DbContextOptions<OraconContext> options)
        : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Enlace> Enlaces { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Entrega> Entregas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ProfesorMap());
            modelBuilder.ApplyConfiguration(new CursoMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new EtiquetaMap());
            modelBuilder.ApplyConfiguration(new MatriculaMap());
            modelBuilder.ApplyConfiguration(new ModuloMap());
            modelBuilder.ApplyConfiguration(new VideoMap());
            modelBuilder.ApplyConfiguration(new DocumentoMap());
            modelBuilder.ApplyConfiguration(new EnlaceMap());
            modelBuilder.ApplyConfiguration(new TareaMap());
            modelBuilder.ApplyConfiguration(new PagoMap());
            modelBuilder.ApplyConfiguration(new AdminMap());
            modelBuilder.ApplyConfiguration(new EntregaMap());
        }
    }
}
