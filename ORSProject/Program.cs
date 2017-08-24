using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORSProjectModels;

namespace ORSProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Model m = GenerateSampleModel();
            DualSimplex.Solve(m);
            Console.ReadLine();
        }

        static Model GenerateSampleModel()
        {
            OptimizationType type = OptimizationType.Max;
            List<string> decVars = new List<string>(){ "x1", "x2" };
            double[] objFunc = { 100,30 };
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
            SignRestriction res1 = new SignRestriction("x1", Positivity.Positive, RestrictionType.Decimal);
            SignRestriction res2 = new SignRestriction("x2", Positivity.Positive, RestrictionType.Decimal);
            restrictions.Add(res1);
            restrictions.Add(res2);
            return restrictions;
        }

    }
}