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

        #region LogDescription
        public String LogDescription
        {
            get { return (String)GetValue(LogDescriptionProperty); }
            set { SetValue(LogDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogDescriptionProperty =
            DependencyProperty.Register("LogDescription", typeof(String), typeof(MainWindow), new PropertyMetadata(String.Empty));
        #endregion //LogDescription

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

        #region Trips
        public List<SimpleTripInfo> Trips
        {
            get { return (List<SimpleTripInfo>)GetValue(TripsProperty); }
            set { SetValue(TripsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trips.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TripsProperty =
            DependencyProperty.Register("Trips", typeof(List<SimpleTripInfo>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //Trips

        #region Vehicles
        public List<Vehicle> Vehicles
        {
            get { return (List<Vehicle>)GetValue(VehiclesProperty); }
            set { SetValue(VehiclesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vehicles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VehiclesProperty =
            DependencyProperty.Register("Vehicles", typeof(List<Vehicle>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //Vehicles

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ReadDestinationsFile();
            ReadTripsFile();
            ReadVehiclesFile();

            Trip = new TripInfo();
            Trip.PropertyChanged += Trip_PropertyChanged;
            var s = Settings.Default;
            Trip.Destination = s.Destination;
            Trip.Vehicle = Vehicles.FirstOrDefault(v => v.Name == s.Vehicle);
        }

        private void Trip_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if  (e.PropertyName == "Destination")
            {
                if (Destinations.Contains(Trip.Destination) == false)
                {
                    Destinations.Add(Trip.Destination);
                    var jsonString = JsonConvert.SerializeObject(Destinations, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                    File.WriteAllText("Destinations.txt", jsonString);
                    ReadDestinationsFile();
                }
            }
        }

        private void ReadDestinationsFile()
        {
            try
            {
                Destinations = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("Destinations.txt"));
                Destinations.Sort();
            }
            catch (Exception)
            {
                Destinations = new List<string>() { "ABC", "Martins", "Other" };
            }
        }

        private void ReadTripsFile()
        {
            try
            {
                Trips = JsonConvert.DeserializeObject<List<SimpleTripInfo>>(File.ReadAllText("Trips.txt"));
                var firstDate = Trips.Min(t => Convert.ToDateTime(t.Date));
                var lastDate = Trips.Max(t => Convert.ToDateTime(t.Date));
                LogDescription = string.Format("Create Log for:\n{0} trips totalling {1} miles\nfrom {2} to {3}",
                    Trips.Count, 
                    Trips.Sum(t => t.Distance), 
                    firstDate.ToShortDateString(), 
                    lastDate.ToShortDateString()
                    );
            }
            catch (Exception)
            {
                Trips = new List<SimpleTripInfo>();
            }
        }

        private void ReadVehiclesFile()
        {
            try
            {
                Vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(File.ReadAllText("Vehicles.txt"));
            }
            catch (Exception)
            {
                Vehicles = new List<Vehicle>()
                {
                    new Vehicle() { Name = "Buick", Odometer = 204615 },
                    new Vehicle() { Name = "Montanna", Odometer = 146000 },
                    new Vehicle() { Name = "Volvo", Odometer = 150000 }
                };
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var s = Settings.Default;
            s.Destination = Trip.Destination;
            if (Trip.Vehicle != null)
                s.Vehicle = Trip.Vehicle.Name;

            var jsonString = JsonConvert.SerializeObject(Vehicles, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.WriteAllText("Vehicles.txt", jsonString);
            s.Save();

        }

        private void SaveTrip_Click(object sender, RoutedEventArgs e)
        {
            var newTrip = new SimpleTripInfo() {
                Destination = Trip.Destination,
                Vehicle = Trip.Vehicle.Name,
                Date = DateTime.Now.ToShortDateString(),
                Start = Trip.Start.Miles,
                End = Trip.End.Miles,
                Distance = Trip.TotalMiles
            };
            Trips.Add(newTrip);

            var jsonString = JsonConvert.SerializeObject(Trips, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.WriteAllText("Trips.txt", jsonString);
            ReadTripsFile();
            Trip.Start.Miles = Trip.End.Miles;
        }

        private void CreateLog_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
