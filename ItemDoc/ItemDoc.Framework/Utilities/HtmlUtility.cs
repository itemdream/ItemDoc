using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
 


namespace ItemDoc.Framework.Utilities
{
    /// <summary>
    /// Html工具类
    /// </summary>
    public class HtmlUtility
    {
        /// <summary>
        /// 多行纯文本型转化为可以在HTML中显示
        /// </summary>
        /// <remarks>
        /// 一般在存储到数据库之前进行转化
        /// </remarks>
        /// <param name="plainText">需要转化的纯文本</param>
        /// <param name="keepWhiteSpace">是否保留空格</param>
        public static string FormatMultiLinePlainTextForStorage(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (keepWhiteSpace)
            {
                plainText = plainText.Replace(" ", "&nbsp;");
                plainText = plainText.Replace("\t", "&nbsp;&nbsp;");
            }
            plainText = plainText.Replace("\r\n", System.Environment.NewLine);
            plainText = plainText.Replace("\n", System.Environment.NewLine);

            return plainText;
        }

        /// <summary>
        /// 多行纯文本型转化为可以在TextArea中正常显示
        /// </summary>
        /// <remarks>
        /// 一般在进行编辑前进行转化
        /// </remarks>
        /// <param name="plainText">需要转化的纯文本</param>
        /// <param name="keepWhiteSpace">是否保留空格</param>
        public static string FormatMultiLinePlainTextForEdit(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            string result = plainText;
            result = result.Replace(System.Environment.NewLine, "\n");
            if (keepWhiteSpace)
                result = result.Replace("&nbsp;", " ");

            return result;
        }


        /// <summary>
        /// 清除标签名称中的非法字词
        /// </summary>
        public static string CleanTagName(string appKey)
        {
            //Remark:20090808_zhengw 删除Url中可编码的特殊字符：'#','&','=','/','%','?','+', '$',
            string[] parts = appKey.Split('!', '.', '@', '^', '*', '(', ')', '[', ']', '{', '}', '<', '>', ',', '\\', '\'', '~', '`', '|');
            appKey = string.Join("", parts);
            return appKey;
        }

        /// <summary>
        /// 友好的文件大小信息
        /// </summary>
        /// <param name="fileSize">文件字节数</param>
        public static string FormatFriendlyFileSize(double fileSize)
        {
            if (fileSize > 0)
            {
                if (fileSize > 1024 * 1024 * 1024)
                    return string.Format("{0:F2}G", (fileSize / (1024 * 1024 * 1024F)));
                else if (fileSize > 1024 * 1024)
                    return string.Format("{0:F2}M", (fileSize / (1024 * 1024F)));
                else if (fileSize > 1024)
                    return string.Format("{0:F2}K", (fileSize / (1024F)));
                else
                    return string.Format("{0:F2}Bytes", fileSize);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 格式化评论内容
        /// </summary>
        /// <param name="text">格式化的内容</param>
        /// <param name="enableNoFollow">Should we include the nofollow rel.</param>
        /// <param name="enableConversionToParagraphs">Should newlines be converted to P tags.</param>
        private static string FormatPlainTextComment(string text, bool enableNoFollow = true, bool enableConversionToParagraphs = true)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = WebUtility.HtmlEncode(text);

            if (enableNoFollow)
            {
                //Find any links
                StringCollection uniqueMatches = new StringCollection();

                string pattern = @"(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
                MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                foreach (Match m in matches)
                {
                    if (!uniqueMatches.Contains(m.ToString()))
                    {
                        text = text.Replace(m.ToString(), "<a rel=\"nofollow\" target=\"_new\" href=\"" + m + "\">" + m.ToString().Trim() + "</a>");
                        uniqueMatches.Add(m.ToString());
                    }
                }
            }

            // Replace Line breaks with <br> and every other concurrent space with &nbsp; (to allow line breaking)
            if (enableConversionToParagraphs)
                text = ConvertPlainTextToParagraph(text);// text.Replace("\n", "<br />");

            text = text.Replace("  ", " &nbsp;");

            return text;
        }

        /// <summary>
        /// 把纯文字格式化成html段落
        /// </summary>
        /// <remarks>
        /// 使文本在Html中保留换行的格式
        /// </remarks>
        private static string ConvertPlainTextToParagraph(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace("\r\n", "\n").Replace("\r", "\n");

            string[] lines = text.Split('\n');

            StringBuilder paragraphs = new StringBuilder();

            foreach (string line in lines)
            {
                if (line != null && line.Trim().Length > 0)
                    paragraphs.AppendFormat("{0}<br />\n", line);
            }
            return paragraphs.ToString().Remove(paragraphs.ToString().LastIndexOf("<br />", StringComparison.Ordinal));
        }
        /// <summary>
        /// 移除html内的Elemtnts/Attributes及&amp;nbsp;，超过charLimit个字符进行截断
        /// </summary>
        /// <param name="rawHtml">待截字的html字符串</param>
        /// <param name="charLimit">最多允许返回的字符数</param>
        public static string TrimHtml(string rawHtml, int charLimit)
        {
            if (string.IsNullOrEmpty(rawHtml))
            {
                return string.Empty;
            }

            string nohtml = StripHtml(rawHtml, true, false);
            nohtml = StripBBTags(nohtml);

            if (charLimit <= 0 || charLimit >= nohtml.Length)
                return nohtml;
            else
                return StringUtility.Trim(nohtml, charLimit);
        }

        /// <summary>
        /// 移除Html标签
        /// </summary>
        /// <param name="rawString">待处理字符串</param>
        /// <param name="removeHtmlEntities">是否移除Html实体</param>
        /// <param name="enableMultiLine">是否保留换行符（<p/><br/>会转换成换行符）</param>
        /// <returns>返回处理后的字符串</returns>
        public static string StripHtml(string rawString, bool removeHtmlEntities, bool enableMultiLine)
        {
            if (string.IsNullOrEmpty(rawString))
            {
                return rawString;
            }

            string result = rawString;
            if (enableMultiLine)
            {
                result = Regex.Replace(result, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                result = Regex.Replace(result, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            result = result.Replace("\"", "''");
            if (removeHtmlEntities)
            {
                //StripEntities removes the HTML Entities
                result = Regex.Replace(result, "&[^;]*;", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            return Regex.Replace(result, "<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 移除Html用于内容预览
        /// </summary>
        /// <remarks>
        /// 将br、p替换为\n，“'”替换为对应Html实体，并过滤所有Html、Xml、UBB标签
        /// </remarks>
        /// <param name="rawString">用于预览的文本</param>
        /// <returns>返回移除换行及html、ubb标签的字符串</returns>
        public static string StripForPreview(string rawString)
        {
            if (string.IsNullOrEmpty(rawString))
            {
                return rawString;
            }

            string tempString;

            tempString = rawString.Replace("<br>", "\n");
            tempString = tempString.Replace("<br/>", "\n");
            tempString = tempString.Replace("<br />", "\n");
            tempString = tempString.Replace("<p>", "\n");
            tempString = tempString.Replace("'", "&#39;");

            tempString = StripHtml(tempString, false, false);
            tempString = StripBBTags(tempString);

            return tempString;
        }

        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="content">待处理的字符串</param>
        /// <remarks>处理后的字符串</remarks>
        public static string StripBBTags(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }

            return Regex.Replace(content, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 移除script标签
        /// Helper function used to ensure we don't inject script into the db.
        /// </summary>
        /// <remarks>
        /// 移除&lt;script&gt;及javascript:
        /// </remarks>
        /// <param name="rawString">待处理的字符串</param>
        /// <remarks>处理后的字符串</remarks>
        public static string StripScriptTags(string rawString)
        {
            if (string.IsNullOrEmpty(rawString))
            {
                return rawString;
            }

            // Perform RegEx
            rawString = Regex.Replace(rawString, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            rawString = rawString.Replace("\"javascript:", "\"");

            return rawString;
        }
     
    }
}