using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace suntvaccinat.Models
{
    //0-19, 20-29, 30-39, 40-49, 50-59, 60+ 
    public class StatsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Id_Event { get; set; }
        public int PersonsUnder19 { get; set; }
        public int PersonsBetween2029 { get; set; }
        public int PersonsBetween3039 { get; set; }
        public int PersonsBetween4049 { get; set; }
        public int PersonsBetween5059 { get; set; }
        public int PersonsGreater60 { get; set; }
    }
}
