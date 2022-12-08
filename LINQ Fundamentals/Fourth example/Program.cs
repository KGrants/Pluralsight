using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Fourth_example;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Cars
{
    class Program
    {
        public static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData();
            QueryData();
        }

        private static void QueryData()
        {
            var db = new CarDb();

            var query = db.Cars.Where(m => m.Manufacturer =="BMW")
                               .OrderByDescending(m => m.Combined)
                               .ThenBy(m => m.Name);

            var query2 =
                from car in db.Cars
                where car.Manufacturer == "BMW"
                orderby car.Combined descending, car.Name ascending
                select car;


            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Name} - {car.Combined}");
            }
        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();

            if(!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
            }
            db.SaveChanges();
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"https://www.test.com/";
            var ex = (XNamespace)"https://www.test.com/test";
            var document = XDocument.Load("fuel.xml");

            var query = 
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car")
                where element.Attribute("Manufacturer").Value == "BMW"
                select element.Attribute("Name").Value;
            
            var query2 =
             document.Element(ns + "Cars").Elements(ex + "Car").Where(c => c.Attribute("Manufacturer").Value == "BMW").Select(c => c.Attribute("Name").Value);

            foreach (var h in query)
                Console.WriteLine(h);
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var ns = (XNamespace)"https://www.olybetliga.com/personas/karlis-grants/1643/2934";
            var ex = (XNamespace)"https://www.olybetliga.com/personas/karlis-grants/1643/2934/test";
           
            var document = new XDocument();
            var cars = new XElement(ns + "Cars", 
                from record in records
                select new XElement(ex + "Car", 
                                    new XAttribute("Name", record.Name),
                                    new XAttribute("Combined", record.Combined),
                                    new XAttribute("Manufacturer", record.Manufacturer)));
        
            cars.Add(new XAttribute(XNamespace.Xmlns+"ex", ex));
            document.Add(cars);
            document.Save("fuel.xml");
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToCar();

               return query.ToList();                
        }

        private static List<Manufacturer>  ProcessManufacturers(string path)
        {
            var query = 
                File.ReadAllLines(path)
                .Where(l => l.Length > 1)
                .Select(l =>
                {
                    var columns = l.Split(',');
                    return new Manufacturer
                    {
                        Name = columns[0],
                        Headquarters = columns[1],
                        Year = int.Parse(columns[2])
                    };
                });
            return query.ToList();
        }
    }

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;

        }

       
        public CarStatistics Accumulate(Car car)
        {   
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);
            return this;
        }

            public CarStatistics Compute()
        {
            Average = Total/Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; } 
        public int Count { get; set; }
        public double Average { get; set; }   

    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {

                        Year = int.Parse(columns[0]),
                        Manufacturer = columns[1],
                        Name = columns[2],
                        Displacement = double.Parse(columns[3]),
                        Cylinders = int.Parse(columns[4]),
                        City = int.Parse(columns[5]),
                        Highway = int.Parse(columns[6]),
                        Combined = int.Parse(columns[7])
                };
            }
        }
    }
}