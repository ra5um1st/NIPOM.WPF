using System;
using System.Collections.Generic;
using System.Text;

namespace NIPOM.WPF.Models
{
    [Serializable]
    internal class ElectricalComponent
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public uint Count { get; set; }
        public string Sum { get; set; }

    }
}
