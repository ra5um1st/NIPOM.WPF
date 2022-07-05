using NIPOM.WPF.Commands;
using NIPOM.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace NIPOM.WPF.ViewModels
{
    internal class ComponentsViewModel : ObservableObject
    {
        public ComponentsViewModel()
        {
            electricalComponents = new List<ElectricalComponent>();
            electricalComponentsSource = new CollectionViewSource() { Source = electricalComponents };

            OpenFileExcel = new DelegateCommand(OnOpenFileExcelExecute);
            OpenFileXML = new DelegateCommand(OnOpenFileXMLExecute);
            SaveFileXML = new DelegateCommand(OnSaveFileXMLExecute);
        }

        private List<ElectricalComponent> electricalComponents;
        private CollectionViewSource electricalComponentsSource;

        public ICollectionView ElectricalComponentsView => electricalComponentsSource.View;

        public ICommand OpenFileExcel { get; }
        private void OnOpenFileExcelExecute(object obj)
        {

        }

        public ICommand OpenFileXML { get; }
        private void OnOpenFileXMLExecute(object obj)
        {
            throw new NotImplementedException();
        }

        public ICommand SaveFileXML { get; }
        private void OnSaveFileXMLExecute(object obj)
        {
            throw new NotImplementedException();
        }

    }
}
