using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class CanonicalGenerator
    {
        private static Model model;

        public static double[][] GenerateCanonicalForm(Model _model, Algorithm algorithmToGenerateFor)
        {
            model = _model;
            switch (algorithmToGenerateFor)
            {
                case Algorithm.PrimalSimplex:
                    return GenerateSimplexCanonical(model);
                case Algorithm.TwoPhaseSimplex:
                    return GenerateTwoPhaseSimplexCanonical(model);
                case Algorithm.DualSimplex:
                    return GenerateDualSimplexCanonical(model);
                case Algorithm.RevisedPrimalSimplex:
                    return GenerateRevisedSimplexCanonical(model);
                case Algorithm.RevisedTwoPhaseSimplex:
                    return GenerateRevisedTwoPhaseSimplexCanonical(model);
                case Algorithm.RevisedDualSimplex:
                    return GenerateRevisedDualSimplexCanonical(model);
                case Algorithm.BranchAndBound:
                    return GenerateBranchAndBoundCanonical(model);
                case Algorithm.CuttingPlane:
                    return GenerateCuttingPlaneCanonical(model);
                default:
                    return null;
            }
        }

        private static double[][] GenerateSimplexCanonical(Model model)
        {
            double[][] canonical;
            int numberOfRows = 0;

            for (int i = 0; i < model.Constraints.Count; i++)
            {
                numberOfRows++;
                switch (model.Constraints[i].Sign)
                {
                    case Sign.Equal:
                        AddSlackVariable(i);
                        AddExcessVariable(i);
                        break;
                    case Sign.GreaterEqual:
                        AddExcessVariable(i);
                        break;
                    case Sign.LessEqual:
                        AddSlackVariable(i);
                        break;
                    default:
                        break;
                }
            }
            int numberOfColumns = model.DecisionVariables.Count + 1;
            canonical = new double[numberOfRows + 1][];
            //Assign objective
            canonical[0] = ConvertObjective();
            for (int i = 0; i < model.Constraints.Count; i++)
            {
                List<double> slacks = new List<double>();
                List<double> excess = new List<double>();
                foreach (var item in model.DecisionVariables)
                {
                    if (item.Contains("s"))
                    {
                        if (item == "s" + (i + 1))
                        {
                            slacks.Add(1);
                        } else
                        {
                            slacks.Add(0);
                        }
                    } else if (item.Contains("e"))
                    {
                        if (item == "e" + (i + 1))
                        {
                            excess.Add(-1);
                        } else
                        {
                            excess.Add(0);
                        }
                    }
                }
                double[] additionalVariables = slacks.Concat(excess).ToArray();
                canonical[i + 1] = model.Constraints[i].Coefficients.Concat(additionalVariables).Concat(new double[] { model.Constraints[i].RHS }).ToArray();
            }
            return canonical;
        }

        private static void AddSlackVariable(int index)
        {
            model.DecisionVariables.Add("s" + (index + 1));
        }

        private static void AddExcessVariable(int index)
        {
            model.DecisionVariables.Add("e" + (index + 1));
        }

        private static double[] ConvertObjective()
        {
            int numberOfColumns = model.Constraints.Count;
            List<double> converted = new List<double>();
            converted.AddRange(model.ObjectiveFunction);
            int numberOfSEAVariables = (from dv in model.DecisionVariables
                                        where dv.Contains("s") || dv.Contains("e") || dv.Contains("a")
                                        select dv).Count();
            for (int i = 0; i < numberOfSEAVariables; i++)
            {
                converted.Add(0);
            }
            converted.Add(0);
            for (int i = 0; i < converted.Count; i++)
            {
                converted[i] *= -1;
            }
            return converted.ToArray();
        }

        private static double[][] GenerateTwoPhaseSimplexCanonical(Model model)
        {
            double[][] canonical;
            int numberOfRows = 0;

            for (int i = 0; i < model.Constraints.Count; i++)
            {
                numberOfRows++;
                switch (model.Constraints[i].Sign)
                {
                    case Sign.Equal:
                        AddArtificalVariable(i);
                        break;
                    case Sign.GreaterEqual:
                        AddExcessVariable(i);
                        AddArtificalVariable(i);
                        break;
                    case Sign.LessEqual:
                        AddSlackVariable(i);
                        break;
                    default:
                        break;
                }
            }
            int numberOfColumns = model.DecisionVariables.Count + 1;
            canonical = new double[numberOfRows + 2][];
            //Assign objective
            canonical[0] = GenerateWRow();
            canonical[1] = ConvertObjective();
            for (int i = 0; i < model.Constraints.Count; i++)
            {
                List<double> slacks = new List<double>();
                List<double> excess = new List<double>();
                List<double> artifical = new List<double>();
                foreach (var item in model.DecisionVariables)
                {
                    if (item.Contains("s"))
                    {
                        if (item == "s" + (i + 1))
                        {
                            slacks.Add(1);
                        } else
                        {
                            slacks.Add(0);
                        }
                    } else if (item.Contains("e"))
                    {
                        if (item == "e" + (i + 1))
                        {
                            excess.Add(-1);
                        } else
                        {
                            excess.Add(0);
                        }
                    } else if (item.Contains("a"))
                    {
                        if (item == "a" + (i + 1))
                        {
                            artifical.Add(1);
                        } else
                        {
                            artifical.Add(0);
                        }
                    }
                }
                double[] additionalVariables = slacks.Concat(excess).Concat(artifical).ToArray();
                canonical[i + 2] = model.Constraints[i].Coefficients.Concat(additionalVariables).Concat(new double[] { model.Constraints[i].RHS }).ToArray();
            }

            return canonical;
        }

        private static double[] GenerateWRow()
        {
            List<double> converted = new List<double>();
            List<int> artificalConstraintIndices = FindAritificalConstraintIndices(model.Constraints);
            List<int> decisionVariableIndices = FindDecisionVariableIndices(model.DecisionVariables);
            List<int> slackVariableIndices = FindSlackVariableIndices(model.DecisionVariables);
            List<int> excessVariableIndices = FindExcessVariableIndices(model.DecisionVariables);
            List<int> artificalVariableIndices = FindArtificialVariableIndices(model.DecisionVariables);
            foreach (var item in decisionVariableIndices)
            {
                double value = GetDecisionVariableValue(model.Constraints, item, artificalConstraintIndices);
                converted.Add(value);
            }
            int numberOfAdditionalVariables = slackVariableIndices.Count + excessVariableIndices.Count + artificalVariableIndices.Count;
            for (int i = 0; i < numberOfAdditionalVariables; i++)
            {
                converted.Add(0);
            }
            foreach (var item in slackVariableIndices)
            {
                converted[item] = 0;
            }
            foreach (var item in excessVariableIndices)
            {
                converted[item] = -1;
            }
            foreach (var item in artificalVariableIndices)
            {
                converted[item] = 0;
            }
            double rhsValue = GetRhsValue(model.Constraints, artificalConstraintIndices);
            converted.Add(rhsValue);
            return converted.ToArray();
        }

        private static double GetDecisionVariableValue(List<Constraint> constraints, int decisionVariableIndex, List<int> artificalConstraintIndices)
        {
            double value = 0;
            for (int i = 0; i < artificalConstraintIndices.Count; i++)
            {
                value += constraints[i].Coefficients[decisionVariableIndex];
            }
            return value;
        }

        private static double GetRhsValue(List<Constraint> constraints, List<int> artificalConstraintIndices)
        {
            double value = 0;
            for (int i = 0; i < artificalConstraintIndices.Count; i++)
            {
                value += constraints[i].RHS;
            }
            return value;
        }

        private static List<int> FindDecisionVariableIndices(List<string> decisionVariables)
        {
            List<int> decisionVariableIndices = new List<int>();
            for (int i = 0; i < decisionVariables.Count; i++)
            {
                if (decisionVariables[i].Contains('x'))
                {
                    decisionVariableIndices.Add(i);
                }
            }
            return decisionVariableIndices;
        }

        private static List<int> FindAritificalConstraintIndices(List<Constraint> constraints)
        {
            List<int> artificalConstrainIndices = new List<int>();
            for (int i = 0; i < constraints.Count; i++)
            {
                if (constraints[i].Sign == Sign.LessEqual)
                {
                    continue;
                }
                artificalConstrainIndices.Add(i);
            }
            return artificalConstrainIndices;
        }

        private static List<int> FindSlackVariableIndices(List<string> decisionVariables)
        {
            List<int> slackVariableIndices = new List<int>();
            for (int i = 0; i < decisionVariables.Count; i++)
            {
                if (decisionVariables[i].Contains('s'))
                {
                    slackVariableIndices.Add(i);
                }
            }
            return slackVariableIndices;
        }

        private static List<int> FindExcessVariableIndices(List<string> decisionVariables)
        {
            List<int> excessVariableIndices = new List<int>();
            for (int i = 0; i < decisionVariables.Count; i++)
            {
                if (decisionVariables[i].Contains('e'))
                {
                    excessVariableIndices.Add(i);
                }
            }
            return excessVariableIndices;
        }

        private static List<int> FindArtificialVariableIndices(List<string> decisionVariables)
        {
            List<int> artificalVariableIndices = new List<int>();
            for (int i = 0; i < decisionVariables.Count; i++)
            {
                if (decisionVariables[i].Contains('a'))
                {
                    artificalVariableIndices.Add(i);
                }
            }
            return artificalVariableIndices;
        }

        private static void AddArtificalVariable(int index)
        {
            model.DecisionVariables.Add("a" + (index + 1));
        }

        public static double[][] GenerateSecondPhaseForTwoPhaseCanonical(Model _model, double[][] endOfPhase1Table, out Model modelForSimplex)
        {
            model = _model;
            double[][] canonical = endOfPhase1Table;
            List<int> decisionVariableIndices = FindDecisionVariableIndices(_model.DecisionVariables);
            List<int> slackVariableIndices = FindSlackVariableIndices(_model.DecisionVariables);
            List<int> excessVariableIndices = FindExcessVariableIndices(_model.DecisionVariables);
            List<int> artificalVariableIndices = FindArtificialVariableIndices(_model.DecisionVariables);
            List<int> basicArtificalVariableIndices = FindBasicArtificialVariableIndices(endOfPhase1Table, artificalVariableIndices);
            for (int i = 0; i < artificalVariableIndices.Count; i++)
            {
                if (!basicArtificalVariableIndices.Contains(artificalVariableIndices[i]))
                {
                    model.DecisionVariables.RemoveAt(i);
                }
            }
            int numberOfColumns = (decisionVariableIndices.Count + slackVariableIndices.Count + excessVariableIndices.Count + basicArtificalVariableIndices.Count) + 1;
            double[][] newCanonical = new double[canonical.Length - 1][];
            List<int> indicesToCopy = (decisionVariableIndices.Concat(slackVariableIndices).Concat(excessVariableIndices).Concat(basicArtificalVariableIndices)).ToList();
            for (int i = 1; i < canonical.Length; i++)
            {
                newCanonical[i - 1] = CopyValues(canonical, i, indicesToCopy);
            }
            modelForSimplex = model;
            return newCanonical;
        }

        private static List<int> FindBasicArtificialVariableIndices(double[][] table, List<int> artificalVariableIndices)
        {
            List<int> indices = new List<int>();
            foreach (var item in artificalVariableIndices)
            {
                double[] values = GetColumnValues(table, item);
                if (SensitivityAnalysis.CheckIfBasicVariable(values))
                {
                    indices.Add(item);
                }
            }
            return indices;
        }

        private static double[] CopyValues(double[][] canonical, int currentRow, List<int> indicesToCopy)
        {
            //List<double> values = new List<double>();
            double[] valuesA = new double[indicesToCopy.Count];
            int rhsIndex = canonical[0].Length - 1;
            for (int j = 0; j < indicesToCopy.Count; j++)
            {
                double copiedValue = canonical[currentRow][indicesToCopy[j]];
                valuesA[indicesToCopy[j]] = copiedValue;
            }
            double rhsValue = canonical[currentRow][rhsIndex];
            valuesA[valuesA.Length - 1] = rhsValue;
            return valuesA;
        }

        private static double[] GetColumnValues(double[][] table, int columnIndex)
        {
            List<double> values = new List<double>();
            for (int i = 0; i < table.Length; i++)
            {
                values.Add(table[i][columnIndex]);
            }
            return values.ToArray();
        }

        private static double[][] GenerateDualSimplexCanonical(Model model)
        {
            double[][] canonical;
            int numberOfRows = 0;

            for (int i = 0; i < model.Constraints.Count; i++)
            {
                numberOfRows++;
                switch (model.Constraints[i].Sign)
                {
                    case Sign.Equal:
                        AddSlackVariable(i);
                        AddExcessVariable(i);
                        break;
                    case Sign.GreaterEqual:
                        AddExcessVariable(i);
                        break;
                    case Sign.LessEqual:
                        AddSlackVariable(i);
                        break;
                    default:
                        break;
                }
            }
            int numberOfColumns = model.DecisionVariables.Count + 1;
            canonical = new double[numberOfRows + 1][];
            canonical[0] = ConvertObjective();
            for (int i = 0; i < model.Constraints.Count; i++)
            {
                List<double> additional = new List<double>();
                foreach (var item in model.DecisionVariables)
                {
                    if (item.Contains("s"))
                    {
                        if (item == "s" + (i + 1))
                        {
                            additional.Add(1);
                        } else
                        {
                            additional.Add(0);
                        }
                    } else if (item.Contains("e"))
                    {
                        if (item == "e" + (i + 1))
                        {
                            additional.Add(-1);
                        } else
                        {
                            additional.Add(0);
                        }
                    }
                }
                canonical[i + 1] = model.Constraints[i].Coefficients.Concat(additional.ToArray()).Concat(new double[] { model.Constraints[i].RHS }).ToArray();
                if (isExcessRow(canonical[i + 1]))
                {
                    for (int j = 0; j < canonical[i + 1].Length; j++)
                    {
                        canonical[i + 1][j] *= -1;
                    }
                }
            }
            return canonical;
        }

        private static bool isExcessRow(double[] values)
        {
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (model.DecisionVariables[i].Contains("e"))
                {
                    if (values[i] == -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static double[][] GenerateRevisedSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private static double[][] GenerateRevisedTwoPhaseSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private static double[][] GenerateRevisedDualSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private static double[][] GenerateBranchAndBoundCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private static double[][] GenerateCuttingPlaneCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }
    }
}