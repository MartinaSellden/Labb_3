using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DisplayReservations();
        }

        private int CheckTableNumberInput()
        {
            string input;
            int tableNumber;

            while (tableNumberComboBox.Text=="")
            {
                MessageBox.Show("Du måste välja ett bordsnummer", "Bordsnummer ej valt!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            input = tableNumberComboBox.Text;
            tableNumber = Convert.ToInt32(input);
            return tableNumber;

        }
        private string CheckTimeInput()
        {

            string time = timeComboBox.Text.ToString();
            while (timeComboBox.Text== "")
            {
                MessageBox.Show("Du måste välja en tid för att göra din bokning", "Tid ej vald", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return time;

        }
        private DateTime CheckDateInput()
        {
            DateTime input = DateTime.Now;
            try
            {
                while (datepicker1.SelectedDate==null)
                {
                    MessageBox.Show("Du behöver välja ett datum för att slutföra bokningen", "Datum ej valt", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                input = datepicker1.SelectedDate.Value.Date;

                if (input<DateTime.Now)
                {
                    MessageBox.Show("Du kan inte välja ett datum innan dagens datum", "Du kan ej göra bokningar bakåt i tiden", MessageBoxButton.OK, MessageBoxImage.Error);
                    CheckDateInput();
                }

            }
            catch (Exception e)
            {

            }
            return input;
        }
        private string CheckNameInput()
        {
            string input = nameTextBox.Text;
            string validInput;
            try
            {
                while (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Du behöver fylla i ett namn för bokningen, försök igen!", "Fyll i namn för bokningen", MessageBoxButton.OK, MessageBoxImage.Error);
                    input = nameTextBox.Text;
                }
                Regex r = new Regex(@"[a-öA-Ö]{2,}");

                validInput = r.IsMatch(input) ? input : "";
                if (string.IsNullOrEmpty(validInput))
                {
                    MessageBox.Show("Ogiltigt format, endast bokstäver!", "Ogiltigt format", MessageBoxButton.OK, MessageBoxImage.Error);
                    CheckNameInput();
                }

            }

            catch (Exception e)
            {

            }
            return input;
        }
        private int CheckNumberOfGuests()
        {
            string numberOfGuests = GuestsComboBox.Text.ToString();
            while (numberOfGuests == null)
            {
                MessageBox.Show("Du måste välja en tid för din bokning", "Tid ej vald", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return Int32.Parse(numberOfGuests);
        }

        private void UpdateReservationListBox()
        {
            reservationListBox.ItemsSource = null;
            reservationListBox.ItemsSource = TableReservation.tableReservationProperties;
        }

        private void WriteToFile()
        {

            try
            {

                using (StreamWriter sw = new StreamWriter("Bokningar.txt", true))
                {
                    TableReservation.tableReservationProperties.ForEach(reservation => sw.WriteLine(reservation));

                }

            }
            catch (Exception e)
            {

            }
        }
        private void WriteNewFile()
        {
            try
            {

                using (StreamWriter sw = new StreamWriter("Bokningar.txt", false))
                {
                    TableReservation.tableReservationProperties.ForEach(reservation => sw.WriteLine(reservation));
                }

            }
            catch (Exception e)
            {

            }
        }
        private void ReadFromFile()
        {
            string fileName = "Bokningar.txt";
            TableReservation.tableReservationProperties.Clear();
            string[] lines = File.ReadAllLines(fileName);
            TableReservation.tableReservationProperties = lines.ToList();

            string date;
            string time;
            int numberOfGuests;
            string tableNumber;
            Table table;
            string name;

            TableReservation.reservationList.Clear();

            foreach (string reservation in TableReservation.tableReservationProperties)
            {
                date = reservation.Substring(0, 10);         
                time = reservation.Substring(11, 5);
                tableNumber = reservation.Substring(17, 1);
                numberOfGuests = Int32.Parse(reservation.Substring(19, 1));
                table = new Table(Int32.Parse(tableNumber), numberOfGuests);
                name = reservation.Substring(21);

                TableReservation.reservationList.Add(new TableReservation(name, table, numberOfGuests, date, time));

            } //sätt try catch för argumentOutOfrangeexception

            // 2022-11-01 20.30 5 2 Martina       exempel
            // få in parametrarna från listan och göra object till en "reservationList" lista av bokningar. Alltså att reservationList blir vad som finns exakt just nu i filen.
            //Namn som kan variera i längd bör står sist. 
        }
        private void Clear()
        {
            datepicker1.SelectedDate=null;
            timeComboBox.SelectedValue=null;
            nameTextBox.Clear();
            tableNumberComboBox.SelectedValue=null;
            GuestsComboBox.SelectedValue=null;
        }

        int GetNumberOfFreeSeatsAtSelectedTable(string date, string name, string time, int tableNumber)
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

        int GetNumberOfReservedSeatsAtSelectedTable(string date, string name, string time, int tableNumber)
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

        void CreateNewReservation(string date, string name, string time, int tableNumber, int numberOfGuests)
        {
            Table newTable = new Table(tableNumber, numberOfGuests);

            TableReservation.reservationList.Add(new TableReservation(name, newTable, numberOfGuests, date, time));

            TableReservation.tableReservationProperties.Clear();

            TableReservation.tableReservationProperties.Add(date+" "+time+" "+tableNumber+" "+numberOfGuests+" "+name);
        }

        string GetFreeTables(string date, string name, string time, int tableNumber,int numberOfGuests)
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

            for (int i = 0; i<listOfAvailableTables.Count; i++)
            {
                listOfAvailableTables = numbersOfTablesAvailable.Where(table => table!=reservedTablesAtSameTime[i]).Select(table => table).ToList();
                otherTablesAtSameTime=numbersOfTablesAvailable.Where(number => number==reservedTablesAtSameTime[i]).Select(number => number).ToList();    
            }

            var tablesOtherThanChosenTable = otherTablesAtSameTime.Where(table => table!=tableNumber).Select(table => table).ToList();
            var tablesOtherThanChosenTableWithoutDuplicates = tablesOtherThanChosenTable.Distinct();
            tablesOtherThanChosenTable.ForEach(table => otherTablesWithReservedSeats.Add(new Table(table, GetNumberOfReservedSeatsAtSelectedTable(date, name, time, tableNumber))));

            otherTablesWithReservedSeats.ForEach(table => table.NumberOfFreeSeats=GetNumberOfFreeSeatsAtSelectedTable(date, name, time, tableNumber));

            var tablesWithEnoughSeats = otherTablesWithReservedSeats.Where(table => table.NumberOfFreeSeats>=numberOfGuests).Select(table => table.Number).ToList();
            listOfAvailableTables.AddRange(tablesWithEnoughSeats);
            var orderedlistOfAvailableTables = listOfAvailableTables.OrderBy(table=>table);  

            //Kolla om borden(som inte är input-bordsnummer) som har bokningar samma tid har lediga platser som är fler eller lika med numberOfGuests.Isf add till available tables -listan
            //Fixa aggregate kolla videolektionen

            //numbersOfTablesAvailable.Where(number => number!=tableNumber).Select(number => number).ToList();

            string availableTables = orderedlistOfAvailableTables.Aggregate("", (allTables, next) => allTables +","+ next.ToString()); //kolla om den fungerar eller fixa

            return availableTables;
        }

        public void MakeReservation()
        {
            ReadFromFile();

            DateTime dateInput = CheckDateInput();
            string date = dateInput.ToShortDateString();

            string name = CheckNameInput();

            string time = CheckTimeInput();

            int tableNumber = CheckTableNumberInput();

            int numberOfGuests = CheckNumberOfGuests();

            int freeSeats = 5;

            int reservedSeats = GetNumberOfReservedSeatsAtSelectedTable(date, name, time, tableNumber);

            //string availableTables = GetFreeTables(date, name, time, tableNumber,numberOfGuests);

            if (reservedSeats!=0)
            {
                freeSeats = GetNumberOfFreeSeatsAtSelectedTable(date, name, time, tableNumber);

                if (freeSeats>=numberOfGuests)
                {
                    CreateNewReservation(date, name, time, tableNumber, numberOfGuests);

                    WriteToFile();

                    Clear();

                    DisplayReservations();
                }
                else if (freeSeats<=0)
                {
                    MessageBox.Show("Det finns inga lediga platser vid bord nummer "+tableNumber+", vänligen välj ett annat bord!\n" +
                        "Bord med lediga platser vald tid är "/*+availableTables+""*/, "Inga lediga platser vid bord "+tableNumber, MessageBoxButton.OK, MessageBoxImage.Error);
                    //eventuellt kolla vilka bord som har lediga platser de tiderna och föreslå.
                }
                else
                {
                    MessageBox.Show("Det finns "+freeSeats+" platser kvar vid bord "+tableNumber+". Justera antalet personer " +
                        "du vill boka för eller välj annat bord. Bord med lediga platser vald tid är: "/*+availableTables+*//*""*/, "Begränsat antal platser vid bordet", MessageBoxButton.OK, MessageBoxImage.Error);
                    //eventuellt kolla vilka bord som har lediga platser de tiderna 
                }


            }
            else
            {
                CreateNewReservation(date, name, time, tableNumber, numberOfGuests);

                WriteToFile();

                Clear();

                DisplayReservations();
            }
        }

        private void DisplayReservations()
        {

            ReadFromFile();
            UpdateReservationListBox();

        }
        private void reservationButton_Click(object sender, RoutedEventArgs e)
        {
            MakeReservation();

        }
        private void RemoveReservation_Click(object sender, RoutedEventArgs e)   //Ändra namn på metoderna
        {
            if (reservationListBox.SelectedItem==null)
                return;
            TableReservation.tableReservationProperties.Remove((string)reservationListBox.SelectedItem);
            WriteNewFile();
            DisplayReservations();
            UpdateReservationListBox();
        }
    }

}




