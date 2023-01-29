using Fern.Engine.Clients;
using Fern.Engine.Screens;

namespace Fern.WF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            using var client = new Client();
            client.Run(new EmptyScreen(client));
        }
    }
}