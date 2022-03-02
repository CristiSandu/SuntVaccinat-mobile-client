using System;
using System.Collections.Generic;
using System.Text;

namespace suntvaccinat.Helpers
{
    public static class DataBaseQuerys
    {
        public static string GetEventsOrderByDate = $"SELECT * FROM EventModel ORDER BY Date";

        public static string GetEventsQuery(int id)
        {
            return $"SELECT * FROM EventModel WHERE id_event = '{id}'";
        }

        public static string GetParticipantsQuery(int id)
        {
            return $"SELECT * FROM ParticipantModel WHERE id_event={id}";
        }

        public static string DeleteEventQuery(int id)
        {
            return $"DELETE FROM ParticipantModel WHERE id_event={id}";
        }
    }
}


// validarea unicitatii -> un telefon -- un certificat /
// un 
// schema :
//  - ma duc la server cer tocken 
//  - 