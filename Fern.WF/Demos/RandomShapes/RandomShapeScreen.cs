using Fern.Engine.Clients;
using Fern.Engine.Graphics;
using Screen = Fern.Engine.Screens.Screen;

namespace Fern.WF.Demos.RandomShapes
{
    public class RandomShapeScreen : Screen
    {
        public RandomShapeScreen(Client client) :
            base(client)
        {
        }

        public override void Initialize()
        {
            _client.Settings.CloseOnEscKey = false;
            _client.Graphics.Clear(Pixel.Black);
        }
        public override void Update()
        {
            if (_client.Keyboard.Released(Engine.Input.Key.Escape))
            {
                _client.Settings.CloseOnEscKey = true;
                _client.Screens.Pop();
            }
        }
    }
}
