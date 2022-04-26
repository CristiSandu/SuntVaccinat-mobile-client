using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace suntvaccinat.Models
{
    public class INSPValidationModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pdfData")]
        public string PdfData { get; set; }
    }
}
