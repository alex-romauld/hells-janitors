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

using SurvivalShooter.Survival;
using System.IO;

namespace SurvivalShooter
{
    
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Viewport FullViewPort;
        SpriteBatch spriteBatch;
        Texture2D square;

        Input p1Input = new Input(PlayerIndex.One);
        KInput kInput = new KInput();

        Debug debug;

        MainMenu mainMenu = new MainMenu();
        PlayerSelector playerSelector = new PlayerSelector();
        GameDirector gameDirector;
        LevelEditor levelEditor;

        Song song1;
        SoundEffect ambience;

        Viewport DefaultViewport;

        SplashScreen splashScreen = new SplashScreen();
        
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            FullViewPort = GraphicsDevice.Viewport;
            Window.Title = "Hell's Janitors";//Clean Up Crew";
        }

        

        protected override void Initialize()
        {

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>("Sprites/Square");
            p1Input.Load(Content);

            mainMenu.Load(Content, GraphicsDevice);
            playerSelector.Load(Content, GraphicsDevice);
            gameDirector = new GameDirector();
            gameDirector.Load(Content, GraphicsDevice);

            levelEditor = new LevelEditor();
            levelEditor.Load(Content, GraphicsDevice);


            debug = new Debug();
            debug.Load(Content);

            song1 = Content.Load<Song>("Music/Shadow Of A Doubt-Another Empire");
            ambience = Content.Load<SoundEffect>("SoundFX/SFX_MenuAmbience");
            //ambience.Play();

            //MediaPlayer.Play(song1);
            MediaPlayer.Volume = 1f;
            //MediaPlayer.IsRepeating = true;
           // IsMouseVisible = true;

//            MediaPlayer.Play(song1);
            splashScreen = new SplashScreen();
            splashScreen.Load(Content, GraphicsDevice);
            LoadOptions();
        }


        private void LoadOptions()
        {
            String path = Content.RootDirectory + Path.DirectorySeparatorChar;
            if (File.Exists(path + "Options" + ".txt"))
            {
                System.IO.Stream _stream = File.OpenRead(path + "Options" + ".txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(_stream);

                sreader.ReadLine();//mainMenu.options.ControllerEnabled = StringToBoolean(sreader.ReadLine());
                mainMenu.options.ControllerVibration = StringToBoolean(sreader.ReadLine());
                mainMenu.options.DrawPlayerCircles = StringToBoolean(sreader.ReadLine());
                int i; int.TryParse(sreader.ReadLine(), out i); mainMenu.options.SoundEffectsVolume = i;
                int a; int.TryParse(sreader.ReadLine(), out a); mainMenu.options.MusicVolume = a;
                mainMenu.options.FullScreen = StringToBoolean(sreader.ReadLine());



                sreader.Close();
                _stream.Close();
            }
        }
        private Boolean StringToBoolean(String _string)
        {
            if (_string == "True")
                return true;
            else
                return false;
        }
        
        protected override void UnloadContent()
        {
        }

        Boolean fKeyUp = true;
        protected override void Update(GameTime gameTime)
        {
            if (graphics.IsFullScreen != mainMenu.options.FullScreen)
            {
                graphics.IsFullScreen = mainMenu.options.FullScreen;
                graphics.ApplyChanges();
            }
            

            if (this.IsActive && splashScreen.active)
                splashScreen.Update(gameTime);
            if (this.IsActive  &&  !splashScreen.active)
            {
               // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)// || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //    this.Exit();
                if (mainMenu.quit)
                {
                    String path = Content.RootDirectory + Path.DirectorySeparatorChar;
                    if (File.Exists(path + "Options" + ".txt"))
                        File.Delete(path + "Options" + ".txt");
                    System.IO.Stream _stream = File.OpenWrite(path + "Options" + ".txt");
                    System.IO.StreamWriter swriter = new System.IO.StreamWriter(_stream);

                        swriter.WriteLine(mainMenu.options.ControllerEnabled);
                        swriter.WriteLine(mainMenu.options.ControllerVibration);
                        swriter.WriteLine(mainMenu.options.DrawPlayerCircles);
                        swriter.WriteLine(mainMenu.options.SoundEffectsVolume);
                        swriter.WriteLine(mainMenu.options.MusicVolume);                        
                        swriter.WriteLine(mainMenu.options.FullScreen);

                    swriter.Close();
                    _stream.Close();

                    this.Exit();
                }
                p1Input.Update(false, false);
                kInput.Update(true);//false);
                debug.Update(gameTime);
                MediaPlayer.Volume = mainMenu.options.MusicVolume / 100f;

                if (mainMenu.InMainMenu)
                {
                    mainMenu.Update(p1Input, kInput);
                    gameDirector.levelLoader.Clear_level();
                }

                for (int i = 0; i < gameDirector.players.Count; i++)
                {
                    gameDirector.players[i].pInput.Update(mainMenu.options.ControllerVibration, gameDirector.players[i].PC);
                    gameDirector.players[i].kInput.Update(gameDirector.players[i].PC);
                }

                //if (Keyboard.GetState().IsKeyDown(Keys.F1))
                //    if (fKeyUp)
                //    {
                //        graphics.IsFullScreen = !graphics.IsFullScreen;
                //        graphics.ApplyChanges();
                //        fKeyUp = false;
                //    }
                //    else
                //        fKeyUp = true;
                for (int p = 0; p < gameDirector.players.Count; p++)
                {
                    if (((gameDirector.players[p].pInput.PauseKey && (mainMenu.options.ControllerEnabled || !mainMenu.SinglePlayer)) || (kInput.GUI_Back && mainMenu.SinglePlayer)) && gameDirector.playing && gameDirector._doPlay && !gameDirector.GameOver)
                    {
                        gameDirector.paused = !gameDirector.paused;
                        gameDirector.InOptions = false;
                        gameDirector.PauseMenu_SelectedOption = 0;
                        if (gameDirector.paused)
                            gameDirector.Player_paused = p;
                    }
                }
                //if (p1Input.Buttons_Press_Start && gameDirector.playing  &&  gameDirector._doPlay)
                //    gameDirector.paused = !gameDirector.paused;

                if (!gameDirector.paused && !mainMenu.InMainMenu && !gameDirector.quit && !mainMenu.ExitToLevelEditor && playerSelector.start)//&& playerSelector.start
                {
                    if (!gameDirector.levelLoader.loaded)
                    {
                        gameDirector.levelLoader.Level_Load(playerSelector.CurrentLevel, playerSelector.CurrentLevelParts, playerSelector.CurrentPr_File);
                    }
                    if (gameDirector.levelLoader.loaded)
                    {
                        //if (gameDirector.paused)
                        //    gameDirector.UpdatePauseMenu();
                        gameDirector.Update(gameTime, playerSelector, mainMenu.options, mainMenu.SinglePlayer);
                        if (gameDirector.players[0].PC && gameDirector.players[0].usingShop)
                            IsMouseVisible = true;
                        else
                            IsMouseVisible = false;
                    }
                }
                else
                    IsMouseVisible = true;

                if (gameDirector.paused && !gameDirector.GameOver)
                {
                    gameDirector.UpdatePauseMenu();
                    if (gameDirector.InOptions)
                        mainMenu.options = gameDirector._options;
                }

                if (mainMenu.ExitToPlayerSelctor)
                {
                    playerSelector = new PlayerSelector();
                    playerSelector.Load(Content, GraphicsDevice);
                    mainMenu.ExitToPlayerSelctor = false;
                }
                if (mainMenu.ExitToLevelEditor)
                {
                    if (!levelEditor.loaded)
                        levelEditor.LoadCustomLevels();
                        
                    levelEditor.Update(gameTime);
                    if (levelEditor.Back)
                    {
                        levelEditor.SaveFadeValue = 0;
                        mainMenu.InMainMenu = true;
                        mainMenu.ExitToLevelEditor = false;
                        levelEditor.Back = false;
                        mainMenu.SelectedOption = 0;
                        playerSelector = new PlayerSelector();
                        playerSelector.Load(Content, GraphicsDevice);
                        gameDirector = new GameDirector();
                        gameDirector.Load(Content, GraphicsDevice);
                        levelEditor.loaded = false;
                    }
                }


                if (!playerSelector.start && !mainMenu.InMainMenu)
                {
                    playerSelector.Update(gameTime, mainMenu.SinglePlayer);
                    IsMouseVisible = true;
                    gameDirector.levelLoader.Clear_level();
                }
                if (playerSelector.start)
                {
                    if (!gameDirector._doPlay)
                    {
                        gameDirector = new GameDirector();
                        gameDirector.Load(Content, GraphicsDevice);
                        gameDirector.ResetGame();
                        gameDirector.GameFadeValue = 255;
                        gameDirector.levelLoader.Level_Load(playerSelector.CurrentLevel, playerSelector.CurrentLevelParts, playerSelector.CurrentPr_File);
                        gameDirector.level_name = playerSelector.Cur_levelName;
                        gameDirector.lb_file = playerSelector.CurrentLb_File;
                    }
                    gameDirector._doPlay = true;
                }

                if (playerSelector.GoBack)
                {
                    mainMenu.InMainMenu = true;
                    mainMenu.SelectedOption = 0;
                    playerSelector = new PlayerSelector();
                    playerSelector.Load(Content, GraphicsDevice);
                }

                if (gameDirector.quit)
                {
                    //  gameDirector.ResetGame();
                    // playerSelector.ResetPlayers();
                    //   gameDirector.levelLoader.Clear_level();
                    //playerSelector.doneSelecting = false;
                    // playerSelector.start = false;
                    mainMenu.InMainMenu = true;
                    mainMenu.SelectedOption = 0;
                    //gameDirector._doPlay = false;
                    //gameDirector.playing = false;
                    //gameDirector.paused = false;
                    //gameDirector.quit = false;
                    playerSelector = new PlayerSelector();
                    playerSelector.Load(Content, GraphicsDevice);
                    GraphicsDevice.Viewport = FullViewPort;
                    gameDirector = new GameDirector();
                    gameDirector.Load(Content, GraphicsDevice);
                    MediaPlayer.Stop();

                }
                if (gameDirector.BackOut && gameDirector.GameOver)
                {
                    for (int i = 0; i < gameDirector.players.Count; i++)
                    {
                        if (gameDirector.TimeTillGOfade >= 3000)
                        {
                            if (gameDirector.players[i].pInput.BackKey || gameDirector.players[i].kInput.GUI_Back)
                            {
                                gameDirector.quit = true;
                                //TimeTillGOfade = 0;
                                //GOfade = 0;
                                //paused = false;
                                //GameOver = false;
                                //newHighScore = false;
                            }
                        }
                    }
                }
                if (gameDirector.GameOver)
                {
                    for (int i = 0; i < gameDirector.players.Count; i++)
                    {
                        gameDirector.players[i].GameOver = gameDirector.GameOver;
                    }
                    gameDirector.UpdateHighScores();
                    gameDirector.TimeTillGOfade += gameTime.ElapsedGameTime.Milliseconds;
                    if (gameDirector.TimeTillGOfade >= 3000)
                        gameDirector.GOfade += 2;
                    gameDirector.paused = false;
                    gameDirector.BackOut = true;
                }
            }
            else
                if (gameDirector.playing)
                    gameDirector.paused = true;
            //Window.Title = "TITLE HERE   |   FPS : " + debug._fps;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (!splashScreen.active)
            {
                gameDirector.Draw(spriteBatch);
                spriteBatch.Begin();

                spriteBatch.End();
                if (mainMenu.InMainMenu)
                    mainMenu.Draw(spriteBatch);
                if (!mainMenu.InMainMenu && !playerSelector.start && !mainMenu.ExitToLevelEditor)
                    playerSelector.Draw(spriteBatch);
                if (mainMenu.ExitToLevelEditor)
                    levelEditor.Draw(spriteBatch);
            }
            debug.UpdateDraw();

            if(splashScreen.active)
                splashScreen.Draw(spriteBatch); 
            base.Draw(gameTime);
            
        }
    }
}
