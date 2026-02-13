namespace Cocodrilo.ElementProperties
{
    /// <summary>
    /// Defines the available options for variuous integration
    /// possibilities between different geometries.
    /// </summary>
    public enum GeometryType
    {
        GeometrySurface,
        SurfaceEdge,
        SurfacePoint,
        GeometryCurve,
        CurveEdge,
        CurvePoint,
        Point,
        SurfaceEdgeSurfaceEdge,
        SurfaceEdgeCurveEdge,
        SurfacePointSurfacePoint,
        SurfacePointCurvePoint,
        CurveEdgeCurveEdge,
        CurvePointCurvePoint,
        ErrorType,
        Mesh
    }

    public enum CheckType
    {
        ConvergenceCheck,
        KLStressCheck
    }

    public enum CouplingType
    {
        CouplingPenaltyCondition,
        CouplingLagrangeCondition,
        CouplingNitscheCondition
    }

    public enum SupportType
    {
        SupportPenaltyCondition,
        SupportLagrangeCondition,
        SupportNitscheCondition
    }
}
