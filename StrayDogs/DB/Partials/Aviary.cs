using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrayDogs.DB
{
    public partial class Aviary
    {
        public string Status
        {
            get
            {
                return DBConnection.stray_DogsEntities.Dog.Any(dog => dog.IdAviary == this.Id) ? "Занят" : "Свободен";
            }
        }
    }
}
