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



namespace AnalyzeInterference.ViewModels
{
    public class ResultWindowViewModel : BindableBase
    {
        private readonly ResultData _model;
        private ObservableCollection<string> _displayData;

        public ObservableCollection<string> DisplayData
        {
            get => _displayData;
            set => SetProperty(ref _displayData, value);
        }

        public ResultWindowViewModel(ResultData model)
        {
            _model = model;
            DisplayData = new ObservableCollection<string>(_model.DataList);
        }

        // 何らかのコマンドまたはメソッドを通じて、Model のデータを操作
        public void AddNewData(string newData)
        {
            _model.AddData(newData);
            DisplayData.Add(newData);
        }
    }

}
