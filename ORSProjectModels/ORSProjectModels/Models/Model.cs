using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class Model
    {
        private OptimizationType optimizationType;
        private string[] decisionVariables;
        private double[] objectiveFunction;
        private List<Constraint> constraints;
        private List<SignRestriction> signRestrictions;
        private double[][] canonicalForm;

        private int numberOfConstraints = 0;

        public OptimizationType OptimizationType
        {
            get { return optimizationType; }
            set { optimizationType = value; }
        }

        public string[] DecisionVariables
        {
            get { return decisionVariables; }
            set { decisionVariables = value; }
        }

        public double[] ObjectiveFunction
        {
            get { return objectiveFunction; }
            set { objectiveFunction = value; }
        }

        public List<Constraint> Constraints
        {
            get { return constraints; }
            set { constraints = value; }
        }

        public List<SignRestriction> SignRestrictions
        {
            get { return signRestrictions; }
            set { signRestrictions = value; }
        }

        public double[][] CanonicalForm
        {
            get { return canonicalForm; }
            set { canonicalForm = value; }
        }

        public Model()
        {

        }

        public Model(OptimizationType optimizationType, string[] decisionVariables, double[] objectiveFunction, List<Constraint> constraints, List<SignRestriction> signRestrictions)
        {
            this.OptimizationType = optimizationType;
            this.DecisionVariables = decisionVariables;
            this.ObjectiveFunction = objectiveFunction;
            this.Constraints = constraints;
            this.SignRestrictions = signRestrictions;
            GenerateCanonicalForm();
        }

        private void GenerateCanonicalForm()
        {
            int numberOfRows = 0;
            //Amount of decision variables plus 1 for rhs
            //int numberOfColumns = decisionVariables.Length + 1;
            foreach (var item in constraints)
            {
                numberOfRows++;
                //numberOfColumns++;
                if (item.Sign == Sign.Equal)
                {
                    numberOfRows++;
                    //numberOfColumns++;
                }
            }
            canonicalForm = new double[numberOfRows][];
            //Assign objective
            canonicalForm[0] = ConvertObjective();
            canonicalForm[1] = ConvertObjective();
            canonicalForm[2] = ConvertObjective();
            canonicalForm[3] = ConvertObjective();
        }

        private double[] ConvertObjective()
        {
            double[] converted = objectiveFunction;
            for (int i = 0; i < converted.Length; i++)
            {
                converted[i] *= -1;
            }
            return converted;
        }

        private void GenerateInitialTable()
        {

        }

        public string GenerateDisplayableCanonical()
        {
            string displayable = "";
            foreach (var item in canonicalForm)
            {
                foreach (var val in item)
                {
                    displayable += val + " ";
                }
                displayable += "\n";
            }
            return displayable;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}