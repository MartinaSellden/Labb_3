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

        public int CheckTableNumberInput()
        {
            string input;
            int tableNumber;

            while (tableNumberComboBox.Text=="")
            {
                MessageBox.Show("Du måste välja ett bordsnummer");
            }
            input = tableNumberComboBox.Text;
            tableNumber = Convert.ToInt32(input);
            return tableNumber;

        }
        public string CheckTimeInput()
        {

            string time = timeComboBox.Text.ToString();
            while (time == null)
            {
                MessageBox.Show("Du måste välja en tid för att göra dn bokning");
            }
            return time;

        }
        public DateTime CheckDateInput()
        {
            DateTime input = DateTime.Now;
            try
            {
                while (datepicker1.SelectedDate==null)
                {
                    MessageBox.Show("Du behöver välja ett datum för att slutföra bokningen");
                }
                input = datepicker1.SelectedDate.Value.Date;

                if (input<DateTime.Now)
                {
                    MessageBox.Show("Du kan inte välja ett datum innan dagens datum");
                    CheckDateInput();
                }

            }
            catch (Exception e)
            {

            }
            return input;
        }
        public string CheckNameInput()
        {
            string input = nameTextBox.Text;
            string validInput;
            try
            {
                while (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Du behöver fylla i ett namn för bokningen, försök igen!");
                    input = nameTextBox.Text;
                }
                Regex r = new Regex(@"[a-öA-Ö]{2,}");

                validInput = r.IsMatch(input) ? input : "";
                if (string.IsNullOrEmpty(validInput))
                {
                    MessageBox.Show("Ogiltigt format, endast bokstäver!");
                    CheckNameInput();
                }

            }

            catch (Exception e)
            {
                //
            }
            return input;//[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }
        public int CheckNumberOfGuests()
        {
            string numberOfGuests = GuestsComboBox.Text.ToString();
            while (numberOfGuests == null)
            {
                MessageBox.Show("Du måste välja en tid för att göra dn bokning");
            }

            return Int32.Parse(numberOfGuests);
        }

        public void UpdateReservationListBox()
        {
            reservationListBox.ItemsSource = null;
            reservationListBox.ItemsSource = TableReservation.tableReservationProperties;
        }

        //public void CreateFile()
        //{

        //    using (StreamWriter sw = new StreamWriter("Bokningar.txt", true))
        //    {
        //        TableReservation.reservationList.Add(new TableReservation("Martina", 5, "2022-11-01", "20.30"));
        //        TableReservation.reservationList.Add(new TableReservation("Kungen", 5, "2022-11-05", "21.30"));

        //        WriteToFile();

        //    }

        //using (var myFile = File.Create("Bokningar.txt"))
        //{
        //    TableReservation.reservationList.Add(new TableReservation("Martina", 5, "2022-11-01", "20.30"));
        //    TableReservation.reservationList.Add(new TableReservation("Kungen", 5, "2022-11-05", "21.30"));
        //    TableReservation.tableReservationProperties.Add("2022-11-01 20.30 5 Martina");
        //    TableReservation.tableReservationProperties.Add("2022-11-05 21.30 5 Kungen ");
        //}

        //}

        public void WriteToFile()
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
        public void WriteNewFile()
        {

            //using (var file = File.OpenWrite("Bokningar.txt"))
            //{
            //    File.AppendAllLines("Bokningar.txt", TableReservation.tableReservationProperties);
            //    UpdateReservationListBox();
            //}

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
        public void ReadFromFile(string fileName)
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

        public void DisplayReservations()
        {
            
            ReadFromFile("Bokningar.txt");
            UpdateReservationListBox();
            //TableReservation.tableReservationProperties.Add("2022-11-01 20.30 5 2 Martina");
            //TableReservation.tableReservationProperties.Add("2022-11-05 21.30 5 2 Kungen ");
            //UpdateReservationListBox();
        }

        public void Clear()
        {
            datepicker1.SelectedDate=null;
            timeComboBox.SelectedValue=null;
            nameTextBox.Clear();
            tableNumberComboBox.SelectedValue=null;
            GuestsComboBox.SelectedValue=null;
        }


        private void reservationButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateInput = CheckDateInput();   //ska lägga allt detta i make reservation sen 
            string date = dateInput.ToShortDateString();

            string name = CheckNameInput();

            string time = CheckTimeInput();

            int tableNumber = CheckTableNumberInput();

            int numberOfGuests = CheckNumberOfGuests();

            Table newTable = new Table(tableNumber, numberOfGuests);

            TableReservation.reservationList.Add(new TableReservation(name, newTable, numberOfGuests, date, time));

            TableReservation.tableReservationProperties.Clear();

            TableReservation.tableReservationProperties.Add(date+" "+time+" "+tableNumber+" "+numberOfGuests+" "+name);

            WriteToFile();

            Clear();

            DisplayReservations();

            

            //kolla att man inte kan lägga till tomma

            //var resservation = TableReservation.reservationList.Select(reservation => "Bokning: " + reservation.Name+reservation.Date+reservation.Time+reservation.TableNumber);
            //string s = resservation.ToString();
            //MessageBox.Show(s);

        }

        //private void Display_Click(object sender, RoutedEventArgs e)
        //{
        //    DisplayReservations();
        //}

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




