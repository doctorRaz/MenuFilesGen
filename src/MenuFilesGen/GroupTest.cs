using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuFilesGen
{
//    //by dRz on 09.07.2025 at 11:54 не умею в группировку
//    // https://stackoverflow.com/questions/1159233/multi-level-grouping-in-linq
//    //List<IGrouping<string, List<IGrouping<string, string[]>>>> commandsTools;
//    using (StreamReader reader = new StreamReader(fileName))
//  {
//      var read = reader.ReadToEnd()
//          .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
//          .Skip(1).ToList()
//          .Select(x => x.Split('\t')).ToList()
//          .Where(c => !(c.Count() > 6 && c[6] == "ИСТИНА")).ToList();

//    var gr = read.GroupBy(c => new { root = c[13], panel = c[3] });

//    var gr0 = read.GroupBy(c => new { root = c[13], panel = c[3] },
//              (key, group) => new
//              {
//                  root = key.root,
//                  panel = key.panel,

//                  items = group.ToList()
//              }).ToList();


//    var grgr = /*from g in read*/
//               from i in read
//               group i by new { root = i[13], panel = i[3] };


//    var ggs = grgr.ToList();

//      foreach (var gg in ggs)
//      {
//          var d = gg.Key;

//    var dr = d.root;
//    var dp = d.panel;
//}
       
//  }
    public static class test
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
}






