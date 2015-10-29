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


namespace Game1
{
    class Environment
    {
        protected Texture2D texture;
        protected char direction;
        protected float speed;
        protected Random rdm = new Random();
        protected Vector2 position;
        protected int depth;

        public Environment()
        {
        }
        public Environment(GraphicsDeviceManager graphics, Texture2D text1, Texture2D text2)
        {
            //Randomises texture, speed, direction of movement, Y-position and from where it spawns
            int x = rdm.Next(1, 3);
            if (x == 1)
                texture = text1;
            else if (x == 2)
                texture = text2;
            speed = (float)rdm.NextDouble();
            if (speed <= 0.2)
                speed = 0.2f;
            x = rdm.Next(1, 3);
            if (x == 1)
                direction = 'L';
            else if (x == 2)
                direction = 'R';
            int y = rdm.Next(20, 90);
            depth = rdm.Next(0, 11);
            if (direction == 'L')
                x = graphics.GraphicsDevice.Viewport.Width - 20;
            else if (direction == 'R')
                x = -75;
            position = new Vector2(x, y);
        }
        public Vector2 pos
        {
            get { return position; }
            set { position = value; }
        }
        public void Animation(GraphicsDeviceManager graphics)
        {
            // animates the cloud
                if (direction == 'L')
                    position -= new Vector2(speed, 0);
                else if (direction == 'R')
                    position += new Vector2(speed, 0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(texture, position, null, Color.White,
                    0, new Vector2(texture.Width/2, texture.Height/2), 1.0f, SpriteEffects.None, depth);
        }
    }
}
