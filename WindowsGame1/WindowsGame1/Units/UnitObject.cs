using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.Common;

namespace WindowsGame1.Units
{
    public abstract class UnitObject : Game
    {
        internal UnitData Data;

        //Maps friction
        public float Friction = 0.98f;

        //private readonly Texture2D _pixel;

        protected UnitObject(UnitData data)//, Texture2D pixel)
        {
            Data = data;

            //_pixel = pixel;
        }

        public void Move()
        {
            //Minumum airspeed
            if (Data.MinimumSpeed > 0 && Data.Velocity.Length() < Data.MinimumSpeed)
                Data.Velocity *= Data.MinimumSpeed;

            //If the unit has queued destinations then caluculate movement
            if (Data.Destination.Count > 0)
            {
                //If we fail collision detection, keep moving
                var curDest = Data.Destination[0];
                //if (Vector2.Distance(Data.Position, curDest.Position) >= Data.CollisionDistance)
                if (!Data.Collision(curDest.Position))
                {
                    switch (Data.MovementMode)
                    {
                        case MovementMode.SEEK:
                            Seek(curDest);
                            break;
                        case MovementMode.INTERCEPT:
                            Intercept(curDest);
                            break;
                        case MovementMode.PATROL:
                            Seek(curDest);
                            break;
                    }
                }
                else
                {
                    Data.Destination.RemoveAt(0);
                    if (Data.MovementMode == MovementMode.PATROL)
                        Data.Destination.Add(curDest);
                }
            }
            //Incorporate the tiles friction
            Data.Velocity *= Friction;
            Data.Position = Data.Position + Data.Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //calculate the offset for texture to the position origin
            //calculate amount of movement as a fraction of elapsed time

            spriteBatch.Draw(Data.Texture, Data.Position, null, Color.White, Data.FacingAngle,
                             Data.Texture.Origin(), 1.0f, SpriteEffects.None, Data.Height);

            foreach (var comp in Data.Components)
            {
                //If the component hasn't a facing direction set, then just point forward
                //comp.Data.FacingAngle = comp.Data.Destination.Count > 0 ?
                //                        comp.Data.Destination[0].Angle() : Data.FacingAngle;

                //Calculate where to draw the component, in relation to the unit
                if (comp.Data.Texture != null)
                {
                    var compPosition = Data.Position + comp.Data.Position.Rotate(Data.FacingAngle);

                    spriteBatch.Draw(comp.Data.Texture, compPosition, null, Color.White,
                        comp.Data.FacingAngle, comp.Data.Texture.Origin(),
                        1.0f, SpriteEffects.None, comp.Data.Height);
                }

                if ((comp.Data.UnitName == UnitsEnum.DEFAULT) && 
                    (comp.Data.Timer.ElapsedMilliseconds > comp.Data.Timeout))
                {
                    var t = UnitFactory.New(UnitsEnum.MISSILE, new UnitData
                    {
                        Position = Data.Position + comp.Data.Position.Rotate(Data.FacingAngle),
                        MovementMode = MovementMode.INTERCEPT,
                        Timeout = 2000,
                        Timer = new System.Diagnostics.Stopwatch(),
                    });
                    t.Data.Timer.Start();
                    comp.Data.Timer.Restart();
                    Game1.AddUnit(t);
                }
            }

            //if (Destination.Count > 0)
            //{
            //    spriteBatch.DrawLine(_pixel, Position, Destination[0], Game1.SelectColor);
            //    spriteBatch.DrawLine(_pixel, Position, Steering, Color.Blue);
            //    spriteBatch.DrawLine(_pixel, Position, Velocity, Color.Red);
            //}
        }

        //http://gamedevelopment.tutsplus.com/series/understanding-steering-behaviors--gamedev-12732
        //Flee + arrival
        //Roam
        //Pursue + evade

        //Move towards a point
        //private void Seek(Vector2 target)
        private void Seek(UnitData target)
        {
            //subtract the position from the target to get the vector from the vehicles position to the target. 
            var directionVector = (target.Position - Data.Position)/* / Data.MaxVelocity*/;

            //Set the facingAngle (used to draw the image, in radians) to velocity
            Data.FacingAngle = Data.Velocity.Angle();

            //Normalize it then multiply by max speed to get the maximum velocity from your position to the target.
            var desiredVelocity = directionVector.Truncate(Data.MaxVelocity);

            //subtract velocity from the desired velocity to get the force vector
            Data.Steering = desiredVelocity - Data.Velocity;

            //divide the steeringForce by the mass(which makes it the acceleration), 
            Data.Steering = (Data.Steering.Truncate(Data.MaxForce)) / Data.Mass;

            //then add it to velocity to get the new velocity
            Data.Velocity = (Data.Velocity + Data.Steering).Truncate(Data.MaxVelocity);

            Data.Position += Data.Velocity;
        }

        private void Intercept(UnitData target)
        {
            //Fomd the distance to target
            var distance = target.Position - Data.Position;

            //Find how many frames the target needs to moves untill it's at pursuit position
            var T = distance.Length()/target.MaxVelocity;

            //Set the pursuit position to the targets next position
            var futurePosition = target.Position + target.Velocity * T;

            //Then move towards this future point
            Seek(new UnitData {Position = futurePosition});
        }
    }
}
