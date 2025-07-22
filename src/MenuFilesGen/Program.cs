using MenuFilesGen.Models;
using MenuFilesGen.Repositories;
using MenuFilesGen.Service;
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

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Utils utils = new Utils();

            ArgsCmdLine argsCmdLine = utils.ParseCmdLine(args);//читаем аргументы ком строки

            Console.WriteLine($"Аргументы ком строки:" +
                           $"\n\t-hrr:[сколько строк пропускать, число] - {argsCmdLine.HeaderRowRange}" +
                           $"\n\t-xpn:[для XLS* номер листа шаблона, число] - {argsCmdLine.XlsPageNumber}" +
                           $"\n\t-exo:[подтверждать выход - 1, не подтверждать - 0] - {argsCmdLine.EchoOnOff}" +
                           $"\n\t[\"полный путь к файлу шаблона с расширением\"] - {argsCmdLine.FileName}\n");

            if (string.IsNullOrEmpty(argsCmdLine.FileName))//если имя файла не аргумент
            {
                OpenFileDialog tableFileDialog = new OpenFileDialog() { Filter = "Книга Excel (*.xls*)|*.xls*|Юникод  разделитель табуляция (*.txt;*.tsv)|*.txt;*.tsv|ASCI разделитель точка запятая (*.csv)|*.csv|Все файлы (*.*)|*.*" };
                if (tableFileDialog.ShowDialog() != DialogResult.OK)
                {
                    Console.WriteLine("Не задан файл");
                    Console.WriteLine("\nДля выхода нажмите любую клавишу...");
                    if (argsCmdLine.EchoOnOff) Console.ReadKey();
                    return;
                }
                argsCmdLine.FileName = tableFileDialog.FileName;//файл шаблона в аргументы
            }

            CommandRepository rep = new CommandRepository(argsCmdLine);//парсим файл

            if (rep.CommandDefinitions is null || rep.CommandDefinitions.Count < 1)//если ничего не напарсили
            {
                Console.WriteLine($"Файл {rep.FileFullName} не прочитан");

                argsCmdLine.FileName = "";//чистим путь к шаблону, на случай если будем запрашивать его в цикле
                Console.WriteLine("Для выхода нажмите любую клавишу...");//todo зациклить выбор файла?
                Console.ReadKey();

                return;
            }

            string addonNameGlobal = rep.AddonNameGlobal;

            CfgCreater cfgCreater = new CfgCreater(rep.CommandDefinitions, rep.AddonNameGlobal);

            Utils.CfgConsoleWrier(cfgCreater.Cfg);
            #region Save *.cfg            

            rep.SaveToCfg(cfgCreater.Cfg);

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
                        if (commandData.HideCommand) continue;
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