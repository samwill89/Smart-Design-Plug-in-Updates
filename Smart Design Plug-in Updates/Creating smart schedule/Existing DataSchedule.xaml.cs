using Autodesk.Revit.Creation;
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
    /// Interaction logic for Existing_DataSchedule.xaml
    /// </summary>
    public partial class Existing_DataSchedule : Window
    {
        public List<Object> CheckedItems;
        public Autodesk.Revit.DB.Document GeneralDocument;
        public Existing_DataSchedule(Dictionary<string,string> Data,List<Object> ItemsToAddToSchedule, Autodesk.Revit.DB.Document doc)
        {
            InitializeComponent();
            Grid1.ItemsSource = ExistingData.Existing(Data);
            NumberOfGroups.Text = ExistingData.Existing(Data).Count.ToString();
            CheckedItems = ItemsToAddToSchedule;
            GeneralDocument = doc;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            string Action = "Add";
            ActionWithExistingSchedule Ac = new ActionWithExistingSchedule();
            Ac.ActionToMakeToExistingSchedule(Action, CheckedItems,GeneralDocument);
            Close();
        }

        private void Merge(object sender, RoutedEventArgs e)
        {
            string Action = "Merge";
            ActionWithExistingSchedule Ac = new ActionWithExistingSchedule();
            Ac.ActionToMakeToExistingSchedule(Action, CheckedItems, GeneralDocument);
            Close();
        }

        private void Replace(object sender, RoutedEventArgs e)
        {
            string Action = "Replace";
            ActionWithExistingSchedule Ac = new ActionWithExistingSchedule();
            Ac.ActionToMakeToExistingSchedule(Action, CheckedItems, GeneralDocument);
            Close();
        }
    }
}
