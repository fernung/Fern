using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Fern.Engine.Graphics
{
    public class Surface : IDisposable
    {
        protected bool _disposed;
        protected int _width;
        protected int _height;
        protected Pixel[] _pixels;
        protected GCHandle _handle;
        protected Bitmap _bitmap;

        public int Width => 
            _width;
        public int Height => 
            _height;
        public int Length =>
            _pixels.Length;
        public Span<Pixel> Pixels => 
            _pixels.AsSpan();
        public Bitmap Bitmap => 
            _bitmap;

        public Pixel this[int i]
        {
            get => InBounds(i) ? _pixels[i] : Pixel.Transparent;
            set { if(InBounds(i)) _pixels[i] = value; }
        }
        public Pixel this[int x, int y]
        {
            get => InBounds(x, y) ? _pixels[Index(x, y)] : Pixel.Transparent;
            set { if (InBounds(x, y)) _pixels[Index(x, y)] = value; }
        }
        public Pixel this[float x, float y]
        {
            get => InBounds(x, y) ? _pixels[Index(x, y)] : Pixel.Transparent;
            set { if (InBounds(x, y)) _pixels[Index(x, y)] = value; }
        }

        public Surface(int width, int height, Span<Pixel> pixels) :
            this(width, height)
        {
            var length = Math.Min(pixels.Length, _pixels.Length);
            pixels.Slice(length).CopyTo(_pixels.AsSpan(0, length));
        }
        public Surface(int width, int height)
        {
            _disposed = false;
            Initialize(width, height);
        }

        public virtual void Resize(int width, int height)
        {
            width = Math.Max(1, width);
            height = Math.Max(1, height);

            _bitmap?.Dispose();
            _handle.Free();

            Initialize(width, height);
        }

        public bool InBounds(int i) =>
            0 <= i && i < _pixels.Length;
        public bool InBounds(int x, int y) =>
            0 <= x && x < _width && 
            0 <= y && y < _height;
        public bool InBounds(float x, float y) =>
            0 <= x && x <= 1 && 
            0 <= y && y <= 1;

        public int Index(int x, int y) =>
            x + y * _width;
        public int Index(float x, float y) =>
            Index((int)(x * _width), (int)(y * _height));
        public (int x, int y) Index(int i) =>
            (i % _width, i / _width);

        public void Dispose()
        {
            if(_disposed)
                return; 
            _disposed = true;
            _bitmap?.Dispose();
            _handle.Free();
        }

        protected virtual void Initialize(int width, int height)
        {
            _width = width;
            _height = height;
            _pixels = new Pixel[width * height];
            _handle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
            _bitmap = new Bitmap(width, height, width << 2, PixelFormat.Format32bppArgb, _handle.AddrOfPinnedObject());
        }
    }
}
