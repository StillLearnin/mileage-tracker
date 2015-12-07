using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileageTracker.Types
{
    public class OdometerInfo : BaseInfo
    {
        #region Miles
        /// <summary>
        /// Store for the Miles property.</summary>
        private Int32 _Miles;
        /// <summary>
        /// Gets or sets Miles property.</summary>
        public Int32 Miles
        {
            get
            {
                return _Miles;
            }
            set
            {
                if (_Miles == value) return;
                _Miles = value;
                OnPropertyChanged("HundredThousands");
                OnPropertyChanged("TenThousands");
                OnPropertyChanged("OneThousands");
                OnPropertyChanged("Hundreds");
                OnPropertyChanged("Tens");
                OnPropertyChanged("Ones");
                OnPropertyChanged("Miles");
            }
        }

        private int getMilesPart(int place)
        {
            return _Miles / place % 10;
        }
        private void setMilesPart(int value, int place, string propertyName)
        {
            var curVal = getMilesPart(place) * place;
            var newVal = value * place;
            if (Miles - curVal + newVal < 0)
                return;
            Miles = Miles - curVal + newVal;
            OnPropertyChanged(propertyName);
        }
        #endregion //Miles

        #region HundredThousands
        /// <summary>
        /// Gets or sets HundredThousands property.</summary>
        public Int32 HundredThousands
        {
            get { return getMilesPart(100000); }
            set { setMilesPart(value, 100000, "HundredThousands"); }
        }

        #endregion //HundredThousands

        #region TenThousands
        /// <summary>
        /// Gets or sets TenThousands property.</summary>
        public Int32 TenThousands
        {
            get { return getMilesPart(10000); }
            set { setMilesPart(value, 10000, "TenThousands"); }
        }
        #endregion //TenThousands

        #region OneThousands
        /// <summary>
        /// Gets or sets OneThousands property.</summary>
        public Int32 OneThousands
        {
            get { return getMilesPart(1000); }
            set { setMilesPart(value, 1000, "OneThousands"); }
        }
        #endregion //OneThousands

        #region Hundreds
        /// <summary>
        /// Gets or sets Hundreds property.</summary>
        public Int32 Hundreds
        {
            get { return getMilesPart(100); }
            set { setMilesPart(value, 100, "Hundreds"); }
        }
        #endregion //Hundreds

        #region Tens
        /// <summary>
        /// Gets or sets Tens property.</summary>
        public Int32 Tens
        {
            get { return getMilesPart(10); }
            set { setMilesPart(value, 10, "Tens"); }
        }
        #endregion //Tens

        #region Ones
        /// <summary>
        /// Gets or sets Ones property.</summary>
        public Int32 Ones
        {
            get { return getMilesPart(1); }
            set { setMilesPart(value, 1, "Ones"); }
        }
        #endregion //Ones

    }
}
