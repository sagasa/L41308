using SAGASALib;

namespace Giraffe
{
    //見た目だけプレイヤー
    public class DummyPlayer:GameObject
    {
        public readonly PlayerRender Render;

        public readonly AnimationManager<PlayerRender> AnimationManager;

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
    }
}