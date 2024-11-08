using System.Numerics;
public interface ISurface
{
    float FactorNoise { get; }
    SurfaceType SurfaceType { get; }
}