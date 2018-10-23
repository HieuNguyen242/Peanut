using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAPINUT.Models.Account
{
    public class OrderForUserViewModel
    {
        public int OrderNumber { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime OrderDate { get; set; }
    }
}