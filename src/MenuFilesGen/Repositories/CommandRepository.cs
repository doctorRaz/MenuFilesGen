using ClosedXML.Excel;
using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public class CommandRepository
    {

        public CommandRepository(string _fileFullName)
        {
            fileFullName = _fileFullName;

            string fileExtension = Path.GetExtension(fileFullName);

            //обработчик по расширению файла перенаправлять в свой читатель
            if (fileExtension.Contains("xls", StringComparison.OrdinalIgnoreCase))
            {
                ReadFromXls();
            }
            else if (fileExtension.Contains("csv", StringComparison.OrdinalIgnoreCase))//разделитель ; ASCI
            {
                addinName = Path.GetFileNameWithoutExtension(fileFullName);

                ReadFromCsv();
            }
            else//разделитель tab юникод или tsb
            {
                addinName = Path.GetFileNameWithoutExtension(fileFullName);

                ReadFromTsv();
            }

        }

        /// <summary> Чтение файла обмена (xls)</summary>
        public void ReadFromXls()
        {
            XLWorkbook workbook = new XLWorkbook(new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

            Console.WriteLine("Введите номер листа:");
            int wscount = 1;
            foreach (IXLWorksheet _ws in workbook.Worksheets)
            {
                Console.WriteLine($"{wscount}.\t{_ws.Name}");
                wscount++;
            }
            string? wsNumber = Console.ReadLine();

            int number = Utils.StringToInt(wsNumber);

            if (number < 1 || number > workbook.Worksheets.Count)//не число
            {
                Console.WriteLine("Такого листа нет");
                return;
            }

            IXLWorksheet worksheet = workbook.Worksheet(number);

            addinName = worksheet.Name;
            Console.WriteLine($"Работаю с листом {addinName}");

            IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed().RowsUsed().Skip(HEADER_ROW_RANGE);

            CommandDefinitions = new List<CommandDefinition>();

            foreach (IXLRangeRow row in rows)
            {
                CommandDefinition res = new CommandDefinition
                {
                    DispName = row.Cell(columnNumbers.DispNameColumn + 1).GetString().Trim(),
                    InterName = row.Cell(columnNumbers.InternameColumn + 1).GetString().Trim(),
                    StatusText = row.Cell(columnNumbers.StatusTextColumn + 1).GetString().Trim(),
                    IconName = row.Cell(columnNumbers.IconColumn + 1).GetString().Trim(),
                    ResourceDllName = row.Cell(columnNumbers.ResourseDllNameColumn + 1).GetString().Trim(),
                    PanelName = row.Cell(columnNumbers.PanelNameColumn + 1).GetString().Trim(),
                    RibbonSplitButtonName = row.Cell(columnNumbers.RibbonSplitButtonColumn + 1).GetString().Trim(),
                    RibbonSize = row.Cell(columnNumbers.RibbonSizeColumn + 1).GetString().Trim(),
                    Root = row.Cell(columnNumbers.RootColumn + 1).GetString().Trim(),
                    LocalName = row.Cell(columnNumbers.LocalNameColumn + 1).GetString().Trim(),
                    RealCommandName = row.Cell(columnNumbers.RealCommandNameColumn + 1).GetString().Trim(),
                    Keyword = row.Cell(columnNumbers.KeywordColumn + 1).GetString().Trim(),
                    ToolTipText = row.Cell(columnNumbers.ToolTipTextColumn + 1).GetString().Trim(),
                    Accelerators = row.Cell(columnNumbers.AcceleratorsColumn + 1).GetString().Trim(),

                    DontMenu = row.Cell(columnNumbers.DontMenuColumn + 1).GetString().Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                    DontTake = row.Cell(columnNumbers.DontTakeColumn + 1).GetString().Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),

                    Weight = Utils.StringToInt(row.Cell(columnNumbers.WeightColumn + 1).GetString(), 10),
                    CmdType = Utils.StringToInt(row.Cell(columnNumbers.CmdTypeColumn + 1).GetString(), 1),

                };

                if (res.DontTake)//пропуск исключенных
                {
                    continue;
                }
                CommandDefinitions.Add(res);
            }
        }

        /// <summary> Чтение файла обмена (csv) разделитель точка с запятой, кодировка ASCI </summary>
        public void ReadFromCsv()
        {


            List<string[]> datas;
            using (StreamReader reader = new StreamReader(fileFullName, Encoding.GetEncoding(1251)))
            {
                datas = reader.ReadToEnd()
                   .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                   .Skip(HEADER_ROW_RANGE)
                   .Select(x => x.Split(';'))
                      .Where(c => !(c[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase)))
                      .ToList();

            }

            //todo копипаста((
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
                             Root = o[columnNumbers.RootColumn].Trim(),
                             DontMenu = o[columnNumbers.DontMenuColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                             DontTake = o[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                             LocalName = o[columnNumbers.LocalNameColumn].Trim(),
                             RealCommandName = o[columnNumbers.RealCommandNameColumn].Trim(),
                             Keyword = o[columnNumbers.KeywordColumn].Trim(),
                             Weight = Utils.StringToInt(o[columnNumbers.WeightColumn], 10),
                             CmdType = Utils.StringToInt(o[columnNumbers.CmdTypeColumn], 1),
                             ToolTipText = o[columnNumbers.ToolTipTextColumn].Trim(),
                             Accelerators = o[columnNumbers.AcceleratorsColumn].Trim(),
                         };
                         return res;
                     })
                );

        }

        // По здравому размышлению понял, что получение колонок для парсера интересно только здесь. Переделываю ;)
        /// <summary> Чтение файла обмена (tsv) разделитель табуляция, кодировка UTF8 </summary>
        /// <param name="fileFullName">Полный путь к обрабатываемому файлу</param>
        /// <param name="columnNumbers">Настройки парсинга</param>
        public void ReadFromTsv()
        {
            List<string[]> datas;
            using (StreamReader reader = new StreamReader(fileFullName, Encoding.UTF8))
            {
                datas = reader.ReadToEnd()
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(HEADER_ROW_RANGE)
                    .Select(o => o.Split('\t'))
                    .Where(c => !(c[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase)))
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

                        RibbonSplitButtonName = o[columnNumbers.RibbonSplitButtonColumn].Trim(),
                        RibbonSize = o[columnNumbers.RibbonSizeColumn].Trim(),
                        Root = o[columnNumbers.RootColumn].Trim(),
                        DontMenu = o[columnNumbers.DontMenuColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                        DontTake = o[columnNumbers.DontTakeColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                        LocalName = o[columnNumbers.LocalNameColumn].Trim(),
                        RealCommandName = o[columnNumbers.RealCommandNameColumn].Trim(),
                        Keyword = o[columnNumbers.KeywordColumn].Trim(),
                        Weight = Utils.StringToInt(o[columnNumbers.WeightColumn], 10),
                        CmdType = Utils.StringToInt(o[columnNumbers.CmdTypeColumn], 1),
                        ToolTipText = o[columnNumbers.ToolTipTextColumn].Trim(),
                        Accelerators = o[columnNumbers.AcceleratorsColumn].Trim(),
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
        public List<CommandDefinition>? CommandDefinitions { get; private set; }
        public List<PanelDefinition>? PanelDefinitions { get; private set; }
        public List<RibbonPaletteDefinition>? RibbonPaletteDefinitions { get; private set; }

        public string addinName { get; set; }

        private ColumnNumbers columnNumbers => new ColumnNumbers();
        private string fileFullName { get; set; }

        private const int HEADER_ROW_RANGE = 3;
    }
}
