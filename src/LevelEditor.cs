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

using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using SurvivalShooter.StandardGame;
using System.IO;
using SurvivalShooter.LevelEditorComponents;


namespace SurvivalShooter
{
    class LevelEditor
    {

        public Boolean Back = false;

        private Texture2D square;
        private Texture2D EscapeKeyTexture;
        private SpriteFont font;
        private GraphicsDevice graphics;
        private ContentManager content;


        private Rectangle[] Button;
        private String[] Button_name;
        private int Button_Selected = -1;

        private Camera camera;
        private Vector2 camCen = new Vector2(0, 0);
        private TileSelection tileSelection;
        private LevelPreview levelPreview;

        private Boolean show_WarningWindow = false;
        private String WarningMessege = "";
        private Rectangle WarningWindow;
        private Rectangle OKButton;
        private Boolean HighlightOKButton = false;

        public int SaveFadeValue = 0;

        private int Open_ScrollY = 0;
        private MouseState PrevMouseScroll;
        private Boolean ShowOpenWindow = false;
        private Boolean _load = false;
        private List<Texture2D> Levels = new List<Texture2D>();
        public int LevelLoadSelected = 0;
        private List<Texture2D> LevelPieces = new List<Texture2D>();
        private List<String> LevelName = new List<string>();
        private List<FileInfo> LevelPr = new List<FileInfo>();
        private Rectangle[] LevelSelect_Button = new Rectangle[3];

        TileManager tileManager = new TileManager();

        private Boolean mouseReleased = false;
        private Boolean InitMouseUp = false;

        public Boolean loaded = false;

        public Texture2D Fire1;
        public Texture2D Fire2;

        private List<String> WeaponTypes = new List<string>();
        private int WeaponTypeIndex = 0;
        String _Weapon = "";
        Color colorID = new Color();

        public LevelEditor()
        {
            

            

        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            tileManager.Load(content);
            square = content.Load<Texture2D>("Sprites/square");
            EscapeKeyTexture = content.Load<Texture2D>("KeyBoard Keys/Key_Esc");
            font = content.Load<SpriteFont>("standardfont");
            this.graphics = graphics;
            this.content = content;

            camera = new Camera();
            

            tileSelection = new TileSelection();
            tileSelection.Load(content, graphics);

            levelPreview = new LevelPreview();
            levelPreview.Load(content, graphics);

            Button = new Rectangle[5];
            Button_name = new string[5];

            Button[0] = new Rectangle(tileSelection.TileSelectionWindow.X, tileSelection.TileSelectionWindow.Y + tileSelection.TileSelectionWindow.Height - 40, tileSelection.TileSelectionWindow.Width, 40);
            Button_name[0] = "Back";
            Button[1] = new Rectangle (tileSelection.TileSelectionWindow.X, tileSelection.TileSelectionWindow.Y + tileSelection.TileSelectionWindow.Height - 80, tileSelection.TileSelectionWindow.Width, 40);
            Button_name[1] = "Save";
            Button[2] = new Rectangle(tileSelection.TileSelectionWindow.X, tileSelection.TileSelectionWindow.Y + tileSelection.TileSelectionWindow.Height - 120, tileSelection.TileSelectionWindow.Width, 40);
            Button_name[2] = "Load";

            Button[3] = new Rectangle(tileSelection.TileSelectionWindow.X, tileSelection.TileSelectionWindow.Y + tileSelection.TileSelectionWindow.Height - 160, tileSelection.TileSelectionWindow.Width, 40);
            Button_name[3] = "New";
            Button[4] = new Rectangle(tileSelection.TileSelectionWindow.X, tileSelection.TileSelectionWindow.Y + tileSelection.TileSelectionWindow.Height - 200, tileSelection.TileSelectionWindow.Width, 40);
            Button_name[4] = fileName;

            Fire1 = content.Load<Texture2D>("Keyboard Keys/Mouse_Left");
            Fire2 = content.Load<Texture2D>("Keyboard Keys/Mouse_Right");

            SetupWeaponTypes();
        }
        private void SetupWeaponTypes()
        {
            WeaponTypes.Add("Pistol");
            WeaponTypes.Add("Rifle");
            WeaponTypes.Add("Shotgun");
        }
        public void LoadCustomLevels()
        {
            LevelName.Clear();
            LevelPr.Clear();
            LevelPieces.Clear();
            Levels.Clear();
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels");
            if (dir.Exists)
            {
                Dictionary<String, Texture2D> _result = new Dictionary<String, Texture2D>();
                DirectoryInfo[] LevelDir = dir.GetDirectories();
                foreach(DirectoryInfo _dir in LevelDir)
                {
                    DirectoryInfo _levelDirectory = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels"  + "\\" + _dir.Name);
                    FileInfo[] files = _levelDirectory.GetFiles("*.*");
                    if (_levelDirectory.Exists)
                    {
                        foreach (FileInfo file in files)
                        {
                            if (!file.Name.Contains("_lb")  &&  !file.Name.Contains("_pr"))
                            {
                                FileStream fs = new FileStream(file.FullName, FileMode.Open);
                                Texture2D t = Texture2D.FromStream(graphics, fs);
                                String FileName = file.Name;
                                if (FileName.Contains("_File1"))
                                {
                                    Levels.Add(t);
                                    String _LevelName = Path.GetFileNameWithoutExtension(file.Name);
                                    String _temp = _LevelName;
                                    String _Name = _LevelName.Remove(_temp.Length - 6, 6);
                                    LevelName.Add(_Name);
                                }
                                if (FileName.Contains("_File2"))
                                {
                                    LevelPieces.Add(t);
                                }
                                fs.Close();
                            }
                            
                            if (file.Name.Contains("_pr"))
                            {
                                LevelPr.Add(file);
                            }
                           
                        }
                    }
                }
            }
            loaded = true;
        }
        public void Update(GameTime gameTime)
        {
            if (!InitMouseUp)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    InitMouseUp = true;
            }
            if (levelPreview.exitPriceEditing)
            {
                InitMouseUp = false;
                mouseReleased = false;
                levelPreview.EditingPrice = false;
                levelPreview.exitPriceEditing = false;
            }
                camera.Update(gameTime, camCen, graphics);
                if (!show_WarningWindow && !ShowOpenWindow )
                {
                    if (mouseReleased && !levelPreview.EditingPrice)
                    {
                        tileSelection.Update(gameTime);
                    }
                    if (mouseReleased)
                    {
                        levelPreview.Update(gameTime, camCen, tileSelection.selectedTile, tileSelection.MouseIntersectsGUI, tileSelection.dragWindow, WeaponTypeIndex);
                    }
                    
                }
                else
                {
                    mouseReleased = false;
                }
                if (!mouseReleased && Mouse.GetState().LeftButton == ButtonState.Released)
                    mouseReleased = true;
                if (ShowOpenWindow)
                {
                    if (!_load)
                    {
                        LoadCustomLevels();
                        LevelSelect_Button = new Rectangle[Levels.Count];
                        _load = true;
                    }
                }
                else
                {
                    Levels.Clear();
                    LevelPieces.Clear();
                    LevelName.Clear();
                }
                if (InitMouseUp)
                {

                    UpdateMouse();
                    UpdateMouseSelection();
                    UpdateCamera();
                    TypeName();
                    Button_name[4] = fileName;
                }
        }

        private void Open(Texture2D File1, Texture2D File2, FileInfo prFile)
        {
            ResetLevel();
            //DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "CustomLevels" + "\\" +  fileName);
            //Texture2D File1 = null;
            //Texture2D File2 = null;

            //if (dir.Exists)
            //{
            //    Dictionary<String, Texture2D> result = new Dictionary<String, Texture2D>();
            //    FileInfo[] files = dir.GetFiles("*.*");
            //    foreach (FileInfo file in files)
            //    {
            //        String FileNameTemp2 = Path.GetFileNameWithoutExtension(file.Name);
            //        String _FileNameTemp1 = FileNameTemp2;
            //        String _filename = _FileNameTemp1.Remove(FileNameTemp2.Length - 6, 6);
                    
            //        if (_filename == fileName)
            //        {
            //            FileStream fs = new FileStream(file.FullName, FileMode.Open);
            //            Texture2D t = Texture2D.FromStream(graphics, fs);
            //            if(_FileNameTemp1.Contains("_File1"))
            //                File1 = t;
            //            else if(_FileNameTemp1.Contains("_File2"))
            //                File2 = t;
            //            fs.Close();
            //        }
            //    }

            //}

            if (File1 != null  &&  File2 != null  &&  prFile != null)
            {
                Color[] levelTile = new Color[File1.Width * File1.Height];
                File1.GetData<Color>(levelTile);

                Color[] levelPieceTile = new Color[File2.Width * File2.Height];
                File2.GetData<Color>(levelPieceTile);

                //String path = content.RootDirectory + Path.DirectorySeparatorChar + "CustomLevels" + Path.DirectorySeparatorChar + prFile.FullName + Path.DirectorySeparatorChar;
                //System.Console.WriteLine(prFile.FullName);
                FileStream fs = new FileStream(prFile.FullName, FileMode.Open);
                //System.IO.Stream _stream = File.OpenWrite(prFile.FullName);//path + fileName + "_pr" + ".txt");
                System.IO.StreamReader sreader = new System.IO.StreamReader(fs);

                for (int i = 0; i < levelPreview.TileType.Length; i++)
                {
                    //if (levelTile[i] == new Color(127, 127, 127))
                    //    levelPreview.TileType[i] = "Wall";
                    //else if (levelTile[i] == new Color(34, 177, 76))
                    //    levelPreview.TileType[i] = "Grass";
                    //else if (levelTile[i] == new Color(195, 195, 195))
                    //    levelPreview.TileType[i] = "WoodFloor";
                    //else
                    //    levelPreview.TileType[i] = "";
                    for(int a = 0; a < tileManager.BasicTile.Length; a++)
                    {
                        if (levelTile[i] == tileManager.BasicTile[a].colorID)
                        {
                            levelPreview.TileType[i] = tileManager.BasicTile[a].name;
                            break;
                        }
                        else if (a >= tileManager.BasicTile.Length - 1)
                        {
                            levelPreview.TileType[i] = "";
                            break;
                        }
                    }
                    for (int a = 0; a < tileManager.AssociatedTypes_Basic.Length; a++)
                    {
                       for(int b = 0; b < tileManager.AssociatedTypes_Basic[a].Count; b++)
                           if (levelTile[i] == tileManager.AssociatedTypes_Basic[a][b].colorID)
                           {
                               levelPreview.TileType[i] = tileManager.AssociatedTypes_Basic[a][b].name;
                               break;
                           }
                    }
                }
                for (int i = 0; i < levelPreview.TileType2.Length; i++)
                {
                    for (int a = 0; a < tileManager.PiecesTile.Length; a++)
                    {
                        if (tileManager.PiecesTile[a].name != "Eraser")
                        {
                            if (levelPieceTile[i] == tileManager.PiecesTile[a].colorID)
                                levelPreview.TileType2[i] = tileManager.PiecesTile[a].name;
                            //System.Console.WriteLine(levelPreview.TileType2[i]);
                            if (levelPreview.TileType2[i] == "Rifle" || levelPreview.TileType2[i] == "Door" || levelPreview.TileType2[i] == "Pistol" ||
                                levelPreview.TileType2[i] == "Machine Gun" || levelPreview.TileType2[i] == "Gatling Gun" || levelPreview.TileType2[i] == "Shotgun" ||
                                levelPreview.TileType2[i] == "Auto-Shotty" || levelPreview.TileType2[i] == "Random Weapon"  || levelPreview.TileType2[i] == "Locked Door" ||
                                levelPreview.TileType2[i] == "Switch"  ||
                                levelPreview.TileType2[i] == "Zapper"  ||
                                levelPreview.TileType2[i] == "Weapon"  ||
                                levelPreview.TileType2[i] == "Item"  ||
                                levelPreview.TileType2[i] == "Teleporter" ||
                                levelPreview.TileType2[i] == "Glass")
                            {
                                levelPreview.GridTile.Add(i);
                                levelPreview.UnsortedTile.Add(i);
                                int p; int.TryParse(sreader.ReadLine(), out p);
                                levelPreview.WeaponPrices.Add(p);
                                int m; int.TryParse(sreader.ReadLine(), out m);
                                levelPreview.AmmoPrices.Add(m);
                                int t; int.TryParse(sreader.ReadLine(), out t);
                                levelPreview.ThirdPrices.Add(t);
                                break;// System.Console.WriteLine();

                            }
                        }
                        else
                            if (levelPieceTile[i] == tileManager.PiecesTile[a].colorID)
                                levelPreview.TileType2[i] = "";

                        for (int w = 0; w < tileManager.weaponTiles.Length; w++)
                        {
                            if (levelPieceTile[i] == tileManager.weaponTiles[w].colorID)
                            {
                               // System.Console.WriteLine("done  " + tileManager.WeaponTilesColor[w]);
                                levelPreview.TileType2[i] = "Weapon";// tileManager.WeaponTiles[w].name;
                                levelPreview.ColorFile2[i] = tileManager.weaponTiles[w].colorID;
                                
                            }
                        }
                        for (int w = 0; w < tileManager.itemTiles.Length; w++)
                        {
                            if (levelPieceTile[i] == tileManager.itemTiles[w].colorID)
                            {
                                levelPreview.TileType2[i] = "Item";
                                levelPreview.ColorFile2[i] = tileManager.itemTiles[w].colorID;

                            }
                        }
                        for (int w = 0; w < tileManager.teleporterTiles.Length; w++)
                        {
                            if (levelPieceTile[i] == tileManager.teleporterTiles[w].colorID)
                            {
                                levelPreview.TileType2[i] = "Teleporter";
                                levelPreview.ColorFile2[i] = tileManager.teleporterTiles[w].colorID;

                            }
                        }
                        //if (levelPieceTile[i] == tileManager.Pistol.colorID)
                        //{
                        //    System.Console.WriteLine("done");
                        //    levelPreview.TileType2[i] = "Weapon";
                        //    levelPreview.ColorFile2[i] = tileManager.Pistol.colorID;
                        //}


                        //if (levelPreview.TileType2[i] == "Rifle"  || levelPreview.TileType2[i] == "Pistol" ||
                        //        levelPreview.TileType2[i] == "Machine Gun" || levelPreview.TileType2[i] == "Gatling Gun" || levelPreview.TileType2[i] == "Shotgun" ||
                        //        levelPreview.TileType2[i] == "Auto-Shotty")
                        //{
                        //    System.Console.WriteLine("done");
                        //    levelPreview.TileType2[i] = "Weapon";
                        //}
                    }

                    //if (levelPieceTile[i] == tileManager.DirtHole.colorID)
                    //    levelPreview.TileType2[i] = "DirtHole";
                    //else if (levelPieceTile[i] == tileManager.RandomBox.colorID)
                    //    levelPreview.TileType2[i] = "RandomWeapon";
                    //else if (levelPieceTile[i] == tileManager.JukeBox.colorID)
                    //    levelPreview.TileType2[i] = "JukeBox";
                    //else if (levelPieceTile[i] == tileManager.MachineGun.colorID)
                    //    levelPreview.TileType2[i] = "MachineGun";
                    //else if (levelPieceTile[i] == tileManager.GatlingGun.colorID)
                    //    levelPreview.TileType2[i] = "GatlingGun";
                    //else if (levelPieceTile[i] == tileManager.Rifle.colorID)
                    //    levelPreview.TileType2[i] = "Rifle";
                    //else if (levelPieceTile[i] == tileManager.ShotGun.colorID)
                    //    levelPreview.TileType2[i] = "ShotGun";
                    //else if (levelPieceTile[i] == tileManager.Door.colorID)
                    //    levelPreview.TileType2[i] = "Door";
                    //else if (levelPieceTile[i] == tileManager.Spawn.colorID)
                    //    levelPreview.TileType2[i] = "Spawn";
                    //else
                    //    levelPreview.TileType2[i] = "";
                   
                }
                sreader.Close();
                fs.Close();

               
            }

            //String path = content.RootDirectory + Path.DirectorySeparatorChar + "CustomLevels" + Path.DirectorySeparatorChar + fileName + Path.DirectorySeparatorChar;
            //System.IO.Stream _stream = File.OpenWrite(path + fileName + "_pr" + ".txt");
            //System.IO.StreamReader sreader = new System.IO.StreamReader(_stream);



            //sreader.Close();
            //_stream.Close();
           
        }
        private void Save(Color[] file1, Color[] file2)
        {
            int NumberOfSpawns = 0;
            int NumberOfDirHoles = 0;
            foreach(String temp in levelPreview.TileType2)
            {
                if (temp == "Spawn")
                    NumberOfSpawns++;
                if (temp == "Dirt Hole")
                    NumberOfDirHoles++;
            }
            if (NumberOfSpawns == 4 && NumberOfDirHoles > 0)
            {
                int textureWidth = 40;
                int textureHeight = 40;
                if (!Directory.Exists(content.RootDirectory + Path.DirectorySeparatorChar + "CustomLevels" + Path.DirectorySeparatorChar + fileName))
                    Directory.CreateDirectory(content.RootDirectory + Path.DirectorySeparatorChar + "CustomLevels" + Path.DirectorySeparatorChar + fileName);
                String path = content.RootDirectory + Path.DirectorySeparatorChar + "CustomLevels" + Path.DirectorySeparatorChar + fileName + Path.DirectorySeparatorChar;
                Texture2D texture2D = new Texture2D(graphics, textureWidth, textureHeight, false, SurfaceFormat.Color);

                Color[] colorData = new Color[textureWidth * textureHeight];
                texture2D.GetData<Color>(colorData);
                for (int i = 0; i < colorData.Length; i++)
                {
                    colorData[i] = file1[i];
                }
                texture2D.SetData<Color>(colorData);

                Stream steam = File.OpenWrite(path + fileName + "_File1" +/*"_" + Guid.NewGuid().ToString() + */".png");
                texture2D.SaveAsPng(steam, textureWidth, textureHeight);
                steam.Close();

                Texture2D levelPieces = new Texture2D(graphics, textureWidth, textureHeight, false, SurfaceFormat.Color);
                Color[] colorData2 = new Color[textureWidth * textureHeight];
                levelPieces.GetData<Color>(colorData2);
                for (int i = 0; i < colorData2.Length; i++)
                {
                    colorData2[i] = file2[i];
                }
                levelPieces.SetData<Color>(colorData2);

                Stream _steam = File.OpenWrite(path + fileName + "_File2" +/*"_" + Guid.NewGuid().ToString() + */".png");
                levelPieces.SaveAsPng(_steam, textureWidth, textureHeight);
                _steam.Close();

                if (File.Exists(path + fileName + "_lb" + ".txt"))
                    File.Delete(path + fileName + "_lb" + ".txt");
                System.IO.Stream _stream2 = File.OpenWrite(path + fileName + "_lb" + ".txt");
                System.IO.StreamWriter swriter = new System.IO.StreamWriter(_stream2);
                for (int i = 0; i < 5; i++)
                {
                    swriter.WriteLine("0");
                }
                swriter.Close();
                _stream2.Close();

                if(File.Exists(path + fileName + "_pr" + ".txt"))
                    File.Delete(path + fileName + "_pr" + ".txt");
                System.IO.Stream _stream3 = File.OpenWrite(path + fileName + "_pr" + ".txt");
                System.IO.StreamWriter swriter3 = new System.IO.StreamWriter(_stream3);
                
                //List<int> unsorted = new List<int>(); unsorted = levelPreview.GridTile;
                //List<int> sorted = new List<int>(); sorted = unsorted;
               
                levelPreview.SortedTile.Sort();

                //System.Console.WriteLine(levelPreview.GridTile.Count + "-");
                //System.Console.WriteLine(levelPreview.SortedTile.Count + "-");
                //System.Console.WriteLine(levelPreview.UnsortedTile.Count + "-");
                //System.Console.WriteLine(levelPreview.WeaponPrices.Count + "-");
                //System.Console.WriteLine(levelPreview.AmmoPrices.Count + "-");
                //System.Console.WriteLine("\n");

                
               // System.Console.WriteLine(sorted[0] + "  " + sorted[1]);
                int onSort = 0;
                onSort = 0;
                while (onSort < levelPreview.SortedTile.Count)
                    for (int i = 0; i < levelPreview.SortedTile.Count; i++)
                    {
                        if(onSort < levelPreview.SortedTile.Count)
                        if (levelPreview.GridTile[i] == levelPreview.SortedTile[onSort])
                        {
                           // System.Console.WriteLine(i + "  " + onSort + "  " + levelPreview.SortedTile[onSort] + "   " + levelPreview.GridTile[i] + "   " + levelPreview.WeaponPrices[i]);
                            swriter3.WriteLine(levelPreview.WeaponPrices[i]);
                            swriter3.WriteLine(levelPreview.AmmoPrices[i]);
                            swriter3.WriteLine(levelPreview.ThirdPrices[i]);
                            onSort++;
                            break;
                        }
                    }
                
                swriter3.Close();
                _stream3.Close();

                levelPreview.SortedTile.Clear();
                levelPreview.SortedTile.AddRange(levelPreview.UnsortedTile);
                //levelPreview.SortedTile = levelPreview.UnsortedTile;
                //if (onSort >= levelPreview.SortedTile.Count)
                //{
                //    levelPreview.GridTile = levelPreview.SortedTile;
                //    onSort = 0;
                //}

                SaveFadeValue = 255;
            }
            else
            {
                show_WarningWindow = true;
                WarningMessege = "You Must Have 4 Spawns And\nAt Least 1 Dirt Hole Before\nSaving.";
            }
            
        }

        private Boolean EditingName = false;
        private String fileName = "New Level 01";
        KeyboardState keyState;
        private KeyboardState oldKeyState = Keyboard.GetState();
        Keys[] keysToCheck = new Keys[] {   

     Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,  

     Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,  

     Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,  

     Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,  

     Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,  

     Keys.Z, Keys.Back, Keys.Space, Keys.D0,
     Keys.D1, Keys.D2, Keys.D3 , Keys.D4,
        Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
        Keys.Escape, Keys.Enter};  

        private void TypeName()
        {
            if (EditingName)
            {
                oldKeyState = keyState;
                keyState = Keyboard.GetState();
                foreach (Keys key in keyState.GetPressedKeys())
                {
                    if (keysToCheck.Contains(key))
                    {
                        if (oldKeyState.IsKeyUp(key))
                        {
                            if (key == Keys.Back)
                            {
                                if (fileName.Length > 0)
                                    fileName = fileName.Remove(fileName.Length - 1, 1);
                            }
                            else if (key == Keys.Space)
                            {
                                fileName += " ";
                            }
                            else if (key == Keys.Enter  ||  key == Keys.Escape)
                            {
                                EditingName = false;
                            }
                            else
                            {
                                AddKeyToText(key);
                            }
                        }
                    }
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Button_Selected != 4)
                {
                    EditingName = false;
                }
            }

        }
        private void AddKeyToText(Keys key)
        {
            string newChar = "";
            switch (key)
            {

                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.D0:
                    newChar += "0";
                    break;
                case Keys.D1:
                    newChar += "1";
                    break;
                case Keys.D2:
                    newChar += "2";
                    break;
                case Keys.D3:
                    newChar += "3";
                    break;
                case Keys.D4:
                    newChar += "4";
                    break;
                case Keys.D5:
                    newChar += "5";
                    break;
                case Keys.D6:
                    newChar += "6";
                    break;
                case Keys.D7:
                    newChar += "7";
                    break;
                case Keys.D8:
                    newChar += "8";
                    break;
                case Keys.D9:
                    newChar += "9";
                    break;


            }

            if (Keyboard.GetState().IsKeyDown(Keys.RightShift) || Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }
            if(fileName.Length < 20)
            fileName += newChar;
        }

        private void UpdateCamera()
        {
            if (!EditingName  &&  !levelPreview.EditingPrice)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))  &&  camCen.Y > 0)
                    camCen.Y -= 5;
                if ((Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) && camCen.Y < levelPreview.height * 40)
                    camCen.Y += 5;

                if ((Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) && camCen.X > 0)
                    camCen.X -= 5;
                if ((Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) && camCen.X < levelPreview.width * 40)
                    camCen.X += 5;
            }


            
        }
        private MouseState oldState = Mouse.GetState();
        private void UpdateMouseSelection()
        {
           
            MouseState newState = Mouse.GetState();
            if (oldState.LeftButton != newState.LeftButton)
            {
                if (newState.LeftButton == ButtonState.Pressed)
                {
                    if (!show_WarningWindow && !ShowOpenWindow  &&  !levelPreview.EditingPrice)
                    {
                        if (Button_Selected == 0)
                        {
                            InitMouseUp = false;
                            mouseReleased = false;
                            Back = true;
                        }
                        if (Button_Selected == 1)
                            Save(levelPreview.ColorFile1, levelPreview.ColorFile2);
                        if (Button_Selected == 2 )
                        {
                           // System.Console.WriteLine(LevelName.Count);
                           // if (LevelName.Count > 0)
                           // {
                                _load = false;
                                ShowOpenWindow = true;
                            //a
                            //Open();
                        }

                        if (Button_Selected == 3)
                            ResetLevel();

                        if (Button_Selected == 4)
                            EditingName = !EditingName;
                    }
                    else if(show_WarningWindow)
                    {
                        if (HighlightOKButton)
                            show_WarningWindow = false;
                    }
                    else if (ShowOpenWindow)
                    {
                        if (LevelLoadSelected < Levels.Count)
                        {
                            Open(Levels[LevelLoadSelected], LevelPieces[LevelLoadSelected], LevelPr[LevelLoadSelected]);
                            fileName = LevelName[LevelLoadSelected];
                            ShowOpenWindow = false;
                        }
                    }
                }
                
                //if (newState.ScrollWheelValue - oldState.ScrollWheelValue == 120 && camera.Zoom < 3)
                //    camera.Zoom += 0.05f;
                //if (newState.ScrollWheelValue - oldState.ScrollWheelValue == -120 && camera.Zoom > 0.1)
                //    camera.Zoom -= 0.05f;
                oldState = newState;
            }
        }
        private void ResetLevel()
        {
            int x = 0;
            int y = 0;
            int width = 50;
            int height = 50;
            for (int i = 0; i < levelPreview.grid.Length; i++)
            {

                levelPreview.grid[i] = new Rectangle(x * width, y * height, width, height);
                levelPreview.grid2[i] = new Rectangle(x * width, y * height, width, height);

                if (x == 0 || x == 39 || y == 0 || y == 39)
                    levelPreview.TileType[i] = "Wall";
                else
                    levelPreview.TileType[i] = "Grass";
                x++;
                if (x >= 40)
                {
                    x = 0;
                    y++;
                }
                levelPreview.TileType2[i] = "";

            }
            levelPreview.GridTile.Clear();
            levelPreview.SortedTile.Clear();
            levelPreview.UnsortedTile.Clear();
            levelPreview.WeaponPrices.Clear();
            levelPreview.AmmoPrices.Clear();
            levelPreview.ThirdPrices.Clear();
            
            fileName = "New Level 01";
        }
        private void UpdateMouse()
        {
            Rectangle mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
            if (!show_WarningWindow &&  !levelPreview.EditingPrice  &&  !ShowOpenWindow)
            {
                for (int i = 0; i < Button.Length; i++)
                {
                    if (mouseRect.Intersects(Button[i]))
                        Button_Selected = i;

                    if (Button_Selected >= 0)
                        if (i >= Button.Length - 1 && !mouseRect.Intersects(Button[Button_Selected]))
                            Button_Selected = -1;
                }
            }
            if (show_WarningWindow)
            {
                if (mouseRect.Intersects(OKButton))
                    HighlightOKButton = true;
                else
                    HighlightOKButton = false;
            }
            if (ShowOpenWindow)
            {
                if (LevelSelect_Button.Length > 0)
                {
                    for (int i = 0; i < LevelSelect_Button.Length; i++)
                    {
                        if (mouseRect.Intersects(LevelSelect_Button[i]))
                        {
                            LevelLoadSelected = i;
                        }
                    }
                    if (Mouse.GetState().ScrollWheelValue != PrevMouseScroll.ScrollWheelValue)
                    {
                        if (Open_ScrollY <= 0 && -Open_ScrollY <= LevelSelect_Button.Length * LevelSelect_Button[0].Height)
                            Open_ScrollY += (Mouse.GetState().ScrollWheelValue - PrevMouseScroll.ScrollWheelValue) / 4;
                        PrevMouseScroll = Mouse.GetState();
                    }
                    if (Open_ScrollY >= 0)
                        Open_ScrollY = 0;
                    if (LevelSelect_Button.Length > 0)
                        if (-Open_ScrollY >= LevelSelect_Button.Length * LevelSelect_Button[0].Height)
                            Open_ScrollY = -LevelSelect_Button.Length * LevelSelect_Button[0].Height;

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        ShowOpenWindow = false;
                }
                else
                    ShowOpenWindow = false;
            }
            else
            {
                Open_ScrollY = 0;
                PrevMouseScroll = Mouse.GetState();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            levelPreview.Draw(spriteBatch);
            spriteBatch.End();
            
            spriteBatch.Begin();
            tileSelection.Draw(spriteBatch);
            for (int i = 0; i < Button.Length; i++)
            {
                Button[i] = new Rectangle(tileSelection.TileSelectionWindow.X, tileSelection.TileSelectionWindow.Y + tileSelection.TileSelectionWindow.Height - (40 * (i + 1)), tileSelection.TileSelectionWindow.Width, 40);
                if(Button_Selected == i  &&  i != 4)
                    spriteBatch.Draw(square, Button[i], Color.Gray);

                if (i == 4 && Button_Selected  == i && !EditingName)
                    spriteBatch.Draw(square, Button[i], Color.Gray);
                if (i == 4 && EditingName)
                    spriteBatch.Draw(square, Button[i], Color.DarkGray);
                    
                spriteBatch.DrawString(font, Button_name[i], new Vector2(Button[i].X + 10, Button[i].Y), Color.Black);
                
            }

            if (levelPreview.EditingPrice)
            {
                int w = 500;
                int h = 250;
                WarningWindow = new Rectangle(graphics.Viewport.Width / 2 - (w / 2), graphics.Viewport.Height / 2 - (h / 2), w, h);
                spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(40, 40, 40, 190));
                spriteBatch.Draw(square, WarningWindow, Color.White);

                spriteBatch.Draw(square, new Rectangle(WarningWindow.X, WarningWindow.Y + ((levelPreview.PriceEditing + 1) * 45), WarningWindow.Width, 45), Color.Gray);

                if (levelPreview.EditingPriceName != "Weapon"  &&  levelPreview.EditingPriceName != "Item"  &&  levelPreview.EditingPriceName != "Teleporter")
                {
                    spriteBatch.DrawString(font, levelPreview.EditingPriceName, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 5), Color.Black);
                }

                if (levelPreview.EditingPriceName == "Weapon")
                {
                    if (WeaponTypeIndex < tileManager.weaponTiles.Length) 
                    _Weapon = tileManager.weaponTiles[WeaponTypeIndex]._WeaponTile.name;

                    //if(!levelPreview.BumpUpWeapon)
                    for (int i = 0; i < tileManager.weaponTiles.Length; i++)
                    {
                        if (levelPreview.ColorFile2[levelPreview.TyleTypeEditing] == tileManager.weaponTiles[i].colorID)
                        {
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.weaponTiles[i].colorID;
                            _Weapon = tileManager.weaponTiles[i]._WeaponTile.name;
                            colorID = tileManager.weaponTiles[i]._WeaponTile.colorID;
                            WeaponTypeIndex = i;
                        }
                    }
                   

                    if (levelPreview.BumpUpWeapon)
                    {
                       // System.Console.WriteLine(tileManager.WeaponTiles[WeaponTypeIndex + 1].name + "    " + tileManager.WeaponTiles[WeaponTypeIndex].name);
                        if (WeaponTypeIndex >= tileManager.weaponTiles.Length - 1)
                         {
                            WeaponTypeIndex = 0;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.weaponTiles[WeaponTypeIndex].colorID;
                            _Weapon = tileManager.weaponTiles[WeaponTypeIndex]._WeaponTile.name;
                            colorID = tileManager.weaponTiles[WeaponTypeIndex]._WeaponTile.colorID;
                            levelPreview.PriceNum = "" + tileManager.weaponTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.weaponTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpUpWeapon = false;
                            
                        }
                        else
                        {
                         // WeaponTypeIndex++;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.weaponTiles[WeaponTypeIndex + 1].colorID;

                            _Weapon = tileManager.weaponTiles[WeaponTypeIndex + 1]._WeaponTile.name;
                            colorID = tileManager.weaponTiles[WeaponTypeIndex + 1].colorID;
                         WeaponTypeIndex = WeaponTypeIndex + 1;
                         levelPreview.PriceNum = "" + tileManager.weaponTiles[WeaponTypeIndex].Price;

                         levelPreview.AmmoPrice = "" + tileManager.weaponTiles[WeaponTypeIndex].AmmoPrice;
                         //System.Console.WriteLine(levelPreview.WeaponPrices[levelPreview.editing]);
                         levelPreview.BumpUpWeapon = false;
                          }



                        //System.Console.WriteLine(WeaponTypeIndex + "    " + tileManager.WeaponTiles[WeaponTypeIndex].colorID);
                        levelPreview.BumpUpWeapon = false;
                    }
                    if (levelPreview.BumpDownWeapon)
                    {
                        if (WeaponTypeIndex <= 0)
                        {
                            WeaponTypeIndex = tileManager.weaponTiles.Length - 1;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.weaponTiles[WeaponTypeIndex].colorID;
                            _Weapon = tileManager.weaponTiles[WeaponTypeIndex]._WeaponTile.name;
                            colorID = tileManager.weaponTiles[WeaponTypeIndex]._WeaponTile.colorID;
                            levelPreview.PriceNum = "" + tileManager.weaponTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.weaponTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpDownWeapon = false;

                        }
                        else
                        {
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.weaponTiles[WeaponTypeIndex - 1].colorID;
                            _Weapon = tileManager.weaponTiles[WeaponTypeIndex - 1]._WeaponTile.name;
                            colorID = tileManager.weaponTiles[WeaponTypeIndex - 1].colorID;
                            WeaponTypeIndex = WeaponTypeIndex - 1;
                            levelPreview.PriceNum = "" + tileManager.weaponTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.weaponTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpDownWeapon = false;
                        }
                    }
                    //System.Console.WriteLine(levelPreview.ColorFile2[levelPreview.TyleTypeEditing]);
                    //for (int i = 0; i < tileManager.WeaponTiles.Length; i++)
                    //{
                    //    if (levelPreview.ColorFile2[levelPreview.TyleTypeEditing] == tileManager.WeaponTiles[i].colorID)
                    //    {
                    //        System.Console.WriteLine(tileManager.WeaponTiles[i].name);
                    //        _Weapon = tileManager.WeaponTiles[i].name;
                    //        colorID = tileManager.WeaponTiles[i].colorID;
                    //        //WeaponTypeIndex = i;
                    //        System.Console.WriteLine(tileManager.WeaponTiles[i].name);
                    //    }
                    //}
                   
                    spriteBatch.DrawString(font, "Weapon: " + _Weapon, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 5), Color.Black);
                    //levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = colorID;

                    spriteBatch.Draw(square, new Rectangle(WarningWindow.X + WarningWindow.Width - 35, WarningWindow.Y + 5, 30, 30), Color.Black);
                    spriteBatch.DrawString(font, ">", new Vector2(WarningWindow.X + WarningWindow.Width - 27, WarningWindow.Y ), Color.White);

                    spriteBatch.Draw(square, new Rectangle(WarningWindow.X + WarningWindow.Width - 70, WarningWindow.Y + 5, 30, 30), Color.Black);
                    spriteBatch.DrawString(font, "<", new Vector2(WarningWindow.X + WarningWindow.Width - 62, WarningWindow.Y), Color.White);
                }
                if (levelPreview.EditingPriceName == "Item")
                {
                    if(WeaponTypeIndex < tileManager.itemTiles.Length)
                    _Weapon = tileManager.itemTiles[WeaponTypeIndex]._WeaponTile.name;

                    //if(!levelPreview.BumpUpWeapon)
                    for (int i = 0; i < tileManager.itemTiles.Length; i++)
                    {
                        if (levelPreview.ColorFile2[levelPreview.TyleTypeEditing] == tileManager.itemTiles[i].colorID)
                        {
                            //System.Console.WriteLine(tileManager.itemTiles[i]._WeaponTile.name);
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.itemTiles[i].colorID;
                            _Weapon = tileManager.itemTiles[i]._WeaponTile.name;
                            colorID = tileManager.itemTiles[i]._WeaponTile.colorID;
                            WeaponTypeIndex = i;
                        }
                    }


                    if (levelPreview.BumpUpWeapon)
                    {
                        // System.Console.WriteLine(tileManager.WeaponTiles[WeaponTypeIndex + 1].name + "    " + tileManager.WeaponTiles[WeaponTypeIndex].name);
                        if (WeaponTypeIndex >= tileManager.itemTiles.Length - 1)
                        {
                            WeaponTypeIndex = 0;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.itemTiles[WeaponTypeIndex].colorID;
                            _Weapon = tileManager.itemTiles[WeaponTypeIndex]._WeaponTile.name;
                            colorID = tileManager.itemTiles[WeaponTypeIndex]._WeaponTile.colorID;
                            levelPreview.PriceNum = "" + tileManager.itemTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.itemTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpUpWeapon = false;

                        }
                        else
                        {
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.itemTiles[WeaponTypeIndex + 1].colorID;
                            _Weapon = tileManager.itemTiles[WeaponTypeIndex + 1]._WeaponTile.name;
                            colorID = tileManager.itemTiles[WeaponTypeIndex + 1].colorID;
                            WeaponTypeIndex = WeaponTypeIndex + 1;
                            levelPreview.PriceNum = "" + tileManager.itemTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.itemTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpUpWeapon = false;
                        }



                        //System.Console.WriteLine(WeaponTypeIndex + "    " + tileManager.WeaponTiles[WeaponTypeIndex].colorID);
                        levelPreview.BumpUpWeapon = false;
                    }
                    if (levelPreview.BumpDownWeapon)
                    {
                        if (WeaponTypeIndex <= 0)
                        {
                            WeaponTypeIndex = tileManager.itemTiles.Length - 1;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.itemTiles[WeaponTypeIndex].colorID;
                            _Weapon = tileManager.itemTiles[WeaponTypeIndex]._WeaponTile.name;
                            colorID = tileManager.itemTiles[WeaponTypeIndex]._WeaponTile.colorID;
                            levelPreview.PriceNum = "" + tileManager.itemTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.itemTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpDownWeapon = false;

                        }
                        else
                        {
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.itemTiles[WeaponTypeIndex - 1].colorID;
                            _Weapon = tileManager.itemTiles[WeaponTypeIndex - 1]._WeaponTile.name;
                            colorID = tileManager.itemTiles[WeaponTypeIndex - 1].colorID;
                            WeaponTypeIndex = WeaponTypeIndex - 1;
                            levelPreview.PriceNum = "" + tileManager.itemTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.itemTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpDownWeapon = false;
                        }
                    }

                    spriteBatch.DrawString(font, "Item: " + _Weapon, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 5), Color.Black);

                    spriteBatch.Draw(square, new Rectangle(WarningWindow.X + WarningWindow.Width - 35, WarningWindow.Y + 5, 30, 30), Color.Black);
                    spriteBatch.DrawString(font, ">", new Vector2(WarningWindow.X + WarningWindow.Width - 27, WarningWindow.Y), Color.White);

                    spriteBatch.Draw(square, new Rectangle(WarningWindow.X + WarningWindow.Width - 70, WarningWindow.Y + 5, 30, 30), Color.Black);
                    spriteBatch.DrawString(font, "<", new Vector2(WarningWindow.X + WarningWindow.Width - 62, WarningWindow.Y), Color.White);
                }
                if (levelPreview.EditingPriceName == "Teleporter")
                {
                    if (WeaponTypeIndex < tileManager.teleporterTiles.Length)
                        _Weapon = tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.name;

                    //if(!levelPreview.BumpUpWeapon)
                    for (int i = 0; i < tileManager.teleporterTiles.Length; i++)
                    {
                        if (levelPreview.ColorFile2[levelPreview.TyleTypeEditing] == tileManager.teleporterTiles[i].colorID)
                        {
                            //System.Console.WriteLine(tileManager.itemTiles[i]._WeaponTile.name);
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.teleporterTiles[i].colorID;
                            _Weapon = tileManager.teleporterTiles[i]._WeaponTile.name;
                            colorID = tileManager.teleporterTiles[i]._WeaponTile.colorID;
                            WeaponTypeIndex = i;
                        }
                    }


                    if (levelPreview.BumpUpWeapon)
                    {
                        if (WeaponTypeIndex >= tileManager.teleporterTiles.Length - 1)
                        {
                            WeaponTypeIndex = 0;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.teleporterTiles[WeaponTypeIndex].colorID;
                            _Weapon = tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.name;
                            colorID = tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.colorID;
                            levelPreview.PriceNum = "" + tileManager.teleporterTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.teleporterTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpUpWeapon = false;

                        }
                        else
                        {
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.teleporterTiles[WeaponTypeIndex + 1].colorID;
                            _Weapon = tileManager.teleporterTiles[WeaponTypeIndex + 1]._WeaponTile.name;
                            colorID = tileManager.teleporterTiles[WeaponTypeIndex + 1].colorID;
                            WeaponTypeIndex = WeaponTypeIndex + 1;
                            levelPreview.PriceNum = "" + tileManager.teleporterTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.teleporterTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpUpWeapon = false;
                        }

                        levelPreview.BumpUpWeapon = false;
                    }
                    if (levelPreview.BumpDownWeapon)
                    {
                        if (WeaponTypeIndex <= 0)
                        {
                            WeaponTypeIndex = tileManager.teleporterTiles.Length - 1;
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.teleporterTiles[WeaponTypeIndex].colorID;
                            _Weapon = tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.name;
                            colorID = tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.colorID;
                            levelPreview.PriceNum = "" + tileManager.teleporterTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.teleporterTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpDownWeapon = false;

                        }
                        else
                        {
                            levelPreview.ColorFile2[levelPreview.TyleTypeEditing] = tileManager.teleporterTiles[WeaponTypeIndex - 1].colorID;
                            _Weapon = tileManager.teleporterTiles[WeaponTypeIndex - 1]._WeaponTile.name;
                            colorID = tileManager.teleporterTiles[WeaponTypeIndex - 1].colorID;
                            WeaponTypeIndex = WeaponTypeIndex - 1;
                            levelPreview.PriceNum = "" + tileManager.teleporterTiles[WeaponTypeIndex].Price;
                            levelPreview.AmmoPrice = "" + tileManager.teleporterTiles[WeaponTypeIndex].AmmoPrice;
                            levelPreview.BumpDownWeapon = false;
                        }
                    }

                    spriteBatch.DrawString(font, "Type: " + _Weapon, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 5), Color.Black);

                    spriteBatch.Draw(square, new Rectangle(WarningWindow.X + WarningWindow.Width - 35, WarningWindow.Y + 5, 30, 30), Color.Black);
                    spriteBatch.DrawString(font, ">", new Vector2(WarningWindow.X + WarningWindow.Width - 27, WarningWindow.Y), Color.White);

                    spriteBatch.Draw(square, new Rectangle(WarningWindow.X + WarningWindow.Width - 70, WarningWindow.Y + 5, 30, 30), Color.Black);
                    spriteBatch.DrawString(font, "<", new Vector2(WarningWindow.X + WarningWindow.Width - 62, WarningWindow.Y), Color.White);
                }
                if (levelPreview.EditingPriceName != "Door" && levelPreview.EditingPriceName != "Switch" && levelPreview.EditingPriceName != "Locked Door"
                    &&  levelPreview.EditingPriceName != "Zapper"  &&  levelPreview.EditingPriceName != "Weapon"  &&  levelPreview.EditingPriceName != "Item"
                    && levelPreview.EditingPriceName != "Teleporter" && levelPreview.EditingPriceName != "Glass")
                    spriteBatch.DrawString(font, "Weapon Price: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);
                else if (levelPreview.EditingPriceName == "Door")
                    spriteBatch.DrawString(font, "Door Price: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);
                else if (levelPreview.EditingPriceName == "Locked Door" || levelPreview.EditingPriceName == "Switch" || levelPreview.EditingPriceName == "Zapper")
                    spriteBatch.DrawString(font, "Channel: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);
                else if(levelPreview.EditingPriceName == "Weapon")
                    spriteBatch.DrawString(font, "Weapon Price: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);
                else if (levelPreview.EditingPriceName == "Item")
                    spriteBatch.DrawString(font, "Item Price: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);
                else if (levelPreview.EditingPriceName == "Teleporter")
                    spriteBatch.DrawString(font, "Channel: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);
                else if (levelPreview.EditingPriceName == "Glass")
                    spriteBatch.DrawString(font, "Glass Durability: " + levelPreview.PriceNum, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 45), Color.Black);

                if (levelPreview.EditingPriceName != "Door"  &&  levelPreview.EditingPriceName != "Random Weapon"
                    && levelPreview.EditingPriceName != "Locked Door" && levelPreview.EditingPriceName != "Switch"
                    && levelPreview.EditingPriceName != "Zapper"
                    && levelPreview.EditingPriceName != "Weapon"
                    && levelPreview.EditingPriceName != "Item"
                    && levelPreview.EditingPriceName != "Teleporter"
                    && levelPreview.EditingPriceName != "Glass")
                spriteBatch.DrawString(font, "Ammo Price: " + levelPreview.AmmoPrice, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 90), Color.Black);
                else if (levelPreview.EditingPriceName == "Switch")
                    spriteBatch.DrawString(font, "Switch Price: " + levelPreview.AmmoPrice, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 90), Color.Black);
                else if (levelPreview.EditingPriceName == "Zapper")
                    spriteBatch.DrawString(font, "Zap Duration: " + levelPreview.AmmoPrice, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 90), Color.Black);
                else if (levelPreview.EditingPriceName == "Weapon")
                    spriteBatch.DrawString(font, "Ammo Price: " + levelPreview.AmmoPrice, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 90), Color.Black);
                else if (levelPreview.EditingPriceName == "Teleporter")
                {
                    if(tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.name ==  tileManager.Sender.name)
                        spriteBatch.DrawString(font, "Teleport Price: " + levelPreview.AmmoPrice, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 90), Color.Black);
                }
                if (levelPreview.EditingPriceName == "Teleporter")
                {
                    if (tileManager.teleporterTiles[WeaponTypeIndex]._WeaponTile.name == tileManager.Sender.name)
                        spriteBatch.DrawString(font, "Cool Down Duration: " + levelPreview.ThirdPrice, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 135), Color.Black);
                }
                Rectangle _OKButton = new Rectangle(WarningWindow.X + WarningWindow.Width - 150, WarningWindow.Y + WarningWindow.Height - 50, 125, 40);
                if(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(_OKButton))
                    spriteBatch.Draw(square, _OKButton, Color.Gray);
                spriteBatch.DrawString(font, "OK", new Vector2((_OKButton.X + _OKButton.Width / 2) - (font.MeasureString("OK").X / 2), _OKButton.Y), Color.Black);
            }

            if (show_WarningWindow)
            {
                int w = 500;
                int h = 250;
                WarningWindow = new Rectangle(graphics.Viewport.Width / 2 - (w / 2), graphics.Viewport.Height / 2 - (h / 2), w, h);
                OKButton = new Rectangle(WarningWindow.X + WarningWindow.Width - 150, WarningWindow.Y + WarningWindow.Height - 50, 125, 40);

                spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(40, 40, 40, 190));
                spriteBatch.Draw(square, WarningWindow, Color.White);
                spriteBatch.DrawString(font, WarningMessege, new Vector2(WarningWindow.X + 10, WarningWindow.Y + 10), Color.Black);
                if (HighlightOKButton)
                    spriteBatch.Draw(square, OKButton, Color.Gray);
                spriteBatch.DrawString(font, "OK", new Vector2((OKButton.X + OKButton.Width / 2) - (font.MeasureString("OK").X / 2), OKButton.Y), Color.Black);
            }
            if (ShowOpenWindow)
            {
                spriteBatch.Draw(square, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), new Color(40, 40, 40, 190));
                DrawLevels(spriteBatch);
            }

            if (SaveFadeValue > 0)
            {
                spriteBatch.Draw(square, new Rectangle(5, 10, (int)font.MeasureString("Saved").X + 10, 40), new Color(0,0,0,SaveFadeValue));
                spriteBatch.DrawString(font, "Saved", new Vector2(10, 10), new Color(SaveFadeValue, SaveFadeValue, SaveFadeValue, SaveFadeValue));
                SaveFadeValue -= 1;
            }
            int y = 5;
            if (levelPreview.LeftButtonTip != "")
            {
                spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width - (int)font.MeasureString(levelPreview.LeftButtonTip).X - 85, y, (int)font.MeasureString(levelPreview.LeftButtonTip).X + 65, 40), Color.Black);
                spriteBatch.DrawString(font, levelPreview.LeftButtonTip, new Vector2(graphics.Viewport.Width - (int)font.MeasureString(levelPreview.LeftButtonTip).X - 30, y), Color.White);
                spriteBatch.Draw(Fire1, new Rectangle(graphics.Viewport.Width - (int)font.MeasureString(levelPreview.LeftButtonTip).X - 30 - 50, y, 40, 40), Color.White);
                y = 50;
            }
            if (levelPreview.RightButtonTip != "")
            {
                spriteBatch.Draw(square, new Rectangle(graphics.Viewport.Width - (int)font.MeasureString(levelPreview.RightButtonTip).X - 85, y, (int)font.MeasureString(levelPreview.RightButtonTip).X + 65, 40), Color.Black);
                spriteBatch.DrawString(font, levelPreview.RightButtonTip, new Vector2(graphics.Viewport.Width - (int)font.MeasureString(levelPreview.RightButtonTip).X - 30, y), Color.White);
                spriteBatch.Draw(Fire2, new Rectangle(graphics.Viewport.Width - (int)font.MeasureString(levelPreview.RightButtonTip).X - 30 - 50, y, 40, 40), Color.White);
            }
            spriteBatch.End();
        }

        private void DrawLevels(SpriteBatch spriteBatch)
        {

                //Rectangle background = new Rectangle(LevelSelect_Button[0].X, LevelSelect_Button[0].Y - 40, 350 + 40, LevelSelect_Button[LevelSelect_Button.Length - 1].Y - LevelSelect_Button[0].Y + 80);
               // spriteBatch.Draw(square, background, Color.Black);
                spriteBatch.Draw(square, new Rectangle(30, 0, 410, graphics.Viewport.Height), new Color(0, 0, 0, 225));
                //spriteBatch.DrawString(font, "Open", new Vector2(background.X, background.Y), Color.White);
                //spriteBatch.Draw(EscapeKeyTexture, new Rectangle(background.X + background.Width - 40, background.Y, 40, 40), Color.White);
                for (int i = 0; i < Levels.Count; i++)
                {
                    LevelSelect_Button[i] = new Rectangle(50, 55 + (i * 45) + Open_ScrollY, 390, 40);

                    if (i == LevelLoadSelected)
                        spriteBatch.Draw(square, LevelSelect_Button[i], new Color(120, 120, 120, 70));
                    else
                        spriteBatch.Draw(square, LevelSelect_Button[i], new Color(40, 40, 40, 10));
                    spriteBatch.DrawString(font, "" + LevelName[i], new Vector2(55, 55 + (i * 45) + Open_ScrollY), Color.White);






                    spriteBatch.Draw(square, new Rectangle(35, 55, 10, graphics.Viewport.Height - 70), Color.DarkGray);
                    spriteBatch.Draw(content.Load<Texture2D>("KeyBoard Keys/Mouse_Scroll"), new Rectangle(32,
                       (int)(55 + ((-Open_ScrollY / (float)(LevelSelect_Button.Length * LevelSelect_Button[0].Height)) * (graphics.Viewport.Height - 110)))
                        , 16, 40), Color.White);


                    spriteBatch.Draw(square, new Rectangle(30, 0, 410, 55), Color.Black);
                    spriteBatch.DrawString(font, "Open", new Vector2(50, 15), Color.White);
                    spriteBatch.Draw(EscapeKeyTexture, new Rectangle(400, 15, 40, 40), Color.White);

                    spriteBatch.Draw(square, new Rectangle(30, graphics.Viewport.Height - 15, 410, 15), Color.Black);

                }
        }
    }
}
