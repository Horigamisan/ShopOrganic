using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace WebDemo.Helper
{
    public class NormalizeTwoTextVN
    {
        public static string Normalize(string text)
        {
            var textNomarlize = text.ToLower().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var item in textNomarlize)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(item) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(item);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormD);
        }
    }
}