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
            DisplayReservationsAsync();
        }
        private async void DisplayReservationsAsync()
        {
            await TableReservation.ReadFromFileAsync(); 
            UpdateReservationListBox();
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
        private async void MakeReservationAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await TableReservation.ReadFromFileAsync();

                string name = "";
                string input = "";
                string validInput = "";
                string time = "";
                int tableNumber = 0;
                int numberOfGuests = 0;
                DateTime date = DateTime.Now;
                input = nameTextBox.Text;
                Regex r = new Regex(@"[a-öA-Ö]{2,}");
                name = r.IsMatch(input) ? input : "";

                if (datepicker1.Text.Length==0 || !r.IsMatch(nameTextBox.Text)|| timeComboBox.SelectedItem == null || tableNumberComboBox.SelectedItem == null || GuestsComboBox.SelectedItem==null)
                {               
                    if (datepicker1.SelectedDate==null)
                    {
                        MessageBox.Show("Du behöver välja ett datum för att slutföra bokningen", "Datum ej valt", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (nameTextBox.Text=="")
                    {
                        MessageBox.Show("Du behöver fylla i ett namn för bokningen, försök igen!", "Fyll i namn för bokningen", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (nameTextBox.Text!="" && !r.IsMatch(nameTextBox.Text))
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

                if (datepicker1.SelectedDate!=null)
                {
                    date = datepicker1.SelectedDate.Value.Date;
                }
                if (datepicker1.SelectedDate!=null && datepicker1.SelectedDate<DateTime.Now)
                {
                    MessageBox.Show("Välj ett datum. Du kan inte välja ett datum innan dagens datum", "Du kan ej göra bokningar bakåt i tiden", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (timeComboBox.SelectedItem != null)
                {
                    time = (timeComboBox.Text);
                }
                if (tableNumberComboBox.SelectedItem != null)
                {
                    tableNumber = Convert.ToInt32(tableNumberComboBox.Text);
                }
                if (GuestsComboBox.SelectedItem != null)
                {
                    numberOfGuests = int.Parse(GuestsComboBox.Text);
                }

                if (date>=DateTime.Now && nameTextBox.Text!="" && r.IsMatch(nameTextBox.Text) && timeComboBox.SelectedItem != null && tableNumberComboBox.SelectedItem != null && GuestsComboBox.SelectedItem!=null)
                {
                    int freeSeats = 5;

                    int reservedSeats = TableReservation.GetNumberOfReservedSeatsAtSelectedTable(date, name, time, tableNumber);

                    if (reservedSeats!=0)
                    {
                        freeSeats = TableReservation.GetNumberOfFreeSeatsAtSelectedTable(date, name, time, tableNumber);

                        if (freeSeats>=numberOfGuests)
                        {
                            TableReservation.CreateNewReservation(date, name, time, tableNumber, numberOfGuests);

                            await TableReservation.WriteToFileAsync();

                            Clear();

                            DisplayReservationsAsync();
                        }
                        else if (freeSeats<=0)
                        {
                            MessageBox.Show("Det finns inga lediga platser vid bord nummer "+tableNumber+", vänligen välj ett annat bord!", "Inga lediga" +
                                " platser vid bord "+tableNumber, MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        else
                        {
                            MessageBox.Show("Det finns "+freeSeats+" platser kvar vid bord "+tableNumber+". Justera antalet personer " +
                                "du vill boka för eller välj annat bord.", "Begränsat antal platser vid bordet", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        TableReservation.CreateNewReservation(date, name, time, tableNumber, numberOfGuests);

                        await TableReservation.WriteToFileAsync();

                        Clear();

                        DisplayReservationsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void RemoveReservationAsync(object sender, RoutedEventArgs e) 
        {
            try
            {
                if (reservationListBox.SelectedItem==null)
                    return;
                TableReservation.tableReservationProperties.Remove((string)reservationListBox.SelectedItem);
                await TableReservation.WriteNewFileAsync();
                DisplayReservationsAsync();
                UpdateReservationListBox();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}




