using System;
using System.Collections.Generic;

namespace Caesar.Models
{
    public partial class tblWeb
    {
        public int id { get; set; }
        public string Url { get; set; }
        public Nullable<int> Ord { get; set; }
    }
}
