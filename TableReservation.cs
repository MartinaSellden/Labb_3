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

        public TableReservation(string name, Table table, int numberOfGuests, DateTime date, string time)
        {
            this.Name = name;
            this.table = table;
            this.NumberOfGuests = numberOfGuests;
            this.Date = date;
            this.Time = time;
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
        public static async Task CreateReservationFileAsync()
        {
            try
            {
                tableReservationProperties = new List<string>
                {"2022-11-25 20.00 3 4 Jack Sparrow",
                 "2022-11-25 20.00 3 1 Davy Jones",
                 "2022-11-30 19.00 2 1 Mahatma Ghandi", 
                 "2022-12-03 19.00 4 2 Claes-Göran",
                 "2022-11-30 19.00 2 1 Vincent van Gogh" };

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

            var reservationsAtSelectedTable = reservationWithSameTime.Where(reservation => reservation.table.Number==tableNumber)
                                                                     .Select(reservation => reservation)
                                                                     .ToList();
            var reservedSeatsPerReservationAtSelectedTable = reservationsAtSelectedTable.Select(reservation => reservation.table.NumberOfReservedSeats)
                                                            .ToList();

            return reservedSeatsPerReservationAtSelectedTable.Sum();
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

            var reservedSeatsPerReservationAtSelectedTable = reservationsAtChosenTable.Select(reservation => reservation.table.NumberOfReservedSeats) 
                                                            .ToList();      

            int sumOfReservedSeats = reservedSeatsPerReservationAtSelectedTable.Sum();
            int sumOfFreeSeats = 5;

            return sumOfFreeSeats-sumOfReservedSeats;
        }
    }
}
