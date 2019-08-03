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

namespace SurvivalShooter
{
    class MainMenu
    {
        GraphicsDevice graphics;
        ContentManager content;
        SpriteFont font;
        SpriteFont font2;
        Texture2D square;
        public Boolean InMainMenu = true;
        public Boolean SinglePlayer = true;
        public Boolean quit = false;
        public Boolean InOptions = false;
        public Boolean ExitToLevelEditor = false;

        private Boolean LB_BackHighlighted = false;
        private Rectangle LB_Back;
        public Boolean InLeaderBoards = false;

        public int SelectedOption = 0;
        private int Options_Selected = 0;
        public Options options = new Options();
        private List<Texture2D> PlayerTextures = new List<Texture2D>();
        private int SelectedSkin = 0;

        private Rectangle[] mainmenuButtons = new Rectangle[6];
        private Rectangle mainmenuButtonArea;
        private Rectangle[] optionButtons = new Rectangle[8];
        private Boolean[] optionSelected = new Boolean[8];
        private Rectangle OptionsBackButtonRect;

        private Rectangle WarningWindow;
        private String WarningMessage = "";
        private Boolean DisplayMessage = false;
        private Rectangle OKButton;
        private Boolean HighlightOKButton = false;

        private int SelectedLeaderBoard = 0;
        private Input input;
        private KInput kInput;

        public Boolean ExitToPlayerSelctor = false;

        Texture2D menuTexture;
        Texture2D[] menuTextures;
        int MenuFade = 0;

        public MainMenu()
        {

        }
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            square = content.Load<Texture2D>("Sprites/square");
            font = content.Load<SpriteFont>("standardfont");
            font2 = content.Load<SpriteFont>("Fonts/GameInfo");
            options.Load(content);

            PlayerTextures.Add(content.Load<Texture2D>("Sprites/Player/player_pistol"));
            PlayerTextures.Add(content.Load<Texture2D>("Sprites/player_pistol"));
            this.graphics = graphics;
            this.content = content;

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                options.ControllerEnabled = true;
            else
                options.ControllerEnabled = false;

            menuTextures = new Texture2D[4];
            menuTextures[0] = content.Load<Texture2D>("Menu/menu1");
            menuTextures[1] = content.Load<Texture2D>("Menu/menu2");
            menuTextures[2] = content.Load<Texture2D>("Menu/menu3");
            menuTextures[3] = content.Load<Texture2D>("Menu/menu4");
            menuTexture = menuTextures[1];
        }
        public void Update(Input input, KInput kInput)
        {
            this.input = input;
            this.kInput = kInput;
            if (!ExitToLevelEditor)
            {
                if (!DisplayMessage)
                {
                    if (!InOptions && !InLeaderBoards)
                    {
                        Options_Selected = 0;
                        if (input.SelectKey || kInput.GUI_Select && SelectedOption >= 0)
                        {
                            if (SelectedOption == 0)
                            {
                                SinglePlayer = true;
                                InMainMenu = false;
                                ExitToPlayerSelctor = true;
                                MenuFade = 0;
                            }
                            if (SelectedOption == 1)
                            {
                                if (GamePad.GetState(PlayerIndex.One).IsConnected
                                    || GamePad.GetState(PlayerIndex.Two).IsConnected
                                    || GamePad.GetState(PlayerIndex.Three).IsConnected
                                    || GamePad.GetState(PlayerIndex.Four).IsConnected)
                                {
                                    SinglePlayer = false;
                                    InMainMenu = false;
                                    ExitToPlayerSelctor = true;
                                    MenuFade = 0;
                                }
                                else
                                {
                                    SelectedOption = -1;
                                    DisplayMessage = true;
                                    WarningMessage = "Split Screen requires that\nyou use 1 or more GamePads.\nPlease connect a GamePad\nbefore playing Split Screen.";
                                }
                            }

                            if (SelectedOption == 2)
                            {
                                ExitToLevelEditor = true;
                                InMainMenu = false;
                                MenuFade = 0;
                            }
                            if (SelectedOption == 3)
                            {
                                InLeaderBoards = true;
                            }


                            if (SelectedOption == 4)
                            {
                                InOptions = true;
                                return;
                            }
                            if (SelectedOption == 5)
                                quit = true;
                        }
                        if ((input.GUI_Down || kInput.GUI_Down) && SelectedOption < mainmenuButtons.Length - 1)
                            SelectedOption++;
                        if ((input.GUI_Up || kInput.GUI_Up) && SelectedOption > 0)
                            SelectedOption--;
                    }
                    if (InOptions)
                    {
                        SelectedOption = 0;
                        if (input.SelectKey || kInput.GUI_Select)
                        {
                            if (Options_Selected == 0)
                                options.ControllerEnabled = !options.ControllerEnabled;
                            if (Options_Selected == 2)
                                options.DrawPlayerCircles = !options.DrawPlayerCircles;
                        }
                        //if (Options_Selected == 2)
                        //{
                        //    if ((input.GUI_Right || kInput.GUI_Right) && SelectedSkin < PlayerTextures.Count - 1)
                        //        SelectedSkin++;
                        //    if ((input.GUI_Left || kInput.GUI_Left) && SelectedSkin > 0)
                        //        SelectedSkin--;
                        //}
                        if (Options_Selected == 4)
                        {
                            if ((input.GUI_Hold_Right || kInput.GUI_Right_Hold) && options.MusicVolume < 100)
                                options.MusicVolume++;
                            if ((input.GUI_Hold_Left || kInput.GUI_Left_Hold) && options.MusicVolume > 0)
                                options.MusicVolume--;
                        }
                        if (Options_Selected == 3)
                        {
                            if ((input.GUI_Hold_Right || kInput.GUI_Right_Hold) && options.SoundEffectsVolume < 100)
                                options.SoundEffectsVolume++;
                            if ((input.GUI_Hold_Left || kInput.GUI_Left_Hold) && options.SoundEffectsVolume > 0)
                                options.SoundEffectsVolume--;
                        }
                        if (input.SelectKey || kInput.GUI_Select)
                        {
                            if (Options_Selected == 1)
                                options.ToggleVibrateController();
                        }
                        if (input.SelectKey || kInput.GUI_Select)
                        {
                            if (Options_Selected == 5)
                                options.ToggleFullScreen();
                        }
                        if (Options_Selected == 6 && kInput.GUI_Select)
                        {
                            InOptions = false;
                            SelectedOption = -1;
                            MenuFade = 0;
                        }


                        Rectangle SFXBar = new Rectangle(graphics.Viewport.Width / 2 + 265, graphics.Viewport.Height / 2 + 37, 350, 25);
                        if (kInput.MouseRect.Intersects(SFXBar))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                options.SoundEffectsVolume = (int)(((kInput.MouseRect.X - SFXBar.X + 2) / (float)SFXBar.Width) * 100); 
                            }
                        }

                        Rectangle MusicBar = new Rectangle(graphics.Viewport.Width / 2 + 265, graphics.Viewport.Height / 2 + 77, 350, 25);
                        if (kInput.MouseRect.Intersects(MusicBar))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                 options.MusicVolume = (int)(((kInput.MouseRect.X - MusicBar.X + 2) / (float)MusicBar.Width) * 100); 
                            }
                        }





                        if (input.BackKey || kInput.GUI_Back)
                        {
                            InOptions = false;
                            MenuFade = 0;
                        }
                        if ((input.GUI_Down || kInput.GUI_Down) && Options_Selected < 6)
                            Options_Selected++;
                        if ((input.GUI_Up || kInput.GUI_Up) && Options_Selected > 0)
                            Options_Selected--;

                    }
                    if (InLeaderBoards)
                    {
                        if (input.SelectKey || kInput.GUI_Select)
                        {
                            if (LB_BackHighlighted)
                            {
                                InLeaderBoards = false;
                                SelectedOption = -1;
                                LB_BackHighlighted = false;
                                MenuFade = 0;
                            }
                        }
                        if (input.BackKey || kInput.GUI_Back)
                        {
                            InLeaderBoards = false;
                            SelectedOption = -1;
                            LB_BackHighlighted = false;
                            MenuFade = 0;
                        }

                    }
                }
                if (DisplayMessage)
                {
                     if (input.SelectKey || kInput.GUI_Select)
                    {
                        if (HighlightOKButton)
                        {
                            HighlightOKButton = false;
                            DisplayMessage = false;
                        }
                    }
                }
                UpdateMouseSelection();

            }
        }

        Rectangle Prev_mouseRect;
        private void UpdateMouseSelection()
        {
            
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (InMainMenu)
            {
                if (!DisplayMessage)
                {
                    if (!InOptions && !InLeaderBoards)
                    {
                        if (Prev_mouseRect != mouseRect)
                        {
                            for (int i = 0; i < mainmenuButtons.Length; i++)
                            {
                                if (mouseRect.Intersects(mainmenuButtons[i]))
                                {
                                    SelectedOption = i;
                                }
                                if (i >= mainmenuButtons.Length - 1 && SelectedOption >= 0 && !mouseRect.Intersects(mainmenuButtonArea))
                                {
                                    if (!mouseRect.Intersects(mainmenuButtons[SelectedOption]))
                                        SelectedOption = -1;
                                }
                            }
                            Prev_mouseRect = mouseRect;
                        }
                    }
                    else if (InOptions)
                    {
                        if (Prev_mouseRect != mouseRect)
                        {
                            for (int i = 0; i < optionButtons.Length; i++)
                            {
                                if (mouseRect.Intersects(optionButtons[i]))
                                {
                                    Options_Selected = i;
                                }
                                if (i >= optionButtons.Length - 1 && Options_Selected >= 0 && !mouseRect.Intersects(optionButtons[Options_Selected]))
                                    Options_Selected = -1;
                            }
                        }
                        Prev_mouseRect = mouseRect;
                    }
                    else if (InLeaderBoards)
                    {
                        if (Prev_mouseRect != mouseRect)
                        {
                            if (mouseRect.Intersects(LB_Back))
                                LB_BackHighlighted = true;
                            else
                                LB_BackHighlighted = false;
                        }
                        Prev_mouseRect = mouseRect;
                    }
                }

                if (DisplayMessage)
                    HighlightOKButton = mouseRect.Intersects(OKButton);
           
                

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (!InOptions && !InLeaderBoards)
            {

                if (MenuFade == 0)
                {
                    Random random = new Random();
                    menuTexture = menuTextures[random.Next(menuTextures.Length)];
                }
                if (MenuFade < 255)
                    MenuFade++;
                spriteBatch.Draw(menuTexture, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(MenuFade, MenuFade, MenuFade, MenuFade));

                for (int i = 0; i < mainmenuButtons.Length; i++)
                    if (i == SelectedOption)
                        spriteBatch.Draw(square, mainmenuButtons[i], new Color(111, 111, 111, 100));//Color.LightGray);
                
                int X = 40;
                int Y = graphics.Viewport.Height / 2 + 20;
                int Spacing = (int)font.MeasureString(".").Y + 10;
                int B_width = 5;
                spriteBatch.DrawString(font, "Hell's Janitors"/*"-||-   ]|[   -||-   |[_   ]{-"*/, new Vector2(X, graphics.Viewport.Height / 2 - 100), Color.White);

                int TextOffsetX = 5;

                mainmenuButtons[0] = new Rectangle(X - B_width, Y, (int)font.MeasureString("Single Player").X + (B_width * 2), (int)font.MeasureString(".").Y);
                spriteBatch.DrawString(font, "Single Player", new Vector2(mainmenuButtons[0].X + TextOffsetX, Y), Color.White);

                mainmenuButtons[1] = new Rectangle(X - B_width, Y + (Spacing * 1), (int)font.MeasureString("Single Player").X + (B_width * 2), (int)font.MeasureString(".").Y);
                spriteBatch.DrawString(font, "Split Screen", new Vector2(mainmenuButtons[1].X + TextOffsetX, mainmenuButtons[1].Y), Color.White);

                mainmenuButtons[2] = new Rectangle(X - B_width, Y + (Spacing * 2), (int)font.MeasureString("Single Player").X + (B_width * 2), (int)font.MeasureString(".").Y);
                spriteBatch.DrawString(font, "Level Editor", new Vector2(mainmenuButtons[2].X + TextOffsetX, mainmenuButtons[2].Y), Color.White);

                mainmenuButtons[3] = new Rectangle(X - B_width, Y + (Spacing * 3), (int)font.MeasureString("Single Player").X + (B_width * 2), (int)font.MeasureString(".").Y);
                spriteBatch.DrawString(font, "Leaderboards", new Vector2(mainmenuButtons[3].X + TextOffsetX, mainmenuButtons[3].Y), Color.White);

                mainmenuButtons[4] = new Rectangle(X - B_width, Y + (Spacing * 4), (int)font.MeasureString("Single Player").X + (B_width * 2), (int)font.MeasureString(".").Y);
                spriteBatch.DrawString(font, "Options", new Vector2(mainmenuButtons[4].X + TextOffsetX, mainmenuButtons[4].Y), Color.White);

                mainmenuButtons[5] = new Rectangle(X - B_width, Y + (Spacing * 5), (int)font.MeasureString("Single Player").X + (B_width * 2), (int)font.MeasureString(".").Y);
                spriteBatch.DrawString(font, "Quit", new Vector2(mainmenuButtons[5].X + TextOffsetX, mainmenuButtons[5].Y), Color.White);

                mainmenuButtonArea = new Rectangle(mainmenuButtons[0].X, mainmenuButtons[0].Y,
                    mainmenuButtons[0].Width, mainmenuButtons[mainmenuButtons.Length - 1].Y - mainmenuButtons[0].Y + mainmenuButtons[0].Height);
            }
            if (InOptions)
            {
                spriteBatch.DrawString(font, "Options", new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("Options").X / 2,
                   graphics.Viewport.Height / 2 - 215), Color.White);

                int ButtonWidth = 500;
                optionButtons[0] = new Rectangle(graphics.Viewport.Width / 2 - ButtonWidth / 2,
                        graphics.Viewport.Height / 2 - 90, ButtonWidth, (int)font.MeasureString(".").Y);//Gamepad Enabled
                optionButtons[2] = new Rectangle(graphics.Viewport.Width / 2 - ButtonWidth / 2,
                        graphics.Viewport.Height / 2 - 10, ButtonWidth, (int)font.MeasureString(".").Y);//Player Circles
                //optionButtons[2] = new Rectangle(graphics.Viewport.Width / 2 - 33, graphics.Viewport.Height / 2 + 10, 75, 75);

                optionButtons[4] = new Rectangle(graphics.Viewport.Width / 2 - ButtonWidth / 2,
                        graphics.Viewport.Height / 2 + 70, ButtonWidth, (int)font.MeasureString(".").Y); //Music Volume

                optionButtons[3] = new Rectangle(graphics.Viewport.Width / 2 - ButtonWidth / 2,
                        graphics.Viewport.Height / 2 + 30, ButtonWidth, (int)font.MeasureString(".").Y);//SFX Volume
                optionButtons[1] = new Rectangle(graphics.Viewport.Width / 2 - ButtonWidth / 2,
                        graphics.Viewport.Height / 2 - 50, ButtonWidth, (int)font.MeasureString(".").Y); //GamePadVibration
                optionButtons[5] = new Rectangle(graphics.Viewport.Width / 2 - ButtonWidth / 2,
                        graphics.Viewport.Height / 2 + 110, ButtonWidth, (int)font.MeasureString(".").Y);//Fullscreen
                optionButtons[6] = new Rectangle(50, graphics.Viewport.Height - 100, 250, 50);

                for (int i = 0; i < optionButtons.Length; i++)
                {
                    if(Options_Selected == i)
                    spriteBatch.Draw(square, optionButtons[i], Color.Gray);
                }

               
                spriteBatch.DrawString(font, "GamePad : " + TF_Label(options.ControllerEnabled, "Disabled", "Enabled"), new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("GamePad : " + TF_Label(options.ControllerEnabled, "Disabled", "Enabled")).X / 2,
                        graphics.Viewport.Height / 2 - 90), Color.White);

                spriteBatch.DrawString(font, "GamePad Vibration : " + TF_Label(options.ControllerVibration, "Disabled", "Enabled"), new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("GamePad Vibration : " + TF_Label(options.ControllerVibration, "Disabled", "Enabled")).X / 2,
                        graphics.Viewport.Height / 2 - 50), Color.White);

                    spriteBatch.DrawString(font, "Player Circles : " + TF_Label(options.DrawPlayerCircles, "Disabled", "Enabled"), new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("Player Circles : " + TF_Label(options.DrawPlayerCircles, "Disabled", "Enabled")).X / 2,
                        graphics.Viewport.Height / 2 - 10), Color.White);

                //if (Options_Selected == 2)
                //    spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 - 33, graphics.Viewport.Height / 2 + 10, 75, 75), new Color(50, 50, 50, 10));
                //spriteBatch.Draw(PlayerTextures[SelectedSkin], new Rectangle(graphics.Viewport.Width / 2 - 33, graphics.Viewport.Height / 2 + 10, 75, 75), Color.White);
                //for (int i = 0; i < PlayerTextures.Count; i++)
                //{
                //    if (SelectedSkin != i)
                //    {
                //        if (i > SelectedSkin)
                //        {
                //            spriteBatch.Draw(PlayerTextures[i], new Rectangle(graphics.Viewport.Width / 2 + 60, graphics.Viewport.Height / 2 + 23, 40, 40), Color.White);
                //        }
                //        if (i < SelectedSkin)
                //        {
                //            spriteBatch.Draw(PlayerTextures[i], new Rectangle(graphics.Viewport.Width / 2 - 99, graphics.Viewport.Height / 2 + 23, 40, 40), Color.White);
                //        }
                //    }
                //}

                spriteBatch.DrawString(font, "Music Volume : " + options.MusicVolume + "%", new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("Music Volume : " + options.MusicVolume + "%").X / 2,
                        graphics.Viewport.Height / 2 + 70), Color.White);
                spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 + 265, graphics.Viewport.Height / 2 + 77, 350, 25), Color.White);
                spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 + 265, graphics.Viewport.Height / 2 + 77, (int)(options.MusicVolume / 100f * 350), 25), Color.Gray);

                spriteBatch.DrawString(font, "SFX Volume : " + options.SoundEffectsVolume + "%", new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("SFX Volume : " + options.SoundEffectsVolume + "%").X / 2,
                        graphics.Viewport.Height / 2 + 30), Color.White);
                spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 + 265, graphics.Viewport.Height / 2 + 37, 350, 25), Color.White);
                spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 + 265, graphics.Viewport.Height / 2 + 37, (int)(options.SoundEffectsVolume / 100f * 350), 25), Color.Gray);



                

                spriteBatch.DrawString(font, "Full Screen : " + TF_Label(options.FullScreen, "Disabled", "Enabled"), new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("Full Screen : " + TF_Label(options.FullScreen, "Disabled", "Enabled")).X / 2,
                       graphics.Viewport.Height / 2 + 110), Color.White);

                OptionsBackButtonRect = new Rectangle(50, graphics.Viewport.Height - 100, 250, 50);
                //if(Options_Selected == 7)
                //    spriteBatch.Draw(square, optionButtons[7], Color.Gray);
             //   else
                  //  spriteBatch.Draw(square, optionButtons[5], Color.White);
                spriteBatch.DrawString(font, "Back", new Vector2(50 + 125 - font.MeasureString("Back").X / 2, graphics.Viewport.Height - 100 +  25 - font.MeasureString(".").Y / 2), Color.White);


                
            }
            if (InLeaderBoards)
            {
                


                spriteBatch.DrawString(font, "Leaderboards", new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("Leaderboards").X / 2,
                   graphics.Viewport.Height / 2 - 215), Color.White);

                
                
                List<String> lb_levels = new List<string>();
                List<FileInfo> lb_file = new List<FileInfo>();
                //#LBlevels
                lb_levels.Add("The Giant");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/The Giant/The Giant_lb.txt"));

                lb_levels.Add("Inside");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/Inside/Inside_lb.txt"));

                lb_levels.Add("Facility");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/Facility/Facility_lb.txt"));

                lb_levels.Add("The Origin");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/The Origin/The Origin_lb.txt"));

                lb_levels.Add("Killing Floor");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/Killing Floor/Killing Floor_lb.txt"));

                lb_levels.Add("Swamp");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/Swamp/Swamp_lb.txt"));

                lb_levels.Add("Lock Box");
                lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/Lock Box/Lock Box_lb.txt"));

                //lb_levels.Add("Graveyard");
                //lb_file.Add(new FileInfo(content.RootDirectory + "/Levels/level2_lb" + ".txt"));


                DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels");
                if (directory.Exists)
                {
                    DirectoryInfo[] levels = directory.GetDirectories();
                    foreach (DirectoryInfo dir in levels)//For each folder in the custom levels folder
                    {
                        foreach (FileInfo file in dir.GetFiles())//for each file in the level folder
                        {
                            if (file.Name.Contains("_lb"))
                            {
                                lb_levels.Add(dir.Name);
                                lb_file.Add(file);
                                //File = leaderboard text file
                            }
                        }
                    }

                }
                if (SelectedLeaderBoard > lb_file.Count - 1)
                    SelectedLeaderBoard = lb_file.Count - 1;
                if (SelectedLeaderBoard < 0)
                    SelectedLeaderBoard = 0;

                    spriteBatch.DrawString(font, lb_levels[SelectedLeaderBoard], new Vector2(graphics.Viewport.Width / 2 - font.MeasureString(lb_levels[SelectedLeaderBoard]).X / 2, graphics.Viewport.Height / 2 - 175), Color.White);

                    int NumOfScores = File.ReadAllLines(lb_file[SelectedLeaderBoard].FullName).Length;
                    FileStream fs = new FileStream(lb_file[SelectedLeaderBoard].FullName, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);


                    //System.Console.WriteLine(sr.ReadLine());

                    for (int i = 0; i < NumOfScores; i++)
                    {
                        int score; int.TryParse(sr.ReadLine(), out score);
                        if(score != 0)
                        spriteBatch.DrawString(font, "" + score, new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("" + score).X / 2, graphics.Viewport.Height / 2 - 125 + (25 * i)), Color.White);
                        if (i == 0 && score == 0)
                            spriteBatch.DrawString(font, "No Leaderboard Data", new Vector2(graphics.Viewport.Width / 2 - font.MeasureString("No Leaderboard Data").X / 2, graphics.Viewport.Height / 2 - 125 + (25 * i)), Color.White);
                    }
                    

                    fs.Close();
                    sr.Close();

                    if ((input.GUI_Right  || kInput.GUI_Right) && SelectedLeaderBoard < lb_levels.Count - 1)
                        SelectedLeaderBoard++;
                    if ((input.GUI_Left || kInput.GUI_Left) && SelectedLeaderBoard > 0)
                        SelectedLeaderBoard--;

                    Rectangle mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
                    if (kInput.LMB)
                    {
                        if (mouseRect.Intersects(new Rectangle(graphics.Viewport.Width / 2 - 200, graphics.Viewport.Height / 2 - 170, 30, 30))
                             && SelectedLeaderBoard > 0)
                            SelectedLeaderBoard--;
                        if (mouseRect.Intersects(new Rectangle(graphics.Viewport.Width / 2 + 170, graphics.Viewport.Height / 2 - 170, 30, 30)) 
                            && SelectedLeaderBoard < lb_levels.Count - 1)
                            SelectedLeaderBoard++;
                    }
                    if (SelectedLeaderBoard > 0)
                    {
                        spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 - 200, graphics.Viewport.Height / 2 - 170, 30, 30), Color.LightGray);
                        spriteBatch.DrawString(font, "<", new Vector2(graphics.Viewport.Width / 2 - 195, graphics.Viewport.Height / 2 - 175), Color.Black);
                    }
                    if (SelectedLeaderBoard < lb_levels.Count - 1)
                    {
                        spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width / 2 + 170, graphics.Viewport.Height / 2 - 170, 30, 30), Color.LightGray);
                        spriteBatch.DrawString(font, ">", new Vector2(graphics.Viewport.Width / 2 + 195 - (int)font.MeasureString(">").X, graphics.Viewport.Height / 2 - 175), Color.Black);
                    }
                
                LB_Back = new Rectangle(50,graphics.Viewport.Height - 100, 250,50);
                if(LB_BackHighlighted)
                 spriteBatch.Draw(square, LB_Back, Color.Gray);

                spriteBatch.DrawString(font, "Back", new Vector2(50 + 125 - font.MeasureString("Back").X / 2, graphics.Viewport.Height - 100 +  25 - font.MeasureString(".").Y / 2), Color.White);

            }

            if (DisplayMessage)
            {
                int w = 500;
                int h = 250;
                WarningWindow = new Rectangle(graphics.Viewport.Width / 2 - (w / 2), graphics.Viewport.Height / 2 - (h / 2), w, h);
                OKButton = new Rectangle(WarningWindow.X + WarningWindow.Width - 150, WarningWindow.Y + WarningWindow.Height - 50, 125, 40);

                spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(40, 40, 40, 190));
                spriteBatch.Draw(square, WarningWindow, Color.White);
                spriteBatch.DrawString(font, WarningMessage, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 10), Color.Black);
                if (HighlightOKButton)
                    spriteBatch.Draw(square, OKButton, Color.Gray);
                spriteBatch.DrawString(font, "OK", new Vector2((OKButton.X + OKButton.Width / 2) - (font.MeasureString("OK").X / 2), OKButton.Y), Color.Black);
            }

            spriteBatch.End();

        }


        private String TF_Label(Boolean toggle, String falseLabel, String trueLabel)
        {
            if (toggle)
                return trueLabel;
            else 
                return falseLabel;
        }
    }
}
