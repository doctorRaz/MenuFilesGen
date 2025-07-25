using MenuFilesGen.Models;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {
        // По здравому размышлению понял, что получение колонок для парсера интересно только здесь. Переделываю ;)
        /// <summary> Чтение файла обмена (tsv) разделитель табуляция, кодировка UTF8 </summary>
        /// <param name="fileFullName">Полный путь к обрабатываемому файлу</param>
        /// <param name="columnNumbers">Настройки парсинга</param>
        public void ReadFromTsv()
        {
            //List<string[]> datas = null;
            try
            {
                using (StreamReader reader = new StreamReader(FileFullName, Encoding.UTF8))
                {
                    datas = reader.ReadToEnd()
                        .Split(newLine, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(HEADER_ROW_RANGE)
                        .Select(o => o.Split('\t'))
                        .Where(c => !(c[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return;
            }

            PanelDefinitions = new List<PanelDefinition>(
            datas
                .Select(o => o[columnNumbers.PanelNameColumn])
                .Distinct()
                .Select(o => new PanelDefinition()
                { NameRaw = o })
        );

            RibbonPaletteDefinitions = new List<RibbonPaletteDefinition>(
                datas
                    .Select(o => o[columnNumbers.RibbonSplitButtonColumn])
                    .Distinct()
                    .Select(o => new RibbonPaletteDefinition()
                    {
                        Name = o
                    })
                );

            GetCmdDefTxt(/*datas*/);

        }
    }
}
