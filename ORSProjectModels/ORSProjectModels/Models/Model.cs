using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class Model
    {
        private OptimizationType optimizationType;
        private List<string> decisionVariables;
        private double[] objectiveFunction;
        private List<Constraint> constraints;
        private List<SignRestriction> signRestrictions;
        private double[][] canonicalForm = null;

        public OptimizationType OptimizationType
        {
            get { return optimizationType; }
            set { optimizationType = value; }
        }

        public List<string> DecisionVariables
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

        public Model(OptimizationType optimizationType, List<string> decisionVariables, double[] objectiveFunction, List<Constraint> constraints, List<SignRestriction> signRestrictions)
        {
            this.OptimizationType = optimizationType;
            this.DecisionVariables = decisionVariables;
            this.ObjectiveFunction = objectiveFunction;
            this.Constraints = constraints;
            this.SignRestrictions = signRestrictions;
        }

        public string GenerateDisplayableCanonical()
        {
            string displayable = "";
            foreach (var item in decisionVariables)
            {
                displayable += item + " ";
            }
            displayable += "RHS\n";
            foreach (var item in canonicalForm)
            {
                foreach (var val in item)
                {
                    if (val < 0)
                    {
                        displayable += val + " ";
                    } else
                    {
                        displayable += val + "  ";
                    }
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