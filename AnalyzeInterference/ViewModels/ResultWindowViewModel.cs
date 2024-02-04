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
        public DelegateCommand<object> RowDoubleClickCommand { get; private set; }


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
                RowDoubleClickCommand = new DelegateCommand<object>(OnRowDoubleClick);

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

        public void OnRowDoubleClick(object parameter)
        {

            MessageBox.Show("a");
            var item = parameter as ComponentData; // YourItemTypeは、DataGridの項目の型です
            if (item != null)
            {
                // ダブルクリックされた項目に対する処理
                MessageBox.Show(item.ComponentOccurrence.Name);
            }

        }

    }

}

