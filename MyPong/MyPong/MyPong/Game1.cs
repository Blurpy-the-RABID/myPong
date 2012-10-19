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

        Entity ball;
        Entity paddle1;
        Entity paddle2;
        /*        Texture2D ball;
                Texture2D paddle1;
                Texture2D paddle2;

                Vector2 ballPosition;
                Rectangle ballBR;
                Vector2 paddlePosition1;
                Rectangle paddle1BR;
                Vector2 paddlePosition2;
                Rectangle paddle2BR;
        */
        int player1Score = 0;
        int player2Score = 0;
        // Store some information about the sprite's motion.
        Vector2 ballSpeed = new Vector2(100.0f, 100.0f);

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
            ball = new Entity(Content.Load<Texture2D>("ball"), new Vector2(400, 0));
            paddle1 = new Entity(Content.Load<Texture2D>("paddle1"), new Vector2(0, 190));
            paddle2 = new Entity(Content.Load<Texture2D>("paddle2"), new Vector2(790, 190));
            
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

            ball.Update(gameTime);
            paddle1.Update(gameTime);
            paddle2.Update(gameTime);

            // These variables specify the boundaries of our game window.
            int MaxX = graphics.GraphicsDevice.Viewport.Width - ball.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - ball.Height;
            int MinY = 0;

            KeyboardState keyboardState = Keyboard.GetState();

            #region Paddle1 & Paddle2 Controls
            // Keyboard input controls for paddle1
            if (keyboardState.IsKeyDown(Keys.Up)) {
                paddle1.Move(0.0f, -5.0f);
            }

            if (keyboardState.IsKeyDown(Keys.Down)) {
                paddle1.Move(0.0f, 5.0f);
            }

            paddle1.Clip(MinX, MaxX, MinY, MaxY);
                        
            // Keyboard input controls for paddle2
            if (keyboardState.IsKeyDown(Keys.NumPad5)) {
                paddle2.Move(0.0f, -5.0f);
            }

            if (keyboardState.IsKeyDown(Keys.NumPad2)) {
                paddle2.Move(0.0f, 5.0f);
            }

            paddle2.Clip(MinX, MaxX, MinY, MaxY);

            #endregion /* Paddle1 & Paddle2 Controls */

            #region Ball Speed, Top & Bottom Bouncing, & Score Keeping
            // Move the sprite by speed, scaled by elapsed time.
            Vector2 P = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ball.Move(P.X, P.Y);

            // This will keep track of when a player scores a point by hitting the ball into the other player's goal zone.
            if (ball.Position.X > MaxX) {
                player1Score += 1;
                ball.Set(30, ball.Position.Y);
            }

            else if (ball.Position.X < MinX) {
                player2Score += 1;
                ball.Set(770, ball.Position.Y);
            }

            // The following keeps the ball from falling off of the top & bottom margins of the game window, making it bounce off of the window's top & bottom edges instead.
            if (ball.Position.Y > MaxY) {
                ballSpeed.Y *= -1;
                ball.Set(ball.Position.X, MaxY);
            }

            else if (ball.Position.Y < MinY) {
                ballSpeed.Y *= -1;
                ball.Set(ball.Position.X, MinY);
            }
            #endregion /* Ball Speed, Top & Bottom Bouncing, & Score Keeping */

            #region Paddle & Ball Interactions
            // The following makes the ball bounce off of paddle1 if their Bounding Rectangles collide.
            if (ball.Intersects(paddle1)) {
                if (keyboardState.IsKeyDown(Keys.Up)) {
                    ball.Set(MinX + paddle1.Width, ball.Position.Y);
                    if (ballSpeed.X == -100.0f) {
                        ballSpeed.X = -150.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = -50.0f;
                    }
                    else if (ballSpeed.X == -150.0f) {
                        ballSpeed.X = -75.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = -200.0f;
                    }

                    else if (ballSpeed.X == -75.0f) {
                        ballSpeed.X = -100.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = -100.0f;
                    }
                }

                else if (keyboardState.IsKeyDown(Keys.Down)) {
                    ball.Set(MinX + paddle1.Width, ball.Position.Y);
                    if (ballSpeed.X == -100.0f) {
                        ballSpeed.X = -150.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = 50.0f;
                    }
                    else if (ballSpeed.X == -150.0f) {
                        ballSpeed.X = -75.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = 200.0f;
                    }

                    else if (ballSpeed.X == -75.0f) {
                        ballSpeed.X = -100.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = 100.0f;
                    }
                }

                else {
                    ball.Set(MinX + paddle1.Width, ball.Position.Y);
                    ballSpeed.X *= -1;
                }
            }

            // The following makes the ball bounce off of paddle2 if their Bounding Rectangles collide.
            if (ball.Intersects(paddle2)) {
                if (keyboardState.IsKeyDown(Keys.NumPad5)) {
                    ball.Set(MaxX - paddle2.Width, ball.Position.Y);
                    if (ballSpeed.X == 100.0f) {
                        ballSpeed.X = 150.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = -50.0f;
                    }
                    else if (ballSpeed.X == 150.0f) {
                        ballSpeed.X = 75.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = -200.0f;
                    }

                    else if (ballSpeed.X == 75.0f) {
                        ballSpeed.X = 100.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = -100.0f;
                    }
                }

                else if (keyboardState.IsKeyDown(Keys.NumPad2)) {
                    ball.Set(MaxX - paddle2.Width, ball.Position.Y);
                    if (ballSpeed.X == 100.0f) {
                        ballSpeed.X = 150.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = 50.0f;
                    }
                    else if (ballSpeed.X == 150.0f) {
                        ballSpeed.X = 75.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = 200.0f;
                    }

                    else if (ballSpeed.X == 75.0f) {
                        ballSpeed.X = 100.0f;
                        ballSpeed.X *= -1;
                        ballSpeed.Y = 100.0f;
                    }
                }

                else {
                    ball.Set(MaxX - paddle2.Width, ball.Position.Y);
                    ballSpeed.X *= -1;
                }
            }
            #endregion /* Paddle & Ball Interactions */

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprites & scoreboard.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.DrawString(font, "P1: " + player1Score, new Vector2(200, 30), Color.Black);
            spriteBatch.DrawString(font, "P2: " + player2Score, new Vector2(600, 30), Color.Black);
            ball.Draw(spriteBatch);
            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
            Update(gameTime);
        }
    }
}