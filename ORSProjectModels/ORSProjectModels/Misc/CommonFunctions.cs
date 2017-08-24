using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class CommonFunctions
    {

        public static bool CheckForDegenercy(double columnValue, double ratioValue)
        {
            if (ratioValue == 0)
            {
                if (columnValue < 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}