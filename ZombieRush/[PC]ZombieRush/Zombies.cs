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
    class Zombies : Player
    {
        #region Variables

        protected bool trapped = false, lockLeft = false, lockRight = false, jumpLock = false, waitUntilLand = false, spawning = true;
        protected Rectangle JumpBox;
        protected int spawnAnimSpeed = 30, spawnFrame = 0, trapHeight;
        protected Texture2D spawnSheet;
        protected bool stopChase = false;
        #endregion

        #region Constuctor

        public Zombies(int x, int y, float speed, Color z, Texture2D sheet)
            : base(x, y, speed, z)
        {
            if (speed < 0.30)
                speed = 0.30f;
            if (speed > 0.65)
                speed = 0.65f;
            runAcc = speed;
            maxSpeed = speed * 3;
            spawnSheet = sheet;
            playerColor = z;
        }
        public Zombies()
        { }

        #endregion

        #region Properties
        public Rectangle jBox
        {
            get { return JumpBox; }
        }
        public Texture2D spwnSheet
        {
            get { return spawnSheet; }
            set { spawnSheet = value; }
        }
        public bool stop
        {
            get { return stopChase; }
            set { stopChase = value; }
        }
        #endregion

        #region Movement
        public void enemyMovement(GameTime gameTime, Player x, List<Rectangle> mapHitBoxes)
        {
            //Collisionbox made to make AI know when to jump to get to another platform
            JumpBox = new Rectangle((int)Position.X - 25, (int)Position.Y - 50, 50, 40);

            if (stopChase)
                moveState = 0;

            //Spawning animation
            if (spawning)
            {
                runningTime += gameTime.ElapsedGameTime.Milliseconds;
                if (runningTime >= spawnAnimSpeed)
                {
                    runningTime = 0;
                    spawnFrame++;
                    if (spawnFrame > 35)
                        spawning = false;
                }
            }    

            //************ZOMBIE MOVEMENT******************
            else if (!spawning)
            {
                if (!stopChase)
                {
                    if (Position.X > x.pos.X && !trapped || lockLeft)
                    {
                        //Movement and animation
                        rCheck = true;
                        flip = false;
                        if (runningFrame > 11)
                            runningFrame = 6;
                        runningTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (runningTime >= animSpeed)
                        {
                            runningTime = 6;
                            runningFrame++;
                            if (runningFrame > 11)
                                runningFrame = 6;
                        }

                        momentum.X -= rAcc;

                        if (momentum.X < -pMaxSpeed)
                            momentum.X = -pMaxSpeed;

                    }

                    if (Position.X < x.pos.X && !trapped || lockRight)
                    {
                        flip = true;
                        rCheck = true;
                        if (runningFrame < 12)
                            runningFrame = 12;
                        runningTime += gameTime.ElapsedGameTime.Milliseconds;
                        if (runningTime >= animSpeed)
                        {
                            runningTime = 0;


                            runningFrame++;
                            if (runningFrame > 17)
                                runningFrame = 12;

                        }

                        momentum.X += rAcc;

                        if (momentum.X > pMaxSpeed)
                            momentum.X = pMaxSpeed;

                    }
                    //Hinders the AI from jumping when it's useless
                    if ((Position.X - x.pos.X <= 50 && Position.X - x.pos.X >= 0 && !inAir && x.iAir && x.pos.Y < Position.Y && !jumpLock) || (Position.X - x.pos.X >= -50 && Position.X - x.pos.X <= 0 && !inAir && x.iAir && x.pos.Y < Position.Y && !jumpLock))
                    {
                        JumpFunc();
                    }
                }
                foreach (Rectangle platform in mapHitBoxes)
                {
                    if (x.pos.Y < Position.Y && JumpBox.Intersects(platform) && !inAir)
                    {
                        JumpFunc();
                    }
                    if (inAir)
                        waitUntilLand = true;

                    //Makes the AI to stop chasing and jump up/fall down before chasing again
                    if (feetBox.Intersects(platform) && x.pos.Y != Position.Y && ((Position.X - x.pos.X >= -50 && Position.X - x.pos.X <= 0) || (Position.X - x.pos.X <= 50 && Position.X - x.pos.X >= 0)))
                    {
                        trapped = true;
                        if (x.pos.Y > Position.Y)
                            jumpLock = true;
                        if (Position.X - x.pos.X >= -50 && Position.X - x.pos.X <= 0 && !lockLeft)
                        {
                            lockRight = true;
                            lockLeft = false;
                            trapHeight = (int)Position.Y;
                        }
                        else if (Position.X - x.pos.X <= 50 && Position.X - x.pos.X >= 0 && !lockRight)
                        {
                            lockLeft = true;
                            lockRight = false;
                            trapHeight = (int)Position.Y;
                        }
                    }

                    if (feetBox.Intersects(platform) && waitUntilLand && Position.Y < trapHeight || x.pos.Y == trapHeight)
                    {
                        trapped = false;
                        lockRight = false;
                        lockLeft = false;
                        waitUntilLand = false;
                    }
                    if (inAir && Position.Y > trapHeight || x.pos.Y == trapHeight)
                    {
                        trapped = false;
                        lockRight = false;
                        lockLeft = false;
                    }

                }
                physics();
            }
        }
        #endregion

        #region ZombieDraw

        public void drawChoice(SpriteBatch spriteBatch, Texture2D sprite, Rectangle move, Texture2D box)
        {
            if (spawning)
            {
                move = new Rectangle((spawnFrame % 6) * 34, (spawnFrame / 6) * 34, 34, 34);
                spriteBatch.Draw(spawnSheet, pos, move, playerColor, 0,
                    new Vector2(17,34), 1.0f, SpriteEffects.None, 0);
            }
            else if (!spawning)
            {

                if (stopChase)
                    move = new Rectangle(0, 0, 34, 34);
                else if (!stopChase)
                    move = spriteFinder(move);
                draw(sprite, move, spriteBatch); 
            }
        }
        #endregion
    }
}
