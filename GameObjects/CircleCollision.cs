using SAGASALib;

namespace Giraffe
{
    public class CircleCollision
    {
        public CircleCollision(Vec2f pos, float rad)
        {
            this.pos = pos;
            radius = rad;
        }
        public Vec2f pos;
        public float radius;
    }
}