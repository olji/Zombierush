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
    class Level
    {
        #region Variables
        protected Rectangle Ground = new Rectangle(0,0,0,0);
        protected List<Rectangle> Walls = new List<Rectangle>();
        protected List<Rectangle> bigPlatforms = new List<Rectangle>();
        protected List<Rectangle> midPlatforms = new List<Rectangle>();
        protected List<Rectangle> smallPlatforms = new List<Rectangle>();


        Texture2D gfx_Ground;
        Texture2D gfx_PlatformLarge;
        Texture2D gfx_PlatformMedium;
        Texture2D gfx_PlatformSmall;
        Texture2D gfx_Walls;
        Texture2D bkgrnd;
        #endregion

        #region Properties
        public Texture2D ground
        {
            get { return gfx_Ground; }
            set { gfx_Ground = value; }
        }
        public Texture2D wall
        {
            get { return gfx_Walls; }
            set { gfx_Walls = value; }
        }
        public Texture2D lrgPlatform
        {
            get { return gfx_PlatformLarge; }
            set { gfx_PlatformLarge = value; }
        }
        public Texture2D midPlatform
        {
            get { return gfx_PlatformMedium; }
            set { gfx_PlatformMedium = value; }
        }
        public Texture2D backGround
        {
            get { return bkgrnd; }
            set { bkgrnd = value; }
        }
        public Texture2D smlPlatform
        {
            get { return gfx_PlatformSmall; }
            set { gfx_PlatformSmall = value; }
        }
        #endregion

        public Level()
        {
        }

        public List<Rectangle> LevelCreator(List<Rectangle> x, GraphicsDeviceManager graphics)
        {
            //walls and ground
            Rectangle groundBox = new Rectangle(0, graphics.GraphicsDevice.Viewport.Height - 125, graphics.GraphicsDevice.Viewport.Width , 150);
            Rectangle wallLeft = new Rectangle(0, 0, 25, graphics.GraphicsDevice.Viewport.Height - 125);
            Rectangle wallRight = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 25, 0, 25, graphics.GraphicsDevice.Viewport.Height - 125);

            //Platforms
            Rectangle temp1 = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 175, graphics.GraphicsDevice.Viewport.Height - 175, 100, 15);
            Rectangle temp2 = new Rectangle(75, graphics.GraphicsDevice.Viewport.Height-175, 100, 15);
            Rectangle temp3 = new Rectangle((graphics.GraphicsDevice.Viewport.Width / 2) - 75, graphics.GraphicsDevice.Viewport.Height - 175, 150, 15);
            Rectangle temp4 = new Rectangle(175, graphics.GraphicsDevice.Viewport.Height - 225, 150, 15);
            Rectangle temp5 = new Rectangle(temp4.X + 300, graphics.GraphicsDevice.Viewport.Height - 225, 150, 15);
            Rectangle temp6 = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 175, graphics.GraphicsDevice.Viewport.Height - 275, 100, 15);
            Rectangle temp7 = new Rectangle(75, graphics.GraphicsDevice.Viewport.Height - 275, 100, 15);
            Rectangle temp8 = new Rectangle((graphics.GraphicsDevice.Viewport.Width / 2) - 75, graphics.GraphicsDevice.Viewport.Height - 275, 150, 15);
            Rectangle temp9 = new Rectangle(175, graphics.GraphicsDevice.Viewport.Height - 325, 150, 15);
            Rectangle temp10 = new Rectangle(temp4.X + 300, graphics.GraphicsDevice.Viewport.Height - 325, 150, 15);
            Rectangle temp13 = new Rectangle((graphics.GraphicsDevice.Viewport.Width / 2) - 75, graphics.GraphicsDevice.Viewport.Height - 375, 150, 15); 
            Rectangle temp16 = new Rectangle(25, graphics.GraphicsDevice.Viewport.Height - 225, 50, 15);
            Rectangle temp17 = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 75, graphics.GraphicsDevice.Viewport.Height - 225, 50, 15);

            // Add to list for draw function
            Ground = groundBox;
            Walls.Add(wallLeft);
            Walls.Add(wallRight);
            
            // Add platforms with a width of 150px to their own list for drawing
            bigPlatforms.Add(temp3);
            bigPlatforms.Add(temp4);
            bigPlatforms.Add(temp5);
            bigPlatforms.Add(temp8);
            bigPlatforms.Add(temp9);
            bigPlatforms.Add(temp10);
            bigPlatforms.Add(temp13);

            // Add platforms with a width of 100px to their own list for drawing
            midPlatforms.Add(temp1);
            midPlatforms.Add(temp2);
            midPlatforms.Add(temp6);
            midPlatforms.Add(temp7);

            // Add platforms with a width of 50px to their own list for drawing
            smallPlatforms.Add(temp16);
            smallPlatforms.Add(temp17);


            // Add to list for collision, with walls and ground first
            x.Add(wallRight);
            x.Add(wallLeft);
            x.Add(groundBox);
            x.Add(temp1);
            x.Add(temp2);
            x.Add(temp3);
            x.Add(temp4);
            x.Add(temp5);
            x.Add(temp6);
            x.Add(temp7);
            x.Add(temp8);
            x.Add(temp9);
            x.Add(temp10);
            x.Add(temp13);
            x.Add(temp16);
            x.Add(temp17);

            return x;
        }
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(bkgrnd, new Rectangle(0, 0, 800, 480), Color.White);
            sprite.Draw(gfx_Ground, Ground, Color.White);
            foreach (Rectangle x in Walls)
                sprite.Draw(gfx_Walls, x, Color.White);
            foreach (Rectangle x in bigPlatforms)
                sprite.Draw(gfx_PlatformLarge, x, Color.White);
            foreach (Rectangle x in midPlatforms)
                sprite.Draw(gfx_PlatformMedium, x, Color.White);
            foreach (Rectangle x in smallPlatforms)
                sprite.Draw(gfx_PlatformSmall, x, Color.White);
        }
    }
}
