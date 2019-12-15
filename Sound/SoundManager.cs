﻿using System.Collections.Generic;
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
        static float interval = 150.0f;

        static float fadeSpeed = bgmDis / 60.0f;
        static float crossSpeed = interval / 60.0f;

        public static bool fadeInit = false;
        public static void Load()
        {
            ListenerPos = DX.VGet(0.0f, 1.0f, 0.0f);//リスナーの位置
            ListenerDir = DX.VGet(0.0f, -1.0f, 0.0f);//リスナーの向き
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);//向きと位置を設定

            bgmMap["title"] = 0;
            bgmMap["play"] = 0;
            bgmMap["result"] = 0;
            bgmMap["play_fast"] = 0;
            bgmMap["tutorial"] = 0;

            bgmPos["title"] = DX.VGet(100.0f, 0.0f, 0.0f);//BGMの初期位置の設定
            bgmPos["play"] = DX.VGet(250.0f, 0.0f, 0.0f);
            bgmPos["result"] = DX.VGet(400.0f, 0.0f, 0.0f);
            bgmPos["tutorial"] = DX.VGet(550.0f, 0.0f, 0.0f);
            bgmPos["play_fast"] = bgmPos["play"];
        }

        public static void FadeIn(string name, int time)
        {
            if (!fadeInit)//初期化
            {
                ListenerPos.x = bgmPos[name].x;
                ListenerPos.z = bgmPos[name].z + bgmDis;
                fadeInit = true;
            }

            if (ListenerPos.z > bgmPos[name].z)
            {
                ListenerPos.z -= fadeSpeed / time;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            else if (ListenerPos.z < bgmPos[name].z)
            {
                ListenerPos.z = bgmPos[name].z;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }

            PlayBgm(name);
        }

        public static void FadeOut(string name, int time)
        {
            if (!fadeInit)//初期化
            {
                ListenerPos.x = bgmPos[name].x;
                ListenerPos.z = bgmPos[name].z;
                fadeInit = true;
            }

            if (ListenerPos.z < bgmPos[name].z + bgmDis + 10)
            {
                ListenerPos.z += fadeSpeed / time;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            else if (ListenerPos.z > bgmPos[name].z + bgmDis + 10)
            {
                ListenerPos.z = bgmPos[name].z + bgmDis + 10;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }

            PlayBgm(name);
        }

        public static void CrossFade(string name1, string name2, int time)
        {
            if (!fadeInit)
            {
                ListenerPos.x = bgmPos[name1].x;
                ListenerPos.z = bgmPos[name1].z;
                bgmPos[name2] = DX.VGet(bgmPos[name1].x + interval, 0.0f, 0.0f);
                fadeInit = true;
            }

            if (ListenerPos.x < bgmPos[name2].x)
            {
                ListenerPos.x += crossSpeed / time;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            else if (ListenerPos.x > bgmPos[name2].x)
            {
                ListenerPos.x = bgmPos[name2].x;
            }

            PlayBgm(name1);
            PlayBgm(name2);
        }

        public static void BgmMove(string name)
        {
            bgmPos[name] = DX.VGet(bgmPos[name].x - fadeSpeed, bgmPos[name].y, bgmPos[name].z);
            DX.Set3DPositionSoundMem(bgmPos[name], bgmMap[name]);
            PlayBgm(name);
        }

        public static void ListenerMove(int time)//テスト用、使い終わったら消去します
        {
            ListenerPos.x += fadeSpeed / time;
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
        }

        public static void PlayBgm(string name)//テストで使うのでpublic、使い終わったらpublicは消す
        {
            //BGMの当たり判定、斜め移動してないのでとりあえず四角形で計算
            if (DX.CheckSoundMem(bgmMap[name]) != 1 &&
                bgmPos[name].x - bgmDis - 5 <= ListenerPos.x && ListenerPos.x <= bgmPos[name].x + bgmDis + 5 &&
                bgmPos[name].z - bgmDis - 5 <= ListenerPos.z && ListenerPos.z <= bgmPos[name].z + bgmDis + 5)//再生してない、範囲内の時
            {
                bgmMap[name] = ResourceLoader.GetSound(name + "_BGM.wav", DX.TRUE);//読み込み
                DX.Set3DRadiusSoundMem(bgmDis, bgmMap[name]);
                DX.Set3DPositionSoundMem(bgmPos[name], bgmMap[name]);
                Sound.Loop(bgmMap[name]);//再生
            }
            else if (DX.CheckSoundMem(bgmMap[name]) == 1 &&
                (ListenerPos.x <= bgmPos[name].x - bgmDis - 10 || bgmPos[name].x + bgmDis + 10 <= ListenerPos.x ||
                 ListenerPos.z <= bgmPos[name].z - bgmDis - 10 || bgmPos[name].z + bgmDis + 10 <= ListenerPos.z))//再生中、範囲外の時
            {
                ResourceLoader.RemoveSound(name + "_BGM.wav");//消去
                DX.DeleteSoundMem(bgmMap[name]);
            }
        }

        public static void Debug()
        {//フェードの可視化、1で再生中,0で停止中,-1でメモリにない(エラー)
            DX.DrawString(0, 15, "　タイトル:" + DX.CheckSoundMem(bgmMap["title"]) +
                                 "　プレイ:" + DX.CheckSoundMem(bgmMap["play"]) +
                                 "　リザルト:" + DX.CheckSoundMem(bgmMap["result"]) +
                                 "　リスナー x:" + ListenerPos.x + "　z:" + ListenerPos.z,
                                 DX.GetColor(255, 255, 255));

            // 音の再生位置を描画
            DX.DrawCircle(//タイトル,青
                (int)bgmPos["title"].x, 150 + (int)bgmPos["title"].z,
                (int)bgmDis, DX.GetColor(0, 0, 255), DX.FALSE);

            DX.DrawCircle(//プレイ,黄
                (int)bgmPos["play"].x, 150 + (int)bgmPos["play"].z,
                (int)bgmDis, DX.GetColor(255, 255, 0), DX.FALSE);

            DX.DrawCircle(//リザルト,紫
                (int)bgmPos["result"].x, 150 + (int)bgmPos["result"].z,
                (int)bgmDis, DX.GetColor(255, 0, 255), DX.FALSE);

            DX.DrawCircle(//リスナー,赤
                (int)ListenerPos.x, 150 + (int)ListenerPos.z,
                2, DX.GetColor(255, 0, 0));
        }
    }
}