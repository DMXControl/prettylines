using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrettyLinesLib;

namespace PrettyLines
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private LineFactory _factory;

        private List<I2DLine> _lines;

        private IThick2DLine _mouse;

        private int _wheel;

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

            _factory = new LineFactory(_graphics);

            _lines = new List<I2DLine>();

            _lines.Add(_factory.GetSimpleLine(new Vector2(100, 100), new Vector2(200, 200), Color.Red));
            _lines.Add(_factory.GetThickLine(new Vector2(200, 200), new Vector2(300, 100), Color.Red, 5));
            _lines.Add(_factory.GetBezierCurve(new Vector2(200, 300), new Vector2(100, 200), Color.Red, 5));
            _lines.Add(_factory.GetBezierCurve(new Vector2(200, 300), new Vector2(300, 200), Color.Red, 5));

            _mouse = _factory.GetBezierCurve(
                new Vector2(_graphics.PreferredBackBufferWidth * 0.5f, _graphics.PreferredBackBufferHeight * 0.5f),
                Vector2.Zero, Color.Gray, 5);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here

            var state = Keyboard.GetState();

            foreach (var key in state.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.NumPad1:
                        _mouse = _factory.GetThickLine(_mouse.Start, _mouse.End, _mouse.Color, _mouse.Thickness);
                        break;
                    case Keys.NumPad2:
                        _mouse = _factory.GetAngledLine(_mouse.Start, _mouse.End, _mouse.Color, _mouse.Thickness);
                        break;
                    case Keys.NumPad3:
                        _mouse = _factory.GetBezierCurve(_mouse.Start, _mouse.End, _mouse.Color, _mouse.Thickness);
                        break;
                }
            }

            var mouseState = Mouse.GetState();
            _mouse.End = new Vector2(mouseState.X, mouseState.Y);

            if (_wheel != mouseState.ScrollWheelValue)
            {
                _mouse.Thickness = _wheel < mouseState.ScrollWheelValue ? _mouse.Thickness + 1 : _mouse.Thickness - 1;
            }

            _wheel = mouseState.ScrollWheelValue;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            foreach (var line in _lines)
            {
                line.Draw();
            }

            _mouse.Draw();

            base.Draw(gameTime);
        }
    }
}
