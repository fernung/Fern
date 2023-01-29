using Fern.Engine.Clients;
using System.Numerics;

namespace Fern.Engine.Graphics
{
    public class GraphicsManager : IDisposable
    {
        private readonly Client _client;
        protected readonly Surface _buffer;

        protected bool _disposed;

        public Surface BackBuffer =>
            _buffer;
        public int BackBufferWidth
        {
            get => _buffer.Width;
            set => ResizeBuffer(value, _buffer.Height);
        }
        public int BackBufferHeight
        {
            get => _buffer.Height;
            set => ResizeBuffer(_buffer.Width, value);
        }

        public int ClientWidth
        {
            get => _client.Width;
            set => _client.Width = value;
        }
        public int ClientHeight
        {
            get => _client.Height;
            set => _client.Height = value;
        }
        public string ClientTitle
        {
            get => _client.Text;
            set => _client.Text = value;
        }
        public string ClientSubTitle
        {
            set => _client.Text = $"{_client.Settings.Title} - {value}";
        }

        public Pixel this[int i]
        {
            get => _buffer[i];
            set => _buffer[i] = value;
        }
        public Pixel this[int x, int y]
        {
            get => _buffer[x, y];
            set => _buffer[x, y] = value;
        }
        public Pixel this[float x, float y]
        {
            get => _buffer[x, y];
            set => _buffer[x, y] = value;
        }

        public GraphicsManager(Client client)
        {
            _disposed = false;
            _client = client;
            _client.Width = _client.Settings.Width;
            _client.Height = _client.Settings.Height;
            _client.Text = _client.Settings.Title;
            _client.MinimizeBox = _client.Settings.AllowMinimize;
            _client.MaximizeBox = _client.Settings.AllowMaximize;
            _buffer = new(_client.Width, _client.Height);
            _client.BackgroundImage = _buffer.Bitmap;
        }

        public void Clear() =>
            Clear(Pixel.Transparent);
        public void Clear(Pixel color) =>
            _buffer.Pixels.Fill(color);

        public void DrawText(string text, int x, int y, Pixel color, int fontSize = 12, string fontFamily = "Calibri")
        {
            using var graphics = System.Drawing.Graphics.FromImage(_buffer.Bitmap);
            using var font = new Font(fontFamily, fontSize);
            using var brush = new SolidBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            graphics.DrawString(text, font, brush, x, y);
        }

        public void DrawLine(int x0, int y0, int x1, int y1, Pixel color)
        {
            bool steep = Math.Abs(x0 - x1) < Math.Abs(y0 - y1);
            if (steep)
            {
                (x0, y0) = (y0, x0);
                (x1, y1) = (y1, x1);
            }
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }
            int index;
            int dx = x1 - x0, dy = y1 - y0;
            int x = x0, xInc = dx << 1;
            int y = y0, yInc = y1 > y0 ? 1 : -1;
            int e = 0, eInc = Math.Abs(dy) << 1;
            for (; x <= x1; x++)
            {
                index = steep ? y + x * _buffer.Width : x + y * _buffer.Width;
                if (index >= 0 && index < _buffer.Length)
                    _buffer[index] = color;
                e += eInc;
                if (e > dx)
                {
                    y += yInc;
                    e -= xInc;
                }
            }
        }
        public void DrawTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Pixel color)
        {
            if (y0 == y1 && y0 == y2) 
                return;
            if (y0 > y1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }
            if (y0 > y2)
            {
                (x0, x2) = (x2, x0);
                (y0, y2) = (y2, y0);
            }
            if (y1 > y2)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }

            DrawLine(x0, y0, x1, y1, color);
            DrawLine(x1, y1, x2, y2, color);
            DrawLine(x2, y2, x0, y0, color);
        }
        public void DrawRectangle(int x0, int y0, int width, int height, Pixel color)
        {
            int index, x = x0, y = y0;
            int x1 = x0 + width;
            int y1 = y0 + height;
            for (; x <= x1; x++)
                for (; y <= y1; y++)
                {
                    index = x + y * _buffer.Width;
                    if (index < _buffer.Length)
                        _buffer[index] = color;
                }
        }
        public void DrawCircle(int x, int y, int radius, Pixel color)
        {
            int d = 3 - 2 * radius;
            int i = 0;
            int j = radius;

            while (i <= j)
            {
                _buffer[x + i, y + j] = color;
                _buffer[x + j, y + i] = color;
                _buffer[x + j, y - i] = color;
                _buffer[x + i, y - j] = color;
                _buffer[x - i, y - j] = color;
                _buffer[x - j, y - i] = color;
                _buffer[x - j, y + i] = color;
                _buffer[x - i, y + j] = color;

                if (d < 0)
                {
                    d += 4 * i + 6;
                }
                else
                {
                    d += 4 * (i - j) + 10;
                    j--;
                }

                i++;
            }
        }

        public void FillTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Pixel color)
        {
            var minX = Math.Min(x0, Math.Min(x1, x2));
            var minY = Math.Min(y0, Math.Min(y1, y2));
            var maxX = Math.Max(x0, Math.Max(x1, x2));
            var maxY = Math.Max(y0, Math.Max(y1, y2));

            for (var y = minY; y <= maxY; y++)
                for (var x = minX; x <= maxX; x++)
                {
                    if(!IsInsideTriangle(x, y, x0, y0, x1, y1, x2, y2))
                        continue;
                    _buffer[x, y] = color;
                }
        }
        public void FillRectangle(int x, int y, int width, int height, Pixel color)
        {
            var xw = x + width;
            var yh = y + height;
            for (var ys = y; ys <= yh; ys++)
                for (var xs = y; xs <= xw; xs++)
                    _buffer[xs, ys] = color;
        }
        public void FillCircle(int x, int y, int radius, Pixel color)
        {
            for (var ys = -radius; ys <= radius; ys++)
                for (var xs = -radius; xs <= radius; xs++)
                    if (xs * xs + ys * ys <= radius * radius)
                        _buffer[x + xs, y + ys] = color;
        }

        public void Resize(int width, int height)
        {
            ResizeScreen(width, height);
            ResizeBuffer(width, height);
        }
        public void ResizeScreen(int width, int height) =>
            _client.Size = new(width, height);
        public void ResizeBuffer(int width, int height)
        {
            _buffer.Resize(width, height);
            _client.BackgroundImage = _buffer.Bitmap;
        }

        public void ShowFps() =>
            ClientSubTitle = $"FPS: {_client.Fps:0.00}";
        public void Present() =>
            _client.Invalidate();

        public void Dispose()
        {
            if (_disposed) 
                return;
            _disposed = true;
            _buffer?.Dispose();
        }

        private static bool IsInsideTriangle(int pX, int pY, int x0, int y0, int x1, int y1, int x2, int y2)
        {
            float w0 = ((y1 - y2) * (pX - x2) + (x2 - x1) * (pY - y2)) /
                       ((y1 - y2) * (x0 - x2) + (x2 - x1) * (y0 - y2));
            float w1 = ((y2 - y0) * (pX - x2) + (x0 - x2) * (pY - y2)) /
                       ((y1 - y2) * (x0 - x2) + (x2 - x1) * (y0 - y2));
            float w2 = 1 - w0 - w1;
            return w0 >= 0 && w1 >= 0 && w2 >= 0;
        }
    }
}
