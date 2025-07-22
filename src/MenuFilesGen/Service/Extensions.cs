namespace MenuFilesGen.Service
{

    /// <summary>
    /// Список в строку, разделитель новая линия
    /// </summary>
    public static class ExtensionsBlckFix
    {
        public static string LstStr(this List<string> lst)
        {
            string str = string.Join(Environment.NewLine, lst.ToArray());
            return str;
        }

    }
}
