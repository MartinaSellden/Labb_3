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

        private void DisplayReservations()
        {
            TableReservation.ReadFromFile();
            UpdateReservationListBox();
        }

        //private int CheckTableNumberInput()
        //{
        //    string input;
        //    int tableNumber;

        //    if (tableNumberComboBox.SelectedItem==null)
        //    {
        //        MessageBox.Show("Du måste välja ett bordsnummer", "Bordsnummer ej valt!", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    else
        //    {
        //        input = tableNumberComboBox.Text;
        //        tableNumber = Convert.ToInt32(input);
        //        return tableNumber;
        //    }


        //}
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
                MessageBox.Show(e.ToString());
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

        private void Clear()
        {
            datepicker1.SelectedDate=null;
            timeComboBox.SelectedValue=null;
            nameTextBox.Clear();
            tableNumberComboBox.SelectedValue=null;
            GuestsComboBox.SelectedValue=null;
        }

        private void MakeReservation(object sender, RoutedEventArgs e)
        {
            try
            {
                TableReservation.ReadFromFile();

                string name = "";
                string input = "";
                string validInput = "";
                string time = "";
                int tableNumber = 0;
                int numberOfGuests = 0;
                DateTime date;
                date = datepicker1.SelectedDate.Value.Date;
                input = nameTextBox.Text;
                Regex r = new Regex(@"[a-öA-Ö]{2,}");
                name = r.IsMatch(input) ? input : "";

                //nameTextBox.Text=="" ||

                if (datepicker1==null || date<DateTime.Now || !r.IsMatch(nameTextBox.Text)|| timeComboBox.SelectedItem == null || tableNumberComboBox.SelectedItem == null || GuestsComboBox.SelectedItem==null)
                {
                    if (datepicker1==null)
                    {
                        MessageBox.Show("Du behöver välja ett datum för att slutföra bokningen", "Datum ej valt", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (date<DateTime.Now)
                    {
                        MessageBox.Show("Välj ett datum. Du kan inte välja ett datum innan dagens datum", "Du kan ej göra bokningar bakåt i tiden", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    //if (nameTextBox.Text=="")
                    //{
                    //    MessageBox.Show("Du behöver fylla i ett namn för bokningen, försök igen!", "Fyll i namn för bokningen", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    if (!r.IsMatch(nameTextBox.Text))
                    {
                        MessageBox.Show("Fyll i ditt namn, endast bokstäver!", "Ogiltigt format", MessageBoxButton.OK, MessageBoxImage.Error);
                    }                 
                    if (timeComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Du måste välja en tid för att göra din bokning", "Tid ej vald", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (tableNumberComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Du måste välja ett bordsnummer", "Bordsnummer ej valt!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (GuestsComboBox.SelectedItem==null)
                    {
                        MessageBox.Show("Du måste fylla i antalet gäster", "Information om antalet gäster saknas", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }



                //else
                //{
                //    MessageBox.Show("Du behöver välja ett datum för att slutföra bokningen", "Datum ej valt", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

                //if (nameTextBox.Text!="")
                //{
                    
    

                //    if (string.IsNullOrEmpty(validInput))
                //    {
                //        MessageBox.Show("Ogiltigt format, endast bokstäver!", "Ogiltigt format", MessageBoxButton.OK, MessageBoxImage.Error);
                //    }
                //    else
                //    name = validInput;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Du behöver fylla i ett namn för bokningen, försök igen!", "Fyll i namn för bokningen", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                if (timeComboBox.SelectedItem != null)
                {
                    time = (timeComboBox.Text);

                }
                //else
                //{
                //    MessageBox.Show("Du måste välja en tid för att göra din bokning", "Tid ej vald", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

                if (tableNumberComboBox.SelectedItem != null)
                {
                    tableNumber = Convert.ToInt32(tableNumberComboBox.Text);

                }
                //else
                //{
                //    MessageBox.Show("Du måste välja ett bordsnummer", "Bordsnummer ej valt!", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

                if (GuestsComboBox.SelectedItem != null)
                {
                    numberOfGuests = int.Parse(GuestsComboBox.Text);
                }
                //else
                //{
                //    MessageBox.Show("Du måste fylla i antalet gäster", "Information om antalet gäster saknas", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

                if (date>=DateTime.Now && nameTextBox.Text!="" && r.IsMatch(nameTextBox.Text) && timeComboBox.SelectedItem != null && tableNumberComboBox.SelectedItem != null && GuestsComboBox.SelectedItem!=null)
                {
                    int freeSeats = 5;

                    int reservedSeats = TableReservation.GetNumberOfReservedSeatsAtSelectedTable(date, name, time, tableNumber);

                    //string availableTables = GetFreeTables(date, name, time, tableNumber, numberOfGuests);

                    if (reservedSeats!=0)
                    {
                        freeSeats = TableReservation.GetNumberOfFreeSeatsAtSelectedTable(date, name, time, tableNumber);

                        if (freeSeats>=numberOfGuests)
                        {
                            TableReservation.CreateNewReservation(date, name, time, tableNumber, numberOfGuests);

                            TableReservation.WriteToFile();

                            Clear();

                            DisplayReservations();
                        }
                        else if (freeSeats<=0)
                        {
                            MessageBox.Show("Det finns inga lediga platser vid bord nummer "+tableNumber+", vänligen välj ett annat bord!"
                                /*"Bord med lediga platser vald tid är "/*+availableTables+""*/, "Inga lediga platser vid bord "+tableNumber, MessageBoxButton.OK, MessageBoxImage.Error);
                            //eventuellt kolla vilka bord som har lediga platser de tiderna och föreslå.
                        }
                        else
                        {
                            MessageBox.Show("Det finns "+freeSeats+" platser kvar vid bord "+tableNumber+". Justera antalet personer " +
                                "du vill boka för eller välj annat bord."/* Bord med lediga platser vald tid är: "*//*+availableTables+""*/, "Begränsat antal platser vid bordet", MessageBoxButton.OK, MessageBoxImage.Error);
                            //eventuellt kolla vilka bord som har lediga platser de tiderna 
                        }
                    }
                    else
                    {
                        TableReservation.CreateNewReservation(date, name, time, tableNumber, numberOfGuests);

                        TableReservation.WriteToFile();

                        Clear();

                        DisplayReservations();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveReservation(object sender, RoutedEventArgs e)   //Ändra namn på metoderna
        {
            if (reservationListBox.SelectedItem==null)
                return;
            TableReservation.tableReservationProperties.Remove((string)reservationListBox.SelectedItem);
            TableReservation.WriteNewFile();
            DisplayReservations();
            UpdateReservationListBox();
        }
    }

}




