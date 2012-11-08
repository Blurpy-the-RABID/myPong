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
    class Paddle : Entity {

        public Paddle(Texture2D sprite, Vector2 position) : base(sprite, position) {}

        public void MoveUp() {
            Move(0.0f, -2.0f);
        }

        public void MoveDown() {
            Move(0.0f, 2.0f);
        }

        public void PaddleMove(KeyboardState keyboardState, Keys up, Keys down) {
            // This method allows the player to control the paddle with an up & down key.
            if (keyboardState.IsKeyDown(up)) {
                MoveUp();
            }
            if (keyboardState.IsKeyDown(down)) {
                MoveDown();
            }
        }

        //public void MoveDown(KeyboardState keyboardState, Keys down) {}

        public void PaddleAI(Ball ball) {
            // Here are the steps that the AI should go through to ensure that it guards its goal area:
            // Step 1:  Determine if its paddle's centerpoint is not horizontally aligned with the
            // centerpoint of the ball.
            // Step 2:  Adjust the paddle's position until it is aligned with the center of the ball.
            
            if (ball.CenterPoint.Y > CenterPoint.Y){
                this.MoveDown();
            }
            if (ball.CenterPoint.Y < CenterPoint.Y) {
                MoveUp();
            }
        }
    }
}
