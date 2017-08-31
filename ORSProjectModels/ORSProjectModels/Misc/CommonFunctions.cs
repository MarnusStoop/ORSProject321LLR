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

        public static string GenerateTableIteration(List<string> decisionVariables,double[][] table)
        {
            string displayable = "";
            foreach (var item in decisionVariables)
            {
                displayable += item + " ";
            }
            displayable += "RHS\n";
            foreach (var item in table)
            {
                foreach (var val in item)
                {
                    if (val < 0)
                    {
                        displayable += val + " ";
                    } else
                    {
                        displayable += val + "  ";
                    }
                }
                displayable += "\n";
            }
            FileHandler.Append(displayable, "output.txt");
            return displayable;
        }

    }
}