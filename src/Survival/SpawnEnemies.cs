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

namespace SurvivalShooter.Survival
{
    class SpawnEnemies
    {
        private int SpawnWaitTime = 11000;
        private int SpawnRate = 1500;
        private int CurSpawnTime = 11000;//275;

        public int Wave = 1;
        private int EnemiesPerWave = 10;//7;//50;//7;//1;
        private int EnemieIncrementWave = 7;
        private int SpawnCap = 40;

        public int SpawnedEnemies;

        private Texture2D missile;
        private List<Texture2D> EnemyTexture = new List<Texture2D>();
        private Texture2D IceBerg;
        private SoundEffect hit;
        private SpriteFont font;

        private Texture2D DirtHole;

        public Vector2 P_Spawn;

        public List<Vector2> OpenSpawns = new List<Vector2>();
        public List<Enemy> enemy = new List<Enemy>();
        public List<int> ColIndexToRemove = new List<int>();
        public Boolean newWave = false;

        List<SurvivalEnemySpawner> spawners = new List<SurvivalEnemySpawner>();


        public Boolean Mutate = false;
        public int SirenAlpha;
        int timer = 0;
        int MutantWaveTimer = 0;
        int MinMutantTime = 100000;
        int MaxMutantTime = 150000; //Time in  sec + 000
        public Boolean FinishedMutating = false;
        private int PrevMutantWave = 0;

        public int NewWaveFade = 0;

        public SpawnEnemies(Vector2 P_Spawn)
        {
            this.P_Spawn = P_Spawn;
        }

        public void Load(ContentManager content)
        {
            missile = content.Load<Texture2D>("Sprites/Missile1");
            hit = content.Load<SoundEffect>("hitsound");
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie"));
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie"));
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie"));
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie2"));
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie2"));
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie3"));
            EnemyTexture.Add(content.Load<Texture2D>("Sprites/Zombies/Zombie4"));
            font = content.Load<SpriteFont>("standardfont");

            DirtHole = content.Load<Texture2D>("Sprites/Zombies/SpawnHole");

            IceBerg = content.Load<Texture2D>("Sprites/IceBerg");

            Random random = new Random();
            MutantWaveTimer = random.Next(MinMutantTime, MaxMutantTime);
        }



        public void Update(GameTime gameTime,List<Player> players, List<PathfindNode> pathFindNode, Boolean purchasedDoor, List<Vector2> SpawnLocs, Boolean GameOver)
        {
           
            Random _Random = new Random();
            if (CurSpawnTime < 0 && SpawnedEnemies < EnemiesPerWave && enemy.Count < SpawnCap  &&  OpenSpawns.Count > 0)
            {
                Enemy tempEnemy = new Enemy();
                Random random = new Random();
                int SpawnLoc = random.Next(0, OpenSpawns.Count);
                tempEnemy.Load(missile, EnemyTexture[random.Next(0, EnemyTexture.Count)], IceBerg, hit, OpenSpawns[SpawnLoc], random.Next(0, players.Count), font, players, pathFindNode, SpawnLoc);
                if (50 * (Wave / 3) < 90)
                {
                 //   tempEnemy.Health = 50 * (Wave / 3 + 1);//control/force enemies health
                }
                if (tempEnemy.Speed / Wave >= 1)
                {
                    tempEnemy.Speed = tempEnemy.Speed / (tempEnemy.Speed / Wave) + 4;
                }
                else
                {
                    tempEnemy.Speed = tempEnemy.Speed + 4;
                }
                if (tempEnemy.Speed > 10)//7
                    tempEnemy.Speed = 10;//7
                if (Wave >= 8)
                    tempEnemy.Speed++;
                if (Wave >= 10)
                    tempEnemy.Speed++;
                if (Wave >= 14)
                    tempEnemy.Speed++;
                tempEnemy.Health = (tempEnemy.Health + (Wave * 3) + Wave / 2) / 3;
                //if (tempEnemy.Speed >= 10) tempEnemy.Health = tempEnemy.Health / 2;
                //tempEnemy.Health = 1;
                tempEnemy.NormalSpeed = tempEnemy.Speed;
                enemy.Add(tempEnemy);
                CurSpawnTime = SpawnRate;
                SpawnedEnemies++;
            }
            //for (int i = 0; i < enemy.Count; i++)
            //{
            //    if (enemy[i].RenderedUseless)
            //    {
            //        SpawnedEnemies -= 1;
            //        OpenSpawns.RemoveAt(enemy[i].Spawner);
            //        enemy.RemoveAt(i);
                    
            //    }
            //}
            if (enemy.Count == 0 && SpawnedEnemies >= EnemiesPerWave && !GameOver)
            {
                newWave = true;
                Wave++;
                SpawnedEnemies = 0;
                CurSpawnTime = SpawnWaitTime;
                EnemiesPerWave += EnemieIncrementWave;
                if (SpawnRate >= 400)//100)
                    SpawnRate -= 250;
                NewWaveFade = 7500;
            }
            if (NewWaveFade > 0)
                NewWaveFade -= gameTime.ElapsedGameTime.Milliseconds;
            else
                NewWaveFade = 0;


            CurSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;//--;

            //if (Wave <= 17)//20 * (17 / Wave) >= 20)
            //{
            //    SpawnRate = (20 * (17 / Wave)) / 63 * 1000;
            //}
            for (int i = 0; i < SpawnLocs.Count; i++)
            {
                if (spawners.Count != SpawnLocs.Count)
                {
                    SurvivalEnemySpawner tempSpawner = new SurvivalEnemySpawner();
                    tempSpawner.Load(SpawnLocs[i]);
                    spawners.Add(tempSpawner);
                }
            }

            for (int i = 0; i < spawners.Count; i++)
            {
                spawners[i].Update(gameTime, players, pathFindNode, P_Spawn, purchasedDoor);
                if (spawners[i].Active)
                {
                    if(!OpenSpawns.Contains(new Vector2(spawners[i].pos.X, spawners[i].pos.Y)))
                    {
                        OpenSpawns.Add(new Vector2(spawners[i].pos.X, spawners[i].pos.Y));
                    }
                }
            }
            if (Mutate)
            {
                SirenAlpha = FlashNumber(25, 100, 2, SirenAlpha);
                for (int i = 0; i < enemy.Count; i++)
                {
                    if (timer < 23500)
                    {
                        FinishedMutating = true;
                        enemy[i].Speed = 20;
                        enemy[i].NormalSpeed = enemy[i].Speed;
                        enemy[i].Health = 1;
                        enemy[i].EnemyTexture = EnemyTexture[_Random.Next(0, EnemyTexture.Count)];
                    }
                    else
                    {
                        FinishedMutating = false;
                        enemy[i].Speed = 0;
                        enemy[i].NormalSpeed = enemy[i].Speed;
                    }
                //    enemy[i].Mutate = true;
                }
            }
            else
            {
                if (SirenAlpha != 0)
                    SirenAlpha = CoolDownNumber(2, 0, SirenAlpha);

            }


            if (newWave)//if (timer <= 0)
            {
                Mutate = false;
                timer = 0;
            }
            else
            {
                timer -= gameTime.ElapsedGameTime.Milliseconds;
            }
            MutantWaveTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (MutantWaveTimer <= 0 && enemy.Count > 8 && SpawnedEnemies <= EnemiesPerWave / 2 && Wave - PrevMutantWave >= 3)//5
            {
                PrevMutantWave = Wave;
                FinishedMutating = false;
                Mutate = true;
                timer = 30000;
                Random _random = new Random();
                MutantWaveTimer = _random.Next(MinMutantTime, MaxMutantTime);
            }
        }
        

        public int FlashNumber(int MinNum, int MaxNum, int Speed, int self)
        {
            self += Speed;
            if (self >= -MinNum && self <= MinNum)
                self = MinNum;
            if (self >= MaxNum)
                self = -MaxNum + 1;
            return self;
        }
        public int CoolDownNumber(int speed, int min, int self)
        {
            if (self > min)
                self--;
            if (self < min)
                self++;
            if (self == min)
                self = min;
            return self;
        }
        public void Draw(SpriteBatch spriteBatch , Vector2 camCen)
        {
            for (int i = 0; i < enemy.Count; i++)
                if (Vector2.Distance(camCen, new Vector2(enemy[i].EnemyRect.X, enemy[i].EnemyRect.Y)) < 1550)
                    if (enemy[i].StunTime > 0)
                        enemy[i].DrawIce(spriteBatch);
           
            for (int i = 0; i < enemy.Count; i++)
            {
                if (Vector2.Distance(camCen, new Vector2(enemy[i].EnemyRect.X, enemy[i].EnemyRect.Y)) < 1550)
                {
                    if(enemy[i].StunTime <= 0)
                        enemy[i].Draw(spriteBatch);
                }
            }
            
                        
        }
        public void DrawSpawns(SpriteBatch spriteBatch, Vector2 camCen)
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                if (Vector2.Distance(camCen, spawners[i].pos) < 1550)
                {
                    spriteBatch.Draw(DirtHole, new Rectangle((int)spawners[i].pos.X - 75, (int)spawners[i].pos.Y - 75, 150, 150), Color.White);
                }
            }
        }
        public void DrawOverlay(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(enemyTexture,new Rectangle(0,0
        }
    }
}
