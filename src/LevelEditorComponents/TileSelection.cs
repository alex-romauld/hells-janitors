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
    class TileSelection
    {
        private Texture2D square;
        private SpriteFont font;
        private GraphicsDevice graphics;

        public Rectangle TileSelectionWindow;
        public Rectangle GrabSelection;
        private Boolean HoveringGrabSection = false;

        private String[] TileSections;
        private Rectangle[] Tab;
        private int SelectedTab;
        private String CurrentTab = "Basic";


        private Rectangle[] BasicTile_Rect;
        private Rectangle[] Tiles_Pieces_Rect;
        private Rectangle[] CurrentSelection = new Rectangle[0];

        private int TileHover = -1;
        public String SelectedTile = "Wall";
        public Boolean MouseIntersectsGUI = false;
        public Boolean dragWindow = false;

        private Rectangle ToolTipRect;
        private String ToolTip = "";

        private TileManager tileManager = new TileManager();
        public Tile selectedTile;
        private Tile[] currentTiles;

        private List<Tile> curretTileTypes = new List<Tile>();
        private int CurrentTileType = 0;

        public TileSelection()
        {
        }

        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            square = content.Load<Texture2D>("Sprites/square");
            font = content.Load<SpriteFont>("standardfont");
            this.graphics = graphics;

            tileManager.Load(content);
            selectedTile = tileManager.Wall;
            currentTiles = tileManager.BasicTile;
            BasicTile_Rect = new Rectangle[tileManager.BasicTile.Length];
            Tiles_Pieces_Rect = new Rectangle[tileManager.PiecesTile.Length];
            

            CurrentSelection = BasicTile_Rect;

            TileSelectionWindow = new Rectangle(75, 35, 350, 650);//(graphics.Viewport.Width - 400, 25, 350, 650);
            GrabSelection = new Rectangle(TileSelectionWindow.X, TileSelectionWindow.Y, TileSelectionWindow.Width, 35);



            TileSections = new String[2];
            Tab = new Rectangle[TileSections.Length];
            TileSections[0] = "Basic";
            //TileSections[1] = "Aesthetic";
            TileSections[1] = "Pieces";
        }
        

        public void Update(GameTime gameTime)
        {
            UpdateMouse();
            UpdateMouseSelection();

            if (CurrentTab == "Basic")
            {
                CurrentSelection = BasicTile_Rect;
                currentTiles = tileManager.BasicTile;
            }
            if (CurrentTab == "Pieces")
            {
                CurrentSelection = Tiles_Pieces_Rect;
                currentTiles = tileManager.PiecesTile;
            }
            GrabSelection = new Rectangle(TileSelectionWindow.X, TileSelectionWindow.Y, TileSelectionWindow.Width, 35);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

            spriteBatch.Draw(square, new Rectangle(TileSelectionWindow.X - 10, TileSelectionWindow.Y - 10, TileSelectionWindow.Width + 20, TileSelectionWindow.Height + 20), Color.Black);
            spriteBatch.Draw(square, TileSelectionWindow, new Color(200, 200, 200));
            spriteBatch.Draw(square, GrabSelection, Color.WhiteSmoke);

            if(CurrentTab == "Basic")
                BasicTile_Rect = DrawTiles(spriteBatch, tileManager.BasicTile, BasicTile_Rect);
            if (CurrentTab == "Pieces")
                Tiles_Pieces_Rect = DrawTiles(spriteBatch, tileManager.PiecesTile, Tiles_Pieces_Rect);
            if (CurrentTab == "Basic")
            {
                if(curretTileTypes.Count > 0)
                if (curretTileTypes[CurrentTileType] != null)
                {



                    spriteBatch.Draw(curretTileTypes[CurrentTileType].texture, new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35, TileSelectionWindow.Y + 300, 70, 70), Color.White);

                    if (!_mouseRect.Intersects(new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35 - 30, TileSelectionWindow.Y + 300, 20, 70)))
                    {
                        spriteBatch.Draw(square, new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35 - 30, TileSelectionWindow.Y + 300, 20, 70), Color.Black);
                        spriteBatch.DrawString(font, "<", new Vector2(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35 - 30, TileSelectionWindow.Y + 300 + 35 - font.MeasureString("<").Y / 2), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(square, new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35 - 30, TileSelectionWindow.Y + 300, 20, 70), Color.White);
                        spriteBatch.DrawString(font, "<", new Vector2(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35 - 30, TileSelectionWindow.Y + 300 + 35 - font.MeasureString("<").Y / 2), Color.Black);
                    }

                    if (!_mouseRect.Intersects(new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 + 35 + 10, TileSelectionWindow.Y + 300, 20, 70)))
                    {
                        spriteBatch.Draw(square, new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 + 35 + 10, TileSelectionWindow.Y + 300, 20, 70), Color.Black);
                        spriteBatch.DrawString(font, ">", new Vector2(TileSelectionWindow.X + TileSelectionWindow.Width / 2 + 35 + 10 + 20 - font.MeasureString(">").X, TileSelectionWindow.Y + 300 + 35 - font.MeasureString(">").Y / 2), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(square, new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 + 35 + 10, TileSelectionWindow.Y + 300, 20, 70), Color.White);
                        spriteBatch.DrawString(font, ">", new Vector2(TileSelectionWindow.X + TileSelectionWindow.Width / 2 + 35 + 10 + 20 - font.MeasureString(">").X, TileSelectionWindow.Y + 300 + 35 - font.MeasureString(">").Y / 2), Color.Black);
                    }
                }
            }
            //CurrentTileType

            DrawTabs(spriteBatch);
            if (TileHover >= 0)
            {
                spriteBatch.Draw(square, new Rectangle(ToolTipRect.X - 3, ToolTipRect.Y - 3, ToolTipRect.Width + 6, ToolTipRect.Height + 6), Color.DarkViolet);
                spriteBatch.Draw(square, ToolTipRect, Color.Black);
                
                spriteBatch.DrawString(font, ToolTip, new Vector2(ToolTipRect.X + 10, ToolTipRect.Y), Color.White);
            }
        }
        
        private MouseState oldState = Mouse.GetState();
        private void UpdateMouseSelection()
        {
            int xOffset = 0;
            int yOffset = 0;
            MouseState newState = Mouse.GetState();

            if (dragWindow)
            {

                if (newState.X > 0 && newState.X < graphics.Viewport.Width)
                    TileSelectionWindow.X = newState.X - GrabSelection.Width / 2;
                if (newState.Y - GrabSelection.Height / 2 > 0 && newState.Y + GrabSelection.Height / 2 < graphics.Viewport.Height)
                    TileSelectionWindow.Y = newState.Y - GrabSelection.Height / 2;

                if (newState.LeftButton == ButtonState.Released)
                    dragWindow = false;
            }
            if (oldState.LeftButton != newState.LeftButton)
            {

                    if (HoveringGrabSection)
                    {
                        dragWindow = true;
                        xOffset = newState.X - TileSelectionWindow.X;
                        yOffset = newState.Y - TileSelectionWindow.Y;
                    }

                    if (oldState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                    if (TileHover >= 0)
                    {
                        selectedTile = currentTiles[TileHover];//##
                        if (CurrentTab == "Basic")
                        {
                            CurrentTileType = 0;
                            curretTileTypes.Clear();
                            curretTileTypes.AddRange(tileManager.AssociatedTypes_Basic[TileHover]);
                        }
                    }

                    if (SelectedTab == 0)
                        CurrentTab = "Basic";
                    if (SelectedTab == 1)
                        CurrentTab = "Pieces";

                    if (oldState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
                    if (CurrentTab == "Basic")
                    {
                        if(curretTileTypes.Count > 0)
                        if (curretTileTypes[CurrentTileType] != null)
                        {
                            Rectangle prevType = new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 - 35 - 30, TileSelectionWindow.Y + 300, 20, 70);
                            Rectangle nextType = new Rectangle(TileSelectionWindow.X + TileSelectionWindow.Width / 2 + 35 + 10, TileSelectionWindow.Y + 300, 20, 70);
                            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(prevType))
                            {
                                if (CurrentTileType <= 0) CurrentTileType = curretTileTypes.Count - 1;
                                else CurrentTileType--;
                                selectedTile = curretTileTypes[CurrentTileType];
                            }
                            if (new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1).Intersects(nextType))
                            {
                                if (CurrentTileType >= curretTileTypes.Count - 1) CurrentTileType = 0;
                                else CurrentTileType++;
                                selectedTile = curretTileTypes[CurrentTileType];
                            }
                        }
                    }
                oldState = newState;
            }
        }
        Rectangle _mouseRect;
        private void UpdateMouse()
        {
            Rectangle mouseRect= new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
            this._mouseRect = mouseRect;
            ToolTipRect = new Rectangle(mouseRect.X + 5, mouseRect.Y - 40, (int)font.MeasureString(ToolTip).X + 20, 35);
            if (TileHover >= 0)
                ToolTip = currentTiles[TileHover].name;
            if (mouseRect.Intersects(TileSelectionWindow))
                MouseIntersectsGUI = true;
            else
                MouseIntersectsGUI = false;
            for (int i = 0; i < currentTiles.Length ; i++)
            {

                if (mouseRect.Intersects(CurrentSelection[i]))
                    TileHover = i;



                if (TileHover >= 0)
                    if (i >= CurrentSelection.Length - 1 && !mouseRect.Intersects(CurrentSelection[TileHover]))
                        TileHover = -1;

            }

            for (int i = 0; i < Tab.Length; i++)
            {
                if (mouseRect.Intersects(Tab[i]))
                    SelectedTab = i;
                if (SelectedTab >= 0)
                    if (i >= Tab.Length - 1 && !mouseRect.Intersects(Tab[SelectedTab]))
                        SelectedTab = -1;
            }
            if (mouseRect.Intersects(GrabSelection))
                HoveringGrabSection = true;
            else
                HoveringGrabSection = false;
            
        }

        private void DrawTabs(SpriteBatch spriteBatch)
        {
            int TabWidth = 150;
            for (int i = 0; i < TileSections.Length; i++)
            {
                Tab[i] = new Rectangle(TileSelectionWindow.X + (TabWidth * i) + (i * 10), TileSelectionWindow.Y + GrabSelection.Height, TabWidth, 40);
                if(SelectedTab == i)
                    spriteBatch.Draw(square, Tab[i], Color.SkyBlue);
                else
                    spriteBatch.Draw(square, Tab[i], Color.Aqua);
                spriteBatch.DrawString(font, TileSections[i], new Vector2(Tab[i].X + 5, Tab[i].Y), Color.White);
            }
        }

        private Rectangle[] DrawTiles(SpriteBatch spriteBatch, Tile[] Tiles, Rectangle[] ButtonRect)
        {
            int x = 0;
            int y = 0;
            int Width = 70;
            int Height = 70;

            int Spacing = 10;
            int XOffset = TileSelectionWindow.X + 10;
            int YOffset = TileSelectionWindow.Y + 90;

            int itemsPerRow = 4;

            for (int i = 0; i < Tiles.Length; i++)
            {
                if (i % itemsPerRow == 0)
                    x = 0;
                else
                    x += Width + Spacing;
                y = (i / itemsPerRow) * (Height + Spacing);

                ButtonRect[i] = new Rectangle(x + XOffset, y + YOffset, Width, Height);
                if(TileHover != i)
                    spriteBatch.Draw(Tiles[i].texture, ButtonRect[i], Color.White);
                else
                    spriteBatch.Draw(Tiles[i].texture, ButtonRect[i], Color.Blue);
            }
            return ButtonRect;
        }



    }
}
