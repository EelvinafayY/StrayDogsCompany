﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrayDogs.DB
{
    internal class DBConnection
    {
        //Подключение базы данных (Варя, Model 1)
        //public static Stray_DogsEntities stray_DogsEntities = new Stray_DogsEntities();

        //Подключение базы данных (Камилла, Model 2)
        public static Stray_DogsEntities1 stray_DogsEntities = new Stray_DogsEntities1();

        public static Employee logginedEmployee;
    }
}
