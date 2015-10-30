using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    class Player
    {
        #region Variables

        protected Rectangle collisionBox, feetBox;
        Texture2D corpseSprite;
        protected Vector2 Position;
        protected int movementState;
        protected int runTime, runFrame;
        protected int sideWallCount = 0;
        protected const int animationSpeed = 50;
        protected Vector2 momentum = new Vector2(0, 0);
        protected const float maxFallSpeed = 40f;
        protected const float gravity = 0.75f;
        protected float runAcc;
        protected float maxSpeed = 5f;
        protected Color playerColor = Color.Black;
        protected bool inAir = false, airCheck = true, fallCheck = false, jumpCheck = false, charDead = false;
        protected bool flipped = false, bjump = false, runCheck = false, rightEnabled = true, leftEnabled = true;
        protected char control;
        protected SoundEffect footSteps;
        protected SoundEffect jumpSound;
        protected SoundEffect landSound;
        protected SoundEffectInstance steps;
        protected bool stepPlaying = false;
        #endregion

        #region Constructor
        public Player()
        { }
        public Player(int x, int y, float speed, Color z)
        {
            Position = new Vector2(x, y);
            playerColor = z;
            runAcc = speed;
        }
        #endregion

        #region Properties
        public Texture2D corpse
        {
            get { return corpseSprite; }
            set { corpseSprite = value; }
        }
        public Vector2 pos
        {
            get { return Position; }
            set { Position = value; }
        }
        public char contr
        {
            get { return control; }
            set { control = value; }
        }
        public bool cDead
        {
            get { return charDead; }
            set { charDead = value; }
        }
        public int moveState
        {
            get { return movementState; }
            set { movementState = value; }
        }
        public int runningTime
        {
            get { return runTime; }
            set { runTime = value; }
        }
        public int runningFrame
        {
            get { return runFrame; }
            set { runFrame = value; }
        }
        public int animSpeed
        {
            get { return animationSpeed; }
        }
        public float momentumX{
            get { return momentum.X; }
            set { momentum.X = value; }
        }
        public float momentumY {
            get { return momentum.Y; }
            set { momentum.Y = value; }
        }
        public Vector2 pMomentum
        {
            get { return momentum; }
            set { momentum = value; }
        }
        public float rAcc
        {
            get { return runAcc; }
        }
        public bool rEnable
        {
            get { return rightEnabled; }
            set { rightEnabled = value; }
        }
        public bool lEnable
        {
            get { return leftEnabled; }
            set { leftEnabled = value; }
        }
        public bool cJump
        {
            get { return bjump; }
            set { bjump = value; }
        }
        public float grav
        {
            get { return gravity; }
        }
        public float pMaxSpeed
        {
            get { return maxSpeed; }
        }
        public Rectangle colBox
        {
            get { return collisionBox; }
            set { collisionBox = value; }
        }
        public float maxFall
        {
            get { return maxFallSpeed; }
        }
        public bool jCheck
        {
            get { return jumpCheck; }
            set { jumpCheck = value; }
        }
        public bool rCheck
        {
            get { return runCheck; }
            set { runCheck = value; }
        }
        public bool fCheck
        {
            get { return fallCheck; }
            set { fallCheck = value; }
        }
        public bool flip
        {
            get { return flipped; }
            set { flipped = value; }
        }
        public bool iAir
        {
            get { return inAir; }
            set { inAir = value; }
        }
        public SoundEffect fSteps
        {
            get { return footSteps; }
            set { footSteps = value; }
        }
        public SoundEffect jSound
        {
            get { return jumpSound; }
            set { jumpSound = value; }
        }
        public SoundEffect lSound
        {
            get { return landSound; }
            set { landSound = value; }
        }
        public SoundEffectInstance stepsInst
        {
            get { return steps; }
            set { steps = value; }
        }
        #endregion

        #region Collision
        public virtual void groundCollisionFunc(Player Character, List<Rectangle> hitBox, GraphicsDeviceManager graphics)
        {
            foreach (Rectangle platform in hitBox)
            {
                //Sets the two first to be walls and the third to ground since that is what they are (Check the LevelCreator method in the Level class for why)
                Rectangle rightWall = hitBox[0];
                Rectangle leftWall = hitBox[1];
                Rectangle groundWall = hitBox[2];

                //Sets the momentum to zero when hitting a wall
                if (collisionBox.Intersects(rightWall))
                {
                    if (momentum.X > 0)
                        momentum.X = 0;
                    else if (momentum.X < 0)
                    { }
                }

                else if (collisionBox.Intersects(leftWall))
                {
                    if (momentum.X < 0)
                        momentum.X = 0;
                    else if (momentum.X > 0)
                    { }
                }

                //Makes sure that the character can't fall through the floor while colliding with the wall, also position the character just beside the wall and disables its ability to run towards it again
                if (collisionBox.Intersects(rightWall) && feetBox.Intersects(groundWall))
                {
                    fCheck = false;
                    jCheck = false;
                    cJump = false;
                    rightEnabled = false;
                    momentum.Y = 0;
                    Position.Y = groundWall.Y;
                    momentum.X = 0;
                    Position.X = rightWall.X - 6;
                }
                else if (collisionBox.Intersects(leftWall) && feetBox.Intersects(groundWall))
                {
                    fCheck = false;
                    jCheck = false;
                    cJump = false;
                    leftEnabled = false;
                    momentum.Y = 0;
                    Position.Y = groundWall.Y;
                    momentum.X = 0;
                    Position.X = leftWall.X + leftWall.Width + 6;
                }
                else if (collisionBox.Intersects(leftWall) && inAir)
                {
                    cJump = false;
                    leftEnabled = false;
                    momentum.X = 0;
                    Position.X = leftWall.X + leftWall.Width + 6;
                    
                    if (feetBox.Intersects(platform) && platform != leftWall)
                    {
                    airCheck = false;
                    inAir = false;
                    fCheck = false;
                    jCheck = false;
                    cJump = false;
                    momentum.Y = 0;
                    Position.Y = platform.Y + 1;
                    }
                }
                else if (collisionBox.Intersects(rightWall) && inAir)
                {
                    cJump = false;
                    rightEnabled = false;
                    momentum.X = 0;
                    Position.X = rightWall.X - 6;
                    
                    if (feetBox.Intersects(platform) && platform != rightWall)
                    {
                        airCheck = false;
                        inAir = false;
                        fCheck = false;
                        jCheck = false;
                        cJump = false;
                        momentum.Y = 0;
                        Position.Y = platform.Y + 1;
                    }
                }

                //Sets all variables to count him as on the ground
                else if (feetBox.Intersects(platform) && fallCheck)
                {
                    airCheck = false;
                    inAir = false;
                    fCheck = false;
                    jCheck = false;
                    cJump = false;
                    momentum.Y = 0;
                    Position.Y = platform.Y + 1;
                }
            }
            //Adds some further fixes for errors that occurs when using a list for hit-detection
            if (!airCheck)
            {
                airCheck = true;
                inAir = false;
            }
            else if (airCheck)
            {
                inAir = true;
            }
        }

        #endregion

        #region Physics
        public void pubPhysics() {
            physics();
        }
        protected virtual void physics()
        {
            // ********PHYSICS*************
            if (momentum.Y < -10) {
                bool x = true;
            }
            //Adds friction and a full stop when the momentum is very low
            if (!inAir)
            {
                if (movementState == 0)
                    momentum.X *= 0.85f;

                if ((momentum.X <= 0.5 && momentum.X >= 0) || (momentum.X >= -0.5 && momentum.X <= 0))
                    momentum.X = 0;
            }

            //Adds gravity and sets the state of the character to falling when he's moving downwards
            else if (inAir)
            {
                momentum.Y += gravity;

                if (momentum.Y >= 0)
                {
                    fallCheck = true;
                    movementState = 2;
                }
            }
            //Puts a cap on the fallspeed
            if (momentum.Y > maxFall)
                momentum.Y = maxFall;

            //Sets a new position based on the momentum
            Position += momentum;

            #region SUMMON HITBOX
            //Moves the hitboxes for body and feet
            if (!charDead)
                collisionBox = new Rectangle((int)Position.X - 6, (int)Position.Y - 34, 12, 24);
            else if (charDead)
                collisionBox = new Rectangle((int)Position.X - 17, (int)Position.Y - 12, 34, 12);
            feetBox = new Rectangle((int)Position.X - 6, (int)Position.Y, 12, 1);

            #endregion
        }
        #endregion

        #region Jumpfunction
        public void JumpFunc()
        {
            //Adds momentum upwards and sets his state to jumping
            momentum.Y = -10;
            inAir = true;
            jumpCheck = true;
            movementState = 1;
        }
        #endregion

        #region DrawingFunctions
        protected Rectangle spriteFinder(Rectangle move)
        {
            if (rCheck && !jCheck && !charDead)
            {
                move = new Rectangle((runFrame % 3) * 34, (runFrame / 3) * 34, 34, 34);
            }
            //Jumpframe
            else if (jCheck && !fCheck && !charDead)
            {
                if (!flip)
                    move = new Rectangle((1 % 3) * 34, (1 / 1) * 34, 34, 34);
                else if (flip)
                    move = new Rectangle((2 % 3) * 34, (1 / 1) * 34, 34, 34);
            }
            //Fallframe
            else if (fCheck && !charDead)
            {
                if (!flip)
                {
                    move = new Rectangle((2 % 3) * 34, (0 / 1) * 34, 34, 34);
                }
                else if (flip)
                {
                    move = new Rectangle((0 & 3) * 34, (1 / 1) * 34, 34, 34);
                }
            }
            else if (charDead)
            {
                move = new Rectangle(0, 0, 34, 34);
            }
            //Idleframe
            else
            {
                move = new Rectangle((0 % 3) * 34, (0 / 2) * 34, 34, 34);
            }

            return move;

        }

        public virtual void drawFunc(SpriteBatch spriteBatch, Texture2D sprite, Rectangle move, Texture2D box)
        {
            move = spriteFinder(move);
            draw(sprite, move, spriteBatch);
        }

        protected void draw(Texture2D gfx, Rectangle x, SpriteBatch sprites)
        {
            if (!charDead)
                sprites.Draw(gfx, pos, x, playerColor,
                            0, new Vector2(17, 34), 1.0f, SpriteEffects.None, 0);
            else if (charDead)
                sprites.Draw(corpseSprite, pos, x, playerColor,
                    0, new Vector2(17, 34), 1.0f, SpriteEffects.None, 0);
        }
        #endregion
    }
}
