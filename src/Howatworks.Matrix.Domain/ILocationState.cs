namespace Howatworks.Matrix.Domain
{
    public interface ILocationState : IState
    {
        Body Body { get; set; }
        SignalSource SignalSource { get; set; }
        StarSystem StarSystem { get; set; }
        Station Station { get; set; }
        SurfaceLocation SurfaceLocation { get; set; }
        
    }
}