using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORSProjectModels;
using System.Windows.Forms;

namespace ORSProject
{
    class Program
    {

        static OpenFileDialog ofd = new OpenFileDialog();

        [STAThread]
        static void Main(string[] args)
        {
            ofd.Filter = "Text Files|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<string> data = FileHandler.Read(ofd.FileName);
                Model mm = GenerateModelFromFile(data);
                PrimalSimplex.Solve(mm);
            }
            //Model m = GenerateSampleModel();
            //DualSimplex.Solve(m);
            Console.ReadLine();
        }

        #region Display

        private static void DisplayMainMenu()
        {

        }

        #endregion

        #region File input

        static Model GenerateModelFromFile(List<string> data)
        {
            List<Constraint> constraints = new List<Constraint>();
            List<SignRestriction> signRestrictions = new List<SignRestriction>();
            string objectiveFunctionLine = data[0];
            OptimizationType type = DetermineOptimizationType(objectiveFunctionLine);
            List<string> decisionVariables = GenerateDecisionVariables(objectiveFunctionLine);
            double[] objFunc = GenerateObjectiveFunction(objectiveFunctionLine);
            string signRestrictionsLine = data[data.Count - 1];
            GenerateObjectiveFunction(objectiveFunctionLine);
            // we start at line 2 and end at second last line
            for (int i = 1; i < data.Count - 1; i++)
            {
                string constraintLine = data[i];
                Constraint con = GenerateConstraint(constraintLine);
                if (con == null)
                {
                    return null;
                }
                constraints.Add(con);
            }
            signRestrictions = GenerateSignRestrictions(signRestrictionsLine);
            return new Model(type, decisionVariables, objFunc, constraints, signRestrictions);
        }

        static OptimizationType DetermineOptimizationType(string line)
        {
            OptimizationType type;
            string[] split = line.Split(' ');
            if (!Enum.TryParse<OptimizationType>(split[0], out type))
            {
                Console.WriteLine("Input file is malformed, optimization type error");
                return OptimizationType.max;
            }
            return type;
        }

        static List<string> GenerateDecisionVariables(string line)
        {
            List<string> decisionVariables = new List<string>();
            string[] split = line.Split(' ');
            //Starting at 1 to skip optimization type
            for (int i = 1; i < split.Length; i++)
            {
                string decVar = "x" + i;
                decisionVariables.Add(decVar);
            }
            return decisionVariables;
        }

        static double[] GenerateObjectiveFunction(string line)
        {
            List<double> zValues = new List<double>();
            string[] split = line.Split(' ');
            //Starting at 1 to skip optimization type
            for (int i = 1; i < split.Length; i++)
            {
                double zValue;
                if (!double.TryParse(split[i], out zValue))
                {
                    Console.WriteLine("Input file is malformed,z value error");
                    return null;
                }
                zValues.Add(zValue);
            }
            return zValues.ToArray();
        }

        static Constraint GenerateConstraint(string line)
        {
            List<double> coeffecients = new List<double>();
            Sign sign = Sign.Equal;
            double rhs = 0;
            string[] split = line.Split(' ');
            //end early to skip rhs
            for (int i = 0; i < split.Length - 1; i++)
            {
                if (split[i].Contains("="))
                {
                    sign = DetermineSign(split[i]);
                } else
                {
                    double coeff;
                    if (!double.TryParse(split[i], out coeff))
                    {
                        Console.WriteLine("Input file is malformed,coefficient error");
                        return null;
                    }
                    coeffecients.Add(coeff);
                }
            }
            if (!double.TryParse(split[split.Length - 1], out rhs))
            {
                Console.WriteLine("Input file is malformed,rhs error");
                return null;
            }
            Constraint con = new Constraint(coeffecients.ToArray(), sign, rhs);
            return con;
        }

        static Sign DetermineSign(string data)
        {
            if (data == "<=")
            {
                return Sign.LessEqual;
            } else if (data == "=")
            {
                return Sign.Equal;
            } else if (data == ">=")
            {
                return Sign.GreaterEqual;
            }
            return Sign.Equal;
        }

        static List<SignRestriction> GenerateSignRestrictions(string line)
        {
            List<SignRestriction> restrictions = new List<SignRestriction>();
            string[] data = line.Split(' ');
            for (int i = 0; i < data.Length; i++)
            {
                RestrictionType restrictionType = DetermineRestrictionType(data[i]);
                string decVar = "x" + (i + 1);
                SignRestriction sr = new SignRestriction(decVar, restrictionType);
                restrictions.Add(sr);
            }
            return restrictions;
        }

        static RestrictionType DetermineRestrictionType(string data)
        {
            if (data == "+")
            {
                return RestrictionType.Positive;
            } else if (data == "-")
            {
                return RestrictionType.Negative;
            } else if (data.ToLower() == "urs")
            {
                return RestrictionType.Unrestricted;
            } else if (data.ToLower() == "bin")
            {
                return RestrictionType.Binary;
            } else if (data.ToLower() == "int")
            {
                return RestrictionType.Integer;
            }
            return RestrictionType.Integer;
        }

        #endregion

        #region Sample Models
        static Model GenerateSampleModel()
        {
            OptimizationType type = OptimizationType.max;
            List<string> decVars = new List<string>() { "x1", "x2" };
            double[] objFunc = { 100, 30 };
            List<Constraint> constraints = GenerateSampleConstraints();
            List<SignRestriction> restrictions = GenerateSampleSignRestrictions();
            return new Model(type, decVars, objFunc, constraints, restrictions);
        }

        static List<Constraint> GenerateSampleConstraints()
        {
            Constraint con1 = new Constraint(new double[] { 0, 1 }, Sign.GreaterEqual, 3);
            Constraint con2 = new Constraint(new double[] { 1, 1 }, Sign.LessEqual, 7);
            Constraint con3 = new Constraint(new double[] { 10, 4 }, Sign.LessEqual, 40);
            List<Constraint> constraints = new List<Constraint>();
            constraints.Add(con1);
            constraints.Add(con2);
            constraints.Add(con3);
            return constraints;
        }

        static List<SignRestriction> GenerateSampleSignRestrictions()
        {
            List<SignRestriction> restrictions = new List<SignRestriction>();
            SignRestriction res1 = new SignRestriction("x1", RestrictionType.Positive);
            SignRestriction res2 = new SignRestriction("x2", RestrictionType.Positive);
            restrictions.Add(res1);
            restrictions.Add(res2);
            return restrictions;
        }
        #endregion
    }
}