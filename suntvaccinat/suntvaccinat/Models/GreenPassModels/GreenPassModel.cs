using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace suntvaccinat.Models.GreenPassModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Content
    {
        [JsonProperty("v")]
        public List<VaccinationEntry> Vaccines { get; set; }

        [JsonProperty("r")]
        public List<RecoveryEntry> Recoveries { get; set; }

        [JsonProperty("t")]
        public List<TestEntry> Tests { get; set; }

        [JsonProperty("dob")]
        public string DateOfBirth { get; set; }

        [JsonProperty("nam")]
        public Name Name { get; set; }

        [JsonProperty("ver")]
        public string Version { get; set; }
    }

    public class MainBody
    {
        [JsonProperty("1")]
        public Content Content { get; set; }
    }

    public class Name
    {
        [JsonProperty("fn")]
        public string Surname { get; set; }

        [JsonProperty("gn")]
        public string Forename { get; set; }

        [JsonProperty("fnt")]
        public string SurnameCaps { get; set; }

        [JsonProperty("gnt")]
        public string ForenameCaps { get; set; }
    }

    public class RecoveryEntry
    {
        [JsonProperty("ci")]
        public string CertificateIdentifier { get; set; }

        [JsonProperty("co")]
        public string Co { get; set; }

        [JsonProperty("df")]
        public string Df { get; set; }

        [JsonProperty("du")]
        public string ValidUntil { get; set; }

        [JsonProperty("fr")]
        public string Fr { get; set; }

        [JsonProperty("is")]
        public string Is { get; set; }

        [JsonProperty("tg")]
        public string Tg { get; set; }

        public DateTimeOffset ExpirationDate => DateTimeOffset.Parse(ValidUntil);
    }

    public class VaccinationEntry
    {
        [JsonProperty("ci")]
        public string CertificateIdentifier { get; set; }

        [JsonProperty("co")]
        public string Co { get; set; }

        [JsonProperty("dn")]
        public int Dn { get; set; }

        [JsonProperty("dt")]
        public string Dt { get; set; }

        [JsonProperty("is")]
        public string Is { get; set; }

        [JsonProperty("ma")]
        public string Ma { get; set; }

        [JsonProperty("mp")]
        public string Mp { get; set; }

        [JsonProperty("sd")]
        public int Sd { get; set; }

        [JsonProperty("tg")]
        public string Tg { get; set; }

        [JsonProperty("vp")]
        public string Vp { get; set; }

        public DateTimeOffset DateOfVaccination => DateTimeOffset.Parse(Dt);

    }

    public class TestEntry
    {
        [JsonProperty("ci")]
        public string CertificateIdentifier { get; set; }

        [JsonProperty("co")]
        public string Co { get; set; }

        [JsonProperty("is")]
        public string Is { get; set; }

        [JsonProperty("nm")]
        public string Nm { get; set; }

        [JsonProperty("sc")]
        public string Sc { get; set; }

        [JsonProperty("tc")]
        public string Tc { get; set; }

        [JsonProperty("tg")]
        public string Tg { get; set; }

        [JsonProperty("tr")]
        public string Tr { get; set; }

        [JsonProperty("tt")]
        public string Tt { get; set; }

        public DateTimeOffset SampleCollectionDate => DateTimeOffset.Parse(Sc);

    }

    public class GreenPassModel
    {
        [JsonProperty("1")]
        public string IssuingCountry { get; set; }

        [JsonProperty("4")]
        public int ExpiresAt { get; set; }

        [JsonProperty("6")]
        public int IssuedAt { get; set; }

        [JsonProperty("-260")]
        public MainBody Body { get; set; }
    }


}
