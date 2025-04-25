using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.Models
{
    public class PerformanceTestSettings
    {
        public int WarmUpDurationInSeconds { get; set; }
        public LoadSimulationSettings LoadSimulation { get; set; }
    }
}
