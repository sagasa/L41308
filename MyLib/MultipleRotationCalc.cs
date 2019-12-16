using System;
using DxLibDLL;

namespace SAGASALib
{
    //複数の回転を追加し描画するツール
    //回転->移動->スケール
    public class MultipleRotationCalc
    {
        private float _rotate = 0f;
        private Vec2f _oldPivot = Vec2f.ZERO;
        private Vec2f _offset = Vec2f.ZERO;
        private readonly Vec2f[] _vertex = new Vec2f[4];

        //位置を指定して描画
        public void Draw(Vec2f pos,int image, bool invert)
        {
            Vec2f[] vertex = new Vec2f[_vertex.Length];
            for (var i = 0; i < _vertex.Length; i++)
            {
                vertex[i] = _vertex[i] + pos;
                Debug.DrawPos(pos, _vertex[i]);
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
            _oldPivot = Vec2f.ZERO;
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
            Debug.DrawPos(new Vec2f(128, 128) , pivot * scale, "pivot", DX.GetColor(0, 100, 200));
            Vec2f rotatePivot = (pivot - _offset).Rotate(_rotate) + _oldPivot;
            Debug.DrawPos(new Vec2f(128, 128),Vec2f.ZERO, "center", DX.GetColor(0, 0, 200));

            Debug.DrawVec2(new Vec2f(64, 128), (pivot - _offset).Rotate(_rotate)*scale);

            Debug.DrawPos(new Vec2f(128, 128), rotatePivot * scale, "pivot", DX.GetColor(0, 200, 200));
            Debug.DrawAngle((rotatePivot * scale) + new Vec2f(128, 128), _rotate, "rotate", DX.GetColor(200, 0, 0));

            for (var i = 0; i < _vertex.Length; i++)
            {
                _vertex[i] = ((_vertex[i] - rotatePivot).Rotate(angle)) + rotatePivot;
            }
            _rotate += angle;
            _offset = pivot;
            _oldPivot = rotatePivot;
            return this;
        }
        public MultipleRotationCalc Move(Vec2f vec)
        {
            for (var i = 0; i < _vertex.Length; i++)
            {
                _vertex[i] = _vertex[i] + vec;
            }
            return this;
        }
    }
}