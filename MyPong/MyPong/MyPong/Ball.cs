using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyPong {
    class Ball : Entity {
        private Vector2 speed = new Vector2(150.0f, 150.0f);
                
        public Ball(Texture2D sprite, Vector2 position) : base(sprite, position) {}

        public Vector2 Speed {
            get { return speed; }
            set { speed = value; }
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            position += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            centerPoint.X = ((int)position.X + (Width / 2));
            centerPoint.Y = ((int)position.Y + (Height / 2));
        }

        public override void Clip(int minX, int maxX, int minY, int maxY) {
            if (position.Y > maxY - Height) {
                speed.Y = -speed.Y;
                position.Y = maxY - Height;
            }
            else if (position.Y < minY) {
                speed.Y = -speed.Y;
                position.Y = minY;
            }
        }

        public void ClipPaddle(Entity paddle, KeyboardState keyboardState, Keys up, Keys down) {
            if (Intersects(paddle)) {
                // The following makes the ball bounce off of paddle2 if their Bounding Rectangles collide.
                if (speed.X > 0) {
                    // If paddle2 is moving at the time of collision with the ball, the ball's trajectory will be changed.
                    if (keyboardState.IsKeyDown(up) || keyboardState.IsKeyDown(down)) {
                        position.X = paddle.Position.X - paddle.Width - Width;
                        if (speed.X == 150.0f) {
                            speed.X = -100.0f;
                            speed.Y = 200.0f;
                        }
                        else if (speed.X == 200.0f) {
                            speed.X = -150.0f;
                            speed.Y = 150.0f;
                        }
                        else if (speed.X == 100.0f) {
                            speed.X = -200.0f;
                            speed.Y = 100.0f;
                        }
                        if (keyboardState.IsKeyDown(up)) {
                            speed.Y = -speed.Y;
                        }
                    }
                    else {
                        position.X = paddle.Position.X - paddle.Width - Width;
                        speed.X = -speed.X;
                    }
                }
                // The following makes the ball bounce off of paddle1 if their Bounding Rectangles collide.
                else {
                    // If paddle1 is moving at the time of collision with the ball, the ball's trajectory will be changed.
                    if (keyboardState.IsKeyDown(up) || keyboardState.IsKeyDown(down)) {
                        position.X = paddle.Position.X + paddle.Width;
                        if (speed.X == -150.0f) {
                            speed.X = 100.0f;
                            speed.Y = 200.0f;
                        }
                        else if (speed.X == -200.0f) {
                            speed.X = 150.0f;
                            speed.Y = 150.0f;
                        }
                        else if (speed.X == -100.0f) {
                            speed.X = 200.0f;
                            speed.Y = 100.0f;
                        }
                        if (keyboardState.IsKeyDown(up)) {
                            speed.Y = -speed.Y;
                        }
                    } else {
                        position.X = paddle.Position.X + paddle.Width;
                        speed.X = -speed.X;
                    }
                }
            }
        }
        
    }
}
