namespace MenuFilesGen.Service
{

    /// <summary>
    /// Список в строку, разделитель новая линия
    /// </summary>
    public static class ExtensionsBlockFix
    {
        public static string LstStr(this List<string> lst)
        {
            string str = string.Join(Environment.NewLine, lst.ToArray());
            return str;
        }


        public static List<string> RawSplit(this string str)
        {

            return str.Split('|').ToList();
        }
    }
}
