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
using SurvivalShooter.StandardGame;


namespace SurvivalShooter
{
    class LoadLevel
    {
        private List<Rectangle> LevelTile = new List<Rectangle>();
        private List<Texture2D> TileTexture = new List<Texture2D>();
        private List<Rectangle> WallLineing = new List<Rectangle>();
        private Texture2D wallline;
        private List<Color> TileColor = new List<Color>();
        private int TileHeight = 150;
        private int TileWidth = 150;

        private int LevelOffsetX = -200;
        private int LevelOffsetY = -200;



        //private Texture2D GrassTile;
        //private Texture2D WallTile;
        //private Texture2D WoodTile;
        //private Texture2D DoorTile;
        //private Texture2D WeaponPurchaseTile;
        //private Texture2D RandomWeaponPurchaseTile;

        public List<Vector2> PlayerSpawns = new List<Vector2>();

        public List<Rectangle> LevelCollisionObjects = new List<Rectangle>();
        public List<Rectangle> AllCollisionObjects = new List<Rectangle>();

        public List<int> DoorPrices = new List<int>();
        public List<int> DoorIndex = new List<int>();
        public List<int> DoorChannel = new List<int>();
        public List<int> DoorHealth = new List<int>();
        public List<Rectangle> LevelPiecesCollisionObject = new List<Rectangle>();
        public int InteractionDist = 125;

        public List<Vector2> EnemySpawnLoc = new List<Vector2>();

        public List<Rectangle> Tiles = new List<Rectangle>();
        public List<Rectangle> Passable = new List<Rectangle>();
        public List<PathfindNode> pathfindNode = new List<PathfindNode>();

        public List<Rectangle> WeaponRects = new List<Rectangle>();
        public List<String> WeaponType = new List<string>();
        public List<int> WeaponCost = new List<int>();
        public List<int> AmmoCost = new List<int>();
        public List<Texture2D> WeaponTileTexture = new List<Texture2D>();

       // private Texture2D _musicplayerTexture;
        public List<Rectangle> MusicPlayerRect = new List<Rectangle>();
        public int MusicPlayerNodeIndex;


        public List<Rectangle> StoreRect = new List<Rectangle>();

        public List<Rectangle> DoorSwitches = new List<Rectangle>();
        public List<int> SwitchChannel = new List<int>();
        public List<int> SwitchPrice = new List<int>();
        public List<int> PurchasedChannel = new List<int>();
        public List<int> PurchasedChannelTraps = new List<int>();

        public List<Rectangle> ZapperRect = new List<Rectangle>();
        public List<int> ZapperChannel = new List<int>();
        public List<int> ZapDuration = new List<int>();


        public List<Rectangle> ItemRect = new List<Rectangle>();
        public List<String> ItemName = new List<string>();
        public List<Texture2D> ItemTexture = new List<Texture2D>();
        public List<int> ItemPrice = new List<int>();

        public List<Teleporter> teleporter = new List<Teleporter>();

        public TileManager tileManager;

        SpriteFont font;
        //Use another file to load in nodes
        public LoadLevel()
        {
        }
        //Texture2D square;
        public void Load(ContentManager content)
        {
          //  level = content.Load<Texture2D>("Level");
          //  level_Pieces = content.Load<Texture2D>("Level_Pieces");
            tileManager = new TileManager();
            tileManager.Load(content);

            //GrassTile = content.Load<Texture2D>("Sprites/GrassTile");
            //WallTile = content.Load<Texture2D>("Sprites/Tiles/Wall");
            //WoodTile = content.Load<Texture2D>("Sprites/WoodFloorTile");
            //DoorTile = content.Load<Texture2D>("Sprites/Tiles/Door");
            //WeaponPurchaseTile = content.Load<Texture2D>("Sprites/Tiles/WeaponTile");
            //RandomWeaponPurchaseTile = content.Load<Texture2D>("Sprites/Tiles/RandomWeaponTile");
            //_musicplayerTexture = content.Load<Texture2D>("Sprites/Tiles/JukeBox");
            wallline = content.Load<Texture2D>("Sprites/Tiles/Basic/Wall");
            font = content.Load<SpriteFont>("standardfont");
            //square = content.Load<Texture2D>("Sprites/Square");
         //   Level_Load();
            

        }
        public Boolean loaded = false;
        public void Clear_level()
        {
            while (EnemySpawnLoc.Count > 0)
                EnemySpawnLoc.RemoveAt(0);
            while (LevelPiecesCollisionObject.Count > 0)
                LevelPiecesCollisionObject.RemoveAt(0);
            while (LevelCollisionObjects.Count > 0)
                LevelCollisionObjects.RemoveAt(0);
            while (DoorIndex.Count > 0)
                DoorIndex.RemoveAt(0);
            while (DoorPrices.Count > 0)
                DoorPrices.RemoveAt(0);
            while (Tiles.Count > 0)
                Tiles.RemoveAt(0);
            while (AllCollisionObjects.Count > 0)
                AllCollisionObjects.RemoveAt(0);
            while (LevelCollisionObjects.Count > 0)
                LevelCollisionObjects.RemoveAt(0);
            while(LevelTile.Count > 0)
                LevelTile.RemoveAt(0);
            while (TileTexture.Count > 0)
                TileTexture.RemoveAt(0);
            while (pathfindNode.Count > 0)
                pathfindNode.RemoveAt(0);
            while (WeaponRects.Count > 0)
                WeaponRects.RemoveAt(0);
            while (WeaponType.Count > 0)
                WeaponType.RemoveAt(0);
            while (WeaponCost.Count > 0)
                WeaponCost.RemoveAt(0);
            while (AmmoCost.Count > 0)
                AmmoCost.RemoveAt(0);
            while (WeaponTileTexture.Count > 0)
                WeaponTileTexture.RemoveAt(0);
            while (Passable.Count > 0)
                Passable.RemoveAt(0);
            while (PlayerSpawns.Count > 0)
                PlayerSpawns.RemoveAt(0);
            DoorChannel.Clear();
            DoorSwitches.Clear();
            DoorHealth.Clear();
            SwitchChannel.Clear();
            SwitchPrice.Clear();
            PurchasedChannel.Clear();
            PurchasedChannelTraps.Clear();
            ZapperRect.Clear();
            ZapperChannel.Clear();
            ZapDuration.Clear();

            ItemName.Clear();
            ItemRect.Clear();
            ItemTexture.Clear();
            ItemPrice.Clear();

            teleporter.Clear();

            StoreRect.Clear();

            MusicPlayerRect.Clear();

            WallLineing.Clear();
            loaded = false;
        }
        private Boolean IsBasicTile(Color colorID)
        {
            for (int a = 0; a < tileManager.BasicTile.Length; a++)
                if (colorID == tileManager.BasicTile[a].colorID)
                    return true;
            return false;
        }
        private Boolean IsWall(Color colorID)
        {
            for (int i = 0; i < tileManager.WallTiles.Count; i++)
                if (colorID == tileManager.WallTiles[i].colorID)
                    return true;
            return false;
        }
        public void Level_Load(Texture2D level, Texture2D level_Pieces, FileInfo prFile)
        {
            Color[] levelTile = new Color[level.Width * level.Height];
            level.GetData<Color>(levelTile);

            Color[] levelPieceTile = new Color[level_Pieces.Width * level_Pieces.Height];
            level_Pieces.GetData<Color>(levelPieceTile);

            FileStream fs = new FileStream(prFile.FullName, FileMode.Open);
            System.IO.StreamReader sreader = new System.IO.StreamReader(fs);

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {

                    LevelTile.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));

                    if(IsBasicTile(levelTile[y * level.Width + x]))
                    for (int a = 0; a < tileManager.BasicTile.Length; a++)
                    {
                        if (levelTile[y * level.Width + x] == tileManager.BasicTile[a].colorID)
                        {
                            TileTexture.Add(tileManager.BasicTile[a].texture);
                            break;
                        }
                        else if (a >= tileManager.BasicTile.Length - 1)
                        {
                            TileTexture.Add(null);
                            break;
                        }
                    }
                    else

                        for (int a = 0; a < tileManager.AssociatedTypes_Basic.Length; a++)
                        {
                            for (int b = 0; b < tileManager.AssociatedTypes_Basic[a].Count; b++)
                                if (levelTile[y * level.Width + x] == tileManager.AssociatedTypes_Basic[a][b].colorID)
                                {
                                    TileTexture.Add(tileManager.AssociatedTypes_Basic[a][b].texture);
                                    break;
                                }
                        }
                    if (IsWall(levelTile[y * level.Width + x]))// == tileManager.Wall.colorID)
                    {
                        LevelCollisionObjects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        AllCollisionObjects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));

                        PathfindNode temp = new PathfindNode(pathfindNode, pathfindNode.Count, new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight), false);
                        temp.walkable = false;
                        pathfindNode.Add(temp);
                        Tiles.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    }
                    else
                    {
                        
                            PathfindNode temp = new PathfindNode(pathfindNode, pathfindNode.Count, new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight), true);
                            if (levelPieceTile[y * level_Pieces.Width + x] == new Color(255, 0, 0)  ||  levelPieceTile[y * level_Pieces.Width + x] == tileManager.LockedDoor.colorID
                                || levelPieceTile[y * level_Pieces.Width + x] == tileManager.Glass.colorID)
                            {
                                temp.walkable = false;
                                DoorIndex.Add(pathfindNode.Count);
                            }
                            else
                            {
                               // if (levelPieceTile[y * level_Pieces.Width + x] != new Color(203, 147, 50))
                                    temp.walkable = true;
                              //  else
                              //  {
                                //    temp.walkable = false;
                                //    MusicPlayerNodeIndex = temp.Index;
                              //  }
                            }
                            pathfindNode.Add(temp);
                            Tiles.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                            Passable.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        
                    }
                    //}
                    TileColor.Add(levelTile[y * level.Width + x]);
                }
            }
            SetupWallLineing();

            

            for (int y = 0; y < level_Pieces.Height; y++)
            {
                for (int x = 0; x < level_Pieces.Width; x++)
                {
                    if (y > 5  &&  x > 10)
                    {
                        //testRect = new Rectangle(10, 10, 10, 10);//x *
                    }
                    if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Door.colorID)
                    {
                        int p;
                        int.TryParse(sreader.ReadLine(), out p); sreader.ReadLine(); sreader.ReadLine();
                        DoorPrices.Add(p);//3000);
                        LevelPiecesCollisionObject.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        DoorHealth.Add(0);
                        DoorChannel.Add(-1);
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.DirtHole.colorID)
                    {
                        EnemySpawnLoc.Add(new Vector2(x * TileWidth + TileWidth / 2 + LevelOffsetX, y * TileHeight + TileHeight / 2 + LevelOffsetY));
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Spawn.colorID)
                    {
                        PlayerSpawns.Add(new Vector2(x * TileWidth + TileWidth / 2 + LevelOffsetX, y * TileHeight + TileHeight / 2 + LevelOffsetY));
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.JukeBox.colorID)
                    {
                        MusicPlayerRect.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                       // LevelCollisionObjects.Add(new Rectangle(x * TileWidth + LevelOffsetX + (TileWidth / 2) - (TileWidth / 10), y * TileHeight + LevelOffsetY, TileWidth / 2 + (TileWidth / 10), TileHeight));
                       // AllCollisionObjects.Add(new Rectangle(x * TileWidth + LevelOffsetX + (TileWidth / 2) - (TileWidth / 10), y * TileHeight + LevelOffsetY, TileWidth / 2 + (TileWidth / 10), TileHeight));
                    }
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.MachineGun.colorID)
                    //{
                    //    int p; int.TryParse(sreader.ReadLine(), out p);
                    //    int a; int.TryParse(sreader.ReadLine(), out a);

                    //    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    WeaponType.Add("Machine Gun");
                    //    WeaponCost.Add(p);
                    //    AmmoCost.Add(a);
                    //    WeaponTileTexture.Add(tileManager.MachineGun.texture);
                    //}
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.GatlingGun.colorID)
                    //{
                    //    int p; int.TryParse(sreader.ReadLine(), out p);
                    //    int a; int.TryParse(sreader.ReadLine(), out a);

                    //    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    WeaponType.Add("Gatling Gun");
                    //    WeaponCost.Add(p);
                    //    AmmoCost.Add(a);
                    //    WeaponTileTexture.Add(tileManager.GatlingGun.texture);
                    //}
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Rifle.colorID)
                    //{
                    //    int p; int.TryParse(sreader.ReadLine(), out p);
                    //    int a; int.TryParse(sreader.ReadLine(), out a);

                    //    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    WeaponType.Add("Rifle");
                    //    WeaponCost.Add(p);
                    //    AmmoCost.Add(a);
                    //    WeaponTileTexture.Add(tileManager.Rifle.texture);
                    //}
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.ShotGun.colorID)
                    //{
                    //    int p; int.TryParse(sreader.ReadLine(), out p);
                    //    int a; int.TryParse(sreader.ReadLine(), out a);

                    //    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    WeaponType.Add("Shotgun");
                    //    WeaponCost.Add(p);
                    //    AmmoCost.Add(a);
                    //    WeaponTileTexture.Add(tileManager.ShotGun.texture);
                    //}
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.AutoShotGun.colorID)
                    //{
                    //    int p; int.TryParse(sreader.ReadLine(), out p);
                    //    int a; int.TryParse(sreader.ReadLine(), out a);

                    //    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    WeaponType.Add("Auto-Shotty");
                    //    WeaponCost.Add(p);
                    //    AmmoCost.Add(a);
                    //    WeaponTileTexture.Add(tileManager.AutoShotGun.texture);
                    //}
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Pistol.colorID)
                    //{
                    //    int p; int.TryParse(sreader.ReadLine(), out p);
                    //    int a; int.TryParse(sreader.ReadLine(), out a);

                    //    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    WeaponType.Add("Pistol");
                    //    WeaponCost.Add(p);
                    //    AmmoCost.Add(a);
                    //    WeaponTileTexture.Add(tileManager.AutoShotGun.texture);
                    //}
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.RandomBox.colorID)
                    {
                        int p; int.TryParse(sreader.ReadLine(), out p); sreader.ReadLine(); sreader.ReadLine();

                        WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        WeaponType.Add("Random Weapon");
                        WeaponCost.Add(p);
                        AmmoCost.Add(0);
                        WeaponTileTexture.Add(tileManager.RandomBox.texture);
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Store.colorID)
                    {
                        StoreRect.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.LockedDoor.colorID)
                    {
                        int p; int.TryParse(sreader.ReadLine(), out p); sreader.ReadLine(); sreader.ReadLine();
                        DoorPrices.Add(-1);
                        LevelPiecesCollisionObject.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        DoorHealth.Add(0);
                        DoorChannel.Add(p);
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Glass.colorID)
                    {
                        int p; int.TryParse(sreader.ReadLine(), out p); sreader.ReadLine(); sreader.ReadLine();
                        LevelPiecesCollisionObject.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        DoorHealth.Add(p);
                        DoorPrices.Add(-2);
                        DoorChannel.Add(-2);
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.DoorSwitch.colorID)
                    {
                        int p; int.TryParse(sreader.ReadLine(), out p);// sreader.ReadLine();
                        int a; int.TryParse(sreader.ReadLine(), out a);
                        sreader.ReadLine();
                        DoorSwitches.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                        SwitchChannel.Add(p);
                        SwitchPrice.Add(a);
                    }
                    else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Zapper.colorID)
                    {
                        int p; int.TryParse(sreader.ReadLine(), out p); ZapperChannel.Add(p);
                        int a; int.TryParse(sreader.ReadLine(), out a); ZapDuration.Add(a);
                        sreader.ReadLine();
                        ZapperRect.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    }
                    else
                    {
                        if (IsWeapon(levelPieceTile[y * level_Pieces.Width + x]))
                        {
                            for (int i = 0; i < tileManager.weaponTiles.Length; i++)
                            {
                                if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.weaponTiles[i].colorID)
                                {
                                    int p; int.TryParse(sreader.ReadLine(), out p);
                                    int a; int.TryParse(sreader.ReadLine(), out a);
                                    sreader.ReadLine();

                                    WeaponRects.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                                    WeaponType.Add(tileManager.weaponTiles[i]._WeaponTile.name);
                                    WeaponCost.Add(p);
                                    AmmoCost.Add(a);
                                    WeaponTileTexture.Add(tileManager.weaponTiles[i]._WeaponTile.texture);
                                }
                            }
                        }
                        if (IsItem(levelPieceTile[y * level_Pieces.Width + x]))
                        {
                            for (int i = 0; i < tileManager.itemTiles.Length; i++)
                            {
                                if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.itemTiles[i].colorID)
                                {
                                    int p; int.TryParse(sreader.ReadLine(), out p);
                                    sreader.ReadLine();
                                    sreader.ReadLine();

                                    ItemName.Add(tileManager.itemTiles[i]._WeaponTile.name);
                                    ItemRect.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                                    ItemPrice.Add(p);
                                    ItemTexture.Add(tileManager.itemTiles[i]._WeaponTile.texture);
                                }
                            }
                        }
                        if(IsTeleporter(levelPieceTile[y * level_Pieces.Width + x]))
                        {
                            //for (int i = 0; i < tileManager.teleporterTiles.Length; i++)
                            //{
                            int p; int.TryParse(sreader.ReadLine(), out p);
                            int a; int.TryParse(sreader.ReadLine(), out a);
                            int t; int.TryParse(sreader.ReadLine(), out t);

                                if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Sender.colorID)
                                {
                                    teleporter.Add(new Teleporter(tileManager.Sender.texture, true, p, a, new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight), t));
                                }
                                if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.Reciever.colorID)
                                {
                                    teleporter.Add(new Teleporter(tileManager.Reciever.texture, false, p, a, new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight), t));
                                }
                            //}
                        }
                    }
                    //else if (levelPieceTile[y * level_Pieces.Width + x] == tileManager.SonicBoom.colorID)
                    //{
                    //    ItemRect.Add(new Rectangle(x * TileWidth + LevelOffsetX, y * TileHeight + LevelOffsetY, TileWidth, TileHeight));
                    //    ItemName.Add("Sonic Boom");
                    //    ItemTexture.Add(tileManager.SonicBoom.texture);
                    //}
                }
            }

            sreader.Close();
            fs.Close();
                for (int i = 0; i < pathfindNode.Count; i++)
                {
                    pathfindNode[i].DetectAdjacentNodes(pathfindNode, i);
                }
                loaded = true;
        }
        private void SetupWallLineing()
        {
            int LineThickness = 10;
            for (int i = 0; i < LevelCollisionObjects.Count; i++)
            {
                Rectangle rect = LevelCollisionObjects[i];
                if (!LevelCollisionObjects.Contains(new Rectangle(rect.X - rect.Width, rect.Y, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y, LineThickness, rect.Height));
                if (!LevelCollisionObjects.Contains(new Rectangle(rect.X + rect.Width, rect.Y, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X + rect.Width - LineThickness, rect.Y, LineThickness, rect.Height));
                if (!LevelCollisionObjects.Contains(new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y + rect.Height - LineThickness, rect.Width, LineThickness));
                if (!LevelCollisionObjects.Contains(new Rectangle(rect.X, rect.Y - rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y, rect.Width, LineThickness));


                if (LevelCollisionObjects.Contains(new Rectangle(rect.X - rect.Width, rect.Y, rect.Width, rect.Height))  &&
                    LevelCollisionObjects.Contains(new Rectangle(rect.X, rect.Y - rect.Height, rect.Width, rect.Height))  &&
                    !LevelCollisionObjects.Contains(new Rectangle(rect.X - rect.Width, rect.Y - rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y, LineThickness, LineThickness));

                if (LevelCollisionObjects.Contains(new Rectangle(rect.X - rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    LevelCollisionObjects.Contains(new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height)) &&
                    !LevelCollisionObjects.Contains(new Rectangle(rect.X - rect.Width, rect.Y + rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X, rect.Y + rect.Height - LineThickness, LineThickness, LineThickness));

                if (LevelCollisionObjects.Contains(new Rectangle(rect.X + rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    LevelCollisionObjects.Contains(new Rectangle(rect.X, rect.Y - rect.Height, rect.Width, rect.Height)) &&
                    !LevelCollisionObjects.Contains(new Rectangle(rect.X + rect.Width, rect.Y - rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X + rect.Width - LineThickness, rect.Y, LineThickness, LineThickness));

                if (LevelCollisionObjects.Contains(new Rectangle(rect.X + rect.Width, rect.Y, rect.Width, rect.Height)) &&
                    LevelCollisionObjects.Contains(new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, rect.Height)) &&
                    !LevelCollisionObjects.Contains(new Rectangle(rect.X + rect.Width, rect.Y + rect.Height, rect.Width, rect.Height)))
                    WallLineing.Add(new Rectangle(rect.X + rect.Width - LineThickness, rect.Y + rect.Height - LineThickness, LineThickness, LineThickness));
            }
        }
        private Boolean IsWeapon(Color ColorID)
        {
            for (int i = 0; i < tileManager.weaponTiles.Length; i++)
            {
                if (ColorID == tileManager.weaponTiles[i].colorID)
                {
                    return true;
                }
            }
            return false;
        }
        private Boolean IsItem(Color ColorID)
        {
            for (int i = 0; i < tileManager.itemTiles.Length; i++)
            {
                if (ColorID == tileManager.itemTiles[i].colorID)
                {
                    return true;
                }
            }
            return false;
        }
        private Boolean IsTeleporter(Color ColorID)
        {
            for (int i = 0; i < tileManager.teleporterTiles.Length; i++)
            {
                if (ColorID == tileManager.teleporterTiles[i].colorID)
                {
                    return true;
                }
            }
            return false;
        }
        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch , Vector2 playerPos)
        {
            for (int i = 0; i < LevelTile.Count; i++)
            {
                if (Vector2.Distance(playerPos, new Vector2(LevelTile[i].X + LevelTile[i].Width / 2, LevelTile[i].Y + LevelTile[i].Height / 2)) < 1550)
                {
                    if (TileTexture[i] != null && !LevelCollisionObjects.Contains(LevelTile[i]))
                    spriteBatch.Draw(TileTexture[i], LevelTile[i], new Color(100, 100, 100));
                }
            }
            //for (int i = 0; i < LevelPiecesCollisionObject.Count; i++)
            //{
            //    if (Vector2.Distance(playerPos, new Vector2(LevelPiecesCollisionObject[i].X + LevelPiecesCollisionObject[i].Width / 2,
            //      LevelPiecesCollisionObject[i].Y + LevelPiecesCollisionObject[i].Height / 2)) < 1550)
            //    {
            //        spriteBatch.Draw(tileManager.Door.texture, LevelPiecesCollisionObject[i], new Color(150,150,150));
            //    }
            //}
            //spriteBatch.Draw(tileManager.JukeBox.texture, MusicPlayer, Color.White);

           // spriteBatch.Draw(square, new Rectangle(400, 150, 250, 250), new Color(20, 20, 20, 20));
            //for (int i = 0; i < Passable.Count; i++)
            //{
            //    spriteBatch.Draw(GrassTile, Passable[i], Color.Red);

            //}
            //for (int i = 0; i < pathfindNode.Count; i++)
            //{
            //    if (pathfindNode[i].walkable)
            //    {
            //        spriteBatch.Draw(GrassTile, pathfindNode[i].atributes, Color.Red);
            //    }

            //}
            
        }

        public void DrawColObjects(SpriteBatch spriteBatch, Vector2 playerPos)
        {
            for (int i = 0; i < LevelTile.Count; i++)
            {
                if (Vector2.Distance(playerPos, new Vector2(LevelTile[i].X + LevelTile[i].Width / 2, LevelTile[i].Y + LevelTile[i].Height / 2)) < 1550)
                {
                    if (TileTexture[i] != null && LevelCollisionObjects.Contains(LevelTile[i]))
                        spriteBatch.Draw(TileTexture[i], LevelTile[i], new Color(100, 100, 100));
                    
                }
            }
            for (int i = 0; i < LevelPiecesCollisionObject.Count; i++)
            {
                if (Vector2.Distance(playerPos, new Vector2(LevelPiecesCollisionObject[i].X + LevelPiecesCollisionObject[i].Width / 2,
                  LevelPiecesCollisionObject[i].Y + LevelPiecesCollisionObject[i].Height / 2)) < 1550)
                {
                    if (DoorChannel[i] == -1)
                        spriteBatch.Draw(tileManager.Door.texture, LevelPiecesCollisionObject[i], new Color(150, 150, 150));
                    else if (DoorChannel[i] == -2)
                        spriteBatch.Draw(tileManager.Glass.texture, LevelPiecesCollisionObject[i], new Color(150, 150, 150));
                    else
                        spriteBatch.Draw(tileManager.LockedDoor.texture, LevelPiecesCollisionObject[i], new Color(150, 150, 150));
                }
            }
            //for (int i = 0; i < MusicPlayerRect.Count; i++)
            //    spriteBatch.Draw(tileManager.JukeBox.texture, MusicPlayerRect[i], Color.White);
            for (int i = 0; i < WallLineing.Count; i++)
                if (Vector2.Distance(playerPos, new Vector2(WallLineing[i].X + WallLineing[i].Width / 2, WallLineing[i].Y + WallLineing[i].Height / 2)) < 1550)
                    spriteBatch.Draw(wallline, WallLineing[i], Color.Black);
        }


    }
}
