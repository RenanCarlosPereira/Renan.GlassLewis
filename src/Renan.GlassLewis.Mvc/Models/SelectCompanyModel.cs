namespace Renan.GlassLewis.Mvc.Models
{
    public class SelectCompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Exchange { get; set; }

        public string Isin { get; set; }

        public string WebSite { get; set; }

        public string Ticker { get; set; }
    }
}