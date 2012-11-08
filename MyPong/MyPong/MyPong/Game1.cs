using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyPong {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        #region Variables
        
        Ball ball;
        Paddle paddle1;
        Paddle paddle2;
        Entity stripe;
        
        int player1Score = 0;
        int player2Score = 0;

        #endregion /* Variables */

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            // Set the coordinates to draw the sprite at.
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load our ball & paddles.
            ball = new Ball(Content.Load<Texture2D>("ball"), new Vector2(400, 0));
            paddle1 = new Paddle(Content.Load<Texture2D>("paddle1"), new Vector2(30, 190));
            paddle2 = new Paddle(Content.Load<Texture2D>("paddle2"), new Vector2(760, 190));
            stripe = new Entity(Content.Load<Texture2D>("stripe"), new Vector2(395, 0));
            
            // Load our font for displaying the scoreboard.
            font = Content.Load<SpriteFont>("SpriteFont1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                this.Exit();
            }

            // These variables specify the boundaries of our game window.
            int MaxX = graphics.GraphicsDevice.Viewport.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height;
            int MinY = 0;
            
            KeyboardState keyboardState = Keyboard.GetState();

            #region Paddle1 & Paddle2 Controls
            // Keyboard input controls for paddle1
            paddle1.PaddleMove(keyboardState, Keys.Up, Keys.Down);
            
            // Keyboard input controls for paddle2
            paddle2.PaddleMove(keyboardState, Keys.NumPad5, Keys.NumPad2);
            paddle2.PaddleAI(ball);
#if DEBUG
            if (keyboardState.IsKeyDown(Keys.Left)) {
                paddle1.Move(-5.0f, 0.0f);
            }

            if (keyboardState.IsKeyDown(Keys.Right)) {
                paddle1.Move(5.0f, 0.0f);
            }
#endif
            #endregion /* Paddle1 & Paddle2 Controls */

            ball.Update(gameTime);
            paddle1.Update(gameTime);
            paddle2.Update(gameTime);

            #region Ball Speed, Top & Bottom Bouncing, & Score Keeping
            // This will keep track of when a player scores a point by hitting the ball into the other player's goal zone.
            if (ball.Position.X > MaxX) {
                player1Score += 1;
                ball.Set(paddle1.Position.X + paddle1.Width, ball.Position.Y);
            }

            else if (ball.Position.X < MinX) {
                player2Score += 1;
                ball.Set(paddle2.Position.X - paddle2.Width - ball.Width, ball.Position.Y);
            }

            // The following keeps the ball & paddles from falling off of the top & bottom margins of the game window,
            // making it bounce off of the window's top & bottom edges instead.
            ball.Clip(MinX, MaxX, MinY, MaxY);
            paddle1.Clip(MinX, MaxX, MinY, MaxY);
            paddle2.Clip(MinX, MaxX, MinY, MaxY);

            #endregion /* Ball Speed, Top & Bottom Bouncing, & Score Keeping */

            #region Paddle & Ball Interactions

            ball.ClipPaddle(paddle1, keyboardState, Keys.Up, Keys.Down);
            ball.ClipPaddle(paddle2, keyboardState, Keys.NumPad5, Keys.NumPad2);

            #endregion /* Paddle & Ball Interactions */

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            graphics.GraphicsDevice.Clear(Color.Green);

            // Draw the sprites & scoreboard.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            stripe.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            spriteBatch.DrawString(font, "P1: " + player1Score, new Vector2(200, 30), Color.White);
            spriteBatch.DrawString(font, "P2: " + player2Score, new Vector2(600, 30), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
            Update(gameTime);
        }
    }
}