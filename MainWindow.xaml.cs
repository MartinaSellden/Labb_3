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
            CreateFile();
            reservationListBox.ItemsSource=TableReservation.reservationList;
            WriteToFile();
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

                if (input.Day<DateTime.Now.Day)
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

        public void UpdateReservationListBox()
        {
            reservationListBox.ItemsSource = null;
            reservationListBox.ItemsSource = TableReservation.reservationList;
        }

        public void CreateFile()
        {
            using (var myFile = File.Create("Bokningar.txt"))
            {
                TableReservation.reservationList.Add(new TableReservation("Martina", 5, "2022-11-01", "20.30"));
                TableReservation.reservationList.Add(new TableReservation("Kungen", 5, "2022-11-05", "21.30"));
                TableReservation.tableReservationProperties.Add("2022-11-01 20.30 5 Martina");
                TableReservation.tableReservationProperties.Add("2022-11-05 21.30 5 Kungen ");              
            }

        }

        public void WriteToFile()
        {
            using (var myFile = File.OpenWrite("Bokningar.txt"))
            {
                File.AppendAllLines("Bokningar.txt", TableReservation.tableReservationProperties);
            }
           
        }

    public void ReadFromFile()
        {
            throw new NotImplementedException();
        }


        private void reservationButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateInput = CheckDateInput();
            string date = dateInput.ToShortDateString();

            string name = CheckNameInput();

            string time = CheckTimeInput();

            int tableNumber = CheckTableNumberInput();

           
            TableReservation.reservationList.Add(new TableReservation(name, tableNumber, date, time));
            UpdateReservationListBox();

            //var resservation = TableReservation.reservationList.Select(reservation => "Bokning: " + reservation.Name+reservation.Date+reservation.Time+reservation.TableNumber);
            //string s = resservation.ToString();
            //MessageBox.Show(s);

        }



    }
 

} 


   

