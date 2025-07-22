using ClosedXML.Excel;
using MenuFilesGen.Models;
using MenuFilesGen.Service;

namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {

        /// <summary> Чтение файла обмена (xls)</summary>
        public void ReadFromXls()
        {
            XLWorkbook workbook = new XLWorkbook();
            try
            {
                /*XLWorkbook*/
                workbook = new XLWorkbook(new FileStream(FileFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            }
            catch (Exception ex)
            {
                return;
            }

            int wscount = 1;
            foreach (IXLWorksheet _ws in workbook.Worksheets)//покажем в консоли номера листов с названиями
            {
                Console.WriteLine($"{wscount}.\t{_ws.Name}");
                //Console.WriteLine($"{wscount}. Имя:\t{_ws.Name}\t\tвидимость=\"{(XLWorksheetVisibilityMod)_ws.Visibility}\"");
                wscount++;
            }

            Console.WriteLine("");

            bool result = false;
            ConsoleKey k = ConsoleKey.NoName;
            do
            {
                if (xlPage == 0)//лист не задан
                {
                    Console.Write("Введите номер листа: ");//запросим номер
                    string? wsNumber = Console.ReadLine();
                    xlPage = Utils.StringToInt(wsNumber);
                }


                if (xlPage > 0 && xlPage <= workbook.Worksheets.Count)//в диапазоне
                {
                    result = true;
                }
                else//вне диапазона
                {
                    Console.WriteLine($"Лист {xlPage} не найден!");
                    Console.Write($"Продолжить - AnyKey\nЗавершить - Esc: ");
                    xlPage = 0;
                    k = Console.ReadKey().Key;
                    Console.WriteLine($"");
                }

            } while (!(result || k == ConsoleKey.Escape));

            if (!result) return;//выход по Esc уходим

            IXLWorksheet worksheet = workbook.Worksheet(xlPage);

            AddonNameGlobal = worksheet.Name;

            Console.WriteLine($"Работаю с листом: {xlPage} - \"{AddonNameGlobal}\"");

            IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed().RowsUsed().Skip(HEADER_ROW_RANGE);

            CommandDefinitions = new List<CommandDefinition>();

            foreach (IXLRangeRow row in rows)
            {
                CommandDefinition res = new CommandDefinition
                {
                    DispName = row.Cell(columnNumbers.DispNameColumn + 1).GetString().Trim(),
                    InterNameRaw = row.Cell(columnNumbers.InternameColumn + 1).GetString().Trim(),
                    StatusText = row.Cell(columnNumbers.StatusTextColumn + 1).GetString().Trim(),
                    IconName = row.Cell(columnNumbers.IconColumn + 1).GetString().Trim(),
                    ResourceDllName = row.Cell(columnNumbers.ResourseDllNameColumn + 1).GetString().Trim(),
                    PanelNameRaw = row.Cell(columnNumbers.PanelNameColumn + 1).GetString().Trim(),
                    RibbonSplitButtonName = row.Cell(columnNumbers.RibbonSplitButtonColumn + 1).GetString().Trim(),
                    RibbonSize = row.Cell(columnNumbers.RibbonSizeColumn + 1).GetString().Trim(),
                    AppName = row.Cell(columnNumbers.AppNameColumn + 1).GetString().Trim(),
                    LocalName = row.Cell(columnNumbers.LocalNameColumn + 1).GetString().Trim(),
                    RealCommandName = row.Cell(columnNumbers.RealCommandNameColumn + 1).GetString().Trim(),
                    Keyword = row.Cell(columnNumbers.KeywordColumn + 1).GetString().Trim(),
                    ToolTipText = row.Cell(columnNumbers.ToolTipTextColumn + 1).GetString().Trim(),
                    Accelerators = row.Cell(columnNumbers.AcceleratorsColumn + 1).GetString().Trim(),

                    HideCommand = row.Cell(columnNumbers.HideCommandColumn + 1).GetString().Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),
                    DontTake = row.Cell(columnNumbers.DontTakeColumn + 1).GetString().Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),

                    Weight = Utils.StringToInt(row.Cell(columnNumbers.WeightColumn + 1).GetString(), 10),
                    CmdType = Utils.StringToInt(row.Cell(columnNumbers.CmdTypeColumn + 1).GetString(), 1),
                    AddonNameRaw = row.Cell(columnNumbers.AddonNameColumn + 1).GetString().Trim(),

                };

                if (res.DontTake)//пропуск исключенных
                {
                    continue;
                }
                CommandDefinitions.Add(res);
            }
        }

    }
}
