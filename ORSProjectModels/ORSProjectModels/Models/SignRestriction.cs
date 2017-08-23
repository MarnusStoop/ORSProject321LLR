using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class SignRestriction
    {
        private string decisionVariable;
        private Positivity positivity;
        private RestrictionType restrictionType;

        public RestrictionType RestrictionType
        {
            get { return restrictionType; }
            set { restrictionType = value; }
        }

        public string DecisionVariable
        {
            get { return decisionVariable; }
            set { decisionVariable = value; }
        }

        public Positivity Positivity
        {
            get { return positivity; }
            set { positivity = value; }
        }

        public SignRestriction()
        {

        }

        public SignRestriction(string decisionVariable, Positivity positivity, RestrictionType restrictionType)
        {
            this.DecisionVariable = decisionVariable;
            this.Positivity = positivity;
            this.RestrictionType = restrictionType;
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