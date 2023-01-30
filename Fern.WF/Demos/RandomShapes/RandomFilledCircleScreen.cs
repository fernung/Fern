using Fern.Engine.Clients;
using Fern.Engine.Graphics;

namespace Fern.WF.Demos.RandomShapes
{
    public class RandomFilledCircleScreen : RandomShapeScreen
    {
        protected int x0, y0;
        protected int radius;
        protected Pixel color;

        public RandomFilledCircleScreen(Client client) :
            base(client)
        {
        }

        public override void Update()
        {
            base.Update();

            x0 = _client.Random.Next(0, _client.Width);
            y0 = _client.Random.Next(0, _client.Height);
            radius = _client.Random.Next(0, _client.Width >> 1);
            color = _client.Random.NextRgb();
        }

        public override void Draw()
        {
            _client.Graphics.FillCircle(x0, y0, radius, color);
        }
    }
}
