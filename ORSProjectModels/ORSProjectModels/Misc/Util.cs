using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class Util
    {

        public static double AbsoluteValue(double value)
        {
            if (value < 0)
            {
                return value * -1;
            }
            return value;
        }

    }
}