#region Namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace Smart_Design_Plug_in_Updates
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            AddSplitButton(a);
            a.ApplicationClosing += a_ApplicationClosing;
            a.Idling += a_Idling;
            return Result.Succeeded;
        }



        private void AddSplitButton(UIControlledApplication a)
        {
            #region Import Images
            Image SmartPluginImg = Properties.Resources.Smart_Plugin;
            Image AddTableImg = Properties.Resources.Add_Table;
            Image DataBaseImg = Properties.Resources.Database;
            ImageSource SmartPluginImgsc = GetImageSource(SmartPluginImg);
            ImageSource AddTableImgsc = GetImageSource(AddTableImg);
            ImageSource DataBaseImgsc = GetImageSource(DataBaseImg);
            #endregion

            #region Pannel
            RibbonPanel panel = ribbonpanel(a);
            #endregion

            #region Assembly 
            string assembly = Assembly.GetExecutingAssembly().Location;
            #endregion
            #region create push buttons for split button drop down

            #region Add table button
            PushButtonData AddTableButton = new PushButtonData("Add Table", "Create Schedule",
             assembly, "Smart_Design_Plug_in_Updates.Command");
            AddTableButton.Image = AddTableImgsc;
            AddTableButton.LargeImage = AddTableImgsc;
            AddTableButton.ToolTip = "Creates schdule with the selected data";
            AddTableButton.LongDescription = "Creates a dummy schedule with the data the user have selected and later this schedule could be used either to extract the data from the file maker database " +
                "and add it to the dummy schedule or add the data in the dummy schedule to the file maker database";
            #endregion

            #region Add file maker button
            PushButtonData FileMakerButton = new PushButtonData("FileMakerButton", "Synchronize",
                    assembly, "Smart_Design_Plug_in_Updates.SynchronizeFileMaker");
            FileMakerButton.Image = DataBaseImgsc;
            FileMakerButton.LargeImage = DataBaseImgsc;
            AddTableButton.ToolTip = "Synchronize data with filemaker database";
            AddTableButton.LongDescription = "This button enable either to extract the data from the file maker database from the specified project and add it to dummy schedule" +
                " or create new items in the file maker project using the existing items in the smart schedule "+
                "or update the project items using the existing items in the smart schedule";
            #endregion

            PulldownButtonData sb1 = new PulldownButtonData("Smart Schedule", "Smart Schedule");
            PulldownButton sb = panel.AddItem(sb1) as PulldownButton;
            sb.Image = SmartPluginImgsc;
            sb.LargeImage = SmartPluginImgsc;
            sb.AddPushButton(AddTableButton);
            sb.AddPushButton(FileMakerButton);
            #endregion
        }

        private BitmapSource GetImageSource(Image img)
        {
            BitmapImage bmp = new BitmapImage();

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;
                bmp.EndInit();
            }
            return bmp;
        }

        public RibbonPanel ribbonpanel(UIControlledApplication a)
        {
            string tab = "Smart Design Plugins";
            RibbonPanel ribbonpanel = null;
            //create tab
            try
            {
                a.CreateRibbonTab(tab);

            }
            catch { }
            //create panel  
            try
            {
                //a.createRibbonPanel(Tab Name, Panel Name)
                RibbonPanel panel = a.CreateRibbonPanel(tab, "Schedules");
            }
            catch { }
            //check if panel exist
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels)
            {
                //check if the pannel exist if it exist return the pannel if not return the new pannel
                if (p.Name == "Schedules")
                {
                    ribbonpanel = p;
                    break;
                }
            }
            return ribbonpanel;

        }

        void a_ApplicationClosing(object sender, Autodesk.Revit.UI.Events.ApplicationClosingEventArgs e)
        {
            throw new NotImplementedException();
        }
        void a_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {

        }
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
