using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public static class Sound
    {
        public static void Play(string handle)
        {
            DX.PlaySoundMem(ResourceLoader.GetSound(handle), DX.DX_PLAYTYPE_BACK);
        }

        public static void Loop(string handle)
        {
            DX.PlaySoundMem(ResourceLoader.GetSound(handle), DX.DX_PLAYTYPE_LOOP);
        }

        public static void Play3D(int handle)
        {
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_BACK);
        }

        public static void Loop3D(int handle)
        {
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_LOOP);
        }
        
        public static void Stop(int handle)
        {
            DX.StopSoundMem(handle);
        }
    }
}