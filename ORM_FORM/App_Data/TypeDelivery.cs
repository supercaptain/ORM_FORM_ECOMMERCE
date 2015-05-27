using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM_FORM
{
    public class TypeDelivery
    {
        public int IdDelivery { get; set; }
        public string Name { get; set; }
        public float Cost { get; set; }

        public TypeDelivery() { }

        public override string ToString()
        {
            return String.Format("ID {0}, Name {1}, Cost {2}" , IdDelivery, Name, Cost);
        }
    }
}
