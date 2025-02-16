using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrayDogs.DB
{
    public partial class Employee
    {
        public string FullName
        {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }

        }
    }

}
