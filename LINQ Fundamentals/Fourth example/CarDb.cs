using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Cars
{
    internal class CarDb : DbContext
    {
        public DbSet<Car> Cars { get; set; }
    }
}
