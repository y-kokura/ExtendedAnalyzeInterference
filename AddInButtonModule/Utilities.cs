using Inventor;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web;
using System.Windows.Forms;

namespace ExtendedAnalyzeInterference
{
    public class Utilities
    {
        #region Simplified Button Creation
        public static ButtonDefinition CreateButtonDefinition(Inventor.Application InvApp,string DisplayName, string InternalName, string ToolTip, string IconFolder)
        {
            ButtonDefinition testDef = null;
            try
            {
                testDef = (ButtonDefinition)InvApp.CommandManager.ControlDefinitions[InternalName];
            }
            catch (Exception)
            {
            }

            if (testDef != null)
            {
                MessageBox.Show("Error when loading the add-in \"ExtendedAnalyzeInterference\". A command already exists with the same internal name. Each add-in must have a unique internal name. Change the internal name in the call to CreateButtonDefinition.",
                                "Inventor Add-In Template",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                return null;
            }

            if (!string.IsNullOrEmpty(IconFolder))
            {
                if (!System.IO.Directory.Exists(IconFolder))
                {
                    string dllPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    IconFolder = System.IO.Path.Combine(dllPath, IconFolder);
                }
            }

            IPictureDisp iPicDisp16x16 = null;
            IPictureDisp iPicDisp32x32 = null;

            if (!string.IsNullOrEmpty(IconFolder) && System.IO.Directory.Exists(IconFolder))
            {
                string fileExtension = ".bmp";
                string filename16x16 = System.IO.Path.Combine(IconFolder, "16x16" + fileExtension);
                string filename32x32 = System.IO.Path.Combine(IconFolder, "32x32" + fileExtension);

                if (System.IO.File.Exists(filename16x16))
                {
                    try
                    {
                        System.Drawing.Bitmap image16x16 = new System.Drawing.Bitmap(filename16x16);
                        iPicDisp16x16 = (IPictureDisp)IPictureDispHost.GetIPictureDisp(image16x16);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

                if (System.IO.File.Exists(filename32x32))
                {
                    try
                    {
                        System.Drawing.Bitmap image32x32 = new System.Drawing.Bitmap(filename32x32);
                        iPicDisp32x32 = (IPictureDisp)IPictureDispHost.GetIPictureDisp(image32x32);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }


            }

            try
            {
                ControlDefinitions controlDefs = InvApp.CommandManager.ControlDefinitions;
                ButtonDefinition btnDef = controlDefs.AddButtonDefinition(DisplayName,
                                                                         InternalName,
                                                                         CommandTypesEnum.kShapeEditCmdType,
                                                                         Globals.AddInClientID,
                                                                         "",
                                                                         ToolTip,
                                                                         iPicDisp16x16,
                                                                         iPicDisp32x32);
                return btnDef;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region hWnd Wrapper Class
        public class WindowWrapper : IWin32Window
        {
            private IntPtr _hwnd;

            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }

            public IntPtr Handle
            {
                get { return _hwnd; }
            }
        }
        #endregion

        internal class IPictureDispHost : AxHost
        {

            public IPictureDispHost() : base("")
            {
            }

            public static object GetIPictureDisp(Image Image)
            {
                try
                {
                    return IPictureDispHost.GetIPictureDispFromPicture(Image);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

    }
}