using DxLibDLL;

namespace SAGASALib
{
    public static class DrawUtil
    {
        public static void DrawThickBox(Vec2f pos, Vec2f vec, uint color, float thick = 1)
        {
            Vec2f right = vec.Right().Normal() * (thick / 2);
            Vec2f left = vec.Left().Normal() * (thick / 2);
            Vec2f pos0 = pos + right;
            Vec2f pos1 = pos + left;
            Vec2f pos2 = pos + vec + left;
            Vec2f pos3 = pos + vec + right;
            DrawTriangle(pos0, pos1, pos2, color);
            DrawTriangle(pos2, pos3, pos0, color);
        }
        public static void DrawTriangle(Vec2f pos0, Vec2f pos1, Vec2f pos2, uint color)
        {
            DX.DrawTriangleAA(pos0.X, pos0.Y, pos1.X, pos1.Y, pos2.X, pos2.Y, color, DX.TRUE);
        }
    }
}