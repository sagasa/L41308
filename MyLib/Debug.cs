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

        public static void DrawPos(Vec2f offset,Vec2f pos,string name = "",uint color= 4294902015)
        {
#if DEBUG
            draw.Enqueue(()=>
            {
                offset += pos;
                DX.DrawBoxAA(offset.X - 15, offset.Y-1, offset.X + 15, offset.Y+1, color,DX.TRUE);
                DX.DrawBoxAA(offset.X-1, offset.Y - 15, offset.X+1, offset.Y + 15, color, DX.TRUE);
                DX.DrawStringToHandle((int)offset.X, (int)offset.Y + 2, name + "(" + pos.X + "," + pos.Y+")", black, Handle,white);
            });
#endif
        }
        public static void DrawVec2(Vec2f pos, Vec2f vec, string name = "", uint color = 4294902015)
        {
#if DEBUG
            draw.Enqueue(() => {
                DrawUtil.DrawThickBox(pos,vec,color);
                DrawUtil.DrawThickBox(pos + vec, vec.Rotate(MyMath.Deg2Rad * 140).Normal() * 15, color);
                DrawUtil.DrawThickBox(pos + vec, vec.Rotate(MyMath.Deg2Rad * -140).Normal() * 15, color);
                DX.DrawStringToHandle((int)(vec+pos).X, (int)(vec + pos).Y + 2, name + "(" + vec.X + "," + vec.Y + ")", black, Handle, white);
            });
#endif
        }
        public static void DrawCircle(Vec2f pos,float rad , uint color = 4294902015)
        {
#if DEBUG
            draw.Enqueue(() => {
                DX.DrawCircleAA(pos.X,pos.Y,rad,10,color,DX.FALSE);
            });
#endif
        }

        private static readonly Vec2f UP = new Vec2f(0,-30);
        public static void DrawAngle(Vec2f pos, float rad, string name = "", uint color = 4294902015)
        {
#if DEBUG
            draw.Enqueue(() => {
                DrawUtil.DrawThickBox(pos,UP.Rotate(rad) , color,2);
                DX.DrawStringToHandle((int)(pos).X, (int)(pos).Y + 2, name + "(" + rad*MyMath.Rad2Deg + "° )", black, Handle, white);
            });
#endif
        }

        

        public static void Draw()
        {
#if DEBUG
            while (0<draw.Count)
            {
                draw.Dequeue().Invoke();
            }
#endif
        }
    }
}