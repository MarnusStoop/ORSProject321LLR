using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class RevisedSimplex
    {

        public bool valid
        {
            get;
            protected set;
        }

        private Matrix B;
        private Matrix I;
        private Matrix C;
        private Matrix Y;
        private Matrix P;

        public RevisedSimplex(double[] zValues, List<double[]> constraints)
        {
            valid = true;
            foreach (var item in constraints)
            {
                if (zValues.Length != (item.Length - constraints.Count - 1))
                {
                    valid = false;
                    break;
                }
                if (valid)
                {
                    I = new Matrix(constraints.Count);
                    B = I;
                }
            }
        }

        public double[] Solve()
        {
            try
            {
                if (!valid)
                    throw new Exception("Varibles does not match");

            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new double[] { 0 };
        }

    }
}