using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1;

namespace Smart_Design_Plug_in_Updates.Synchronize
{

    class AddDataToSmartSchedule
    {
        public void AddData(List<WpfApp1.Models.Item> Records, Document doc)
        {

            ViewSchedule smartSchedule;

            // Modify document within a transaction
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Creating Smart Schedule");

                #region Check If Schedule Exist
                string scheduleTitle = "Smart Schedule";
                smartSchedule = (from v in new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewSchedule))
                         .Cast<ViewSchedule>()
                                 where v.Name == scheduleTitle
                                 select v).FirstOrDefault();
                #endregion               


                #region Gathering Headers to be used for the table data

                List<string> headerList = new List<string>();
                headerList.Add("Record ID");
                headerList.Add("Revit ID");
                headerList.Add("Project Number");
                headerList.Add("Item Number");
                headerList.Add("Item Name");
                headerList.Add("Location");
                headerList.Add("Quantity");
                headerList.Add("Vendor");
                headerList.Add("Manufacturer");
                headerList.Add("Model");
                headerList.Add("Description");
                headerList.Add("Website");
                #endregion

                #region Setting the table and location where the data will go
                var tableData = smartSchedule.GetTableData();
                var tsd = tableData.GetSectionData(SectionType.Header);
                #endregion





                #region Filling Data
                int x = 0;
                int stringLen = 0;
                foreach (WpfApp1.Models.Item Da in Records)
                {
                    string ItemName = Da.ItemName;
                    stringLen = ItemName.Length;

                    tsd.SetColumnWidth(0, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(1, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(2, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(3, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(4, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(5, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(6, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(7, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(8, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(9, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(10, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(11, (stringLen / 25 + 1));
                    tsd.InsertRow(tsd.FirstRowNumber + 2 + x);

                    string RecordID = Da.FileMakerRecordId.ToString();
                    if (RecordID == null)
                    {
                        RecordID = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 0, RecordID);

                    string RevitID = Da.z1020_CRMRecordID;
                    if (RevitID == null)
                    {
                        RevitID = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 1, RevitID);

                    string ProjectNum = Da.ProjectNumber;
                    if (ProjectNum == null)
                    {
                        ProjectNum = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 2, ProjectNum);

                    string ItemNum = Da.ItemNumber;
                    if (ItemNum == null)
                    {
                        ItemNum = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 3, ItemNum);

                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 4, ItemName);
                    string Location = Da.Area;
                    if (Location == null)
                    {
                        Location = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 5, Location);
                    string Quantity = Da.Quantity.ToString();
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 6, Quantity);
                    string Vendor = Da.SourceCompany;
                    if (Vendor == null)
                    {
                        Vendor = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 7, Vendor);
                    string Manufacturer = Da.Manufacturer;
                    if (Manufacturer == null)
                    {
                        Manufacturer = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 8, Manufacturer);
                    string Model = Da.ModelNumber;
                    if (Model == null)
                    {
                        Model = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 9, Model);
                    string Description = Da.Description;
                    if (Description == null)
                    {
                        Description = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 10, Description);
                    string Website = Da.Website;
                    if (Website == null)
                    {
                        Website = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 11, Website);
                    x = x + 1;
                }
                #endregion


                tx.Commit();

            }



        }
        public void AddDataInternal(List<object> Records, Document doc)
        {

            ViewSchedule smartSchedule;

            // Modify document within a transaction
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Creating Smart Schedule");

                #region Check If Schedule Exist
                string scheduleTitle = "Smart Schedule";
                smartSchedule = (from v in new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewSchedule))
                         .Cast<ViewSchedule>()
                                 where v.Name == scheduleTitle
                                 select v).FirstOrDefault();
                #endregion


                #region Gathering Headers to be used for the table data

                List<string> headerList = new List<string>();
                headerList.Add("Record ID");
                headerList.Add("Revit ID");
                headerList.Add("Project Number");
                headerList.Add("Item Number");
                headerList.Add("Item Name");
                headerList.Add("Location");
                headerList.Add("Quantity");
                headerList.Add("Vendor");
                headerList.Add("Manufacturer");
                headerList.Add("Model");
                headerList.Add("Description");
                headerList.Add("Website");
                #endregion

                #region Setting the table and location where the data will go
                var tableData = smartSchedule.GetTableData();
                var tsd = tableData.GetSectionData(SectionType.Header);
                #endregion





                #region Filling Data
                int x = 0;
                int stringLen = 0;
                foreach (object Daa in Records)
                {
                    ClustersData Da = Daa as ClustersData;
                    string ItemName = Da.Family__Type;
                    stringLen = ItemName.Length;

                    tsd.SetColumnWidth(0, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(1, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(2, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(3, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(4, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(5, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(6, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(7, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(8, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(9, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(10, (stringLen / 25 + 1));
                    tsd.SetColumnWidth(11, (stringLen / 25 + 1));
                    tsd.InsertRow(tsd.FirstRowNumber + 2 + x);

                    string RecordID = Da.RecordID;
                    if (RecordID == null)
                    {
                        RecordID = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 0, RecordID);

                    string RevitID = Da.RevitID;
                    if (RevitID == null)
                    {
                        RevitID = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 1, RevitID);

                    string ProjectNum = Da.ProjectNumber;
                    if (ProjectNum == null)
                    {
                        ProjectNum = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 2, ProjectNum);

                    string ItemNum = Da.ItemNumber;
                    if (ItemNum == null)
                    {
                        ItemNum = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 3, ItemNum);

                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 4, ItemName);
                    string Location = Da.Location;
                    if (Location == null)
                    {
                        Location = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 5, Location);
                    string Quantity = Da.Quantity.ToString();
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 6, Quantity);
                    string Vendor = Da.Vendor;
                    if (Vendor == null)
                    {
                        Vendor = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 7, Vendor);
                    string Manufacturer = Da.Manufacturer;
                    if (Manufacturer == null)
                    {
                        Manufacturer = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 8, Manufacturer);
                    string Model = Da.Model;
                    if (Model == null)
                    {
                        Model = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 9, Model);
                    string Description = Da.Description;
                    if (Description == null)
                    {
                        Description = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 10, Description);
                    string Website = Da.Website;
                    if (Website == null)
                    {
                        Website = " ";
                    }
                    tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 11, Website);
                    x = x + 1;
                }
                #endregion


                tx.Commit();

            }
        }
    }
}
