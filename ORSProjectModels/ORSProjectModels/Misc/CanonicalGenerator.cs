using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class CanonicalGenerator
    {
        private static CanonicalGenerator instance;
        private Model model;

        private CanonicalGenerator()
        {

        }

        public static CanonicalGenerator GetInstance()
        {
            if (instance == null)
            {
                instance = new CanonicalGenerator();
            }
            return instance;
        }

        public double[][] GenerateCanonicalForm(Model model, Algorithm algorithmToGenerateFor)
        {
            this.model = model;
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

        private double[][] GenerateSimplexCanonical(Model model)
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

        private void AddSlackVariable(int index)
        {
            model.DecisionVariables.Add("s" + (index + 1));
        }

        private void AddExcessVariable(int index)
        {
            model.DecisionVariables.Add("e" + (index + 1));
        }

        private double[] ConvertObjective()
        {
            int numberOfColumns = model.Constraints.Count;
            List<double> converted = new List<double>();
            converted.AddRange(model.ObjectiveFunction);
            int numberOfSEVariables = (from dv in model.DecisionVariables
                                       where dv.Contains("s") || dv.Contains("e")
                                       select dv).Count();
            for (int i = 0; i < numberOfSEVariables; i++)
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

        private double[][] GenerateTwoPhaseSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private double[][] GenerateDualSimplexCanonical(Model model)
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

            return canonical;
        }

        private double[][] GenerateRevisedSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private double[][] GenerateRevisedTwoPhaseSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private double[][] GenerateRevisedDualSimplexCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private double[][] GenerateBranchAndBoundCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }

        private double[][] GenerateCuttingPlaneCanonical(Model model)
        {
            double[][] canonical = new double[2][];

            return canonical;
        }
    }
}