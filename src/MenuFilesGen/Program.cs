using Cyrillic.Convert;
using MenuFilesGen.Models;
using MenuFilesGen.Repositories;
using MenuFilesGen.Service;
using NickBuhro.Translit;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace MenuFilesGen
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            string newLine0 = Environment.NewLine;//переносы

            List<string> Ribbon = new List<string>() { $"{2}[\\ribbon\\{10}]" ,
                            $"{2}CUIX=s%CFG_PATH%\\{2}.cuix" ,
                            $"{2}visible=f1" };
            Ribbon.Add("test");
            var dd = String.Join(newLine0, Ribbon);

            //Cyrillic.Convert;
            var conversion = new Conversion();


            string latin = "a,b,v,g,d,đ,e,ž,z,i,j,k,l,m,n,o,p,r,s,t,ć,u,f,h,c,č,š,Lj,Nj,Dž,lj,nj,dž";
            string ru = "ю я й сорок тыся апизян тестъ апвъ  пвпь пвпвь";

            var latin0 = Transliteration.CyrillicToLatin("Предками данная мудрость народная!", Language.Russian);
            var rr = Transliteration.CyrillicToLatin(ru);
            string en = ru.ToRussianLatin();

            var cyrillicExtension = latin.ToSerbianCyrilic(); //Convert with extension method
            var cyrillic = conversion.SerbianLatinToCyrillic(latin); //Convert with forwarded string
            var latinConvert = conversion.SerbianCyrillicToLatin(cyrillic);
            var latinExtension = cyrillic.ToSerbianLatin();

#endif


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Utils utils = new Utils();

            ArgsCmdLine argsCmdLine = utils.ParseCmdLine(args);//читаем аргументы ком строки

            Console.WriteLine($"Аргументы ком строки:" +
                           $"\n\t-hrr:[сколько строк пропускать, число] - {argsCmdLine.HeaderRowRange}" +
                           $"\n\t-xpn:[для XLS* номер листа шаблона, число] - {argsCmdLine.XlsPageNumber}" +
                           $"\n\t-exo:[подтверждать выход - 1, не подтверждать - 0] - {argsCmdLine.EchoOnOff}" +
                           $"\n\t[\"полный путь к файлу шаблона с расширением\"] - {argsCmdLine.FilesName}\n");

            if (string.IsNullOrEmpty(argsCmdLine.FilesName))//если имя файла не аргумент
            {
                OpenFileDialog tableFileDialog = new OpenFileDialog() { Filter = "Книга Excel (*.xls*)|*.xls*|Юникод  разделитель табуляция (*.txt;*.tsv)|*.txt;*.tsv|ASCI разделитель точка запятая (*.csv)|*.csv|Все файлы (*.*)|*.*" };
                if (tableFileDialog.ShowDialog() != DialogResult.OK)
                {
                    Console.WriteLine("Не задан файл");
                    Console.WriteLine("\nДля выхода нажмите любую клавишу...");
                    if (argsCmdLine.EchoOnOff) Console.ReadKey();
                    return;
                }
                argsCmdLine.FilesName = tableFileDialog.FileName;//файл шаблона в аргументы
            }

            CommandRepository rep = new CommandRepository(argsCmdLine);//парсим файл

            if (rep.CommandDefinitions is null || rep.CommandDefinitions.Count < 1)//если ничего не напарсили
            {
                Console.WriteLine($"Файл {rep.FileFullName} не прочитан");

                argsCmdLine.FilesName = "";//чистим путь к шаблону, на случай если будем запрашивать его в цикле
                Console.WriteLine("Для выхода нажмите любую клавишу...");//todo зациклить выбор файла?
                Console.ReadKey();

                return;
            }

            string newLine = Environment.NewLine;//переносы


            string addinNameGlobal = rep.AddonNameGlobal;

            CfgDefinition cfg = new CfgDefinition(addinNameGlobal);//конфиг

            //собираем в строки конфиг

        

            ////команды
            //cfg.Configman = $"{newLine}[\\configman]" +
            //             $"{newLine}[\\configman\\commands]";

            //горячие клавиши
            //string accelerators = $"{newLine}[\\Accelerators]";// хоткеи

            //меню
            //string Menu = $"{newLine}[\\menu]";

            //панели
            //cfg.Toolbars = $"{newLine};Панели" +
            //                    $"{newLine}[\\toolbars]";

            //всплывающее меню панелей
            //cfg.ToolbarPopupMenu = $"{newLine};Popup меню" +
            //                        $"{newLine}[\\ToolbarPopupMenu]" +
            //                        $"{newLine}[\\ToolbarPopupMenu\\{addinNameGlobal}]" +
            //                        $"{newLine}Name=s{addinNameGlobal}";

            //команды вызова панелей
            //string toolbarsCmd = $"{newLine}; Команды вызова панелей";

            //меню вид панелей
            //string toolbarsViewMenu = $"{newLine};View меню" +
            //                            /*        $"{newLine}[\\menu\\View\\toolbars\\{AddonNameGlobal}]" +*/
            //                            $"{newLine}[\\menu\\View\\toolbars\\{addinNameGlobal}]" +
            //                            /*    $"{newLine}Name=s{AddonNameGlobal}";*/
            //                            $"{newLine}Name=s{addinNameGlobal}";
            //todo начинаем с уровня приложения, добавить уровень
            foreach (AppDefinition AppName in rep.HierarchicalGrouping)//уровень приложения
            {

            }
            /*
            foreach (AppDefinition AppName in rep.HierarchicalGrouping)
            {
                string appName = AppName.Name;
                string appMenu = $"{AddonNameGlobal}";

                #region Классическое меню шапка

                if (!string.IsNullOrEmpty(appName))
                {
                    appMenu = $"{appName}\\{AddonNameGlobal}";

                    menu += $"{newLine}[\\menu\\{appName}]" +
                            $"{newLine}Name=s{appName}" +
                            $"{newLine}[\\menu\\{appMenu}]" +
                            $"{newLine}Name=s{AddonNameGlobal}";

                }
                else
                {
                    menu += $"{newLine}[\\menu\\{appMenu}]" +
                            $"{newLine} Name=s{AddonNameGlobal}";
                }
                #endregion

                foreach (var panel in AppName.Addon )
                {
                    string panelName = panel.Name;


                    string panelNameRu = $"{AddonNameGlobal}_{panelName.Replace(' ', '_')}";//в имени команды не должно быть пробелов 
                    string panelNameEn = Transliteration.CyrillicToLatin(panelNameRu, Language.Russian);//intername е должно содержать кириллицы

                    string intername = $"ShowToolbar_{panelNameEn}";
                    string localName = $"Панель_{panelNameRu}";

                    #region Панели

                    //панели
                    toolbars += $"{newLine}{newLine}[\\toolbars\\{panelNameEn}]" +
                                $"{newLine}name=s{panelName}";

                    CommandDefinition cmd0 = panel.command[0] as CommandDefinition; //добавлять к команде показа панели иконку, по первой команде панели

                    //команды
                    toolbarsCmd += $"{newLine}{newLine}[\\configman\\commands\\{intername}]" +
                                    $"{newLine}weight=i10" +
                                    $"{newLine}cmdtype=i0" +
                                    $"{newLine}Intername=s{intername}" +
                                    $"{newLine}StatusText=sОтображение панели {panelName}" +
                                    $"{newLine}ToolTipText=sОтображение панели {panelName}" +
                                    $"{newLine}DispName=sОтображение панели {panelName}" +
                                    $"{newLine}LocalName=s{localName}" +
                                    $"{Utils.IconDefinition(cmd0)}";
                    //поп меню
                    toolbarPopupMenu += $"{newLine}[\\ToolbarPopupMenu\\{AddonNameGlobal}\\{intername}]" +
                     $"{newLine}Name=s{panelName}" +
                     $"{newLine}InterName=s{intername}";
                    //вью меню
                    toolbarsViewMenu += $"{newLine}[\\menu\\View\\toolbars\\{AddonNameGlobal}\\{intername}]" +
                                        $"{newLine}Name=s{panelName}" +
                                        $"{newLine}InterName=s{intername}";

                    #endregion

                    #region Классическое меню раздел

                    menu += $"{newLine}{newLine}[\\menu\\{appMenu}\\{panelName}]" +
                            $"{newLine}name=s{panelName}";

                    #endregion

                    foreach (CommandDefinition cmd in panel.command)
                    {
                        #region Регистрация команд

                        string _toolTipText = !string.IsNullOrEmpty(cmd.ToolTipText) ? $"{newLine}ToolTipText=s{cmd.ToolTipText}" : "";
                        string _localName = !string.IsNullOrEmpty(cmd.LocalName) ? $"{newLine}LocalName=s{cmd.LocalName}" : "";
                        string _realCommandName = !string.IsNullOrEmpty(cmd.RealCommandName) ? $"{newLine}RealCommandName=s{cmd.RealCommandName}" : "";
                        string _keyword = !string.IsNullOrEmpty(cmd.Keyword) ? $"{newLine}Keyword=s{cmd.Keyword}" : "";

                        configman += $"{newLine}{newLine}[\\configman\\commands\\{cmd.InterName}]" +
                                     $"{newLine}weight=i{cmd.Weight}" +
                                     $"{newLine}cmdtype=i{cmd.CmdType}" +
                                     $"{newLine}intername=s{cmd.InterName}" +
                                     $"{newLine}DispName=s{cmd.DispName}" +
                                     $"{newLine}StatusText=s{cmd.StatusText}" +
                                     $"{_toolTipText}" +
                                     $"{_localName}" +
                                     $"{_realCommandName}" +
                                     $"{_keyword}" +
                                     $"{Utils.IconDefinition(cmd)}";

                        #endregion

                        if (!string.IsNullOrEmpty(cmd.Accelerators))
                        {
                            accelerators += $"{newLine}{cmd.InterName}=s{cmd.Accelerators}";

                        }

                        if (cmd.DontMenu) continue;// не добавлять в меню пропуск
                        #region Классическое меню

                        menu += $"{newLine}[\\menu\\{appMenu}\\{panelName}\\s{cmd.InterName}]" +
                                $"{newLine}name=s{cmd.DispName}" +
                                $"{newLine}Intername=s{cmd.InterName}";

                        #endregion

                        #region Панели
                        toolbars += $"{newLine}[\\toolbars\\{panelNameEn}\\{cmd.InterName}]" +
                                    $"{newLine}Intername=s{cmd.InterName}";
                        #endregion
                    }
                }
            }
            */
            #region Save *.cfg            

            rep.SaveToCfg(cfg);
            /*
                        using (StreamWriter writer = new StreamWriter(rep.CfgFilePath, false, Encoding.GetEncoding(65001)))
                        {
                            writer.WriteLine(menu);//меню
                            writer.WriteLine(toolbarPopupMenu); //поп меню
                            writer.WriteLine(toolbarsViewMenu); //виев меню

                            writer.WriteLine(toolbars);//панели

                            writer.WriteLine(configman);//команды
                            writer.WriteLine(toolbarsCmd);//команды меню

                            writer.WriteLine(ribbon);//лента
                            writer.WriteLine(accelerators);//горячие кнопки
                        }
            */
            #endregion

            //группировка по PanelName
            List<IGrouping<string, CommandDefinition>> groupsPanel = rep.CommandDefinitions
                                                                    .GroupBy(e => e.PanelName).ToList();

            #region Ribbon
            // Ленточное меню
            //Создание XML документа 
            XDocument xDoc = new XDocument();

            //Корневой элемент
            XElement ribbonRoot = new XElement("RibbonRoot");
            xDoc.Add(ribbonRoot);

            XElement ribbonPanelSourceCollection = new XElement("RibbonPanelSourceCollection");
            ribbonRoot.Add(ribbonPanelSourceCollection);

            XElement ribbonTabSourceCollection = new XElement("RibbonTabSourceCollection");
            ribbonRoot.Add(ribbonTabSourceCollection);

            XElement ribbonTabSource = new XElement("RibbonTabSource");
            ribbonTabSource.Add(new XAttribute("Text", addinNameGlobal));
            ribbonTabSource.Add(new XAttribute("UID", $"{addinNameGlobal.Replace(" ", "")}_Tab"));
            ribbonTabSourceCollection.Add(ribbonTabSource);

            foreach (IGrouping<string, CommandDefinition> cmd in groupsPanel)
            {
                XElement ribbonPanelSource = new XElement("RibbonPanelSource");
                ribbonPanelSource.Add(new XAttribute("UID", cmd.Key));
                ribbonPanelSource.Add(new XAttribute("Text", cmd.Key));
                ribbonPanelSourceCollection.Add(ribbonPanelSource);

                // Временный контейнер для сбора кнопок
                XElement panelButtons = new XElement("Temp");

                // Группируем команды по тому, объединяются ли они в RibbonSplitButton5
                List<IGrouping<string, CommandDefinition>> unitedCommands = cmd.GroupBy(c => c.RibbonSplitButtonName).ToList();

                foreach (IGrouping<string, CommandDefinition> unitedCommandGroup in unitedCommands)
                {
                    XElement container = panelButtons;
                    if (!string.IsNullOrWhiteSpace(unitedCommandGroup.Key))
                    {
                        XElement ribbonSplitButton = new XElement("RibbonSplitButton");
                        ribbonSplitButton.Add(new XAttribute("Text", unitedCommandGroup.Key));
                        ribbonSplitButton.Add(new XAttribute("Behavior", "SplitFollowStaticText"));
                        ribbonSplitButton.Add(new XAttribute("ButtonStyle", unitedCommandGroup.First().RibbonSize));

                        panelButtons.Add(ribbonSplitButton);
                        container = ribbonSplitButton;
                    }

                    foreach (CommandDefinition commandData in unitedCommandGroup)
                    {
                        if (commandData.DontMenu) continue;
                        container.Add(CreateButton(commandData));
                    }
                }

                Dictionary<bool, IGrouping<bool, XElement>> sortedButtons = panelButtons
                    .Elements()
                    .GroupBy(button => button.Attributes().First(attr => attr.Name == "ButtonStyle").Value.Contains("Small"))
                    .ToDictionary(g => g.Key);

                if (sortedButtons.ContainsKey(false))
                {
                    foreach (XElement button in sortedButtons[false])
                        ribbonPanelSource.Add(button);
                }

                if (sortedButtons.ContainsKey(true))
                {
                    XElement ribbonRowPanel = null;
                    int ribbonRowPanelButtonsCount = 3;

                    foreach (XElement button in sortedButtons[true])
                    {
                        if (ribbonRowPanelButtonsCount == 3)
                        {
                            ribbonRowPanel = new XElement("RibbonRowPanel");
                            ribbonPanelSource.Add(ribbonRowPanel);
                            ribbonRowPanelButtonsCount = 0;
                        }

                        XElement ribbonRow = new XElement("RibbonRow");
                        ribbonRow.Add(button);
                        ribbonRowPanel.Add(ribbonRow);

                        ribbonRowPanelButtonsCount++;
                    }
                }

                XElement ribbonPanelSourceReference = new XElement("RibbonPanelSourceReference");
                ribbonPanelSourceReference.Add(new XAttribute("PanelId", cmd.Key));
                ribbonTabSource.Add(ribbonPanelSourceReference);
            }

            xDoc.Save(rep.CuiFilePath);

            // Удаление .cuix файла, если он существует
            if (File.Exists(rep.CuixFilePath))
                File.Delete(rep.CuixFilePath);

            // Создание архива (.cuix), добавление в него сформированного .xml файла
            using (ZipArchive zip = ZipFile.Open(rep.CuixFilePath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(rep.CuiFilePath, "RibbonRoot.cui");
            }

            // Удаление ribbon.cui файла, если он существует
            if (File.Exists(rep.CuiFilePath))
            {
                File.Delete(rep.CuiFilePath);
            }

            #endregion

            Console.WriteLine($"\nФайлы:\t{addinNameGlobal}.cfg" +
                $"\n\t{addinNameGlobal}.cuix" +
                $"\nсохранены в: {rep.directoryPath}");

            if (argsCmdLine.EchoOnOff)
            {
                Console.WriteLine("\nДля выхода нажмите любую клавишу...");
                Console.ReadKey();
            }

        }

        public static XElement CreateButton(CommandDefinition commandData)
        {
            XElement ribbonCommandButton = new XElement("RibbonCommandButton");
            ribbonCommandButton.Add(new XAttribute("Text", commandData.DispName));
            ribbonCommandButton.Add(new XAttribute("ButtonStyle", commandData.RibbonSize));
            ribbonCommandButton.Add(new XAttribute("MenuMacroID", commandData.InterName));
            return ribbonCommandButton;
        }
    }
}