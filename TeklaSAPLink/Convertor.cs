using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v1;
using System.IO;
using CommonClassLibrary;

using Tekla.Structures.Model;
using Tekla.Structures.Analysis;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using Tekla.Structures;

namespace TeklaSAPLink
{
    public static class Convertor
    {        
        public static void GenerateLinkFile(List<MyBeam> mbs)//создание файла связки 
        {
            string path = Environment.CurrentDirectory;
            Model m = new Model();
            ModelInfo modelInfo = m.GetInfo();
            string[] sep1 = { "." };
            string FileLinkName = "LinkFile_" + modelInfo.ModelName.Split(sep1, StringSplitOptions.RemoveEmptyEntries)[0] + ".txt";
            if (File.Exists(path + "\\" + FileLinkName))
            {
                System.Windows.MessageBox.Show("File is found");
            }
            else
            {                
                foreach(MyBeam mb in mbs)
                {
                    using (var writer = new StreamWriter(new FileStream(path + "\\" + FileLinkName, FileMode.Append, FileAccess.Write)))
                    {
                        writer.WriteLine(mb.tekla_ID + ";" + mb.SAP_ID + ";" + mb.XYZ1[0] + ";" +mb.XYZ1[1] + ";" +mb.XYZ1[2] + ";" +mb.XYZ2[0] + ";" +mb.XYZ2[1] + ";" +mb.XYZ2[2] + ";" +
                            mb.tekla_Material + ";" + mb.SAP_Material + ";" + mb.tekla_Profile + ";" + mb.SAP_Profile + ";" + mb.Type + ";" + mb.Orientaion);
                    }
                }
            }
        }
        public static List<MyBeam> MyBeamsByLinkFile(string LinkFile)//получение информации о LinkFile
        {
            List<MyBeam> ret = new List<MyBeam>();
            string[] sep1 = { "\r\n" };
            string[] sep2 = { ";" };
            string[] s1 = LinkFile.Split(sep1, StringSplitOptions.RemoveEmptyEntries);            
            for(int i = 0; i < s1.Length; i++)
            {
                string[] s2 = s1[i].Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                ret.Add(new MyBeam());
                ret.LastOrDefault().SAP_ID = s2[0];
                ret.LastOrDefault().tekla_ID = s2[1];
                ret.LastOrDefault().XYZ1[0] = Convert.ToDouble(s2[2]);
                ret.LastOrDefault().XYZ1[1] = Convert.ToDouble(s2[3]);
                ret.LastOrDefault().XYZ1[2] = Convert.ToDouble(s2[4]);
                ret.LastOrDefault().XYZ2[0] = Convert.ToDouble(s2[5]);
                ret.LastOrDefault().XYZ2[1] = Convert.ToDouble(s2[6]);
                ret.LastOrDefault().XYZ2[2] = Convert.ToDouble(s2[7]);
                ret.LastOrDefault().tekla_Material = s2[8];
                ret.LastOrDefault().SAP_Material = s2[9];
                ret.LastOrDefault().tekla_Profile = s2[10];
                ret.LastOrDefault().SAP_Profile = s2[11];
            }
            return ret;
        }        
        public static string [,] MappingSectionsFileData()//находим соответствие сечения в SAP
        {            
            #region сбор информации из файла соответствия
            string s = Properties.Resources.MappingSectionsFile;
            string[] sep1 = { "\r\n" };
            string[] sep2 = { ";" };
            string[] s1 = s.Split(sep1, StringSplitOptions.RemoveEmptyEntries);
            string[] s2 = s1[0].Split(sep2, StringSplitOptions.RemoveEmptyEntries);
            string[,] ss = new string[s1.Length-1, s2.Length];
            for (int i1 = 0; i1 < s1.Length-1; i1++)
            {
                s2 = s1[i1+1].Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                for (int i2 = 0; i2 < s2.Length; i2++)
                {
                    ss[i1, i2] = s2[i2];
                }
            }
            #endregion
            return ss;
        }
        public static Dictionary<string, List<MyBeam>> CheckTeklaChanges(string LinkFile)//
        {
            Dictionary<string, List<MyBeam>> ret = new Dictionary<string, List<MyBeam>>();
            List<MyBeam> LinkBeams = Convertor.MyBeamsByLinkFile(LinkFile);
            List<MyBeam> TeklaBeams = TeklaCommands.GetBeamsAndCollumnObjects();

            foreach (MyBeam mbt in TeklaBeams)
            {
                
            }
            return ret;
        }
    }
}
