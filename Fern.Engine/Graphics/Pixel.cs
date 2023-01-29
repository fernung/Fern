using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Fern.Engine.Graphics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Pixel
    {
        [FieldOffset(0)] public int Packed;
        [FieldOffset(0)] public uint UPacked;
        [FieldOffset(0)] public byte B;
        [FieldOffset(1)] public byte G;
        [FieldOffset(2)] public byte R;
        [FieldOffset(3)] public byte A;

        public Pixel(int packed)
        {
            Packed = packed;
        }
        public Pixel(uint packed)
        {
            UPacked = packed;
        }
        public Pixel(byte b, byte g, byte r, byte a = byte.MaxValue)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        public override int GetHashCode() =>
            Packed.GetHashCode();
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            GetHashCode() == obj?.GetHashCode();
        public override string ToString() =>
            $"#{Packed:X2}";

        public static readonly Pixel Transparent = new(0);
        public static readonly Pixel White  = new(0xFFFFFFFF);
        public static readonly Pixel Gray   = new(0xFF7F7F7F);
        public static readonly Pixel Black  = new(0xFF000000);

        public static readonly Pixel Yellow     = new(0xFFFFFF00);
        public static readonly Pixel Magenta    = new(0xFFFF00FF);
        public static readonly Pixel Red        = new(0xFFFF0000);
        public static readonly Pixel Cyan       = new(0xFF00FFFF);
        public static readonly Pixel Green      = new(0xFF00FF00);
        public static readonly Pixel Blue       = new(0xFF0000FF);
    }

    public static class PixelExtensions
    {
        public static Pixel NextRgb(this Random r) =>
            new(r.Next() | (0xFF << 24));
        public static Pixel NextRgba(this Random r) =>
            new(r.Next());
    }
}
