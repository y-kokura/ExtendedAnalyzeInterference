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
        private ObservableCollection<ComponentData> _componentDatas;

        public ObservableCollection<ComponentData> ComponentDatas
        {
            get { return _componentDatas; }
            set { SetProperty(ref _componentDatas, value); }
        }

        public DelegateCommand<object> RowDoubleClickCommand { get; private set; }

        public ResultWindowViewModel(ObservableCollection<ComponentData> data)
        {
            ComponentDatas = data;
        }

        public ResultWindowViewModel()
        {
            try {
                ComponentDatas = new ObservableCollection<ComponentData>();
                RowDoubleClickCommand = new DelegateCommand<object>(RowDoubleClicked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void RowDoubleClicked(object selectedItem)
        {
            if (selectedItem is ComponentData componentData)
            {
                MessageBox.Show($"Name: {componentData.ComponentOccurrence.Name}");
            }
        }
    }
}

