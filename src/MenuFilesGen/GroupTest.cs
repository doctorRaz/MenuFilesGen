using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuFilesGen
{
    //  
    public class GroupTest
    {
        public void Run(string fileName)
        {

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

            //прописка ленты
            string ribbon = $"{newLine}[\\ribbon\\{addinName}]" +
                             $"{newLine}CUIX=s%CFG_PATH%\\{addinName}.cuix" +
                             $"{newLine}visible=f1";

            //прописка команд
            string configman = $"{newLine}[\\configman]" +
                        $"{newLine}[\\configman\\commands]";//имхо это лишнее надо проверить

            //прописка хоткеев
            string accelerators = $"{newLine}[\\Accelerators]";



            foreach (var root in hierarchicalGrouping)
            {
                string _root = root.root;

                foreach (var panel in root.panel)
                {
                    string _panel = panel.panel;

                    foreach (var cmd in panel.command)
                    {
       
                        Console.WriteLine($"    - {cmd.Intername}");
                        Console.WriteLine($"        - {cmd.DispName}");
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(cfgFilePath, false, Encoding.GetEncoding(1251)))
            {
                writer.WriteLine(ribbon);
                writer.WriteLine(configman);
                writer.WriteLine(accelerators);
            }
            //! в нанокад бага не умеет в ком строке UTF-8 латиницу, поэтому в АСКИ
            //using (StreamWriter writer = new StreamWriter(cfgFilePath, false, Encoding.GetEncoding(1251)))
            //{
            //    #region Регистрация команд
            //   
            //    foreach (IGrouping<string, string[]> commandGroup in commands)
            //    {
            //        foreach (string[] commandData in commandGroup)
            //        {

            //            writer.WriteLine(
            //                $"\r\n[\\configman\\commands\\{commandData[1]}]" +
            //                $"\r\nweight=i10" +
            //                $"\r\ncmdtype=i1" +
            //                $"\r\nintername=s{commandData[1]}" +
            //                $"\r\nDispName=s{commandData[0]}" +
            //                $"\r\nStatusText=s{commandData[2]}");

            //            if (!string.IsNullOrEmpty(commandData[12]))//иконки из dll
            //            {
            //                writer.WriteLine(
            //                  $"BitmapDll11=s{commandData[11]}" +
            //                  $"\r\nIcon=s{commandData[12]}"
            //                  );
            //            }
            //            else if (!string.IsNullOrEmpty(commandData[11])) //прописана  иконка с относительным путем и расширением
            //            {
            //                writer.WriteLine(
            //                    $"BitmapDll11=s{commandData[11]}");
            //            }
            //            else //иконка не прописана, имя иконки название команды в каталоге \\icons
            //            {
            //                writer.WriteLine(
            //                     $"BitmapDll11=sicons\\{commandData[1]}");
            //            }
            //        }
            //    }

            //    #endregion
            //    #region Классическое меню

            //    //header
            //    if (!string.IsNullOrEmpty(rootName))
            //    {
            //        writer.WriteLine(
            //        "\r\n[\\menu]" +
            //        $"\r\n[\\menu\\{rootName}]" +
            //        $"\r\nName=s{rootName}" +
            //        $"\r\n[\\menu\\{rootMenu}_Menu]" +
            //        $"\r\nName=s{addinName}");
            //    }
            //    else
            //    {
            //        writer.WriteLine(
            //        "\r\n[\\menu]" +
            //        $"\r\n[\\menu\\{rootMenu}_Menu]" +
            //        $"\r\nName=s{addinName}");
            //    }



            //    foreach (IGrouping<string, string[]> commandGroup in commands)
            //    {
            //        writer.WriteLine(
            //            $@"[\menu\{rootMenu}_Menu\{commandGroup.Key}]" +
            //            $"\r\nname=s{commandGroup.Key}");

            //        foreach (string[] commandData in commandGroup)
            //        {
            //            writer.WriteLine(
            //                $@"[\menu\{rootMenu}_Menu\{commandGroup.Key}\s{commandData[1]}]" +
            //                $"\r\nname=s{commandData[0]}" +
            //                $"\r\nIntername=s{commandData[1]}");
            //        }
            //    }

            //    #endregion

            //    #region Панели инструментов


            //    //todo добавить в [\menu\View\toolbars] и ...[\ToolbarPopupMenu\
            //    string toolbarLine = "\r\n[\\toolbars]";
            //    string toolbarLineCmd = "";

            //    //writer.WriteLine("\r\n[\\toolbars]");

            //    foreach (IGrouping<string, string[]> commandGroup in commands)
            //    {
            //        var panelName = $"{addinName}_{commandGroup.Key.Replace(' ', '_')}";


            //        //tool bar
            //        toolbarLine += $"\n[\\toolbars\\{panelName}]" +
            //                $"\r\nname=s{commandGroup.Key}\n" /*+
            //     $"\r\nIntername=s{panelName}"*/;

            //        //writer.WriteLine($"[\\toolbars\\{panelName}]" +
            //        //        $"\r\nname=s{commandGroup.Key}" /*+
            //        //        $"\r\nIntername=s{panelName}"*/);

            //        //cmd
            //        toolbarLineCmd += $"[\\configman\\commands\\ShowToolbar_{panelName}]\n";
            //        toolbarLineCmd += $"weight=i0\n";
            //        toolbarLineCmd += $"cmdtype=i0\n";
            //        toolbarLineCmd += $"intername=sShowToolbar_{panelName}\n";
            //        /* добавить
            //            LocalName=sПанель_публикации_PlotSPDS
            //            BitmapDll11=sPlotSPDS_Res.dll
            //            Icon12=sPLOT
            //            StatusText=sПоказать/Скрыть панель PlotSPDS
            //            ; ToolTipText=sПоказать/Скрыт панель PlotSPDS
            //            DispName0=sПоказать/Скрыть панель PlotSPDS
            //        */
            //        foreach (string[] commandData in commandGroup)
            //        {
            //            toolbarLine += $"[\\toolbars\\{panelName}\\{commandData[1]}]" +
            //                         $"\r\nIntername=s{commandData[1]}\n";

            //            //writer.WriteLine(
            //            //    $"[\\toolbars\\{panelName}\\{commandData[1]}]" +
            //            //    $"\r\nIntername=s{commandData[1]}");
            //        }
            //    }
            //    writer.WriteLine(toolbarLine);
            //    writer.WriteLine(toolbarLineCmd);

            //    #endregion

            //    #region  [\menu\View\toolbars]
            //    /*
            //    [\menu\View]
            //    [\menu\View\toolbars]
            //    [\menu\View\toolbars\drzTools]
            //    Name=sdrzTools
            //    [\menu\View\toolbars\drzTools\ShowToolbar_Correct_Blocks]
            //    Name=sCorrect Blocks
            //    InterName=sShowToolbar_Correct_Blocks

            //    */
            //    #endregion


            //    #region [\ToolbarPopupMenu\

            //    #endregion

            //    //todo [\Accelerators]
            //}








            //группировка по PanelName3
            List<IGrouping<string, CommandDescription>> groups = readdata
                                                                    .GroupBy
                                                                     (e => e.PanelName3).ToList();

            Console.ReadKey();

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
    }

    public static class Test
    {

        //https://dotnettutorials.net/lesson/groupby-in-linq/
        public static void prg()
        {
            var hierarchicalGrouping = Employee.GetAllEmployees()
           .GroupBy(e => e.Department)
           .Select(departmentGroup => new
           {
               Department = departmentGroup.Key,
               Roles = departmentGroup
                   .GroupBy(e => e.Role)
                   .Select(roleGroup => new
                   {
                       Role = roleGroup.Key,
                       Employees = roleGroup.ToList()
                   })
                   .ToList()
           })
           .ToList();
            foreach (var department in hierarchicalGrouping)
            {
                Console.WriteLine($"Department: {department.Department}");
                foreach (var role in department.Roles)
                {
                    Console.WriteLine($"  Role: {role.Role}");
                    foreach (var employee in role.Employees)
                    {
                        Console.WriteLine($"    - {employee.Name}");
                    }
                }
            }

            Console.ReadKey();
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
class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public string Role { get; set; }
    public static List<Employee> GetAllEmployees()
    {
        List<Employee> employees = new List<Employee>
            {
                new Employee { Name = "Alice", Department = "IT", Role = "Developer" },
                new Employee { Name = "Bob", Department = "IT", Role = "Tester" },
                new Employee { Name = "Charlie", Department = "HR", Role = "Recruiter" },
                new Employee { Name = "David", Department = "IT", Role = "Developer" },
                new Employee { Name = "Eve", Department = "HR", Role = "Manager" },
                new Employee { Name = "Frank", Department = "IT", Role = "Developer" }
            };
        return employees;
    }
}







