using System.ComponentModel.DataAnnotations;

namespace Renan.GlassLewis.Service.CompaniesUseCases.Models
{
    public class UpdateCompanyModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Exchange { get; set; }

        [Required]
        public string Isin { get; set; }

        public string WebSite { get; set; }

    }
}