using MenuFilesGen.Enums;
using MenuFilesGen.Models;

namespace MenuFilesGen.Repositories
{
    public class CommandRepository
    {
        public CommandRepository(ColumnNumbers columnNumbers)
        {
            _columnNumbers = columnNumbers;
        }
        public void ReadFromCsv(string csvFileFullName)
        {
            List<string[]> datas;
            using (StreamReader reader = new StreamReader(csvFileFullName))
            {
                datas = reader.ReadToEnd()
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(HEADER_ROW_RANGE)
                    .Select(o => o.Split('\t'))
                    .ToList();
            }

            PanelDefinitions = new List<PanelDefinition>(
                datas
                    .Select(o => o[_columnNumbers.PanelNameColumn])
                    .Distinct()
                    .Select(o => new PanelDefinition()
                    { Name = o })
            );

            RibbonPaletteDefinitions = new List<RibbonPaletteDefinition>(
                datas
                    .Select(o => o[_columnNumbers.RibbonNameColumn])
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
                        MenuCommandName = null,
                        InternalName = null,
                        Description = null,
                        IconName = null,
                        ResourceDllName = null,
                        HideCommand = false,
                        Panel = null,
                        RibbonPanel = null,
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


        private ColumnNumbers _columnNumbers;
        private const int HEADER_ROW_RANGE = 1;
    }
}
