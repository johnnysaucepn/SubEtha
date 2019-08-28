namespace Howatworks.Matrix.Domain
{
    public interface ISessionState : IState
    {
        string Build { get; set; }
        string CommanderName { get; set; }
        string GameMode { get; set; }
        string Group { get; set; }
        
    }
}