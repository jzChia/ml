using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PanGu;
using System.Text.RegularExpressions;

namespace userControls
{
    public class createKeywords
    {
        public static List<String> DisplaySegment(String textBoxSource, out String tipswords)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var words = new Segment().DoSegment(textBoxSource);
            stopwatch.Stop();
            tipswords = String.Format("源字符串长度：\n{0} \n分词时间：\n{1} \n分词速度：\n", textBoxSource.Length, stopwatch.Elapsed);

            if (stopwatch.ElapsedMilliseconds == 0L)
            {
                tipswords += "无穷大";
            }
            else
            {
                tipswords += ((((long)textBoxSource.Length) / stopwatch.ElapsedMilliseconds) * 1000.0).ToString();
            }
            List<String> wordss = new List<string>();
            foreach (WordInfo info in words)
            {
                if (info != null && Regex.IsMatch(info.Word,"[\u4e00-\u9fa5]"))
                {
                    wordss.Add(info.Word);
                }
            }

            tipswords  += " char/s";
            return wordss;
        }
    }
}
