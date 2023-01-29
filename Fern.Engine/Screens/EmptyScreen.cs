using Fern.Engine.Clients;
using Fern.Engine.Graphics;

namespace Fern.Engine.Screens
{
    public class EmptyScreen : Screen
    {
        public EmptyScreen(Client client)
            : base(client) 
        { }

        public override void Initialize() { }
        public override void LoadContent() { }
        public override void UnloadContent() { }
        public override void Update() { }
        public override void Draw() { _client.Graphics.Clear(Pixel.Black); }
    }
}
