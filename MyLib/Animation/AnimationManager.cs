using System;
using System.Collections.Generic;
using System.Linq;

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
        public void Start(AnimationEntry<T> entry)
        {
            //同じEntryから再生していなければ
            if (_list.All(animation => animation.Entry!= entry))
            {
                Animation<T> animation = new Animation<T>(entry);
                animation.Init(_target);
                _list.Add(animation);
            }
            else
                Console.Error.WriteLine("再生中のアニメーションが追加されました");
        }

        public void StartNext(AnimationEntry<T> before, AnimationEntry<T> next)
        {
            //要素を検索
            Animation<T> beforeAnimation = _list.FirstOrDefault(anim => anim.Entry == before);
            if (beforeAnimation != null)
            {
                beforeAnimation.NextAnimation = next;
            }
            else
                Console.Error.WriteLine("接続対象のアニメーションが見つかりません");
        }

        //再生中のアニメーションを停止する
        public void Stop(AnimationEntry<T> entry, bool finish=false)
        {
            //要素を検索
            Animation<T> animation = _list.FirstOrDefault(anim => anim.Entry == entry);
            if (animation != null)
            {
                if (finish)
                    animation.Finish(_target);
                _list.Remove(animation);
            }
        }

        public void StopAll(bool finish = false)
        {   
            _list.ForEach(value=>value.Finish(_target));
            _list.Clear();
        }

        //アニメーションを再生中か?
        public bool IsPlaying(AnimationEntry<T> entry)=> _list.Any(animation => animation.Entry==entry);

        //アニメーションを適応
        public void Update()
        {
            _list.ForEach(animation => animation.Update(_target));
            for (var i = _list.Count - 1; i >= 0; i--)
            {
                if (_list[i].IsEnd())
                {
                    if (_list[i].NextAnimation != null)
                        Start(_list[i].NextAnimation);
                    _list.RemoveAt(i);
                }
            }
        }
    }
}