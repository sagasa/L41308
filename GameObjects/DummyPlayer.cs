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
        public bool isDunnyRight = false;
        public bool isDummyNeck = false;


        public DummyPlayer(Scene scene):base(scene)
        {
            Render = new PlayerRender(this);
            AnimationManager = new AnimationManager<PlayerRender>(Render);
            AnimationManager.Start(Animations.StandAngle);
            AnimationManager.StartNext(Animations.StandAngle, Animations.WalkGround);
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
            if (isDummyNeck==true)
            {
                AnimationManager.Start(Animations.DefaultAngle);

                
                if (Input.UP.IsPush()&& Render.NeckExt < 7.5f)
                {
                    Render.NeckExt +=2.85f;
                    if (!Sound.CheckPlaySound("neck_up_SE.mp3"))
                        Sound.Play("neck_up_SE.mp3");
                }

                if (Input.DOWN.IsPush() && 2.0f <=Render.NeckExt)
                {
                    Render.NeckExt -=2.85f;
                    if (!Sound.CheckPlaySound("neck_down_SE.mp3"))
                        Sound.Play("neck_down_SE.mp3");
                }
            }
            //if (Input.LEFT.IsPush() || Input.RIGHT.IsPush())
            //{
            //    Sound.Play("step_SE.mp3");
            //}
        }
    }
}