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
                MessageBox.Show("Du måste välja ett bordsnummer","Bordsnummer ej valt!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            input = tableNumberComboBox.Text;
            tableNumber = Convert.ToInt32(input);
            return tableNumber;

        }
        private string CheckTimeInput()
        {

            string time = timeComboBox.Text.ToString();
            while (time == null)
            {
                MessageBox.Show("Du måste välja en tid för att göra dn bokning", "Tid ej vald", MessageBoxButton.OK, MessageBoxImage.Error);
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

            //using (var file = File.OpenWrite("Bokningar.txt"))
            //{
            //    File.AppendAllLines("Bokningar.txt", TableReservation.tableReservationProperties);
            //    UpdateReservationListBox();
            //}

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
        private void ReadFromFile(string fileName)
        {
            TableReservation.tableReservationProperties.Clear();
            string[] lines = File.ReadAllLines(fileName);
            TableReservation.tableReservationProperties = lines.ToList();

            string date;
            string time;
            int numberOfGuests;
            string tableNumber;
            Table table;
            string name;

            foreach(string reservation in TableReservation.tableReservationProperties)
            {
                date = reservation.Substring(0,10);
                time = reservation.Substring(11, 5);
                tableNumber = reservation.Substring(17, 1);
                numberOfGuests = Int32.Parse(reservation.Substring(19, 1));
                table = new Table(Int32.Parse(tableNumber), 5-numberOfGuests);
                name = reservation.Substring(21);

                TableReservation.reservationList.Add(new TableReservation(name,table,numberOfGuests,date,time));

            } //sätt try catch för argumentOutOfrangeexception

            // 2022-11-01 20.30 5 2 Martina       exempel
            // få in parametrarna från listan och göra object till en "reservationList" lista av bokningar. Alltså att reservationList blir vad som finns exakt just nu i filen.
            //Namn som kan variera i längd bör står sist. 
        }

        public void MakeReservation()
        {

            DateTime dateInput = CheckDateInput();   
            string date = dateInput.ToShortDateString();

            string name = CheckNameInput();

            string time = CheckTimeInput();

            int tableNumber = CheckTableNumberInput();

            int numberOfGuests = CheckNumberOfGuests();

            var reservationsWithSameDate = TableReservation.reservationList.Where(reservation => reservation.Date==date)
                                                                           .Select(reservation=>reservation)
                                                                           .ToList();
            var reservationWithSameTime = reservationsWithSameDate.Where(reservation => reservation.Time==time)
                                                                  .Select(reservation => reservation)
                                                                  .ToList();
            //var reservationWithSameTableNumber = reservationWithSameTime.SelectMany(reservation=>reservation.table)

            //var TableNumbersInSameTime = reservationWithSameTime.Select(reservation=>reservation.table.Number).ToList();
            //var TableNumberCorrespondingToNewReservation = TableNumbersInSameTime.Where(number => number==tableNumber).Select(number => number).ToList();
            var reservationsAtChosenTable = reservationWithSameTime.Where(reservation => reservation.table.Number==tableNumber)
                                                                     .Select(reservation => reservation)
                                                                     .ToList();
            var seats = reservationsAtChosenTable.Select(reservation => reservation.table.NumberOfReservedSeats).ToList();  //freeSeats istället ?

            int seatsAvailable = 5;
            int sumOfAvailableSeats = 0;

            foreach (var available in seats)
            {
                sumOfAvailableSeats += available; //seats[0];
            }
            if (sumOfAvailableSeats!=0)
            {
                seatsAvailable = sumOfAvailableSeats;
 
                if (seatsAvailable>=numberOfGuests)
                {
                    Table newTable = new Table(tableNumber, numberOfGuests);

                    TableReservation.reservationList.Add(new TableReservation(name, newTable, numberOfGuests, date, time));

                    TableReservation.tableReservationProperties.Clear();

                    TableReservation.tableReservationProperties.Add(date+" "+time+" "+tableNumber+" "+numberOfGuests+" "+name);

                    WriteToFile();

                    Clear();

                    DisplayReservations();
                }
                else if (seatsAvailable<=0)
                {
                    MessageBox.Show("Det finns inga platser kvar vid valt bord, vänligen försök igen!", "Inga lediga platser vid valt bord", MessageBoxButton.OK, MessageBoxImage.Error);//eventuellt kolla vilka bord som har lediga platser de tiderna och föreslå.
                }
                else
                {
                    MessageBox.Show("Det finns "+ (seatsAvailable) + " platser kvar vid valt bord. Justera antalet personer du vill boka för eller välj annat bord", "Begränsat antal platser vid bordet", MessageBoxButton.OK, MessageBoxImage.Error); //eventuellt kolla vilka bord som har lediga platser de tiderna 
                }


            }
            else
            {
                Table newTable = new Table(tableNumber, numberOfGuests);

                TableReservation.reservationList.Add(new TableReservation(name, newTable, numberOfGuests, date, time));

                TableReservation.tableReservationProperties.Clear();

                TableReservation.tableReservationProperties.Add(date+" "+time+" "+tableNumber+" "+numberOfGuests+" "+name);

                WriteToFile();

                Clear();

                DisplayReservations();
            }
        }

        private void DisplayReservations()
        {
            
            ReadFromFile("Bokningar.txt");
            UpdateReservationListBox();

        }

        private void Clear()
        {
            datepicker1.SelectedDate=null;
            timeComboBox.SelectedValue=null;
            nameTextBox.Clear();
            tableNumberComboBox.SelectedValue=null;
            GuestsComboBox.SelectedValue=null;
        }


        private void reservationButton_Click(object sender, RoutedEventArgs e)
        {
            MakeReservation();  

            //kolla att man inte kan lägga till tomma

            //var resservation = TableReservation.reservationList.Select(reservation => "Bokning: " + reservation.Name+reservation.Date+reservation.Time+reservation.TableNumber);
            //string s = resservation.ToString();
            //MessageBox.Show(s);

        }

        private void RemoveReservation_Click(object sender, RoutedEventArgs e)   //Ändra namn på metoderna
        {
            if (reservationListBox.SelectedItem==null)
                return;
            TableReservation.tableReservationProperties.Remove((string)reservationListBox.SelectedItem);
            WriteNewFile();
            DisplayReservations();
            UpdateReservationListBox();

            //Fortsätta kolla. Den tar bort det jag valt. Men inte från filen.
        }
    }

}




