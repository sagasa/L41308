using System;
using System.Collections.Generic;

namespace SAGASALib
{
    public class AnimationManager<T>
    {
        //アニメーションリスト
        private List<Animation<T>> _list = new List<Animation<T>>();

        //実行対象
        private readonly T _target;
        
        public AnimationManager(T target)
        {
            _target = target;
        }

        //アニメーションを再生する
        public void Start(Animation<T> animation)
        {
            if (!_list.Contains(animation))
            {
                animation.Init(_target);
                _list.Add(animation);
            }
            else
                Console.Error.WriteLine("再生中のアニメーションが追加されました");
        }

        //再生中のアニメーションを停止する
        public void Stop(Animation<T> animation,bool finish)
        {

        }

        //アニメーションを適応
        public void Update()
        {

        }
    }
}