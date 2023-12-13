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



namespace AnalyzeInterference.ViewModels
{
    internal class ResultWindowViewModel : BindableBase
    {
        private ObservableCollection<ComponentData> _componentData;
        private ComponentData _selectedComponent;

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

        public DelegateCommand<object> DataGridDoubleClickCommand { get; private set; }

        public ResultWindowViewModel()
        {
            try
            {
                ComponentData = new ObservableCollection<ComponentData>();
                DataGridDoubleClickCommand = new DelegateCommand<object>(DataGridDoubleClicked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ResultWindowViewModel(ObservableCollection<ComponentData> data)
        {
            ComponentData = data;
            DataGridDoubleClickCommand = new DelegateCommand<object>(DataGridDoubleClicked);
        }

        private void DataGridDoubleClicked(object selectedItem)
        {
            if (selectedItem is ComponentData componentData)
            {
                MessageBox.Show($"Name: {componentData.ComponentOccurrence.Name}");
            }
        }
    }

}

