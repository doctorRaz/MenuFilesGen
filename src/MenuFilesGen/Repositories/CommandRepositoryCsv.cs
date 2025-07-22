using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {
        /// <summary> Чтение файла обмена (csv) разделитель точка с запятой, кодировка ASCI </summary>
        public void ReadFromCsv()
        {


            List<string[]> datas = null;
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

            //todo копипаста(( оптимизировать, потом
            CommandDefinitions = new List<CommandDefinition>(
                datas.Select(o =>
                     {
                         CommandDefinition res = new CommandDefinition
                         {
                             DispName = o[columnNumbers.DispNameColumn].Trim(),
                             InterNameRaw = o[columnNumbers.InternameColumn].Trim(),
                             StatusText = o[columnNumbers.StatusTextColumn].Trim(),
                             IconName = o[columnNumbers.IconColumn].Trim(),
                             ResourceDllName = o[columnNumbers.ResourseDllNameColumn].Trim(),
                             PanelNameRaw = o[columnNumbers.PanelNameColumn].Trim(),

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
                             AddonNameRaw = o[columnNumbers.AddonNameColumn].Trim(),

                         };
                         return res;
                     })
                );

        }
    }
}
