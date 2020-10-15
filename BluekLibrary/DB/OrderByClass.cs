using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.DB {
    public abstract class OrderByClass {
        protected OrderByClass order=null;
        protected string Name ;
        public abstract String CreateOrderByString();

        public OrderByClass Append(OrderByClass item) {
            order=item;
            return item;
        }
    }

    public class asc: OrderByClass {
        public asc(string name) {
            Name = name;
        }

        public override string CreateOrderByString() {
            return String.Format(" {0} ASC ", Name);
        }
    }

    public class desc: OrderByClass {
        public desc(string name) {
             Name = name;
        }

        public override string CreateOrderByString() {
            return String.Format(" {0} DESC ", Name) +( order != null ? " , "+order.CreateOrderByString(): ""); 
        }

    }
}
  