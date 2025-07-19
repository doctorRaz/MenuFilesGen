using Cyrillic.Convert;
using MenuFilesGen.Models;
using MenuFilesGen.Repositories;
using MenuFilesGen.Service;
using NickBuhro.Translit;
using System.CodeDom;
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
            {
                string pan = "Название панели на хеячсмитьбюйцукен гшщзхъфывапролджэ ЙЦУКЕНГШЩЗФЫВАПРОЛДЮЖЭЯЧСМИТЬБЮ,!№;%:?*()_++++++++++ ";

                string tr = CurToLaninConverter.CyrilicToLatin(pan);




                //Cyrillic.Convert;
                var conversion = new Conversion();


                var rr = Transliteration.CyrillicToLatin(pan);

                string en = pan.ToRussianLatin();

                var lc = conversion.RussianCyrillicToLatin(pan);

                Console.WriteLine(rr.ToString());
                Console.WriteLine(en.ToString());
                Console.WriteLine(lc.ToString());
            }

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


            string addonNameGlobal = rep.AddonNameGlobal;

            CfgDefinition cfg = new CfgDefinition(addonNameGlobal);//конфиг

            #region Формируем меню   
            // группировка приложение аддон панель
            
            //++ ********** уровень приложения *********

            foreach (AppDefinition App in rep.GroupingAppAddonPanel)//уровень приложения
            {
                string appName = App.Name;

                string panelRootApp = string.IsNullOrEmpty(appName) ? addonNameGlobal : appName;//роот для попап и вид панелей

                //+попап панелей
                string popupPanelRoot = $"[\\ToolbarPopupMenu\\{panelRootApp}";
                cfg.ToolbarPopupMenu.Add($"{popupPanelRoot}]");
                cfg.ToolbarPopupMenu.Add($"Name=s{panelRootApp}");

                //+ меню вид
                string viewPanelRoot = $"[\\menu\\View\\toolbars\\{panelRootApp}";
                cfg.ToolbarsViewMenu.Add($"{viewPanelRoot}]");
                cfg.ToolbarsViewMenu.Add($"Name=s{panelRootApp}");

                // Классическое меню шапка
                string menuApp = "[\\menu";

                if (!string.IsNullOrEmpty(appName))
                {
                    menuApp += $"\\{appName}";

                    cfg.Menu.Add($"{menuApp}]");
                    cfg.Menu.Add($"Name=s{appName}");
                }
                //++ ***** уровень аддона *****
                foreach (AddonDefinition Addon in App.Addons)
                {
                    string addonName = Addon.Name;

                    string menuAddon = "";

                    if (!string.IsNullOrEmpty(addonName))
                    {
                        menuAddon = $"{menuApp}\\{addonName}";

                        cfg.Menu.Add($"{menuAddon}]");
                        cfg.Menu.Add($"Name=s{addonName}");
                    }
                    else
                    {
                        menuAddon = menuApp;
                    }
                    //++ *******уровень панели *************
                    foreach (PanelDefinition Panel in Addon.Panel)
                    {
                        string panelName = Panel.Name; //имя панели не может быть пустым
                        string menuPanel = "";


                        //+menu
                        menuPanel += $"{menuAddon}\\{panelName}";
                        cfg.Menu.Add($"{menuPanel}]");
                        cfg.Menu.Add($"Name=s{panelName}");

                        //todo 1. обрабатывать панельки надо в отдельном цикле
                        //цикл тут сборка меню
                        //цикл по App привязка панелек к меню вид и попап
                        //цикл по панелькам наполнение панелек, регистрация команд, привязка команд к панелькам

                        //+toolbar название
                        string panelNameRu = $"{addonNameGlobal}_{panelName.Replace(' ', '_')}";//в имени команды не должно быть пробелов 
                        string panelNameEn = Transliteration.CyrillicToLatin(panelNameRu, Language.Russian);//intername не должно содержать кириллицы

                        //+toolbar вызов
                        string toolbarIntername = $"ShowToolbar_{panelNameEn}";
                        string toolbarLocalName = $"Панель_{panelNameRu}";

                        //+регистрация панели
                        cfg.Toolbars.Add($"[\\toolbars\\{panelNameEn}]");
                        cfg.Toolbars.Add($"name=s{panelName}");

                        //+регистрируем команду вызова панели
                        cfg.ToolbarsCmd.Add($"[\\configman\\commands\\{toolbarIntername}]");
                        cfg.ToolbarsCmd.Add($"weight=i10");
                        cfg.ToolbarsCmd.Add($"cmdtype=i0");
                        cfg.ToolbarsCmd.Add($"Intername=s{toolbarIntername}");
                        cfg.ToolbarsCmd.Add($"StatusText=sОтображение панели {panelName}");
                        cfg.ToolbarsCmd.Add($"ToolTipText=sОтображение панели {panelName}");
                        cfg.ToolbarsCmd.Add($"DispName=sОтображение панели {panelName}");
                        cfg.ToolbarsCmd.Add($"LocalName=s{toolbarLocalName}");

                        //добавлять к команде показа панели иконку, по первой команде панели
                        cfg.ToolbarsCmd.AddRange(Utils.IconDefinition(Panel.Command[0]));

                        //+ ****** поп меню ****************
                        cfg.ToolbarPopupMenu.Add($"{popupPanelRoot}\\{toolbarIntername}]");
                        cfg.ToolbarPopupMenu.Add($"Name=s{panelName}");
                        cfg.ToolbarPopupMenu.Add($"InterName=s{toolbarIntername}");

                        //+ ****** вью меню ****************
                        cfg.ToolbarsViewMenu.Add($"{viewPanelRoot}\\{toolbarIntername}]");
                        cfg.ToolbarsViewMenu.Add($"Name=s{panelName}");
                        cfg.ToolbarsViewMenu.Add($"InterName=s{toolbarIntername}");

                        //+ **** уровень команды ********
                        foreach (CommandDefinition cmd in Panel.Command)
                        {

                        }
                    }

                }


            }
            
            #endregion

            #region Формируем панельки и прописываем команды
            //todo группировка по панелькам

            
            #endregion

            #region Привязываем панельки к попап и виев
            //группировка по App и панелькам
            
            #endregion
            /*
            foreach (AppDefinition App in rep.GroupingAppAddonPanel)
            {
                string appName = App.Name;
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

                foreach (var panel in App.Addons )
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

                    CommandDefinition _cmd = panel.command[0] as CommandDefinition; //добавлять к команде показа панели иконку, по первой команде панели

                    //команды
                    toolbarsCmd += $"{newLine}{newLine}[\\configman\\commands\\{intername}]" +
                                    $"{newLine}weight=i10" +
                                    $"{newLine}cmdtype=i0" +
                                    $"{newLine}Intername=s{intername}" +
                                    $"{newLine}StatusText=sОтображение панели {panelName}" +
                                    $"{newLine}ToolTipText=sОтображение панели {panelName}" +
                                    $"{newLine}DispName=sОтображение панели {panelName}" +
                                    $"{newLine}LocalName=s{localName}" +
                                    $"{Utils.IconDefinition(_cmd)}";
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
            ribbonTabSource.Add(new XAttribute("Text", addonNameGlobal));
            ribbonTabSource.Add(new XAttribute("UID", $"{addonNameGlobal.Replace(" ", "")}_Tab"));
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

            Console.WriteLine($"\nФайлы:\t{addonNameGlobal}.cfg" +
                $"\n\t{addonNameGlobal}.cuix" +
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