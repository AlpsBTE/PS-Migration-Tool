using System.Globalization;
using System.Text;

namespace PS_Migration_Tool.Helpers
{
    internal static class StringExtensions
    {
        // https://stackoverflow.com/a/72128774
        private static readonly IReadOnlyDictionary<string, string> SpecialDiacritics = new Dictionary<string, string>
                                                                   {
                                                                        { "ä".Normalize(NormalizationForm.FormD), "ae".Normalize(NormalizationForm.FormD) },
                                                                        { "Ä".Normalize(NormalizationForm.FormD), "Ae".Normalize(NormalizationForm.FormD) },
                                                                        { "ö".Normalize(NormalizationForm.FormD), "oe".Normalize(NormalizationForm.FormD) },
                                                                        { "Ö".Normalize(NormalizationForm.FormD), "Oe".Normalize(NormalizationForm.FormD) },
                                                                        { "ü".Normalize(NormalizationForm.FormD), "ue".Normalize(NormalizationForm.FormD) },
                                                                        { "Ü".Normalize(NormalizationForm.FormD), "Ue".Normalize(NormalizationForm.FormD) },
                                                                        { "ß".Normalize(NormalizationForm.FormD), "ss".Normalize(NormalizationForm.FormD) },
                                                                   };

        public static string RemoveDiacritics(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            var stringBuilder = new StringBuilder(s.Normalize(NormalizationForm.FormD));

            // Replace certain special chars with special combinations of ascii chars (eg. german umlauts and german double s)
            foreach (KeyValuePair<string, string> keyValuePair in SpecialDiacritics)
                stringBuilder.Replace(keyValuePair.Key, keyValuePair.Value);

            // Remove other diacritic chars eg. non spacing marks https://www.compart.com/en/unicode/category/Mn
            for (int i = 0; i < stringBuilder.Length; i++)
            {
                char c = stringBuilder[i];

                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                    stringBuilder.Remove(i, 1);
            }

            return stringBuilder.ToString();
        }
    }
}