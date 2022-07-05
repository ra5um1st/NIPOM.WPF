using NIPOM.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIPOM.WPF.Models
{
    [Serializable]
    public class ObservableElComponent : ObservableObject
    {
        public ObservableElComponent()
        {
            this.component = new ElComponent();
        }
        public ObservableElComponent(ElComponent component)
        {
            this.component = component;
        }

        private ElComponent component;

        private string name;
        public string Name
        {
            get => component.Name;
            set
            {
                if (Set(ref name, value))
                {
                    component.Name = value;
                }
            }
        }

        private string manufacturer;
        public string Manufacturer
        {
            get => component.Manufacturer;
            set
            {
                if (Set(ref manufacturer, value))
                {
                    component.Manufacturer = value;
                }
            }
        }

        private string category;
        public string Category
        {
            get => component.Category;
            set
            {
                if (Set(ref category, value))
                {
                    component.Category = value;
                }
            }
        }

        private double price;
        public double Price
        {
            get => component.Price;
            set
            {
                if (Set(ref price, value))
                {
                    component.Price = value;
                    OnPropertyChanged(nameof(Sum));
                }
            }
        }

        private double count;
        public double Count
        {
            get => component.Count;
            set
            {
                if(Set(ref count, value))
                {
                    component.Count = value;
                    OnPropertyChanged(nameof(Sum));
                }
            }
        }

        public double Sum => Count * Price;
    }
}
