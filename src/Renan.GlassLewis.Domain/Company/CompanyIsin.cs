using FluentValidation.Results;

namespace Renan.GlassLewis.Domain.Company
{
    public class CompanyIsin
    {
        public CompanyIsin(string isin)
        {
            Isin = isin;
        }

        private CompanyIsin()
        {
        }

        public int CompanyEntityId { get; set; }
        public string Isin { get; set; }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Isin))
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyIsinMustHaveValue));

            if (Isin?.Length < 2)
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyIsinMustBeGreaterThenTwoChars));

            if (Isin?.Length > 2 && (!char.IsLetter(Isin!, 0) || !char.IsLetter(Isin!, 1)))
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyIsinMustStartWithTwoLetter));

            return result;
        }
    }
}