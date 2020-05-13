using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Notificacion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Services.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Componente = ho1a.reclutamiento.models.Seguridad.Componente;

namespace ho1a.reclutamiento.services.Data
{
    public class ApplicationDbContext : IdentityDbContext<CandidatoUser, RolUser, string>
    {
        public ApplicationDbContext(
            DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Accion> Acciones { get; set; }

        public DbSet<Candidato> Candidatos { get; set; }

        public DbSet<Componente> Componentes { get; set; }

        public DbSet<Configuracion> Configuraciones { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Expediente> Expedientes { get; set; }

        public DbSet<Localidad> Localidades { get; set; }

        public DbSet<Mercado> Mercados { get; set; }

        public DbSet<MotivoIngreso> MotivoIngresos { get; set; }

        public DbSet<NotificacionCorreos> NotificacionesCorreos { get; set; }

        public DbSet<ComponentePermisos> PermisosVistas { get; set; }

        public DbSet<PuestoSolicitado> PuestoSolicitados { get; set; }

        public DbSet<Salario> RangosSalarios { get; set; }

        public DbSet<Reasignacion> Reasignaciones { get; set; }

        public DbSet<ReferenciaVacante> ReferenciaVacante { get; set; }

        public DbSet<Requisicion> Requisiciones { get; set; }

        public DbSet<RequisicionDetalle> RequisicionesCandidatos { get; set; }

        public DbSet<RolUser> RolUsers { get; set; }

        public DbSet<TipoPlaza> TipoPlazas { get; set; }

        public DbSet<UsuarioReasignacion> UsuariosReasignacion { get; set; }

        public DbSet<Validacion> Validaciones { get; set; }

        public DbSet<ValidaRequisicion> ValidaRequisiciones { get; set; }

        public DbSet<Vista> Vistas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            base.OnModelCreating(builder);

            // dbo
            builder.Entity<FileUpload>(this.ConfigureArchivo);

            // Configuracion
            builder.Entity<Configuracion>(this.ConfigureConfiguracion);
            builder.Entity<Reasignacion>(this.ConfigureReasignacion);

            // Seguridad
            builder.Entity<CandidatoUser>(this.ConfigureCandidatoUser);
            builder.Entity<RolUser>(this.ConfigureRolUser);
            builder.Entity<Componente>(this.ConfigureComponente);
            builder.Entity<ComponentePermisos>(this.ConfigurePermisoVista);
            builder.Entity<Validacion>(this.ConfigureValidacion);
            builder.Entity<Vista>(this.ConfigureVista);
            builder.Entity<Accion>(this.ConfigureAccion);
            builder.Entity<AccionPermisos>(this.ConfigureAccionPermisos);
            builder.Entity<TipoAccion>(this.ConfigureTipoAccion);
            builder.Entity<Suplantar>(this.ConfigureSuplantar);

            // Catálogos
            builder.Entity<EstadoCivil>(this.ConfigureEstadoCivil);
            builder.Entity<Departamento>(this.ConfigureDepartamento);
            builder.Entity<Empresa>(this.ConfigureEmpresa);
            builder.Entity<Localidad>(this.ConfigureLocalidad);
            builder.Entity<Mercado>(this.ConfigureMercado);
            builder.Entity<MotivoIngreso>(this.ConfigureMotivoIngreso);
            builder.Entity<PuestoSolicitado>(this.ConfigurePuestoSolicitado);
            builder.Entity<TipoPlaza>(this.ConfigureTipoPlaza);
            builder.Entity<Direccion>(this.ConfigureDireccion);
            builder.Entity<Salario>(this.ConfigureSalario);
            builder.Entity<UsuarioReasignacion>(this.ConfigureUsuarioReasignacion);
            builder.Entity<ReferenciaVacante>(this.ConfigureUsuarioReferenciaVacante);
            builder.Entity<UltimoTrabajo>(this.ConfigureUltimoTrabajo);
            builder.Entity<Ingreso>(this.ConfigureIngreso);
            builder.Entity<Prestacion>(this.ConfigurePrestacion);
            builder.Entity<TabuladorSalario>(this.TabuladorSalario);
            builder.Entity<PuestoTabulador>(this.PuestoTabulador);

            // Plazas
            builder.Entity<Requisicion>(this.ConfigureRequisicion);
            builder.Entity<ValidaRequisicion>(this.ConfigureValidaciones);
            builder.Entity<Ternas>(this.ConfigureTernas);
            builder.Entity<TernaCandidato>(this.ConfigureCandidatoTerna);
            builder.Entity<Entrevista>(this.ConfigureEntrevista);
            builder.Entity<Competencia>(this.ConfigureCompetencia);
            builder.Entity<EntrevistaResumen>(this.ConfigureEntrevistaResumen);
            builder.Entity<RequisicionDetalle>(this.ConfigureRequisicionCandidato);
            builder.Entity<RequisicionArchivo>(this.ConfigureRequisicionArchivo);
            builder.Entity<PlantillaEntrevista>(this.ConfigurePlantillaEntrevista);
            builder.Entity<RequisicionPropuesta>(this.ConfigureRequisicionPropuesta);

            // Candidatos
            builder.Entity<Candidato>(this.ConfigureCandidato);
            builder.Entity<CandidatoDetalle>(this.ConfigureCandidatoDetalle);
            builder.Entity<Expediente>(this.ConfigureExpediente);
            builder.Entity<ReferenciaLaboral>(this.ConfigureReferenciaLaboral);
            builder.Entity<ReferenciaPersonal>(this.ConfigureReferenciaPersonal);
            builder.Entity<CandidatoExpediente>(this.ConfigureCandidatoExpediente);
            builder.Entity<ExpedienteArchivo>(this.ConfigureExpedienteFileUpload);

            // Notificaciones
            builder.Entity<NotificacionCorreos>(this.ConfigureNotificacionCorreos);
        }

        private void PuestoTabulador(EntityTypeBuilder<PuestoTabulador> builder)
        {
            builder.ToTable("PuestoTabulador", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureRolUser(EntityTypeBuilder<RolUser> builder)
        {
            builder.ToTable("RolUser", "Seguridad");
        }

        private void ConfigureAccion(EntityTypeBuilder<Accion> builder)
        {
            builder.ToTable("Accion", "Seguridad");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasOne(s => s.AccionPermisos)
                .WithOne(s => s.Accion)
                .HasForeignKey<AccionPermisos>(s => s.AccionId);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureAccionPermisos(EntityTypeBuilder<AccionPermisos> builder)
        {
            builder.ToTable("AccionPermisos", "Seguridad");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasOne(s => s.Accion)
                .WithOne(s => s.AccionPermisos)
                .HasForeignKey<Accion>(s => s.AccionPermisosId);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureArchivo(EntityTypeBuilder<FileUpload> builder)
        {
            builder.ToTable("Archivos", "Plazas");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureCandidato(EntityTypeBuilder<Candidato> builder)
        {
            if (builder != null)
            {
                builder.ToTable("Candidato", "Candidato");
                builder.Property(propiedad => propiedad.Created)
                    .HasDefaultValueSql("GETDATE()");
                builder.Property(propiedad => propiedad.Active)
                    .HasDefaultValue(true);

                builder.HasOne(s => s.CandidatoDetalle)
                    .WithOne(s => s.Candidato)
                    .HasForeignKey<CandidatoDetalle>(s => s.CandidatoId);

                builder.HasQueryFilter(p => p.Active);
            }
        }

        private void ConfigureCandidatoDetalle(EntityTypeBuilder<CandidatoDetalle> builder)
        {
            builder.ToTable("CandidatoDetalle", "Candidato");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasOne(s => s.Direccion)
                .WithOne(s => s.CandidatoDetalle)
                .HasForeignKey<Direccion>(s => s.CandidatoDetalleId);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureCandidatoExpediente(EntityTypeBuilder<CandidatoExpediente> builder)
        {
            builder.ToTable("CandidatoExpediente", "Candidato");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureCandidatoTerna(EntityTypeBuilder<TernaCandidato> builder)
        {
            builder.ToTable("CandidatoTerna", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureCandidatoUser(EntityTypeBuilder<CandidatoUser> builder)
        {
            builder.ToTable("CandidatoUser", "Seguridad");
        }

        private void ConfigureCompetencia(EntityTypeBuilder<Competencia> builder)
        {
            builder.ToTable("Competencia", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureComponente(EntityTypeBuilder<Componente> builder)
        {
            builder.ToTable("Componentes", "Seguridad");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasOne(p => p.Vista)
                .WithMany(p => p.Componentes)
                .HasForeignKey(p => p.VistaId);

            builder.HasIndex("ComponentePadreId", "VistaId", "Nombre")
                .IsUnique(true)
                .HasFilter("Nombre is not null");

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureConfiguracion(EntityTypeBuilder<Configuracion> builder)
        {
            builder.ToTable("Configuracion", "Configuracion");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureDepartamento(EntityTypeBuilder<Departamento> builder)
        {
            builder.ToTable("Departamentos", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureDireccion(EntityTypeBuilder<Direccion> builder)
        {
            builder.ToTable("Direccion", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureEmpresa(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresas", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureEntrevista(EntityTypeBuilder<Entrevista> builder)
        {
            builder.ToTable("Entrevista", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureEntrevistaResumen(EntityTypeBuilder<EntrevistaResumen> builder)
        {
            builder.ToTable("EntrevistaResumen", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);

        }

        private void ConfigureEstadoCivil(EntityTypeBuilder<EstadoCivil> builder)
        {
            builder.ToTable("EstadoCivil", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureExpediente(EntityTypeBuilder<Expediente> builder)
        {
            builder.ToTable("Expediente", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);

        }

        private void ConfigureExpedienteFileUpload(EntityTypeBuilder<ExpedienteArchivo> builder)
        {
            builder.ToTable("ExpedienteArchivo", "Candidato");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureIngreso(EntityTypeBuilder<Ingreso> builder)
        {
            builder.ToTable("Ingreso", "Candidato");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureLocalidad(EntityTypeBuilder<Localidad> builder)
        {
            builder.ToTable("Localidades", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureMercado(EntityTypeBuilder<Mercado> builder)
        {
            builder.ToTable("Mercados", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureMotivoIngreso(EntityTypeBuilder<MotivoIngreso> builder)
        {
            builder.ToTable("MotivosIngresos", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureNotificacionCorreos(EntityTypeBuilder<NotificacionCorreos> builder)
        {
            builder.ToTable("ConfiguracionCorreos", "Notificaciones");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigurePermisoVista(EntityTypeBuilder<ComponentePermisos> builder)
        {
            builder.ToTable("ComponentePermisos", "Seguridad");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasOne(p => p.Componente)
                .WithOne(p => p.Permiso)
                .HasForeignKey<ComponentePermisos>(s => s.ComponenteId);

            builder.HasIndex("ComponenteId", "RolId", "Visible", "Edicion")
                .IsUnique(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigurePlantillaEntrevista(EntityTypeBuilder<PlantillaEntrevista> builder)
        {
            builder.ToTable("PlantillaEntrevista", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigurePrestacion(EntityTypeBuilder<Prestacion> builder)
        {
            builder.ToTable("Prestacion", "Candidato");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigurePuestoSolicitado(EntityTypeBuilder<PuestoSolicitado> builder)
        {
            builder.ToTable("PuestosSolicitados", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);
        }

        private void ConfigureReasignacion(EntityTypeBuilder<Reasignacion> builder)
        {
            builder.ToTable("Reasignaciones", "Configuracion");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureReferenciaLaboral(EntityTypeBuilder<ReferenciaLaboral> builder)
        {
            builder.ToTable("ReferenciaLaboral", "Candidato");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureReferenciaPersonal(EntityTypeBuilder<ReferenciaPersonal> builder)
        {
            builder.ToTable("ReferenciaPersonal", "Candidato");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureRequisicion(EntityTypeBuilder<Requisicion> builder)
        {
            builder.ToTable("Requisiciones", "Plazas");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureRequisicionArchivo(EntityTypeBuilder<RequisicionArchivo> builder)
        {
            builder.ToTable("RequisicionArchivo", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureRequisicionCandidato(EntityTypeBuilder<RequisicionDetalle> builder)
        {
            builder.ToTable("RequisicionDetalle", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureRequisicionPropuesta(EntityTypeBuilder<RequisicionPropuesta> builder)
        {
            builder.ToTable("RequisicionPropuesta", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureSalario(EntityTypeBuilder<Salario> builder)
        {
            builder.ToTable("RangoSalarios", "Catalogos");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureSuplantar(EntityTypeBuilder<Suplantar> builder)
        {
            builder.ToTable("Suplantar", "Seguridad");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureTernas(EntityTypeBuilder<Ternas> builder)
        {
            builder.ToTable("Ternas", "Plazas");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureTipoAccion(EntityTypeBuilder<TipoAccion> builder)
        {
            builder.ToTable("TipoAccion", "Seguridad");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureTipoPlaza(EntityTypeBuilder<TipoPlaza> builder)
        {
            builder.ToTable("TiposPlazas", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureUltimoTrabajo(EntityTypeBuilder<UltimoTrabajo> builder)
        {
            builder.ToTable("UltimoTrabajo", "Candidato");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureUsuarioReasignacion(EntityTypeBuilder<UsuarioReasignacion> builder)
        {
            builder.ToTable("UsuariosReasignacion", "Catalogos");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureUsuarioReferenciaVacante(EntityTypeBuilder<ReferenciaVacante> builder)
        {
            builder.ToTable("ReferenciaVacante", "Catalogos");

            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureValidacion(EntityTypeBuilder<Validacion> builder)
        {
            builder.ToTable("Validacion", "Seguridad");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureValidaciones(EntityTypeBuilder<ValidaRequisicion> builder)
        {
            builder.ToTable("Validaciones", "Plazas");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);
            builder.Ignore(propiedad => propiedad.UserValidador);

            builder.HasQueryFilter(p => p.Active);
        }

        private void ConfigureVista(EntityTypeBuilder<Vista> builder)
        {
            builder.ToTable("Vistas", "Seguridad");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasMany(p => p.Componentes)
                .WithOne(p => p.Vista)
                .HasForeignKey(p => p.VistaId);

            builder.HasQueryFilter(p => p.Active);
        }

        private void TabuladorSalario(EntityTypeBuilder<TabuladorSalario> builder)
        {
            builder.ToTable("TabuladorSalario", "Catalogo");
            builder.Property(propiedad => propiedad.Created)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(propiedad => propiedad.Active)
                .HasDefaultValue(true);

            builder.HasQueryFilter(p => p.Active);
        }
    }
}