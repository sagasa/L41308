using static System.Math;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    //回転テスト
    public class StaticMapObject
    {
        public int x, y;

        private const float maxX = 16;

        private int image = ResourceLoader.GetGraph("off2.png");

        private float offsetAngle;

        const float viewAngle = (float)PI / 180f * 80f;

        private int sizeX;
        private int sizeY;
        public StaticMapObject(int x, int y)
        {
            this.x = x;
            this.y = y;
            DX.GetGraphSize(image, out sizeX, out sizeY);
            offsetAngle = (float)PI * 2f * (x / maxX) + (float)PI / 11f;
        }

        private uint white = DX.GetColor(255, 255, 255);
        public void Draw()
        {
            offsetAngle += (float)PI / 180;

            float scaleX = (float)Cos(offsetAngle) * 0.5f;
            float scaleY = 0.5f;
            float a = (float)Sin(offsetAngle) * sizeX / 4 * (float)Cos(viewAngle);
            float posX = (float)Sin(offsetAngle) * 160 + 200;
            float posY = (float)Cos(offsetAngle) * 160 * (float)Cos(viewAngle) + 200;
            DX.DrawCircle(200, 200, 3, white);
            float angle = -offsetAngle;
            DX.DrawModiGraphF(posX - sizeX / 2f * scaleX, posY - sizeY / 2f * scaleY + a,
                              posX + sizeX / 2f * scaleX, posY - sizeY / 2f * scaleY - a,
                              posX + sizeX / 2f * scaleX, posY + sizeY / 2f * scaleY - a,
                              posX - sizeX / 2f * scaleX, posY + sizeY / 2f * scaleY + a, image);
        }
    }
}