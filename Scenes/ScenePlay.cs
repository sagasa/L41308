using System;

namespace Giraffe
{
    public class ScenePlay : Scene
    {

        private Player player;
        private Leaf leaf;



        
        public ScenePlay(Game game) : base(game)
        {
            player = new Player(this);
            leaf = new Leaf(this);
        }


        public override void Draw()
        {
            player.Draw();
            leaf.Draw();
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
            leaf.Update();
        }
    }
}