using MenuFilesGen.Models;

namespace MenuFilesGen.Service
{
    public class Utils
    {
        public static void CfgConsoleWriter(CfgDefinition cfg)
        {
            Console.Clear();
            Console.WriteLine("[\\cfg]");
            Console.WriteLine(cfg.Menu.LstStr());//меню
            Console.WriteLine(cfg.ToolBarPopUpMenu.LstStr()); //поп меню
            Console.WriteLine(cfg.ToolBarsViewMenu.LstStr()); //виев меню

            Console.WriteLine(cfg.ToolBars.LstStr());//панели

            Console.WriteLine(cfg.Configman.LstStr());//команды
            Console.WriteLine(cfg.ToolBarsCmd.LstStr());//команды меню

            Console.WriteLine(cfg.Ribbon.LstStr());//лента
            Console.WriteLine(cfg.Accelerators.LstStr());//горячие кнопки
        }


        /// <summary>
        /// Описание иконок
        /// </summary>
        /// <param name="cmd">Описание команд.</param>
        /// <returns>  описание иконки</returns>
        public static List<string> IconDefinition(CommandDefinition cmd)
        {
            List<string> configman = new List<string>();


            if (!string.IsNullOrEmpty(cmd.IconName))//иконки из dll
            {
                configman.Add($"BitmapDll=s{cmd.ResourceDllName}");
                configman.Add($"Icon=s{cmd.IconName}");
            }
            else if (!string.IsNullOrEmpty(cmd.ResourceDllName)) //прописана  иконка с относительным путем и расширением
            {
                configman.Add($"BitmapDll=s{cmd.ResourceDllName}");
            }
            else //иконка не прописана, имя иконки = название команды в каталоге \\icons
            {
                configman.Add($"BitmapDll=sicons\\{cmd.InterName}.ico");
            }
            return configman;
        }

        public static int StringToInt(string str, int def = 0)
        {

            if (int.TryParse(str, out int result))
            {
                return result;
            }
            else
            {
                return def;
            }

        }
        public static bool StringToBool(string str)
        {
            return str.Trim() == "1";

        }

        //https://ru.stackoverflow.com/questions/466805/Как-парсить-аргументы-командной-строки
        /// <summary>
        /// Parses the command line.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public ArgsCmdLine ParseCmdLine(string[] args)
        {
            ArgsCmdLine argsCmdLine = new ArgsCmdLine();

            List<string> argsL = args.ToList();

            if (args.Length == 0)
                return argsCmdLine;


            for (int i = 0; i < argsL.Count; i++)
            {
                //Console.WriteLine(argsL[i]);

                if (File.Exists(argsL[i]))//если файл есть
                {

                    argsCmdLine.FileName = argsL[i];

                }
                else if (Directory.Exists(args[i]))//директория есть
                {
                    argsCmdLine.DirectoryPath = argsL[i];
                }

                else if (args[i].StartsWith("-"))//аргументы ком строки
                {
                    string parameterWithoutHyphen = args[i].Substring(1);
                    string[] nameValue = parameterWithoutHyphen.Split(':');
                    if (nameValue.Length > 1)
                    {
                        switch (nameValue[0].ToLower())
                        {
                            case "xpn":
                                argsCmdLine.XlsPageNumber = StringToInt(nameValue[1], argsCmdLine.XlsPageNumber);
                                break;
                            case "hrr":
                                argsCmdLine.HeaderRowRange = StringToInt(nameValue[1], argsCmdLine.HeaderRowRange);
                                break;
                            case "exo":
                                argsCmdLine.EchoOnOff = StringToBool(nameValue[1]);
                                break;
                        }//todo добавить парсер isDuplicatePanel и добавить описание в хелп
                    }

                }

            }

            return argsCmdLine;
        }

        //static string newLine = Environment.NewLine;
    }

    /// <summary>
    /// результат парсинга ком строки
    /// </summary>
    public class ArgsCmdLine
    {
        /// <summary>
        /// Шаблон для конфига
        /// </summary>
        /// <value>
        /// The name of the files.
        /// </value>
        public string FileName { get; set; } = "";

        /// <summary>
        /// директория конфигов, может задаваться аргументом ком строки
        /// </summary>
        public string DirectoryPath { get; set; } = "";

        /// <summary>
        /// Количество пропускаемых строк шаблона -hrr:3
        /// </summary>
        /// <value>
        /// The header row range.
        /// </value>
        public int HeaderRowRange { get; set; } = 3;

        /// <summary>
        /// Номер листа шаблона если эксель -xln:1
        /// </summary>
        /// <value>
        /// The XLS page number умолчание 0, такого листа не существует.
        /// </value>
        public int XlsPageNumber { get; set; } = 0;

        /// <summary>
        /// Подтверждать выход 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [echo ON]; otherwise, <c>false</c>.
        /// </value>
        public bool EchoOnOff { get; set; } = true;


        /// <summary>
        /// Дубликат панели в выпадашке под лентой
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is duplicate panel; otherwise, <c>false</c>.
        /// </value>
        public bool IsDuplicatePanel { get; set; } = true;//todo IsDuplicatePanel


    }
}
