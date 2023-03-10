using Fern.Engine.Clients;
using Fern.Engine.Graphics;

namespace Fern.WF.Demos.RandomShapes
{
    public class RandomRectangleScreen : RandomShapeScreen
    {
        protected int x0, y0;
        protected int x1, y1;
        protected Pixel color;

        public RandomRectangleScreen(Client client) :
            base(client)
        {
        }

        public override void Update()
        {
            base.Update();

            x0 = _client.Random.Next(0, _client.Width);
            y0 = _client.Random.Next(0, _client.Height);
            x1 = _client.Random.Next(0, _client.Width);
            y1 = _client.Random.Next(0, _client.Height);
            color = _client.Random.NextRgb();
        }

        public override void Draw()
        {
            _client.Graphics.DrawRectangle(x0, y0, Math.Abs(x1 - x0), Math.Abs(y1 - y0), color);
        }
    }
}
