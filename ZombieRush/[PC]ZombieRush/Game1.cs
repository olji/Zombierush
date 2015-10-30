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
using System.IO;

namespace Game1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Dictionary<Keys, Tuple<Player, ICommand>> playerControls = new Dictionary<Keys, Tuple<Player, ICommand>>();
        List<Player> players = new List<Player>();
        ICommand idle = new idleCommand();

        #region Variables


        //Create variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont gameFont;
        Random zomSpeed = new Random();

        KeyboardState ks;
        MouseState ms;

        Texture2D playerSprite;
        Texture2D GFX_titleScreen;
        Texture2D startButton;
        Texture2D insaneButton;
        Texture2D easyButton;
        Texture2D mediumButton;
        Texture2D hardButton;
        Texture2D spawnSheet;
        Texture2D box;
        Texture2D highScoreButton;
        Texture2D retryBtn;
        Texture2D titleLogo;
        Texture2D ggLogo;
        Texture2D menuOverlay;
        Texture2D menuBtn;
        Texture2D cloud1;
        Texture2D cloud2;
        Texture2D soundEnabled;
        Texture2D credits;
        Texture2D soundDisabled;
        Texture2D settingsButton;
        Texture2D creditsButton;

        SoundEffect gameOver;
        Song gameMusic;

        Player Character = new Player(0, 0, 0.75f, Color.Violet);
        List<Zombies> Zombie = new List<Zombies>();
        List<Rectangle> mapHitboxes = new List<Rectangle>();
        List<int> highScore = new List<int>();
        List<Environment> cloudList = new List<Environment>();
        Level Map = new Level();

        double speed;
        int spawnSpeed = 10000, zombieLimit = 999;
        int spawnTime;
        string menuScreen = "Menu";
        int scoreTotal = 0;
        int getReady = 0;
        int deathDelay = 0;
        int cloudSpawnSpeed = 1500;
        int elapsedTime;
        string difficulty = "None";
        string endChoice = null;
        bool runOnce = false, gameRunning = true, deathScreen = false;
        bool clickCheck = false, soundOn = true, deadClick = false, keyReleased = true;

        Vector2 startButtonPos;
        Vector2 scoreButPos;

        Rectangle leftBox;
        Rectangle rightBox;
        Rectangle jumpBox;
        Rectangle retryBox;
        Rectangle menuBox;
        Rectangle scoreBox;
        Rectangle startBox;
        Rectangle easyBox;
        Rectangle mediumBox;
        Rectangle hardBox;
        Rectangle insaneBox;
        Rectangle credBox;
        Rectangle titleBox;
        Rectangle soundIconRec;
        Rectangle clickBox = new Rectangle(0, 0, 0, 0);

        Color retryColor = Color.White;
        Color menuColor = Color.White;
        Color startColor = Color.White;
        Color scoreColor = Color.White;
        Color creditColor = Color.White;
        Color easyColor = Color.White;
        Color mediumColor = Color.White;
        Color hardColor = Color.White;
        Color insaneColor = Color.White;
        #endregion

        #region Game Properties
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            // Limits the frames per second to 60
            this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }
        #endregion

        #region Initialize
        protected override void Initialize()
        {
            players.Add(Character);
            playerControls.Add(Keys.Left, new Tuple<Player, ICommand>(Character, new moveLeftCommand()));
            playerControls.Add(Keys.Right, new Tuple<Player, ICommand>(Character, new moveRightCommand()));
            playerControls.Add(Keys.Up, new Tuple<Player, ICommand>(Character, new jumpCommand()));
            base.Initialize();
        }
        #endregion

        #region Content Load
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loading graphics and fonts
            playerSprite = Content.Load<Texture2D>("SpriteChart");
            Character.corpse = Content.Load<Texture2D>("CharDead");
            GFX_titleScreen = Content.Load<Texture2D>("TitleScreen");
            startButton = Content.Load<Texture2D>("Startbutton");
            insaneButton = Content.Load<Texture2D>("InsaneButton");
            easyButton = Content.Load<Texture2D>("EasyButton");
            mediumButton = Content.Load<Texture2D>("NormalButton");
            hardButton = Content.Load<Texture2D>("HardButton");
            settingsButton = Content.Load<Texture2D>("SettingsButton");
            Map.ground = Content.Load<Texture2D>("Ground");
            Map.lrgPlatform = Content.Load<Texture2D>("PlatformLarge");
            Map.midPlatform = Content.Load<Texture2D>("PlatformMid");
            Map.smlPlatform = Content.Load<Texture2D>("PlatformSmall");
            Map.wall = Content.Load<Texture2D>("Wall");
            retryBtn = Content.Load<Texture2D>("RetryButton");
            menuBtn = Content.Load<Texture2D>("MenuButton");
            highScoreButton = Content.Load<Texture2D>("HighScoreButton");
            Map.backGround = Content.Load<Texture2D>("Background");
            menuOverlay = Content.Load<Texture2D>("MenuOverlay");
            titleLogo = Content.Load<Texture2D>("Title");
            ggLogo = Content.Load<Texture2D>("GGLogo");
            cloud1 = Content.Load<Texture2D>("Cloud1");
            cloud2 = Content.Load<Texture2D>("Cloud2");
            creditsButton = Content.Load<Texture2D>("CreditsButton");
            soundEnabled = Content.Load<Texture2D>("SoundOn");
            soundDisabled = Content.Load<Texture2D>("SoundOff");
            credits = Content.Load<Texture2D>("Credits");
            Character.fSteps = Content.Load<SoundEffect>("footsteps");
            Character.lSound = Content.Load<SoundEffect>("land");
            Character.jSound = Content.Load<SoundEffect>("jump");
            gameOver = Content.Load<SoundEffect>("Record Scratch");
            gameMusic = Content.Load<Song>("Music");
            spawnSheet = Content.Load<Texture2D>("ZombieSpawnSpriteSheet");
            gameFont = Content.Load<SpriteFont>("ScoreFont");

            //Creates a sound instance
            Character.stepsInst = Character.fSteps.CreateInstance();
            Character.stepsInst.IsLooped = true;
            box = new Texture2D(GraphicsDevice, 1, 1);
            box.SetData(new[] { Color.White });
        }
        #endregion

        #region Content Unload
        protected override void UnloadContent()
        {

            base.UnloadContent();
            spriteBatch.Dispose();
            box.Dispose();
        }
        #endregion

        #region Update
        protected override void Update(GameTime gameTime)
        {

            //Controlstates
            if (this.IsActive)
            {
                ks = Keyboard.GetState();
                ms = Mouse.GetState();
            }
            soundIconRec = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 50, 0, 50, 50);

            //Toggles sounds on and off
            if (ks.IsKeyDown(Keys.M) && keyReleased)
            {
                if (soundOn)
                    soundOn = false;
                else if (!soundOn)
                    soundOn = true;
                keyReleased = false;
            }
            else if (ks.IsKeyUp(Keys.M))
                keyReleased = true;

            #region Clouds
            //Adds a new cloud if it's less than 10 on screen
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= cloudSpawnSpeed && cloudList.Count <= 10)
            {
                cloudList.Add(new Environment(graphics, cloud1, cloud2));
                elapsedTime = 0;
            }


            foreach (Environment cloud in cloudList)
            {
                //animates each cloud
                cloud.Animation(graphics);
            }
            int temp = 0;
            foreach (Environment cloud in cloudList)
            {
                // Deletes a cloud from the list if it's outside the screen
                cloud.Animation(graphics);
                if (cloud.pos.X <= -10)
                    cloudList[temp] = null;
                else if (cloud.pos.X >= graphics.GraphicsDevice.Viewport.Width + 5)
                    cloudList[temp] = null;
                cloudList.Remove(null);
                temp++;
                break;
            }
            #endregion

            //Game Exit
            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                //Keep moving the box created upon clicking around while left mouse button is pressed
                clickCheck = true;
                clickBox = new Rectangle((int)ms.X, (int)ms.Y, 1, 1);
            }
            if (ms.LeftButton == ButtonState.Released && clickCheck)
            {
                //Set the bool to false and thus making it possible to fill all criterias for registering a click
                clickCheck = false;
            }

            //Toggles sounds on and off if the icon is clicked
            if (clickBox.Intersects(soundIconRec))
            {
                if (!clickCheck)
                {
                    if (soundOn)
                        soundOn = false;
                    else if (!soundOn)
                        soundOn = true;
                    clickReset();
                }
            }
            #region Menu Screen 1 (Menu Screen)

            if (menuScreen == "Menu")
            {
                //Activating some buttons' hitboxes
                scoreButPos = new Vector2((graphics.GraphicsDevice.Viewport.Width / 2), graphics.GraphicsDevice.Viewport.Height / 2);
                startButtonPos = new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - 275, graphics.GraphicsDevice.Viewport.Height / 2);
                scoreBox = new Rectangle((int)scoreButPos.X, (int)scoreButPos.Y, easyButton.Width, easyButton.Height);
                startBox = new Rectangle((int)startButtonPos.X, (int)startButtonPos.Y, startButton.Width, startButton.Height);
                titleBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2 - 150, 250, 141);
                credBox = new Rectangle((int)startButtonPos.X + startButton.Width / 2 + ((int)scoreButPos.X - (int)startButtonPos.X - startButton.Width), (int)startButtonPos.Y + startButton.Height + 10, creditsButton.Width, creditsButton.Height);


                if (mapHitboxes.Count == 0)
                {
                    //Calls on a method in the Level class for making a list of all the hitboxes for the map
                    mapHitboxes = Map.LevelCreator(mapHitboxes, graphics);
                    Character.pos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2 - 35);
                }
                if (clickBox.Intersects(startBox))
                {
                    //Changes color of the button while it's pressed
                    if (ms.LeftButton == ButtonState.Pressed)
                        startColor = Color.DarkGray;

                    //Resets clickbox and advances a screen
                    if (!clickCheck)
                        screenChange("DifSelect");
                }

                else if (clickBox.Intersects(scoreBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        scoreColor = Color.DarkGray;

                    if (!clickCheck)
                    {
                        difficulty = "None";
                        runOnce = false;
                        screenChange("HighScore");
                    }
                }
                else if (clickBox.Intersects(credBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        creditColor = Color.DarkGray;
                    if (!clickCheck)
                        screenChange("Credits");
                }
                else
                {
                    //Resets the colors of the buttons
                    startColor = Color.White;
                    scoreColor = Color.White;
                    creditColor = Color.White;
                }

            }
            #endregion

            #region Menu Screen 2 (Difficulty Select)
            if (menuScreen == "DifSelect")
            {
                //Creates hitboxes
                easyBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - 273, graphics.GraphicsDevice.Viewport.Height / 2 - 50, 223, 89);
                mediumBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 + 50, graphics.GraphicsDevice.Viewport.Height / 2 - 50, 223, 89);
                hardBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - 273, graphics.GraphicsDevice.Viewport.Height / 2 + 50, 223, 89);
                insaneBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 + 50, graphics.GraphicsDevice.Viewport.Height / 2 + 50, 223, 89);
                bool x = false;
                if (clickBox.Intersects(easyBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        easyColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        difficulty = "Easy";
                        x = true;
                    }
                }
                else if (clickBox.Intersects(mediumBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        mediumColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        difficulty = "Medium";
                        x = true;
                    }
                }
                else if (clickBox.Intersects(hardBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        hardColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        difficulty = "Hard";
                        x = true;
                    }
                }
                else if (clickBox.Intersects(insaneBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        insaneColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        difficulty = "Insane";
                        x = true;
                    }
                }
                else
                {
                    easyColor = Color.White;
                    mediumColor = Color.White;
                    hardColor = Color.White;
                    insaneColor = Color.White;
                }
                //Advances the screen if one of the buttons has been clicked
                if (x)
                    screenChange("Game");
            }
            #endregion

            #region Menu Screen 3 (Game)
            if (menuScreen == "Game")
            {

                if (difficulty == "Easy")
                    spawnSpeed = 5000;
                else if (difficulty == "Medium")
                    spawnSpeed = 2500;
                else if (difficulty == "Hard")
                    spawnSpeed = 1000;
                else if (difficulty == "Insane")
                    spawnSpeed = 10;

                if (getReady < spawnSpeed)
                {
                    getReady += gameTime.ElapsedGameTime.Milliseconds;
                }

                spawnTime += gameTime.ElapsedGameTime.Milliseconds;
                if (spawnTime >= spawnSpeed && Zombie.Count <= zombieLimit && getReady >= spawnSpeed && gameRunning)
                {
                    Random xPos = new Random();
                    int x = xPos.Next(35, graphics.GraphicsDevice.Viewport.Width - 35);
                    int y = graphics.GraphicsDevice.Viewport.Height - 125;
                    //Sets the zombie speed at a random value, adds another zombie into the list with and then resets the countdown for the next zombie-spawn
                    speed = zomSpeed.NextDouble();
                    Zombie.Add(new Zombies(x, y, (float)speed, Color.Green, spawnSheet));
                    spawnTime = 0;
                }

                #region Point System

                if (Zombie.Count >= 1 && gameRunning)
                    scoreTotal += gameTime.ElapsedGameTime.Milliseconds;

                #endregion

                #region Collision


                if (runOnce == false && soundOn)
                {
                    //Sets up the music volume and plays
                    MediaPlayer.Volume = 0.3f;
                    MediaPlayer.Play(gameMusic);
                    runOnce = true;
                }

                //Controlboxes for the touch interface on mobile phones
                /*
                leftBox = new Rectangle(0, graphics.GraphicsDevice.Viewport.Height / 2, graphics.GraphicsDevice.Viewport.Width / 2,
                    graphics.GraphicsDevice.Viewport.Height / 2);
                rightBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2,
                    graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);*/
                #endregion

                #region Control
                // Apply controls
                List<Player> nonIdle = new List<Player>();
                foreach (Keys key in ks.GetPressedKeys()) {
                    Tuple<Player, ICommand> value = null;
                    if (playerControls.TryGetValue(key, out value)) {
                        value.Item2.execute(gameTime, value.Item1, soundOn);
                        nonIdle.Add(value.Item1);
                    }
                }
                // Apply physics
                foreach (Player player in players) {
                    if (!nonIdle.Contains(player))
                        idle.execute(gameTime, player, soundOn);
                    player.pubPhysics();
                }
                //if (ks.IsKeyDown(Keys.Left)) {
                //    //Restricts the jumping to when the character isn't in the air
                //    if (ks.IsKeyDown(Keys.Up) && !Character.iAir)
                //        Character.cJump = true;
                //    else if (ks.IsKeyUp(Keys.Up))
                //        Character.cJump = false;

                //    if (Character.lEnable) {
                //        //Sets the movement to left, and unlocks the movement to right that may have gotten disabled upon running into a wall
                //        Character.contr = 'L';
                //        if (!Character.rEnable)
                //            Character.rEnable = true;
                //    } else if (!Character.lEnable)
                //        Character.contr = 'I';
                //} else if (ks.IsKeyDown(Keys.Right)) {
                //    //Restricts the jumping
                //    if (ks.IsKeyDown(Keys.Up) && !Character.iAir)
                //        Character.cJump = true;
                //    else if (ks.IsKeyUp(Keys.Up))
                //        Character.cJump = false;

                //    if (Character.rEnable) {
                //        //Sets movement to right and enables left movement if it has been disabled
                //        Character.contr = 'R';
                //        if (!Character.lEnable)
                //            Character.lEnable = true;
                //    } else if (!Character.rEnable)
                //        Character.contr = 'I';
                //} else
                //    Character.contr = 'I';

                //if (ks.IsKeyDown(Keys.Up) && !Character.iAir) {
                //    Character.cJump = true;
                //} else
                //    Character.cJump = false;

                #endregion

                #region Movement Function

                // Calls the function for the players movement and collision
                //Character.playerMovement(gameTime, Character.contr, soundOn);
                Character.groundCollisionFunc(Character, mapHitboxes, graphics);

                //Calls movement and collision function for each zombie in the list
                foreach (Zombies Enemy in Zombie)
                {
                    Enemy.enemyMovement(gameTime, Character, mapHitboxes);
                    Enemy.groundCollisionFunc(Character, mapHitboxes, graphics);
                }

                #endregion
            }
            #endregion

            #region Menu Screen 4 (High Scores)
            if (menuScreen == "HighScore")
            {
                menuBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - (menuBtn.Width / 2), graphics.GraphicsDevice.Viewport.Height / 2 + 180 - (menuBtn.Height / 2), menuBtn.Width, menuBtn.Height);
                easyBox = new Rectangle(50, titleBox.Y + 50, easyButton.Width, easyButton.Height);
                hardBox = new Rectangle(50, easyBox.Y + 50 + easyBox.Height, hardButton.Width, hardButton.Height);
                mediumBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 50 - mediumBox.Width, titleBox.Y + 50, mediumButton.Width, mediumButton.Height);
                insaneBox = new Rectangle(mediumBox.X, mediumBox.Y + 50 + mediumBox.Height, insaneButton.Width, insaneButton.Height);
                if (clickBox.Intersects(menuBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        menuColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        runOnce = false;
                        screenChange("Menu");
                    }
                }
                else if (clickBox.Intersects(easyBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        easyColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        clickReset();
                        difficulty = "Easy";
                    }
                }
                else if (clickBox.Intersects(mediumBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        mediumColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        clickReset();
                        difficulty = "Medium";
                    }
                }
                else if (clickBox.Intersects(hardBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        hardColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        clickReset();
                        difficulty = "Hard";
                    }
                }
                else if (clickBox.Intersects(insaneBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        insaneColor = Color.DarkGray;
                    if (!clickCheck)
                    {
                        clickReset();
                        difficulty = "Insane";
                    }
                }
                else
                {
                    menuColor = Color.White;
                    easyColor = Color.White;
                    mediumColor = Color.White;
                    hardColor = Color.White;
                    insaneColor = Color.White;
                }
            }
            #endregion

            #region Credit Screen
            if (menuScreen == "Credits")
            {
                menuBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - (menuBtn.Width / 2), graphics.GraphicsDevice.Viewport.Height / 2 + 180 - (menuBtn.Height / 2), menuBtn.Width, menuBtn.Height);
                if (clickBox.Intersects(menuBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                        menuColor = Color.DarkGray;
                    if (!clickCheck)
                        screenChange("Menu");
                }
                else
                    menuColor = Color.White;
            }
            #endregion

            #region Death

            if (Zombie.Count > 0)
            {
                foreach (Zombies Enemy in Zombie)
                {
                    //Resets the players position if he's colliding with a zombie

                    if (Character.colBox.Intersects(Enemy.colBox) && !Character.cDead)
                    {
                        gameRunning = false;

                        MediaPlayer.Stop();
                        if (soundOn)
                            gameOver.Play();
                        Character.cDead = true;
                        break;
                    }
                    else if (Character.colBox.Intersects(Enemy.colBox) && Character.cDead)
                    {
                        Enemy.stop = true;
                    }
                    else if (!Character.colBox.Intersects(Enemy.colBox) && Character.cDead)
                    {
                        Enemy.stop = false;
                    }
                }
            }
            if (Character.cDead)
                deathDelay += gameTime.ElapsedGameTime.Milliseconds;
            if (deathDelay >= 2000)
            {
                retryBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - (retryBtn.Width / 2) + 175, graphics.GraphicsDevice.Viewport.Height / 2 - (retryBtn.Height / 2), retryBtn.Width, retryBtn.Height);
                menuBox = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - (menuBtn.Width / 2) - (175), graphics.GraphicsDevice.Viewport.Height / 2 - (menuBtn.Height / 2), menuBtn.Width, retryBtn.Height);
                deathScreen = true;
                deathDelay = 0;
            }
            if (deathScreen)
            {
                if (clickBox.Intersects(retryBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        endChoice = "Retry";
                        retryColor = Color.DarkGray;
                        deadClick = true;
                    }
                }

                else if (clickBox.Intersects(menuBox))
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        endChoice = "Menu";
                        menuColor = Color.DimGray;
                        deadClick = true;
                    }
                }
                else
                {
                    menuColor = Color.White;
                    retryColor = Color.White;
                }

                if (deadClick && ms.LeftButton == ButtonState.Released)
                {
                    deadClick = false;
                    if (clickBox.Intersects(retryBox))
                    {
                        endChoice = "Reset";
                        reset();
                        clickBox = new Rectangle(0, 0, 0, 0);
                    }
                    else if (clickBox.Intersects(menuBox))
                    {
                        endChoice = "Menu";
                        reset();
                        clickBox = new Rectangle(0, 0, 0, 0);
                    }
                    else
                        clickBox = new Rectangle(0, 0, 0, 0);
                }
            }
            #endregion

            if (playerSprite == null)
                LoadContent();

            if (!soundOn)
            {
                MediaPlayer.Volume = 0f;
            }
            else if (soundOn)
                MediaPlayer.Volume = 0.4f;
            base.Update(gameTime);
        }
        #endregion

        protected void clickReset()
        {
            clickBox = new Rectangle(0, 0, 0, 0);
        }

        protected void screenChange(String x)
        {
            clickReset();
            menuScreen = x;
        }

        #region Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            spriteBatch.Begin();


            // Draws ground and clouds
            foreach (Environment cloud in cloudList)
                cloud.Draw(spriteBatch);

            Map.Draw(spriteBatch);

            if (menuScreen != "Game")
            {

                spriteBatch.Draw(box, new Rectangle(0, 0, 800, 480), new Color(0, 0, 0, 75));
                spriteBatch.Draw(menuOverlay, new Rectangle(0, 0, 800, 480), Color.White);
                spriteBatch.Draw(ggLogo, new Rectangle(15, 15, 50, 50), Color.White);
                //Draws menu screen
                #region Menu
                if (menuScreen == "Menu")
                {
                    spriteBatch.Draw(titleLogo, titleBox, null, Color.White,
                        0, new Vector2(titleLogo.Width / 2, titleLogo.Height / 2), SpriteEffects.None, 0);
                    spriteBatch.Draw(startButton, startBox, null, startColor);
                    spriteBatch.Draw(highScoreButton, scoreBox, null, scoreColor);
                    spriteBatch.Draw(creditsButton, credBox, null, creditColor);
                }
                #endregion

                //Draws difficulty selection screen
                #region Difficulty Selection
                else if (menuScreen == "DifSelect")
                {
                    spriteBatch.Draw(easyButton, easyBox, easyColor);
                    spriteBatch.Draw(mediumButton, mediumBox, mediumColor);
                    spriteBatch.Draw(hardButton, hardBox, hardColor);
                    spriteBatch.Draw(insaneButton, insaneBox, insaneColor);
                }
                #endregion

                //Draws highscore screen
                #region High Score
                else if (menuScreen == "HighScore")
                {
                    spriteBatch.Draw(titleLogo, titleBox, null, Color.White,
                        0, new Vector2(titleLogo.Width / 2, titleLogo.Height / 2), SpriteEffects.None, 0);
                    spriteBatch.Draw(menuBtn, menuBox, null, menuColor);
                    spriteBatch.Draw(easyButton, easyBox, null, easyColor);
                    spriteBatch.Draw(hardButton, hardBox, null, hardColor);
                    spriteBatch.Draw(mediumButton, mediumBox, null, mediumColor);
                    spriteBatch.Draw(insaneButton, insaneBox, null, insaneColor);
                    Vector2 textPos = new Vector2(easyBox.X + easyBox.Width, easyBox.Y + easyBox.Height);
                    if (difficulty == "None")
                        spriteBatch.DrawString(gameFont, "Choose a difficulty to view.", textPos, Color.White);
                    else
                    {
                        int count = 0;
                        textPos -= new Vector2(-10, easyButton.Height - 10);
                        highScoreLoad(difficulty);
                        string scoreString;
                        foreach (int i in highScore)
                        {
                            count++;
                            scoreString = Convert.ToString(i);
                            spriteBatch.DrawString(gameFont, count + ": " + scoreString, textPos, Color.White);
                            textPos += new Vector2(0, 20);
                        }
                        highScore.Clear();
                        //runOnce = true;
                    }
                }
                #endregion

                else if (menuScreen == "Credits")
                {
                    spriteBatch.Draw(credits, new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2, credits.Width, credits.Height), null, Color.White,
                        0, new Vector2(credits.Width / 2, credits.Height / 2), SpriteEffects.None, 0);
                    spriteBatch.Draw(menuBtn, menuBox, null, menuColor);

                }
            }
            //Draws game components
            #region Game
            else if (menuScreen == "Game")
            {

                if (getReady < spawnSpeed)
                    spriteBatch.DrawString(gameFont, "Get Ready", new Vector2(10, 10), Color.White);
                else if (getReady >= spawnSpeed)
                    spriteBatch.DrawString(gameFont, "Score: " + scoreTotal / 10, new Vector2(10, 10), Color.White);

                //Spritefinding rectangle
                Rectangle MoveTmp = new Rectangle();

                //Calls the drawfunction for the player and zombie sprites
                Character.drawFunc(spriteBatch, playerSprite, MoveTmp, box);

                foreach (Zombies Enemy in Zombie)
                    Enemy.drawChoice(spriteBatch, playerSprite, MoveTmp, box);

                if (deathScreen)
                {
                    spriteBatch.Draw(box, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), new Color(0, 0, 0, 75));
                    spriteBatch.Draw(retryBtn, retryBox, retryColor);
                    spriteBatch.Draw(menuBtn, menuBox, menuColor);
                    string x = "Score: " + scoreTotal / 10;
                    Vector2 y = gameFont.MeasureString(x);
                    spriteBatch.DrawString(gameFont, x, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, 150), Color.White
                    , 0, y, 1.0f, SpriteEffects.None, 0);
                }
            }
            #endregion

            if (soundOn)
                spriteBatch.Draw(soundEnabled, soundIconRec, Color.Green);
            else if (!soundOn)
                spriteBatch.Draw(soundDisabled, soundIconRec, Color.Green);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        protected void reset()
        {
            Character.cDead = false;
            Zombie.Clear();

            highScoreSave();
            scoreTotal = 0;
            getReady = 0;
            Character.pos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2 - 35);
            Character.lEnable = true;
            Character.rEnable = true;
            runOnce = false;
            gameRunning = true;
            deathScreen = false;
            deathDelay = 0;

            clickBox = new Rectangle(0, 0, 0, 0);
            if (endChoice == "Menu")
                menuScreen = "Menu";
        }

        protected void highScoreLoad(string dif)
        {
            //Reads the file for a specific difficulty if it exists, and puts it into a list
            if (File.Exists("highScore" + dif + ".txt"))
            {
                StreamReader read = new StreamReader("highScore" + dif + ".txt");

                while (!read.EndOfStream)
                {
                    highScore.Add(Convert.ToInt32(read.ReadLine()));
                }
                read.Close();
            }
        }

        protected void highScoreSave()
        {
            highScoreLoad(difficulty);
            //Adds the score and sorts the list biggest first
            highScore.Add(scoreTotal / 10);
            highScore.Sort();
            highScore.Reverse();
            //Removes all but the 10 highest numbers after sorting is done
            while (highScore.Count > 10)
            {
                highScore[10] = 0;
                highScore.Remove(0);
            }
            //Makes a new file (Or overwrite if it exists) and puts the lists content into it
            FileStream file = new FileStream("highScore" + difficulty + ".txt", FileMode.Create);
            StreamWriter write = new StreamWriter(file);
            foreach (int x in highScore)
            {
                write.WriteLine(x);
            }
            write.Close();
            file.Close();
            highScore.Clear();
        }
    }
}