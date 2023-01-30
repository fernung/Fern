using Fern.Engine.Clients;
using Fern.Engine.Graphics;

namespace Fern.WF.Demos.RandomShapes
{
    public class RandomFilledTriangleScreen : RandomShapeScreen
    {
        protected int x0, y0;
        protected int x1, y1;
        protected int x2, y2;
        protected Pixel color;

        public RandomFilledTriangleScreen(Client client) :
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
            x2 = _client.Random.Next(0, _client.Width);
            y2 = _client.Random.Next(0, _client.Height);
            color = _client.Random.NextRgb();
        }

        public override void Draw()
        {
            _client.Graphics.FillTriangle(x0, y0, x1, y1, x2, y2, color);
        }
    }
}
