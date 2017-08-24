using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORSProjectModels
{

    public enum Sign
    {
        Equal,
        GreaterEqual,
        LessEqual
    }

    public enum Positivity
    {
        Positive,
        Negative,
        Unrestricted
    }

    public enum OptimizationType
    {
        Max,
        Min
    }

    public enum Algorithm
    {
        PrimalSimplex,
        TwoPhaseSimplex,
        DualSimplex,
        RevisedPrimalSimplex,
        RevisedTwoPhaseSimplex,
        RevisedDualSimplex,
        BranchAndBound,
        CuttingPlane
    }

    public enum RestrictionType
    {
        Integer,
        Binary,
        Decimal
    }
}