using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Design_Plug_in_Updates
{
    class CreateSmartScheduleEmpty
    {
        public void CreateSchedule(Document doc)
        {
            ViewSchedule smartSchedule;

            // Modify document within a transaction
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


                    TableMergedCell cells = new TableMergedCell(0, 0, 0, headerList.Count);
                    tsd.MergeCells(cells);

                }

                



                tx.Commit();
            }







        }
    }
}
