using FluentValidation.Results;

namespace Renan.GlassLewis.Domain.Company
{
    public class CompanyIsin
    {
        public CompanyIsin(string value)
        {
            Value = value;
        }

        private CompanyIsin()
        {
        }

        //public int CompanyEntityId { get; set; }
        public string Value { get; set; }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Value))
                result.Errors.Add(new ValidationFailure(nameof(CompanyEntity.Isin), CompanyConstants.CompanyIsinMustHaveValue));

            if (Value?.Length < 2)
                result.Errors.Add(new ValidationFailure(nameof(CompanyEntity.Isin), CompanyConstants.CompanyIsinMustBeGreaterThenTwoChars));

            if (Value?.Length >= 2 && (!char.IsLetter(Value!, 0) || !char.IsLetter(Value!, 1)))
                result.Errors.Add(new ValidationFailure(nameof(CompanyEntity.Isin), CompanyConstants.CompanyIsinMustStartWithTwoLetter));

            return result;
        }
    }
}