using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public static class SoundManager
    {
        static DX.VECTOR ListenerPos;
        static DX.VECTOR ListenerDir;
        //各BGMの状態を保存しておく変数
        public static int titleBGM;
        public static int playBGM;
        public static int resultBGM;

        public static DX.VECTOR titleBGMPos;
        public static DX.VECTOR playBGMPos;
        public static DX.VECTOR resultBGMPos;

        static float BGMdis = 100.0f;//聞こえる範囲
        static float speed = 1.0f / 6.0f;//リスナーの移動速度

        public static void Load()
        {
            ListenerPos = DX.VGet(0.0f, 0.0f, 0.0f);//リスナーの位置
            ListenerDir = DX.VGet(0.0f, 0.0f, 1.0f);//リスナーの向き
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);//向きと位置を設定

            titleBGMPos = DX.VGet(100.0f, 0.0f, 0.0f);
            playBGMPos = DX.VGet(250.0f, 0.0f, 0.0f);
            resultBGMPos = DX.VGet(400.0f, 0.0f, 0.0f);
        }

        public static void ListenerMove()
        {
            ListenerPos.x += speed;
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            if (ListenerPos.x >= 500)
            {
                ListenerPos.x = 0.0f;
            }
        }

        public static int BGM(string sceneName, int BGMName, DX.VECTOR BGMPos)//書き直します
        {
            if (DX.CheckSoundMem(BGMName) != 1 && BGMPos.x - BGMdis - 10 <= ListenerPos.x && ListenerPos.x <= BGMPos.x + BGMdis + 10)
            {
                DX.SetCreate3DSoundFlag(DX.TRUE);
                BGMName = DX.LoadSoundMem("Resources/Sound/" + sceneName + "_BGM.wav");
                DX.SetCreate3DSoundFlag(DX.FALSE);
                DX.Set3DRadiusSoundMem(BGMdis, BGMName);
                DX.Set3DPositionSoundMem(BGMPos, BGMName);
                Sound.Loop(BGMName);
            }
            else if (DX.CheckSoundMem(BGMName) == 1 && (ListenerPos.x <= BGMPos.x - BGMdis - 15 || BGMPos.x + BGMdis + 15 <= ListenerPos.x))
            {
                DX.DeleteSoundMem(BGMName);
            }
            return BGMName;
        }

        public static void Debug()//フェードの可視化、1で再生中,0で停止中,-1でメモリにない(エラー)
        {
            DX.DrawString(0, 15, "タイトル:" + DX.CheckSoundMem(titleBGM) +
                     "　プレイ:" + DX.CheckSoundMem(playBGM) +
                     "　リザルト:" + DX.CheckSoundMem(resultBGM),
                     DX.GetColor(255, 255, 255));

            // 音の再生位置を描画
            DX.DrawCircle(//タイトル,青
                100 + (int)titleBGMPos.x, 200 + (int)titleBGMPos.z,
                (int)BGMdis, DX.GetColor(0, 0, 255), DX.FALSE);

            DX.DrawCircle(//プレイ,黄
                100 + (int)playBGMPos.x, 200 + (int)playBGMPos.z,
                (int)BGMdis, DX.GetColor(255, 255, 0), DX.FALSE);

            DX.DrawCircle(//リザルト,紫
                 100 + (int)resultBGMPos.x, 200 + (int)resultBGMPos.z,
                 (int)BGMdis, DX.GetColor(255, 0, 255), DX.FALSE);

            DX.DrawCircle(//リスナー,赤
                100 + (int)ListenerPos.x, 200 + (int)ListenerPos.z,
                2, DX.GetColor(255, 0, 0));
        }
    }
}