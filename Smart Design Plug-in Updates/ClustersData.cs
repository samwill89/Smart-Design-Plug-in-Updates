using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Design_Plug_in_Updates
{
    class ClustersData
    {

        public bool Checked { get; set; }

        public string RecordID { get; set; }

        public string RevitID { get; set; }

        public string ProjectNumber { get; set; }

        public string ItemNumber { get; set; }

        public string Family__Type { get; set; }

        public string Location { get; set; }

        public int Quantity { get; set; }

        public string Vendor { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }


        public static ObservableCollection<ClustersData> GetData(Dictionary<string, List<Element>> ElementsGroups,Document doc,string Selection,List<string>GroupNames,string PID)
        {
            
            var Data = new ObservableCollection<ClustersData>();

            #region making rows with the data

            foreach (string Group in ElementsGroups.Keys)
            {
                #region Get the parameters
                Element ele = ElementsGroups[Group][0];
                ElementType eleType = doc.GetElement(ele.GetTypeId()) as ElementType;
                var FamilyAndTypeAndRoom = Group.Split(new[] { "__Splitter__" }, StringSplitOptions.RemoveEmptyEntries);
                string GroupName = FamilyAndTypeAndRoom[0] + "____" + FamilyAndTypeAndRoom[1];
                string Location= FamilyAndTypeAndRoom[2];

                string ProjectID = PID;

                int Quantity = ElementsGroups[Group].Count;
                string Vendor="";
                if (eleType.LookupParameter("Vendor") != null)
                {
                    Vendor = eleType.LookupParameter("Vendor").AsString();
                }
                string Manufacturer = "";
                if (eleType.LookupParameter("Manufacturer") != null)
                {
                    Manufacturer = eleType.LookupParameter("Manufacturer").AsString();
                }
                string Model = "";
                if (eleType.LookupParameter("Model") != null)
                {
                    Model = eleType.LookupParameter("Model").AsString();
                }
                string Description = "";
                if (eleType.LookupParameter("Description") != null)
                {
                    Description = eleType.LookupParameter("Description").AsString();
                }
                string Website = "";
                if (eleType.LookupParameter("URL") != null)
                {
                    
                    Website = eleType.LookupParameter("URL").AsString();
                }
                #endregion


                #region Checked
                bool Checked = false;
                if (Selection=="Check None")
                {
                     Checked= false;
                }
                else if(Selection=="Check All")
                {
                    Checked = true;
                }
                else if(Selection=="Check Selected")
                {
                    if (GroupNames.Contains(Group))
                    {
                        Checked = true;
                    }
                    else
                    {
                        Checked = false;
                    }
                }
                #endregion

                Data.Add(new ClustersData() { Checked = Checked,RevitID=ProjectID, Family__Type = GroupName,Location=Location, Quantity = Quantity,Vendor=Vendor,Manufacturer=Manufacturer,Model=Model,Description=Description,Website= Website });
            }

            #endregion
            return Data;
        }

    }
}
