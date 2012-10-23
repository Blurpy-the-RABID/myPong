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
    class Entity {
        private Texture2D sprite;
        private Rectangle bounds;
        protected Vector2 position;

        public Entity(Texture2D sprite, Vector2 position) {
            this.sprite = sprite;
            this.position = position;
            this.bounds = new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height);
        }

        public Vector2 Position { get { return position; } }
        public int Width { get { return bounds.Width; } }
        public int Height { get { return bounds.Height; } }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, Position, Color.White);
        }

        public void Update(GameTime gameTime) {
            bounds.X = (int)Position.X;
            bounds.Y = (int)Position.Y;
        }

        public bool Intersects(Entity entity) {
            return bounds.Intersects(entity.bounds);
        }

        public void Move(float x, float y) {
            position.X += x;
            position.Y += y;
        }

        public void Set(float x, float y) {
            position.X = x;
            position.Y = y;
        }

        public virtual void Clip(int minX, int maxX, int minY, int maxY) {
            if (Position.X > maxX - Width) {
                position.X = (maxX - Width);
            } else if (Position.X < minX) {
                position.X = minX;
            }

            if (Position.Y > maxY - Height) {
                position.Y = (maxY - Height);
            } else if (Position.Y < minY) {
                position.Y = minY;
            }
        }
    }
}
