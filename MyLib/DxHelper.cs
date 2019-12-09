using DxLibDLL;

namespace SAGASALib
{
    public static class DxHelper
    {
        static int currentBlendMode = DX.DX_BLENDMODE_ALPHA;
        static int currentAlpha = 255;
        static int currentBrightRed   = 255;
        static int currentBrightGreen = 255;
        static int currentBrightBlue  = 255;

        /// <summary>
        /// 画像を描画する際のブレンドモードとアルファ値を設定する
        /// </summary>
        /// <param name="blendMode">ブレンドモード（DX.DX_BLENDMODE_XXX）</param>
        /// <param name="alpha">アルファ値（0～255）</param>
        public static void SetBlendMode(int blendMode, int alpha)
        {
            if (blendMode != currentBlendMode || alpha != currentAlpha)
            {
                currentBlendMode = blendMode;
                currentAlpha = alpha;
                DX.SetDrawBlendMode(currentBlendMode, currentAlpha);
            }
        }

        /// <summary>
        /// 画像を描画する際のアルファ値を設定する
        /// </summary>
        /// <param name="alpha"></param>
        public static void SetAlpha(int alpha)
        {
            if (alpha != currentAlpha)
            {
                currentAlpha = alpha;
                DX.SetDrawBlendMode(currentBlendMode, currentAlpha);
            }
        }

        /// <summary>
        /// 画像を描画する際の輝度（乗算カラー）を設定する
        /// </summary>
        /// <param name="red">赤（0～255）</param>
        /// <param name="green">緑（0～255）</param>
        /// <param name="blue">青（0～255）</param>
        public static void SetColor(int red, int green, int blue)
        {
            if (red != currentBrightRed || green != currentBrightGreen || blue != currentBrightBlue)
            {
                currentBrightRed = red;
                currentBrightGreen = green;
                currentBrightBlue = blue;
                DX.SetDrawBright(currentBrightRed, currentBrightGreen, currentBrightBlue);
            }
        }
    }
}
