using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{

    public class OrderViewModel
    {
        [Required(ErrorMessage = "Tên phải được nhập")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Họ phải được nhập")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string AddressLine1 { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]

        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tỉnh hoặc thành phố")]
        public string City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có 10 số")]
        public string PhoneNumber { get; set; }

        public string Notes { get; set; }

        public List<Carts> carts { get; set; }

        public List<Products> products { get; set; }
    }
}