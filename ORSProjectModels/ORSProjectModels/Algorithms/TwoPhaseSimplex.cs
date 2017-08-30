using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class TwoPhaseSimplex
    {

        private static Model model;

        public static Answer Solve(Model _model)
        {
            if (_model == null)
            {
                return null;
            }
            model = _model;
            if (model.CanonicalForm == null)
            {
                model.CanonicalForm = CanonicalGenerator.GenerateCanonicalForm(model, Algorithm.TwoPhaseSimplex);
            }
            double[][] table = model.CanonicalForm;
            Console.WriteLine(model.GenerateDisplayableCanonical());
            while (CheckIfDoneWithFirstPhase(table) == false)
            {
                //Console.WriteLine("Running");
                double[] zRow = table[0];
                int pivotColumnIndex = IdentifyPivotColumn(zRow);
                Console.WriteLine(pivotColumnIndex);
                double[] rhsValues = GetRHSValues(table);
                double[] columnValues = GetPivotColumnValues(table, pivotColumnIndex);
                int pivotRowIndex = IdentifyPivotRow(columnValues, rhsValues);
                Console.WriteLine(pivotRowIndex);
                table = Pivoting.PivotTable(table, pivotColumnIndex, pivotRowIndex);
                Console.WriteLine(CommonFunctions.GenerateTableIteration(model.DecisionVariables, table));
            }
            Model modelForSimplex;
            double[][] newCanonical = CanonicalGenerator.GenerateSecondPhaseForTwoPhaseCanonical(model, table, out modelForSimplex);
            modelForSimplex.CanonicalForm = newCanonical;
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

        private static int IdentifyPivotColumn(double[] zRow)
        {
            int pivotColumnIndex = 0;
            double[] values = new double[zRow.Length - 1];
            for (int i = 0; i < zRow.Length - 1; i++)
            {
                values[i] = zRow[i];
            }
            double largestPositive = values.Where(x => x > 0).Max(x => x);
            pivotColumnIndex = FindColumnIndex(values, largestPositive);
            return pivotColumnIndex;
        }

        private static int FindColumnIndex(double[] row, double number)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == number)
                {
                    return i;
                }
            }
            return -1;
        }

        private static int IdentifyPivotRow(double[] pivotColumn, double[] rhsValues)
        {
            List<double> ratios = new List<double>();
            ratios.Add(-1);
            ratios.Add(-1);
            for (int i = 2; i < rhsValues.Length; i++)
            {
                double ratio = CalculateRatio(pivotColumn[i], rhsValues[i]);
                ratios.Add(ratio);
            }
            double lowestRatio = ratios.Where(x => x > 0).Min(x => x);
            int rowIndex = FindRowIndex(ratios, lowestRatio);
            return rowIndex;
        }

        private static int FindRowIndex(List<double> ratios, double number)
        {
            for (int i = 2; i < ratios.Count; i++)
            {
                if (ratios[i] == number)
                {
                    return i;
                }
            }
            return -1;
        }

        private static double CalculateRatio(double columnValue, double rhsValue)
        {
            return rhsValue / columnValue;
        }

        private static bool CheckIfDoneWithFirstPhase(double[][] table)
        {
            int rhsColumnIndex = table[0].Length - 1;
            if (table[0][rhsColumnIndex] == 0)
            {
                return true;
            }
            return false;
        }

        private static bool CheckIfInfeasible(double[][] table)
        {
            //to be done
            if (model.OptimizationType == OptimizationType.max)
            {

            } else
            {

            }
            return false;
        }

    }
}