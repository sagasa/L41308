

using SAGASALib;

namespace Giraffe
{
    public static class Screen
    {
        public const int Width = 640;

        public const int Height = 800;

        public static readonly Vec2f Size;

        static Screen()
        {
            Size = new Vec2f(Width,Height);
        }
    }
}
