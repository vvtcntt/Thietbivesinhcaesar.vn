using System;
using System.Collections.Generic;

namespace Caesar.Models
{
    public partial class tblCompetitor
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Website { get; set; }
        public Nullable<int> Ord { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
    }
}
