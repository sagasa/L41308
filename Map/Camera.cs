using DxLibDLL;

namespace Giraffe
{
    // 画面のスクロール量を管理するクラス。
    // 画像の描画機能を持つ。
    // スクロールの影響を受けるオブジェクトはこのクラスを通じて描画することで
    // スクロールを意識せずに描画を行うことができる。
    public static class Camera
    {
        // カメラの位置。
        // 画面左上のワールド座標を表す。
        public static float x;
        public static float y;

        // 指定されたワールド座標が画面の中心に来るように、カメラの位置を変更する
        public static void LookAt(float targetX)
        {
            x = targetX - Screen.Width / 2;
        }

        /// <summary>
        /// 画像を描画する。スクロールの影響を受ける。
        /// </summary>
        /// <param name="worldX">左端のx座標</param>
        /// <param name="worldY">上端のy座標</param>
        /// <param name="handle">画像ハンドル</param>
        /// <param name="flip">左右反転するならtrue, しないならfalse（反転しない場合は省略可）</param>
        public static void DrawGraph(float worldX, float worldY, int handle, bool flip = false)
        {
            if (flip) DX.DrawTurnGraphF(worldX - x, worldY - y, handle);
            else DX.DrawGraphF(worldX - x, worldY - y, handle);
        }

        /// <summary>
        /// 四角形（枠線のみ）を描画する
        /// </summary>
        /// <param name="left">左端</param>
        /// <param name="top">上端</param>
        /// <param name="right">右端</param>
        /// <param name="bottom">下端</param>
        /// <param name="color">色</param>
        public static void DrawLineBox(float left, float top, float right, float bottom, uint color)
        {
            DX.DrawBox(
                (int)(left - x + 0.5f),
                (int)(top - y + 0.5f),
                (int)(right - x + 0.5f),
                (int)(bottom - y + 0.5f),
                color,
                DX.FALSE);
        }
    }
}