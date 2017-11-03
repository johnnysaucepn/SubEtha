namespace Howatworks.Configuration
{
    public interface IConfigLoader
    {
        IConfigReader GetConfigurationSection(params string[] parts);
    }
}