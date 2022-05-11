using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v1;

namespace TeklaSAPLink
{
    public static class SAPCommands
    {
        public static void GenerateSAPModel(List<MyBeam> MyBeams)// процедура создания модели на основании данных Tekla
        {
            cHelper myHelper = new Helper();
            cOAPI mySapObject = myHelper.GetObject("CSI.SAP2000.API.SapObject");
            cSapModel mySapModel = mySapObject.SapModel;

            GetBeamAndCollumnObjects(mySapModel);

            int ret = 0;            
            ret = mySapModel.SetPresentUnits(eUnits.Ton_mm_C);
            string Name = "";
            ret = mySapModel.PropMaterial.AddMaterial(ref Name, eMatType.Steel, "Russia", "СП 16.13330.2011", "C245_(2...20mm)"); 
            ret = mySapModel.PropFrame.ImportProp("Дк_30К1", "C245_(2...20mm)", "Russian2020_ru.pro", "Дк_30К1");
            ret = mySapModel.PropFrame.ImportProp("Дк_35К1", "C245_(2...20mm)", "Russian2020_ru.pro", "Дк_35К1");
            ret = mySapModel.PropFrame.ImportProp("Дш_40Ш1", "C245_(2...20mm)", "Russian2020_ru.pro", "Дш_40Ш1");
            ret = mySapModel.PropFrame.ImportProp("Дб_30Б1", "C245_(2...20mm)", "Russian2020_ru.pro", "Дб_30Б1");
            ret = mySapModel.PropFrame.ImportProp("Дб_20Б1", "C245_(2...20mm)", "Russian2020_ru.pro", "Дб_20Б1");

            List<string> FrameNames = new List<string>();
            string temp_string1 = "";          
            
            foreach (MyBeam mb in MyBeams)
            {
                ret = mySapModel.FrameObj.AddByCoord(mb.XYZ1[0], mb.XYZ1[1], mb.XYZ1[2],
                    mb.XYZ2[0], mb.XYZ2[1], mb.XYZ2[2], ref temp_string1, mb.SAP_Profile);
                mb.SAP_ID = temp_string1;
                ret = mySapModel.FrameObj.SetLocalAxes(mb.SAP_ID, mb.Orientaion);
            }
            mySapModel.View.RefreshView();
        }        
        public static List<MyBeam> GetBeamAndCollumnObjects(cSapModel mySapModel)//получение данных об элементах содержащихся в модели
        {
            List<MyBeam> MyBeams = new List<MyBeam>();
            int ret = 0;
            int NumberNames = 0;
            string[] ListName = { };            

            ret = mySapModel.FrameObj.GetNameList(ref NumberNames, ref ListName);//список элементов в модели SAP

            foreach(string id in ListName)
            {
                string point1 = "";
                string point2 = "";
                ret = mySapModel.FrameObj.GetPoints(id, ref point1, ref point2);
                double X1 = 0;
                double Y1 = 0;
                double Z1 = 0;
                double X2 = 0;
                double Y2 = 0;
                double Z2 = 0;
                ret = mySapModel.PointObj.GetCoordCartesian(point1, ref X1, ref Y1, ref Z1);
                ret = mySapModel.PointObj.GetCoordCartesian(point2, ref X2, ref Y2, ref Z2);

                string PropNameSection = "";
                string SAuto = "";
                ret = mySapModel.FrameObj.GetSection(id, ref PropNameSection, ref SAuto);

                string FileName = "";
                string MatProp = "";
                double t3 = 0;
                double t2 = 0;
                double Area = 0;
                double As2 = 0;
                double As3 = 0;
                double Torsion = 0;
                double I22 = 0;
                double I33 = 0;
                double S22 = 0;
                double S33 = 0;
                double Z22 = 0;
                double Z33 = 0;
                double R22 = 0;
                double R33 = 0;
                int Color = 0;
                string Notes = "";
                string GUID = "";
                ret = mySapModel.PropFrame.GetISection(PropNameSection, ref FileName, ref MatProp, ref t3, ref t2, ref Area, ref As2, ref As3, 
                    ref Torsion,ref Color, ref Notes, ref GUID); 

                MyBeams.Add(new MyBeam());
                //MyBeams.LastOrDefault().teklastartPoint = new Tekla.Structures.Geometry3d.Vector(X1, Y1, Z1);
                //MyBeams.LastOrDefault().teklaendPoint = new Tekla.Structures.Geometry3d.Vector(X2, Y2, Z2);
                //MyBeams.LastOrDefault().SAPMaterial = MatProp;
                //MyBeams.LastOrDefault().SAPProfile = PropNameSection;

            }

            return MyBeams;
        }
    }
}
