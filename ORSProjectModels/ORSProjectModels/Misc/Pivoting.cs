using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class Pivoting
    {

        public static double[][] PivotTable(double[][] table, int pivotColumnIndex, int pivotRowIndex)
        {
            double[][] newTable = table;
            double crossSectionValue = table[pivotRowIndex][pivotColumnIndex];
            for (int i = 0; i < newTable[pivotRowIndex].Length; i++)
            {
                newTable[pivotRowIndex][i] /= crossSectionValue;
            }
            for (int i = 0; i < table.Length; i++)
            {
                if (i == pivotRowIndex)
                {
                    continue;
                }
                double pivotColumnValue = table[i][pivotColumnIndex];
                for (int j = 0; j < table[i].Length; j++)
                {
                    double originalValue = table[i][j];
                    double pivotRowValue = table[pivotRowIndex][j];
                    table[i][j] = CalculateNewValue(originalValue, pivotColumnValue, pivotRowValue);
                }
            }
            return newTable;
        }

        private static double CalculateNewValue(double originalValue, double pivotColumnValue, double pivotRowValue)
        {
            return originalValue - (pivotColumnValue * pivotRowValue);
        }
    }
}