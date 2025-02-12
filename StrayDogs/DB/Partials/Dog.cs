using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrayDogs.DB
{
    public partial class Dog
    {
        public string Color
        {
            get
            {
                var aplicationDog = DBConnection.stray_DogsEntities.Aplication
    .Where(x => x.IDDog == Id)
    .ToList();

                return aplicationDog.Any(x => x.IDStatusAplication == 3) ? "#8BA598" : "#16343C";
            }
        }
        public string ColorButton
        {
            get
            {
                var aplicationDog = DBConnection.stray_DogsEntities.Aplication
    .Where(x => x.IDDog == Id)
    .ToList();

                return aplicationDog.Any(x => x.IDStatusAplication == 3) ? "#16343C" : "#8BA598";
            }
        }
        public string ColorButtonCursor
        {
            get
            {
                var aplicationDog = DBConnection.stray_DogsEntities.Aplication
    .Where(x => x.IDDog == Id)
    .ToList();

                return aplicationDog.Any(x => x.IDStatusAplication == 3) ? "#8BA598" : "#16343C";
            }
        }

    }
}
