using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AnalyzeInterference.ViewModels
{
    internal class AnalyzeControlViewModel: BindableBase
    {
        public DelegateCommand StartAnalysisCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }


        private bool _check_BOMkPhantom;

        public bool Check_BOMkPhantom
        {
            get => _check_BOMkPhantom;
            set => SetProperty(ref _check_BOMkPhantom, value);
        }



        public AnalyzeControlViewModel()
        {
            StartAnalysisCommand = new DelegateCommand(ExecuteStartAnalysis);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        private void ExecuteStartAnalysis()
        {
            throw new NotImplementedException();
        }
        public void ExecuteCancel()
        {
            throw new NotImplementedException();
        }
    }

}
