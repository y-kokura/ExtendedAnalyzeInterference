using AnalyzeInterference.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AnalyzeInterference
{
    public class AnalyzeInterferenceModule : IModule
    {

        public class ComponentData
        {
            public string InterferenceCountType { get; set; }
        }


        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}