using System;
using System.Collections.Generic;
using System.Timers;
using WindowsGame1.Core;
using WindowsGame1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.Common;
using WindowsGame1.Units;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        public static readonly Dictionary<UnitsEnum, Texture2D> Textures = new Dictionary<UnitsEnum, Texture2D>();
        private static List<UnitObject> _units = new List<UnitObject>();
        List<Particle> _particles = new List<Particle>();
        private MouseState _lastMouseState;
        private Camera _camera;
        private Map _map;

        public static Color SelectColor = new Color(94, 218, 158);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _lastMouseState = Mouse.GetState();
            IsMouseVisible = true;
            Window.Title = "GameTest";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GraphicsDevice t = GraphicsDevice;
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the camera class
            _camera = new Camera(new Vector2(400, 300), 800, 600);

            // Check required resources exist

            var txPixel = new Texture2D(GraphicsDevice, 1, 1);
            txPixel.SetData(new[] { Color.White });

            //Content.RootDirectory = "Content";
            // load the map, get the map elements and load them
            Textures.Add(UnitsEnum.ADELAIDE, Content.Load<Texture2D>("Adelaide"));
            Textures.Add(UnitsEnum.TURRET, Content.Load<Texture2D>("Turret"));
            Textures.Add(UnitsEnum.RAPTOR, Content.Load<Texture2D>("Raptor"));
            Textures.Add(UnitsEnum.MISSILE, Content.Load<Texture2D>("Missile"));
            Textures.Add(UnitsEnum.CLOUD1, Content.Load<Texture2D>("Cloud11"));
            Textures.Add(UnitsEnum.CLOUD2, Content.Load<Texture2D>("Cloud15"));
            Textures.Add(UnitsEnum.CLOUD3, Content.Load<Texture2D>("Cloud3"));
            Textures.Add(UnitsEnum.WATER1, Content.Load<Texture2D>("Water1"));
            Textures.Add(UnitsEnum.WATERBUMP1, Content.Load<Texture2D>("WaterBump1"));

            // Load the map class
            _map = new Map(2, 2);
            _map.Wind = new Vector2(0.3f, 0.2f);
            _map.Tiles[0, 0] = new Tile(Textures[UnitsEnum.WATER1]) {Bumpmap = Textures[UnitsEnum.WATERBUMP1]};
            _map.Tiles[1, 0] = new Tile(Textures[UnitsEnum.WATER1]) {Bumpmap = Textures[UnitsEnum.WATERBUMP1]};
            _map.Tiles[0, 1] = new Tile(Textures[UnitsEnum.WATER1]) {Bumpmap = Textures[UnitsEnum.WATERBUMP1]};
            _map.Tiles[1, 1] = new Tile(Textures[UnitsEnum.WATER1]) {Bumpmap = Textures[UnitsEnum.WATERBUMP1]};
            _map.Tiles[0, 0].UpdateBumpmap();


            //Load units data
            var ud = new UnitData
            {
                Position = new Vector2(0, 0),
                //Destination = new List<UnitData> {new Vector2(150, 200)};
                Destination = new List<UnitData>{new UnitData{Position = new Vector2(10, 10)}}
            };

            var ud2 = new UnitData
            {
                Position = new Vector2(400, 250),
                //Destination = new List<Vector2> {new Vector2(200, 550)}
                //Destination = new List<UnitData> { new UnitData { Position = new Vector2(100, 55) } }
                MovementMode = MovementMode.PATROL,
            };

            //var ud3 = new UnitData
            //{
            //    Position = new Vector2(1000, 1000),
            //    MovementMode = MovementMode.INTERCEPT,
            //    Timeout = 2000,
            //    Timer = new System.Diagnostics.Stopwatch(),
            //};
            //ud3.Timer.Start();

            //Load each unit into memory
            //_units.Add(UnitFactory.New(UnitsEnum.ADELAIDE, ud));
            _units.Add(UnitFactory.New(UnitsEnum.RAPTOR, ud));
            _units.Add(UnitFactory.New(UnitsEnum.ADELAIDE, ud2));
            //_units.Add(UnitFactory.New(UnitsEnum.MISSILE, ud3));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            //Check for command updates
            Mouse.WindowHandle = Window.Handle;

            var mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //Check if left mouse clicked (draw a selection box or select unit)
            
            //Check for mouse click (if it was previously released and is now pressed
            if (_lastMouseState.RightButton == ButtonState.Released &&
                Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                //If given a single command, drop all queue items
                if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    _units[0].Data.Destination.Clear();

                //Add the queue item
                var position = mousePos + new Vector2(_camera.Viewport.X, _camera.Viewport.Y);
                _units[0].Data.Destination.Add(new UnitData {Position = position});
            }
            _lastMouseState = Mouse.GetState();

            var keyState = Keyboard.GetState();
            // Move the camera if the mouse is near the window bounds
            _camera.UpdateCamera(GraphicsDevice.Viewport.Bounds, mousePos, keyState);

            var rnd = new Random();
            //Update each unit
            foreach (var u in _units)
            {
                //if (u.Data.UnitName == UnitsEnum.RAPTOR && u.Data.Destination.Count < 1)
                //    u.Data.Destination.Add(_units[0].Data);

                //if (u.Data.UnitName == UnitsEnum.ADELAIDE)
                //    u.Data.Components[0].Data.FacingAngle = _units[1].Data.Position.Angle();

                if (u.Data.UnitName == UnitsEnum.MISSILE)
                {
                    u.Data.Destination.Insert(0, _units[0].Data);

                    _particles.Add(new Particle(u.Data.Position, Vector2.Zero,
                        Textures[(UnitsEnum) rnd.Next(5, 7)], 0.2f));
                }

                u.Move();
            }

            for (var p = _particles.Count - 1; p >= 0; p--)
            {
                _particles[p].Update();
                if (_particles[p].Expired)
                    _particles.RemoveAt(p);
            }

            //If your a missile, do timeout and refiring
            //if (_units[1].Data.Timer != null && _units[1].Data.Timer.ElapsedMilliseconds > _units[1].Data.Timeout)
            //{
            //    _units.Remove(_units[1]);
            //    var t = UnitFactory.New(UnitsEnum.MISSILE, new UnitData
            //    {
            //        Position = new Vector2(300, 300),
            //        MovementMode = MovementMode.INTERCEPT,
            //        Timeout = 2000,
            //        Timer = new System.Diagnostics.Stopwatch(),
            //    });
            //    t.Data.Timer.Start();
            //    _units.Add(t);
            //}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //if (gameTime.ElapsedGameTime)
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprite.
            //_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
                               SamplerState.PointClamp, DepthStencilState.None,
                               RasterizerState.CullNone, null, _camera.GetTransformations());

            _map.Draw(_spriteBatch);

            foreach (var u in _units.ToArray())
            {
                if (u.Data.UnitName == UnitsEnum.MISSILE && 
                   (u.Data.Timer.ElapsedMilliseconds > u.Data.Timeout || 
                    Vector2.Distance(u.Data.Position, _units[0].Data.Position) < u.Data.CollisionDistance))
                    _units.Remove(u);

                u.Draw(_spriteBatch);
            }

            foreach (var p in _particles)
            {
                p.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void AddUnit(UnitObject u)
        {
            _units.Add(u);
        }
    }
}
