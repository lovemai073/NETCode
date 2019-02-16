using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClearOldFile
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> delFilePaths = LoadFilePaths();
            ClearOldLogFiles(delFilePaths, DelDays());
            //Console.Read();
        }
        private static void ClearOldLogFiles(List<string> filepaths,int beforeDays)
        {
            try
            {
                foreach(string itemPath in filepaths)
                {
                    Console.WriteLine(itemPath);
                    DirectoryInfo DirInfo = new DirectoryInfo(itemPath);
                    FileInfo[] filesArray = DirInfo.GetFiles();
                    if (filesArray.Length > 0)
                    {
                        for (int i = 0; i < filesArray.Length; i++)
                        {
                            string fileName = filesArray[i].Name;
                            DateTime dtFileCreationTime = filesArray[i].CreationTime;
                            TimeSpan ts = DateTime.Now - dtFileCreationTime;
                            int diffDays = ts.Days;
                            if (diffDays >= beforeDays)
                            {
                                try
                                {
                                    Console.WriteLine(itemPath + fileName);
                                    File.Delete(itemPath + fileName);
                                }
                                catch (Exception e)
                                {
                                    //if path not exist skip
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static List<string> LoadFilePaths()
        {
            List<string> listData = new List<string>();
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + $"/Path.json");
            var ConfigJson = JObject.Parse(reader.ReadToEnd());
            reader.Close();
            var JData = ConfigJson.SelectToken($@".DataPaths");
            foreach (var jitem in JData)
            {
                listData.Add(jitem.ToString());
            }
            return listData;
        }

        public static int DelDays()
        {
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + $"/Path.json");
            var ConfigJson = JObject.Parse(reader.ReadToEnd());
            reader.Close();
            var deldays = ConfigJson.SelectToken($@".DelDays");
            return Convert.ToInt32(deldays);
        }
    }
}
