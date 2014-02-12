using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Vsa;
using Microsoft.JScript.Vsa;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SAC.Helper
{
    public class StringHelper
    {

        /// <summary>
        /// 数组排序(冒泡)
        /// </summary>
        /// <param name="arr">排序数组</param>
        /// <returns></returns>
        public string[] GetArraySort(string[] arr)
        {
            string temp;

            for (int j = 0; j < arr.Length; j++)
            {
                for (int i = arr.Length - 1; i > j; i--)
                {
                    if (arr[i] != null && arr[j] != null)
                    {
                        if (double.Parse(arr[j]) < double.Parse(arr[i]))
                        {
                            temp = arr[j];
                            arr[j] = arr[i];
                            arr[i] = temp;
                        }
                    }
                }
            }

            return arr;
        }

        /// <summary>
        /// 计算方法
        /// </summary>
        /// <param name="formule">计算公式</param>
        /// <param name="num">显示小数位数</param>
        /// <param name="statues">数值状态</param>
        /// <returns></returns>
        public string GetCaleFormule(string formule, string num, out string statues)
        {
            double val = 0;
            string res = "0";

            if (num == "-1")
                num = "4";

            try
            {
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();

                res = Microsoft.JScript.Eval.JScriptEvaluate(formule, ve).ToString();

                statues = res;

                if (res == "0")
                { return "0"; }
                else if (res == "非数字")
                { return "0"; }
                else if (res == "正无穷大")
                { return "0"; }
                else if (res == "负无穷大")
                { return "0"; }
                else if (res == "" || res == null)
                { return "0"; }
                else
                {
                    try
                    { val = Convert.ToDouble(res); }
                    catch
                    { val = 0; }
                }
                return val.ToString("f" + num);

            }
            catch (Exception ce)
            {
                statues = ce.Message;
            }

            return res;
        }

        /// <summary>
        /// 将数字转换成中文
        /// </summary>
        /// <param name="str">阿拉伯数字</param>
        /// <param name="appendStr">追加字符串</param>
        /// <param name="fontType">type:大写 or any string</param>
        /// <returns></returns>
        public string ConvertChinese(string str, string appendStr, string fontType)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億圆整角分"
            string cstr = "";

            if (fontType == "大写")
            {
                switch (str)
                {
                    case "0": cstr = "零"; break;
                    case "1": cstr = "壹"; break;
                    case "2": cstr = "贰"; break;
                    case "3": cstr = "叁"; break;
                    case "4": cstr = "肆"; break;
                    case "5": cstr = "伍"; break;
                    case "6": cstr = "陆"; break;
                    case "7": cstr = "柒"; break;
                    case "8": cstr = "捌"; break;
                    case "9": cstr = "玖"; break;
                }
            }
            else
            {
                switch (str)
                {
                    case "0": cstr = "零"; break;
                    case "1": cstr = "一"; break;
                    case "2": cstr = "二"; break;
                    case "3": cstr = "三"; break;
                    case "4": cstr = "四"; break;
                    case "5": cstr = "五"; break;
                    case "6": cstr = "六"; break;
                    case "7": cstr = "七"; break;
                    case "8": cstr = "八"; break;
                    case "9": cstr = "九"; break;
                }
            }

            return cstr + appendStr;
        }

        /// <summary>
        /// 处理形如'{0}'，而参数中又有单引号的情况，将参数中的单引号double
        /// 如果args中的单引号不是作为文本的话，不要用这个方法，直接用string.format()
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static public string stringFormat(string format, params object[] args)
        {
            if (format.IndexOf("'") == -1) //没有在单引号内的占位符
                return string.Format(format, args);

            ArrayList al = new ArrayList();
            for (int i = 0; i < args.Length; i++)
                al.Add(args[i].ToString().Replace("'", "''")); // 处理单引号
            return string.Format(format, al.ToArray());
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetFirstString(string stringToSub, int length)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + ". . .";
            else
                return sb.ToString();


        }
    }
}
