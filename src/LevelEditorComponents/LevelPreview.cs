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

namespace SurvivalShooter.LevelEditorComponents
{
    class LevelPreview
    {
        TileManager tileManager;
        private Texture2D square;
        private SpriteFont font;
        private GraphicsDevice graphics;

        private int TileHover = -1;
        private int Prev_TileHover = -1;

        //private Texture2D GrassTile;
        //private Texture2D WallTile;
        //private Texture2D WoodTile;


        //private Texture2D DirtHole;
        //private Texture2D RandomWeapon;
        //private Texture2D JukeBox;
        //private Texture2D MachineGun;
        //private Texture2D GatlingGun;
        //private Texture2D Door;
        //private Texture2D Spawn;

        public String[] TileType = new String[1600];
        public Rectangle[] grid = new Rectangle[1600];
        public Color[] ColorFile1 = new Color[1600];

        public String[] TileType2 = new String[1600];
        public Rectangle[] grid2 = new Rectangle[1600];
        public Color[] ColorFile2 = new Color[1600];

        public int width = 50;
        public int height = 50;

       // public int[] Prices = new int[1600];
        public List<int> WeaponPrices = new List<int>();
        public List<int> AmmoPrices = new List<int>();
        public List<int> ThirdPrices = new List<int>();
        public List<int> GridTile = new List<int>();
        public List<int> SortedTile = new List<int>();
        public List<int> UnsortedTile = new List<int>();
        public Boolean EditingPrice = false;
        public int editing = 0;
        public int TyleTypeEditing = 0;
        public int PriceEditing = 0;
        public String EditingPriceName = "";

        public String LeftButtonTip = "";
        public String RightButtonTip = "";

        public Boolean exitPriceEditing = false;

        public Boolean BumpUpWeapon = false;
        public Boolean BumpDownWeapon = false;
        private KInput kInput = new KInput();

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            square = content.Load<Texture2D>("Sprites/Tiles/Wall");
            font = content.Load<SpriteFont>("standardfont");
            tileManager = new TileManager();
            tileManager.Load(content);
            this.graphics = graphics;

            //GrassTile = content.Load<Texture2D>("Sprites/GrassTile");
            //WallTile = content.Load<Texture2D>("Sprites/Tiles/Wall");
            //WoodTile = content.Load<Texture2D>("Sprites/WoodFloorTile");

            //DirtHole = content.Load<Texture2D>("Sprites/Zombies/SpawnHole");
            //RandomWeapon = content.Load<Texture2D>("Sprites/Tiles/RandomWeaponTile");
            //JukeBox = content.Load<Texture2D>("Sprites/Tiles/JukeBox");
            //MachineGun = content.Load<Texture2D>("Sprites/Tiles/WeaponTile");
            //GatlingGun = content.Load<Texture2D>("Sprites/Tiles/WeaponTile");
            //Door = content.Load<Texture2D>("Sprites/Tiles/Door");
            //Spawn = content.Load<Texture2D>("Sprites/Player/player_Circle");

            int x = 0;
            int y = 0;

            for (int i = 0; i < grid.Length; i++)
            {

                grid[i] = new Rectangle(x * width, y * height, width, height);
                grid2[i] = new Rectangle(x * width, y * height, width, height);

                if (x == 0 || x == 39 || y == 0 || y == 39)
                    TileType[i] = "Wall";
                else
                    TileType[i] = "Grass";
                x++;
                if (x >= 40)
                {
                    x = 0;
                    y++;
                }

            }
        }

        public void Update(GameTime gameTime, Vector2 CamCen, Tile selectedTile, Boolean MouseIntersectsGUI, Boolean dragWindow, int weaponTypeindex)
        {
            kInput.Update(true);
            SortedTile.Clear();
            SortedTile.AddRange(UnsortedTile);
                UpdateMouse(CamCen);
                if (!MouseIntersectsGUI && !dragWindow && !EditingPrice)
                {
                    UpdateMouseSelection(selectedTile);
                    UpdateToolTip(selectedTile);
                }
                else
                {
                    LeftButtonTip = "";
                    RightButtonTip = "";
                }
                if (EditingPrice)
                {
                    TileHover = -1;
                    TypePrice();
                    if (kInput.GUI_Down &&
                        (EditingPriceName != "Door" && EditingPriceName != "Random Weapon"
                        && EditingPriceName != "Locked Door" && EditingPriceName != "Item"
                        && EditingPriceName != "Glass"))
                    {
                        if (EditingPriceName == "Teleporter" && tileManager.teleporterTiles[weaponTypeindex].colorID == tileManager.Sender.colorID)
                        {
                            if(PriceEditing == 1)
                                PriceEditing = 2;
                        }
                        if ((EditingPriceName == "Teleporter" && tileManager.teleporterTiles[weaponTypeindex].colorID == tileManager.Sender.colorID)
                            || EditingPriceName != "Teleporter")
                            if(PriceEditing == 0)
                            PriceEditing = 1;
                    }
                    else if (kInput.GUI_Up)//Keyboard.GetState().IsKeyDown(Keys.Up))
                        if (PriceEditing > 0)
                            PriceEditing--;


                    int w = 500;
                    int h = 250;
                    Rectangle WarningWindow = new Rectangle(graphics.Viewport.Width / 2 - (w / 2), graphics.Viewport.Height / 2 - (h / 2), w, h);
                    Rectangle mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
                    if (kInput.LMB)
                    {
                        if (mouseRect.Intersects(new Rectangle(WarningWindow.X + WarningWindow.Width - 35, WarningWindow.Y + 5, 30, 30)))
                        {
                            PriceEditing = 0;
                            BumpUpWeapon = true;
                        }
                        if (mouseRect.Intersects(new Rectangle(WarningWindow.X + WarningWindow.Width - 70, WarningWindow.Y + 5, 30, 30)))
                        {
                            PriceEditing = 0;
                            BumpDownWeapon = true;
                        }
                    }
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        
                        if (mouseRect.Intersects(new Rectangle(WarningWindow.X, WarningWindow.Y + ((1) * 45), WarningWindow.Width, 45)))
                        {
                            PriceEditing = 0;
                        }
                        if (mouseRect.Intersects(new Rectangle(WarningWindow.X, WarningWindow.Y + ((2) * 45), WarningWindow.Width, 45)))
                        {
                            if ((EditingPriceName != "Door" && EditingPriceName != "Random Weapon"
                                && EditingPriceName != "Locked Door"  &&  EditingPriceName != "Item"
                                &&  EditingPriceName != "Glass"))
                            {
                                if ((EditingPriceName == "Teleporter" && tileManager.teleporterTiles[weaponTypeindex].colorID == tileManager.Sender.colorID)
                            || EditingPriceName != "Teleporter")
                                PriceEditing = 1;
                            }
                        }
                        if (mouseRect.Intersects(new Rectangle(WarningWindow.X, WarningWindow.Y + ((3) * 45), WarningWindow.Width, 45)))
                        {
                            if (EditingPriceName == "Teleporter" && tileManager.teleporterTiles[weaponTypeindex].colorID == tileManager.Sender.colorID)
                                PriceEditing = 2;
                        }
                        if (mouseRect.Intersects(new Rectangle(WarningWindow.X + WarningWindow.Width - 150, WarningWindow.Y + WarningWindow.Height - 50, 125, 40)))
                        {
                            int a;
                            int.TryParse(PriceNum, out a);
                            WeaponPrices[editing] = a;

                            int b;
                            int.TryParse(AmmoPrice, out b);
                            AmmoPrices[editing] = b;
                            EditingPrice = false;
                            TileHover = -1;
                            exitPriceEditing = true;

                            int c;
                            int.TryParse(ThirdPrice, out c);
                            ThirdPrices[editing] = c;
                        }
                    }
                   // System.Console.WriteLine(WeaponPrices[editing]);
                }
                //System.Console.WriteLine(selectedTile.name + "  " + selectedTile.colorID);
              //  if (TileHover >= 0)
                   // System.Console.WriteLine(TileType2[TileHover] + "  " + selectedTile.colorID);
        }

        private void UpdateToolTip(Tile SelectedTile)
        {
            if (Prev_TileHover != TileHover)
            {
                LeftButtonTip = "";
                RightButtonTip = "";
                Prev_TileHover = TileHover;
            }
            String hoverTile1;
            String hoverTile2;
            if (TileHover >= 0)
            {
                hoverTile1 = TileType[TileHover];
                hoverTile2 = TileType2[TileHover];
                //if (SelectedTile.name == "Wall" || SelectedTile.name == "Wood Floor" || SelectedTile.name == "Grass" || SelectedTile.name == "Marble"
                //    || SelectedTile.name == "Sand" || SelectedTile.name == "Pavement" || SelectedTile.name == "Dirt")//Basic Tiles
                if(IsBasicTile(SelectedTile))
                {
                    if (!SelectedTile.collision || (SelectedTile.collision && (TileType2[TileHover] == "" || TileType2[TileHover] == null)))
                    {
                       
                            LeftButtonTip = "Place " + SelectedTile.name;
                            if (TileType2[TileHover] == "" || TileType2[TileHover] == null)//#floodfillfix
                            {
                                if (RightButtonTip == "")
                                    RightButtonTip = "Fill With " + SelectedTile.name;
                            }
                    }
                }
                else if (!IsWall(TileType[TileHover]))// != "Wall")//Collision Tiles
                {
                    if (SelectedTile.name == "Eraser" && (hoverTile2 != "" && hoverTile2 != "Eraser" && hoverTile2 != null))
                    {
                       // System.Console.WriteLine(hoverTile2);
                        LeftButtonTip = "Erase " + hoverTile2;
                    }
                    else
                    {
                        if(SelectedTile.name != "Eraser")
                        LeftButtonTip = "Place " + SelectedTile.name;
                    }
                }

                if (hoverTile2 == "Door" || hoverTile2 == "Pistol" || hoverTile2 == "Rifle" ||
                           hoverTile2 == "Machine Gun" || hoverTile2 == "Gatling Gun" || hoverTile2 == "Shotgun" ||
                           hoverTile2 == "Auto-Shotty" || hoverTile2 == "Random Weapon" || hoverTile2 == "Locked Door"
                           || hoverTile2 == "Switch" || hoverTile2 == "Zapper" || hoverTile2 == "Weapon" || hoverTile2 == "Item"
                            || hoverTile2 == "Glass"  ||  hoverTile2 == "Teleporter")
                {
                    RightButtonTip = hoverTile2 + " Properties";
                }
            }
            
        }
        private Boolean IsBasicTile(Tile tile)
        {
            for (int i = 0; i < tileManager.BasicTile.Length; i++)
                if (tile.name == tileManager.BasicTile[i].name)
                    return true;
            for (int a = 0; a < tileManager.AssociatedTypes_Basic.Length; a++)
            {
                for (int b = 0; b < tileManager.AssociatedTypes_Basic[a].Count; b++)
                    if (tile.name  == tileManager.AssociatedTypes_Basic[a][b].name)
                    {
                        return true;
                    }
            }
            return false;
        }
        private Boolean IsWall(String name)
        {
            for (int i = 0; i < tileManager.WallTiles.Count; i++)
            {
                if (tileManager.WallTiles[i].name == name)
                    return true;
            }
            return false;
        }
        private void Fill(Tile tile, int tileHover, String _originalTile)
        {
            //String _originalTile = TileType[tileHover];
            //System.Console.WriteLine(tile.name + "  " + _originalTile);
            if (tile.name != _originalTile)
            {
                if (!IsWall(tile.name) || (IsWall(tile.name) && (TileType2[tileHover] == "" || TileType2[tileHover] == null)))
                {
                    TileType[tileHover] = tile.name;
                    if ((tileHover + 1 >= 0 && TileType.Length > tileHover + 1)&& ((float)tileHover - 39.0f) % 40 != 0)
                        if (TileType[tileHover + 1] == _originalTile)
                            Fill(tile, tileHover + 1, _originalTile);
                    if ((tileHover - 1 >= 0 && TileType.Length > tileHover - 1) && (float)tileHover % 40.0f != 0)
                        if (TileType[tileHover - 1] == _originalTile)
                            Fill(tile, tileHover - 1, _originalTile);

                    if (tileHover + 40 >= 0 && TileType.Length > tileHover + 40)
                        if (TileType[tileHover + 40] == _originalTile)
                            Fill(tile, tileHover + 40, _originalTile);
                    if (tileHover - 40 >= 0 && TileType.Length > tileHover - 40)
                        if (TileType[tileHover - 40] == _originalTile)
                            Fill(tile, tileHover - 40, _originalTile);
                }
            }
        }

        private MouseState oldState;
        private void UpdateMouseSelection(Tile SelectedTile)
        {
            MouseState newState = Mouse.GetState();
            if (oldState != newState)
            {
                if (newState.RightButton == ButtonState.Pressed  && oldState.RightButton == ButtonState.Released)
                {
                    if (TileHover >= 0)
                    {
                        if (IsBasicTile(SelectedTile))
                        {
                            //System.Console.WriteLine(TileType2[TileHover]);
                            if(TileType2[TileHover] == ""  ||  TileType2[TileHover] == null) //#floodfillfix
                            Fill(SelectedTile, TileHover, TileType[TileHover]);
                        }
                    }
                }

                oldState = newState;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (TileHover >= 0)
                {
                    //if (SelectedTile.name == "Wall" || SelectedTile.name == "Wood Floor" || SelectedTile.name == "Grass" || SelectedTile.name == "Marble"
                    //    || SelectedTile.name == "Sand" || SelectedTile.name == "Pavement" || SelectedTile.name == "Dirt")//Basic Tiles
                    if (IsBasicTile(SelectedTile))
                    {
                        if (!SelectedTile.collision || (SelectedTile.collision && (TileType2[TileHover] == "" || TileType2[TileHover] == null)))
                        TileType[TileHover] = SelectedTile.name;
                    }
                    else if (!IsWall(TileType[TileHover]))// != "Wall")//Collision Tiles
                    {
                        if(SelectedTile.name == "Eraser")
                        {
                            for (int i = 0; i < GridTile.Count; i++)
                            {
                                if (GridTile[i] == TileHover)
                                {
                                    WeaponPrices.RemoveAt(i);
                                    AmmoPrices.RemoveAt(i);
                                    ThirdPrices.RemoveAt(i);
                                    GridTile.RemoveAt(i);
                                   // SortedTile.RemoveAt(i);
                                    UnsortedTile.RemoveAt(i);
                                    SortedTile.Clear();
                                    SortedTile.AddRange(UnsortedTile);
                                }
                            }
                        }
                        TileType2[TileHover] = SelectedTile.name;
                        int p = 0;
                        int a = 0;
                        int t = 0;
                        if (SelectedTile.name == "Door") { p = 3000; a = 0; t = 0; }
                        if (SelectedTile.name == "Pistol") { p = 0; a = 0; t = 0; }
                        if (SelectedTile.name == "Rifle") { p = 700; a = 300; t = 0; }
                        if (SelectedTile.name == "Machine Gun") { p = 2500; a = 1000; t = 0; }
                        if (SelectedTile.name == "Gatling Gun") { p = 5750; a = 3500; t = 0; }
                        if (SelectedTile.name == "Shotgun") { p = 3000; a = 1500; t = 0; }
                        if (SelectedTile.name == "Auto-Shotty") { p = 3750; a = 2500; t = 0; }
                        if (SelectedTile.name == "Random Weapon") { p = 1250; a = 0; t = 0; }
                        if (SelectedTile.name == "Locked Door") { p = 0; a = 0; t = 0; }
                        if (SelectedTile.name == "Switch") { p = 0; a = 0; t = 0; }
                        if (SelectedTile.name == "Zapper") { p = 0; a = 30; t = 0; }
                        if (SelectedTile.name == "Weapon") { p = 0; a = 0; t = 0; }
                        if (SelectedTile.name == "Item") { p = 4500; a = 0; t = 0; }
                        if (SelectedTile.name == "Teleporter") { p = 0; a = 0; t = 30; }
                        if (SelectedTile.name == "Glass") { p = 100; a = 0; t = 0; }

                        if (SelectedTile.name == "Door" || SelectedTile.name == "Pistol" || SelectedTile.name == "Rifle" ||
                            SelectedTile.name == "Machine Gun" || SelectedTile.name == "Gatling Gun" || SelectedTile.name == "Shotgun" ||
                            SelectedTile.name == "Auto-Shotty" || SelectedTile.name == "Random Weapon"  ||  SelectedTile.name == "Locked Door"
                            ||  SelectedTile.name == "Switch"  ||  SelectedTile.name == "Zapper"  ||  SelectedTile.name == "Weapon"  ||  SelectedTile.name == "Item"
                            ||  SelectedTile.name == "Teleporter" ||  SelectedTile.name == "Glass")
                        {
                            for (int i = 0; i < GridTile.Count; i++)//#addedforloop
                            {
                                if (GridTile[i] == TileHover)
                                {
                                    WeaponPrices.RemoveAt(i);
                                    AmmoPrices.RemoveAt(i);
                                    ThirdPrices.RemoveAt(i);
                                    GridTile.RemoveAt(i);
                                    // SortedTile.RemoveAt(i);
                                    UnsortedTile.RemoveAt(i);
                                    SortedTile.Clear();
                                    SortedTile.AddRange(UnsortedTile);
                                }
                            }
                            if (!GridTile.Contains(TileHover))
                            {
                                WeaponPrices.Add(p);
                                AmmoPrices.Add(a);
                                ThirdPrices.Add(t);
                                GridTile.Add(TileHover);
                               // SortedTile.Add(TileHover);
                                UnsortedTile.Add(TileHover);
                                SortedTile.Clear();
                                SortedTile.AddRange(UnsortedTile);
                                if (SelectedTile.name == "Weapon")
                                {
                                    ColorFile2[TileHover] = tileManager.WeaponTile.colorID;
                                }
                                if (SelectedTile.name == "Item")
                                {
                                    ColorFile2[TileHover] = tileManager.ItemTile.colorID;
                                }
                                if (SelectedTile.name == "Teleporter")
                                {
                                    ColorFile2[TileHover] = tileManager.TeleporterTile.colorID;
                                }
                            }
                        }


                    }

               }
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (TyleTypeEditing >= 0)
                {
                    for(int i = 0; i < GridTile.Count; i++)
                    {
                        if (GridTile[i] == TileHover)
                        {
                            EditingPrice = true;
                            editing = i;
                            PriceNum = "" + WeaponPrices[i];
                            AmmoPrice = "" + AmmoPrices[i];
                            ThirdPrice = "" + ThirdPrices[i];
                            EditingPriceName = TileType2[TileHover];
                            PriceEditing = 0;
                            TyleTypeEditing = TileHover;
                        }
                    }
                }
            }
        }
        public String PriceNum = "";
        public String AmmoPrice = "";
        public String ThirdPrice = "";
        KeyboardState keyState;
        private KeyboardState oldKeyState = Keyboard.GetState();
        Keys[] keysToCheck = new Keys[] {   
        Keys.Back, Keys.Space, 
        Keys.D0, Keys.D1, Keys.D2, Keys.D3 , Keys.D4,
        Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
        Keys.Escape, Keys.Enter};

        private void TypePrice()
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
                                if (PriceEditing == 0)
                                    if (PriceNum.Length > 0)
                                        PriceNum = PriceNum.Remove(PriceNum.Length - 1, 1);
                                if(PriceEditing == 1)
                                    if (AmmoPrice.Length > 0)
                                        AmmoPrice = AmmoPrice.Remove(AmmoPrice.Length - 1, 1);
                                if (PriceEditing == 2)
                                    if (ThirdPrice.Length > 0)
                                        ThirdPrice = ThirdPrice.Remove(ThirdPrice.Length - 1, 1);
                            }
                            else if (key == Keys.Enter || key == Keys.Escape)
                            {
                                int a;
                                int.TryParse(PriceNum, out a);
                                WeaponPrices[editing] = a;

                                int b;
                                int.TryParse(AmmoPrice, out b);
                                AmmoPrices[editing] = b;
                                EditingPrice = false;

                                int c;
                                int.TryParse(ThirdPrice, out c);
                                ThirdPrices[editing] = c;

                            }
                            else
                            {
                                AddKeyToText(key);
                            }
                        }
                    }
                }
        }
        private void AddKeyToText(Keys key)
        {
            string newChar = "";
            switch (key)
            {
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

            if (PriceEditing == 0)
                if (PriceNum.Length < 6)
                    PriceNum += newChar;
            if (PriceEditing == 1)
                if (AmmoPrice.Length < 6)
                    AmmoPrice += newChar;
            if (PriceEditing == 2)
                if (ThirdPrice.Length < 6)
                    ThirdPrice += newChar;
        }


        private void UpdateMouse(Vector2 CamCen)
        {
            Rectangle mouseRect = new Rectangle((int)(CamCen.X + (Mouse.GetState().X - (graphics.Viewport.Width / 2)) * 2), (int)(CamCen.Y + (Mouse.GetState().Y - (graphics.Viewport.Height / 2)) * 2), 1, 1);
            for (int i = 0; i < grid.Length; i++)
            {
                if (mouseRect.Intersects(grid[i]))
                    TileHover = i;

                if (TileHover >= 0)
                    if (i >= grid.Length - 1 && !mouseRect.Intersects(grid[TileHover]))
                        TileHover = -1;
            }
         //   if(TileHover >= 0)
         //   System.Console.WriteLine(TileType2[TileHover]);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < grid.Length; i++)
            {
                Texture2D texture = null;
                Texture2D texture2 = null;

                for (int a = 0; a < tileManager.BasicTile.Length; a++)
                {
                    if (TileType[i] == tileManager.BasicTile[a].name)
                    {
                        texture = tileManager.BasicTile[a].texture;
                        ColorFile1[i] = tileManager.BasicTile[a].colorID;
                    }
                }
                for (int a = 0; a < tileManager.AssociatedTypes_Basic.Length; a++)
                {
                    for (int b = 0; b < tileManager.AssociatedTypes_Basic[a].Count; b++)
                        if (TileType[i] == tileManager.AssociatedTypes_Basic[a][b].name)
                        {
                            texture = tileManager.AssociatedTypes_Basic[a][b].texture;
                            ColorFile1[i] = tileManager.AssociatedTypes_Basic[a][b].colorID;
                        }
                }

                for(int a = 0; a < tileManager.PiecesTile.Length; a++)
                {
                    if (tileManager.PiecesTile[a].name != "Eraser")
                    {
                        if (TileType2[i] == tileManager.PiecesTile[a].name)
                        {

                            texture2 = tileManager.PiecesTile[a].texture;
                            if (TileType2[i] != "Weapon" && TileType2[i] != "Item"  &&  TileType2[i] != "Teleporter")
                            {
                                ColorFile2[i] = tileManager.PiecesTile[a].colorID;
                            }
                        }
                    }
                    else
                    {
                        if (TileType2[i] == tileManager.PiecesTile[a].name  ||  TileType2[i] == "")
                        {
                                TileType2[i] = "";
                                texture2 = null;
                                ColorFile2[i] = new Color(0, 0, 0, 0);
                        }
                    }
                }
                
                
                if (texture != null)
                {
                    if (i != TileHover)
                        spriteBatch.Draw(texture, grid[i], Color.White);
                    else
                        spriteBatch.Draw(texture, grid[i], Color.Blue);
                }
                if (texture2 != null)
                {
                    if (i != TileHover)
                        spriteBatch.Draw(texture2, grid2[i], Color.White);
                    else
                        spriteBatch.Draw(texture2, grid2[i], Color.Blue);
                }
            }


        }
        private void SetupWallLineing()
        {

        }


    }
}
