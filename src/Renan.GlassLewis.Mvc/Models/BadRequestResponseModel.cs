using System.Collections.Generic;

namespace Renan.GlassLewis.Mvc.Models
{
    public class BadRequestResponseModel
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}