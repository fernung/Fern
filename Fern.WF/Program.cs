using Fern.Engine.Clients;
using Fern.Engine.Screens;
using Fern.WF.Demos.RandomShapes;

namespace Fern.WF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var settings = new ClientSettings(800, 600, "Fern Client")
            {
                AllowMinimize = false,
                AllowMaximize = true,
                CloseOnEscKey = false
            };
            using var client = new Client(settings);
            client.Run(new MainMenuScreen(client));
        }
    }
}