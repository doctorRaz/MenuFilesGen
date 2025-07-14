using MenuFilesGen.Models;
using MenuFilesGen.Repositories;
using NickBuhro.Translit;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MenuFilesGen
{
    //  
    public class GroupTest
    {
        public void Run(string fileName)
        {
            ColumnNumbers  _columnNumbers = new ColumnNumbers();
            CommandRepository rep = new CommandRepository();
            rep.ReadFromCsv(fileName, _columnNumbers);

            // https://stackoverflow.com/questions/1159233/multi-level-grouping-in-linq

            List<CommandDescription> readdata = GetRes(fileName);//прочитали файл в класс

            string newLine = Environment.NewLine;

            string directoryPath = Path.GetDirectoryName(fileName);
            string addinName = Path.GetFileNameWithoutExtension(fileName);

            string cfgFilePath = $"{directoryPath}\\{addinName}.cfg";
            string cuiFilePath = $"{directoryPath}\\RibbonRoot.cui";
            string cuixFilePath = $"{directoryPath}\\{addinName}.cuix";

            //группируем по RootData13 и PanelName3
            var hierarchicalGrouping = GetHierarchicalGrouping(readdata);

            //собираем в строки конфиг

            //прописываем ленту
            string ribbon = $"{newLine}[\\ribbon\\{addinName}]" +
                            $"{newLine}CUIX=s%CFG_PATH%\\{addinName}.cuix" +
                            $"{newLine}visible=f1";

            //команды
            string configman = $"{newLine}[\\configman]" +
                        $"{newLine}[\\configman\\commands]";//имхо это лишнее надо проверить

            //горячие клавиши
            string accelerators = $"{newLine}[\\Accelerators]";
            /* [\Accelerators]
                drz_PublishMC=sCtrl+Shift+P
            */

            //меню
            string menu = $"{newLine}[\\menu]";

            //панели
            string toolbars = $"{newLine};Панели" +
                                $"{newLine}[\\toolbars]";

            //всплывающее меню панелей
            string toolbarPopupMenu = $"{newLine};Popup меню" +
                                    $"{newLine}[\\ToolbarPopupMenu]" +
                                    $"{newLine}[\\ToolbarPopupMenu\\{addinName}]" +
                                    $"{newLine}Name=s{addinName}";

            //команды вызова панелей
            string toolbarsCmd = $"{newLine}; Команды вызова панелей";

            //меню вид панелей
            string toolbarsViewMenu = $"{newLine};View меню"+
                                        $"{newLine}[\\menu\\View\\toolbars\\{addinName}]" +
                                        $"{newLine}Name=s{addinName}";

            foreach (var root in hierarchicalGrouping)
            {
                string rootName = root.root;
                string rootMenu = $"{addinName}";

                #region Классическое меню шапка

                if (!string.IsNullOrEmpty(rootName))
                {
                    rootMenu = $"{rootName}\\{addinName}";

                    menu += $"{newLine}[\\menu\\{rootName}]" +
                            $"{newLine}Name=s{rootName}" +
                            $"{newLine}[\\menu\\{rootMenu}]" +
                            $"{newLine}Name=s{addinName}";

                }
                else
                {
                    menu += $"{newLine}[\\menu\\{rootMenu}]" +
                            $"{newLine} Name=s{addinName}";
                }
                #endregion

                foreach (var panel in root.panel)
                {
                    string panelName = panel.panel;


                    string panelNameRu = $"{addinName}_{panelName.Replace(' ', '_')}";//todo бага в меню вид панель с именем команд 
                    string panelNameEn =Transliteration.CyrillicToLatin(panelNameRu, Language.Russian);
                    //panelNameEn = panelNameEn.Replace('`', '0');

                    string intername = $"ShowToolbar_{panelNameEn}";
                    string localName = $"Панель_{panelNameRu}";
                    #region Панели

                    //панели
                    toolbars += $"{newLine}{newLine}[\\toolbars\\{panelNameEn}]" +
                                $"{newLine}name=s{panelName}";

                    //команды
                    toolbarsCmd += $"{newLine}{newLine}[\\configman\\commands\\{intername}]" +
                                    $"{newLine}weight=i10" +
                                    $"{newLine}cmdtype=i0" +
                                    $"{newLine}Intername=s{intername}" +
                                    $"{newLine}StatusText=sОтображение панели {panelName}" +
                                    $"{newLine}ToolTipText=sОтображение панели {panelName}" +
                                    $"{newLine}DispName=sОтображение панели {panelName}" +
                                    $"{newLine}LocalName=s{localName}";

                    //поп меню
                    toolbarPopupMenu += $"{newLine}[\\ToolbarPopupMenu\\{addinName}\\{intername}]"+
                     $"{newLine}Name=s{panelName}"+
                     $"{newLine}InterName=s{intername}";
                    //вью меню
                    toolbarsViewMenu += $"{newLine}[\\menu\\View\\toolbars\\{addinName}\\{intername}]"+
                                        $"{newLine}Name=s{panelName}"+
                                        $"{newLine}InterName=s{intername}";

                    #endregion

                    #region Классическое меню раздел

                    menu += $"{newLine}{newLine}[\\menu\\{rootMenu}\\{panelName}]" +
                            $"{newLine}name=s{panelName}";

                    #endregion

                    foreach (CommandDescription cmd in panel.command)
                    {
                        #region Регистрация команд
                        configman += $"{newLine}{newLine}[\\configman\\commands\\{cmd.Intername1}]" +
                                     $"{newLine}weight=i10" +
                                     $"{newLine}cmdtype=i1" +
                                     $"{newLine}intername=s{cmd.Intername1}" +
                                     $"{newLine}DispName=s{cmd.DispName0}" +
                                     $"{newLine}StatusText=s{cmd.Description2}";

                        if (!string.IsNullOrEmpty(cmd.Icon12))//иконки из dll
                        {
                            configman += $"{newLine}BitmapDll=s{cmd.BitmapDll11}" +
                                         $"{newLine}Icon=s{cmd.Icon12}";
                        }
                        else if (!string.IsNullOrEmpty(cmd.BitmapDll11)) //прописана  иконка с относительным путем и расширением
                        {
                            configman += $"{newLine}BitmapDll=s{cmd.BitmapDll11}";
                        }
                        else //иконка не прописана, имя иконки название команды в каталоге \\icons
                        {
                            configman += $"{newLine}BitmapDll=sicons\\{cmd.Intername1}.ico";
                        }
                        #endregion

                        #region Классическое меню

                        menu += $"{newLine}[\\menu\\{rootMenu}\\{panelName}\\s{cmd.Intername1}]" +
                                $"{newLine}name=s{cmd.DispName0}" +
                                $"{newLine}Intername=s{cmd.Intername1}";

                        #endregion

                        #region Панели
                        toolbars += $"{newLine}[\\toolbars\\{panelNameEn}\\{cmd.Intername1}]" +
                                    $"{newLine}Intername=s{cmd.Intername1}";
                        #endregion
                    }
                }
            }

            #region Save *.cfg            

            using (StreamWriter writer = new StreamWriter(cfgFilePath, false, Encoding.GetEncoding(65001)))
            //using (StreamWriter writer = new StreamWriter(cfgFilePath, false, new UTF8Encoding(false)))
            //using (StreamWriter writer = new StreamWriter(cfgFilePath, false, Encoding.GetEncoding(1251)))
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

            #endregion

            //группировка по PanelName3
            List<IGrouping<string, CommandDescription>> groupsPanel = readdata
                                                                    .GroupBy
                                                                     (e => e.PanelName3).ToList();

            #region Ribbon
            // Ленточное меню
            //Создание XML документа 
            var xDoc = new XDocument();
            //Корневой элемент
            var ribbonRoot = new XElement("RibbonRoot");
            xDoc.Add(ribbonRoot);

            var ribbonPanelSourceCollection = new XElement("RibbonPanelSourceCollection");
            ribbonRoot.Add(ribbonPanelSourceCollection);

            var ribbonTabSourceCollection = new XElement("RibbonTabSourceCollection");
            ribbonRoot.Add(ribbonTabSourceCollection);

            var ribbonTabSource = new XElement("RibbonTabSource");
            ribbonTabSource.Add(new XAttribute("Text", addinName));
            ribbonTabSource.Add(new XAttribute("UID", $"{addinName.Replace(" ", "")}_Tab"));
            ribbonTabSourceCollection.Add(ribbonTabSource);

            foreach (IGrouping<string, CommandDescription> cmd in groupsPanel)
            //foreach (IGrouping<string, string[]> commandGroup in commands)
            {
                var ribbonPanelSource = new XElement("RibbonPanelSource");
                ribbonPanelSource.Add(new XAttribute("UID", cmd.Key));
                ribbonPanelSource.Add(new XAttribute("Text", cmd.Key));
                ribbonPanelSourceCollection.Add(ribbonPanelSource);

                // Временный контейнер для сбора кнопок
                var panelButtons = new XElement("Temp");

                // Группируем команды по тому, объединяются ли они в RibbonSplitButton5
                List<IGrouping<string, CommandDescription>> unitedCommands = cmd.GroupBy(c => c.RibbonSplitButton5).ToList();

                foreach (IGrouping<string, CommandDescription> unitedCommandGroup in unitedCommands)
                {
                    XElement container = panelButtons;
                    if (!string.IsNullOrWhiteSpace(unitedCommandGroup.Key))
                    {
                        var ribbonSplitButton = new XElement("RibbonSplitButton");
                        ribbonSplitButton.Add(new XAttribute("Text", unitedCommandGroup.Key));
                        ribbonSplitButton.Add(new XAttribute("Behavior", "SplitFollowStaticText"));
                        ribbonSplitButton.Add(new XAttribute("ButtonStyle", unitedCommandGroup. First().SizeFeed4));

                        panelButtons.Add(ribbonSplitButton);
                        container = ribbonSplitButton;
                    }

                    foreach (CommandDescription commandData in unitedCommandGroup)
                        container.Add(CreateButton(commandData));
                }

                var sortedButtons = panelButtons
                    .Elements()
                    .GroupBy(button => button.Attributes().First(attr => attr.Name == "ButtonStyle").Value.Contains("Small"))
                    .ToDictionary(g => g.Key);

                if (sortedButtons.ContainsKey(false))
                {
                    foreach (var button in sortedButtons[false])
                        ribbonPanelSource.Add(button);
                }

                if (sortedButtons.ContainsKey(true))
                {
                    XElement ribbonRowPanel = null;
                    var ribbonRowPanelButtonsCount = 3;

                    foreach (var button in sortedButtons[true])
                    {
                        if (ribbonRowPanelButtonsCount == 3)
                        {
                            ribbonRowPanel = new XElement("RibbonRowPanel");
                            ribbonPanelSource.Add(ribbonRowPanel);
                            ribbonRowPanelButtonsCount = 0;
                        }

                        var ribbonRow = new XElement("RibbonRow");
                        ribbonRow.Add(button);
                        ribbonRowPanel.Add(ribbonRow);

                        ribbonRowPanelButtonsCount++;
                    }
                }

                var ribbonPanelSourceReference = new XElement("RibbonPanelSourceReference");
                ribbonPanelSourceReference.Add(new XAttribute("PanelId", cmd.Key));
                ribbonTabSource.Add(ribbonPanelSourceReference);
            }

            xDoc.Save(cuiFilePath);

            // Удаление .cuix файла, если он существует
            if (File.Exists(cuixFilePath))
                File.Delete(cuixFilePath);

            // Создание архива (.cuix), добавление в него сформированного .xml файла
            using (var zip = ZipFile.Open(cuixFilePath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(cuiFilePath, "RibbonRoot.cui");
            }

            // Удаление ribbon.cui файла, если он существует
            if (File.Exists(cuiFilePath))
                File.Delete(cuiFilePath);

            MessageBox.Show($"Файлы {addinName}.cfg и {addinName}.cuix сохранены в папке {directoryPath}");




            #endregion

             

            //Console.ReadKey();

        }

        public dynamic GetHierarchicalGrouping(List<CommandDescription> readdata)
        {
            var hierarchicalGrouping = readdata
                .GroupBy(e => e.RootData13)
                .Select(root => new
                {
                    root = root.Key,
                    panel = root
                .GroupBy(e => e.PanelName3)
                .Select(panel => new
                {
                    panel = panel.Key,
                    command = panel.ToList()
                }).ToList()
                }).ToList();
            return hierarchicalGrouping;

        }


        /// <summary>
        /// Анонимный класс из ТХТ
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public List<CommandDescription> GetRes(string fileName)
        {
            List<CommandDescription> readdata;
            using (StreamReader reader = new StreamReader(fileName))
            {
                readdata = reader.ReadToEnd()
                          .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                          .Skip(1).ToList()
                          .Select(x => x.Split('\t')).ToList()
                          .Where(c => !(c.Count() > 6 && c[6] == "ИСТИНА"))
                          .Select(c => new CommandDescription
                          {
                              DispName0 = c[0],
                              Intername1 = c[1],
                              Description2 = c[2],
                              PanelName3 = c[3],
                              SizeFeed4 = c[4],
                              RibbonSplitButton5 = c[5],
                              DontTake6 = c[6],
                              DontDisplay7 = c[7],
                              Comment8 = c[8],
                              HelpPriority9 = c[9],
                              Video10 = c[10],
                              BitmapDll11 = c[11],
                              Icon12 = c[12],
                              RootData13 = c[13]
                          }).ToList();
            }
            return readdata;
        }

        public static XElement CreateButton(CommandDescription commandData)
        {
            var ribbonCommandButton = new XElement("RibbonCommandButton");
            ribbonCommandButton.Add(new XAttribute("Text", commandData.DispName0));
            ribbonCommandButton.Add(new XAttribute("ButtonStyle", commandData.SizeFeed4));
            ribbonCommandButton.Add(new XAttribute("MenuMacroID", commandData.Intername1));
            return ribbonCommandButton;
        }
    }



}


public class CommandDescription
{
    public string DispName0 { get; set; }
    public string Intername1 { get; set; }
    public string Description2 { get; set; }
    public string PanelName3 { get; set; }
    public string SizeFeed4 { get; set; }
    public string RibbonSplitButton5 { get; set; }
    public string DontTake6 { get; set; }
    public string DontDisplay7 { get; set; }
    public string Comment8 { get; set; }
    public string HelpPriority9 { get; set; }
    public string Video10 { get; set; }
    public string BitmapDll11 { get; set; }
    public string Icon12 { get; set; }
    public string RootData13 { get; set; }

}








