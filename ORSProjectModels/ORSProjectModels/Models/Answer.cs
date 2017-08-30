using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{
    public class Answer
    {
        private double zValue;
        private Dictionary<string, double> answers;
        private string infeasibility;

        public double ZValue
        {
            get { return zValue; }
            set { zValue = value; }
        }

        public Dictionary<string, double> Answers
        {
            get { return answers; }
            set { answers = value; }
        }


        public string Infeasibility
        {
            get { return infeasibility; }
            set { infeasibility = value; }
        }

        public Answer()
        {

        }

        public Answer(string infeasibility)
        {
            this.Infeasibility = infeasibility;
        }

        public Answer(double zValue)
        {
            this.ZValue = zValue;
            this.Answers = new Dictionary<string, double>();
        }

        public Answer(double zValue, Dictionary<string, double> answers)
        {
            this.ZValue = zValue;
            this.Answers = answers;
        }

        public override string ToString()
        {
            string answerString = string.Format("Z = {0}", zValue);
            foreach (var item in Answers)
            {
                answerString += string.Format("\n{0} = {1}", item.Key, item.Value);
            }
            return answerString;
        }

    }
}