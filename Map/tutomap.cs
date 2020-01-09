using SAGASALib;
using System;
using System.IO;
using Debug = System.Diagnostics.Debug;
using DxLibDLL;

namespace Giraffe
{
    public class tutomap
    {
        public const int None = -1;

        enum MapObject
        {
            Goal,
        }

        //Tile１セルのサイズ
        public static readonly Vec2f CellSize = new Vec2f(64, 64);
        //画面に入るセルの量
        public static readonly Vec2f ScreenSize = Screen.Size / CellSize;
        
        Tutolal tutolal;

        public readonly int[,] MapData;
        public readonly Vec2f MapSize;


        public tutomap(Tutolal tutolal, string filePath)
        {
            this.tutolal= tutolal;
            MapData = Read("Map/" + filePath + ".csv");
            MapSize = new Vec2f(MapData.GetLength(0), MapData.GetLength(1));

            SpawnObject();
            //LoadObject("Map/" + filePath + ".csv");
        }


        //表示時のCSVの左上座標
        private Vec2f cameraPos;



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

                        tutolal.gameObjects.Add(new Leaf(tutolal, new Vec2f(x, y)));

                    }
                    else if (objectID == 1)
                    {

                        tutolal.gameObjects.Add(new Goal(tutolal, new Vec2f(x, y)));

                    }
                                    
                }
            }
        }

        void SpawnObject(Vec2f pos, int objectID)
        {
            // 生成位置


        }

       
    }
}
