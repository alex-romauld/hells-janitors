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

namespace SurvivalShooter
{
    class Enemy
    {
        public Texture2D EnemyTexture;

        public Vector2 pos;
        float rotation;

        private List<Vector2> BulletList = new List<Vector2>();
        private Texture2D BulletTexture;

        public int HitBullet = -1;
        public int Health;

     //   public Boolean Mutate = false;
     //   public int Prev_Health;
     //   public int Prev_Speed;



        public SoundEffect hit;
        public Boolean hitTick = false;

        //private int BulletDamage;
        public int TargetPlayer = 0;

        public int Speed = 5;
        public int NormalSpeed = 5;

        public Rectangle EnemyRect;
        private Rectangle Bottom_Rect;
        private Rectangle Top_Rect;
        private Rectangle Left_Rect;
        private Rectangle Right_Rect;

        public Rectangle CollisionRect;


        public Boolean CollisionSafe = false;
        public Boolean AddedToCollision = false;
        public int CollisionIndex = 0;

        private int P_WithlowestPlayerDist;

        public int Drop = 0;

        public int StunTime = -1;
        private int StunedHealthTick = 0;
        public Boolean StunHurt = false;
        public int PlayerStunnedBy = 0;
        private Texture2D IceBerg;

        List<Player> players = new List<Player>();
        public int Spawner;
        public Boolean RenderedUseless = false;
        //Pathfind doesn't update target tile when player dies.
        public Enemy()
        {

        }
        SpriteFont font;
        int ActualTarget;
        private List<PathfindNode> pathFindNode = new List<PathfindNode>();
        private Boolean[] OnOpenList;
        private Boolean[] OnClosedList;
        private Boolean[] OnPathList;
        Rectangle Cen;
        public void Load(Texture2D BulletTexture, Texture2D EnemyTexture, Texture2D IceBerg, SoundEffect hit, Vector2 pos, int TargetPlayer, SpriteFont font, List<Player> players, List<PathfindNode> _pathFindNode, int Spawner)
        {
            this.players = players;
            this.Spawner = Spawner;
            this.pathFindNode = _pathFindNode;
            OnOpenList = new bool[_pathFindNode.Count];
            OnClosedList = new bool[_pathFindNode.Count];
            OnPathList = new bool[_pathFindNode.Count];
            this.BulletTexture = BulletTexture;
            this.EnemyTexture = EnemyTexture;
            this.hit = hit;
            this.font = font;
            this.pos = pos;

            Random random = new Random();
            Health = random.Next(90, 140);
            Drop = random.Next(1, 250);

            this.TargetPlayer = TargetPlayer;
            ActualTarget = TargetPlayer;
           // System.Console.WriteLine(EnemyTexture.Width);
            EnemyRect = new Rectangle((int)(pos.X - EnemyTexture.Width / 2), (int)pos.Y - EnemyTexture.Width / 2, EnemyTexture.Width, EnemyTexture.Height);

            Speed = random.Next(6,8);//15, 25);//4, 8);
            NormalSpeed = Speed;
            GetClosestPlayer(players);

            this.IceBerg = IceBerg;            
        }






           //Values For Enemy
            int My_Tile;
           //Values For Pathfinding
            private List<PathfindNode> openList = new List<PathfindNode>();
            private List<PathfindNode> closedList = new List<PathfindNode>();
            private PathfindNode checkingNode;
            private PathfindNode startingNode;
            private PathfindNode targetNode;
            private Boolean foundTarget = false;
            private int baseMovementCost = 10;


            //private List<PathfindNode> pathfindNode = new List<PathfindNode>();

            int Prev_PlayerCen;

            private void CalculateHeuritics()
            {
                for (int i = 0; i < pathFindNode.Count; i++)
                {
                    pathFindNode[i].h_heuaristicValue = ((Math.Abs(pathFindNode[i].atributes.X - targetNode.atributes.X) + Math.Abs(pathFindNode[i].atributes.Y - targetNode.atributes.Y)) / targetNode.atributes.Width) * 10;
                    pathFindNode[i].CalculateFValue();
                }
            }

            private Boolean OptimizedPathFinding = false;
            private int DoPathChecksTick = 0;
            private void FindPath()
            {
                if (foundTarget == false)
                {
                    if (checkingNode.north != null  &&  ((DoPathChecksTick == 0 &&  OptimizedPathFinding) ||  !OptimizedPathFinding))
                        DetermineNodeValues(checkingNode, checkingNode.north);
                    if (checkingNode.south != null  &&  ((DoPathChecksTick == 1 &&  OptimizedPathFinding) ||  !OptimizedPathFinding))
                        DetermineNodeValues(checkingNode, checkingNode.south);
                    if (checkingNode.east != null  &&  ((DoPathChecksTick == 2 &&  OptimizedPathFinding) ||  !OptimizedPathFinding))
                        DetermineNodeValues(checkingNode, checkingNode.east);
                    if (checkingNode.west != null  &&  ((DoPathChecksTick == 3 &&  OptimizedPathFinding) ||  !OptimizedPathFinding))
                        DetermineNodeValues(checkingNode, checkingNode.west);

                    if (foundTarget == false)
                    {
                        if (checkingNode.walkable)
                        {
                            OnClosedList[checkingNode.Index] = true;
                            closedList.Add(checkingNode);
                            OnOpenList[checkingNode.Index] = false;
                            openList.Remove(checkingNode);
                        }
                        else
                        {
                            return;
                        }

                        int SmallestFNode = 0;
                        for (int i = 0; i < openList.Count; i++)
                        {
                            if (i < openList.Count)
                                checkingNode = null;
                            if (openList[i].f_totalCost < openList[SmallestFNode].f_totalCost)
                            {
                                SmallestFNode = i;
                            }
                            if (i >= openList.Count - 1)
                            {
                                checkingNode = openList[SmallestFNode];
                            }
                        }
                        //checkingNode = getsmallestFvalue
                    }
                }
                    
            }
            private List<PathfindNode> path = new List<PathfindNode>();

            private void DetermineNodeValues(PathfindNode currentNode, PathfindNode testing)
            {
                if (testing == null)
                    return;

                if (testing == targetNode)
                {
                    targetNode.parentNode = currentNode;
                    foundTarget = true;
                    return;
                }
                for (int i = 0; i < players.Count; i++)//This was added #newPathFind
                {
                    if (players[i].Cur_Node == testing.Index  &&  !players[i].dead)
                    {
                        targetNode.parentNode = currentNode;
                        foundTarget = true;
                        return;
                    }
                }
                if (!testing.walkable)
                    return;

                if (!closedList.Contains(testing))// OnClosedList[testing.Index])//!testing.OnCloseList)
                {
                    if (openList.Contains(testing))//OnOpenList[testing.Index])// openList.Contains(testing))
                    {
                        int newGCost = currentNode.g_movementCost + baseMovementCost;

                        if (newGCost < testing.g_movementCost)
                        {
                            testing.parentNode = currentNode;
                            testing.g_movementCost = newGCost;
                            testing.CalculateFValue();
                        }
                    }
                    else
                    {
                        if (testing.walkable)
                        {
                            testing.parentNode = currentNode;
                            testing.g_movementCost = currentNode.g_movementCost + baseMovementCost;
                            testing.CalculateFValue();
                            OnOpenList[testing.Index] = true;
                            openList.Add(testing);
                        }
                        else
                            return;
                    }
                }
                DoPathChecksTick++;
                if (DoPathChecksTick == 4)
                    DoPathChecksTick = 0;
            }
            private void TraceBackPath(Player _targetPlayer)
            {
                PathfindNode node = targetNode;
                do
                {
                    if (!path.Contains(node))//OnPathList[node.Index])//path.Contains(node))//!pathFindNode[node.Index].OnPathList)
                    {
                        if (node.walkable)
                        {
                            OnPathList[node.Index] = true;
                            path.Add(node);
                            node = node.parentNode;
                        }
                        else
                            ResetPath(pathFindNode, Cen, _targetPlayer);
                    }
                    else
                    {
                        break;
                    }

                } while (node != null);
                if (node == null)
                {
                    clearLists();
                }
            }

            private void clearLists()
            {
                for (int i = 0; i < closedList.Count; i++)
                {
                    while (closedList.Count > 0)
                    {
                        OnClosedList[closedList[i].Index] = false;
                        closedList.RemoveAt(i);
                    }
                }
                for (int i = 0; i < openList.Count; i++)
                {
                    while (openList.Count > 0)
                    {
                        OnOpenList[openList[i].Index] = false;
                        openList.RemoveAt(i);
                    }
                }
            }
            private Boolean once = false;
            private Boolean doOnce = false;
            private int PathTimeOutTime = 0;
            private int PathTimeOut = 35000;//35000
            private Vector2 PrevClockedPos;
            private int CheckTimerTime = 0;
            private int CheckTimer = 2000;

            

            

            Vector2 E_Direction;

            Boolean attackPlayer = false;
            int CheckSpeed = 75;
            Vector2 CheckPos;
            private Rectangle CheckRect;

            Boolean getNewTargerNode = false;
            int CheckTarSpeed = 75;
            Vector2 CheckTarPos;
            private Rectangle CheckTarRect;



            private void PathFind(GameTime gameTime, List<PathfindNode> pathFindNode, Player _targetPlayer)
            {
                for (int i = 0; i < pathFindNode.Count; i++)
                {
                    if (foundTarget  ||  attackPlayer)
                    {
                        for (int a = 0; a < openList.Count; a++)
                        {
                            OnOpenList[openList[a].Index] = false;
                            openList.RemoveAt(a);
                        }
                        for (int a = 0; a < closedList.Count; a++)
                        {
                            OnClosedList[closedList[a].Index] = false;
                            closedList.RemoveAt(a);
                        }
                    }
                    if (attackPlayer)
                    {
                        for (int a = 0; a < path.Count; a++)
                        {
                            OnPathList[path[a].Index] = false;
                            path.RemoveAt(a);
                        }
                    }
                }

               
                Cen = new Rectangle(EnemyRect.X + EnemyRect.Width / 2, EnemyRect.Y + EnemyRect.Height / 2, 1, 1);
                if (!attackPlayer && foundTarget && path.Count == 0)
                {
                    foundTarget = false;
                    ResetPath(pathFindNode, Cen, _targetPlayer);
                }
                PathTimeOutTime += (int)gameTime.ElapsedGameTime.Milliseconds;
                CheckTimerTime += (int)gameTime.ElapsedGameTime.Milliseconds;
                if (PathTimeOutTime > PathTimeOut && !foundTarget)
                {
                    ResetPath(pathFindNode, Cen, _targetPlayer);
                    RenderedUseless = true;

                }
                if (CheckTimerTime > CheckTimer)
                {
                    if (foundTarget)
                    {
                        if (Vector2.Distance(PrevClockedPos, pos) < 100)
                        {
                            ResetPath(pathFindNode, Cen, _targetPlayer);
                            foundTarget = false;
                        }
                    }
                    PrevClockedPos = pos;
                    CheckTimerTime = 0;
                }
                if (!once)
                {
                  //  this.pathfindNode = pathFindNode;
                    for (int i = 0; i < pathFindNode.Count; i++)
                    {
                        if (Cen.Intersects(pathFindNode[i].atributes))
                        {
                            My_Tile = i;
                            startingNode = pathFindNode[i];
                        }
                    }
                    targetNode = pathFindNode[_targetPlayer.Cur_Node];
                    startingNode = pathFindNode[My_Tile];
                    CheckPos = pos;
                    checkingNode = startingNode;
                    CalculateHeuritics();
                    PrevClockedPos = pos;

                    once = true;
                }
                if (!foundTarget)//
                {//
                    for (int i = 0; i < pathFindNode.Count; i++)//
                    {//
                        if (Cen.Intersects(pathFindNode[i].atributes))//
                        {
                            My_Tile = i;//
                            startingNode = pathFindNode[i];//
                        }//
                    }//
                }//
                if (!foundTarget && !attackPlayer && !_targetPlayer.dead)
                {
                    FindPath();
                }
                if (foundTarget)
                {
                    if (path.Count > 0)
                    {
                        if (path[0] == targetNode)
                        {
                            if (!doOnce)
                            {
                                path.Reverse();
                                for (int i = 0; i < path.Count; i++)
                                {
                                    if (path[i] == targetNode)
                                    {
                                        OnPathList[path[i].Index] = false;
                                        path.RemoveAt(i);
                                    }
                                }

                                EmptyCloseOpenLists();
                                doOnce = true;
                            }
                        }
                    }
                    TraceBackPath(_targetPlayer);
                }
                int TargetNode = _targetPlayer.Cur_Node;
                for (int i = 0; i < pathFindNode.Count; i++)
                {
                    if (Cen.Intersects(pathFindNode[i].atributes))
                    {
                        My_Tile = i;
                        startingNode = pathFindNode[i];
                    }
                }

                if (Prev_PlayerCen != _targetPlayer.Cur_Node && getNewTargerNode && path.Count == 0)
                {
                    ResetPath(pathFindNode, Cen, _targetPlayer);
                    for (int i = 0; i < pathFindNode.Count; i++)
                    {
                        if (Cen.Intersects(pathFindNode[i].atributes))
                        {
                            My_Tile = i;
                            startingNode = pathFindNode[i];
                        }
                    }
                    Prev_PlayerCen = _targetPlayer.Cur_Node;
                    targetNode = pathFindNode[_targetPlayer.Cur_Node];
                }
            }
            //private Boolean GoodPath()
            //{
            //    for(int i = 0; i
            //    return true;
            //}
            private Boolean SafeFromViews()
            {
                for(int i = 0; i < players.Count; i++)
                {
                    if (Vector2.Distance(players[i].pos, pos) < 1550)
                        return false;
                }
                return true;
            }
            private void MoveToTarget(Player _targetPlayer)
            {
                Rectangle Cen = new Rectangle(EnemyRect.X + EnemyRect.Width / 2, EnemyRect.Y + EnemyRect.Height / 2, 1, 1);
                if (!_targetPlayer.dead)
                {
                    if (!attackPlayer && /*foundTarget && path.Count > 10 && */SafeFromViews())
                    {
                        if (Speed == NormalSpeed)
                        {
                            Speed = 21;
                        }
                    }
                    else
                    {
                        if(Speed == 21)
                            Speed = NormalSpeed;
                    }
                    if (Vector2.Distance(_targetPlayer.pos, pos) > 150 && !attackPlayer)
                    {
                        if (path.Count > 0 && foundTarget)
                        {
                            if (path.Count > 1)
                            {
                                if (path[0] != startingNode)
                                {

                                    Vector2 pathNodePos = new Vector2(path[0].atributes.X + path[0].atributes.Width / 2, path[0].atributes.Y + path[0].atributes.Height / 2);
                                    E_Direction = pathNodePos - pos;
                                    E_Direction.Normalize();
                                    rotation = (float)Math.Atan2((double)E_Direction.Y, (double)E_Direction.X);
                                    EnemyRect.X += (int)(E_Direction.X * Speed);
                                    EnemyRect.Y += (int)(E_Direction.Y * Speed);
                                    pos = new Vector2(EnemyRect.X + EnemyTexture.Width / 2, EnemyRect.Y + EnemyTexture.Height / 2);
                                    if (Vector2.Distance(pathNodePos, pos) < 25)//80 50
                                    {
                                        OnPathList[path[0].Index] = false;
                                        path.RemoveAt(0);
                                    }
                                    //     ResetPath(pathfindNode, Cen, players[TargetPlayer], false);
                                }
                                else
                                {
                                    OnPathList[path[0].Index] = false;
                                    path.RemoveAt(0);
                                    // if(doOnce)
                                    //  ResetPath(pathfindNode, Cen, players[TargetPlayer]);
                                }
                            }
                            else
                            {
                                ResetPath(pathFindNode, Cen, _targetPlayer);
                            }
                        }
                    }
                    else
                    {

                        E_Direction = PlayerPos - pos;
                        E_Direction.Normalize();
                        rotation = (float)Math.Atan2((double)E_Direction.Y, (double)E_Direction.X);
                        EnemyRect.X += (int)(E_Direction.X * Speed);
                        EnemyRect.Y += (int)(E_Direction.Y * Speed);
                        pos = new Vector2(EnemyRect.X + EnemyTexture.Width / 2, EnemyRect.Y + EnemyTexture.Height / 2);
                        if (attackPlayer)
                        {
                            ResetPath(pathFindNode, Cen, _targetPlayer);
                        }
                    }
                }
            }
            private void PerformPathChecks(Player _targetPlayer, List<PathfindNode> pathFindNode)
            {
                Rectangle Cen = new Rectangle(EnemyRect.X + EnemyRect.Width / 2, EnemyRect.Y + EnemyRect.Height / 2, 1, 1);
                Vector2 Check_Direction = PlayerPos - pos;
                Check_Direction.Normalize();
                //CheckRot = (float)Math.Atan2((double)Check_Direction.Y, (double)Check_Direction.X);
                CheckPos.X += (int)(Check_Direction.X * CheckSpeed);
                CheckPos.Y += (int)(Check_Direction.Y * CheckSpeed);
                CheckRect.X = (int)CheckPos.X - (CheckRect.Width / 2);
                CheckRect.Y = (int)CheckPos.Y - (CheckRect.Height / 2);
                for (int i = 0; i < path.Count; i++)
                {
                    if (!path[i].walkable)
                    {
                        ResetPath(pathFindNode, Cen, _targetPlayer);
                        foundTarget = false;
                    }
                }

                if (/*CheckRect.Intersects(targetPlayer.PlayerRect))// */Vector2.Distance(CheckPos, _targetPlayer.pos) < EnemyTexture.Width * 2 + _targetPlayer.playerTexture.Width)
                {
                    CheckPos = pos;
                    Check_Direction = PlayerPos - pos;
                    ResetPath(pathFindNode, Cen, _targetPlayer);
                    attackPlayer = true;
                }
                else
                {


                    for (int i = 0; i < pathFindNode.Count; i++)
                    {
                        if (Vector2.Distance(CheckPos, new Vector2(pathFindNode[i].atributes.X + pathFindNode[i].atributes.Width / 2,
                               pathFindNode[i].atributes.Y + pathFindNode[i].atributes.Height / 2)) < EnemyTexture.Width / 2 + pathFindNode[i].atributes.Width / 2)
                        {
                            if (!pathFindNode[i].walkable)
                            {
                                //  if (CheckRect.Intersects(pathfindNode[i].atributes))
                                //  {
                                CheckPos = pos;
                                Check_Direction = PlayerPos - pos;
                                attackPlayer = false;
                                // }
                            }
                        }

                    }
                }
                if (Vector2.Distance(CheckPos, pos) > Vector2.Distance(_targetPlayer.pos, pos) + 100)
                {
                    CheckPos = pos;
                    Check_Direction = PlayerPos - pos;
                }


                if (!attackPlayer)
                {
                    Vector2 CheckTar_Direction = PlayerPos - new Vector2(targetNode.atributes.X + targetNode.atributes.Width / 2,
                        targetNode.atributes.Y + targetNode.atributes.Height / 2);
                    CheckTar_Direction.Normalize();
                    //CheckTarRot = 0;// (float)Math.Atan2((double)CheckTar_Direction.Y, (double)CheckTar_Direction.X);
                    CheckTarPos.X += (int)(CheckTar_Direction.X * CheckTarSpeed);
                    CheckTarPos.Y += (int)(CheckTar_Direction.Y * CheckTarSpeed);

                    CheckTarRect.X = (int)CheckTarPos.X - (CheckTarRect.Width / 2);
                    CheckTarRect.Y = (int)CheckTarPos.Y - (CheckTarRect.Height / 2);

                    if (CheckTarRect.Intersects(_targetPlayer.PlayerRect))//Vector2.Distance(CheckTarPos, targetPlayer.pos) < EnemyTexture.Width * 2 + targetPlayer.playerTexture.Width)
                    {
                        CheckTarPos = new Vector2(targetNode.atributes.X + targetNode.atributes.Width / 2,
                            targetNode.atributes.Y + targetNode.atributes.Height / 2);
                        CheckTar_Direction = PlayerPos - new Vector2(targetNode.atributes.X + targetNode.atributes.Width / 2,
                            targetNode.atributes.Y + targetNode.atributes.Height / 2);
                        getNewTargerNode = false;
                    }
                    else
                    {


                        for (int i = 0; i < pathFindNode.Count; i++)
                        {
                            if (Vector2.Distance(CheckTarPos, new Vector2(pathFindNode[i].atributes.X + pathFindNode[i].atributes.Width / 2,
                                   pathFindNode[i].atributes.Y + pathFindNode[i].atributes.Height / 2)) < EnemyTexture.Width / 2 + pathFindNode[i].atributes.Width / 2)
                            {
                                if (!pathFindNode[i].walkable)
                                {
                                    //   if (CheckTarRect.Intersects(pathfindNode[i].atributes))
                                    // {
                                    CheckTarPos = new Vector2(targetNode.atributes.X + targetNode.atributes.Width / 2,
                                        targetNode.atributes.Y + targetNode.atributes.Height / 2);
                                    CheckTar_Direction = PlayerPos - new Vector2(targetNode.atributes.X + targetNode.atributes.Width / 2,
                                        targetNode.atributes.Y + targetNode.atributes.Height / 2);
                                    //  startingNode = targetNode;
                                    if (path.Count == 0 && Vector2.Distance(_targetPlayer.pos, new Vector2(targetNode.atributes.X, targetNode.atributes.Y)) > targetNode.atributes.Width * 10)//Distance to get new TargetNode
                                    {
                                        getNewTargerNode = true;
                                    }
                                }
                            }
                            //   }

                        }


                    }
                }
            }
            Vector2 PlayerPos;
            public void Update(List<Projectile> BulletList/*, Vector2 PlayerPos*/, List<Rectangle> CollisionObjects, List<Rectangle> LevelPicesCollisionObjects, List<Player> players, GameTime gameTime)
            {
                TargetPlayer = ActualTarget;
                CheckRect.Width = EnemyRect.Width;
                CheckRect.Height = EnemyRect.Height;

                CheckTarRect.Width = EnemyRect.Width;
                CheckTarRect.Height = EnemyRect.Height;

                if (players[TargetPlayer].dead)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (!players[i].dead)
                            TargetPlayer = i;
                    }
                }
                //if (!Mutate)
                //{
                //    Prev_Health = Health;
                //    Prev_Speed = Speed;
                //}
                //for (int i = 0; i < path.Count; i++)
                //{
                //    if (i != path.Count - 1)
                //    {
                //        if(path[i].atributes.X
                //    }
                //}
                PlayerPos = players[TargetPlayer].pos;
                Rectangle Cen = new Rectangle(EnemyRect.X + EnemyRect.Width / 2, EnemyRect.Y + EnemyRect.Height / 2, 1, 1);
                CollisionRect =
                        new Rectangle(EnemyRect.X + 17,
                            EnemyRect.Y + 17,
                            EnemyRect.Width - 34,
                            EnemyRect.Height - 34);


                //for (int i = 0; i < pathFindNode.Count; i++)
                //{
                //    if (Cen.Intersects(pathFindNode[i].atributes))
                //    {
                //        My_Tile = i;
                //        startingNode = pathFindNode[i];
                //    }
                //}
                PathFind(gameTime, pathFindNode, players[TargetPlayer]);
                if (StunTime <= 0)
                MoveToTarget(players[TargetPlayer]);


                for (int i = 0; i < BulletList.Count; i++)
                {
                    if (Vector2.Distance(BulletList[i].Pos, new Vector2(pos.X, pos.Y)) <= BulletTexture.Width / 2 + EnemyTexture.Width / 2)
                    {
                        HitBullet = i;
                        hitTick = true;
                    }
                }


              


               
                
                





                Player targetPlayer;
                targetPlayer = players[TargetPlayer];
                PerformPathChecks(targetPlayer, pathFindNode);

                if (hitTick)
                {
                    hitTime++;
                    if (hitTime > 2)
                    {
                        hit.Play();
                        hitTick = false;
                        hitTime = 0;
                    }
                }
                if (StunTime > 0)
                {
                    StunTime -= gameTime.ElapsedGameTime.Milliseconds;
                    if (StunedHealthTick < 200)
                        StunedHealthTick+= gameTime.ElapsedGameTime.Milliseconds;
                    else
                    {
                        if(Health - 1 > 0)
                            Health--;
                        else
                            StunHurt = true;
                        StunedHealthTick = 0;
                    }
                }
                else
                    StunedHealthTick = 0;

                

                GetClosestPlayer(players);

                DetectCollisions(CollisionObjects, Speed);
                DetectCollisions(LevelPicesCollisionObjects, Speed);//
            }
            private void GetClosestPlayer(List<Player> players)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (i != P_WithlowestPlayerDist)
                    {
                        if (Vector2.Distance(players[i].pos, pos) < Vector2.Distance(players[P_WithlowestPlayerDist].pos, pos) + 75)
                        {
                            P_WithlowestPlayerDist = i;
                        }
                    }
                }
                //targetPlayer = players[P_WithlowestPlayerDist];
                ActualTarget = P_WithlowestPlayerDist;
            }
        //Change from rectangles to vector2's
        private void ResetPath(List<PathfindNode> pathFindNode, Rectangle cen, Player targetPlayer)
        {
            for (int i = 0; i < closedList.Count; i++)
            {
                while (closedList.Count > 0)
                {
                    OnClosedList[closedList[i].Index] = false;
                    closedList.RemoveAt(i);
                }
            }
            for (int i = 0; i < openList.Count; i++)
            {
                while (openList.Count > 0)
                {
                    OnOpenList[pathFindNode[i].Index] = false;
                    openList.RemoveAt(i);
                }
            }
            for (int i = 0; i < path.Count; i++)
            {
                while (path.Count > 0)
                {
                    OnPathList[path[i].Index] = false;
                    path.RemoveAt(i);
                }
            }            
            for (int i = 0; i < pathFindNode.Count; i++)
            {
                if (cen.Intersects(pathFindNode[i].atributes))
                {
                    My_Tile = i;
                    startingNode = pathFindNode[i];
                }
                OnOpenList[pathFindNode[i].Index] = false;
                OnClosedList[pathFindNode[i].Index] = false;
                OnPathList[i] = false;
            }
            targetNode = pathFindNode[targetPlayer.Cur_Node];
            startingNode = pathFindNode[My_Tile];

            checkingNode = pathFindNode[My_Tile];
            foundTarget = false;
            doOnce = false;
            PathTimeOutTime = 0;
        }
        private void EmptyCloseOpenLists()
        {
            for (int i = 0; i < closedList.Count; i++)
            {
                while (closedList.Count > 0)
                {
                    OnClosedList[closedList[i].Index] = false;
                    closedList.RemoveAt(i);
                }
            }
            for (int i = 0; i < openList.Count; i++)
            {
                while (openList.Count > 0)
                {
                    OnOpenList[pathFindNode[i].Index] = false;
                    openList.RemoveAt(i);
                }
            }
        }

        public void CheckCollisionSafe(List<Enemy> Enemy, List<Vector2> Spawns)
        {
            if (!attackPlayer)
            {
                CollisionSafe = false;
            }
            for (int i = 0; i < Enemy.Count; i++)
            {
                //Make sure it's not inside an enmey that isn't itself
                if (Enemy[i].CollisionRect.Intersects(CollisionRect) && Enemy[i] != this)
                {
                    CollisionSafe = false;
                    break;
                }
                if (i >= Enemy.Count - 1)
                {
                    for (int b = 0; b < Spawns.Count; b++)
                    {
                        //Make sure its a good dist from all spawns
                        if (Vector2.Distance(pos, Spawns[b]) < EnemyRect.Width + EnemyRect.Height)
                        {
                            CollisionSafe = false;
                            break;
                        }
                        if (b >= Spawns.Count - 1  &&  attackPlayer)
                        {
                            CollisionSafe = true;
                        }
                    }
                }
            }
        }
        private void DetectCollisions(List<Rectangle> CollisionObjects, int Speed)
        {
            int Offset = 5;
            Bottom_Rect = new Rectangle(EnemyRect.X + Offset, EnemyRect.Y + EnemyRect.Height - 1, EnemyRect.Width - (Offset * 2), 2);
            Top_Rect = new Rectangle(EnemyRect.X + Offset, EnemyRect.Y - 1, EnemyRect.Width - (Offset * 2), 2);
            Left_Rect = new Rectangle(EnemyRect.X - 1, EnemyRect.Y + Offset, 2, EnemyRect.Height - (Offset * 2));
            Right_Rect = new Rectangle(EnemyRect.X + EnemyRect.Width - 1, EnemyRect.Y + Offset, 2, EnemyRect.Height - (Offset * 2));

            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                if (i != CollisionIndex)
                {
                    if (EnemyRect.Y + EnemyRect.Height <= CollisionObjects[i].Y + Speed || EnemyRect.Y >= CollisionObjects[i].Y + CollisionObjects[i].Height - Speed)
                    {
                        if (Top_Rect.Intersects(CollisionObjects[i]))
                        {
                            if (EnemyRect.X < CollisionObjects[i].X + CollisionObjects[i].Width - Speed &&
                                EnemyRect.X + EnemyRect.Width > CollisionObjects[i].X + Speed)
                            {
                                EnemyRect.Y = CollisionObjects[i].Y + CollisionObjects[i].Height;
                            }
                        }
                        else if (Bottom_Rect.Intersects(CollisionObjects[i]))
                        {
                            EnemyRect.Y = CollisionObjects[i].Y - EnemyRect.Height;
                        }
                        Bottom_Rect = new Rectangle(EnemyRect.X + Offset, EnemyRect.Y + EnemyRect.Height - 1, EnemyRect.Width - (Offset * 2), 2);
                        Top_Rect = new Rectangle(EnemyRect.X + Offset, EnemyRect.Y - 1, EnemyRect.Width - (Offset * 2), 2);
                        Left_Rect = new Rectangle(EnemyRect.X - 1, EnemyRect.Y + Offset, 2, EnemyRect.Height - (Offset * 2));
                        Right_Rect = new Rectangle(EnemyRect.X + EnemyRect.Width - 1, EnemyRect.Y + Offset, 2, EnemyRect.Height - (Offset * 2));

                    }
                    else
                    {
                        if (Right_Rect.Intersects(CollisionObjects[i]))
                        {
                            EnemyRect.X = CollisionObjects[i].X - EnemyRect.Width;
                        }
                        else if (Left_Rect.Intersects(CollisionObjects[i]))
                        {
                            EnemyRect.X = CollisionObjects[i].X + CollisionObjects[i].Width;
                        }
                        Bottom_Rect = new Rectangle(EnemyRect.X + Offset, EnemyRect.Y + EnemyRect.Height - 1, EnemyRect.Width - (Offset * 2), 2);
                        Top_Rect = new Rectangle(EnemyRect.X + Offset, EnemyRect.Y - 1, EnemyRect.Width - (Offset * 2), 2);
                        Left_Rect = new Rectangle(EnemyRect.X - 1, EnemyRect.Y + Offset, 2, EnemyRect.Height - (Offset * 2));
                        Right_Rect = new Rectangle(EnemyRect.X + EnemyRect.Width - 1, EnemyRect.Y + Offset, 2, EnemyRect.Height - (Offset * 2));
                    }
                }
            }
        }
        public int hitTime = 0;
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Health > 0)
            {
                if (!hitTick)
                {
                    spriteBatch.Draw(EnemyTexture, pos, null, Color.White, rotation, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                }
                else
                {
                    spriteBatch.Draw(EnemyTexture, pos, null, Color.Red, rotation, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                   
                }
                //spriteBatch.Draw(EnemyTexture, CheckTarPos, null, Color.Red, 0f, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                //spriteBatch.Draw(EnemyTexture, CheckPos, null, Color.Red, 0f, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
              //  spriteBatch.Draw(EnemyTexture, EnemyRect, Color.Green);
            }

            //if(StunTime > 0)
            //    spriteBatch.Draw(IceBerg, pos, null, Color.White, rotation, new Vector2(IceBerg.Width / 2, IceBerg.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
         //   spriteBatch.Draw(EnemyTexture, CheckTarRect, Color.Green);
         //   spriteBatch.Draw(EnemyTexture, startingNode.atributes, Color.Green);
         //   spriteBatch.Draw(EnemyTexture, targetNode.atributes, Color.White);
            
            //for (int i = 0; i < path.Count; i++)
            //{
            //    spriteBatch.Draw(EnemyTexture, path[i].atributes, Color.Yellow);
            //    //spriteBatch.Draw(EnemyTexture, OpenList[i], Color.Yellow);
            //}
           // spriteBatch.DrawString(font, "" + foundTarget + "   " + openList.Count, new Vector2(pos.X - 30, pos.Y - 45), Color.White);
           // spriteBatch.DrawString(font, "" + foundTarget + "   " + openList.Count + " / " + closedList.Count + " / " + path.Count, new Vector2(pos.X - 30, pos.Y - 45), Color.White);
            //spriteBatch.DrawString(font, "" + CollisionSafe, new Vector2(pos.X - 30, pos.Y - 45), Color.White);
            //spriteBatch.DrawString(font, "" + Health, new Vector2(pos.X - 30, pos.Y - 45), Color.White);
            //for (int i = 0; i < pathFindNode.Count; i++)
            //{
            //    //spriteBatch.DrawString(font, "" + pathFindNode[i].OnPathList, new Vector2(pathFindNode[i].atributes.X + pathFindNode[i].atributes.Width / 3,
            //    //    pathFindNode[i].atributes.Y + pathFindNode[i].atributes.Height / 3), Color.White);
            //    if (pathFindNode[i].OnOpenList)
            //        spriteBatch.Draw(EnemyTexture, pathFindNode[i].atributes, Color.White);
            //    if (pathFindNode[i].OnCloseList)
            //        spriteBatch.Draw(EnemyTexture, pathFindNode[i].atributes, Color.Green);
            //}
            //spriteBatch.Draw(EnemyTexture, targetNode.atributes, Color.Red);

        }
        public void DrawIce(SpriteBatch spriteBatch)
        {
            if (Health > 0)
            {
                if (!hitTick)
                {
                    spriteBatch.Draw(EnemyTexture, pos, null, Color.White, rotation, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                }
                else
                {
                    spriteBatch.Draw(EnemyTexture, pos, null, Color.Red, rotation, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                   
                }
                //spriteBatch.Draw(EnemyTexture, CheckTarPos, null, Color.Red, 0f, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                //spriteBatch.Draw(EnemyTexture, CheckPos, null, Color.Red, 0f, new Vector2(EnemyTexture.Width / 2, EnemyTexture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
              //  spriteBatch.Draw(EnemyTexture, EnemyRect, Color.Green);
            }
            spriteBatch.Draw(IceBerg, pos, null, Color.White, rotation, new Vector2(IceBerg.Width / 2, IceBerg.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
        }

    }
}
