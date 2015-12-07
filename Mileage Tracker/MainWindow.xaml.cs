using MileageTracker.Properties;
using MileageTracker.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MileageTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Destinations
        public List<String> Destinations
        {
            get { return (List<String>)GetValue(DestinationsProperty); }
            set { SetValue(DestinationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Destinations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationsProperty =
            DependencyProperty.Register("Destinations", typeof(List<String>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //Destinations
        
        #region Trip
        public TripInfo Trip
        {
            get { return (TripInfo)GetValue(TripProperty); }
            set { SetValue(TripProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TripProperty =
            DependencyProperty.Register("Trip", typeof(TripInfo), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //Trip

        #region Vehicles
        public List<String> Vehicles
        {
            get { return (List<String>)GetValue(VehiclesProperty); }
            set { SetValue(VehiclesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vehicles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VehiclesProperty =
            DependencyProperty.Register("Vehicles", typeof(List<String>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //Vehicles

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Destinations = new List<string>() { "ABC", "Martins" };
            Vehicles = new List<string>() { "Buick", "Montanna", "Volvo" };
            Trip = new TripInfo();
            var s = Settings.Default;

            Trip.Destination = s.Destination;
            Trip.Vehicle = s.Vehicle;
            Trip.Start.Miles = s.StartMiles;
            Trip.End.Miles = s.EndMiles;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var s = Settings.Default;
            s.Destination = Trip.Destination;
            s.Vehicle = Trip.Vehicle;
            s.StartMiles = Trip.Start.Miles;
            s.EndMiles = Trip.End.Miles;
            s.Save();
        }

        private void SaveTrip_Click(object sender, RoutedEventArgs e)
        {
            var newTrip = new {
                Destination = Trip.Destination,
                Vehicle = Trip.Vehicle,
                Date = DateTime.Now.ToShortDateString(),
                Start = Trip.Start.Miles,
                End = Trip.End.Miles,
                Distance = Trip.TotalMiles
            };
            var jsonString = JsonConvert.SerializeObject(newTrip, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.AppendAllText("Trips.txt", jsonString);
        }
    }
}
