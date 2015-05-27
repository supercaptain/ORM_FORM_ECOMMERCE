using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
     public class Login
    {
        public Customer customer { get; set; } //Chceme ziskat atribut IDCUSTOMER
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
