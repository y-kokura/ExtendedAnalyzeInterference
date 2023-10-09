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


namespace AnalyzeInterference.ViewModels
{
    internal class StartWindowViewModel : BindableBase
    {
        public DelegateCommand StartAnalysisCommand { get;  }
        public DelegateCommand CancelCommand { get;}


        #region"Selection of Interference Analysis Target"
        public bool AllComponent { get; set; }
        public bool SelectedComponent { get; set; }
        #endregion

        #region "CheckBox Property"

        public bool kReferenceBOM { get; set; }
        public bool kPhantomBOM { get; set; }
        public bool Disable { get; set; }
        public bool Hidden { get; set; }
        #endregion



        public StartWindowViewModel()
        {
            // 初期値を設定
            AllComponent = true;
            SelectedComponent = false;
            Disable = true;
            Hidden = true;
            kReferenceBOM = true;
            kPhantomBOM = false;

            // コマンドの初期化
            StartAnalysisCommand = new DelegateCommand(ExecuteStartAnalysis);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }



        private void ExecuteStartAnalysis()
        {
            UpdateModel(AnalyzeLogic.Instance);
            AnalyzeLogic.Instance.RunAnalysis();
        }
        public void ExecuteCancel()
        {
            return;
            //System.Windows.Application.Current.Shutdown();
        }

        public void UpdateModel(AnalyzeLogic model){

            model.AllComponent = AllComponent;
            model.SelectedComponent = SelectedComponent;
            model.kReferenceBOM = kReferenceBOM;
            model.kPhantomBOM = kPhantomBOM;
            model.Disable = Disable;
            model.Hidden = Hidden;
        }


    }

}
