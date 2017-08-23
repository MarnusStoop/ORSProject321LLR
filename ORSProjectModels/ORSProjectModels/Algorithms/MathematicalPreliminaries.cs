using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public static class MathematicalPreliminaries
    {
        static Matrix<double> initialTable = new Matrix<double>();
        static Matrix<double> optimalTable = new Matrix<double>();

        public static void Calculate(Model model)
        {
            initialTable = DetermineInitialTable(model);
            optimalTable = DetermineOptimalTable(initialTable);
        }

        static Matrix<double> DetermineInitialTable(Model model)
        {
            return new Matrix<double>();
        }

        static Matrix<double> DetermineOptimalTable(Matrix<double> initialTable)
        {

            return initialTable;
        }

        static List<string> DetermineXbv()
        {

            return null;
        }

        static Matrix<double> DetermineOptimalRHS()
        {

            return null;
        }

        static double DetermineOptimalZValue()
        {

            return 0.0;
        }

        static Matrix<double> DetermineCbv()
        {

            return null;
        }

        static Matrix<double> DetermineCbvBInverse()
        {

            return null;
        }

        static Matrix<double> DetermineB()
        {

            return null;
        }

        static Matrix<double> DetermineBInverse()
        {

            return null;
        }
    }
}