using Fern.Engine.Clients;
using Fern.Engine.Graphics;

namespace Fern.WF.Demos.RandomShapes
{
    public class RandomPixelScreen : RandomShapeScreen
    {
        public RandomPixelScreen(Client client) :
            base(client)
        {
        }

        public override void Update()
        {
            base.Update();

            var pixels = _client.Graphics.BackBuffer.Pixels;
            for (var i = 0; i < pixels.Length; ++i)
                pixels[i] = _client.Random.NextRgb();
        }
    }
}
