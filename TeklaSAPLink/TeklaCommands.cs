using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures.Model;
using Tekla.Structures.Analysis;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using Tekla.Structures;

namespace TeklaSAPLink
{
    public static class TeklaCommands
    {
        public static List<MyBeam> GetBeamsAndCollumnObjects() //получение данных о колоннах и балках в модели Tekla
        {
            List<MyBeam> MyBeams = new List<MyBeam>();
            Model m = new Model();
            Analysis MyAnalysis = new Analysis();            
            AnalysisObjectEnumerator MyParts = MyAnalysis.GetAnalysisObjectSelector().GetAllObjectsWithType(AnalysisObject.AnalysisObjectEnum.ANALYSIS_PART, "SAP");

            foreach(AnalysisPart ap in MyParts)
            {
                if (ap.AnalysisType != AnalysisPart.AnalysisTypeEnum.ANALYSIS_TYPE_IGNORE)
                {
                    AnalysisBar ab = ap.AnalysisBars[0];
                    Beam b = (Beam)m.SelectModelObject(ap.PartID);

                    MyBeams.Add(new MyBeam());
                    MyBeams.LastOrDefault().tekla_ID = ab.ID.ID.ToString();
                    MyBeams.LastOrDefault().tekla_Material = b.Material.MaterialString;
                    MyBeams.LastOrDefault().tekla_Profile = b.Profile.ProfileString;
                    MyBeams.LastOrDefault().XYZ1[0] = ab.Positions.FirstOrDefault().Node.Position.X;
                    MyBeams.LastOrDefault().XYZ1[1] = ab.Positions.FirstOrDefault().Node.Position.Y;
                    MyBeams.LastOrDefault().XYZ1[2] = ab.Positions.FirstOrDefault().Node.Position.Z;
                    MyBeams.LastOrDefault().XYZ2[0] = ab.Positions.LastOrDefault().Node.Position.X;
                    MyBeams.LastOrDefault().XYZ2[1] = ab.Positions.LastOrDefault().Node.Position.Y;
                    MyBeams.LastOrDefault().XYZ2[2] = ab.Positions.LastOrDefault().Node.Position.Z;
                    int a = 1;
                    if (MyBeams.LastOrDefault().XYZ2[0] == MyBeams.LastOrDefault().XYZ1[0] &
                        MyBeams.LastOrDefault().XYZ2[1] == MyBeams.LastOrDefault().XYZ1[1] &
                        MyBeams.LastOrDefault().XYZ2[2] > MyBeams.LastOrDefault().XYZ1[2])//условие по определению колонны
                    {
                        MyBeams.LastOrDefault().Orientaion = b.Position.RotationOffset + 90;
                    }
                    else
                    {
                        MyBeams.LastOrDefault().Orientaion = b.Position.RotationOffset;// TODO: есть дополнительное свойство Rotation, определяющее начальный угол поворота, сейчас у всех элементов "спереди"
                    }
                }
            }
            SetBeamsType(MyBeams);
            return MyBeams;
        } 
        public static void SetBeamsType(List<MyBeam> mbs)//заполняется информация о типе сечения и SAP профилей
        {
            string[,] Data = Convertor.MappingSectionsFileData();
            foreach (MyBeam mb in mbs)
            {
                for (int i = 0; i < Data.GetLength(0); i++)
                {
                    if (mb.tekla_Profile == Data[i, 2])
                    {
                        mb.Type = Data[i, 0];
                        mb.SAP_Profile = Data[i, 1];
                        break;
                    }
                }
            }
        }

           
    }
}
