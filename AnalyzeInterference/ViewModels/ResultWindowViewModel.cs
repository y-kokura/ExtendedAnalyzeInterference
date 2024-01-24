using AnalyzeInterference.Views;
using AnalyzeInterference.Models;
using Inventor;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows.Controls;



namespace AnalyzeInterference.ViewModels
{
    internal class ResultWindowViewModel : BindableBase
    {
        private ObservableCollection<ComponentData> _componentData;
        private ComponentData _selectedComponent;
        public DelegateCommand<object> DataGridRowDoubleClickCommand { get; private set; }


        public DelegateCommand<object> CellDoubleClickCommand { get; private set; }

        public ObservableCollection<ComponentData> ComponentData
        {
            get { return _componentData; }
            set { SetProperty(ref _componentData, value); }
        }

        public ComponentData SelectedComponent
        {
            get { return _selectedComponent; }
            set { SetProperty(ref _selectedComponent, value); }
        }



        public ResultWindowViewModel()
        {
            

            try
            {
                ComponentData = new ObservableCollection<ComponentData>();
                DataGridRowDoubleClickCommand = new DelegateCommand<object>(ExecuteRowDoubleClick);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ResultWindowViewModel(ObservableCollection<ComponentData> data)
        {
            ComponentData = data;
        }

        private void ExecuteRowDoubleClick(object selectedItem)
        {
            if (selectedItem is ComponentData componentData)
            {
                MessageBox.Show($"Name: {componentData.ComponentOccurrence.Name}");
            }
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
                if (row != null)
                {
                    var cellContent = row.Item; // ここで選択された行のデータを取得します。
                    MessageBox.Show(cellContent.ToString()); // MessageBoxでデータを表示します。
                }
            }
        }

    }

}

