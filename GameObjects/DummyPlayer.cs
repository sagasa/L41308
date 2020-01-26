using SAGASALib;

namespace Giraffe
{
    //見た目だけプレイヤー
    public class DummyPlayer:GameObject
    {
        public readonly PlayerRender Render;

        public readonly AnimationManager<PlayerRender> AnimationManager;

        private float Spin = 0.1f;
        int WalkSETime =60;
        private bool WalkSEFlag=false;

        public DummyPlayer(Scene scene):base(scene)
        {
            Render = new PlayerRender(this);
            AnimationManager = new AnimationManager<PlayerRender>(Render);
        }

        public override void Draw()
        {
            AnimationManager.Update();
            Render.Draw();
           base.Draw();
        }

        public override bool IsDead()
        {
            return false;
        }

        public override void OnInteract(GameObject obj, float extend)
        {
            ;
        }
        public override void Update()
        {
            if (Input.LEFT.IsPush())
            {
                vel = vel.SetX(MyMath.Lerp(vel.X, -Spin, 0.1f));
                Sound.Play("step_SE.mp3");
                WalkSEFlag = true;
            }
            if (Input.RIGHT.IsPush())
            {
                vel = vel.SetX(MyMath.Lerp(vel.X, Spin, 0.1f));
                Sound.Play("step_SE.mp3");
                WalkSEFlag = true;
            }
            if (WalkSEFlag==true)
            { WalkSETime--;
                
                if (WalkSETime<=0)
                {
                    Sound.Play("step_SE.mp3");
                    WalkSEFlag = false;
                    WalkSETime = 60;
                }
            }
            AnimationManager.Start(Animations.StandAngle);
            AnimationManager.StartNext(Animations.StandAngle, Animations.WalkGround);
            AnimationManager.Start(Animations.MouthClose);
            //首が！？  
            base.Update();
        }
    }
}