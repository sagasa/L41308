using System;
using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class BgmManager
    {
        public bool playOn = true;

        DX.VECTOR ListenerPos;
        DX.VECTOR ListenerDir;

        Dictionary<string, int> bgmMap = new Dictionary<string, int>();
        Dictionary<string, DX.VECTOR> bgmPos = new Dictionary<string, DX.VECTOR>();
        string[] bgmName = new string[] { "title", "play", "result", "tutorial" }; 

        float bgmDis = 100.0f;//聞こえる範囲
        float interval = 150.0f;//bgmDisとintervalでクロスフェードの重なりを調整する

        public string currentScene = "none";//クロスフェードで使用

        float r = 0;
        float degree = 0;
        float fadeDegre = 0;
        float radian;

        public void Load()
        {
            ListenerPos = DX.VGet(0.0f, 1.0f, bgmDis);//リスナーの位置
            ListenerDir = DX.VGet(0.0f, -1.0f, 0.0f);//リスナーの向き
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);//向きと位置を設定

            bgmMap["title"] = 0;
            bgmMap["play"] = 0;
            bgmMap["result"] = 0;
            bgmMap["play_fast"] = 0;
            bgmMap["tutorial"] = 0;

            bgmPos["title"] = DX.VGet(0.0f, 0.0f, 0.0f);//BGMの位置の設定
            bgmPos["play"] = DX.VGet(0.0f, 0.0f, 0.0f);
            bgmPos["result"] = DX.VGet(0.0f, 0.0f, 0.0f);
            bgmPos["tutorial"] = DX.VGet(0.0f, 0.0f, 0.0f);
            //bgmPos["play_fast"] = bgmPos["play"];
        }

        public void FadeIn(string name, int time)
        {
            if (!CheckPlayBgm(name))
            {
                ListenerPos.x = bgmPos[name].x;
                ListenerPos.z = bgmPos[name].z + bgmDis;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }

            if (ListenerPos.z > bgmPos[name].z)
            {
                ListenerPos.z -= bgmDis / time;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            else if (ListenerPos.z < bgmPos[name].z)
            {
                ListenerPos.z = bgmPos[name].z;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            PlayBgm(name);
        }

        public void FadeOut(string name, int time)
        {
            if (ListenerPos.x != bgmPos[name].x)
            {
                if (ListenerPos.x > bgmPos[name].x + 1)
                {
                    ListenerPos.x -= 0.7f;
                }
                else if (ListenerPos.x < bgmPos[name].x - 1)
                {
                    ListenerPos.x += 0.7f;
                }
                else if (ListenerPos.x != bgmPos[name].x)
                {
                    ListenerPos.x = bgmPos[name].x;
                }
            }

            if (ListenerPos.z < bgmPos[name].z + bgmDis + 10)
            {
                ListenerPos.z += bgmDis / time;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            else if (ListenerPos.z > bgmPos[name].z + bgmDis + 10)
            {
                ListenerPos.z = bgmPos[name].z + bgmDis + 10;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            PlayBgm(name);
        }

        public void CrossFade(string name, int time)
        {
            if (bgmPos[name].x != bgmPos[currentScene].x + interval)
            {
                bgmPos[name] = DX.VGet(bgmPos[currentScene].x + interval, 0.0f, 0.0f);
                r = ListenerPos.z - bgmPos[currentScene].z;
                fadeDegre = 90 / (time / interval * r);
            }
            if (ListenerPos.z != 0)
            {
                if (ListenerPos.z > 0)
                {
                    degree += fadeDegre;
                    radian = (90 + degree) * (float)Math.PI / 180;
                    ListenerPos.z = r * (float)Math.Sin(radian) + bgmPos[currentScene].z;
                }
                if (ListenerPos.z < 0)
                    ListenerPos.z = 0;
            }
            if (ListenerPos.x < bgmPos[name].x)
            {
                ListenerPos.x += interval / time;
                DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);
            }
            else if (ListenerPos.x > bgmPos[name].x)
            {
                ListenerPos.x = bgmPos[name].x;
            }
            PlayBgm(currentScene);
            PlayBgm(name);
        }

        //void BgmMove(string name)//検証用
        //{
        //    bgmPos[name] = DX.VGet(bgmPos[name].x - fadeSpeed, bgmPos[name].y, bgmPos[name].z);
        //    DX.Set3DPositionSoundMem(bgmPos[name], bgmMap[name]);
        //    PlayBgm(name);
        //}

        void PlayBgm(string name)
        {
            if (playOn)
            {
                //BGMの当たり判定、斜め移動してないのでとりあえず四角形で計算
                if (!CheckPlayBgm(name) &&
                    bgmPos[name].x - bgmDis - 5 <= ListenerPos.x && ListenerPos.x <= bgmPos[name].x + bgmDis + 5 &&
                    bgmPos[name].z - bgmDis - 5 <= ListenerPos.z && ListenerPos.z <= bgmPos[name].z + bgmDis + 5)//再生してない、範囲内の時
                {
                    bgmMap[name] = ResourceLoader.GetSound3D(name + "_BGM.wav");//読み込み
                    DX.Set3DRadiusSoundMem(bgmDis, bgmMap[name]);
                    DX.Set3DPositionSoundMem(bgmPos[name], bgmMap[name]);
                    DX.PlaySoundMem(bgmMap[name], DX.DX_PLAYTYPE_LOOP);
                }
                else if (CheckPlayBgm(name) &&
                    (ListenerPos.x <= bgmPos[name].x - bgmDis - 10 || bgmPos[name].x + bgmDis + 10 <= ListenerPos.x ||
                     ListenerPos.z <= bgmPos[name].z - bgmDis - 10 || bgmPos[name].z + bgmDis + 10 <= ListenerPos.z))//再生中、範囲外の時
                {
                    ResourceLoader.RemoveSound(name + "_BGM.wav");//消去
                    DX.DeleteSoundMem(bgmMap[name]);
                }
            }
            else if (CheckPlayBgm(name))
            {
                ResourceLoader.RemoveSound(name + "_BGM.wav");//消去
                DX.DeleteSoundMem(bgmMap[name]);
            }
        }

        public bool CheckPlayBgm (string name)//鳴っていたらtrue
        {
            if(DX.CheckSoundMem(bgmMap[name])==1)
                return true;
            return false;
        }

        //public void Remove(string name)//強制停止用
        //{
        //    ResourceLoader.RemoveSound(name + "_BGM.wav");//消去
        //    DX.DeleteSoundMem(bgmMap[name]);
        //}

        public void AllRemove()
        {
            for (int i = 0; i < bgmName.Length; i++)
            {
                ResourceLoader.RemoveSound(bgmName[i] + "_BGM.wav");
                DX.DeleteSoundMem(bgmMap[bgmName[i]]);
            }
        }

        public void Debug()
        {//フェードの可視化、1で再生中,0で停止中,-1でメモリにない(エラー)
            DX.DrawString(0, 15, "　タイトル:" + DX.CheckSoundMem(bgmMap["title"]) +
                                 "　プレイ:"   + DX.CheckSoundMem(bgmMap["play"]) +
                                 "　リザルト:" + DX.CheckSoundMem(bgmMap["result"]) +
                                 "　リスナー x:" + ListenerPos.x + "　z:" + ListenerPos.z, DX.GetColor(255, 0, 0));
            // BGMの再生位置を描画
            //タイトル,赤
            DX.DrawCircle(100 + (int)bgmPos["title"].x, 150 + (int)bgmPos["title"].z, (int)bgmDis, DX.GetColor(255, 0, 0), DX.FALSE);
            //プレイ,黄
            DX.DrawCircle(100 + (int)bgmPos["play"].x, 150 + (int)bgmPos["play"].z, (int)bgmDis, DX.GetColor(255, 255, 0), DX.FALSE);
            //リザルト,紫
            DX.DrawCircle(100 + (int)bgmPos["result"].x, 150 + (int)bgmPos["result"].z, (int)bgmDis, DX.GetColor(255, 0, 255), DX.FALSE);
            //リスナー,青
            DX.DrawCircle(100 + (int)ListenerPos.x, 150 + (int)ListenerPos.z, 2, DX.GetColor(0, 0, 255));
        }
    }
}