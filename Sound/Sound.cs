using DxLibDLL;

namespace Giraffe
{
    public static class Sound
    {
        public static void Play(int handle)
        {//一回のみ再生、主にSE用
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_BACK);
        }

        public static void Loop(int handle)
        {//ループ、主にBGM用
            DX.PlaySoundMem(handle, DX.DX_PLAYTYPE_LOOP);
        }

        public static void Stop(int handle)
        {
            DX.StopSoundMem(handle);
        }
    }
}