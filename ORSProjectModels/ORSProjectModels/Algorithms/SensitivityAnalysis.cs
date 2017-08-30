using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class SensitivityAnalysis
    {

        public static List<string> IdentifyXbv(double[][] optimalTable, List<string> decisionVariables)
        {
            List<string> basicVariables = new List<string>();
            for (int i = 0; i < decisionVariables.Count; i++)
            {
                if (CheckIfBasicVariable(GetColumnValues(optimalTable, i)))
                {
                    basicVariables.Add(decisionVariables[i]);
                }
            }
            return basicVariables;
        }

        private static double[] GetColumnValues(double[][] optimalTable, int columnIndex)
        {
            List<double> columnValues = new List<double>();
            for (int i = 0; i < optimalTable.Length; i++)
            {
                columnValues.Add(optimalTable[i][columnIndex]);
            }
            return columnValues.ToArray();
        }

        public static bool CheckIfBasicVariable(double[] columnValues)
        {
            if (columnValues.Where(x => x > 1 || x < 0).Count() > 0)
            {
                return false;
            }
            int numberOfOnes = (from v in columnValues
                                where v == 1
                                select v).Count();
            int numberOfZeros = (from v in columnValues
                                 where v == 0
                                 select v).Count();
            if ((numberOfOnes + numberOfZeros) == columnValues.Length)
            {
                return true;
            }
            return false;
        }

    }
}