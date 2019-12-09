using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using DxLibDLL;

namespace Giraffe
{
    public class PlayMap
    {
        public const int None = -1;

        const int Width = 10;
        const int Height = 25;
        const int CellSize = 64;

        ScenePlay scenePlay;
        int[,] mapData;

        public PlayMap(ScenePlay scenePlay, string filePath)
        {
            this.scenePlay = scenePlay;
            Load("Map/" + filePath + ".csv");
            LoadObject("Map/" + filePath + ".csv");
        }

        void Load(string filePath)
        {
            mapData = new int[Width, Height];

            string[] lines = File.ReadAllLines(filePath);

            Debug.Assert(lines.Length == Height, filePath + "CSVファイルの高さが不正です:" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                string[] splitted = lines[y].Split(new char[] { ',' });

                Debug.Assert(splitted.Length == Width, "CSVファイルの" + y + "行目の列数が不正です:" +
                    splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    mapData[x, y] = int.Parse(splitted[x]);
                }
            }
        }

        void LoadObject(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            Debug.Assert(lines.Length == Height, filePath + "の高さが不正です:" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                string[] splitted = lines[y].Split(new char[] { ',' });

                Debug.Assert(splitted.Length == Width, filePath + "の" + y + "行目の列数が不正です:" + splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    int id = int.Parse(splitted[x]);

                    if (id == -1) continue;

                    SpawnObject(x, y, id);
                }
            }
        }

        void SpawnObject(int mapX, int mapY, int objectID)
        {
            // 生成位置
            float spawnX = mapX * CellSize;
            float spawnY = mapY * CellSize;

            if (objectID == 0) //Leaf
            {
               
                scenePlay.gameObjects.Add( new Leaf(scenePlay,spawnX,spawnY));
            }
            else
            {
                Debug.Assert(false, "オブジェクトID" + objectID + "番の生成処理は未実装です。");
            }
        }

        //public void DrawTerrain()
        //{
        //    // 画面内のマップのみ描画するようにする 
        //    int left = (int)(Camera.x / CellSize);
        //    int top = (int)(Camera.y / CellSize);
        //    int right = (int)((Camera.x + Screen.Width - 1) / CellSize);
        //    int bottom = (int)((Camera.y + Screen.Height - 1) / CellSize);

        //    if (left < 0) left = 0;
        //    if (top < 0) top = 0;
        //    if (right >= Width) right = Width - 1;
        //    if (bottom >= Height) bottom = Height - 1;

        //    for (int y = top; y <= bottom; y++)
        //    {
        //        for (int x = left; x <= right; x++)
        //        {
        //            int id = mapData[x, y];

        //            if (id == None) continue; // 描画しない 

        //            Camera.DrawGraph(x * CellSize, y * CellSize, Image.mapchip[id]);
        //        }
        //    }
        //}

        //public int GetTerrain(float worldX, float worldY)
        //{
        //    // マップ座標系（二次元配列の行と列）に変換する 
        //    int mapX = (int)(worldX / CellSize);
        //    int mapY = (int)(worldY / CellSize);

        //    // 二次元配列の範囲外は、何もないものとして扱う 
        //    if (mapX < 0 || mapX >= Width || mapY < 0 || mapY >= Height)
        //        return None;

        //    return mapData[mapX, mapY]; // 二次元配列から地形IDを取り出して返却する
        //}

        public void Draw()
        {
           
        }
    }
}



