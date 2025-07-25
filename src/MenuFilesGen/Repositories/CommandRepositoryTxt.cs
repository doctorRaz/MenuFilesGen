namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {
        /// <summary> Чтение файла обмена  разделитель splitChar , кодировка encoding </summary>
        public void ReadFromTxt()
        {
            try
            {
                using (StreamReader reader = new StreamReader(FileFullName, encoding))
                {
                    datas = reader.ReadToEnd()
                       .Split(newLine, StringSplitOptions.RemoveEmptyEntries)
                       .Skip(HEADER_ROW_RANGE)
                       .Select(x => x.Split(splitChar))
                          .Where(c => !(c[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase)))
                          .ToList();

                }
            }
            catch (Exception ex)
            {
                return;
            }

            GetCmdDefTxt();//сбор описаний команд

        }
    }
}
