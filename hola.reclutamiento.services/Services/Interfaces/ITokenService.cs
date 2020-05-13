using ho1a.reclutamiento.models.Seguridad;
using Microsoft.IdentityModel.Tokens;
using System;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user, DateTime expiry);

        TokenValidationParameters GetValidationParameters();
    }
}
