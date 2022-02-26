using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace suntvaccinat.Models
{
    public class EventModel
    {
        [PrimaryKey, AutoIncrement]
        public int id_event { get; set; }

        public string Name { get; set; }

        public bool IsNoEnded { get; set; }

        public DateTime Date { get; set; }
    }
}
