using SQLite;

namespace suntvaccinat.Models
{
    public class ParticipantModel
    {
        [PrimaryKey, AutoIncrement]
        public int id_part { get; set; }

        public int id_event { get; set; }

        public string Name { get; set; }
    }
}
