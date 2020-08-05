using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Design_Plug_in_Updates.Synchronize
{
    class ExtractDataFromSchedule
    {
        public List<WpfApp1.Models.Item> NewRecords(List<WpfApp1.Models.Item> RecordsSorted,Document doc)
        {
            List<WpfApp1.Models.Item> NewItems = new List<WpfApp1.Models.Item>();

            #region Get last item number
            int ListLength = RecordsSorted.Count-1;
            int Counter = 0;
            double LastNumber = 0;
            foreach(WpfApp1.Models.Item item in RecordsSorted)
            {
                if (double.TryParse(RecordsSorted[ListLength - Counter].ItemNumber,out double LastItemNumber))
                {
                    LastNumber = LastItemNumber;
                    break;
                }
                Counter = Counter + 1;
            }
            #endregion

            #region Extract data from schedule
            #region Getting the schedule and its tabs
            string scheduleTitle = "Smart Schedule";
            ViewSchedule smartSchedule = (from v in new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewSchedule))
                         .Cast<ViewSchedule>()
                                          where v.Name == scheduleTitle
                                          select v).FirstOrDefault();


            #endregion

            #region Get the table data
            var tableData = smartSchedule.GetTableData();
            var tsd = tableData.GetSectionData(SectionType.Header);
            #endregion


            #region Get data
            int NumberOfRows=tsd.NumberOfRows-3;
            int Increase = 1;
            for(int i=0;i<NumberOfRows;i++)
            {
                string ItemNum = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 3);
                if (ItemNum==" " )
                {
                    #region Add new item
                    WpfApp1.Models.Item NewItem = new WpfApp1.Models.Item();
                    NewItem.RecordID = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 0);



                    NewItem.z1020_CRMRecordID = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 1);


                    NewItem.ProjectNumber = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 2);


                    NewItem.ItemNumber = (LastNumber+Increase).ToString();


                    NewItem.ItemName = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 4);


                    NewItem.Area = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 5);


                    NewItem.Quantity = Convert.ToInt32(tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 6));


                    NewItem.SourceCompany = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 7);


                    NewItem.Manufacturer = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 8);


                    NewItem.ModelNumber = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 9);


                    NewItem.Description = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 10);


                    NewItem.Website = tsd.GetCellText(tsd.FirstRowNumber + 2 + i, 11);
                    #endregion
                    NewItems.Add(NewItem);
                    Increase = Increase + 1;
                }



            }
            #endregion
            #endregion



            return NewItems;
        }
    }
}
