namespace Renan.GlassLewis.Domain.Company
{
    public static class CompanyConstants
    {
        public const string CompanyWithSameIsinAlreadyExists = "Company with the same Isin already exists";
        public const string CompanyIsinMustHaveValue = "Value must have a Isin";
        public const string CompanyIsinMustBeGreaterThenTwoChars = "Company Value must be greater then 2 caracteres";
        public const string CompanyIsinMustStartWithTwoLetter = "Isin must start with 2 letters";
        public const string CompanyIdMustExist = "Company Id Must Exist";
        public const string NameIsRequired = "Name is Required";
        public const string ExchangeIsRequired = "Exchange is Required";
        public const string TickerIsRequired = "Ticker is Required";
        public const string IsinIsRequired = "Isin is Required";
    }
}