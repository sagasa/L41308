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
        
        public static void Stop(string handle)
        {
            DX.StopSoundMem(ResourceLoader.GetSound(handle));
        }
    }
}