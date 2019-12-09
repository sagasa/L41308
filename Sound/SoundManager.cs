using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public static class SoundManager
    {
        static DX.VECTOR ListenerPos;
        static DX.VECTOR ListenerDir;

        public static Dictionary<string, int> bgmMap = new Dictionary<string, int>();
        public static Dictionary<string, DX.VECTOR> bgmPos = new Dictionary<string, DX.VECTOR>();

        static float bgmDis = 100.0f;//聞こえる範囲
        static float speed = 1.0f / 6.0f;//リスナーの移動速度

        public static void Load()
        {
            ListenerPos = DX.VGet(0.0f, 0.0f, 0.0f);//リスナーの位置
            ListenerDir = DX.VGet(0.0f, 0.0f, 1.0f);//リスナーの向き
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);//向きと位置を設定

            bgmMap["title"] = 0;
            bgmMap["play"] = 0;
            bgmMap["result"] = 0;

            bgmPos["title"] = DX.VGet(100.0f, 0.0f, 0.0f);
            bgmPos["play"] = DX.VGet(250.0f, 0.0f, 0.0f);
            bgmPos["result"] = DX.VGet(400.0f, 0.0f, 0.0f);
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

        public static int bgmMove(string name)
        {
            if (DX.CheckSoundMem(bgmMap[name]) != 1 &&
                bgmPos[name].x - bgmDis - 10 <= ListenerPos.x && ListenerPos.x <= bgmPos[name].x + bgmDis + 10)//再生してない、範囲内の時
            {
                bgmMap[name] = ResourceLoader.GetSound(name + "_BGM.wav", DX.TRUE);
                DX.Set3DRadiusSoundMem(bgmDis, bgmMap[name]);
                DX.Set3DPositionSoundMem(bgmPos[name], bgmMap[name]);
                Sound.Loop(bgmMap[name]);
            }
            else if (DX.CheckSoundMem(bgmMap[name]) == 1 &&
                (ListenerPos.x <= bgmPos[name].x - bgmDis - 15 || bgmPos[name].x + bgmDis + 15 <= ListenerPos.x))//再生中、範囲外の時
            {
                ResourceLoader.RemoveSound(name + "_BGM.wav");
                DX.DeleteSoundMem(bgmMap[name]);
            }
            return bgmMap[name];
        }

        public static void Debug()//フェードの可視化、1で再生中,0で停止中,-1でメモリにない(エラー)
        {
            DX.DrawString(0, 15, "　タイトル:" + DX.CheckSoundMem(bgmMap["title"]) +
                                 "　プレイ:" + DX.CheckSoundMem(bgmMap["play"]) +
                                 "　リザルト:" + DX.CheckSoundMem(bgmMap["result"]),
                          DX.GetColor(255, 255, 255));

            // 音の再生位置を描画
            DX.DrawCircle(//タイトル,青
                100 + (int)bgmPos["title"].x, 200 + (int)bgmPos["title"].z,
                (int)bgmDis, DX.GetColor(0, 0, 255), DX.FALSE);

            DX.DrawCircle(//プレイ,黄
                100 + (int)bgmPos["play"].x, 200 + (int)bgmPos["play"].z,
                (int)bgmDis, DX.GetColor(255, 255, 0), DX.FALSE);

            DX.DrawCircle(//リザルト,紫
                 100 + (int)bgmPos["result"].x, 200 + (int)bgmPos["result"].z,
                 (int)bgmDis, DX.GetColor(255, 0, 255), DX.FALSE);

            DX.DrawCircle(//リスナー,赤
                100 + (int)ListenerPos.x, 200 + (int)ListenerPos.z,
                2, DX.GetColor(255, 0, 0));
        }
    }
}