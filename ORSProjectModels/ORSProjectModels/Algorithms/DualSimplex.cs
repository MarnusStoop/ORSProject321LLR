using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class DualSimplex
    {

        private static Model model;

        public static double[][] Solve(Model _model)
        {
            if (_model == null)
            {
                return null;
            }
            model = _model;
            if (model.CanonicalForm == null)
            {
                model.CanonicalForm = CanonicalGenerator.GenerateCanonicalForm(model, Algorithm.DualSimplex);
            }
            double[][] table = model.CanonicalForm;
            Console.WriteLine(model.GenerateDisplayableCanonical());
            while (CheckIfDoneWithDualPhase(table) == false)
            {
                //Console.WriteLine("Running");
                double[] rhsValues = GetRHSValues(table);
                int pivotRowIndex = IdentifyPivotRow(rhsValues);
                double[] zRow = table[0];
                double[] pivotRowValues = GetPivotRowValues(table, pivotRowIndex);
                if (CheckIfInfeasible(pivotRowValues))
                {
                    return null;
                }
                int pivotColumnIndex = IdentifyPivotColumn(zRow, pivotRowValues);
                //Console.WriteLine(pivotColumnIndex);
                //Console.WriteLine(pivotRowIndex);
                table = Pivoting.PivotTable(table, pivotColumnIndex, pivotRowIndex);
                Console.WriteLine(CommonFunctions.GenerateTableIteration(model.DecisionVariables, table));
            }
            Model modelForSimplex = model;
            model.CanonicalForm = table;
            return PrimalSimplex.Solve(modelForSimplex);
        }

        private static double[] GetRHSValues(double[][] table)
        {
            List<double> rhs = new List<double>();
            int rhsColumnIndex = table[0].Length - 1;
            for (int i = 0; i < table.Length; i++)
            {
                rhs.Add(table[i][rhsColumnIndex]);
            }
            return rhs.ToArray();
        }

        private static double[] GetPivotColumnValues(double[][] table, int columnIndex)
        {
            List<double> column = new List<double>();
            for (int i = 0; i < table.Length; i++)
            {
                double value = table[i][columnIndex];
                column.Add(value);
            }
            return column.ToArray();
        }

        private static int IdentifyPivotColumn(double[] zRow, double[] pivotRowValues)
        {
            int pivotColumnIndex = 0;
            List<double> ratios = new List<double>();
            for (int i = 0; i < zRow.Length; i++)
            {
                double ratio = CalculateRatio(pivotRowValues[i], zRow[i]);
                ratio = Util.AbsoluteValue(ratio);
                if (pivotRowValues[i] > 0)
                {
                    ratio *= -1;
                }
                ratios.Add(ratio);
            }
            double smallestPositiveRatio = ratios.Where(x => x > 0).Min(x => x);
            pivotColumnIndex = FindColumnIndex(ratios, smallestPositiveRatio);
            return pivotColumnIndex;
        }

        private static double[] GetPivotRowValues(double[][] table, int pivotRowIndex)
        {
            return table[pivotRowIndex];
        }

        private static int FindColumnIndex(List<double> ratios, double number)
        {
            for (int i = 0; i < ratios.Count; i++)
            {
                if (ratios[i] == number)
                {
                    return i;
                }
            }
            return -1;
        }

        private static int IdentifyPivotRow(double[] rhsValues)
        {
            double largestNegativeRowValue = rhsValues.Where(x => x < 0).Min(x => x);
            int rowIndex = FindRowIndex(rhsValues, largestNegativeRowValue);
            return rowIndex;
        }

        private static int FindRowIndex(double[] rhsValues, double number)
        {
            for (int i = 0; i < rhsValues.Length; i++)
            {
                if (rhsValues[i] == number)
                {
                    return i;
                }
            }
            return -1;
        }

        private static double CalculateRatio(double columnValue, double zValue)
        {
            return zValue / columnValue;
        }

        private static bool CheckIfDoneWithDualPhase(double[][] table)
        {
            double[] rhsValues = GetRHSValues(table);
            for (int i = 0; i < rhsValues.Length; i++)
            {
                if (rhsValues[i] < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckIfInfeasible(double[] pivotRow)
        {
            //-1 to ignore rhs value
            for (int i = 0; i < pivotRow.Length - 1; i++)
            {
                if (pivotRow[i] < 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}