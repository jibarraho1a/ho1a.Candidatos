using ho1a.Api.HttpClient;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly ICandidatoService candidatoService;
        private readonly IGlobalConfiguration<Configuracion> configuracion;
        private readonly IHttpContextAccessor context;
        private readonly IAsyncRepository<Entrevista> entrevistaRepository;
        private readonly IAsyncRepository<Requisicion> requisicionRepository;
        private readonly IAsyncRepository<Suplantar> suplantarRepository;
        private readonly IAsyncRepository<TernaCandidato> ternaCandidatoRepository;
        private readonly UserManager<CandidatoUser> userManager;
        private readonly IAsyncRepository<UsuarioReasignacion> usuarioReasignadoRepository;

        private const string RolAdministrador = "Administrador";
        private const string RolAdministradorExpediente = "Administrador Expediente";

        public UserResolverService(
            UserManager<CandidatoUser> userManager,
            IHttpContextAccessor context,
            IGlobalConfiguration<Configuracion> configuracion,
            ICandidatoService candidatoService,
            IAsyncRepository<Requisicion> requisicionRepository,
            IAsyncRepository<Entrevista> entrevistaRepository,
            IAsyncRepository<TernaCandidato> ternaCandidatoRepository,
            IAsyncRepository<UsuarioReasignacion> usuarioReasignadoRepository,
            IAsyncRepository<Suplantar> suplantarRepository)
        {
            this.userManager = userManager;
            this.context = context;
            this.configuracion = configuracion;
            this.candidatoService = candidatoService;
            this.requisicionRepository = requisicionRepository;
            this.entrevistaRepository = entrevistaRepository;
            this.ternaCandidatoRepository = ternaCandidatoRepository;
            this.usuarioReasignadoRepository = usuarioReasignadoRepository;
            this.suplantarRepository = suplantarRepository;
        }

        public bool ValidateUser(Credenciales cred)
        {
            string domainAndUsername = $"{cred.Compania}\\{cred.Username}";
            DirectoryEntry entry = new DirectoryEntry("LDAP://NHELIOS", domainAndUsername, cred.Password);

            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = $"(SAMAccountName={cred.Username})";
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("displayName");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();

            return (result != null) ? true : false;
        }

        public async Task<User> GetUserODSAsync(string userNameOrigin)
        {
            try
            {
                //var identity = this.User?.Identity;
                //var userNameOrigin = identity?.Name?.Replace(@"NHELIOS\", string.Empty);

                var userName = this.configuracion?.Configuration<string>("UserSuplantado");

                if (string.IsNullOrEmpty(userName))
                {
                    var userSuplantado = await this.suplantarRepository.Single(
                                             new SuplantarSpecification(userNameOrigin)).ConfigureAwait(false);

                    userName = userSuplantado?.UserProfile;
                }

                userName = userName ?? userNameOrigin;

                var urlApi = this.configuracion?.Configuration<string>("IdentityAPI");

                var response = await HttpRequestFactory.GetAsync($"{urlApi}/api" + $"/Identity?username={userName}")
                                   .ConfigureAwait(false);
                var strResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var user = JsonConvert.DeserializeObject<User>(strResult);

                var candidatoUser = new CandidatoUserSpecification(user.UserName);
                var candidato = this.candidatoService.Single(candidatoUser);

                var usersEspaciales = this.configuracion.Configuration<string>("UserEspeciales").Split(';')
                    .Select(u => u.ToUpper()).ToList();

                user.ShowNew = (int)user.NivelCompetencia >= 4 || usersEspaciales.Contains(user.UserName.ToUpper());

                user.ShowSearch = user.ShowNew || await this.IsAutorizador(user.UserName).ConfigureAwait(false)
                                               || await this.HasEntrevistas(user.UserName).ConfigureAwait(false)
                                               || this.IsAdminExpediente(user.UserName)
                                               || await this.IsAdministrador(user.UserName)
                                               || this.IsAdministradorRh(user.UserName)
                                               || await this.IsReclutador(user.UserName).ConfigureAwait(false)
                                               || this.IsCoordinadorRs(user.UserName)
                                               || this.IsCompensaciones(user.UserName)
                                               || this.IsPlaneacionEstrategica(user.UserName);

                user.ShowCatalog = await this.IsAdministradorExpediente(user.UserName)
                                   || await this.IsAdministrador(user.UserName);

                if (candidato == null)
                {
                    return user;
                }

                user.CandidatoUser = candidato.CandidatoUser;
                user.ShowExpediente = candidato.CandidatoDetalle.StatusSeleccion == EEstadoCandidato.Colaborador;
                user.CandidatoUser.Candidato = null;

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<bool> HasEntrevistas(string userName)
        {
            var entrevistadorSpecificaciones = new EntrevistaSpecification(userName);
            var entrevistasAsignadas = await this.entrevistaRepository.ListAsync(entrevistadorSpecificaciones)
                                           .ConfigureAwait(false);

            return entrevistasAsignadas.Any();
        }

        public bool IsAdminExpediente(string userName)
        {
            var strAdministradores = this.configuracion.Configuration<string>("UsersAdministrador");

            var listAdministrador = strAdministradores.ToUpper().Split(';').ToList();
            return listAdministrador.Any(a => a == userName.ToUpper());
        }

        public bool IsPlaneacionEstrategica(string userName)
        {
            var strAdministradores = this.configuracion.Configuration<string>("UserPlaneacionEstrategica");

            var listAdministrador = strAdministradores.ToUpper().Split(';').ToList();
            return listAdministrador.Any(a => a == userName.ToUpper());
        }

        public async Task<bool> IsAdministrador(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName)
                           .ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }

            var rolesUser = await this.userManager.GetRolesAsync(user)
                                .ConfigureAwait(false);

            return rolesUser.Contains(RolAdministrador);
        }

        public async Task<bool> IsAdministradorExpediente(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName)
                           .ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }

            var rolesUser = await this.userManager.GetRolesAsync(user)
                                .ConfigureAwait(false);

            return rolesUser.Contains(RolAdministradorExpediente);
        }

        public bool IsAdministradorRh(string userName)
        {
            var strAdministradores = this.configuracion.Configuration<string>("UsersAdministradorExpediente");

            var listAdministrador = strAdministradores.ToUpper().Split(';').ToList();
            return listAdministrador.Any(a => a == userName.ToUpper());
        }

        public async Task<bool> IsAutorizador(string userName)
        {
            var validadorSpecificaciones = new RequisicionByValidadorSpecifiation(userName);
            var requisicionesComoValidador = await this.requisicionRepository.ListAsync(validadorSpecificaciones);

            return EnumerableExtensions.Any(requisicionesComoValidador);
        }

        public bool IsCoordinadorRs(string userName)
        {
            var strAdministradores = this.configuracion.Configuration<string>("UserCoordinadorRS");

            var listAdministrador = strAdministradores.ToUpper().Split(';').ToList();
            return listAdministrador.Any(a => a == userName.ToUpper());
        }

        public bool IsCompensaciones(string userName)
        {
            var strAdministradores = this.configuracion.Configuration<string>("UsersCompensaciones");

            var listAdministrador = strAdministradores.ToUpper().Split(';').ToList();
            return listAdministrador.Any(a => a == userName.ToUpper());
        }

        public async Task<bool> IsReclutador(string userName)
        {
            var usuariosReclutadores = await this.usuarioReasignadoRepository.ListAllAsync();

            return usuariosReclutadores.Any(r => r.UserName.ToUpper() == userName.ToUpper());
        }
    }
}