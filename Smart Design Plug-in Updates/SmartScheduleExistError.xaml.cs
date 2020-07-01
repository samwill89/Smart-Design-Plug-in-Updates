using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Smart_Design_Plug_in_Updates
{
    /// <summary>
    /// Interaction logic for SmartScheduleExistError.xaml
    /// </summary>
    public partial class SmartScheduleExistError : Window
    {
        public Document GenerelDocument;
        public List<Object> ItemToAddToSchedule;
        public SmartScheduleExistError(List<Object> Data, Document doc)
        {
            InitializeComponent();
            GenerelDocument = doc;
            ItemToAddToSchedule = Data;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void Register_Click(object sender, RoutedEventArgs e)
        {
            #region Getting the schedule and its tabs
            string scheduleTitle = "Smart Schedule";
            ViewSchedule smartSchedule = (from v in new FilteredElementCollector(GenerelDocument)
                         .OfClass(typeof(ViewSchedule))
                         .Cast<ViewSchedule>()
                         where v.Name == scheduleTitle
                         select v).FirstOrDefault();
            #endregion

            #region Get the table data
            var tableData = smartSchedule.GetTableData();
            var tsd = tableData.GetSectionData(SectionType.Header);
            #endregion

            #region Get number or rows and columns
            int NumberOfColumns = tsd.LastColumnNumber;
            int NumberOfRows = tsd.LastRowNumber;
            int NumberOfDataRows = NumberOfRows - 2;
            #endregion

            #region Get Data
            Dictionary<string, string> Data = new Dictionary<string, string>();
            
            for(int i = 0; i < NumberOfDataRows; i++)
            {
                string ItemsName = tsd.GetCellText(i + 2, 0);
                string Location = tsd.GetCellText(i + 2, 1);
                string NameAndLocation = ItemsName + "__Splitter__" + Location;
                string Quantity = tsd.GetCellText(i + 2, 2);
                Data.Add(NameAndLocation, Quantity);
            }
            #endregion
            Close();
            Existing_DataSchedule x = new Existing_DataSchedule(Data,ItemToAddToSchedule, GenerelDocument);
            x.Height = 510;
            x.Width = 600;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = x.Width;
            double windowHeight = x.Height;
            x.Left = (screenWidth / 2) - (windowWidth / 2);
            x.Top = (screenHeight / 2) - (windowHeight / 2);
            x.ShowDialog();
        }
    }
}
