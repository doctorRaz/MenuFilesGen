using MenuFilesGen.Models;
using MenuFilesGen.Service;
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
            List<string[]> datas = null;
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
                { Name = o })
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

            CommandDefinitions = new List<CommandDefinition>(
                datas.Select(o =>
                {
                    CommandDefinition res = new CommandDefinition
                    {
                        DispName = o[columnNumbers.DispNameColumn].Trim(),
                        InterName = o[columnNumbers.InternameColumn].Trim(),
                        StatusText = o[columnNumbers.StatusTextColumn].Trim(),
                        IconName = o[columnNumbers.IconColumn].Trim(),
                        ResourceDllName = o[columnNumbers.ResourseDllNameColumn].Trim(),
                        PanelName = o[columnNumbers.PanelNameColumn].Trim(),

                        RibbonSplitButtonName = o[columnNumbers.RibbonSplitButtonColumn].Trim(),
                        RibbonSize = o[columnNumbers.RibbonSizeColumn].Trim(),
                        AppName = o[columnNumbers.AppNameColumn].Trim(),
                        HideCommand = o[columnNumbers.HideCommandColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                        DontTake = o[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                        LocalName = o[columnNumbers.LocalNameColumn].Trim(),
                        RealCommandName = o[columnNumbers.RealCommandNameColumn].Trim(),
                        Keyword = o[columnNumbers.KeywordColumn].Trim(),
                        Weight = Utils.StringToInt(o[columnNumbers.WeightColumn], 10),
                        CmdType = Utils.StringToInt(o[columnNumbers.CmdTypeColumn], 1),
                        ToolTipText = o[columnNumbers.ToolTipTextColumn].Trim(),
                        Accelerators = o[columnNumbers.AcceleratorsColumn].Trim(),
                        AddonName = o[columnNumbers.AddonNameColumn].Trim(),
                    };
                    return res;
                })
            );
        }
    }
}
