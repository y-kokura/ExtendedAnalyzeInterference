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
using System.IO;
using System.Reflection;


namespace AnalyzeInterference.ViewModels
{
    internal class StartWindowViewModel : BindableBase
    {
        // シングルトンのインスタンスを作成
        private static StartWindowViewModel _instance;
        public static StartWindowViewModel Instance => _instance ?? (_instance = new StartWindowViewModel());


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


            //AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            //{
            //    if (args.Name.Contains("Microsoft.Xaml.Behaviors"))
            //    {
            //        string assemblyPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "特定のパス", "Microsoft.Xaml.Behaviors.dll");
            //        return Assembly.LoadFrom(assemblyPath);
            //    }
            //    return null;
            //};

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            // あなたのアプリケーションの初期化や実行のコード
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // 実行中のアセンブリのフルパスを取得
            string executablePath = Assembly.GetExecutingAssembly().Location;
            // 実行中のアセンブリのディレクトリパスを取得
            string executableDirectory = System.IO.Path.GetDirectoryName(executablePath);

            // 要求されたアセンブリのファイル名を解析
            string assemblyName = new AssemblyName(args.Name).Name + ".dll";
            // 実行中のアセンブリと同じディレクトリからアセンブリを検索
            string assemblyPath = System.IO.Path.Combine(executableDirectory, assemblyName);

            if (System.IO.File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }

            return null; // アセンブリが見つからない場合はnullを返す
        }





        private void ExecuteStartAnalysis()
        {
            UpdateModel(ComponentOccurrenceCollection.Instance);
            ThreadInterferenceAnalysisWorkFlow threadInterferenceAnalysisWorkFlow = new ThreadInterferenceAnalysisWorkFlow();
            threadInterferenceAnalysisWorkFlow.RunInterferenceAnalysis();

        }
        private void ExecuteCancel()
        {
            return;
            //System.Windows.Application.Current.Shutdown();
        }

        public void UpdateModel(ComponentOccurrenceCollection model){

            model.AllComponent = AllComponent;
            model.SelectedComponent = SelectedComponent;
            model.kReferenceBOM = kReferenceBOM;
            model.kPhantomBOM = kPhantomBOM;
            model.Disable = Disable;
            model.Hidden = Hidden;
        }


    }

}
