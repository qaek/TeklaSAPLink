using System;
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
    public class MyBeam
    {  
        public string Type; //тип сечения для правильного поиска сечений в SAP (двутавр, труба, уголок или профиль общего типа)
        public double [] XYZ1 = new double[3];
        public double [] XYZ2 = new double[3];
        public double Orientaion;

        public string tekla_ID;
        public string SAP_ID;
        public string tekla_Material;
        public string tekla_Profile;
        public string SAP_Material;
        public string SAP_Profile;

        public bool IsCreated;//если элемент был создан
        public bool IsChanged;//если элемент был изменен
        public bool IsDeleted;//если элемент был удален

        public MyBeam()
        {

        }
                
    }
}
