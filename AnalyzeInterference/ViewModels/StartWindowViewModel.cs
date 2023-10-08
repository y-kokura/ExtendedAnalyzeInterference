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


    internal class StartWindowViewModel: BindableBase
    {
        public DelegateCommand StartAnalysisCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        #region "RadioButton Property"
        private bool _allComponent;
        public bool AllComponent
        {
            get => _allComponent;
            set => SetProperty(ref _allComponent, value);
        }

        private bool _selectedComponent;
        public bool SelectedComponent
        {
            get => _selectedComponent;
            set => SetProperty(ref _selectedComponent, value);
        }

        #endregion

        #region "CheckBox Property"
        private bool _kReferenceBOM;
        public bool kReferenceBOM
        {
            get => _kReferenceBOM;
            set => SetProperty(ref _kReferenceBOM, value);
        }

        private bool _kPhantomBOM;
        public bool kPhantomBOM
        {
            get => _kPhantomBOM;
            set => SetProperty(ref _kPhantomBOM, value);
        }

        private bool _disable;
        public bool Disable
        {
            get => _disable;
            set => SetProperty(ref _disable, value);
        }

        private bool _hidden;
        public bool Hidden
        {
            get => _hidden;
            set => SetProperty(ref _hidden, value);
        }
        #endregion

        public StartWindowViewModel()
        {
            // 初期値を設定
            AllComponent=true;
            SelectedComponent = false;
            Disable = true;
            Hidden = true;
            kReferenceBOM= true;
            kPhantomBOM = false;

            // コマンドの初期化
            StartAnalysisCommand = new DelegateCommand(ExecuteStartAnalysis);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }



        private void ExecuteStartAnalysis()
        {

        }
        public void ExecuteCancel()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

}
