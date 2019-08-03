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

using SurvivalShooter.StandardGame;
using Microsoft.Xna.Framework.Storage;
//using TApplication = System.Windows.Forms.Application;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;

namespace SurvivalShooter
{
    class PlayerSelector
    {
        private GraphicsDevice graphics;
        private ContentManager content;
        private SpriteFont font;
        private Texture2D background;

        public Boolean Add_Player1 = false;//true
        public Boolean Add_Player2 = false;
        public Boolean Add_Player3 = false;
        public Boolean Add_Player4 = false;

        private Boolean player1Ready = false;//false;
        private Boolean player2Ready = false;//false;
        private Boolean player3Ready = false;//false;
        private Boolean player4Ready = false;//false;

        public int Player1_Screen = 0;
        public int Player2_Screen = 1;
        public int Player3_Screen = 2;
        public int Player4_Screen = 3;
        private Boolean ScreenSelection = false;
        private int ScreenSelection_Option = 0;
        private Input ScreenSelectionInput;
        private Boolean Player1Counted = false;
        private Boolean Player2Counted = false;
        private Boolean Player3Counted = false;
        private Boolean Player4Counted = false;
        private int CountedPlayers = 0;
        private Boolean AllPlayersCounted = false;

        public int[] playerClass = new int[4];

        private KInput KInput = new KInput();
        private Input Input_player1 = new Input(PlayerIndex.One);
        private Input Input_player2 = new Input(PlayerIndex.Two);
        private Input Input_player3 = new Input(PlayerIndex.Three);
        private Input Input_player4 = new Input(PlayerIndex.Four);

        public Boolean GoBack = false;
        public Boolean doneSelecting = false;
        private int Countdown = 4000;//10000;
        public Boolean start = false;

        private Boolean[] ADD_players = new bool[4];
        private Boolean[] READY_players = new bool[4];

        private int CurrentSelected = 0;
        public Texture2D CurrentLevel;
        private List<Texture2D> Levels = new List<Texture2D>();
        public Texture2D CurrentLevelParts;
        private List<Texture2D> LevelPieces = new List<Texture2D>();
        private List<String> LevelName = new List<string>();
        public String Cur_levelName = "";
        private Texture2D[] LevelTiles;
        private Texture2D[] PreviewTile;
        private Rectangle[] PreviewTileRect;
        private Texture2D[] PreviewTilePiece;
        private Rectangle[] PreviewTilePieceRect;

        private List<FileInfo> LbFiles = new List<FileInfo>();
        public FileInfo CurrentLb_File;
        private List<FileInfo> PrFiles = new List<FileInfo>();
        public FileInfo CurrentPr_File;

        private Texture2D square;

        private Boolean ShowDeleteWarning = false;
        private Rectangle[] WarningButtons = new Rectangle[2];
        private int WarningButtonSelected = -1;
        private Rectangle[] DeleteLevelButton = new Rectangle[0];
        private Texture2D DeleteTexture;
        private int SelectedDeleteButon = -1;
        private int CustomLevels = 0;

        private Boolean SelectingLevel = true;
        private int ScrollY = 0;
        private MouseState PrevScrollState;

        //Level Selection
        private int Level_CurrentSelected = 0;


        private Rectangle[] PC_Button = new Rectangle[4];
        private int PC_Selected = -1;
        private Rectangle[] LevelSelect_Button = new Rectangle[3];

        TileManager tileManager = new TileManager();
        private Texture2D lineTexture;
        private Boolean LevelChange = true;



        Texture2D SentryTexture;
        Texture2D SentryStand;
        Texture2D StimGasIcon;
        Texture2D DamageBoostIcon;
        Texture2D AmmoBagIcon;

        public PlayerSelector()
        {

        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;
            this.content = content;


            SentryTexture = content.Load<Texture2D>("Sprites/SentryGun");
            SentryStand = content.Load<Texture2D>("Sprites/SentryGunStand");
            StimGasIcon = content.Load<Texture2D>("Sprites/HealthKick");
            DamageBoostIcon = content.Load<Texture2D>("Sprites/DamageBoost");
            AmmoBagIcon = content.Load<Texture2D>("Sprites/Misc/AmmoBag");

            background = content.Load<Texture2D>("Sprites/Pistol_GUI");
            font = content.Load<SpriteFont>("standardfont");
            lineTexture = content.Load<Texture2D>("Sprites/square");

            DeleteTexture = content.Load<Texture2D>("Sprites/RedX");

            Input_player1.Load(content);
            square = content.Load<Texture2D>("Sprites/square");

            tileManager.Load(content);

            LevelTiles = new Texture2D[8];
            LevelTiles[0] = content.Load<Texture2D>("Sprites/GrassTile");
            LevelTiles[1] = content.Load<Texture2D>("Sprites/Tiles/Wall");
            LevelTiles[2] = content.Load<Texture2D>("Sprites/WoodFloorTile");
            LevelTiles[3] = content.Load<Texture2D>("Sprites/Tiles/Door");
            LevelTiles[4] = content.Load<Texture2D>("Sprites/Tiles/WeaponTile");
            LevelTiles[5] = content.Load<Texture2D>("Sprites/Tiles/RandomWeaponTile");
            LevelTiles[6] = content.Load<Texture2D>("Sprites/Tiles/JukeBox");
            LevelTiles[7] = content.Load<Texture2D>("Sprites/Zombies/SpawnHole");

            //#LEVELS
            //Levels.Add(content.Load<Texture2D>("Level"));
            //LevelPieces.Add(content.Load<Texture2D>("Level_Pieces"));
            //LevelName.Add("Facility");
            //LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/level1_lb" + ".txt"));
            //PrFiles.Add

            //Levels.Add(content.Load<Texture2D>("Levels/Level2"));
            //LevelPieces.Add(content.Load<Texture2D>("Levels/Level2_Pieces"));
            //LevelName.Add("Graveyard");
            //LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/level2_lb" + ".txt"));
            
            Levels.Add(content.Load<Texture2D>("Levels/The Giant/The Giant_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/The Giant/The Giant_File2"));
            LevelName.Add("The Giant");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/The Giant/The Giant_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/The Giant/The Giant_pr.txt"));

            Levels.Add(content.Load<Texture2D>("Levels/Inside/Inside_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/Inside/Inside_File2"));
            LevelName.Add("Inside");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Inside/Inside_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Inside/Inside_pr.txt"));

            Levels.Add(content.Load<Texture2D>("Levels/Facility/Facility_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/Facility/Facility_File2"));
            LevelName.Add("Facility");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Facility/Facility_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Facility/Facility_pr.txt"));

            Levels.Add(content.Load<Texture2D>("Levels/The Origin/The Origin_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/The Origin/The Origin_File2"));
            LevelName.Add("The Origin");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/The Origin/The Origin_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/The Origin/The Origin_pr.txt"));

            Levels.Add(content.Load<Texture2D>("Levels/Killing Floor/Killing Floor_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/Killing Floor/Killing Floor_File2"));
            LevelName.Add("Killing Floor");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Killing Floor/Killing Floor_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Killing Floor/Killing Floor_pr.txt"));

            Levels.Add(content.Load<Texture2D>("Levels/Swamp/Swamp_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/Swamp/Swamp_File2"));
            LevelName.Add("Swamp");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Swamp/Swamp_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Swamp/Swamp_pr.txt"));

            Levels.Add(content.Load<Texture2D>("Levels/Lock Box/Lock Box_File1"));
            LevelPieces.Add(content.Load<Texture2D>("Levels/Lock Box/Lock Box_File2"));
            LevelName.Add("Lock Box");
            LbFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Lock Box/Lock Box_lb.txt"));
            PrFiles.Add(new FileInfo(content.RootDirectory + "/Levels/Lock Box/Lock Box_pr.txt"));

            LoadCustomLevels(content);

            LevelSelect_Button = new Rectangle[Levels.Count];
            SelectingLevel = false;
        }

        private void LoadCustomLevels(ContentManager content)
        {
            

         //   String T = "Texture2D";
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels");
            if (dir.Exists)
            {
                Dictionary<String, Texture2D> _result = new Dictionary<String, Texture2D>();
                DirectoryInfo[] LevelDir = dir.GetDirectories();
                //System.Console.WriteLine(_files[0].Name);
                foreach (DirectoryInfo _dir in LevelDir)
                {
                    //System.Console.WriteLine(_dir.Name);
                    DirectoryInfo _levelDirectory = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels" + "\\" + _dir.Name);
                    FileInfo[] files = _levelDirectory.GetFiles("*.*");
                    if (_levelDirectory.Exists)
                    {
                        foreach (FileInfo file in files)
                        {
                            String FileName = file.Name;
                            if (!FileName.Contains("_lb")  &&  !FileName.Contains("_pr"))
                            {
                                FileStream fs = new FileStream(file.FullName, FileMode.Open);
                                Texture2D t = Texture2D.FromStream(graphics, fs);

                                if (FileName.Contains("_File1"))
                                {
                                    Levels.Add(t);
                                    String _LevelName = Path.GetFileNameWithoutExtension(file.Name);
                                    String _temp = _LevelName;
                                    String _Name = _LevelName.Remove(_temp.Length - 6, 6);
                                    LevelName.Add(_Name);
                                    CustomLevels++;
                                    DeleteLevelButton = new Rectangle[CustomLevels];
                                }
                                if (FileName.Contains("_File2"))
                                {
                                    LevelPieces.Add(t);
                                }
                                fs.Close();
                            }
                            else if(FileName.Contains("_lb"))
                            {
                                LbFiles.Add(file);
                               // System.Console.WriteLine(file.Name);
                                //ReadLeaderBoardData ... maybe
                            }
                            else if (FileName.Contains("_pr"))
                            {
                                PrFiles.Add(file);
                            }
                        }
                    }
                }
                /*
                Dictionary<String, Texture2D> result = new Dictionary<String, Texture2D>();
                FileInfo[] files = dir.GetFiles("*.*");
                foreach (FileInfo file in files)
                {
                    FileStream fs = new FileStream(file.FullName, FileMode.Open);
                    Texture2D t = Texture2D.FromStream(graphics, fs);
                    String FileName = file.Name;
                    if (FileName.Contains("_File1"))
                    {
                        Levels.Add(t);
                        String levelName = Path.GetFileNameWithoutExtension(file.Name);
                        String _temp = levelName;
                        levelName.Remove(_temp.Length - 6, 6);
                        String _Name = levelName.Remove(_temp.Length - 6, 6);
                        LevelName.Add(_Name);
                    }
                    if (FileName.Contains("_File2"))
                    {
                        LevelPieces.Add(t);

                    }
                    fs.Close();
                    
                    //StorageContainer storageContainer = new StorageContainer();
                    //Stream steam = storageContainer.OpenFile(files[0].FullName, FileMode.Open);
                    
                 //   string key = Path.GetFileNameWithoutExtension(file.Name);
                   // t = content.Load<Texture2D>(content.RootDirectory + "/" + "CustomLevels" + "/" + key);
                  //  temp = "" + key;
                }


                */
            }

            
        }


        private Boolean SinglePlayer;
        public void Update(GameTime gameTime, Boolean SinglePlayer)
        {
                this.SinglePlayer = SinglePlayer;
                if (SinglePlayer)
                    Add_Player1 = true;

                KInput.Update(true);
                Input_player1.Update(false, false);
                Input_player2.Update(false, false);
                Input_player3.Update(false, false);
                Input_player4.Update(false, false);

                if (!ScreenSelection)
                {
                    int minOptions;
                    if (SinglePlayer)
                        minOptions = 0;
                    else
                        minOptions = 2;
                    if (!SelectingLevel)
                    {
                        PlayerSelection(gameTime);
                        if (Input_player1.GUI_Down || KInput.GUI_Down)
                            if (PC_Selected < PC_Button.Length - 1)
                                PC_Selected++;

                        if (Input_player1.GUI_Up || KInput.GUI_Up)
                            if (PC_Selected > minOptions)
                                PC_Selected--;
                        //if (Input_player1.SelectKey)// || KInput.GUI_Select)
                        //   SelectingLevel = true;
                    }
                    else
                    {
                        if (Input_player1.GUI_Down || KInput.GUI_Down)
                            if (Level_CurrentSelected < Levels.Count - 1)
                            {
                                Level_CurrentSelected++;
                                LevelChange = true;

                                if (55 + (Level_CurrentSelected * 45) + ScrollY > graphics.Viewport.Height - 45)
                                ScrollY -= 45;
                            }
                        if (Input_player1.GUI_Up || KInput.GUI_Up)
                            if (Level_CurrentSelected > 0)
                            {
                                Level_CurrentSelected--;
                                LevelChange = true;
                                if (ScrollY < -45)
                                    ScrollY += 45;
                                else
                                    ScrollY = 0;
                                
                            }
                        if (Input_player1.BackKey  ||  KInput.GUI_Back)
                        {
                            CurrentSelected = Level_CurrentSelected;
                            CurrentLevel = Levels[Level_CurrentSelected];
                            CurrentLevelParts = LevelPieces[Level_CurrentSelected];
                            CurrentLb_File = LbFiles[Level_CurrentSelected];
                            CurrentPr_File = PrFiles[Level_CurrentSelected];
                            LevelChange = true;
                            SelectingLevel = false;
                        }
                        if (SelectedDeleteButon == -1)
                        {
                            if (Input_player1.SelectKey || KInput.GUI_Select)
                            {
                                CurrentSelected = Level_CurrentSelected;
                                CurrentLevel = Levels[Level_CurrentSelected];
                                CurrentLevelParts = LevelPieces[Level_CurrentSelected];
                                CurrentLb_File = LbFiles[Level_CurrentSelected];
                                CurrentPr_File = PrFiles[Level_CurrentSelected];
                                LevelChange = true;
                                SelectingLevel = false;
                            }
                        }
                        else
                        {
                            if (DeleteLevelButton.Length > 0)
                                if (Input_player1.SelectKey || KInput.GUI_Select)
                                {
                                    Level_CurrentSelected = Levels.Count - (CustomLevels - SelectedDeleteButon);
                                    LevelChange = true;
                                    ShowDeleteWarning = true;

                                }
                        }

                        PC_Selected = -1;
                    }



                    UpdateMouse();

                    if (SinglePlayer && PC_Selected < 0)
                        PC_Selected = 0;
                    if (!SinglePlayer && PC_Selected < 2)
                        PC_Selected = 2;
                }
                if (!SinglePlayer)
                {
                    if (Input_player1.UseKey && !ScreenSelection)
                    {
                        ScreenSelection = true;
                        ScreenSelectionInput = Input_player1;
                    }
                    else if (Input_player2.UseKey && !ScreenSelection)
                    {
                        ScreenSelection = true;
                        ScreenSelectionInput = Input_player2;
                    }
                    else if (Input_player3.UseKey && !ScreenSelection)
                    {
                        ScreenSelection = true;
                        ScreenSelectionInput = Input_player3;
                    }
                    else if (Input_player4.UseKey && !ScreenSelection)
                    {
                        ScreenSelection = true;
                        ScreenSelectionInput = Input_player4;
                    }

                    if (ScreenSelection)
                    {
                        ScreenSelectionMenu();
                    }
                    else
                    {
                        Player1Counted = false;
                        Player2Counted = false;
                        Player3Counted = false;
                        Player4Counted = false;
                        CountedPlayers = 0;
                        AllPlayersCounted = false;
                    }
                }
                
        }
        private void ScreenSelectionMenu()
        {
            if (ScreenSelectionInput.BackKey)
                ScreenSelection = false;
            if (ScreenSelectionInput.GUI_Down  &&  ScreenSelection_Option < CountedPlayers - 1)
                ScreenSelection_Option++;
            if (ScreenSelectionInput.GUI_Up && ScreenSelection_Option > 0)
                ScreenSelection_Option--;
        }
        private void DeleteLevel(String levelName, int index)
        {
            if (CustomLevels > 0)
            {

                DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels" + "\\" + levelName);
                if (dir.Exists)
                {
                    dir.Delete(true);


                    Levels.RemoveAt(index);
                    LevelPieces.RemoveAt(index);
                    LevelName.RemoveAt(index);
                    PrFiles.RemoveAt(index);
                    LbFiles.RemoveAt(index);
                    CustomLevels -= 1;
                    DeleteLevelButton = new Rectangle[CustomLevels];
                    SelectedDeleteButon = -1;
                    Level_CurrentSelected = 0;
                    LevelSelect_Button = new Rectangle[LevelSelect_Button.Length - 1];
                    LevelChange = true;
                }
            }
        }
        private void PlayerSelection(GameTime gameTime)
        {
            Add_Player1 = CheckPlayerAdd(Input_player1, Add_Player1);
            player1Ready = CheckPlayerReady(Input_player1, Add_Player1, player1Ready);
            UpdateClass(Input_player1, 0);

            if (!SinglePlayer)
            {
                Add_Player2 = CheckPlayerAdd(Input_player2, Add_Player2);
                player2Ready = CheckPlayerReady(Input_player2, Add_Player2, player2Ready);
                UpdateClass(Input_player2, 1);

                Add_Player3 = CheckPlayerAdd(Input_player3, Add_Player3);
                player3Ready = CheckPlayerReady(Input_player3, Add_Player3, player3Ready);
                UpdateClass(Input_player3, 2);

                Add_Player4 = CheckPlayerAdd(Input_player4, Add_Player4);
                player4Ready = CheckPlayerReady(Input_player4, Add_Player4, player4Ready);
                UpdateClass(Input_player4, 3);
            }
            doneSelecting = Get_finished();
            if (!doneSelecting)
                Countdown = 4000;//10000;
            else
                Countdown -= gameTime.ElapsedGameTime.Milliseconds;
            if (Countdown <= 0)
                start = true;

            if (!Add_Player1 && !Add_Player2 && !Add_Player3 && !Add_Player4 &&
                (Input_player1.BackKey || Input_player2.BackKey || Input_player3.BackKey || Input_player4.BackKey))
                GoBack = true;
            if (KInput.GUI_Back)
                GoBack = true;

            Cur_levelName = LevelName[CurrentSelected];
            CurrentLevel = Levels[CurrentSelected];
            CurrentLevelParts = LevelPieces[CurrentSelected];
            CurrentLb_File = LbFiles[CurrentSelected];
            CurrentPr_File = PrFiles[CurrentSelected];
        }
        private void UpdateClass(Input input, int player)
        {
            if (playerClass[player] > 3)
                playerClass[player] = 0;
            if (playerClass[player] < 0)
                playerClass[player] = 3;
            if (input.ChangeClass_right ||  (KInput.GUI_ChangleClass &&  SinglePlayer))
            {
                playerClass[player]++;
                
                
            }
            if (input.ChangeClass_left)
            {
                playerClass[player]--;
                
            }  
        }
        private Boolean CheckPlayerAdd(Input input, Boolean ADD_PLAYER)
        {
            if (input.SelectKey)
            {
                if (!ADD_PLAYER)//!!
                  //  ADD_PLAYER = false;
               // else
                    ADD_PLAYER = true;
            }
            if (input.BackKey || (KInput.GUI_Back && SinglePlayer))
                ADD_PLAYER = false;

            return ADD_PLAYER;
        }
        private Boolean CheckPlayerReady(Input input, Boolean ADD_PLAYER, Boolean READY_PLAYER)
        {
            if ((input.PauseKey || (KInput.GUI_Ready && SinglePlayer)) && ADD_PLAYER)
            {
                if (READY_PLAYER)
                    READY_PLAYER = false;
                else
                    READY_PLAYER = true;
            }
            return READY_PLAYER;
        }
        public void ResetPlayers()
        {
            Add_Player1 = false;
            player1Ready = false;
            Add_Player2 = false;
            player2Ready = false;
            Add_Player3 = false;
            player3Ready = false;
            Add_Player4 = false;
            player4Ready = false;
        }
        int PlayersAdded = 0;
        int PlayersReady = 0;
        private Boolean[] ADD_playersADDED = new bool[4];
        private Boolean[] READY_playersADDED = new bool[4];
        private Boolean Get_finished()
        {
            ADD_players[0] = Add_Player1;
            ADD_players[1] = Add_Player2;
            ADD_players[2] = Add_Player3;
            ADD_players[3] = Add_Player4;
            READY_players[0] = player1Ready;
            READY_players[1] = player2Ready;
            READY_players[2] = player3Ready;
            READY_players[3] = player4Ready;

            for (int i = 0; i < ADD_players.Length; i++)
            {
                if (ADD_players[i] == false && READY_players[i] == true)
                {
                    switch (i)
                    {
                        case 0 :
                            player1Ready = false;
                            break;
                        case 1:
                            player2Ready = false;
                            break;
                        case 2:
                            player3Ready = false;
                            break;
                        case 3:
                            player4Ready = false;
                            break;

                    }
                    READY_players[i] = false;
                }
                    
                if (ADD_playersADDED[i] == false  &&  ADD_players[i] == true)
                {
                    PlayersAdded += 1;
                    ADD_playersADDED[i] = true;
                }
                if (ADD_playersADDED[i] == true && ADD_players[i] == false)
                {
                    PlayersAdded -= 1;
                    ADD_playersADDED[i] = false;
                }
            }

            for (int i = 0; i < READY_players.Length; i++)
            {
                if (READY_playersADDED[i] == false && READY_players[i] == true)
                {
                    PlayersReady += 1;
                    READY_playersADDED[i] = true;
                }
                if (READY_playersADDED[i] == true && READY_players[i] == false)
                {
                    PlayersReady -= 1;
                    READY_playersADDED[i] = false;
                }
            }

            if (PlayersReady == PlayersAdded && PlayersAdded != 0)


                return true;
            else
                return false;
        }



        private Rectangle[] ReadyButton = new Rectangle[4];


        Rectangle Prev_mouseRect;
        private void UpdateMouse()
        {
            MouseState mouse = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);


            if (SelectingLevel)
            {
                if (Mouse.GetState().ScrollWheelValue != PrevScrollState.ScrollWheelValue)
                {
                    if (ScrollY <= 0 && -ScrollY <= LevelSelect_Button.Length * LevelSelect_Button[0].Height)
                        ScrollY += (Mouse.GetState().ScrollWheelValue - PrevScrollState.ScrollWheelValue) / 4;
                    PrevScrollState = Mouse.GetState();
                }
                if (ScrollY >= 0)
                    ScrollY = 0;
                if (-ScrollY >= LevelSelect_Button.Length * LevelSelect_Button[0].Height)
                    ScrollY = -LevelSelect_Button.Length * LevelSelect_Button[0].Height;
            }
            else
            {
                ScrollY = 0;
                PrevScrollState = Mouse.GetState();
            }

            if (Prev_mouseRect != mouseRect)
            {
                if (!ShowDeleteWarning)
                {
                    for (int i = 0; i < PC_Button.Length; i++)
                    {
                        if (mouseRect.Intersects(PC_Button[i]))
                        {
                            PC_Selected = i;
                        }
                        //if (i >= PC_Button.Length - 1 && PC_Selected >= 0)
                        //{
                        //    if (!mouseRect.Intersects(PC_Button[PC_Selected]))
                        //        PC_Selected = -1;
                        //}
                    }
                    if (SelectingLevel)
                    {
                        if (SelectedDeleteButon == -1)
                        {
                            for (int i = 0; i < LevelSelect_Button.Length; i++)
                            {
                                if (mouseRect.Intersects(LevelSelect_Button[i]))
                                {
                                    Level_CurrentSelected = i;
                                    LevelChange = true;
                                }
                            }
                        }
                        for (int i = 0; i < DeleteLevelButton.Length; i++)
                        {
                            if (mouseRect.Intersects(DeleteLevelButton[i]))
                                SelectedDeleteButon = i;
                            if (SelectedDeleteButon >= 0)
                                if (i >= DeleteLevelButton.Length - 1 && !mouseRect.Intersects(DeleteLevelButton[SelectedDeleteButon]))
                                    SelectedDeleteButon = -1;
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < WarningButtons.Length; i++)
                    {
                        if(mouseRect.Intersects(WarningButtons[i]))
                            WarningButtonSelected = i;
                        if (WarningButtonSelected >= 0)
                            if (i >= WarningButtons.Length - 1 && !mouseRect.Intersects(WarningButtons[WarningButtonSelected]))
                                WarningButtonSelected = -1;

                    }
                }
                Prev_mouseRect = mouseRect;
            }
            

            if (KInput.GUI_Select  ||  Input_player1.SelectKey)
            {
                if (!SelectingLevel)
                {
                    if (SinglePlayer)
                    {
                        if (PC_Selected == 0)
                            player1Ready = !player1Ready;
                        if (PC_Selected == 1)
                            playerClass[0]++;
                    }
                    if (PC_Selected == 2)
                    {
                        SelectingLevel = true;
                        LevelChange = true;
                    }

                    if (PC_Selected == 3)
                        GoBack = true;
                }
                if (ShowDeleteWarning)
                {
                    if (WarningButtonSelected == 0)
                    {
                        DeleteLevel(LevelName[Levels.Count - (CustomLevels - SelectedDeleteButon)], Levels.Count - (CustomLevels - SelectedDeleteButon));
                        ShowDeleteWarning = false;
                        SelectedDeleteButon = -1;
                        WarningButtonSelected = -1;
                    }
                    if (WarningButtonSelected == 1)
                    {
                        ShowDeleteWarning = false;
                        SelectedDeleteButon = -1;
                        WarningButtonSelected = -1;
                    }
                }
            }
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (!SelectingLevel)
            {
                DrawBaseLobby(spriteBatch);
                DrawPC_UI(spriteBatch);
            }
            if (SelectingLevel)
                DrawLevelSelection(spriteBatch);
            if (ShowDeleteWarning)
            {
                String WarningMessege = "Doing This Will Permenently\nDelete The File.\nContinue?";
                int w = 500;
                int h = 250;
                Rectangle WarningWindow = new Rectangle(graphics.Viewport.Width / 2 - (w / 2), graphics.Viewport.Height / 2 - (h / 2), w, h);
                spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(40, 40, 40, 190));
                spriteBatch.Draw(square, WarningWindow, Color.White);
                spriteBatch.DrawString(font, WarningMessege, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 10), Color.Black);

                WarningButtons[0] = new Rectangle(WarningWindow.X + 10, WarningWindow.Y + WarningWindow.Height - 70, 125, 40);
                WarningButtons[1] = new Rectangle(WarningWindow.X + WarningWindow.Width - 125 - 10, WarningWindow.Y + WarningWindow.Height - 70, 125, 40);

                for (int i = 0; i < WarningButtons.Length; i++)
                    if (i == WarningButtonSelected)
                        spriteBatch.Draw(square, WarningButtons[i], Color.Gray);

                spriteBatch.DrawString(font, "Yes", new Vector2((WarningButtons[0].X + WarningButtons[0].Width / 2) - (font.MeasureString("Yes").X / 2), WarningButtons[0].Y), Color.Black);
                spriteBatch.DrawString(font, "No", new Vector2((WarningButtons[1].X + WarningButtons[1].Width / 2) - (font.MeasureString("No").X / 2), WarningButtons[1].Y), Color.Black);
                
             }
            if (ScreenSelection)
            {
                DrawScreenSelection(spriteBatch);
            }

            spriteBatch.End();
        }
        private void DrawPC_UI(SpriteBatch spriteBatch)
        {
            String[] Button_Name = new string[4];

            Button_Name[0] = "Ready";
            PC_Button[0] = new Rectangle(graphics.Viewport.Width - 550, graphics.Viewport.Height - 195, 550, 40);

            string classtext = "";
            if (playerClass[0] == 0)
                classtext = "Sentry Gun";
            if (playerClass[0] == 1)
                classtext = "Regenerator";
            if (playerClass[0] == 2)
                classtext = "Damage Boost";
            if (playerClass[0] == 3)
                classtext = "Ammo Bags";

            Button_Name[1] = "Change Class: " + classtext;
            PC_Button[1] = new Rectangle(graphics.Viewport.Width - 550, graphics.Viewport.Height - 155, 550, 40);

            Button_Name[2] = "Select Level: "  + LevelName[CurrentSelected];
            PC_Button[2] = new Rectangle(graphics.Viewport.Width - 550, graphics.Viewport.Height - 115, 550, 40);

            Button_Name[3] = "Back";
            PC_Button[3] = new Rectangle(graphics.Viewport.Width - 550, graphics.Viewport.Height - 75, 550, 40);



            for (int i = 0; i < PC_Button.Length; i++)
            {
                if (SinglePlayer || (i != 0  &&  i != 1))
                {
                    if (i == PC_Selected)
                        spriteBatch.Draw(square, PC_Button[i], Color.Black);
                    spriteBatch.DrawString(font, Button_Name[i], new Vector2(PC_Button[i].X + 10, PC_Button[i].Y), Color.White);
                }
            }
            if (!SinglePlayer)
            {
                spriteBatch.Draw(Input_player1.ButtonRB, new Rectangle(PC_Button[1].X, PC_Button[1].Y - 10, 90,40), Color.White);
                spriteBatch.Draw(Input_player1.ButtonLB, new Rectangle(PC_Button[1].X + 100, PC_Button[1].Y - 10, 90, 40), Color.White);
                spriteBatch.DrawString(font, "Change Class", new Vector2(PC_Button[1].X + 200, PC_Button[1].Y - 10), Color.White);

                spriteBatch.Draw(Input_player1.ButtonX, new Rectangle(PC_Button[0].X, PC_Button[0].Y - 30, 40, 40), Color.White);
                spriteBatch.DrawString(font, "Screen Selection", new Vector2(PC_Button[0].X + 60, PC_Button[0].Y - 27), Color.White);

                spriteBatch.Draw(Input_player1.ButtonStart, new Rectangle(PC_Button[0].X, PC_Button[0].Y - 90, 50, 50), Color.White);
                spriteBatch.DrawString(font, "Ready", new Vector2(PC_Button[0].X + 60, PC_Button[0].Y - 77), Color.White);

                //spriteBatch.Draw(Input_player1.ButtonStart, new Rectangle(PC_Button[0].X, PC_Button[0].Y - 40, 50, 50), Color.White);
                //spriteBatch.DrawString(font, "Ready", new Vector2(PC_Button[0].X + 60, PC_Button[0].Y - 27), Color.White);

            }


        }
        private void DrawLevelSelection(SpriteBatch spriteBatch)
        {
            /*
            if (ScrollY <= 0 && -ScrollY <= LevelSelect_Button.Length * LevelSelect_Button[0].Height)
                        ScrollY += (Mouse.GetState().ScrollWheelValue - PrevScrollState.ScrollWheelValue) / 4;
                    PrevScrollState = Mouse.GetState();
                }
                if (ScrollY >= 0)
                    ScrollY = 0;
                if (-ScrollY >= LevelSelect_Button.Length * LevelSelect_Button[0].Height)
                    ScrollY = -LevelSelect_Button.Length * LevelSelect_Button[0].Height;
            */
            

            spriteBatch.Draw(square, new Rectangle(35, 55, 10, graphics.Viewport.Height - 70), Color.DarkGray);
            spriteBatch.Draw(content.Load<Texture2D>("KeyBoard Keys/Mouse_Scroll"), new Rectangle(32,
               (int)(55 + ((-ScrollY / (float)(LevelSelect_Button.Length * LevelSelect_Button[0].Height)) * (graphics.Viewport.Height - 110)))
                ,16, 40), Color.White);
            
            for (int i = 0; i < Levels.Count; i++)
            {
                LevelSelect_Button[i] = new Rectangle(50, 55 + (i * 45) + ScrollY, 350, 40);
                if(i == Level_CurrentSelected)
                    spriteBatch.Draw(square, LevelSelect_Button[i], new Color(120, 120, 120, 70));
                else
                    spriteBatch.Draw(square, LevelSelect_Button[i], new Color(40, 40, 40, 10));
                spriteBatch.DrawString(font, "" + LevelName[i], new Vector2(55, 55 + (i * 45) + ScrollY), Color.White);

                if (i >= Levels.Count - CustomLevels)
                {
                    DeleteLevelButton[CustomLevels - (Levels.Count - i)] = new Rectangle(LevelSelect_Button[i].X + LevelSelect_Button[i].Width + 10,
                        LevelSelect_Button[i].Y, 40,40);
                    if (CustomLevels - (Levels.Count - i) == SelectedDeleteButon)
                        spriteBatch.Draw(DeleteTexture, DeleteLevelButton[CustomLevels - (Levels.Count - i)], Color.Aqua);
                    else
                        spriteBatch.Draw(DeleteTexture, DeleteLevelButton[CustomLevels - (Levels.Count - i)], Color.White);
                }
                
            }

            spriteBatch.Draw(square, new Rectangle(0, 0, 500, 55), Color.Black);
            spriteBatch.Draw(square, new Rectangle(0, graphics.Viewport.Height - 15, 500, 15), Color.Black);
            spriteBatch.DrawString(font, "Select Level", new Vector2(50, 15), Color.White);
            Rectangle level_PreviewRect = new Rectangle(graphics.Viewport.Width - 650, 30, 600, 550);
            if(Level_CurrentSelected >= 0  &&  Level_CurrentSelected < Levels.Count)
            DrawLevelPreview(spriteBatch, Levels[Level_CurrentSelected], LevelPieces[Level_CurrentSelected], level_PreviewRect.X, level_PreviewRect.Y, 15, 15);//spriteBatch.Draw(Levels[Level_CurrentSelected], level_PreviewRect, Color.White);
           // spriteBatch.Draw(LevelPieces[Level_CurrentSelected], level_PreviewRect, Color.White);

        }
        private void DrawBaseLobby(SpriteBatch spriteBatch)
        {
            //Background Image (ex - HALO 2)
            spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.Black);
            //spriteBatch.Draw(background, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(100,100,100,100));
            spriteBatch.Draw(square, new Rectangle(45, 100, graphics.Viewport.Width / 2, graphics.Viewport.Height), new Color(40, 40, 40, 10));
           
           // spriteBatch.Draw(CurrentLevel, new Rectangle(50, 335, graphics.Viewport.Width / 2 - 10, 350), Color.White);
            DrawLevelPreview(spriteBatch, Levels[Level_CurrentSelected], LevelPieces[Level_CurrentSelected], 65, 110, 15, 15);



            spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width  - 595, 0, 546, graphics.Viewport.Height), new Color(40, 40, 40, 10));//PlayersBackground
            DrawPlayerSetup(spriteBatch, Add_Player1, player1Ready, 1);
            if (!SinglePlayer)
            {
                DrawPlayerSetup(spriteBatch, Add_Player2, player2Ready, 2);
                DrawPlayerSetup(spriteBatch, Add_Player3, player3Ready, 3);
                DrawPlayerSetup(spriteBatch, Add_Player4, player4Ready, 4);
            }
            if(doneSelecting)
                spriteBatch.DrawString(font, "Game Starting in: " + (int)(Countdown / 1000 /*+ 1*/) , new Vector2(graphics.Viewport.Width - 435, 70), Color.White);

            
            spriteBatch.DrawString(font, "Lobby", new Vector2(50, 50), Color.White);
            if (!SinglePlayer)
            {
               // spriteBatch.DrawString(font, "Level : " + LevelName[CurrentSelected], new Vector2(55, 150), Color.White);
                if (PlayersAdded - PlayersReady > 1)
                    spriteBatch.DrawString(font, "Need " + (PlayersAdded - PlayersReady) + " Players To Be Ready", new Vector2(graphics.Viewport.Width - 500, 70), Color.White);
                else if (PlayersAdded - PlayersReady == 1)
                    spriteBatch.DrawString(font, "Need 1 Player To Be Ready", new Vector2(graphics.Viewport.Width - 500, 70), Color.White);


                //Rectangle B1 = new Rectangle(55, 200, 250, 40);
                //levelSelectionButton = B1;
                //if (Menu_CurrentSelection == 0)
                //    spriteBatch.Draw(square, B1, new Color(120, 120, 120, 70));
                //else
                //    spriteBatch.Draw(square, B1, new Color(40, 40, 40, 10));
                //spriteBatch.DrawString(font, "Select Level", new Vector2(B1.X + B1.Width / 2 - font.MeasureString("Select Level").X / 2, 200), Color.Black);

                //Rectangle B2 = new Rectangle(365, 200, 250, 40);
                //if (Menu_CurrentSelection == 1)
                //    spriteBatch.Draw(square, B2, new Color(120, 120, 120, 70));
                //else
                //    spriteBatch.Draw(square, B2, new Color(40, 40, 40, 10));
                //spriteBatch.DrawString(font, "Tutorial Level", new Vector2(B2.X + B2.Width / 2 - font.MeasureString("Tutorial Level").X / 2, 200), Color.Black);
            }
            else
            {
             //   spriteBatch.DrawString(font, "Level : " + LevelName[CurrentSelected], new Vector2(50, 295), Color.White);
            }
        }
        private void DrawPlayerSetup(SpriteBatch spriteBatch, Boolean ADD, Boolean READY, int PLAYER)
        {
            int ALPHA = 150;
            int Y = 70 + (40 * PLAYER);
            int x = 430;//470
            spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width - (x + 5), Y, 325, 35), new Color(ALPHA, ALPHA, ALPHA, ALPHA));

            String className = "";
            switch (playerClass[PLAYER - 1])
            {
                case 0:
                    className = "Support";
                    break;
                case 1:
                    className = "Medic";
                    break;
                case 2:
                    className = "Assault";
                    break;
                case 3:
                    className = "Supplier";
                    break;

            }
            if (ADD)
            {
                //spriteBatch.DrawString(font, className, new Vector2(graphics.Viewport.Width - (x + 20) - font.MeasureString(className).X, Y), Color.White);
                switch (playerClass[PLAYER - 1])
                {
                    case 0:
                        DrawItemIcon(graphics.Viewport.Width - (x + 45), Y + 3, spriteBatch, SentryStand, SentryTexture);
                        break;
                    case 1:
                        DrawItemIcon(graphics.Viewport.Width - (x + 45), Y + 3, spriteBatch, StimGasIcon, null);
                        break;
                    case 2:
                        DrawItemIcon(graphics.Viewport.Width - (x + 45), Y + 3, spriteBatch, DamageBoostIcon, null);
                        break;
                    case 3:
                        DrawItemIcon(graphics.Viewport.Width - (x + 45), Y + 3, spriteBatch, AmmoBagIcon, null);
                        break;

                }
            }

            ReadyButton[PLAYER - 1] = new Rectangle(graphics.Viewport.Width - (x - 325), Y, 35,35);
            if (READY)
                spriteBatch.Draw(square, ReadyButton[PLAYER - 1], new Color(0, 255, 0, ALPHA));
            else
                spriteBatch.Draw(square, ReadyButton[PLAYER - 1], new Color(255, 0, 0, ALPHA));

            if (ADD)
                spriteBatch.DrawString(font, "Player " + PLAYER, new Vector2(graphics.Viewport.Width - x, Y), Color.Black);
            else
                spriteBatch.DrawString(font, "Press 'A' To Join", new Vector2(graphics.Viewport.Width - x, Y), Color.Black);
        }
        private void DrawScreenSelection(SpriteBatch spriteBatch)
        {
            Rectangle backgroundRect = new Rectangle(Scaled(300),Scaled(100),Scaled(graphics.Viewport.Width - 600),Scaled(graphics.Viewport.Height - 200));
            spriteBatch.Draw(square, new Rectangle(backgroundRect.X - 4, backgroundRect.Y - 4, backgroundRect.Width + 8, backgroundRect.Height + 8), Color.Black);
            spriteBatch.Draw(square, backgroundRect, Color.LightGray);


            spriteBatch.Draw(square, new Rectangle(backgroundRect.X, backgroundRect.Y, backgroundRect.Width, 50), Color.White);
            spriteBatch.DrawString(font, "Screen Selection", new Vector2(backgroundRect.X + 5, backgroundRect.Y + 5), Color.Black);

            spriteBatch.DrawString(font, "Player", new Vector2(backgroundRect.X + 5, backgroundRect.Y + 55), Color.Black);
            spriteBatch.DrawString(font, "Screen", new Vector2(backgroundRect.X + backgroundRect.Width - font.MeasureString("Screen").X - 5, backgroundRect.Y + 55), Color.Black);

            spriteBatch.Draw(square, new Rectangle(backgroundRect.X, backgroundRect.Y + 55 + (int)font.MeasureString(".").Y + 2, backgroundRect.Width, 4), Color.Black);
            

            Rectangle ScreenRect = new Rectangle(backgroundRect.X + backgroundRect.Width / 2 - 100, backgroundRect.Y + backgroundRect.Height - 155,
                200, 150);
            spriteBatch.Draw(square, ScreenRect, Color.Black);
            spriteBatch.Draw(square, new Rectangle(ScreenRect.X + 4, ScreenRect.Y + 4, ScreenRect.Width - 8, ScreenRect.Height - 8), Color.LightGray);


            if(CountedPlayers == 2)
            {
                spriteBatch.Draw(square, new Rectangle(ScreenRect.X, ScreenRect.Y + ScreenRect.Height / 2 - 2, ScreenRect.Width, 4), Color.Black);
            }
            if (CountedPlayers == 3)
            {
                spriteBatch.Draw(square, new Rectangle(ScreenRect.X, ScreenRect.Y + ScreenRect.Height / 2 - 2, ScreenRect.Width, 4), Color.Black);
                spriteBatch.Draw(square, new Rectangle(ScreenRect.X + ScreenRect.Width / 2 - 2, ScreenRect.Y + ScreenRect.Height / 2, 4, ScreenRect.Height / 2), Color.Black);
            }
            if (CountedPlayers == 4)
            {
                spriteBatch.Draw(square, new Rectangle(ScreenRect.X, ScreenRect.Y + ScreenRect.Height / 2 - 2, ScreenRect.Width, 4), Color.Black);
                spriteBatch.Draw(square, new Rectangle(ScreenRect.X + ScreenRect.Width / 2 - 2, ScreenRect.Y, 4, ScreenRect.Height), Color.Black);
            }
            //for (int i = 0; i < CountedPlayers; i++)
            //{
            //    int a = 0;
            //    if(i == Player1_Screen)
                    
            //}

            
            int Offset = 100;
            int InitialOffset = Offset;
            Rectangle _playerRect = new Rectangle(backgroundRect.X, backgroundRect.Y + (50 * ScreenSelection_Option) + Offset, backgroundRect.Width, 50);
            spriteBatch.Draw(square, _playerRect, Color.DarkGray);
            if (Add_Player1)
            {
                if (!Player1Counted)
                {
                    CountedPlayers++;
                    Player1Counted = true;
                }
                if(AllPlayersCounted)
                if (Player1_Screen >= CountedPlayers)// -1
                    Player1_Screen = CountedPlayers - 1;
                Rectangle playerRect = new Rectangle(backgroundRect.X, backgroundRect.Y + Offset, backgroundRect.Width, 50);
                spriteBatch.DrawString(font, "Player 1", new Vector2(playerRect.X + 5, playerRect.Y), Color.Black);

                spriteBatch.DrawString(font, "" + (Player1_Screen + 1), new Vector2(playerRect.X + playerRect.Width - 50, playerRect.Y), Color.Black);
                Offset += 50;
                if (ScreenSelection_Option == ((Offset - InitialOffset) / 50) - 1)
                {
                    if (ScreenSelectionInput.GUI_Left && Player1_Screen > 0)
                        Player1_Screen--;
                    if (ScreenSelectionInput.GUI_Right && Player1_Screen < CountedPlayers - 1)
                        Player1_Screen++;
                }
                DrawScreenPreview(spriteBatch, 1, ScreenRect, Player1_Screen);
            }
            if (Add_Player2)
            {
                if (!Player2Counted)
                {
                    CountedPlayers++;
                    Player2Counted = true;
                }
                if(AllPlayersCounted)
                if (Player2_Screen >= CountedPlayers)
                    Player2_Screen = CountedPlayers - 1;
                Rectangle playerRect = new Rectangle(backgroundRect.X, backgroundRect.Y + Offset, backgroundRect.Width, 50);
                spriteBatch.DrawString(font, "Player 2", new Vector2(playerRect.X + 5, playerRect.Y), Color.Black);

                spriteBatch.DrawString(font, "" + (Player2_Screen + 1), new Vector2(playerRect.X + playerRect.Width - 50, playerRect.Y), Color.Black);
                Offset += 50;
                if (ScreenSelection_Option == ((Offset - InitialOffset) / 50) - 1)
                {
                    if (ScreenSelectionInput.GUI_Left && Player2_Screen > 0)
                        Player2_Screen--;
                    if (ScreenSelectionInput.GUI_Right && Player2_Screen < CountedPlayers - 1)
                        Player2_Screen++;
                }
                DrawScreenPreview(spriteBatch, 2, ScreenRect, Player2_Screen);
            }
            if (Add_Player3)
            {
                if (!Player3Counted)
                {
                    CountedPlayers++;
                    Player3Counted = true;
                }
                if(AllPlayersCounted)
                if (Player3_Screen >= CountedPlayers)
                    Player3_Screen = CountedPlayers - 1;
                Rectangle playerRect = new Rectangle(backgroundRect.X, backgroundRect.Y + Offset, backgroundRect.Width, 50);
                spriteBatch.DrawString(font, "Player 3", new Vector2(playerRect.X + 5, playerRect.Y), Color.Black);

                spriteBatch.DrawString(font, "" + (Player3_Screen + 1), new Vector2(playerRect.X + playerRect.Width - 50, playerRect.Y), Color.Black);
                Offset += 50;
                if (ScreenSelection_Option == ((Offset - InitialOffset) / 50) - 1)
                {
                    if (ScreenSelectionInput.GUI_Left && Player3_Screen > 0)
                        Player3_Screen--;
                    if (ScreenSelectionInput.GUI_Right && Player3_Screen < CountedPlayers - 1)
                        Player3_Screen++;
                }
                DrawScreenPreview(spriteBatch, 3, ScreenRect, Player3_Screen);
            }
            if (Add_Player4)
            {
                if (!Player4Counted)
                {
                    CountedPlayers++;
                    Player4Counted = true;
                }
                if(AllPlayersCounted)
                if (Player4_Screen >= CountedPlayers)
                    Player4_Screen = CountedPlayers - 1;
                Rectangle playerRect = new Rectangle(backgroundRect.X, backgroundRect.Y + Offset, backgroundRect.Width, 50);
                spriteBatch.DrawString(font, "Player 4", new Vector2(playerRect.X + 5, playerRect.Y), Color.Black);

                spriteBatch.DrawString(font, "" + (Player4_Screen + 1), new Vector2(playerRect.X + playerRect.Width - 50, playerRect.Y), Color.Black);
                Offset += 50;
                if (ScreenSelection_Option == ((Offset - InitialOffset) / 50) - 1)
                {
                    if (ScreenSelectionInput.GUI_Left && Player4_Screen > 0)
                        Player4_Screen--;
                    if (ScreenSelectionInput.GUI_Right && Player4_Screen < CountedPlayers - 1)
                        Player4_Screen++;
                }
               
                DrawScreenPreview(spriteBatch, 4, ScreenRect, Player4_Screen);
            }
            AllPlayersCounted = true;
            
        }
        private void DrawScreenPreview(SpriteBatch spriteBatch, int player, Rectangle ScreenRect, int Screen)
        {
            if (CountedPlayers == 1)
            {
                spriteBatch.DrawString(font, "" + player, new Vector2(ScreenRect.X + ScreenRect.Width / 2 - font.MeasureString("" + player).X / 2, ScreenRect.Y + ScreenRect.Height / 2 - font.MeasureString("" + player).Y / 2), Color.Black);
            }
            if (CountedPlayers == 2)
            {
                Vector2[] ScreenParts = new Vector2[2];
                ScreenParts[0] = new Vector2(ScreenRect.X + ScreenRect.Width / 2, ScreenRect.Y + ScreenRect.Height / 4);
                ScreenParts[1] = new Vector2(ScreenRect.X + ScreenRect.Width / 2, ScreenRect.Y + ScreenRect.Height * 0.75f);
                if(Screen < ScreenParts.Length)
                spriteBatch.DrawString(font, "" + player, new Vector2(ScreenParts[Screen].X - font.MeasureString("" + player).X / 2, ScreenParts[Screen].Y - font.MeasureString("" + player).Y / 2), Color.Black);
            }
            if (CountedPlayers == 3)
            {
                Vector2[] ScreenParts = new Vector2[3];
                ScreenParts[0] = new Vector2(ScreenRect.X + ScreenRect.Width / 2, ScreenRect.Y + ScreenRect.Height / 4);
                ScreenParts[1] = new Vector2(ScreenRect.X + ScreenRect.Width / 4, ScreenRect.Y + ScreenRect.Height * 0.75f);
                ScreenParts[2] = new Vector2(ScreenRect.X + ScreenRect.Width * 0.75f, ScreenRect.Y + ScreenRect.Height * 0.75f);
                if (Screen < ScreenParts.Length)
                spriteBatch.DrawString(font, "" + player, new Vector2(ScreenParts[Screen].X - font.MeasureString("" + player).X / 2, ScreenParts[Screen].Y - font.MeasureString("" + player).Y / 2), Color.Black);
            }
            if (CountedPlayers == 4)
            {
                Vector2[] ScreenParts = new Vector2[4];
                ScreenParts[0] = new Vector2(ScreenRect.X + ScreenRect.Width / 4, ScreenRect.Y + ScreenRect.Height / 4);
                ScreenParts[1] = new Vector2(ScreenRect.X + ScreenRect.Width * 0.75f, ScreenRect.Y + ScreenRect.Height / 4);
                ScreenParts[2] = new Vector2(ScreenRect.X + ScreenRect.Width / 4, ScreenRect.Y + ScreenRect.Height * 0.75f);
                ScreenParts[3] = new Vector2(ScreenRect.X + ScreenRect.Width * 0.75f, ScreenRect.Y + ScreenRect.Height * 0.75f);
                if (Screen < ScreenParts.Length)
                spriteBatch.DrawString(font, "" + player, new Vector2(ScreenParts[Screen].X - font.MeasureString("" + player).X / 2, ScreenParts[Screen].Y - font.MeasureString("" + player).Y / 2), Color.Black);
            }
        }
        private int Scaled(int num)
        {
            int NUM = ((graphics.Viewport.Width / 1280) * (graphics.Viewport.Height / 720)) * num;
            return NUM;
        }
        List<Rectangle> WallLineing = new List<Rectangle>();
        private List<Rectangle> SetupWallLineing(List<Rectangle> walls)
        {
            int LineThickness = 2;
            List<Rectangle> WallLineing = new List<Rectangle>();
            for (int i = 0; i < walls.Count; i++)
            {
                Rectangle rect = walls[i];
                if (!walls.Contains(new Rectangle(rect.X - rect.Width, rect.Y, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y, LineThickness, rect.Height));
                if (!walls.Contains(new Rectangle(rect.X + rect.Width, rect.Y, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X + rect.Width - LineThickness, rect.Y, LineThickness, rect.Height));
                if (!walls.Contains(new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y + rect.Height - LineThickness, rect.Width, LineThickness));
                if (!walls.Contains(new Rectangle(rect.X, rect.Y - rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y, rect.Width, LineThickness));


                if (walls.Contains(new Rectangle(rect.X - rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    walls.Contains(new Rectangle(rect.X, rect.Y - rect.Height, rect.Width, rect.Height)) &&
                    !walls.Contains(new Rectangle(rect.X - rect.Width, rect.Y - rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y, LineThickness, LineThickness));

                if (walls.Contains(new Rectangle(rect.X - rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    walls.Contains(new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height)) &&
                    !walls.Contains(new Rectangle(rect.X - rect.Width, rect.Y + rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y + rect.Height - LineThickness, LineThickness, LineThickness));

                if (walls.Contains(new Rectangle(rect.X + rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    walls.Contains(new Rectangle(rect.X, rect.Y - rect.Height, rect.Width, rect.Height)) &&
                    !walls.Contains(new Rectangle(rect.X + rect.Width, rect.Y - rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X + rect.Width - LineThickness, rect.Y, LineThickness, LineThickness));

                if (walls.Contains(new Rectangle(rect.X + rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    walls.Contains(new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height)) &&
                    !walls.Contains(new Rectangle(rect.X + rect.Width, rect.Y + rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X + rect.Width - LineThickness, rect.Y + rect.Height - LineThickness, LineThickness, LineThickness));
            }
            return WallLineing;
        }
        private Boolean IsWall(Color colorID)
        {
            for (int i = 0; i < tileManager.WallTiles.Count; i++)
            {
                if (tileManager.WallTiles[i].colorID == colorID)
                    return true;
            }
            return false;
        }
        private void DrawLevelPreview(SpriteBatch spriteBatch, Texture2D CurLevel, Texture2D CurLevelPieces, int Xpos, int Ypos, int Width, int Height)
        {
            PreviewTile = new Texture2D[CurLevel.Width * CurLevel.Height];
            PreviewTileRect = new Rectangle[CurLevel.Width * CurLevel.Height];

            List<Rectangle> WallTiles = new List<Rectangle>();
           
            Color[] levelTile = new Color[CurLevel.Width * CurLevel.Height];
            CurLevel.GetData<Color>(levelTile);

            for (int y = 0; y < CurLevel.Height; y++)
            {
                for (int x = 0; x < CurLevel.Width; x++)
                {

                    int i = y * CurLevel.Width + x;
                    PreviewTileRect[i] = new Rectangle(x * Width + Xpos, y * Height + Ypos, Width, Height);
                    for (int a = 0; a < tileManager.BasicTile.Length; a++)
                    {
                        if (levelTile[y * CurLevel.Width + x] == tileManager.BasicTile[a].colorID)
                            PreviewTile[i] = tileManager.BasicTile[a].texture;                           
                    }

                    if (IsWall(levelTile[y * CurLevel.Width + x]))
                        WallTiles.Add(new Rectangle(x * Width + Xpos, y * Height + Ypos, Width, Height));

                    for (int a = 0; a < tileManager.AssociatedTypes_Basic.Length; a++)
                    {
                        for (int b = 0; b < tileManager.AssociatedTypes_Basic[a].Count; b++)
                            if (levelTile[y * CurLevel.Width + x] == tileManager.AssociatedTypes_Basic[a][b].colorID)
                            {
                                PreviewTile[i] = tileManager.AssociatedTypes_Basic[a][b].texture;
                            }
                    }

                    
                }
            }
            if (LevelChange)
            {
                WallLineing = new List<Rectangle>();
                WallLineing = SetupWallLineing(WallTiles);
                LevelChange = false;
            }

            Color[] levelPieceTile = new Color[CurLevelPieces.Width * CurLevelPieces.Height];
            CurLevelPieces.GetData<Color>(levelPieceTile);
            PreviewTilePiece = new Texture2D[CurLevelPieces.Width * CurLevelPieces.Height];
            PreviewTilePieceRect = new Rectangle[CurLevelPieces.Width * CurLevelPieces.Height];

            for (int y = 0; y < CurLevelPieces.Height; y++)
            {
                for (int x = 0; x < CurLevelPieces.Width; x++)
                {
                    int i = y * CurLevelPieces.Width + x;
                    PreviewTilePieceRect[i] = new Rectangle(x * Width + Xpos, y * Height + Ypos, Width, Height);

                    for (int b = 0; b < tileManager.PiecesTile.Length; b++)
                        if (tileManager.PiecesTile[b].name != "Eraser")
                            if (levelPieceTile[y * CurLevelPieces.Width + x] == tileManager.PiecesTile[b].colorID)
                                PreviewTilePiece[i] = tileManager.PiecesTile[b].texture;
                    for(int b = 0; b < tileManager.weaponTiles.Length; b++)
                        if(levelPieceTile[y * CurLevelPieces.Width + x] == tileManager.weaponTiles[b].colorID)
                            PreviewTilePiece[i] = tileManager.weaponTiles[b]._WeaponTile.texture;
                    for(int b = 0; b < tileManager.itemTiles.Length; b++)
                        if (levelPieceTile[y * CurLevelPieces.Width + x] == tileManager.itemTiles[b].colorID)
                            PreviewTilePiece[i] = tileManager.itemTiles[b]._WeaponTile.texture;
                    for(int b = 0; b < tileManager.teleporterTiles.Length; b++)
                        if (levelPieceTile[y * CurLevelPieces.Width + x] == tileManager.teleporterTiles[b].colorID)
                            PreviewTilePiece[i] = tileManager.teleporterTiles[b]._WeaponTile.texture;
                }

            }



            for (int i = 0; i < PreviewTile.Length; i++)
            {
                if(PreviewTile[i] != null)
                    spriteBatch.Draw(PreviewTile[i], PreviewTileRect[i], Color.White);
                if (PreviewTilePiece[i] != null)
                    spriteBatch.Draw(PreviewTilePiece[i], PreviewTilePieceRect[i], Color.White);
            }
            for (int i = 0; i < WallLineing.Count; i++)
            {
                spriteBatch.Draw(lineTexture, WallLineing[i], Color.Black);
            }
        }
        private void DrawItemIcon(int x, int y, SpriteBatch spriteBatch, Texture2D img1, Texture2D img2)
        {
            spriteBatch.Draw(square, new Rectangle(Scaled((x - 2)), Scaled((y - 2)), Scaled(34), Scaled(34)), Color.Black);
            spriteBatch.Draw(square, new Rectangle(Scaled(x), Scaled(y), Scaled(30), Scaled(30)), Color.RoyalBlue);
            spriteBatch.Draw(img1, new Rectangle(Scaled(x + 1), Scaled(y + 1), Scaled(28), Scaled(28)), Color.White);
            if (img2 != null)
                spriteBatch.Draw(img2, new Rectangle(Scaled(x + 1), Scaled(y + 1), Scaled(28), Scaled(28)), Color.White);
        }
    }
}
