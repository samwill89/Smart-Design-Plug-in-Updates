using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Smart_Design_Plug_in_Updates
{
    class ActionWithExistingSchedule
    {
        public void ActionToMakeToExistingSchedule(string Action,List<Object> Data, Autodesk.Revit.DB.Document GeneralDocument)
        {
            #region Get the data from the items
            #region Getting the schedule and its tabs
            string scheduleTitle = "Smart Schedule";
            ViewSchedule smartSchedule = (from v in new FilteredElementCollector(GeneralDocument)
                         .OfClass(typeof(ViewSchedule))
                         .Cast<ViewSchedule>()
                                          where v.Name == scheduleTitle
                                          select v).FirstOrDefault();


            #endregion

            #region Get the table data
            var tableData = smartSchedule.GetTableData();
            var tsd = tableData.GetSectionData(SectionType.Header);
            #endregion


            #endregion

            if (Action == "Add")
            {
                using (Transaction tx = new Transaction(GeneralDocument))
                {
                    tx.Start("Add items to existing schedule");
                    int LastRow = tsd.LastRowNumber;
                    double stringLen = tsd.GetColumnWidth(0);
                    int i = 0;
                    foreach (object Item in Data)
                    {
                        ClustersData Da = Item as ClustersData;

                        string ItemName = Da.Family__Type;

                        if (ItemName.Length > stringLen)
                        {
                            stringLen = ItemName.Length;
                        }
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
                        tsd.InsertRow(tsd.LastRowNumber );
                        string RecordID = Da.RecordID;
                        if (RecordID == null)
                        {
                            RecordID = " ";
                        }
                        tsd.SetCellText(LastRow + i, 0, RecordID);

                        string RevitID = Da.RevitID;
                        if (RevitID == null)
                        {
                            RevitID = " ";
                        }
                        tsd.SetCellText(LastRow + i, 1, RevitID);

                        string ProjectNum = Da.ProjectNumber;
                        if (ProjectNum == null)
                        {
                            ProjectNum = " ";
                        }
                        tsd.SetCellText(LastRow + i, 2, ProjectNum);

                        string ItemNum = Da.ItemNumber;
                        if (ItemNum == null)
                        {
                            ItemNum = " ";
                        }
                        tsd.SetCellText(LastRow + i, 3, ItemNum);

                        tsd.SetCellText(LastRow + i, 4, ItemName);
                        string Location = Da.Location;
                        if (Location == null)
                        {
                            Location = " ";
                        }
                        tsd.SetCellText(LastRow+i, 5, Location);
                        string Quantity = Da.Quantity.ToString();
                        tsd.SetCellText(LastRow+i, 6, Quantity);
                        string Vendor = Da.Vendor;
                        if (Vendor == null)
                        {
                            Vendor = " ";
                        }
                        tsd.SetCellText(LastRow + i, 7, Vendor);
                        string Manufacturer = Da.Manufacturer;
                        if (Manufacturer == null)
                        {
                            Manufacturer = " ";
                        }
                        tsd.SetCellText(LastRow + i, 8, Manufacturer);
                        string Model = Da.Model;
                        if (Model == null)
                        {
                            Model = " ";
                        }
                        tsd.SetCellText(LastRow + i, 9, Model);
                        string Description = Da.Description;
                        if (Description == null)
                        {
                            Description = " ";
                        }
                        tsd.SetCellText(LastRow + i, 10, Description);
                        string Website = Da.Website;
                        if (Website == null)
                        {
                            Website = " ";
                        }
                        tsd.SetCellText(LastRow + i, 11, Website);
                        i = i + 1;
                    }
                    tx.Commit();
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBox.Show("Data Added", "Done", buttons);
                }

            }

            if (Action == "Merge")
            {
                using (Transaction tx = new Transaction(GeneralDocument))
                {
                    tx.Start("Merge items with existing schedule");
                    int LastRow = tsd.LastRowNumber;
                    double stringLen = tsd.GetColumnWidth(0);
                    int DataCount = Data.Count;
                    int i = 0;
                    List<object> LeftData = new List<object>();
                    foreach (object item in Data)
                    {
                        LeftData.Add(item);
                    }
                    foreach (object Item in Data)
                    {
                        ClustersData Da = Item as ClustersData;

                        string ItemName = Da.Family__Type;
                        string Location = Da.Location;
                        if (Location == null)
                        {
                            Location = " ";
                        }
                        string Quantity = Da.Quantity.ToString();

                        for (int x = 0; x < tsd.LastRowNumber - 1; x++)
                        {
                            if (ItemName == tsd.GetCellText(x + 2, 4) && Location == tsd.GetCellText(x + 2, 5))
                            {
                                int Qu = Convert.ToInt32(Quantity) + Convert.ToInt32(tsd.GetCellText(x + 2, 6));
                                tsd.SetCellText(x + 2, 6, Qu.ToString());
                                DataCount = DataCount - 1;
                                LeftData.Remove(Item);
                            }
                        }
                    }
                    if (DataCount != 0)
                    {
                        foreach (object Item in LeftData)
                        {
                            ClustersData Da = Item as ClustersData;
                            #region Adding new items
                            string ItemName = Da.Family__Type;

                            if (ItemName.Length > stringLen)
                            {
                                stringLen = ItemName.Length;
                            }
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
                            tsd.InsertRow(tsd.LastRowNumber);

                            string RecordID = Da.RecordID;
                            if (RecordID == null)
                            {
                                RecordID = " ";
                            }
                            tsd.SetCellText(LastRow + i, 0, RecordID);

                            string RevitID = Da.RevitID;
                            if (RevitID == null)
                            {
                                RevitID = " ";
                            }
                            tsd.SetCellText(LastRow + i, 1, RevitID);

                            string ProjectNum = Da.ProjectNumber;
                            if (ProjectNum == null)
                            {
                                ProjectNum = " ";
                            }
                            tsd.SetCellText(LastRow + i, 2, ProjectNum);

                            string ItemNum = Da.ItemNumber;
                            if (ItemNum == null)
                            {
                                ItemNum = " ";
                            }
                            tsd.SetCellText(LastRow + i, 3, ItemNum);


                            tsd.SetCellText(LastRow + i, 4, ItemName);
                            string Location = Da.Location;
                            if (Location == null)
                            {
                                Location = " ";
                            }
                            tsd.SetCellText(LastRow + i, 5, Location);
                            string Quantity = Da.Quantity.ToString();
                            tsd.SetCellText(LastRow + i, 6, Quantity);
                            string Vendor = Da.Vendor;
                            if (Vendor == null)
                            {
                                Vendor = " ";
                            }
                            tsd.SetCellText(LastRow + i, 7, Vendor);
                            string Manufacturer = Da.Manufacturer;
                            if (Manufacturer == null)
                            {
                                Manufacturer = " ";
                            }
                            tsd.SetCellText(LastRow + i, 8, Manufacturer);
                            string Model = Da.Model;
                            if (Model == null)
                            {
                                Model = " ";
                            }
                            tsd.SetCellText(LastRow + i, 9, Model);
                            string Description = Da.Description;
                            if (Description == null)
                            {
                                Description = " ";
                            }
                            tsd.SetCellText(LastRow + i, 10, Description);
                            string Website = Da.Website;
                            if (Website == null)
                            {
                                Website = " ";
                            }
                            tsd.SetCellText(LastRow + i, 11, Website);
                            i = i + 1;
                            #endregion


                        }
                    }
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBox.Show("Data Merged", "Done", buttons);
                    tx.Commit();
                }

            }

            if (Action == "Replace")
            {
                using (Transaction tx = new Transaction(GeneralDocument))
                {
                    tx.Start("Replace Items");
                    int LastRow = tsd.LastRowNumber;
                    double stringLen = tsd.GetColumnWidth(0);
                    int DataCount = Data.Count;
                    int i = 0;
                    List<object> LeftData = new List<object>();
                    foreach (object item in Data)
                    {
                        LeftData.Add(item);
                    }
                    foreach (object Item in Data)
                    {
                        ClustersData Da = Item as ClustersData;
                        string RecordID = Da.RecordID;
                        if (RecordID == null)
                        {
                            RecordID = " ";
                        }
                        string RevitID = Da.RevitID;
                        if (RevitID == null)
                        {
                            RevitID = " ";
                        }
                        string ProjectNum = Da.ProjectNumber;
                        if (ProjectNum == null)
                        {
                            ProjectNum = " ";
                        }
                        string ItemNum = Da.ItemNumber;
                        if (ItemNum == null)
                        {
                            ItemNum = " ";
                        }
                        string ItemName = Da.Family__Type;
                        string Location = Da.Location;
                        if (Location == null)
                        {
                            Location = " ";
                        }
                        string Quantity = Da.Quantity.ToString();
                        string Vendor = Da.Vendor;
                        if (Vendor == null)
                        {
                            Vendor = " ";
                        }
                        string Manufacturer = Da.Manufacturer;
                        if (Manufacturer == null)
                        {
                            Manufacturer = " ";
                        }
                        string Model = Da.Model;
                        if (Model == null)
                        {
                            Model = " ";
                        }
                        string Description = Da.Description;
                        if (Description == null)
                        {
                            Description = " ";
                        }
                        string Website = Da.Website;
                        if (Website == null)
                        {
                            Website = " ";
                        }
                        for (int x = 0; x < tsd.LastRowNumber-1; x++)
                        {
                            if (ItemName == tsd.GetCellText(x + 2, 4) && Location == tsd.GetCellText(x + 2, 5))
                            {
                                tsd.SetCellText(x + 2, 0, RecordID);
                                tsd.SetCellText(x + 2, 1, RevitID);
                                tsd.SetCellText(x + 2, 2, ProjectNum);
                                tsd.SetCellText(x + 2, 3, ItemNum);
                                tsd.SetCellText(x+2, 4, ItemName);
                                tsd.SetCellText(x + 2, 5, Location);
                                tsd.SetCellText(x + 2, 6, Quantity);
                                tsd.SetCellText(x + 2, 7, Vendor);
                                tsd.SetCellText(x + 2, 8, Manufacturer);
                                tsd.SetCellText(x + 2, 9, Model);
                                tsd.SetCellText(x + 2, 10, Description);
                                tsd.SetCellText(x + 2, 11, Website);
                                DataCount = DataCount - 1;
                                LeftData.Remove(Item);
                            }
                        }


                    }
                    if (DataCount != 0)
                    {
                        foreach (object Item in LeftData)
                        {
                            ClustersData Da = Item as ClustersData;
                            #region Adding new items
                            string ItemName = Da.Family__Type;

                            if (ItemName.Length > stringLen)
                            {
                                stringLen = ItemName.Length;
                            }
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
                            tsd.InsertRow(tsd.LastRowNumber);

                            string RecordID = Da.RecordID;
                            if (RecordID == null)
                            {
                                RecordID = " ";
                            }
                            tsd.SetCellText(LastRow + i, 0, RecordID);

                            string RevitID = Da.RevitID;
                            if (RevitID == null)
                            {
                                RevitID = " ";
                            }
                            tsd.SetCellText(LastRow + i, 1, RevitID);

                            string ProjectNum = Da.ProjectNumber;
                            if (ProjectNum == null)
                            {
                                ProjectNum = " ";
                            }
                            tsd.SetCellText(LastRow + i, 2, ProjectNum);

                            string ItemNum = Da.ItemNumber;
                            if (ItemNum == null)
                            {
                                ItemNum = " ";
                            }
                            tsd.SetCellText(LastRow + i, 3, ItemNum);


                            tsd.SetCellText(LastRow + i, 4, ItemName);
                            string Location = Da.Location;
                            if (Location == null)
                            {
                                Location = " ";
                            }
                            tsd.SetCellText(LastRow + i, 5, Location);
                            string Quantity = Da.Quantity.ToString();
                            tsd.SetCellText(LastRow + i, 6, Quantity);
                            string Vendor = Da.Vendor;
                            if (Vendor == null)
                            {
                                Vendor = " ";
                            }
                            tsd.SetCellText(LastRow + i, 7, Vendor);
                            string Manufacturer = Da.Manufacturer;
                            if (Manufacturer == null)
                            {
                                Manufacturer = " ";
                            }
                            tsd.SetCellText(LastRow + i, 8, Manufacturer);
                            string Model = Da.Model;
                            if (Model == null)
                            {
                                Model = " ";
                            }
                            tsd.SetCellText(LastRow + i, 9, Model);
                            string Description = Da.Description;
                            if (Description == null)
                            {
                                Description = " ";
                            }
                            tsd.SetCellText(LastRow + i, 10, Description);
                            string Website = Da.Website;
                            if (Website == null)
                            {
                                Website = " ";
                            }
                            tsd.SetCellText(LastRow + i, 11, Website);
                            i = i + 1;
                            #endregion


                        }
                    }
                    tx.Commit();
                }

                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBox.Show("Data Replaced", "Done", buttons);
            }
        }
    }
}
