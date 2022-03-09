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

        public override string ToString()
        {
            Regex pattern = new Regex("_| ");
            string rez = pattern.Replace(SecondName, "-");
            return $"{Name.Trim(' ').ToUpper()} {rez.Trim(' ').ToUpper()} {Sex[0]} {Age.Trim(' ')}={PhoneNumber}";
        }
    }
}
