using Second_example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static void Main(string[] args)
    {
        Func<int, int> square = x => x * x;
        Func<int, int, int> add = (x, y) => x + y;
        Console.WriteLine(square(add(2, 2)));

        Action<int> write = x => Console.WriteLine(x);
        write(square(add(3, 3)));


        var developers = new Employee[]
                {
            new Employee{Id = 1, Name = "Scott"},
            new Employee{Id = 2, Name = "Chris"}
                };

        var sales = new List<Employee>()
        {
            new Employee { Id = 3, Name = "Alex"}
        };

        var query = developers.Where(e => e.Name.Length == 5)
                              .OrderBy(e => e.Name)
                              .Select(e => e).Count();

        var query2 = from developer in developers
                     where developer.Name.Length == 5
                     orderby developer.Name
                     select developer;

        foreach (var employee in query2)
            Console.WriteLine(employee.Name);

        foreach (var employee in sales.Where(
                e => e.Name.StartsWith('A')))
            Console.WriteLine(employee.Name);
    }
}