using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Design_Plug_in_Updates
{



    class ExistingData
    {
        public string Family__Type { get; set; }

        public string Location { get; set; }

        public string Quantity { get; set; }

        public static ObservableCollection<ExistingData> Existing(Dictionary<string,string> Data)
        {
            var EData = new ObservableCollection<ExistingData>();

            #region Add Data
            foreach(string ItemAndLocation in Data.Keys)
            {
                var ItAndLoca = ItemAndLocation.Split(new[] { "__Splitter__" }, StringSplitOptions.RemoveEmptyEntries);
                string Family_Type = ItAndLoca[0];
                string Location = ItAndLoca[1];
                string Quantity = Data[ItemAndLocation];

                EData.Add(new ExistingData() { Family__Type = Family_Type, Location = Location, Quantity = Quantity });
            }

            #endregion
            return EData;
        }
    }
}
