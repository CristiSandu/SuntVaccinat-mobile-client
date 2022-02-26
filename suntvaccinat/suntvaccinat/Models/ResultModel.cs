namespace suntvaccinat.Models
{
    public class ResultModel
    {
        public string Hash { get; set; }
        public string Format { get; set; }
        public string Semnatar { get; set; }
        public string RawData { get; set; }
        public bool Validator { get; set; }
        public bool Find { get; set; }

        public override string ToString()
        {
            return $"{Hash.Trim(' ').ToUpper()} {Format.Trim(' ').ToUpper()} {Semnatar} {Validator} {Find}";
        }
    }
}
