using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko
{
    static class ChotaikoUtilities
    {
        static double LinearInterpolate(double a, double b, double pos)
        {
            if (pos <= 0) return a;
            else if (pos >= 1) return b;
            else return a + ((b - a) * pos);
        }

        static int LinearInterpolate(int a, int b, double pos)
        {
            if (pos <= 0) return a;
            else if (pos >= 1) return b;
            else return (int)(a + ((b - a) * pos));
        }
    }
}
