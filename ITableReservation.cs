using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3
{
    internal interface ITableReservation
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public Table table { get; set; }
        public int NumberOfGuests { get; set; }

        public static List<string> tableReservationProperties = new List<string>();
        public static List<TableReservation> reservationList = new List<TableReservation>();

    }
}
