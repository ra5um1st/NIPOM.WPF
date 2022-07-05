using Microsoft.Win32;
using NIPOM.WPF.Commands;
using NIPOM.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NIPOM.WPF.ViewModels
{
    internal class ComponentsViewModel : ObservableObject
    {
        public ComponentsViewModel()
        {
            ElectricalComponents = new ObservableCollection<ObservableElComponent>();
           
            OpenFileExcel = new DelegateCommandAsync(OnOpenFileExcelExecute);
            OpenFileXML = new DelegateCommand(OnOpenFileXMLExecute);
            SaveFileXML = new DelegateCommand(OnSaveFileXMLExecute);
        }

        private ObservableCollection<ObservableElComponent> electricalComponents;
        public ObservableCollection<ObservableElComponent> ElectricalComponents
        {
            get => electricalComponents;
            set => Set(ref electricalComponents, value);
        }

        #region Open Excel File Command

        public ICommand OpenFileExcel { get; }
        private async Task OnOpenFileExcelExecute(object obj)
        {
            string sheetName = "Лист1";

            OpenFileDialog fileDialog = new OpenFileDialog() { Filter = " Excel files | *.xlsx" };
            if(fileDialog.ShowDialog() == false)
            {
                return;
            }

            ElectricalComponents.Clear();
            DataTable data = await ReadExcelAsync(fileDialog.FileName, sheetName);
            var resultCollection = data.AsEnumerable();

            foreach (var item in resultCollection)
            {
                try
                {
                    ElectricalComponents.Add(new ObservableElComponent(new ElComponent() 
                    {
                        Name = item.Field<string>(0),
                        Manufacturer = item.Field<string>(1),
                        Category = item.Field<string>(2),
                        Price = item.Field<double>(3),
                        Count = item.Field<double>(4),
                    }));
                }
                catch (Exception)
                {
                    MessageBox.Show($"Не удалось открыть Excel файл. Файл содержит некорректные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        #endregion

        #region Open XML File Command

        public ICommand OpenFileXML { get; }
        private void OnOpenFileXMLExecute(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog() { Filter = " XML files | *.xml" };
            if (fileDialog.ShowDialog() == false)
            {
                return;
            }

            var result = ReadXML<ObservableCollection<ObservableElComponent>>(fileDialog.FileName);
            if(result != null)
            {
                ElectricalComponents = result;
            }
        }

        #endregion

        #region Save XML File Command

        public ICommand SaveFileXML { get; }
        private void OnSaveFileXMLExecute(object obj)
        {
            SaveFileDialog fileDialog = new SaveFileDialog() { Filter = " XML files | *.xml" };
            if (fileDialog.ShowDialog() == false)
            {
                return;
            }

            CreateXML(fileDialog.FileName, ElectricalComponents);
        }

        #endregion

        #region Private Methods

        private async Task<DataTable> ReadExcelAsync(string path, string sheetName)
        {
            OleDbConnectionStringBuilder connectionBuilder = new OleDbConnectionStringBuilder();
            connectionBuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
            connectionBuilder.DataSource = path;
            connectionBuilder.PersistSecurityInfo = true;
            connectionBuilder.Add("Extended Properties", "Excel 12.0 Xml;");

            DataTable data = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(connectionBuilder.ConnectionString))
            {
                OleDbCommand readCommand = new OleDbCommand($"select * from [{sheetName}$]", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(readCommand);

                await connection.OpenAsync();
                adapter.Fill(data);
                await connection.CloseAsync();
            }
            data.Rows.RemoveAt(0);

            return data;
        }
        private T ReadXML<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None, 4096, true);
            T result = default;
            try
            {
                result = (T)serializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось прочитать файл: {path}.\n\n{e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return result;
        }
        private void CreateXML<T>(string path, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            try
            {
                serializer.Serialize(stream, obj);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось создать файл: {path}.\n\n{e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
