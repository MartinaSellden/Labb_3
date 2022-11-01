using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Threading;

namespace Labb_3
{
    internal class TableReservation
    {
        public string Name { get; set; }
        public string Date { get; set; } //kanske göra till datetime
        public string Time { get; set; }
        public Table table { get; set; }  
        public string DisplayData { get; set; }
        public int NumberOfGuests { get; set; }
        
        //public static List<string> tableReservationProperties = new List<string>();
        public static List<TableReservation> reservationList = new List<TableReservation>();
    
        public TableReservation(string Name, Table table, int numberOfGuests, string Date, string Time)
        {
           
            this.Name = Name;
            this.table = table;
            this.NumberOfGuests = numberOfGuests; 
            this.Date = Date; 
            this.Time = Time;
            this.DisplayData = $"Datum: {Date} Tid: {Time} Bord nr: {table.Number} Antal gäster: {NumberOfGuests}";

        }

        public static void CreateNewReservation(string date, string name, string time, int tableNumber, int numberOfGuests)
        {
            Table newTable = new Table(tableNumber, numberOfGuests);

            TableReservation.reservationList.Add(new TableReservation(name, newTable, numberOfGuests, date, time));

            //TableReservation.tableReservationProperties.Clear();

            //TableReservation.tableReservationProperties.Add(date+" "+time+" "+tableNumber+" "+numberOfGuests+" "+name);
        }

        //public static void WriteToFile()
        //{

        //    try
        //    {

        //        using (StreamWriter sw = new StreamWriter("Bokningar.txt", true))
        //        {
        //            TableReservation.tableReservationProperties.ForEach(reservation => sw.WriteLine(reservation));

        //        }

        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //public static void WriteNewFile()
        //{
        //    try
        //    {

        //        using (StreamWriter sw = new StreamWriter("Bokningar.txt", false))
        //        {
        //            TableReservation.tableReservationProperties.ForEach(reservation => sw.WriteLine(reservation));
        //        }

        //    }
        //    catch (Exception e)
        //    {

        //    }
        ////}

        //public static void ReadFromFile()
        //{
        //    try
        //    {
        //        string fileName = "Bokningar.txt";
        //        TableReservation.tableReservationProperties.Clear();
        //        string[] lines = File.ReadAllLines(fileName);
        //        TableReservation.tableReservationProperties = lines.ToList();

        //        string date;
        //        string time;
        //        int numberOfGuests;
        //        string tableNumber;
        //        Table table;
        //        string name;

        //        TableReservation.reservationList.Clear();

        //        foreach (string reservation in TableReservation.tableReservationProperties)
        //        {
        //            date = reservation.Substring(0, 10);
        //            time = reservation.Substring(11, 5);
        //            tableNumber = reservation.Substring(17, 1);
        //            numberOfGuests = Int32.Parse(reservation.Substring(19, 1));
        //            table = new Table(Int32.Parse(tableNumber), numberOfGuests);
        //            name = reservation.Substring(21);

        //            TableReservation.reservationList.Add(new TableReservation(name, table, numberOfGuests, date, time));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}

        public static int GetNumberOfReservedSeatsAtSelectedTable(string date, string name, string time, int tableNumber)
        {
            var reservationsWithSameDate = TableReservation.reservationList.Where(reservation => reservation.Date==date)
                                                                           .Select(reservation => reservation)
                                                                           .ToList();
            var reservationWithSameTime = reservationsWithSameDate.Where(reservation => reservation.Time==time)
                                                                  .Select(reservation => reservation)
                                                                  .ToList();

            var reservationsAtChosenTable = reservationWithSameTime.Where(reservation => reservation.table.Number==tableNumber)
                                                                     .Select(reservation => reservation)
                                                                     .ToList();
            var freeSeatsAtTable = reservationsAtChosenTable.Select(reservation => reservation.table.NumberOfFreeSeats)
                                                            .ToList();

            List<int> reservedSeatsPerReservation = new List<int>();

            for (int i = 0; i<freeSeatsAtTable.Count; i++)
            {
                int reservedAtTable = 5 - freeSeatsAtTable[i];
                reservedSeatsPerReservation.Add(reservedAtTable);
            }
            int sumOfReservedSeats = 0;

            for (int i = 0; i<reservedSeatsPerReservation.Count; i++)
            {
                sumOfReservedSeats = sumOfReservedSeats + reservedSeatsPerReservation[i];
            }

            return sumOfReservedSeats;
        }

        public static int GetNumberOfFreeSeatsAtSelectedTable(string date, string name, string time, int tableNumber)
        {
            var reservationsWithSameDate = TableReservation.reservationList.Where(reservation => reservation.Date==date)
                                                                           .Select(reservation => reservation)
                                                                           .ToList();
            var reservationWithSameTime = reservationsWithSameDate.Where(reservation => reservation.Time==time)
                                                                  .Select(reservation => reservation)
                                                                  .ToList();

            var reservationsAtChosenTable = reservationWithSameTime.Where(reservation => reservation.table.Number==tableNumber)
                                                                     .Select(reservation => reservation)
                                                                     .ToList();
            var freeSeatsAtTable = reservationsAtChosenTable.Select(reservation => reservation.table.NumberOfFreeSeats)
                                                            .ToList();

            List<int> reservedSeatsPerReservation = new List<int>();

            for (int i = 0; i<freeSeatsAtTable.Count; i++)
            {
                int reservedAtTable = 5 - freeSeatsAtTable[i];
                reservedSeatsPerReservation.Add(reservedAtTable);
            }
            int sumOfReservedSeats = 0;

            for (int i = 0; i<reservedSeatsPerReservation.Count; i++)
            {
                sumOfReservedSeats = sumOfReservedSeats + reservedSeatsPerReservation[i];
            }

            int sumOfFreeSeats = 5;

            return sumOfFreeSeats-sumOfReservedSeats;
        }

        string GetFreeTables(string date, string name, string time, int tableNumber, int numberOfGuests)
        {
            //Gå igenom ordentligt och kolla varför det inte fungerar

            var reservationsWithSameDate = TableReservation.reservationList.Where(reservation => reservation.Date==date)
                                                                           .Select(reservation => reservation)
                                                                           .ToList();
            var reservationWithSameTime = reservationsWithSameDate.Where(reservation => reservation.Time==time)
                                                                  .Select(reservation => reservation)
                                                                  .ToList();

            var reservedTablesAtSameTime = reservationWithSameTime.Select(reservation => reservation.table.Number).ToList();

            //var reservationsAtChosenTable = reservationWithSameTime.Where(reservation => reservation.table.Number==tableNumber)
            //                                                         .Select(reservation => reservation)
            //                                                         .ToList();
            //reservationsAtChosenTable.Select(reservation => reservation.table.Number).ToList();

            List<int> numbersOfTablesAvailable = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            List<int> listOfAvailableTables = new List<int>();
            List<Table> otherTablesWithReservedSeats = new List<Table>();
            var otherTablesAtSameTime = numbersOfTablesAvailable;

            for (int i = 0; i<reservedTablesAtSameTime.Count; i++)
            {
                listOfAvailableTables = numbersOfTablesAvailable.Where(table => table!=reservedTablesAtSameTime[i]).Select(table => table).ToList();
                otherTablesAtSameTime=numbersOfTablesAvailable.Where(number => number==reservedTablesAtSameTime[i]).Select(number => number).ToList();
            }

            var tablesOtherThanChosenTable = otherTablesAtSameTime.Where(table => table!=tableNumber).Select(table => table).Distinct().ToList();

            tablesOtherThanChosenTable.ForEach(table => otherTablesWithReservedSeats.Add(new Table(table, GetNumberOfFreeSeatsAtSelectedTable(date, name, time, table))));

            otherTablesWithReservedSeats.Select(table => table).ToList();

            var tablesWithEnoughSeats = otherTablesWithReservedSeats.Where(table => table.NumberOfFreeSeats>=numberOfGuests).Select(table => table.Number).ToList();
            listOfAvailableTables.AddRange(tablesWithEnoughSeats);
            var orderedlistOfAvailableTables = listOfAvailableTables.OrderBy(table => table);

            //Kolla om borden(som inte är input-bordsnummer) som har bokningar samma tid har lediga platser som är fler eller lika med numberOfGuests.Isf add till available tables -listan
            //Fixa aggregate kolla videolektionen

            //numbersOfTablesAvailable.Where(number => number!=tableNumber).Select(number => number).ToList();

            string availableTables = orderedlistOfAvailableTables.Aggregate("", (allTables, next) => allTables +" "+ next.ToString()); //kolla om den fungerar eller fixa

            return availableTables;
        }

    }

}
