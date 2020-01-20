using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public enum State
    {
        Sleep,
        Active,
        Dead,
    }

    public class Particle
    {
        public float x;               //中心位置x座標
        public float y;               //中心位置ｙ座標
        public int lifeSpan;          //寿命（フレーム）
        public int imageHndle;        //画像ハンドル
        public float vx;              //移動速度（ｘ）
        public float vy;              //移動速度（ｙ）
        public float forceX;          //横向きの外力（風など）
        public float forceY;　　　　  //縦方向の外力（重力や浮力）　　　　　　　
        public float startScale = 1f; //開始時の拡大率
        public float endScale = 1f;   //終了時の拡大率
        public int red = 255;         //赤
        public int green = 255;       //緑
        public int blue = 255;        //青
        public float angle;           //画像の向き（ラジアン）
        public float angularVelocity; //回転速度（ラジアン/フレーム）
        public float angularDamp = 1f;//回転速度の減衰（維持率）
        public float damp = 1f;       //空気抵抗による速度の減衰（速度の維持率）
        public int blendMode = DX.DX_BLENDMODE_ALPHA;//ブレンドモード
        public int delay = 0;         //発生までの延長時間（フレーム）
        public State state = State.Sleep;//パーティクルの状態
        public int alpha = 255;       //フェードインもフェードアウトのしてないときの基本のアルファ値
        public float fadeInTime = 0f; //フェードインにかける時間（0～１で指定）
        public float fadeOutTime = 0f;//フェードアウトにかける時間（0～1で指定）
        private int age = 0;          //生まれてからの経過時間（フレーム）

        public void Update()
        {
            //発生までの遅延時間が設定されている場合は、何もせず待機
            if (delay > 0)
            {
                delay--;//デクリメント
                return;//何もしないで終了
            }

            state = State.Active;

            age++; //経過時間をインクリメント

            //寿命を超えたら、死亡、消える
            if (age > lifeSpan)
            {
                //isDead = true;
                state = State.Dead;
                return;
            }

            //外力を適用
            vx += forceX;
            vy += forceY;

            //空気抵抗による速度の減衰
            vx *= damp;
            vy *= damp;

            //速度分だけ移動
            x += vx;
            y += vy;

            //回転速度の減衰
            angularVelocity *= angularDamp;

            //回転速度の分だけ回転
            angle += angularVelocity;
        }

        public void Draw()
        {
            //アクティブじゃないパーティクルは描画しない
            if (state != State.Active)
                return;

            //進歩率
            float progressRate = (float)age / lifeSpan;

            //拡大率を計算
            float scale = MyMath.Lerp(startScale, endScale, progressRate);

            //アルファ値の計算
            int currentAlpha = (int)(Math.Min(Math.Min(progressRate /
                fadeInTime, (1f - progressRate) / fadeOutTime), 1f) * alpha);

            //色を指定
            DX.SetDrawBright(red, green, blue);

            //アルファ値を指定
            DX.SetDrawBlendMode(blendMode, currentAlpha);

            //描画する
            DX.DrawRotaGraphF(x, y, scale, angle, imageHndle);

            //アルファ値を元に戻す
            DX.SetDrawBlendMode(DX.DX_BLENDGRAPHTYPE_ALPHA, 255);
            //色を元に戻す
            DX.SetDrawBright(255, 255, 255);
        }
    }
}
