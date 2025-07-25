using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {
        /// <summary> Чтение файла обмена (csv) разделитель точка с запятой, кодировка ASCI </summary>
        public void ReadFromCsv()
        {


            //List<string[]> datas = null;
            try
            {
                using (StreamReader reader = new StreamReader(FileFullName, Encoding.GetEncoding(1251)))
                {
                    datas = reader.ReadToEnd()
                       .Split(newLine, StringSplitOptions.RemoveEmptyEntries)
                       .Skip(HEADER_ROW_RANGE)
                       .Select(x => x.Split(';'))
                          .Where(c => !(c[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase)))
                          .ToList();

                }
            }
            catch (Exception ex)
            {
                return;
            }

            GetCmdDefTxt(/*datas*/);


        }
    }
}
