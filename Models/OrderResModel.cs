using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{
    public class OrderResModel
    {
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public string UserID { get; set; }
        public string NameCustomer { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string NoteOrder { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public virtual ICollection<OrderProductResModel> OrderProduct { get; set; }

    }
}