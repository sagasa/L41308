using System;

namespace Giraffe
{
    public class ScenePlay : Scene
    {

        private Player player;
        public ScenePlay(Game game) : base(game)
        {
            player = new Player(this);
        }


        public override void Draw()
        {
            player.Draw();
        }

        public override void OnExit()
        {
        }

        public override void OnLoad()
        {
        }

        public override void Update()
        {
            player.Update();
        }
    }
}