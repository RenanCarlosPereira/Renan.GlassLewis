namespace Renan.GlassLewis.Domain.Company
{
    public class CompanyEntity
    {
        public CompanyEntity(string name, string exchange, CompanyIsin isin, string webSite, int id = default)
        {
            Name = name;
            Exchange = exchange;
            Isin = isin;
            WebSite = webSite;
            Id = id;
        }

        private CompanyEntity()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public CompanyIsin Isin { get; set; }
        public string WebSite { get; set; }
    }
}