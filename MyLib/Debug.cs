using System;
using System.Collections.Generic;
using DxLibDLL;

namespace SAGASALib
{
    public static class Debug
    {
        private static int Handle = DX.CreateFontToHandle(null, 13, 1);


        static Queue<Action> draw = new Queue<Action>();

        private static readonly uint black = DX.GetColor(0, 0, 0);

        private static readonly uint white = DX.GetColor(255, 255, 255);

        public static void DrawVec2(Vec2f vec,string name = "",uint color= 4294902015)
        {
            draw.Enqueue(()=>{
                DX.DrawBoxAA(vec.X - 15, vec.Y-1, vec.X + 15, vec.Y+1, color,DX.TRUE);
                DX.DrawBoxAA(vec.X-1, vec.Y - 15, vec.X+1, vec.Y + 15, color, DX.TRUE);
                DX.DrawStringToHandle((int)vec.X, (int)vec.Y + 2, name + "(" + vec.X + "," + vec.Y+")", black, Handle,white);
            });
        }

        public static void DrawVec2(Vec2f pos, Vec2f vec, string name = "", uint color = 4294902015)
        {
            draw.Enqueue(() => {
                DrawThickBox(pos,vec,color);
                DrawThickBox(pos + vec, vec.Rotate(MyMath.Deg2Rad * 140).Normal() * 15, color);
                DrawThickBox(pos + vec, vec.Rotate(MyMath.Deg2Rad * -140).Normal() * 15, color);
                DX.DrawStringToHandle((int)(vec+pos).X, (int)(vec + pos).Y + 2, name + "(" + vec.X + "," + vec.Y + ")", black, Handle, white);
            });
        }

        private static void DrawThickBox(Vec2f pos,Vec2f vec, uint color, float thick=2)
        {
            Vec2f right = vec.Right().Normal()*(thick/2);
            Vec2f left = vec.Left().Normal() * (thick / 2);
            Vec2f pos0 = pos + right;
            Vec2f pos1 = pos + left;
            Vec2f pos2 = pos + vec + left;
            Vec2f pos3 = pos + vec + right;
            DrawTriangle(pos0, pos1, pos2, color);
            DrawTriangle(pos2, pos3, pos0, color);
        }
        private static void DrawTriangle(Vec2f pos0, Vec2f pos1, Vec2f pos2,uint color)
        {
              DX.DrawTriangleAA(pos0.X,pos0.Y,pos1.X,pos1.Y,pos2.X,pos2.Y,color,DX.TRUE);
        }

        public static void Draw()
        {
            while (0<draw.Count)
            {
                draw.Dequeue().Invoke();
            }
        }
    }
}