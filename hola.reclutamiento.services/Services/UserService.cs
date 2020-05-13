using ho1a.Api.HttpClient;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Light.GuardClauses;
using Newtonsoft.Json;
using System.DirectoryServices;

namespace ho1a.reclutamiento.services.Services
{
    public class UserService : IUserService
    {
        private readonly IGlobalConfiguration<Configuracion> configuration;

        public UserService(IGlobalConfiguration<Configuracion> configuration)
        {
            this.configuration = configuration;
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

        public async Task<User> GetUserByIdColaboradorAsync(int idColaborador)
        {
            var globalConfiguration = this.configuration;
            if (globalConfiguration == null)
            {
                return null;
            }

            var requestUri = globalConfiguration.Configuration<string>("IdentityAPI");
            var url = $"{requestUri}/api/Identity/GetByIdColaborador/{idColaborador}";

            var response = await HttpRequestFactory.GetAsync(url)
                                     .ConfigureAwait(false);
            var strResult = await response.Content.ReadAsStringAsync()
                                      .ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<User>(strResult);

            return result;  
        }

        public async Task<User> GetUserByUserNameAsync(string userName, bool? detalle = null)
        {
            userName.MustNotBeNull(() => new Exception("userName requerido"));

            var globalConfiguration = this.configuration;
            if (globalConfiguration == null)
            {
                return null;
            }

            var requestUri = globalConfiguration.Configuration<string>("IdentityAPI");
            var url = $"{requestUri}/api/Identity?username={userName}";
            if (detalle != null && detalle.Value)
            {
                url += "&includeDetails=true";
            }

            var response = await HttpRequestFactory.GetAsync(url)
                                     .ConfigureAwait(false);
            var strResult = await response.Content.ReadAsStringAsync()
                                      .ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<User>(strResult);

            return result;
        }

        public async Task<List<User>> GetUserNamesByUserNameAsync(string userName)
        {
            userName.MustNotBeNull(() => new Exception("userName requerido"));

            var urlApi = this.configuration.Configuration<string>("IdentityAPI");

            var url = $"{urlApi}/api/Identity?username={userName}&includeDetails=true&recortado=true";

            var response = await HttpRequestFactory.GetAsync(url)
                                     .ConfigureAwait(false);
            var strResult = await response.Content.ReadAsStringAsync()
                                      .ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<List<User>>(strResult);

            return result;
        }

        public bool IsAdmin(string userName)
        {
            var administradoresConfiguration = this.configuration.Configuration<string>("AdministradorAplicacion");

            if (string.IsNullOrEmpty(administradoresConfiguration))
            {
                return false;
            }
            var administradores = administradoresConfiguration.Split(";")
                .ToList();

            return administradores.Any() && administradores.Contains(userName);
        }
    }
}