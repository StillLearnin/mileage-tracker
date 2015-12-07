using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileageTracker.Types
{
    public class SimpleTripInfo
    {
        public String Destination { get; set; }
        public String Vehicle { get; set; }
        public String Date { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Distance { get; set; }
    }
}
