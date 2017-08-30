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

    public enum OptimizationType
    {
        max,
        min
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
        Positive,
        Negative,
        Unrestricted
    }

    public enum InfeasiblityReason
    {
        UnsolvableWithAlgorithm,
        GeneralUnsolvability
    }

    public enum MainMenuOption
    {
        SolvePrimal = 1,
        SolveTwoPhase,
        SolveDual,
        SolveRevisedPrimal,
        SolveRevisedTwoPhase,
        SolveRevisedDual,
        SolveBranchAndBound,
        SolveCuttingPlane,
        LoadModel,
        Exit
    }

    public enum SensitivityAnalysisOption
    {

    }
}