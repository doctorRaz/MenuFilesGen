using ClosedXML.Excel;
using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{



    public class CommandRepository
    {
        /// <summary> Чтение файла обмена (xls)</summary>
        public void ReadFromXls(string xlsFileFullName, ColumnNumbers columnNumbers)
        {
            XLWorkbook workbook = new XLWorkbook(xlsFileFullName);

            Console.WriteLine("Введите номер листа:");
            int wscount = 1;
            foreach (var _ws in workbook.Worksheets)
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

            IXLRangeRows rows = worksheet.RangeUsed().RowsUsed();

            CommandDefinitions = new List<CommandDefinition>();

          
             

            for(int i =1+ HEADER_ROW_RANGE;i<=rows.Count();++i)
            {

                IXLRow row =worksheet. Row(i);

                int celN = 7;
                var cel = row.Cell(celN);
                var celV = row.Cell(celN).Value;

                var gcel = celV.TryGetText;

                CommandDefinition res = new CommandDefinition
                {
                    DispName = row.Cell(columnNumbers.DispNameColumn+1).Value.ToString().Trim(),
                    InterName = row.Cell(columnNumbers.InternameColumn+1).Value.ToString().Trim(),
                    StatusText = row.Cell(columnNumbers.StatusTextColumn + 1).Value.ToString().Trim(),
                    IconName = row.Cell(columnNumbers.IconColumn + 1).Value.ToString().Trim(),
                    ResourceDllName = row.Cell(columnNumbers.ResourseDllNameColumn+1).Value.ToString().Trim(),
                    PanelName = row.Cell(columnNumbers.PanelNameColumn + 1).Value.ToString().Trim(),
                    RibbonSplitButtonName = row.Cell(columnNumbers.RibbonSplitButtonColumn + 1).Value.ToString().Trim(),
                    RibbonSize = row.Cell(columnNumbers.RibbonSizeColumn+1).ToString().Trim(),
                    Root = row.Cell(columnNumbers.RootColumn + 1).Value.ToString().Trim(),
                    DontMenu = row.Cell(columnNumbers.DontMenuColumn+1  ).ToString().Trim().ToUpper() == "ИСТИНА",
                    DontTake = row.Cell(columnNumbers.DontTakeColumn + 1).Value.ToString().Trim().ToUpper() == "ИСТИНА",
                    LocalName = row.Cell(columnNumbers.LocalNameColumn+1).ToString().Trim(),
                    RealCommandName = row.Cell(columnNumbers.RealCommandNameColumn + 1).Value.ToString().Trim(),
                    Keyword = row.Cell(columnNumbers.KeywordColumn+1).Value.ToString().Trim(),
                    Weight = Utils.StringToInt(row.Cell(columnNumbers.WeightColumn + 1).ToString().Trim(), 10),
                    CmdType = Utils.StringToInt(row.Cell(columnNumbers.CmdTypeColumn+1).Value.ToString().Trim(), 1),
                    ToolTipText = row.Cell(columnNumbers.ToolTipTextColumn + 1).Value.ToString().Trim(),
                    Accelerators = row.Cell(columnNumbers.AcceleratorsColumn + 1).Value.ToString().Trim(),
                };
                if(res.DontTake)
                {
                    continue;
                }

                CommandDefinitions.Add(res);
                // Вместо строки можно заносить в базу согласно модели.
                Console.WriteLine($"{row.Cell(1).Value}\t{row.Cell(2).Value}");
                // для проверки, что данные были получены - можно поставить точку останова
            }


        }

        /// <summary> Чтение файла обмена (csv) разделитель точка с запятой, кодировка ASCI </summary>
        public void ReadFromCsv(string csvFileFullName, ColumnNumbers columnNumbers)
        {
            throw new NotImplementedException();

        }

        // По здравому размышлению понял, что получение колонок для парсера интересно только здесь. Переделываю ;)
        /// <summary> Чтение файла обмена (tsv) разделитель табуляция, кодировка UTF8 </summary>
        /// <param name="tsvFileFullName">Полный путь к обрабатываемому файлу</param>
        /// <param name="columnNumbers">Настройки парсинга</param>
        public void ReadFromTsv(string tsvFileFullName, ColumnNumbers columnNumbers)
        {
            List<string[]> datas;
            using (StreamReader reader = new StreamReader(tsvFileFullName, Encoding.UTF8))
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
                        LocalName = o[columnNumbers.LocalNameColumn].Trim(),
                        RealCommandName = o[columnNumbers.RealCommandNameColumn].Trim(),
                        Keyword = o[columnNumbers.KeywordColumn].Trim(),
                        Weight = Utils.StringToInt(o[columnNumbers.WeightColumn].Trim(), 10),
                        CmdType = Utils.StringToInt(o[columnNumbers.CmdTypeColumn].Trim(), 1),
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
        public List<CommandDefinition> CommandDefinitions { get; private set; }
        public List<PanelDefinition> PanelDefinitions { get; private set; }
        public List<RibbonPaletteDefinition> RibbonPaletteDefinitions { get; private set; }

        private const int HEADER_ROW_RANGE = 3;
    }
}
