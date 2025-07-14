using System.Text;
using MenuFilesGen.Enums;
using MenuFilesGen.Models;
using MenuFilesGen.Service;

namespace MenuFilesGen.Repositories
{
    public class CommandRepository
    {
        // По здравому размышлению понял, что получение колонок для парсера интересно только здесь. Переделываю ;)
        /// <summary> Чтение файла обмена (csv) </summary>
        /// <param name="csvFileFullName">Полный путь к обрабатываемому файлу</param>
        /// <param name="columnNumbers">Настройки парсинга</param>
        public void ReadFromCsv(string csvFileFullName, ColumnNumbers columnNumbers)
        {
            List<string[]> datas;
            using (StreamReader reader = new StreamReader(csvFileFullName, Encoding.UTF8))
            {
                datas = reader.ReadToEnd()
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(HEADER_ROW_RANGE)
                    .Select(o => o.Split('\t'))
                    .Where(c => !(c[columnNumbers.DontTakeColumn].Trim() == "ИСТИНА"))
                    .ToList();
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
                        //HideCommand = false,
                        RibbonSplitButtonName = o[columnNumbers.RibbonSplitButtonColumn].Trim(),
                        RibbonSize = o[columnNumbers.RibbonSizeColumn].Trim(),
                        Root = o[columnNumbers.RootColumn].Trim(),
                        DontMenu = o[columnNumbers.DontMenuColumn].Trim().ToUpper() == "ИСТИНА",
                        DontTake = o[columnNumbers.DontTakeColumn].Trim().ToUpper() == "ИСТИНА",
                        LocalName=o[columnNumbers.LocalNameColumn].Trim(),
                        RealCommandName = o[columnNumbers.RealCommandNameColumn].Trim(),
                        Keyword = o[columnNumbers.KeywordColumn].Trim(),
                        Weight =Utils.StringToInt( o[columnNumbers.WeightColumn].Trim(),10),
                        CmdType =Utils.StringToInt( o[columnNumbers.CmdTypeColumn].Trim(),1),
                    };




                    return res;
                })
            );
        }

        public void SaveToCfg(string cfgFileName)
        {
            throw new NotImplementedException();
        }

        public void SaveToCuix(string cuixFileName)
        {
            throw new NotImplementedException();
        }

        public dynamic HierarchicalGrouping
        {
            get
            {
                return CommandDefinitions
                     .GroupBy(e => e.Root)
                     .Select(root => new
                     {
                         root = root.Key,
                         panel = root
                     .GroupBy(e => e.PanelName)
                     .Select(panel => new
                     {
                         panel = panel.Key,
                         command = panel.ToList()
                     }).ToList()
                     }).ToList();

            }
        }
        public List<CommandDefinition> CommandDefinitions { get; private set; }
        public List<PanelDefinition> PanelDefinitions { get; private set; }
        public List<RibbonPaletteDefinition> RibbonPaletteDefinitions { get; private set; }

        private const int HEADER_ROW_RANGE = 3;
    }
}
