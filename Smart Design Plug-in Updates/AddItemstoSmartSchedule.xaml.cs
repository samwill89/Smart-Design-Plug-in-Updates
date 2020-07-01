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
    /// Interaction logic for AddItemstoSmartSchedule.xaml
    /// </summary>
    public partial class AddItemstoSmartSchedule : Window
    {
        public Dictionary<string, List<Element>> ElementsGroupsGeneral =new Dictionary<string, List<Element>>();
        public Document docGeneral;
        public AddItemstoSmartSchedule(Dictionary<string, List<Element>> ElementsGroups,Document doc)
        {
            InitializeComponent();
            ElementsGroupsGeneral = ElementsGroups;
            docGeneral = doc;
            List<string> GroupNames = new List<string>();
            Grid1.ItemsSource = ClustersData.GetData(ElementsGroups,doc, "Check None",GroupNames);
            NumberOfGroups.Text=ClustersData.GetData(ElementsGroups, doc, "Check None", GroupNames).Count.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Check_All(object sender, RoutedEventArgs e)
        {
            List<string> GroupNames = new List<string>();
            Grid1.ItemsSource = ClustersData.GetData(ElementsGroupsGeneral, docGeneral, "Check All", GroupNames);
        }

        private void Check_None(object sender, RoutedEventArgs e)
        {
            List<string> GroupNames = new List<string>();
            Grid1.ItemsSource = ClustersData.GetData(ElementsGroupsGeneral, docGeneral, "Check None", GroupNames);
        }

        private void Check_Selected(object sender, RoutedEventArgs e)
        {
            var ItemsInGrid = Grid1.SelectedItems;
            List<string> GroupNames = new List<string>();
            foreach(ClustersData data in ItemsInGrid)
            {
                string CurrentGroupName = data.Family__Type;
                var FamilyAndType = CurrentGroupName.Split(new[] { "____" }, StringSplitOptions.RemoveEmptyEntries);
                string Family = FamilyAndType[0];
                string Type = FamilyAndType[1];
                string Location = data.Location;
                string GroupName=Family+ "__Splitter__" + Type+ "__Splitter__" + Location;
                GroupNames.Add(GroupName);
            }
            Grid1.ItemsSource = ClustersData.GetData(ElementsGroupsGeneral, docGeneral, "Check Selected", GroupNames);
        }

        private void CreateSchedule(object sender, RoutedEventArgs e)
        {
            var CheckedItems = Grid1.Items;
            List<ClustersData> ScheduleItems = new List<ClustersData>();
            foreach(ClustersData CheckedItem in CheckedItems)
            {
                if (CheckedItem.Checked == true)
                {
                    ScheduleItems.Add(CheckedItem);
                    
                }
            }
            if (ScheduleItems.Count != 0)
            {
                #region Send to create schedule class
                CreateSmartSchedule create = new CreateSmartSchedule();
                string Exist = create.CreateSchedule(docGeneral, ScheduleItems);
                #endregion

                if (Exist == "Do Not Exist")
                {
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBox.Show("Smart Schedule Created Sucessfully","Done",buttons);
                    Close();
                }
                else
                {
                    #region Get Checked Data
                    var CheckedItemsTwo = Grid1.Items;
                    List<Object> ScheduleItemsTwo = new List<Object>();
                    foreach (var Item in CheckedItems)
                    {
                        ClustersData CheckedItem = Item as ClustersData;
                        if (CheckedItem.Checked == true)
                        {
                            ScheduleItemsTwo.Add(CheckedItem);

                        }
                    }
                    #endregion

                    SmartScheduleExistError x = new SmartScheduleExistError(ScheduleItemsTwo, docGeneral);
                    x.Height = 160;
                    x.Width = 350;
                    double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                    double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                    double windowWidth = x.Width;
                    double windowHeight = x.Height;
                    x.Left = (screenWidth / 2) - (windowWidth / 2);
                    x.Top = (screenHeight / 2) - (windowHeight / 2);
                    x.ShowDialog();

                    Close();
                }
            }
            else
            {
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBox.Show("Please choose a group","Error",buttons,MessageBoxImage.Error);
            }

            
        }
    }
}
