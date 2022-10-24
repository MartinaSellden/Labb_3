using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Labb_3
{
    internal class TableReservation
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int TableNumber { get; set; }    
        
        public static List<string> tableReservationProperties = new List<string>();
        public static List<TableReservation> reservationList = new List<TableReservation>();
        //static List<Restaurant> restaurantsAvailable = new List<Restaurant>();

        public TableReservation(string Name, int TableNumber, string Date, string Time)
        {
           
            this.Name = Name;
            this.TableNumber = TableNumber; //kolla att numret är 1-5
            this.Date = Date; //fixa kalender man kan trycka på 
            this.Time = Time;

            //var bord = bookingList.Where(TableReservation =>TableReservation.Date == Date&&TableReservation.Time == Time)
            //           .Select(TableReservation => TableReservation.TableNumber==TableNumber? "Bordet är bokat denna tid": TableNumber)
            //           .ToList();

        }

        public void MakeReservation() //TableReservation eller void?
        {


            //TableReservation nyBokning = new TableReservation();
            //bookingList.Add(nyBokning);
            //skriv nya bokningen till fil
        }

        public void CancelReservation()
        {
            /*bookingList.RemoveAt(); *///hämta in index för den som ska bort
            // ta bort bokningen från filen
        }

        public List<TableReservation> ReadFromFile(string fileName)
        {
           string [] lines =  File.ReadAllLines(fileName);
           lines.ToList();
            // få in parametrarna från listan och göra object till en lista av bokningar. 
            //Namn som kan variera i längd bör står sist. 
            return new List<TableReservation>();
        }

        public void DisplayReservations()
        {
           
        }

        //public string CheckNameInput()
        //{
        //    string input = ;
        //    try
        //    {
        //        while (string.IsNullOrEmpty(input))
        //        {
        //            MessageBox.Show("Ej giltigt namn, försök igen!");
        //            input = ;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        //}

      

        //När man kollar datuminput, kolla att datumet inte är innan idag. 

    }

}
