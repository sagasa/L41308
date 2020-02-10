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

        private const int bgmRange = 100;//BGMの聞こえる範囲
        private const int interval = 150;//BGM同士の距離,クロスフェードで使用
        
        string oldScene = "null";
        string newScene = "title";
        int fadeTime = 60;

        public void Load()
        {
            ListenerPos = DX.VGet(0, 1, 0);//リスナーの位置
            ListenerDir = DX.VGet(0, -1, 0);//リスナーの向き
            DX.Set3DSoundListenerPosAndFrontPos_UpVecY(ListenerPos, ListenerDir);//向きと位置を設定

            for (int i = 0; i < bgmName.Length; i++)
            {
                bgmMap[bgmName[i]] = 0;
                bgmPos[bgmName[i]] = DX.VGet(0, 0, 0);
            }
        }

        public delegate void Update();
        public Update update;

        public void Set(int fadeTime_, string newScene_ = null, string oldScene_ = null)
        {
            fadeTime = fadeTime_;
            if (newScene_ != null)
                newScene = newScene_;
            if (oldScene_ != null)
                oldScene = oldScene_;
        }

        public void CrossFade()
        {
            if (!CheckPlayBgm(newScene))//初期化
            {
                bgmPos[newScene] = DX.VGet(interval, 0, 0);
                PlayBgm(newScene);
            }

            bgmPos[newScene] = DX.VAdd(bgmPos[newScene], DX.VGet(-interval / fadeTime, 0, 0));
            bgmPos[oldScene] = DX.VAdd(bgmPos[oldScene], DX.VGet(-interval / fadeTime, 0, 0));

            DX.Set3DPositionSoundMem(bgmPos[newScene], bgmMap[newScene]);
            DX.Set3DPositionSoundMem(bgmPos[oldScene], bgmMap[oldScene]);

            if (bgmPos[newScene].x <= ListenerPos.x + 1 && bgmPos[oldScene].x <= ListenerPos.x - bgmRange + 1)
            {
                bgmPos[newScene] = DX.VGet(0, 0, 0);
                DX.Set3DPositionSoundMem(bgmPos[newScene], bgmMap[newScene]);
                Remove(oldScene);
                update = Delegate.Remove(update, new Update(CrossFade)) as Update;
            }
        }

        public void FadeIn()
        {
            if (!CheckPlayBgm(newScene))//初期化
            {
                bgmPos[newScene] = DX.VGet(bgmRange, 0, 0);
                PlayBgm(newScene);
            }
            bgmPos[newScene] = DX.VAdd(bgmPos[newScene], DX.VGet(-bgmRange / fadeTime, 0, 0));
            DX.Set3DPositionSoundMem(bgmPos[newScene], bgmMap[newScene]);

            if (bgmPos[newScene].x <= ListenerPos.x + 1)
            {
                bgmPos[newScene] = DX.VGet(0, 0, 0);
                DX.Set3DPositionSoundMem(bgmPos[newScene], bgmMap[newScene]);
                update = Delegate.Remove(update, new Update(FadeIn)) as Update;
            }
        }

        public void FadeOut()
        {
            bgmPos[oldScene] = DX.VAdd(bgmPos[oldScene], DX.VGet(-bgmRange / fadeTime, 0, 0));
            DX.Set3DPositionSoundMem(bgmPos[oldScene], bgmMap[oldScene]);

            if (bgmPos[oldScene].x <= ListenerPos.x - bgmRange + 1)
            {
                Remove(oldScene);
                update = Delegate.Remove(update, new Update(FadeOut)) as Update;
            }
        }
        
        void PlayBgm(string name)
        {
            if (!CheckPlayBgm(name))
            {
                bgmMap[name] = ResourceLoader.GetSound3D(name + "_BGM.wav");
                DX.Set3DRadiusSoundMem(bgmRange, bgmMap[name]);
                DX.Set3DPositionSoundMem(bgmPos[name], bgmMap[name]);
                DX.PlaySoundMem(bgmMap[name], DX.DX_PLAYTYPE_LOOP);
            }
        }

        public bool CheckPlayBgm (string name)//鳴っていたらtrue
        {
            if (DX.CheckSoundMem(bgmMap[name]) == 1)
                return true;
            return false;
        }

        public void AllRemove()
        {
            for (int i = 0; i < bgmName.Length; i++)
            {
                Remove(bgmName[i]);
            }
        }

        void Remove(string name)
        {
            ResourceLoader.RemoveSound(name + "_BGM.wav");
            DX.DeleteSoundMem(bgmMap[name]);
        }

        void Stop(string name)
        {
            DX.StopSoundMem(bgmMap[name]);
        }
        
        public void Debug()
        {
            DX.DrawGraph(0, 0, ResourceLoader.GetGraph("bg/shadow25.png"));
            //フェードの可視化、1:再生中, 0:停止中, -1:メモリにない(エラー)
            DX.DrawString(0, 60, "　タ:" + DX.CheckSoundMem(bgmMap[bgmName[0]]) +
                                 "　プ:" + DX.CheckSoundMem(bgmMap[bgmName[1]]) +
                                 "　リ:" + DX.CheckSoundMem(bgmMap[bgmName[2]]) +
                               "　チュ:" + DX.CheckSoundMem(bgmMap[bgmName[3]]), DX.GetColor(255, 255, 255));
            DX.DrawString(0, 100, " タx:" + bgmPos[bgmName[0]].x +
                                  " プx:" + bgmPos[bgmName[1]].x +
                                  " リx:" + bgmPos[bgmName[2]].x +
                                " チュx:" + bgmPos[bgmName[3]].x, DX.GetColor(255, 255, 255));

            // BGMの再生位置を描画
            //タイトル,赤
            DX.DrawCircle(Screen.Width / 2 + (int)bgmPos[bgmName[0]].x, Screen.Height / 2 + (int)bgmPos[bgmName[0]].z, bgmRange, DX.GetColor(255, 0, 0), DX.FALSE);
            //プレイ,黄
            DX.DrawCircle(Screen.Width / 2 + (int)bgmPos[bgmName[1]].x, Screen.Height / 2 + (int)bgmPos[bgmName[1]].z, bgmRange, DX.GetColor(255, 255, 0), DX.FALSE);
            //リザルト,紫
            DX.DrawCircle(Screen.Width / 2 + (int)bgmPos[bgmName[2]].x, Screen.Height / 2 + (int)bgmPos[bgmName[2]].z, bgmRange, DX.GetColor(255, 0, 255), DX.FALSE);
            //チュートリアル,白
            DX.DrawCircle(Screen.Width / 2 + (int)bgmPos[bgmName[3]].x, Screen.Height / 2 + (int)bgmPos[bgmName[3]].z, bgmRange, DX.GetColor(255, 255, 255), DX.FALSE);
            //リスナー,青
            DX.DrawCircle(Screen.Width / 2 + (int)ListenerPos.x, Screen.Height / 2 + (int)ListenerPos.z, 2, DX.GetColor(0, 0, 255));
        }

    }
}