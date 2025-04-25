using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.Models
{
    public class LoadSimulationSettings
    {
        public int UserCount { get; set; }
        public int DurationInSeconds { get; set; }
    }
}
