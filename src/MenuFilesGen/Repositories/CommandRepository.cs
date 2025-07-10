using System.Text;
using MenuFilesGen.Enums;
using MenuFilesGen.Models;

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
                    .Select(o => o[columnNumbers.RibbonNameColumn])
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
                        MenuCommandName = o[columnNumbers.CommandNameColumn],
                        InternalName = o[columnNumbers.CommandInternaleNameColumn],
                        Description = o[columnNumbers.CommandDescriptionColumn],
                        IconName = o[columnNumbers.IconColumn],
                        ResourceDllName = o[columnNumbers.ResourseDllNameColumn],
                        HideCommand = false,
                        Panel = PanelDefinitions.Where(p => p.Name.Equals(o[columnNumbers.PanelNameColumn],
                            StringComparison.InvariantCultureIgnoreCase))
                            .ToList(),
                        RibbonPanel = RibbonPaletteDefinitions
                            .Where(p => p.Name.Equals(o[columnNumbers.RibbonNameColumn]))
                            .ToList(),
                        RibbonSize = RibbonButtonSize.None
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

        public List<CommandDefinition> CommandDefinitions { get; private set; }
        public List<PanelDefinition> PanelDefinitions { get; private set; }
        public List<RibbonPaletteDefinition> RibbonPaletteDefinitions { get; private set; }

        private const int HEADER_ROW_RANGE = 1;
    }
}
