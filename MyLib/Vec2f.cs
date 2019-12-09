﻿using System;

namespace SAGASALib
{
    public class Vec2f
    {
        public readonly float X;
        public readonly float Y;

        public static readonly Vec2f ZERO = new Vec2f(0,0);

        public Vec2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length()
        {
            return (float) Math.Sqrt(X * X + Y * Y);
        }
        public Vec2f Rotate(float angle)
        {
            return new Vec2f(X * (float)Math.Cos(angle) - Y * (float)Math.Sin(angle), X * (float)Math.Sin(angle) + Y * (float)Math.Cos(angle));
        }
        public Vec2f Normal()
        {
            return this/Length();
        }

        public Vec2f Right()
        {
            return new Vec2f(Y,-X);
        }

        public Vec2f Left()
        {
            return new Vec2f(-Y, X);
        }

        public static Vec2f operator +(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.X + b.X, a.Y + b.Y);
        }
        public static Vec2f operator -(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.X - b.X, a.Y - b.Y);
        }
        public static Vec2f operator *(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.X * b.X, a.Y * b.Y);
        }
        public static Vec2f operator /(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.X / b.X, a.Y / b.Y);
        }
        public static Vec2f operator *(Vec2f a, float b)
        {
            return new Vec2f(a.X * b, a.Y * b);
        }
        public static Vec2f operator /(Vec2f a, float b)
        {
            return new Vec2f(a.X / b, a.Y / b);
        }

        public override string ToString()
        {
            return "Vec2f[x="+X+",y="+Y+"]";
        }
    }
}