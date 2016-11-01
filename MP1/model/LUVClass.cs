using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1.model
{
    class LUVClass
    {
        public double L { get; set; }
        public double u { get; set; }
        public double v { get; set; }

        public LUVClass()
        {
            
        }

        public LUVClass(double L, double u, double v)
        {
            this.L = L;
            this.u = u;
            this.v = v;
        }

        public override string ToString()
        {
            return "L: " + L + " U: " + u + " V: " + v;
        }
    }
}
