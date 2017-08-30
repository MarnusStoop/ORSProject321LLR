using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class SensitivityAnalysis
    {

        public static void IdentifyXbv(double[][] optimalTable, List<string> decisionVariables)
        {

        }

        public static bool CheckIfBasicVariable(double[] columnValues)
        {
            if (columnValues.Select(x => x > 1 || x < 0).Count() > 0)
            {
                return false;
            }
            int numberOfOnes = (from v in columnValues
                                where v == 1
                                select v).Count();
            int numberOfZeros = (from v in columnValues
                                 where v == 0
                                 select v).Count();
            return false;
        }

    }
}