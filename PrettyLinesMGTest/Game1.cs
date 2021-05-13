using System;
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
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private LineFactory factory;

        private List<Base2DLine> lines;

        private Thick2DLine mouse;

        private int wheel;

        private SpriteFont font;

        private Matrix transform;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            factory = new LineFactory(graphics.GraphicsDevice);

            lines = new List<Base2DLine>();

            lines.Add(factory.GetSimpleLine(new Vector2(100, 100), new Vector2(200, 200), Color.Red, "Label"));
            lines.Add(factory.GetThickLine(new Vector2(200, 200), new Vector2(300, 100), Color.Red, 5, "Label"));
            lines.Add(factory.GetBezierCurve(new Vector2(200, 300), new Vector2(100, 200), Color.Red, 5, "Label"));
            lines.Add(factory.GetBezierCurve(new Vector2(200, 300), new Vector2(300, 200), Color.Red, 5, "Label"));

            mouse = factory.GetBezierCurve(
                new Vector2(graphics.PreferredBackBufferWidth * 0.5f, graphics.PreferredBackBufferHeight * 0.5f),
                Vector2.Zero, Color.Gray, 5, "Label");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            font = this.Content.Load<SpriteFont>("font");
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
                        mouse = factory.GetThickLine(mouse.Start, mouse.End, mouse.Color, mouse.Thickness, "Label");
                        break;
                    case Keys.NumPad2:
                        mouse = factory.GetAngledLine(mouse.Start, mouse.End, mouse.Color, mouse.Thickness, "Label");
                        break;
                    case Keys.NumPad3:
                        mouse = factory.GetBezierCurve(mouse.Start, mouse.End, mouse.Color, mouse.Thickness, "Label");
                        break;
                }
            }

            var mouseState = Mouse.GetState();
            mouse.End = new Vector2(mouseState.X, mouseState.Y);

            if (wheel != mouseState.ScrollWheelValue)
            {
                mouse.Thickness = wheel < mouseState.ScrollWheelValue ? mouse.Thickness + 1 : mouse.Thickness - 1;
            }

            wheel = mouseState.ScrollWheelValue;

            var translate = Matrix.CreateTranslation(300, 300, 0);

            transform = Matrix.Invert(translate) *
                        Matrix.CreateRotationZ((float)(0.2f * Math.PI * gameTime.TotalGameTime.TotalSeconds)) *
                        translate;

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

            foreach (var line in lines)
            {
                line.Draw(Matrix.Identity);
            }

            mouse.Draw(Matrix.Identity);

            spriteBatch.Begin();

            foreach (var line in lines)
            {
                line.DrawLabel(font, Color.White);
            }

            spriteBatch.End();

            mouse.DrawLabel(font, Color.White);

            base.Draw(gameTime);
        }
    }
}