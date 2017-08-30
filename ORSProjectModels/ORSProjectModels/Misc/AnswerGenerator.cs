using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class AnswerGenerator
    {

        public static Answer GenerateAnswer(double[][] optimalTable, List<string> decisionVariables)
        {
            double zValue = optimalTable[0][optimalTable[0].Length - 1];
            List<string> xbv = SensitivityAnalysis.IdentifyXbv(optimalTable, decisionVariables);
            List<int> basicVariableColumnIndices = GetBasicVariableColumnIndices(xbv, decisionVariables);
            List<int> basicVariableRowIndices = GetBasicVariableRowIndices(optimalTable, basicVariableColumnIndices);
            double[] rhsValues = GetRHSValues(optimalTable);
            Dictionary<string, double> values = GetBasicVariableValues(rhsValues, basicVariableRowIndices, xbv);
            return new Answer(zValue, values);
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

        private static List<int> GetBasicVariableColumnIndices(List<string> xbv, List<string> decisionVariables)
        {
            List<int> indices = new List<int>();
            foreach (var item in xbv)
            {
                int index = decisionVariables.IndexOf(item);
                indices.Add(index);
            }
            return indices;
        }
        private static List<int> GetBasicVariableRowIndices(double[][] optimal, List<int> basicVariableColumnIndices)
        {
            List<int> indices = new List<int>();
            foreach (var columnIndex in basicVariableColumnIndices)
            {
                for (int row = 0; row < optimal.Length; row++)
                {
                    if (optimal[row][columnIndex] == 1)
                    {
                        indices.Add(row);
                    }
                }
            }
            return indices;
        }

        private static Dictionary<string, double> GetBasicVariableValues(double[] rhsValues, List<int> basicVariableRowIndices, List<string> xbv)
        {
            Dictionary<string, double> values = new Dictionary<string, double>();

            for (int i = 0; i < xbv.Count; i++)
            {
                string variableName = xbv[i];
                double variableValue = rhsValues[basicVariableRowIndices[i]];
                values.Add(variableName, variableValue);
            }
            return values;
        }

        public static Answer GenerateInfeasibleAnswer(InfeasiblityReason reason)
        {
            if (reason == InfeasiblityReason.GeneralUnsolvability)
            {
                return new Answer("The model has no feasible solution");
            } else if (reason == InfeasiblityReason.UnsolvableWithAlgorithm)
            {
                return new Answer("The model cannot be solved using this algorithm");
            }
            return new Answer("The model is unsolvable");
        }

    }
}