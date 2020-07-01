#region Namespaces
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace Smart_Design_Plug_in_Updates
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            #region Get selection
            IList<Reference> myRefs = uidoc.Selection.PickObjects(ObjectType.Element);
            #endregion

            #region Get elements
            List<Element> SelectedElements = new List<Element>();
            foreach (Reference refele in myRefs)
            {
                Element Element = doc.GetElement(refele);
                SelectedElements.Add(Element);

            }
            #endregion

            #region Filter out elements that are not family instance
            List<Element> FilteredElements = new List<Element>();
            
            foreach (Element Ele in SelectedElements)
            {
                FamilyInstance FN = Ele as FamilyInstance;

                if (FN != null)
                {
                    FilteredElements.Add(Ele);
                }
            }

            #endregion


            #region Grouping elements if they have the same family and type

            #region Get all families
            List<string> Families = new List<string>();
            foreach (Element ele in FilteredElements) 
            {
                ElementType Type = doc.GetElement(ele.GetTypeId()) as ElementType;
                string FamilyName = Type.FamilyName;
                if (!(Families.Contains(FamilyName)))
                {
                    Families.Add(FamilyName);
                }
                
            }
            #endregion

            #region Get dictionary of each family types
            Dictionary<string, List<string>> FamiliesTypes = new Dictionary<string, List<string>>();
            foreach(string Family in Families)
            {
                List<string> Types = new List<string>();
                foreach (Element ele in FilteredElements)
                {
                    ElementType Type = doc.GetElement(ele.GetTypeId()) as ElementType;
                    string FamilyName = Type.FamilyName;
                    if (FamilyName == Family)
                    {
                        string TypeName = Type.LookupParameter("Type Name").AsString();

                        if (!(Types.Contains(TypeName)))
                        {
                            Types.Add(TypeName);
                        }
                    }
                }
                FamiliesTypes.Add(Family, Types);
            }
            #endregion



            #region Get dictionary of each family types rooms
            Dictionary<string, List<string>> ElementsRooms = new Dictionary<string, List<string>>();

            foreach (string Family in FamiliesTypes.Keys)
            {
                foreach (string Type in FamiliesTypes[Family])
                {
                    string FamiliesTypesNames = Family + "__Splitter__" + Type;

                    List<string> Rooms = new List<string>();


                    foreach (Element ele in FilteredElements)
                    {
                        ElementType eleType = doc.GetElement(ele.GetTypeId()) as ElementType;
                        string FamilyName = eleType.FamilyName;
                        if (FamilyName == Family)
                        {
                            string TypeName = eleType.LookupParameter("Type Name").AsString();

                            if (TypeName == Type)
                            {
                                #region Get Room
                                XYZ ElementPoint = new XYZ(0, 0, 0);
                                if (ele.Location as LocationPoint != null)
                                {
                                    LocationPoint ElementLocationPoint = ele.Location as LocationPoint;
                                    ElementPoint = ElementLocationPoint.Point;
                                }
                                else if (ele.Location as LocationCurve != null)
                                {
                                    LocationCurve ElementLocationCurve = ele.Location as LocationCurve;
                                    Line ElementLocationLine = ElementLocationCurve.Curve as Line;
                                    ElementPoint = ElementLocationLine.Origin;
                                }
                                Room room = doc.GetRoomAtPoint(ElementPoint);
                                String RoomName="Not inside a room";
                                if (room != null)
                                {
                                    RoomName = room.Name;
                                }

                                if (!(Rooms.Contains(RoomName)))
                                {
                                    Rooms.Add(RoomName);
                                }
                                #endregion

                            }
                        }
                    }
                    ElementsRooms.Add(FamiliesTypesNames, Rooms);
                }
            }
            #endregion

               

            #region Making Groups Of Elements
            Dictionary<string, List<Element>> ElementsGroups = new Dictionary<string, List<Element>>();

            foreach(string FamilyType in ElementsRooms.Keys)
            {
                var FamilyAndType = FamilyType.Split(new[] { "__Splitter__" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string roomname in ElementsRooms[FamilyType])
                {
                    string GroupName = FamilyAndType[0] + "__Splitter__" + FamilyAndType[1] + "__Splitter__" + roomname;

                    List<Element> GroupElements = new List<Element>();

                    foreach (Element ele in FilteredElements)
                    {
                        ElementType eleType = doc.GetElement(ele.GetTypeId()) as ElementType;
                        string FamilyName = eleType.FamilyName;                                              

                        if (FamilyName == FamilyAndType[0])
                        {
                            string TypeName = eleType.LookupParameter("Type Name").AsString();

                            if (TypeName == FamilyAndType[1])
                            {
                                #region Get Room
                                XYZ ElementPoint = new XYZ(0, 0, 0);
                                if (ele.Location as LocationPoint != null)
                                {
                                    LocationPoint ElementLocationPoint = ele.Location as LocationPoint;
                                    ElementPoint = ElementLocationPoint.Point;
                                }
                                else if (ele.Location as LocationCurve != null)
                                {
                                    LocationCurve ElementLocationCurve = ele.Location as LocationCurve;
                                    Line ElementLocationLine = ElementLocationCurve.Curve as Line;
                                    ElementPoint = ElementLocationLine.Origin;
                                }
                                Room room = doc.GetRoomAtPoint(ElementPoint);
                                String FinalRoomName = "Not inside a room";
                                if (room != null)
                                {
                                    FinalRoomName = room.Name;
                                }
                                #endregion

                                if (FinalRoomName == roomname)
                                {
                                    GroupElements.Add(ele);
                                }

                                
                            }
                        }
                    }
                    ElementsGroups.Add(GroupName, GroupElements);
                }
            }
            #endregion

            #endregion






            #region Open the pop up          
            AddItemstoSmartSchedule x = new AddItemstoSmartSchedule(ElementsGroups,doc);
            x.Height = 555;
            x.Width = 1200;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = x.Width;
            double windowHeight = x.Height;
            x.Left = (screenWidth / 2) - (windowWidth / 2);
            x.Top = (screenHeight / 2) - (windowHeight / 2);
            x.ShowDialog();
            
            #endregion


            // Modify document within a transaction

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Transaction Name");
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
