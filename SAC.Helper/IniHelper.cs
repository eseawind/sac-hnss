using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;

namespace SAC.Helper
{
    public class IniHelper
    {
        static string mapPath = "";
        //System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

        #region   API函数声明

        [DllImport("kernel32 ")]//返回0表示失败，非0为成功 
        private static extern long WritePrivateProfileString(string section, string key,
                string val, string filePath);

        [DllImport("kernel32 ")]//返回取得字符串缓冲区的长度 
        private static extern long GetPrivateProfileString(string section, string key,
                string def, StringBuilder retVal, int size, string filePath);

        #endregion

        #region   读Ini文件

        public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region   读Ini文件

        public static string ReadIniData(string Section, string Key, string NoText)
        {
            mapPath = System.AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationSettings.AppSettings["conName"].ToString();

            if (File.Exists(mapPath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, mapPath);
                string a = temp.ToString();
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region   写Ini文件

        public static bool WriteIniData(string Section, string Key, string Value)
        {
            mapPath = System.AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationSettings.AppSettings["conName"].ToString();

            if (File.Exists(mapPath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, mapPath);

                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion



    }
}
