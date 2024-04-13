using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{
    public class OrderProductResModel
    {
        public string ProductName { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public string Image { get; set; }
    }
}