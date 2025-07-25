using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {
        public CommandRepository(ArgsCmdLine _cs)
        {
            FileFullName = _cs.FileName;
            HEADER_ROW_RANGE = _cs.HeaderRowRange;
            xlPage = _cs.XlsPageNumber;
            directoryPath = !string.IsNullOrEmpty(_cs.DirectoryPath) ? _cs.DirectoryPath : Path.GetDirectoryName(FileFullName);

            string fileExtension = Path.GetExtension(FileFullName);

            //обработчик по расширению файла перенаправлять в свой читатель
            if (fileExtension.Contains("xls", StringComparison.OrdinalIgnoreCase))
            {
                ReadFromXls();
            }
            else//txt
            {

                if (fileExtension.Contains("csv", StringComparison.OrdinalIgnoreCase))//разделитель ; ASCI
                {
                    encoding = Encoding.GetEncoding(1251);
                    splitChar = ';';

                }
                else//разделитель tab юникод или tsv
                {
                    encoding = Encoding.UTF8;
                    splitChar = '\t';

                }

                AddonNameGlobal = Path.GetFileNameWithoutExtension(FileFullName);
                ReadFromTxt();
            }
        }

        /// <summary>
        /// Gets the command definition text.
        /// </summary>
        /// <param name="datas">The datas.</param>
        void GetCmdDefTxt(/*List<string[]> datas*/)
        {
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
                        IsVirtualPanel = o[columnNumbers.IsVirtualPanelColumn].Contains("ИСКЛЮЧИТЬ", StringComparison.OrdinalIgnoreCase),//not used
                    };
                    return res;
                })
                );
        }


        /// <summary>
        /// Saves to CFG.
        /// </summary>
        /// <param name="cfgFileName">Name of the CFG file.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveToCfg(CfgDefinition cfg)
        {
            using (StreamWriter writer = new StreamWriter(CfgFilePath, false, Encoding.GetEncoding(65001)))
            {
                writer.WriteLine("[\\cfg]");
                writer.WriteLine(cfg.Menu.LstStr());//меню
                writer.WriteLine(cfg.ToolbarPopupMenu.LstStr()); //поп меню
                writer.WriteLine(cfg.ToolbarsViewMenu.LstStr()); //виев меню

                writer.WriteLine(cfg.Toolbars.LstStr());//панели

                writer.WriteLine(cfg.Configman.LstStr());//команды
                writer.WriteLine(cfg.ToolbarsCmd.LstStr());//команды меню

                writer.WriteLine(cfg.ViewPopupMenu.LstStr());//меню по ПКМ чертежа, notUsed

                writer.WriteLine(cfg.Ribbon.LstStr());//лента
                writer.WriteLine(cfg.Accelerators.LstStr());//горячие кнопки

            }
        }

        public void SaveToCuix(XDocument xDoc)
        {
            xDoc.Save(CuiFilePath);

            // Удаление .cuix файла, если он существует
            if (File.Exists(CuixFilePath))
                File.Delete(CuixFilePath);

            // Создание архива (.cuix), добавление в него сформированного .xml файла
            using (ZipArchive zip = ZipFile.Open(CuixFilePath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(CuiFilePath, "RibbonRoot.cui");
            }

            // Удаление ribbon.cui файла, если он существует
            if (File.Exists(CuiFilePath))
            {
                File.Delete(CuiFilePath);
            }
        }



        /// <summary>
        /// Определения команд
        /// </summary>
        /// <value>
        /// The command definitions.
        /// </value>
        public List<CommandDefinition>? CommandDefinitions { get; private set; }

        /// <summary>
        /// Not used
        /// </summary>
        /// <value>
        /// The panel definitions.
        /// </value>
        public List<PanelDefinition>? PanelDefinitions { get; private set; }

        /// <summary>
        /// Not used
        /// </summary>
        /// <value>
        /// The ribbon palette definitions.
        /// </value>
        public List<RibbonPaletteDefinition>? RibbonPaletteDefinitions { get; private set; }

        /// <summary>
        /// Название приложения
        /// </summary>
        /// <value>
        /// The name of the addin.
        /// </value>
        public string AddonNameGlobal { get; set; }

        /// <summary>
        /// Номера столбцов для парсинга
        /// </summary>
        /// <value>
        /// The column numbers.
        /// </value>
        private ColumnNumbers columnNumbers => new ColumnNumbers();

        /// <summary>
        /// Файл для парсинга
        /// </summary>
        /// <value>
        /// The full name of the file.
        /// </value>
        public string FileFullName { get; set; }

        /// <summary>
        /// директория конфигов
        /// </summary>
        /// <value>
        /// The directory path.
        /// </value>
        public string directoryPath { get; private set; } = "";

        /// <summary>
        /// файл конфигурации.
        /// </summary>
        /// <value>
        /// The CFG file path.
        /// </value>
        public string CfgFilePath => Path.Combine(directoryPath, AddonNameGlobal + ".cfg");//   $"{directoryPath}\\{addonNameGlobal}.Cfg";

        /// <summary>
        /// файл заготовка ленты.
        /// </summary>
        /// <value>
        /// The cui file path.
        /// </value>
        public string CuiFilePath => Path.Combine(directoryPath, "RibbonRoot.cui");

        /// <summary>
        /// файл ленты zip
        /// </summary>
        /// <value>
        /// The cuix file path.
        /// </value>
        public string CuixFilePath => Path.Combine(directoryPath, AddonNameGlobal + ".cuix");

        /// <summary>
        /// Количество строк пропустить при парсинге
        /// </summary>
        private int HEADER_ROW_RANGE { get; set; } = 3;

        private int xlPage { get; set; } = 0;

        private string newLine = Environment.NewLine;

        /// <summary>
        /// сырые данные из txt
        /// </summary>
        List<string[]> datas { get; set; } = new List<string[]>();

        Encoding encoding { get; set; }

        char splitChar { get; set; }
    }

}
