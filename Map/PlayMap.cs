using SAGASALib;
using System;
using System.IO;
using Debug = System.Diagnostics.Debug;
using DxLibDLL;

namespace Giraffe
{
    public class PlayMap
    {
        public const int None = -1;

        enum MapObject
        {
            Goal,
        }

        //const int Width = 10;
        //const int Height = 25;

        //Tile１セルのサイズ
        public static readonly Vec2f CellSize = new Vec2f(64,64);
        //画面に入るセルの量
        public static readonly Vec2f ScreenSize = Screen.Size / CellSize;

        ScenePlay scenePlay;
        public readonly int[,] MapData;
        public readonly Vec2f MapSize;
      

        public PlayMap(ScenePlay scenePlay, string filePath)
        {
            this.scenePlay = scenePlay;
            MapData = Read("Map/" + filePath + ".csv");
            MapSize = new Vec2f(MapData.GetLength(0), MapData.GetLength(1));

            SpawnObject();
            //LoadObject("Map/" + filePath + ".csv");

            
        }


        //表示時のCSVの左上座標
        private Vec2f cameraPos;


        /* 縦横の事前定義が必要ない読込に変更
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
        }//*/

        private static readonly char[] COMMA = new[] { ',' };

        private static int[,] Read(string filePath)
        {
            string[] allLine = File.ReadAllLines(filePath);
            if (allLine.Length <= 0)
                throw new IndexOutOfRangeException();

            int[,] data = new int[allLine[0].Split(COMMA).Length, allLine.Length];
            for (int y = data.GetLength(1) - 1; y >= 0; y--)
            {
                string[] splitted = allLine[y].Split(COMMA);
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    data[x, y] = int.Parse(splitted[x]);
                }
            }
            return data;
        }

        /*
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
        }//*/

        void SpawnObject()
        {
            // 生成位置
            for (int y = MapData.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < MapData.GetLength(0); x++)
                {
                    int objectID = MapData[x, y];
                    if (objectID == 0) //Leaf
                    {

                        scenePlay.gameObjects.Add(new Leaf(scenePlay, new Vec2f(x,y)));
                       
                    }
                    else if(objectID==1)
                    {
                        scenePlay.gameObjects.Add(new Goal(scenePlay, new Vec2f(x, y)));
                    }
                    //else
                    //{
                    //    Debug.Assert(false, "オブジェクトID" + objectID + "番の生成処理は未実装です。");
                    //}                    
                }
            }
        }

        void SpawnObject(Vec2f pos, int objectID)
        {
            // 生成位置

            
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
    }
}



