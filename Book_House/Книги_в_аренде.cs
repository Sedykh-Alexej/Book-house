//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Book_House
{
    using System;
    using System.Collections.Generic;
    
    public partial class Книги_в_аренде
    {
        public int id { get; set; }
        public Nullable<int> id_Сотрудника { get; set; }
        public Nullable<int> id_Клиента { get; set; }
        public Nullable<int> id_Книги { get; set; }
        public System.DateTime Дата_получения { get; set; }
        public System.DateTime Дата_возврата { get; set; }
        public int Статус { get; set; }
        public Nullable<System.DateTime> Фактическая_дата_возврата { get; set; }
        public Nullable<int> Количество { get; set; }
        public Nullable<int> К_оплате { get; set; }
    
        public virtual Клиенты Клиенты { get; set; }
        public virtual Книги Книги { get; set; }
        public virtual Сотрудники Сотрудники { get; set; }
        public virtual Статус Статус1 { get; set; }
    }
}
