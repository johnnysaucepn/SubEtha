namespace SubEtha.Domain
{
    public interface IShipState : IState
    {
        int ShipID { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        string Ident { get; set; }
        bool? ShieldsUp { get; set; }
        decimal? HullIntegrity { get; set; }
    }
}