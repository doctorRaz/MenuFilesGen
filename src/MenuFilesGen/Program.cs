﻿using System.IO.Compression;
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



            GroupTest test = new GroupTest();
            string fileNametst = @"d:\@Developers\Programmers\!NET\!bundle\BlockFix.bundle\Resources\BlockFix.txt";
            test.Run(fileNametst);
            return;
#if !DEBUG

            OpenFileDialog tableFileDialog = new OpenFileDialog() { Filter = "TXT (*.txt)|*.txt|CSV (*.csv)|*.csv|TSV files (*.tsv)|*.tsv" };
            if (tableFileDialog.ShowDialog() != DialogResult.OK)
                return;
            string fileName = tableFileDialog.FileName;

#else
            //сохранять как текст в юникоде
            //обрезать пустые строки
            //string fileName = @"d:\@Developers\Programmers\!NET\!bundle\BlockFix.bundle\Resources\BlockFix.txt";
            string fileName = @"d:\@Developers\Programmers\!NET\!bundle\BlockFix.bundle\Resources\drzTools_BlockFix.txt";

#endif

            string directoryPath = Path.GetDirectoryName(fileName);
            string csvName = Path.GetFileNameWithoutExtension(fileName);
            var name = csvName.Split('_');

            string rootName = "";
            string rootMenu = "";
            string addinName = "";

            if (name.Length > 1)//костылище(((
            {
                rootName = name[0];
                addinName = name[1];
                rootMenu = $"{rootName}\\{addinName}";
            }
            else
            {
                addinName = rootMenu = csvName;
            }

            // Описания команд,сгруппированных по имени панели
            List<IGrouping<string, string[]>> commands;
            using (StreamReader reader = new StreamReader(fileName))
            {
                //https://stackoverflow.com/questions/7647716/how-to-remove-empty-lines-from-a-formatted-string
                commands = reader
                    .ReadToEnd()
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    //.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1) // Заголовок таблицы
                             .Select(c => c.Split('\t')) // Разделитель - табуляция
                                                         //.Select(c => c.Split(';')) // Разделитель - ;
                                                         //.Where(c => !(c.Count() > 6 && c[6] == "TRUE")) // Пропуск скрытых команд
                    .Where(c => !(c.Count() > 6 && c[6] == "ИСТИНА")) // Пропуск скрытых команд
                    .GroupBy(c => c[3])
                    .ToList();
            }

            string cfgFilePath = $"{directoryPath}\\{addinName}.cfg";
            string cuiFilePath = $"{directoryPath}\\RibbonRoot.cui";
            string cuixFilePath = $"{directoryPath}\\{addinName}.cuix";

            //! в нанокад бага не умеет в ком строке UTF-8 латиницу, поэтому в АСКИ
            //using (StreamWriter writer = new StreamWriter(cfgFilePath, false, new UTF8Encoding(false)))
            using (StreamWriter writer = new StreamWriter(cfgFilePath, false, Encoding.GetEncoding(1251)))
            {
                #region Регистрация команд
                //todo сгрупировать сбор записей в один цикл
                writer.WriteLine(
                    $"[\\ribbon\\{addinName}]" +
                    $"\r\nCUIX=s%CFG_PATH%\\{addinName}.cuix" +
                    "\r\n" +
                    "\r\n[\\configman]" +
                    "\r\n[\\configman\\commands]");

                foreach (IGrouping<string, string[]> commandGroup in commands)
                {
                    foreach (string[] commandData in commandGroup)
                    {

                        writer.WriteLine(
                            $"\r\n[\\configman\\commands\\{commandData[1]}]" +
                            $"\r\nweight=i10" +
                            $"\r\ncmdtype=i1" +
                            $"\r\nintername=s{commandData[1]}" +
                            $"\r\nDispName=s{commandData[0]}" +
                            $"\r\nStatusText=s{commandData[2]}");

                        if (!string.IsNullOrEmpty(commandData[12]))//иконки из dll
                        {
                            writer.WriteLine(
                              $"BitmapDll=s{commandData[11]}" +
                              $"\r\nIcon=s{commandData[12]}"
                              );
                        }
                        else if (!string.IsNullOrEmpty(commandData[11])) //прописана  иконка с относительным путем и расширением
                        {
                            writer.WriteLine(
                                $"BitmapDll=s{commandData[11]}");
                        }
                        else //иконка не прописана, имя иконки название команды в каталоге \\icons
                        {
                            writer.WriteLine(
                                 $"BitmapDll=sicons\\{commandData[1]}");
                        }
                    }
                }

                #endregion
                #region Классическое меню

                //header
                if (!string.IsNullOrEmpty(rootName))
                {
                    writer.WriteLine(
                    "\r\n[\\menu]" +
                    $"\r\n[\\menu\\{rootName}]" +
                    $"\r\nName=s{rootName}" +
                    $"\r\n[\\menu\\{rootMenu}_Menu]" +
                    $"\r\nName=s{addinName}");
                }
                else
                {
                    writer.WriteLine(
                    "\r\n[\\menu]" +
                    $"\r\n[\\menu\\{rootMenu}_Menu]" +
                    $"\r\nName=s{addinName}");
                }



                foreach (IGrouping<string, string[]> commandGroup in commands)
                {
                    writer.WriteLine(
                        $@"[\menu\{rootMenu}_Menu\{commandGroup.Key}]" +
                        $"\r\nname=s{commandGroup.Key}");

                    foreach (string[] commandData in commandGroup)
                    {
                        writer.WriteLine(
                            $@"[\menu\{rootMenu}_Menu\{commandGroup.Key}\s{commandData[1]}]" +
                            $"\r\nname=s{commandData[0]}" +
                            $"\r\nIntername=s{commandData[1]}");
                    }
                }

                #endregion

                #region Панели инструментов


                //todo добавить в [\menu\View\toolbars] и ...[\ToolbarPopupMenu\
                string toolbarLine = "\r\n[\\toolbars]";
                string toolbarLineCmd = "";

                //writer.WriteLine("\r\n[\\toolbars]");

                foreach (IGrouping<string, string[]> commandGroup in commands)
                {
                    var panelName = $"{addinName}_{commandGroup.Key.Replace(' ', '_')}";


                    //tool bar
                    toolbarLine += $"\n[\\toolbars\\{panelName}]" +
                            $"\r\nname=s{commandGroup.Key}\n" /*+
                            $"\r\nIntername=s{panelName}"*/;

                    //writer.WriteLine($"[\\toolbars\\{panelName}]" +
                    //        $"\r\nname=s{commandGroup.Key}" /*+
                    //        $"\r\nIntername=s{panelName}"*/);

                    //cmd
                    toolbarLineCmd += $"[\\configman\\commands\\ShowToolbar_{panelName}]\n";
                    toolbarLineCmd += $"weight=i0\n";
                    toolbarLineCmd += $"cmdtype=i0\n";
                    toolbarLineCmd += $"intername=sShowToolbar_{panelName}\n";
                    /* добавить
                        LocalName=sПанель_публикации_PlotSPDS
                        BitmapDll11=sPlotSPDS_Res.dll
                        Icon12=sPLOT
                        StatusText=sПоказать/Скрыть панель PlotSPDS
                        ; ToolTipText=sПоказать/Скрыт панель PlotSPDS
                        DispName0=sПоказать/Скрыть панель PlotSPDS
                    */
                    foreach (string[] commandData in commandGroup)
                    {
                        toolbarLine += $"[\\toolbars\\{panelName}\\{commandData[1]}]" +
                                     $"\r\nIntername=s{commandData[1]}\n";

                        //writer.WriteLine(
                        //    $"[\\toolbars\\{panelName}\\{commandData[1]}]" +
                        //    $"\r\nIntername=s{commandData[1]}");
                    }
                }
                writer.WriteLine(toolbarLine);
                writer.WriteLine(toolbarLineCmd);

                #endregion

                #region  [\menu\View\toolbars]
                /*
                [\menu\View]
                [\menu\View\toolbars]
                [\menu\View\toolbars\drzTools]
                Name=sdrzTools
                [\menu\View\toolbars\drzTools\ShowToolbar_Correct_Blocks]
                Name=sCorrect Blocks
                InterName=sShowToolbar_Correct_Blocks

                */
                #endregion


                #region [\ToolbarPopupMenu\

                #endregion

                //todo [\Accelerators]
            }


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

            foreach (IGrouping<string, string[]> commandGroup in commands)
            {
                var ribbonPanelSource = new XElement("RibbonPanelSource");
                ribbonPanelSource.Add(new XAttribute("UID", commandGroup.Key));
                ribbonPanelSource.Add(new XAttribute("Text", commandGroup.Key));
                ribbonPanelSourceCollection.Add(ribbonPanelSource);

                // Временный контейнер для сбора кнопок
                var panelButtons = new XElement("Temp");

                // Группируем команды по тому, объединяются ли они в RibbonSplitButton5
                var unitedCommands = commandGroup.GroupBy(c => c[5]).ToList();

                foreach (var unitedCommandGroup in unitedCommands)
                {
                    XElement container = panelButtons;
                    if (!string.IsNullOrWhiteSpace(unitedCommandGroup.Key))
                    {
                        var ribbonSplitButton = new XElement("RibbonSplitButton");
                        ribbonSplitButton.Add(new XAttribute("Text", unitedCommandGroup.Key));
                        ribbonSplitButton.Add(new XAttribute("Behavior", "SplitFollowStaticText"));
                        ribbonSplitButton.Add(new XAttribute("ButtonStyle", unitedCommandGroup.First()[4]));

                        panelButtons.Add(ribbonSplitButton);
                        container = ribbonSplitButton;
                    }

                    foreach (string[] commandData in unitedCommandGroup)
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
                ribbonPanelSourceReference.Add(new XAttribute("PanelId", commandGroup.Key));
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
        }

        public static XElement CreateButton(string[] commandData)
        {
            var ribbonCommandButton = new XElement("RibbonCommandButton");
            ribbonCommandButton.Add(new XAttribute("Text", commandData[0]));
            ribbonCommandButton.Add(new XAttribute("ButtonStyle", commandData[4]));
            ribbonCommandButton.Add(new XAttribute("MenuMacroID", commandData[1]));
            return ribbonCommandButton;
        }
    }
}