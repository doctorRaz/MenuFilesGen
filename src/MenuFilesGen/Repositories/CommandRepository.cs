using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CommandRepository
    {
        public CommandRepository(ArgsCmdLine _cs)
        {
            FileFullName = _cs.FilesName;
            HEADER_ROW_RANGE = _cs.HeaderRowRange;
            xlPage = _cs.XlsPageNumber;


            string fileExtension = Path.GetExtension(FileFullName);

            //обработчик по расширению файла перенаправлять в свой читатель
            if (fileExtension.Contains("xls", StringComparison.OrdinalIgnoreCase))
            {
                ReadFromXls();
            }
            else if (fileExtension.Contains("csv", StringComparison.OrdinalIgnoreCase))//разделитель ; ASCI
            {
                AddonNameGlobal = Path.GetFileNameWithoutExtension(FileFullName);

                ReadFromCsv();
            }
            else//разделитель tab юникод или tsb
            {
                AddonNameGlobal = Path.GetFileNameWithoutExtension(FileFullName);

                ReadFromTsv();
            }

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

                writer.WriteLine(cfg.Ribbon.LstStr());//лента
                writer.WriteLine(cfg.Accelerators.LstStr());//горячие кнопки
            }
        }

        public void SaveToCuix()
        {
            throw new NotImplementedException();
        }

        // https://stackoverflow.com/questions/1159233/multi-level-grouping-in-linq        
        /// <summary>
        /// Группируем по AppName потом по панелям
        /// </summary>
        /// <value>
        /// The hierarchical grouping.
        /// </value>
        public List<AppDefinition> GroupingAppAddonPanel
        {
            get
            {

                return CommandDefinitions
                     .GroupBy(e => e.AppName)
                     .Select(appName => new AppDefinition
                     {
                         Name =string.IsNullOrEmpty(appName.Key) ? AddonNameGlobal: appName.Key,
                         Addons = appName
                     .GroupBy(e => e.AddonName)
                     .Select(addon => new AddonDefinition
                     {
                         Parent = string.IsNullOrEmpty(appName.Key) ? AddonNameGlobal: appName.Key,
                         Name = addon.Key,
                         Panel = addon
                         .GroupBy(e => e.PanelName)
                         .Select(panel => new PanelDefinition
                         {
                             Parent =   addon.Key,
                             Name = panel.Key,
                             Command = panel.ToList() /*new List<CommandDefinition>(panel)*/,
                         }).ToList()
                     }).ToList()
                     }).ToList();

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
        public string directoryPath => Path.GetDirectoryName(FileFullName);

        /// <summary>
        /// файл конфигурации.
        /// </summary>
        /// <value>
        /// The CFG file path.
        /// </value>
        public string CfgFilePath => Path.Combine(directoryPath, AddonNameGlobal + ".cfg");//   $"{directoryPath}\\{AddonNameGlobal}.cfg";

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
    }

    public enum XLWorksheetVisibilityMod
    {
        Видим,
        Скрыт,
        СуперСкрыт
    }
}
