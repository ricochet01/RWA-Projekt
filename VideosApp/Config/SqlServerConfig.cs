using MessagePack.Resolvers;

namespace VideosApp.Config
{
    public class SqlServerConfig
    {
        private static IConfigurationBuilder configBuilder = 
            new ConfigurationBuilder().AddJsonFile($"appsettings.json");

        private static IConfigurationRoot config = configBuilder.Build();

        public static string GetConnectionString()
        {
            return config.GetConnectionString("DefaultConnection");
        }
    }
}
