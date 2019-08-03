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

using SurvivalShooter.LevelEditorComponents;


namespace SurvivalShooter.StandardGame
{
    class TileManager
    {
        //public List<Tile> tiles = new List<Tile>();
        //public List<Color> CollisionID = new List<Color>();


        public Tile[] BasicTile;
        public Tile Wall;
        public Tile WoodFloor;
        public Tile Grass;
        public Tile Marble;
        public Tile Sand;
        public Tile Pavement;
        public Tile Dirt;


        public Tile[] PiecesTile;
        public Tile Door;
        public Tile DirtHole;
        public Tile Spawn;
        public Tile JukeBox;
        public Tile RandomBox;
        public Tile Store;
        public Tile LockedDoor;
        public Tile DoorSwitch;
        public Texture2D DoorSwitchOn;
        public Tile Zapper;
        public Tile Glass;

        public Tile WeaponTile;
        public Tile ItemTile;

       // public Tile[] WeaponTiles;
      //  public Color[] WeaponTilesColor;

        public WeaponTile[] weaponTiles;
        public WeaponTile[] itemTiles;

        public Tile TeleporterTile;
        public Tile Sender;
        public Tile Reciever;

        public WeaponTile[] teleporterTiles;
        public Texture2D Teleporter_SenderOn;
        public Texture2D Teleporter_ReceiverOn;



        public List<Tile>[] AssociatedTypes_Basic;
        public List<Tile> GrassTiles = new List<Tile>();
        public List<Tile> WallTiles = new List<Tile>();
        public List<Tile> WoodFloorTiles = new List<Tile>();
        public List<Tile> MarbleTiles = new List<Tile>();
        public List<Tile> SandTiles = new List<Tile>();
        public List<Tile> PavementTiles = new List<Tile>();
        public List<Tile> DirtTiles = new List<Tile>();


      //  public Tile Cement;
        public TileManager()
        {

        }
        public void Load(ContentManager content)
        {
            AssociatedTypes_Basic = new List<Tile>[7];
            BasicTile = new Tile[7];
            Grass = new Tile(content.Load<Texture2D>("Sprites/GrassTile"), new Color(34, 177, 76), "Grass", false); BasicTile[0] = Grass;
            Wall = new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/Wall"), new Color(127, 127, 127), "Wall", true); BasicTile[1] = Wall;
            WoodFloor = new Tile(content.Load<Texture2D>("Sprites/WoodFloorTile"), new Color(195, 195, 195), "Wood Floor", false); BasicTile[2] = WoodFloor;
            Marble = new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/Marble"), new Color(166, 166, 166), "Marble", false); BasicTile[3] = Marble;
            Sand = new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/Sand"), new Color(168, 168, 168), "Sand", false); BasicTile[4] = Sand;
            Pavement = new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/Pavement"), new Color(169, 169, 169), "Pavement", false); BasicTile[5] = Pavement;
            Dirt = new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/Dirt"), new Color(1, 1, 1), "Dirt", false); BasicTile[6] = Dirt;

            

            PiecesTile = new Tile[14];

            Spawn = new Tile(content.Load<Texture2D>("Sprites/Player/player_Circle"), new Color(0, 0, 0), "Spawn", false); PiecesTile[0] = Spawn;
            DirtHole = new Tile(content.Load<Texture2D>("Sprites/Zombies/SpawnHole"), new Color(0, 255, 0), "Dirt Hole", false); PiecesTile[1] = DirtHole;       
            Door = new Tile(content.Load<Texture2D>("Sprites/Tiles/Door"), new Color(255, 0, 0), "Door", true); PiecesTile[2] = Door;
            //JukeBox = new Tile(content.Load<Texture2D>("Sprites/Tiles/JukeBox"), new Color(203, 147, 50), "Jukebox", true); PiecesTile[3] = JukeBox;
            JukeBox = new Tile(content.Load<Texture2D>("Sprites/Tiles/MusicTile"), new Color(203, 147, 50), "Jukebox", true); PiecesTile[3] = JukeBox;

            WeaponTile = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(158, 110, 157), "Weapon", false); PiecesTile[4] = WeaponTile;
            RandomBox = new Tile(content.Load<Texture2D>("Sprites/Tiles/RandomWeaponTile"), new Color(0, 255, 255), "Random Weapon", false); PiecesTile[5] = RandomBox;
            
            Store = new Tile(content.Load<Texture2D>("Sprites/Tiles/Store"), new Color(159, 111, 158), "Store", false); PiecesTile[6] = Store;
            ItemTile = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(172, 124, 171), "Item", false); PiecesTile[7] = ItemTile;
            LockedDoor = new Tile(content.Load<Texture2D>("Sprites/Tiles/LockedDoor"), new Color(160, 112, 159), "Locked Door", true); PiecesTile[8] = LockedDoor;
            DoorSwitch = new Tile(content.Load<Texture2D>("Sprites/Tiles/DoorSwitchOff"), new Color(161, 113, 160), "Switch", false); PiecesTile[9] = DoorSwitch;
            DoorSwitchOn = content.Load<Texture2D>("Sprites/Tiles/DoorSwitchOn");
            Zapper = new Tile(content.Load<Texture2D>("Sprites/Misc/Zapper"), new Color(162, 114, 161), "Zapper", false); PiecesTile[10] = Zapper;
            Glass = new Tile(content.Load<Texture2D>("Sprites/Tiles/Glass"), new Color(175, 127, 174), "Glass", true); PiecesTile[11] = Glass;

            TeleporterTile = new Tile(content.Load<Texture2D>("Sprites/Tiles/Teleporter/Sender_Off"), new Color(180, 132, 179), "Teleporter", false); PiecesTile[12] = TeleporterTile;

            PiecesTile[13] = new Tile(content.Load<Texture2D>("Sprites/Misc/Eraser"), new Color(0, 0, 0, 0), "Eraser", false);


            GrassTiles.Add(Grass); GrassTiles.Add(new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/Grass_2"), new Color(176, 128, 175), "Swamp Grass", false));
            WoodFloorTiles.Add(WoodFloor); WoodFloorTiles.Add(new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/WoodFloor2"), new Color(177, 129, 176), "Dark Wood Floor", false));
            WallTiles.Add(Wall); WallTiles.Add(new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/StonePath"), new Color(178, 130, 177), "Stone Wall", true));

            PavementTiles.Add(Pavement); PavementTiles.Add(new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/StoneFloor"), new Color(179, 131, 178), "Sone Floor", false));
           // PavementTiles.Add(new Tile(content.Load<Texture2D>("Sprites/Tiles/Basic/StonePath_right"), new Color(180, 132, 179), "Sone Path Right", false));

            AssociatedTypes_Basic[0] = GrassTiles;
            AssociatedTypes_Basic[1] = WallTiles;
            AssociatedTypes_Basic[2] = WoodFloorTiles;
            AssociatedTypes_Basic[3] = MarbleTiles;
            AssociatedTypes_Basic[4] = SandTiles;
            AssociatedTypes_Basic[5] = PavementTiles;
            AssociatedTypes_Basic[6] = DirtTiles;
           // SonicBoom = new Tile(content.Load<Texture2D>("Sprites/square"), new Color(163, 115, 162), "Sonic Boom", false); PiecesTile[9] = SonicBoom;

            

            




            Tile MachineGun = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(255, 255, 255), "Machine Gun", false);
            Tile GatlingGun = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(154, 106, 153), "Gatling Gun", false);
            Tile Rifle = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(155, 107, 154), "Rifle", false);
            Tile ShotGun = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(156, 108, 155), "Shotgun", false);
            Tile AutoShotGun = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(157, 109, 156), "Auto-Shotty", false);
            Tile Pistol = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(158, 110, 157), "Pistol", false);
            Tile DualUZIs = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(163, 115, 162), "Dual UZIs", false);
            Tile RocketLauncher = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(164, 116, 163), "Rocket Launcher", false);
            Tile Tanker = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(165, 117, 164), "Tanker", false);

            Tile Mines = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(166, 118, 165), "Land Mines", false);
            Tile Flamethrower = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(167, 119, 166), "Flamethrower", false);
            Tile RayGun = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(168, 120, 167), "Ray Gun", false);
            Tile FreezeRay = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(169, 121, 168), "Freeze Ray", false);
            Tile Bow = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(170, 122, 169), "Bow and Arrow", false);
            Tile PotatoGun = new Tile(content.Load<Texture2D>("Sprites/Tiles/WeaponTile"), new Color(171, 123, 170), "Potato Gun", false);

            weaponTiles = new WeaponTile[14];
            weaponTiles[0] = new WeaponTile(Pistol, Pistol.colorID, 0, 0);
            weaponTiles[1] = new WeaponTile(Rifle, Rifle.colorID, 700, 300);
            weaponTiles[2] = new WeaponTile(MachineGun, MachineGun.colorID, 2500, 1000);
            weaponTiles[3] = new WeaponTile(ShotGun, ShotGun.colorID, 3000, 1500);
            weaponTiles[4] = new WeaponTile(AutoShotGun, AutoShotGun.colorID, 3750, 2500);
            weaponTiles[5] = new WeaponTile(GatlingGun, GatlingGun.colorID, 5750, 3500);
            weaponTiles[6] = new WeaponTile(DualUZIs, DualUZIs.colorID, 4000, 2000);
            weaponTiles[7] = new WeaponTile(RocketLauncher, RocketLauncher.colorID, 2500, 1500);
            weaponTiles[8] = new WeaponTile(Tanker, Tanker.colorID, 5000, 3500);

            weaponTiles[9] = new WeaponTile(Mines, Mines.colorID, 2000, 1250);
            weaponTiles[10] = new WeaponTile(Flamethrower, Flamethrower.colorID, 3000, 1500);
            weaponTiles[11] = new WeaponTile(RayGun, RayGun.colorID, 4500, 3000);
            weaponTiles[12] = new WeaponTile(FreezeRay, FreezeRay.colorID, 6000, 4000);
            //weaponTiles[13] = new WeaponTile(Bow, Bow.colorID, 1000, 750);
            weaponTiles[13] = new WeaponTile(PotatoGun, PotatoGun.colorID, 200, 100);



            Tile SonicBoom = new Tile(content.Load<Texture2D>("Sprites/WeaponPurchaseStations/Item_SonicBoom"), new Color(172, 124, 171), "Sonic Boom", false);
            Tile Slower = new Tile(content.Load<Texture2D>("Sprites/WeaponPurchaseStations/Item_Slower"), new Color(173, 125, 172), "Slower", false);
            Tile SecurityPlate = new Tile(content.Load<Texture2D>("Sprites/WeaponPurchaseStations/Item_SecurityPlate"), new Color(174, 126, 173), "Security Plate", false);
            itemTiles = new WeaponTile[3];
            itemTiles[0] = new WeaponTile(SonicBoom, SonicBoom.colorID, 4500, 0);
            itemTiles[1] = new WeaponTile(Slower, Slower.colorID, 2500, 0);
            itemTiles[2] = new WeaponTile(SecurityPlate, SecurityPlate.colorID, 1500, 0);


            Teleporter_SenderOn = content.Load<Texture2D>("Sprites/Tiles/Teleporter/Sender_On");
            Teleporter_ReceiverOn = content.Load<Texture2D>("Sprites/Tiles/Teleporter/Receiver_On");
            Sender = new Tile(content.Load<Texture2D>("Sprites/Tiles/Teleporter/Sender_Off"), new Color(180, 132, 179), "Sender", false);
            Reciever = new Tile(content.Load<Texture2D>("Sprites/Tiles/Teleporter/Receiver_Off"), new Color(181, 133, 180), "Receiver", false);
            teleporterTiles = new WeaponTile[2];
            teleporterTiles[0] = new WeaponTile(Sender, Sender.colorID, 0, 0);
            teleporterTiles[1] = new WeaponTile(Reciever, Reciever.colorID, 0, 0);
            //WeaponTiles = new Tile[6];

            // WeaponTiles[5] = MachineGun;
            // WeaponTiles[4] = GatlingGun;
            // WeaponTiles[3] = Rifle;
            // WeaponTiles[2] = ShotGun;
            // WeaponTiles[1] = AutoShotGun;
            // WeaponTiles[0] = Pistol;

            // WeaponTilesColor = new Color[6];
            // WeaponTilesColor[5] = MachineGun.colorID;
            // WeaponTilesColor[4] = GatlingGun.colorID;
            // WeaponTilesColor[3] = Rifle.colorID;
            // WeaponTilesColor[2] = ShotGun.colorID;
            // WeaponTilesColor[1] = AutoShotGun.colorID;
            // WeaponTilesColor[0] = Pistol.colorID;

        }
    }
}
