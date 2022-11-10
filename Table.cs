using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3
{
    internal class Table: ITable
    {
       public int Number { get; }
       public int NumberOfReservedSeats { get; }

        public Table(int number, int numberOfGuests)
        {
            Number = number; 
            NumberOfReservedSeats = numberOfGuests;
        }
    }
}
