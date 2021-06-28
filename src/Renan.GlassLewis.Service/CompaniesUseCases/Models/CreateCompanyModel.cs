using System.ComponentModel.DataAnnotations;
using Renan.GlassLewis.Domain.Company;

namespace Renan.GlassLewis.Service.CompaniesUseCases.Models
{
    public class CreateCompanyModel
    {
        [Required(ErrorMessage = CompanyConstants.NameIsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = CompanyConstants.ExchangeIsRequired)]
        public string Exchange { get; set; }

        [Required(ErrorMessage = CompanyConstants.IsinIsRequired)]
        public string Isin { get; set; }

        [Required(ErrorMessage = CompanyConstants.TickerIsRequired)]
        public string Ticker { get; set; }

        public string WebSite { get; set; }
    }
}