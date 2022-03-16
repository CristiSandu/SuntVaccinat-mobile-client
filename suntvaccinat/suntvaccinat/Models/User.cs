using SQLite;
using System.Text.RegularExpressions;

namespace suntvaccinat.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { set; get; }
        public string SecondName { set; get; }
        public string Sex { set; get; }
        public string Age { set; get; }
        public string PhoneNumber { set; get; }

        [Ignore]
        public string FullName => $"{Name} {SecondName}";

        [Ignore]
        public string Initials => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Name) ? $"{Name[0]}{SecondName[0]}" : string.Empty;

        public override string ToString()
        {
            Regex pattern = new Regex("_| ");
            string rez = pattern.Replace(SecondName, "-");
            return $"{Name.Trim(' ').ToUpper()} {rez.Trim(' ').ToUpper()} {Sex[0]} {Age.Trim(' ')}={PhoneNumber}";
        }
    }
}
