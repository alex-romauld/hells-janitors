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
    class SurvivalEnemySpawner
    {

        //Pathfind
        private List<PathfindNode> path = new List<PathfindNode>();
        private List<PathfindNode> pathfindNode = new List<PathfindNode>();
        private PathfindNode checkingNode;
        private PathfindNode startingNode;
        private PathfindNode targetNode;
        private List<PathfindNode> openList = new List<PathfindNode>();
        private List<PathfindNode> closedList = new List<PathfindNode>();
        int My_Tile;
        private int StartNode;
        private Boolean foundTarget = false;
        private int baseMovementCost = 10;
        private Boolean once = false;
        private Boolean doOnce = false;
        private int PathTimeOut = 30000;
        private int PathTimeOutTime = 0;
        List<Player> players = new List<Player>();

        private void CalculateHeuritics()
        {
            for (int i = 0; i < pathfindNode.Count; i++)
            {
                pathfindNode[i].h_heuaristicValue = ((Math.Abs(pathfindNode[i].atributes.X - targetNode.atributes.X) + Math.Abs(pathfindNode[i].atributes.Y - targetNode.atributes.Y)) / targetNode.atributes.Width) * 10;
                pathfindNode[i].CalculateFValue();
            }
        }
        private void FindPath()
        {
            if (foundTarget == false)
            {
                if (checkingNode.north != null)
                    DetermineNodeValues(checkingNode, checkingNode.north);
                if (checkingNode.south != null)
                    DetermineNodeValues(checkingNode, checkingNode.south);
                if (checkingNode.east != null)
                    DetermineNodeValues(checkingNode, checkingNode.east);
                if (checkingNode.west != null)
                    DetermineNodeValues(checkingNode, checkingNode.west);

                if (foundTarget == false)
                {
                    closedList.Add(checkingNode);
                    openList.Remove(checkingNode);

                    int SmallestFNode = 0;
                    for (int i = 0; i < openList.Count; i++)
                    {
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
                if(players[i].Cur_Node == testing.Index  &&  !players[i].dead)
                {
                    foundTarget = true;
                    return;
                }
            }
            if (!testing.walkable)
                return;

            if (!closedList.Contains(testing))
            {
                if (openList.Contains(testing))
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
                    testing.parentNode = currentNode;
                    testing.g_movementCost = currentNode.g_movementCost + baseMovementCost;
                    testing.CalculateFValue();
                    openList.Add(testing);
                }
            }
        }
        private void TraceBackPath()
        {
            PathfindNode node = targetNode;
            do
            {
                if (!path.Contains(node))
                {
                    path.Add(node);
                    node = node.parentNode;
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
                    closedList.RemoveAt(i);
                }
            }
            for (int i = 0; i < openList.Count; i++)
            {
                while (openList.Count > 0)
                {
                    openList.RemoveAt(i);
                }
            }
        }
        private void ResetPath(List<PathfindNode> pathFindNode)
        {
            for (int i = 0; i < closedList.Count; i++)
            {
                while (closedList.Count > 0)
                {
                    closedList.RemoveAt(i);
                }
            }
            for (int i = 0; i < openList.Count; i++)
            {
                while (openList.Count > 0)
                {
                    openList.RemoveAt(i);
                }
            }
            for (int i = 0; i < path.Count; i++)
            {
                while (path.Count > 0)
                {
                    path.RemoveAt(i);
                }
            }
            PathTimeOutTime = 0;
            checkingNode = pathfindNode[My_Tile];
            foundTarget = false;
            doOnce = false;
        }
        Rectangle start;
        private void PathFind(List<PathfindNode> pathFindNode, Vector2 spawner, Vector2 Start, Boolean PurchasedDoor)
        {
            //EnemySpanerVector
            Rectangle Cen = new Rectangle((int)spawner.X, (int)spawner.Y, 1, 1);
            start = new Rectangle((int)Start.X, (int)Start.Y, 10, 10);
            if (!once)
            {

                // CalculateHeuritics(i, players[TargetPlayer].Cur_Node);
                this.pathfindNode = pathFindNode;

                    for (int i = 0; i < pathFindNode.Count; i++)
                    {
                        if (Cen.Intersects(pathFindNode[i].atributes))
                        {
                            My_Tile = i;
                            startingNode = pathFindNode[i];
                            
                        }
                    }
                    for (int i = 0; i < pathFindNode.Count; i++)
                    {
                        if (start.Intersects(pathFindNode[i].atributes))
                        {
                            StartNode = i;
                            
                            targetNode = pathFindNode[i];
                        }
                    }

                    

                checkingNode = startingNode;

                CalculateHeuritics();

                once = true;
            }

            if (!foundTarget)
            {
                FindPath();
                if (PurchasedDoor)
                {
                    ResetPath(pathFindNode);
                }
            }
            if (foundTarget)
            {
                Active = true;
                TraceBackPath();
            }


            //for (int i = 0; i < pathFindNode.Count; i++)
            //{
            //    if (Cen.Intersects(pathFindNode[i].atributes))
            //    {
            //        My_Tile = i;
            //        startingNode = pathfindNode[i];
            //    }
            //}
            //Start Node
            //    ResetPath(pathfindNode, Cen, player);
           // targetNode = pathFindNode[StartNode];
        }

        public Boolean Active = false;

        public SurvivalEnemySpawner()
        {

        }

        public Vector2 pos;

        public void Load(Vector2 pos)
        {
            this.pos = pos;
        }

        public void Update(GameTime gameTime, List<Player> players, List<PathfindNode> pathFindNode, Vector2 _start, Boolean PurchasedDoor)
        {
            if (!foundTarget)
            {
                PathFind(pathFindNode, pos, _start, PurchasedDoor);
                PathTimeOutTime += gameTime.ElapsedGameTime.Milliseconds;
                if (PathTimeOutTime >= PathTimeOut)
                {
                    ResetPath(pathFindNode);
                    PathTimeOutTime = 0;
                }
                this.players = players;
            }
            

        }


    }
}
