using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using WindowsGame1.Common;

namespace WindowsGame1.Units
{
    class UnitFactory
    {
        public static Unit New(UnitsEnum unit, UnitData data)
        {
            switch (unit)
            {
                case UnitsEnum.ADELAIDE:
                    data.UnitName = UnitsEnum.ADELAIDE;
                    data.Texture = Game1.Textures[UnitsEnum.ADELAIDE];
                    data.MaxVelocity = 20;
                    data.Mass = 1000;
                    data.MaxForce = 1500;
                    data.CollisionDistance = 75;
                    data.Height = 0.3f;
                    data.Destination.Add(new UnitData{Position = new Vector2(100, 300)});
                    data.Destination.Add(new UnitData { Position = new Vector2(800, 400) });
                    data.Destination.Add(new UnitData { Position = new Vector2(400, 100) });

                    var comp1 = new UnitData
                    {
                        UnitName = UnitsEnum.TURRET,
                        Texture = Game1.Textures[UnitsEnum.TURRET],
                        MaxVelocity = 0,
                        Mass = 50,
                        MaxForce = 100,
                        FacingAngle = (float)Math.PI / 2,
                        Height = 0.2f,
                        Position = new Vector2(0, -90)
                    };

                    var comp2 = new UnitData
                    {
                        UnitName = UnitsEnum.DEFAULT,
                        Position = new Vector2(0, -60),
                        Height = 0.2f,
                        Timeout = 2500,
                    };
                    comp2.Timer.Start();

                    data.Components = new List<Unit>();
                    data.Components.Add(new Unit(comp1));
                    data.Components.Add(new Unit(comp2));

                    return new Unit(data);

                case UnitsEnum.RAPTOR:
                    data.UnitName = UnitsEnum.RAPTOR;
                    data.Texture = Game1.Textures[UnitsEnum.RAPTOR];
                    //data.MaxVelocity = 15;
                    //data.Mass = 20;
                    //data.MaxForce = 7;
                    //data.CollisionDistance = 30;
                    //data.Height = 0;
                    //data.MinimumSpeed = 4;
                    data.MaxVelocity = 5;
                    data.Mass = 6.6f;
                    data.MaxForce = 2.3f;
                    data.CollisionDistance = 30;
                    data.MinimumSpeed = 1.3f;
                    return new Unit(data);

                case UnitsEnum.MISSILE:
                    data.UnitName = UnitsEnum.MISSILE;
                    data.Texture = Game1.Textures[UnitsEnum.MISSILE];
                    data.MaxVelocity = 6;
                    data.Mass = 1;
                    data.MaxForce = 1;
                    data.CollisionDistance = 15;
                    data.MinimumSpeed = 2;
                    return new Unit(data);
                default:
                    return null;
            }
        }
    }
}
