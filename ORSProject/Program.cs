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
            Console.WriteLine(m.GenerateDisplayableCanonical());
            Console.ReadLine();
        }

        static Model GenerateSampleModel()
        {
            OptimizationType type = OptimizationType.Max;
            string[] decVars = { "x1", "x2", "x3" };
            double[] objFunc = { 4, 5, -3 };
            List<Constraint> constraints = GenerateSampleConstraints();
            List<SignRestriction> restrictions = GenerateSampleSignRestrictions();
            return new Model(type, decVars, objFunc, constraints, restrictions);
        }

        static List<Constraint> GenerateSampleConstraints()
        {
            Constraint con1 = new Constraint(new double[] { 2, 1, 4 }, Sign.LessEqual, 5);
            Constraint con2 = new Constraint(new double[] { -1, 2, 2 }, Sign.GreaterEqual, -6);
            Constraint con3 = new Constraint(new double[] { 3, 0, -1 }, Sign.Equal, 2);
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
            SignRestriction res2 = new SignRestriction("x2", Positivity.Negative, RestrictionType.Decimal);
            SignRestriction res3 = new SignRestriction("x3", Positivity.Unrestricted, RestrictionType.Decimal);
            restrictions.Add(res1);
            restrictions.Add(res2);
            restrictions.Add(res3);
            return restrictions;
        }

    }
}