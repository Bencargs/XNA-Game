using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using WindowsGame1.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Units
{
    public enum MovementMode
    {
        SEEK = 0,
        INTERCEPT = 1,
        EVADE = 2,
        ROAM = 3,
        PATROL = 4,
    }

    public class UnitData
    {
        public UnitsEnum UnitName;

        public float Height;

        // This is a texture we can render.
        public Texture2D Texture;

        //// Bounding box used for collision detection
        //public Rectangle BoundingBox;

        // Set the coordinates to draw the sprite at.
        internal float MaxVelocity;
        internal float Mass;
        internal float MaxForce;
        internal float MinimumSpeed;

        public MovementMode MovementMode;

        //Can be used for reloading or fuel etc..
        public Stopwatch Timer = new Stopwatch();
        public float Timeout;

        //Hit radius (for movement)
        internal int CollisionDistance;

        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Steering;

        //public List<Vector2> Destination = new List<Vector2>();
        public List<UnitData> Destination = new List<UnitData>();

        public float FacingAngle;

        //Weapons etc (turtles all the way down)
        public List<Unit> Components = new List<Unit>();
    }
}
