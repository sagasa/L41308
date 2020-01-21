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

        //再生中のアニメーションを停止する
        public void Stop(AnimationEntry<T> entry, bool finish)
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

        //アニメーションを再生中か?
        public bool IsPlaying(AnimationEntry<T> entry)=> _list.Any(animation => animation.Entry==entry);

        //アニメーションを適応
        public void Update()
        {
            _list.ForEach(animation => animation.Update(_target));
            _list.RemoveAll(animation => animation.IsEnd());
        }
    }
}