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
        protected Vector2 centerPoint;

        public Entity(Texture2D sprite, Vector2 position) {
            this.sprite = sprite;
            this.position = position;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            centerPoint = new Vector2(((int)position.X + (Width / 2)), ((int)position.Y + (Height / 2)));
        }

        public Vector2 CenterPoint { get { return centerPoint; } }
        public Vector2 Position { get { return position; } }
        public int Width { get { return bounds.Width; } }
        public int Height { get { return bounds.Height; } }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public virtual void Update(GameTime gameTime) {
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;
            centerPoint.X = ((int)position.X + (Width / 2));
            centerPoint.Y = ((int)position.Y + (Height / 2));
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
            if (position.X > maxX - Width) {
                position.X = (maxX - Width);
            } else if (position.X < minX) {
                position.X = minX;
            }

            if (position.Y > maxY - Height) {
                position.Y = (maxY - Height);
            } else if (position.Y < minY) {
                position.Y = minY;
            }
        }
    }
}
