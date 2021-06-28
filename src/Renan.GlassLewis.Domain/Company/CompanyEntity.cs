using FluentValidation.Results;

namespace Renan.GlassLewis.Domain.Company
{
    public class CompanyEntity
    {
        public CompanyEntity(string name, string exchange, CompanyIsin isin, string webSite, string ticker)
        {
            Name = name;
            Exchange = exchange;
            Isin = isin;
            WebSite = webSite;
            Ticker = ticker;
        }

        private CompanyEntity()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public CompanyIsin Isin { get; set; }
        public string WebSite { get; set; }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Name))
                result.Errors.Add(new ValidationFailure(nameof(Name), CompanyConstants.NameIsRequired));

            if (string.IsNullOrWhiteSpace(Exchange))
                result.Errors.Add(new ValidationFailure(nameof(Exchange), CompanyConstants.ExchangeIsRequired));

            if (string.IsNullOrWhiteSpace(Ticker))
                result.Errors.Add(new ValidationFailure(nameof(Ticker), CompanyConstants.TickerIsRequired));

            result.Errors.AddRange(Isin.Validate().Errors);

            return result;
        }
    }
}