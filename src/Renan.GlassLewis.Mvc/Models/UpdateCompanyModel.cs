using System.ComponentModel.DataAnnotations;

namespace Renan.GlassLewis.Mvc.Models
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

        public string Ticker { get; set; }
    }
}