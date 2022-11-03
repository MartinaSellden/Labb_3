using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using System.Runtime.CompilerServices;

namespace Labb_3
{
    internal class TableReservation
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public Table table { get; set; }
        public int NumberOfGuests { get; set; }

        public static List<string> tableReservationProperties = new List<string>();
        public static List<TableReservation> reservationList = new List<TableReservation>();

        public TableReservation(string Name, Table table, int numberOfGuests, DateTime Date, string Time)
        {
            this.Name = Name;
            this.table = table;
            this.NumberOfGuests = numberOfGuests;
            this.Date = Date;
            this.Time = Time;
        }
        public static void CreateNewReservation(DateTime date, string name, string time, int tableNumber, int numberOfGuests)
        {
            Table newTable = new Table(tableNumber, numberOfGuests);

            reservationList.Add(new TableReservation(name, newTable, numberOfGuests, date, time));

            tableReservationProperties.Clear();

            tableReservationProperties.Add(date.ToShortDateString()+" "+time+" "+tableNumber+" "+numberOfGuests+" "+name);
        }
        public static async Task WriteToFileAsync()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Bokningar.txt", true))
                {
                    foreach (var reservationLine in tableReservationProperties)
                    {
                        await sw.WriteLineAsync(reservationLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        } 
        public static async Task WriteNewFileAsync()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Bokningar.txt", false))
                {
                    foreach (var reservationLine in tableReservationProperties)
                    {
                        await sw.WriteLineAsync(reservationLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task ReadFromFileAsync()
        {
            try
            {
                string fileName = "Bokningar.txt";
                tableReservationProperties.Clear();
                string[] lines = await File.ReadAllLinesAsync(fileName);
                tableReservationProperties = lines.ToList();

                string dateFromFile;
                DateTime date;
                string time;
                int numberOfGuests;
                string tableNumber;
                Table table;
                string name;

                reservationList.Clear();

                foreach (string reservation in tableReservationProperties)
                {
                    dateFromFile = reservation.Substring(0, 10);
                    date = DateTime.Parse(dateFromFile);
                    time = reservation.Substring(11, 5);
                    tableNumber = reservation.Substring(17, 1);
                    numberOfGuests = Int32.Parse(reservation.Substring(19, 1));
                    table = new Table(Int32.Parse(tableNumber), numberOfGuests);
                    name = reservation.Substring(21);

                    reservationList.Add(new TableReservation(name, table, numberOfGuests, date, time));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static int GetNumberOfReservedSeatsAtSelectedTable(DateTime date, string name, string time, int tableNumber)
        {
            var reservationsWithSameDate = reservationList.Where(reservation => reservation.Date==date)
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
        public static int GetNumberOfFreeSeatsAtSelectedTable(DateTime date, string name, string time, int tableNumber)
        {
            var reservationsWithSameDate = reservationList.Where(reservation => reservation.Date==date)
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
    }
}
