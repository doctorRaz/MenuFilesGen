using MenuFilesGen.CFG;
using MenuFilesGen.Repositories;
using MenuFilesGen.Service;
using System.Text;

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

            Console.WriteLine
                    (
                        $"Аргументы ком строки:" +
                        $"\n\t-hrr:[сколько строк пропускать, число] - {argsCmdLine.HeaderRowRange}" +
                        $"\n\t-xpn:[для *.XLS номер листа шаблона, число] - {argsCmdLine.XlsPageNumber}" +
                        $"\n\t-exo:[подтверждать выход из консоли - 1, не подтверждать - 0] - {argsCmdLine.EchoOnOff}" +
                        $"\n\t[\"полный путь к файлу шаблона с расширением\"] - {argsCmdLine.FileName}\n" +
                        $"\n\t[\"путь к выходному каталогу\"] - {argsCmdLine.DirectoryPath}\n"
                    );



            if (string.IsNullOrEmpty(argsCmdLine.FileName))//если имя файла не аргумент
            {
                OpenFileDialog OFD = new OpenFileDialog() { Filter = "Книга Excel (*.xls*)|*.xls*|Юникод  разделитель табуляция (*.txt;*.tsv)|*.txt;*.tsv|ASCI разделитель точка запятая (*.csv)|*.csv|Все файлы (*.*)|*.*" };
                if (OFD.ShowDialog(new Form() { TopMost = true/*, TopLevel =true*/ }) != DialogResult.OK)
                {
                    Console.WriteLine("Не задан файл");
                    Console.WriteLine("\nДля выхода нажмите любую клавишу...");
                    if (argsCmdLine.EchoOnOff) Console.ReadKey();
                    return;
                }
                argsCmdLine.FileName = OFD.FileName;//файл шаблона в аргументы
            }

            CommandRepository rep = new CommandRepository(argsCmdLine);//парсим файл

            if (rep.CommandDefinitions is null || rep.CommandDefinitions.Count < 1)//если ничего не напарсили
            {
                Console.WriteLine($"Файл {rep.FileFullName} не прочитан");

                argsCmdLine.FileName = "";//чистим путь к шаблону, на случай если будем запрашивать его в цикле
                Console.WriteLine("Для выхода нажмите любую клавишу...");
                Console.ReadKey();

                return;
            }

            string addonNameGlobal = rep.AddOnNameGlobal;//x 

            CfgCreator cfgCreater = new CfgCreator(rep.CommandDefinitions, rep.AddOnNameGlobal);

            if (argsCmdLine.EchoOnOff)
            {
                Utils.CfgConsoleWriter(cfgCreater.Cfg);//вывод в консоль результата
                Console.WriteLine(cfgCreater.XDoc);
            }

            #region Save *.cfg            

            rep.SaveToCfg(cfgCreater.Cfg);

            rep.SaveToCuix(cfgCreater.XDoc);

            #endregion

            Console.WriteLine($"\nФайлы:\t{addonNameGlobal}.cfg" +
                $"\n\t{addonNameGlobal}.cuix" +
                $"\nсохранены в: {rep.directoryPath}" +
                $"\n");

            if (argsCmdLine.EchoOnOff)
            {
                Console.WriteLine("\nДля выхода нажмите любую клавишу...");
                Console.ReadKey();
            }

        }

    }
}