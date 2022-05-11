using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CommonClassLibrary
{
    /*  Новая версия класса Work_with_TXT от 05.12.2017
     *  Исправлены принципы работы:
     *  1) Добавлены раздельные свойства класса: имя файла, расширение, путь к папке с файлом и полный путь файла     *  
     * 
     */
    public class Work_with_TXT
    {
        string FilePathWithName_prop; // переменная свойства, определяющее путь файла считывания включая имя файла
        string FileName_prop; //имя файла
        string FileExpantion_prop; //расширение файла
        string FilePath_prop;

        public string FilePathWithName
        {
            get
            {
                return FilePathWithName_prop;
            }
            private set
            {
                FilePathWithName_prop = value;
            }
        }
        public string FilePath
        {
            get
            {
                return FilePath_prop;
            }
            private set
            {
                FilePath_prop=value;
            }
        }
        public string FileName
        {
            get
            {
                return FileName_prop;
            }
            private set
            {
                FileName_prop = value;
            }
        }
        public string FileExpantion
        {
            get
            {
                return FileExpantion_prop;
            }
            private set
            {
                FileExpantion_prop = value;
            }
        }


        //конструктор класса        
        public Work_with_TXT(string PATH)
        {
            this.FilePathWithName = PATH;
            string [] str1 = { "\\"};
            string[] str2 = { "." };
            string[] s1 = PATH.Split(str1, StringSplitOptions.RemoveEmptyEntries);
            string[] s2 = s1.LastOrDefault().Split(str2, StringSplitOptions.RemoveEmptyEntries);

            this.FileName = s2.FirstOrDefault();
            this.FileExpantion = s2.LastOrDefault();
            for(int i=0; i < s1.GetLength(0)-1; i++)
            {
                this.FilePath = this.FilePath + s1[i] + "\\";
            }
            this.FilePath = this.FilePath.Remove(this.FilePath.Length - 1);
        }

        //процедура считывания строки из TXT
        public string Load_TXT()
        {
            string ret;
            FileStream fin;

            fin = new FileStream(FilePathWithName, FileMode.Open);
            StreamReader fstr_in = new StreamReader(fin, System.Text.Encoding.Default);
            try
            {
                ret = fstr_in.ReadToEnd();
            }
            catch (IOException exc)
            {
                ret = exc.Message;
            }
            finally
            {
                fstr_in.Close();
            }
            return ret;
        }
        public string Load_TXT_xml()
        {
            string ret;
            FileStream fin;

            fin = new FileStream(FilePathWithName, FileMode.Open);
            StreamReader fstr_in = new StreamReader(fin, System.Text.Encoding.UTF8);
            try
            {
                ret = fstr_in.ReadToEnd();
            }
            catch (IOException exc)
            {
                ret = exc.Message;
            }
            finally
            {
                fstr_in.Close();
            }
            return ret;
        }

        public void Write(List<string> Output_info, string ADD_NAME)
        {
            FileStream fin;
            fin = new FileStream(FilePath + "\\" + FileName + "_" + ADD_NAME + ".txt", FileMode.Append);
            StreamWriter fstr_out = new StreamWriter(fin, System.Text.Encoding.Default);
            try
            {
                foreach (string s in Output_info)
                {
                    fstr_out.WriteLine(s);
                }
            }
            catch (IOException exc)
            {
                
            }
            finally
            {
                fstr_out.Close();
            }
        }
        public void Write(string Output_info, string ADD_NAME)
        {
            FileStream fin;
            fin = new FileStream(FilePath + "\\" + FileName + "_" + ADD_NAME + ".txt", FileMode.Append);
            StreamWriter fstr_out = new StreamWriter(fin, System.Text.Encoding.Default);
            try
            {
                fstr_out.WriteLine(Output_info);
            }
            catch (IOException exc)
            {

            }
            finally
            {
                fstr_out.Close();
            }
        }

        //сохранение текстового файла
        public void Save_TXT(string TEXT, string ADD_NAME)
        {            
            File.WriteAllText(FilePath + "\\" + FileName + "" +ADD_NAME+ ".txt", TEXT);
        }
        public void Save_XML(string TEXT, string ADD_NAME)
        {
            File.WriteAllText(FilePath + "\\" + FileName + "" + ADD_NAME + ".xml", TEXT);
        }
    }
}
