using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RPG
{
    public class MapCharacter
    {
        public bool isMoving = false;
        protected Texture2D Texture; 
        public Color Color = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        public float Scale = 1f;
        public SpriteEffects SpriteEffect;
        protected Rectangle[] Rectangles;
        protected int FrameIndex = 0;
        public int X = 0;
        public int Y = 0;
        public int TilePosX;
        public int TilePosY;
        public int Direction;
        public bool Running = false;
        public int CountToMove = 0;
        public bool Visible = false;
        public bool IsWandering = false;
        public byte Speed = 1;
        public Random RandomWander;
        public int Seed;

        public MapCharacter(Texture2D Texture, int frames, byte posX, byte posY, int RandomSeed)
        {
            X = (posX * GlobalVariables.TileSize);
            Y = (posY * GlobalVariables.TileSize);
            TilePosX = posX;
            TilePosY = posY;
            this.Texture = Texture;
            int width = Texture.Width / frames;
            Rectangles = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
                Rectangles[i] = new Rectangle(
                    i * width, 0, width, GlobalVariables.TileSize);
            RandomWander = new Random(RandomSeed);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 viewportOffset)
        {
            //Console.WriteLine(TilePosX + " " + TilePosY);
            //spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(
                Texture,
                new Vector2((X - (int)viewportOffset.X), (Y - (int)viewportOffset.Y)),
                Rectangles[FrameIndex],
                Color,
                Rotation,
                Origin,
                Scale,
                SpriteEffect,
                0f);
            //spriteBatch.End();
        }

        public void Move(int direction)
        {
            if (CountToMove == 0)
                isMoving = false;
            if (!isMoving)
            {
                Direction = direction;
                isMoving = true;
                CountToMove = GlobalVariables.TileSize;
            }
        }

        public void MoveFor(int direction, int TilesToMove)
        {
            if (CountToMove == 0)
                isMoving = false;
            if (!isMoving)
            {
                Direction = direction;
                isMoving = true;
                CountToMove = (TilesToMove * GlobalVariables.TileSize);
            }
        }

        public void Wander()
        {
            Move(RandomWander.Next(4));
        }

        public void Update()
        {
            if (FrameIndex == 8)
                FrameIndex = 0;

            TilePosX = (X / GlobalVariables.TileSize);
            TilePosY = (Y / GlobalVariables.TileSize);

            if (TilePosX == -1)
                X = (X + GlobalVariables.TileMapResolutionX);
            if (TilePosY == -1)
                Y = (Y + GlobalVariables.TileMapResolutionY);
            if (TilePosX == GlobalVariables.TileMapWidth)
                X = (X - GlobalVariables.TileMapResolutionX);
            if (TilePosY == GlobalVariables.TileMapHeight)
                Y = (Y - GlobalVariables.TileMapResolutionY);

            if (IsWandering)
                Wander();

            if (isMoving == false)
            {
                CountToMove = 0;
            }
            if (isMoving)
            {
                for (int i = 0; i < Speed; i++)
                {
                    if (Direction == 0)
                        Y--;
                    if (Direction == 1)
                        Y++;
                    if (Direction == 2)
                        X++;
                    if (Direction == 3)
                        X--;
                    FrameIndex++;
                    CountToMove--;
                }
            }
            if ((CountToMove > 0) && Running)
                for (int i = 0; i < Speed; i++)
                {
                    if (Direction == 0)
                        Y--;
                    if (Direction == 1)
                        Y++;
                    if (Direction == 2)
                        X++;
                    if (Direction == 3)
                        X--;
                    FrameIndex++;
                    CountToMove--;
                }
            if (CountToMove == 0)
            {
                //If the moving flag hasn't been turned off yet...
                if (isMoving)
                {
                    //Console.WriteLine("Just walked one tile!");
                    //Do stuff for when the character has landed on a tile
                    //Check for battle...
                    //Check for poison...
                    //Check for damage tile...
                    //Increment global step counter...
                }

                isMoving = false;
            }
        }
    }
}
