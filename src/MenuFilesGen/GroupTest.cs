using NickBuhro.Translit;
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
                    var panelNameEn=Transliteration.CyrillicToLatin(panelNameRu, Language.Russian);

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







