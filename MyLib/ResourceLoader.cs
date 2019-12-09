using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DxLibDLL;

namespace SAGASALib
{
    public static class ResourceLoader
    {
        private static Dictionary<string, int> imageMap = new Dictionary<string, int>();
        public static readonly int MissingImage;

        static ResourceLoader()
        {
            MissingImage = DX.LoadGraph("resources/images/missing.png");
            MissingSound = DX.LoadSoundMem("resources/sound/error.mp3");
        }
        //画像の読み込みとキャッシング
        public static int GetSimpleDrawable(string name,int sizeX = -1,int sizeY=-1)
        {
            int image = GetGraph(name);
            DX.GetGraphSize(image,out int x, out int y);
            return 0;
        }
        //画像の読み込みとキャッシング
        public static int GetGraph(string name)
        {
            if (!imageMap.ContainsKey(name))
            {
                //読み込めなかったらmissingを入れる
                try
                {
                    imageMap[name] = DX.LoadGraph("resources/images/"+name);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                    imageMap[name] = MissingImage;
                }
            }
            return imageMap[name];
        }

        private static Dictionary<string, int[]> imageArrayMap = new Dictionary<string, int[]>();
        public static int[] GetGraph(string name,int count)
        {
            //登録名
            string path = name + count;
            if (!imageArrayMap.ContainsKey(path))
            {
                //読み込めなかったらmissingを入れる           y
                try
                {
                    int propImage = DX.LoadGraph("resources/images/" + name);
                    DX.GetGraphSize(propImage, out int x, out int y);

                    DX.DeleteGraph(propImage);
                    int[] array = new int[count];
                    imageArrayMap[path] = array;
                    Console.WriteLine("resources/images/" + name, count, count, 1, x / count, y, array);
                    DX.LoadDivGraph("resources/images/" + name, count, count,1,x/count,y,array);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                    imageArrayMap[path] = Enumerable.Repeat(MissingImage, count).ToArray(); ;
                }
            }
            return imageArrayMap[path];
        }

        private static Dictionary<string, int> soundMap = new Dictionary<string, int>();

        //サウンドの読み込みとキャッシング
        public static int GetSound(string name)
        {
            if (!soundMap.ContainsKey(name))
            {
                //読み込めなかったらmissingを入れる
                try
                {
                    soundMap[name] = DX.LoadSoundMem("resources/sounds/" + name);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                    soundMap[name] = MissingSound;
                }
            }
            return soundMap[name];
        }

        public static void RemoveSound(string name)
        {
            if (soundMap.ContainsKey(name))
            {
                DX.DeleteSoundMem(soundMap[name]);
            }
            soundMap.Remove(name);
        }
    }
}