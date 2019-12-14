using System;
using DxLibDLL;

namespace SAGASALib
{
    //複数の回転を追加し描画するツール
    //回転->移動->スケール
    public class MultipleRotationCalc
    {
        private float _rotate = 0f;
        private Vec2f _offset = Vec2f.ZERO;
        private readonly Vec2f[] _vertex = new Vec2f[4];

        //位置を指定して描画
        public void Draw(Vec2f pos,int image, bool invert)
        {
            Vec2f[] vertex = new Vec2f[_vertex.Length];
            for (var i = 0; i < _vertex.Length; i++)
            {
                vertex[i] = _vertex[i] + pos;
                Debug.DrawVec2(vertex[i]);
            }
            if (invert)
            {
                DX.DrawModiGraphF(vertex[1].X, vertex[1].Y, vertex[0].X, vertex[0].Y,
                    vertex[3].X, vertex[3].Y, vertex[2].X, vertex[2].Y, image);
            }
            else
            {
                DX.DrawModiGraphF(vertex[0].X, vertex[0].Y, vertex[1].X, vertex[1].Y,
                    vertex[2].X, vertex[2].Y, vertex[3].X, vertex[3].Y, image);
            }
            
        }

        public MultipleRotationCalc Clear()
        {
            _vertex[0] = new Vec2f(0, 0);
            _vertex[1] = new Vec2f(1, 0);
            _vertex[2] = new Vec2f(1, 1);
            _vertex[3] = new Vec2f(0, 1);
            _rotate = 0;
            _offset = Vec2f.ZERO;
            return this;
        }

        //軸を指定してスケール
        public MultipleRotationCalc Scale(Vec2f scale, Vec2f pivot)
        {
            for (var i = 0; i < _vertex.Length; i++)
            {
                _vertex[i] = ((_vertex[i]-pivot)*scale)+pivot;
            }
            return this;
        }
        public MultipleRotationCalc Rotate(Vec2f pivot,float angle)
        {
            Vec2f scale = new Vec2f(128, 128);
            pivot = pivot.Rotate(_rotate);

            Debug.DrawVec2((pivot * scale)+new Vec2f(128,128),"rotate",DX.GetColor(0,200,0));
            for (var i = 0; i < _vertex.Length; i++)
            {
                _vertex[i] = ((_vertex[i] - pivot).Rotate(angle)) + pivot;
            }
            _rotate += angle;
            return this;
        }
        public MultipleRotationCalc Move(Vec2f vec)
        {
            _offset += vec;
            for (var i = 0; i < _vertex.Length; i++)
            {
                _vertex[i] = _vertex[i] + vec;
            }
            return this;
        }
    }
}