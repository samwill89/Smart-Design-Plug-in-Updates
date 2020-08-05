using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Smart_Design_Plug_in_Updates.Synchronize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1;
using WpfApp1.Models;

namespace Smart_Design_Plug_in_Updates
{
    [Transaction(TransactionMode.Manual)]
    class SynchronizeFileMaker: IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;
            List<ClustersData> ScheduleItems = new List<ClustersData>();
            #region Add schedule if it do not exist
            #region Check If Schedule Exist
            string scheduleTitle = "Smart Schedule";
            ViewSchedule CheckIfScheduleExist = (from v in new FilteredElementCollector(doc)
                     .OfClass(typeof(ViewSchedule))
                     .Cast<ViewSchedule>()
                                                 where v.Name == scheduleTitle
                                                 select v).FirstOrDefault();
            #endregion

            if (CheckIfScheduleExist == null)
            {
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                var UserResp = MessageBox.Show("No smart schedule has been created do you want to make an empty schedule?", "Warning", buttons, MessageBoxImage.Warning);
                if (UserResp.ToString() == "OK")
                {
                    #region Send to create schedule class
                    CreateSmartSchedule create = new CreateSmartSchedule();
                    string Exist = create.CreateSchedule(doc, ScheduleItems);
                    #endregion



                    #region Intialize window
                    MainWindow x = new MainWindow();
                    double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                    double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                    double windowWidth = x.Width;
                    double windowHeight = x.Height;
                    x.Left = (screenWidth / 2) - (windowWidth / 2);
                    x.Top = (screenHeight / 2) - (windowHeight / 2);
                    x.ShowDialog();
                    #endregion

                    #region Start the method                   
                    string Method = x.Method;
                    List<WpfApp1.Models.Item> RecordsUnsorted = x.FileMakerItems;
                    Identify(doc, Method, RecordsUnsorted);
                    #endregion


                }
                else
                {
                    MessageBoxButton buttons1 = MessageBoxButton.OK;
                    MessageBox.Show("Can't synchronize without creating a smart schedule", "Error", buttons1, MessageBoxImage.Error);
                    
                }
            }


            #endregion
            else
            {
                var pCurrView = doc.ActiveView;
                
                if(pCurrView.Name== "Smart Schedule")
                {
                    MessageBoxButton buttons1 = MessageBoxButton.OK;
                    MessageBox.Show("Close the Smart Schedule before syncing", "Error", buttons1, MessageBoxImage.Error);
                }
                else
                {
                    #region Intialize window
                    MainWindow x = new MainWindow();
                    double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                    double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                    double windowWidth = x.Width;
                    double windowHeight = x.Height;
                    x.Left = (screenWidth / 2) - (windowWidth / 2);
                    x.Top = (screenHeight / 2) - (windowHeight / 2);
                    x.ShowDialog();
                    #endregion

                    #region Start the method                   
                    string Method = x.Method;
                    List<WpfApp1.Models.Item> RecordsUnsorted = x.FileMakerItems;
                    Identify(doc, Method, RecordsUnsorted);

                    #endregion
                }


            }



            return Result.Succeeded;
        }

        public void Identify(Document doc,string Method, List<WpfApp1.Models.Item> RecordsUnsorted)
        {
            IdentifyingChosenMethod Identifying = new IdentifyingChosenMethod();
            Identifying.IdnetifyMethod(doc, Method, RecordsUnsorted);
        }

    }
}
