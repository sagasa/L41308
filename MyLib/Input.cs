using System;
using System.Collections.Generic;
using System.Diagnostics;
using DxLibDLL;

namespace SAGASALib
{
    // 入力クラス
    public static class Input
    {
        public static readonly InputBind UP;
        public static readonly InputBind DOWN;
        public static readonly InputBind RIGHT;
        public static readonly InputBind LEFT;

        public static readonly InputBind ACTION;

        //ダブルクリックのタイムアウト(ms
        internal const int CLICK_TIMEOUT = 300;

        internal static List<InputBind> binds = new List<InputBind>();

        static Input()
        {
            binds.Clear();
            UP = new InputBind(DX.KEY_INPUT_UP);
            DOWN = new InputBind(DX.KEY_INPUT_DOWN);
            RIGHT = new InputBind(DX.KEY_INPUT_RIGHT);
            LEFT = new InputBind(DX.KEY_INPUT_LEFT);
            ACTION = new InputBind(DX.KEY_INPUT_SPACE);
        }

        // 最新の入力状況に更新する処理。
        // 毎フレームの最初に（ゲームの処理より先に）呼んでください。
        public static void Update()
        {
            int time = GetTimeMills();
            binds.ForEach(bind =>
            {
                bind.lastKey = bind.nowKey;
                bind.nowKey = DX.CheckHitKey(bind.keyCode) != 0;
                
                if (bind.nowKey)
                {
                    if (!bind.lastKey)
                    {
                        bind.pushStartTime = time;
                        if (time - bind.lastClickTime < CLICK_TIMEOUT)
                            bind.keyCount++;
                    }
                }
                else
                {
                    //最終入力
                    if (bind.lastKey)
                        bind.lastClickTime = time;
                    else
                    {
                        bind.pushStartTime = 0;
                    }
                    if (time - bind.lastClickTime >= CLICK_TIMEOUT)
                        bind.keyCount = 0;
                }
            });
        }

        private static int GetTimeMills()
        {
            return (int)(Stopwatch.GetTimestamp() / TimeSpan.TicksPerMillisecond);
        }

        public class InputBind
        {
            internal InputBind(int key)
            {
                keyCode = key;
                Input.binds.Add(this);
            }
            internal readonly int keyCode;
            internal bool nowKey = false;
            internal bool lastKey = false;
            internal int keyCount = 0;
            internal int lastClickTime = 0;
            internal int pushStartTime = 0;

            public bool IsHold()
            {
                return nowKey;
            }
            public bool IsPush()
            {
                return !lastKey&&nowKey;
            }
            public  bool IsRelease()
            {
                return lastKey&&!nowKey;
            }
            public int GetLastClickTime()
            {
                return lastClickTime;
            }
            public int GetPushTime()
            {
                return pushStartTime <= 0 ? 0 : GetTimeMills() - pushStartTime;
            }
            public int GetClickCount()
            {
                return keyCount;
            }
        }
    }
}