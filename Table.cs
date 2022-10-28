using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3
{
    internal class Table
    {
       public int Number { get; set; }
        public int NumberOfReservedSeats { get; set; }
       //public int NumberOfFreeSeats { get; set; }

        public Table(int number, int numberOfGuests)
        {
            Number = number;
           
            this.NumberOfReservedSeats = numberOfGuests;

        }
    }
}
