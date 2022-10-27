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
       public int numberOfFreeSeats = 5;

        public Table(int number, int numberOfSeats)
        {
            Number = number;
            this.numberOfFreeSeats -= numberOfSeats;

        }
    }
}
