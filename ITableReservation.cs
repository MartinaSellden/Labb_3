using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3
{
    internal interface ITableReservation
    {
        public string Name { get; }
        public DateTime Date { get; }
        public string Time { get; }
        public Table table { get; }
        public int NumberOfGuests { get; }

    }
}
