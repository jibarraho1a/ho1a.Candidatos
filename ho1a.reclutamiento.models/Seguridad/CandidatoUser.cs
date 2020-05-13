using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class CandidatoUser : IdentityUser
    {
        public Candidato Candidato { get; set; }
        public int? CandidatoId { get; set; }
        public string LinkedInId { get; set; }
        public int? NoColaborador { get; set; }
        public string PictureLinkedInUrl { get; set; }
        [NotMapped]
        public User User { get; set; }
        public string UserAd { get; set; }
    }
}