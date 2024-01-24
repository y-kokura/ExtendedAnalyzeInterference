using Inventor;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using AnalyzeInterference.Views;
using AnalyzeInterference.ViewModels;

namespace ExtendedAnalyzeInterference
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute(Globals.SimpleAddInClientID)]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {
        // クラスレベルでの変数宣言
        private ButtonDefinition createdAddInButton;
        private UserInterfaceEvents uiEvents;
        private static Inventor.Application InvApp;

        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            // This method is called by Inventor when it loads the addin.
            // The AddInSiteObject provides access to the Inventor Application object.
            // The FirstTime flag indicates if the addin is loaded for the first time.

            // Initialize AddIn members.
            //Globals.InvApp = addInSiteObject.Application;

            // TODO: Add ApplicationAddInServer.Activate implementation.
            // e.g. event initialization, command creation etc.

            try
            {
                InvApp = addInSiteObject.Application;

                uiEvents = InvApp.UserInterfaceManager.UserInterfaceEvents;

                // イベントハンドラの設定
                uiEvents.OnResetRibbonInterface += new UserInterfaceEventsSink_OnResetRibbonInterfaceEventHandler(OnResetRibbonInterface);

                // Create Button Definition
                createdAddInButton = Utilities.CreateButtonDefinition(InvApp,"ExtendedAnalyzeInterference", "ExtendedAnalyzeInterference", "", "ButtonResources");
                createdAddInButton.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(OnAddInButtonExecute);

                if (firstTime)
                {
                    AddToUserInterface();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected failure in the activation of the add-in \"ExtendedAnalyzeInterference\"/n" + ex.Message);
            }
        }

        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation

            // Release objects.
            createdAddInButton = null;
            uiEvents = null;
            InvApp = null;
            try { createdAddInButton.OnExecute -= new ButtonDefinitionSink_OnExecuteEventHandler(OnAddInButtonExecute); } catch { }
            

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        private void OnResetRibbonInterface(NameValueMap Context)
        {
            AddToUserInterface();
        }

        private void OnAddInButtonExecute(NameValueMap Context)
        {

            AnalyzeInterference.Common.Globals.InvApp = InvApp;

            var startWindow = new StartWindow();
            startWindow.Show();



        }

        private void AddToUserInterface()
        {

            foreach (Ribbon ribbon in InvApp.UserInterfaceManager.Ribbons)
            {
                RibbonTab ribbonTab;
                try
                {
                    ribbonTab = ribbon.RibbonTabs["AddIn"];
                }
                catch
                {
                    ribbonTab = ribbon.RibbonTabs.Add("アドイン", "AddIn", Globals.AddInClientID);
                }

                RibbonPanel customPanel;
                try
                {
                    customPanel = ribbonTab.RibbonPanels["others"];
                }
                catch
                {
                    customPanel = ribbonTab.RibbonPanels.Add("その他", "others", Globals.AddInClientID);
                }

                if (createdAddInButton != null)
                {
                    customPanel.CommandControls.AddButton(createdAddInButton);
                }
            }
        }
        #endregion
    }

    internal class Globals
    {
        public const string SimpleAddInClientID = "4571000a-b104-4c29-9f85-04d03e740459";
        public const string AddInClientID = "{" + SimpleAddInClientID + "}";
    }

}

