using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public static class Sound
    {
        public static bool playOn = true;

        public static void Play(string handle)
        {
            if (playOn)
            {
                DX.PlaySoundMem(ResourceLoader.GetSound(handle), DX.DX_PLAYTYPE_BACK);
            }
        }

        public static void Loop(string handle)
        {
            if(playOn)
            {
                DX.PlaySoundMem(ResourceLoader.GetSound(handle), DX.DX_PLAYTYPE_LOOP);
            }
        }
        
        public static void DefinitelyPlay(string handle)//絶対に鳴らす
        {
            DX.PlaySoundMem(ResourceLoader.GetSound(handle), DX.DX_PLAYTYPE_BACK);
        }

        public static void Stop(string handle)
        {
            DX.StopSoundMem(ResourceLoader.GetSound(handle));
        }


        public static bool CheckPlaySound(string handle)
        {
            if (DX.CheckSoundMem(ResourceLoader.GetSound(handle)) == 1)
                return true;
            return false;
        }
    }
}