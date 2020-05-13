using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {

            PointOfEntry.Run();
            //HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            //Animal duke = db.Animals.Where(a => a.AnimalId == 1).Single();
            //Query.RemoveAnimal(duke);
        }
    }
}
