using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileageTracker.Types
{
    public class TripInfo : BaseInfo
    {
        #region Destination
        /// <summary>
        /// Store for the Destination property.</summary>
        private String _Destination;
        /// <summary>
        /// Gets or sets Destination property.</summary>
        public String Destination
        {
            get { return _Destination; }
            set
            {
                if (_Destination == value) return;
                _Destination = value;
                OnPropertyChanged("Destination");
            }
        }
        #endregion //Destination

        #region End
        /// <summary>
        /// Store for the End property.</summary>
        private OdometerInfo _End;
        /// <summary>
        /// Gets or sets End property.</summary>
        public OdometerInfo End
        {
            get
            {
                if (_End == null)
                {
                    _End = new OdometerInfo();
                    _End.PropertyChanged += _End_PropertyChanged;
                }
                return _End;
            }
        }

        private void _End_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
                OnPropertyChanged("TotalMiles");
        }
        #endregion //End

        #region Start
        /// <summary>
        /// Store for the Start property.</summary>
        private OdometerInfo _Start;
        /// <summary>
        /// Gets or sets Start property.</summary>
        public OdometerInfo Start
        {
            get
            {
                if (_Start == null)
                {
                    _Start = new OdometerInfo();
                    _Start.PropertyChanged += _Start_PropertyChanged;
                }
                return _Start;
            }
        }

        private void _Start_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Miles" && Start.Miles > End.Miles)
                End.Miles = Start.Miles;
            else
                OnPropertyChanged("TotalMiles");
        }
        #endregion //Start

        #region TotalMiles
        /// <summary>
        /// Gets Total Miles for trip.</summary>
        public int TotalMiles
        {
            get { return _End.Miles - _Start.Miles; }
        }
        #endregion //TotalMiles

        #region Vehicle
        /// <summary>
        /// Store for the Vehicle property.</summary>
        private String _Vehicle;
        /// <summary>
        /// Gets or sets Vehicle property.</summary>
        public String Vehicle
        {
            get { return _Vehicle; }
            set
            {
                if (_Vehicle == value) return;
                _Vehicle = value;
                OnPropertyChanged("Vehicle");
            }
        }
        #endregion //Vehicle

    }
}
