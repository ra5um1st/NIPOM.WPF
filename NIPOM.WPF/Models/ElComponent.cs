using NIPOM.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIPOM.WPF.Models
{
    [Serializable]
    public class ElComponent
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public double Sum => Count * Price;
    }
}
