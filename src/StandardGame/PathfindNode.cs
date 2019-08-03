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

namespace SurvivalShooter.StandardGame
{
    class PathfindNode
    {
        public int h_heuaristicValue = 0;
        public int g_movementCost = 0;
        public int f_totalCost = 0;
        public PathfindNode parentNode = null;
        public PathfindNode north = null;
        public PathfindNode east = null;
        public PathfindNode south = null;
        public PathfindNode west = null;

        public Rectangle atributes;
        public Boolean walkable;

        public int Index;
        //public Boolean OnOpenList = false;
        //public Boolean OnCloseList = false;
        //public Boolean OnPathList = false;

        public PathfindNode(List<PathfindNode> pathfindNode, int Index, Rectangle atributes, Boolean walkable)
        {
           
              //  DetectAdjacentNodes(pathfindNode, Index);
                this.Index = Index;
            this.atributes = atributes;
            this.walkable = walkable;
        }

        public void CalculateFValue()
        {
            f_totalCost = g_movementCost + h_heuaristicValue;
        }
        public void DetectAdjacentNodes(List<PathfindNode> pathfindNode, int index)
        {
            for (int i = 0; i < pathfindNode.Count; i++)
            {
                if (pathfindNode[i].atributes.X == pathfindNode[index].atributes.X)
                {
                    if (pathfindNode[i].atributes.Y == pathfindNode[index].atributes.Y - pathfindNode[i].atributes.Height)
                    {//Top
                        north = pathfindNode[i];
                    }
                    if (pathfindNode[i].atributes.Y == pathfindNode[index].atributes.Y + pathfindNode[index].atributes.Height)
                    {//Bottom
                        south = pathfindNode[i];
                    }
                }
                if (pathfindNode[i].atributes.Y == pathfindNode[index].atributes.Y)
                {
                    if (pathfindNode[i].atributes.X == pathfindNode[index].atributes.X - pathfindNode[i].atributes.Width)
                    {//Left
                        west = pathfindNode[i];
                    }
                    if (pathfindNode[i].atributes.X == pathfindNode[index].atributes.X + pathfindNode[index].atributes.Width)
                    {//Right
                        east = pathfindNode[i];
                    }
                }
            }
        }

        
    }
}
