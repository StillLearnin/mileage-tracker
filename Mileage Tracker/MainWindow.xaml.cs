using MileageTracker.Properties;
using MileageTracker.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ObservableCollection<String> Destinations
        {
            get { return (ObservableCollection<String>)GetValue(DestinationsProperty); }
            set { SetValue(DestinationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Destinations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestinationsProperty =
            DependencyProperty.Register("Destinations", typeof(ObservableCollection<String>), typeof(MainWindow), new PropertyMetadata(null));
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
        public ObservableCollection<SimpleTripInfo> Trips
        {
            get { return (ObservableCollection<SimpleTripInfo>)GetValue(TripsProperty); }
            set { SetValue(TripsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trips.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TripsProperty =
            DependencyProperty.Register("Trips", typeof(ObservableCollection<SimpleTripInfo>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //Trips

        #region PreviousLogs
        public ObservableCollection<FileInfo> PreviousLogs
        {
            get { return (ObservableCollection<FileInfo>)GetValue(PreviousLogsProperty); }
            set { SetValue(PreviousLogsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousLogs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousLogsProperty =
            DependencyProperty.Register("PreviousLogs", typeof(ObservableCollection<FileInfo>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //PreviousLogs

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

        #region VehicleTotals
        public ObservableCollection<SimpleTripInfo> VehicleTotals
        {
            get { return (ObservableCollection<SimpleTripInfo>)GetValue(VehicleTotalsProperty); }
            set { SetValue(VehicleTotalsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VehicleTotals.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VehicleTotalsProperty =
            DependencyProperty.Register("VehicleTotals", typeof(ObservableCollection<SimpleTripInfo>), typeof(MainWindow), new PropertyMetadata(null));
        #endregion //VehicleTotals

        public MainWindow()
        {
            Trips = new ObservableCollection<SimpleTripInfo>();
            PreviousLogs = new ObservableCollection<FileInfo>();
            Destinations = new ObservableCollection<string>();
            VehicleTotals = new ObservableCollection<SimpleTripInfo>();

            InitializeComponent();
            DataContext = this;
            ReadDestinationsFile();
            ReadTripsFile();
            ReadVehiclesFile();
            ReadPreviousLogsFiles();

            Trip = new TripInfo();
            Trip.PropertyChanged += Trip_PropertyChanged;
            LoadAppSettings();
        }

        private void LoadAppSettings()
        {
            var s = Settings.Default;
            Trip.Destination = s.Destination;
            Trip.Vehicle = Vehicles.FirstOrDefault(v => v.Name == s.Vehicle);
        }

        private void Trip_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if  (e.PropertyName == "Destination")
            {
                if (!string.IsNullOrWhiteSpace(Trip.Destination) && Destinations.Contains(Trip.Destination) == false)
                {
                    Destinations.Add(Trip.Destination);
                    SaveAppSettings();
                    var jsonString = JsonConvert.SerializeObject(Destinations.ToList(), new JsonSerializerSettings() { Formatting = Formatting.Indented });
                    File.WriteAllText("Destinations.txt", jsonString);
                    ReadDestinationsFile();
                    LoadAppSettings();
                }
            }
        }

        private void ReadPreviousLogsFiles()
        {
            PreviousLogs.Clear();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Mileage Log*");
            foreach (var file in files)
            {
                PreviousLogs.Add(new FileInfo(file));
            }
        }

        private void ReadDestinationsFile()
        {
            try
            {
                Destinations.Clear();
                var dests = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("Destinations.txt"));
                dests.Sort();
                dests.Where(d => !String.IsNullOrWhiteSpace(d)).ToList().ForEach(d => Destinations.Add(d));
                if (Destinations.Count < 1)
                    throw new InvalidDataException("No destinations!");
            }
            catch (Exception)
            {
                Destinations.Add("ABC");
                Destinations.Add("Martins");
                Destinations.Add("Other");
            }
        }

        private void ReadTripsFile()
        {
            try
            {
                Trips.Clear();
                JsonConvert.DeserializeObject<List<SimpleTripInfo>>(File.ReadAllText("Trips.txt")).ForEach(t => Trips.Add(t));
                if (Trips.Count < 1)
                {
                    LogDescription = "No Trips Logged.";
                }
                else
                {
                    var firstDate = Trips.Min(t => Convert.ToDateTime(t.Date));
                    var lastDate = Trips.Max(t => Convert.ToDateTime(t.Date));
                    LogDescription = string.Format("Create Log for:\n{0} trips totalling {1} miles\nfrom {2} to {3}",
                        Trips.Count,
                        Trips.Sum(t => t.Distance),
                        firstDate.ToShortDateString(),
                        lastDate.ToShortDateString()
                        );
                }
            }
            catch (Exception)
            {
            }
        }

        private void ReadVehiclesFile()
        {
            try
            {
                Vehicles = new List<Vehicle>();
                JsonConvert.DeserializeObject<List<Vehicle>>(File.ReadAllText("Vehicles.txt")).ForEach(v => Vehicles.Add(v));
            }
            catch (Exception)
            {
                Vehicles = new List<Vehicle>();
                Vehicles.Add(new Vehicle() { Name = "Buick", Odometer = 204615 });
                Vehicles.Add(new Vehicle() { Name = "Montanna", Odometer = 146000 });
                Vehicles.Add(new Vehicle() { Name = "Volvo", Odometer = 150000 });
            }
        }

        private void SaveAppSettings()
        {
            var s = Settings.Default;
            s.Destination = Trip.Destination;
            if (Trip.Vehicle != null)
                s.Vehicle = Trip.Vehicle.Name;
            s.Save();
        }

        private void WriteVehiclesFile()
        {
            var jsonString = JsonConvert.SerializeObject(Vehicles, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.WriteAllText("Vehicles.txt", jsonString);
        }

        private void SaveTrip_Click(object sender, RoutedEventArgs e)
        {
            var newTrip = new SimpleTripInfo()
            {
                Destination = Trip.Destination,
                Vehicle = Trip.Vehicle.Name,
                Date = DateTime.Now.ToShortDateString(),
                Start = Trip.Start.Miles,
                End = Trip.End.Miles,
                Distance = Trip.TotalMiles
            };
            Trips.Add(newTrip);

            Trip.Start.Miles = Trip.End.Miles;
            SaveAppSettings();
            WriteTripsFile();
            WriteVehiclesFile();

            ReadTripsFile();
            LoadAppSettings();
        }

        private void WriteTripsFile()
        {
            var jsonString = JsonConvert.SerializeObject(Trips, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.WriteAllText("Trips.txt", jsonString);
        }

        private void CreateLog_Click(object sender, RoutedEventArgs e)
        {
            if (Trips.Count < 1)
            {
                MessageBox.Show("Save some trips before creating log file.");
                return;
            }
            var firstDate = Trips.Min(t => Convert.ToDateTime(t.Date));
            var lastDate = Trips.Max(t => Convert.ToDateTime(t.Date));
            var totalMiles = Trips.Sum(t => t.Distance);

            var jsonString = JsonConvert.SerializeObject(
                new {
                    Miles = totalMiles,
                    From = firstDate,
                    To = lastDate,
                    Details = Trips
                }, 
                new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var fileName = String.Format("Mileage Log from {0} to {1} ({2} Miles).txt", 
                firstDate.ToString("yyyy-MM-dd"), 
                lastDate.ToString("yyyy-MM-dd"), 
                totalMiles);
            File.WriteAllText(fileName, jsonString);

            // Start a new process for explorer
            // in this location     
            ProcessStartInfo l_psi = new ProcessStartInfo();
            l_psi.FileName = "explorer";
            l_psi.Arguments = string.Format("/select,{0}", fileName);

            Process l_newProcess = new Process();
            l_newProcess.StartInfo = l_psi;
            l_newProcess.Start();
            Trips.Clear();
            WriteTripsFile();
            ReadTripsFile();
            ReadPreviousLogsFiles();
        }

        private void PrevLogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VehicleTotals.Clear();
            var lv = sender as ListView;

            var logs = new List<SimpleTripInfo>();

            foreach (FileInfo log in lv.SelectedItems)
            {
                JsonConvert.DeserializeObject<List<SimpleTripInfo>>(JObject.Parse(File.ReadAllText(log.FullName)).SelectToken("Details").ToString()).ForEach(t => logs.Add(t));
            }

            (from t in logs
             group t by t.Vehicle into v
             select v).ToList().ForEach(v => VehicleTotals.Add(
                 new SimpleTripInfo()
                 {
                     Vehicle = v.First().Vehicle,
                     Distance = v.Sum(t => t.Distance)
                 }));
        }
    }
}
