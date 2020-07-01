using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Design_Plug_in_Updates
{
    class CreateSmartSchedule
    {
        public string CreateSchedule(Autodesk.Revit.DB.Document doc,List<ClustersData> Data)
        {
            ViewSchedule smartSchedule;

            // Modify document within a transaction
            string Exist = "";
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Creating Smart Schedule");

                #region Check If Schedule Exist
                string scheduleTitle = "Smart Schedule";
                ViewSchedule CheckIfScheduleExist = (from v in new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewSchedule))
                         .Cast<ViewSchedule>()
                          where v.Name == scheduleTitle
                          select v).FirstOrDefault();
                #endregion
                if ((CheckIfScheduleExist == null))
                {
                    Exist = "Do Not Exist";
                    #region Create a blank multicategory schedule
                    smartSchedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.INVALID));
                    #endregion

                    #region Finding all schedulable fields
                    var wfsSchedFields = smartSchedule.Definition.GetSchedulableFields();
                    #endregion

                    #region
                    List<SchedulableField> createdFields = new List<SchedulableField>();
                    List<string> createdFieldNames = new List<string>();
                    List<ScheduleField> createdFieldDef = new List<ScheduleField>();
                    #endregion

                    var markParam = new ElementId(BuiltInParameter.ALL_MODEL_MARK);

                    smartSchedule.Definition.IsItemized = false;

                    foreach (var wf in wfsSchedFields)
                    {
                        if (wf.ParameterId == markParam)
                        {
                            var fieldDef = smartSchedule.Definition.AddField(wf);
                            createdFieldDef.Add(fieldDef);
                            createdFieldNames.Add("Mark");
                            createdFields.Add(wf);
                        }
                    }

                    #region Creating schedule filters
                    foreach (var cfn in createdFieldNames)
                    {
                        foreach (var cf in createdFieldDef)
                        {
                            if (cfn == "Mark")
                            {
                                var markFieldId = cf.FieldId;
                                var schedFilter1 = new ScheduleFilter(markFieldId, ScheduleFilterType.Equal, " ");
                                smartSchedule.Definition.AddFilter(schedFilter1);
                            }

                        }
                    }

                    #endregion

                    #region Gathering Headers to be used for the table data

                    List<string> headerList = new List<string>();
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




                    smartSchedule.Name = scheduleTitle;

                    tsd.SetCellText(tsd.FirstRowNumber, tsd.FirstColumnNumber, scheduleTitle);
                    tsd.InsertRow(1);
                    tsd.InsertRow(1);


                    for (int i = 0; i < headerList.Count; i++)
                    {
                        tsd.InsertColumn(1);
                    }

                    for (int i = 0; i < headerList.Count; i++)
                    {
                        var stringgLen = ((headerList[i]).ToString()).Length;
                        tsd.SetCellText(tsd.FirstRowNumber + 1, i, (headerList[i]).ToString());
                        tsd.SetColumnWidth(i, (stringgLen / 25 + 1));
                    }

                    #region Filling Data
                    int x = 0;
                    int stringLen = 0;
                    foreach (ClustersData Da in Data)
                    {
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
                        tsd.InsertRow(tsd.FirstRowNumber + 2 + x);
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 0, ItemName);
                        string Location = Da.Location;
                        if (Location == null)
                        {
                            Location = " ";
                        }
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 1, Location);
                        string Quantity = Da.Quantity.ToString();
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 2, Quantity);
                        string Vendor = Da.Vendor;
                        if (Vendor == null)
                        {
                            Vendor = " ";
                        }
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 3, Vendor);
                        string Manufacturer = Da.Manufacturer;
                        if (Manufacturer == null)
                        {
                            Manufacturer = " ";
                        }
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 4, Manufacturer);
                        string Model = Da.Model;
                        if (Model == null)
                        {
                            Model = " ";
                        }
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 5, Model);
                        string Description = Da.Description;
                        if (Description == null)
                        {
                            Description = " ";
                        }
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 6, Description);
                        string Website = Da.Website;
                        if (Website == null)
                        {
                            Website = " ";
                        }
                        tsd.SetCellText(tsd.FirstRowNumber + 2 + x, 7, Website);
                        x = x + 1;
                    }
                    #endregion

                    TableMergedCell cells = new TableMergedCell(0, 0, 0, headerList.Count);
                    tsd.MergeCells(cells);

                }

                else
                {
                    Exist = "Exist";
                }



                tx.Commit();
            }






            return Exist;
        }
    }
}
