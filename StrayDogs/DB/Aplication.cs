//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StrayDogs.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Aplication
    {
        public int IDApplication { get; set; }
        public Nullable<int> IDDog { get; set; }
        public string Phone { get; set; }
        public Nullable<int> IDStatusAplication { get; set; }
    
        public virtual ApplicationStatus ApplicationStatus { get; set; }
        public virtual Dog Dog { get; set; }
    }
}
