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
            dynamic readdata=GetRes(fileName);


        }

        /// <summary>
        /// Анонимный класс из ТХТ
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public dynamic GetRes(string fileName)
        {
            dynamic readdata;
            using (StreamReader reader = new StreamReader(fileName))
            {
                readdata = reader.ReadToEnd()
                          .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                          .Skip(1).ToList()
                          .Select(x => x.Split('\t')).ToList()
                          .Where(c => !(c.Count() > 6 && c[6] == "ИСТИНА"))
                          .Select(c => new
                          {
                              DispName = c[0],
                              Intername = c[1],
                              Description = c[2],
                              PanelName = c[3],
                              SizeFeed = c[4],
                              RibbonSplitButton = c[5],
                              DontTake = c[6],
                              DontDisplay = c[7],
                              Comment = c[8],
                              HelpPriority = c[9],
                              Video = c[10],
                              BitmapDll = c[11],
                              Icon = c[12],
                              RootData = c[13]
                          }).ToList();
            }
            return readdata;
        }
    }

    public static class Test
    {


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







