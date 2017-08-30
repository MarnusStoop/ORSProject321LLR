using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class PrimalSimplex
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
                model.CanonicalForm = CanonicalGenerator.GenerateCanonicalForm(model, Algorithm.PrimalSimplex);
            }
            double[][] table = model.CanonicalForm;
            //if (GetRHSValues(table).Where(x => x < 0).Count() > 0)
            //{
            //    return AnswerGenerator.GenerateInfeasibleAnswer(InfeasiblityReason.UnsolvableWithAlgorithm);
            //}
            Console.WriteLine(model.GenerateDisplayableCanonical());
            while (CheckIfOptimal(table) == false)
            {
                //Console.WriteLine("Running");
                double[] zRow = table[0];
                int pivotColumnIndex = IdentifyPivotColumn(zRow);
                //Console.WriteLine(pivotColumnIndex);
                double[] rhsValues = GetRHSValues(table);
                double[] columnValues = GetPivotColumnValues(table, pivotColumnIndex);
                int pivotRowIndex = IdentifyPivotRow(columnValues, rhsValues);
                //Console.WriteLine(pivotRowIndex);
                table = Pivoting.PivotTable(table, pivotColumnIndex, pivotRowIndex);
                Console.WriteLine(CommonFunctions.GenerateTableIteration(model.DecisionVariables, table));
            }
            return AnswerGenerator.GenerateAnswer(table, model.DecisionVariables);
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
            if (model.OptimizationType == OptimizationType.min)
            {
                double lowestPositive = zRow.Where(x => x > 0).Max(x => x);
                pivotColumnIndex = FindColumnIndex(zRow, lowestPositive);
            } else
            {
                double highestNegative = zRow.Where(x => x < 0).Min(x => x);
                pivotColumnIndex = FindColumnIndex(zRow, highestNegative);
            }
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
            for (int i = 0; i < rhsValues.Length; i++)
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
            for (int i = 0; i < ratios.Count; i++)
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

        private static bool CheckIfOptimal(double[][] table)
        {
            if (model.OptimizationType == OptimizationType.min)
            {
                for (int i = 0; i < table[0].Length - 1; i++)
                {
                    if (table[0][i] > 0)
                    {
                        return false;
                    }
                }
            } else
            {
                for (int i = 0; i < table[0].Length - 1; i++)
                {
                    if (table[0][i] < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
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