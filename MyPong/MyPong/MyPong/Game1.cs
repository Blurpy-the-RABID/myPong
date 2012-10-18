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

        Texture2D ball;
        Texture2D paddle1;
        Texture2D paddle2;

        Vector2 ballPosition;
        Rectangle ballBR;
        Vector2 paddlePosition1;
        Rectangle paddle1BR;
        Vector2 paddlePosition2;
        Rectangle paddle2BR;

        int player1Score = 0;
        int player2Score = 0;
        // Store some information about the sprite's motion.
        Vector2 ballSpeed = new Vector2(100.0f, 100.0f);

        #endregion /* Variables */

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            // Set the coordinates to draw the sprite at.
            ballPosition = new Vector2(400, 0);
            paddlePosition1 = new Vector2(0, 190);
            paddlePosition2 = new Vector2(790, 190);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load our font for displaying the scoreboard.
            font = Content.Load<SpriteFont>("SpriteFont1");

            // Here's the ball and its Bounding Rectangle.
            ball = Content.Load<Texture2D>("ball");
            ballBR = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, ball.Width, ball.Height);

            // Here's the paddles and their corresponding Bounding Rectangles.
            paddle1 = Content.Load<Texture2D>("paddle1");
            paddle2 = Content.Load<Texture2D>("paddle2");
            paddle1BR = new Rectangle((int)paddlePosition1.X, (int)paddlePosition1.Y, paddle1.Width, paddle1.Height);
            paddle2BR = new Rectangle((int)paddlePosition2.X, (int)paddlePosition2.Y, paddle2.Width, paddle2.Height);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update the position of the Bounding Rectangle for the ball so that it matches the position of the ball texture.
            ballBR.X = (int)ballPosition.X;
            ballBR.Y = (int)ballPosition.Y;

            // Update the position of the Bounding Rectangles for each paddle so that they match the position of their corresponding textures.
            paddle1BR.X = (int)paddlePosition1.X;
            paddle1BR.Y = (int)paddlePosition1.Y;

            paddle2BR.X = (int)paddlePosition2.X;
            paddle2BR.Y = (int)paddlePosition2.Y;

            // These variables specify the boundaries of our game window.
            int MaxX = graphics.GraphicsDevice.Viewport.Width - ball.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - ball.Height;
            int MinY = 0;

            KeyboardState keyboardState = Keyboard.GetState();

            #region Paddle1 & Paddle2 Controls
            // Keyboard input controls for paddle1
            if (keyboardState.IsKeyDown(Keys.Up)) {
                paddlePosition1.Y -= 5;
            }

            if (keyboardState.IsKeyDown(Keys.Down)) {
                paddlePosition1.Y += 5;
            }

            if (paddlePosition1.Y > MaxY - paddle1.Height + 20) {
                paddlePosition1.Y = MaxY - paddle1.Height + 20;
            }

            else if (paddlePosition1.Y < MinY) {
                paddlePosition1.Y = MinY;
            }

            // Keyboard input controls for paddle2
            if (keyboardState.IsKeyDown(Keys.NumPad5)) {
                paddlePosition2.Y -= 5;
            }

            if (keyboardState.IsKeyDown(Keys.NumPad2)) {
                paddlePosition2.Y += 5;
            }

            if (paddlePosition2.Y > MaxY - paddle2.Height + 20) {
                paddlePosition2.Y = MaxY - paddle2.Height + 20;
            }

            else if (paddlePosition2.Y < MinY) {
                paddlePosition2.Y = MinY;
            }
            #endregion /* Paddle1 & Paddle2 Controls */

            #region Ball Speed, Top & Bottom Bouncing, & Score Keeping
            // Move the sprite by speed, scaled by elapsed time.
            ballPosition += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // This will keep track of when a player scores a point by hitting the ball into the other player's goal zone.
            if (ballPosition.X > MaxX) {
                player1Score += 1;
                ballPosition.X = 30;
            }

            else if (ballPosition.X < MinX) {
                player2Score += 1;
                ballPosition.X = 770;
            }

            // The following keeps the ball from falling off of the top & bottom margins of the game window, making it bounce off of the window's top & bottom edges instead.
            if (ballPosition.Y > MaxY) {
                ballSpeed.Y *= -1;
                ballPosition.Y = MaxY;
            }

            else if (ballPosition.Y < MinY) {
                ballSpeed.Y *= -1;
                ballPosition.Y = MinY;
            }
            #endregion /* Ball Speed, Top & Bottom Bouncing, & Score Keeping */

            #region Paddle & Ball Interactions
            // The following makes the ball bounce off of paddle1 if their Bounding Rectangles collide.
            if (ballBR.Intersects(paddle1BR)) {
                if (keyboardState.IsKeyDown(Keys.Up)) {
                    ballPosition.X = MinX + paddle1.Width;
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
                    ballPosition.X = MinX + paddle1.Width;
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
                    ballPosition.X = MinX + paddle1.Width;
                    ballSpeed.X *= -1;
                }
            }

            // The following makes the ball bounce off of paddle2 if their Bounding Rectangles collide.
            if (ballBR.Intersects(paddle2BR)) {
                if (keyboardState.IsKeyDown(Keys.NumPad5)) {
                    ballPosition.X = MaxX - paddle2.Width;
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
                    ballPosition.X = MaxX - paddle2.Width;
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
                    ballPosition.X = MaxX - paddle2.Width;
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
            spriteBatch.Draw(ball, ballPosition, Color.White);
            spriteBatch.Draw(paddle1, paddlePosition1, Color.White);
            spriteBatch.Draw(paddle2, paddlePosition2, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
            Update(gameTime);
        }
    }
}