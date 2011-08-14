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

//xTile engine namespaces
using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;

namespace RPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables
        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        Microsoft.Xna.Framework.Rectangle destinationRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);
        
        //xTile Stuff...
        XnaDisplayDevice mapDisplayDevice;
        //IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;
        xTile.Dimensions.Rectangle viewportNorth;
        xTile.Dimensions.Rectangle viewportSouth;
        xTile.Dimensions.Rectangle viewportEast;
        xTile.Dimensions.Rectangle viewportWest;


        //Numbers and whatnot...
        Vector2 DesignResolution = new Vector2(256, 192);
        Vector2 ScreenResolution = new Vector2(1280, 720);
        Vector2 TempResolution;
        Vector2 ScreenCenter;
        Vector2 AspectRatio;
        int ScaleFactorX = 0;
        float MapRotation = 0;
        public Vector2 viewportOffset;
        Random RandomSeed;
        #endregion

        #region Game Objects
        //Game Objects
        MapCharacter[] mapCharacter;
        Map GameMap;
        #endregion

        public Game1()
        {
            AspectRatio.X = (DesignResolution.X / GlobalVariables.TileSize);
            AspectRatio.Y = (DesignResolution.Y / GlobalVariables.TileSize);
            TempResolution.X = DesignResolution.X;
            TempResolution.Y = DesignResolution.Y;
            // made a change
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 65);

            //Graphics Initialization
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)ScreenResolution.X;
            graphics.PreferredBackBufferHeight = (int)ScreenResolution.Y;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            RandomSeed = new Random();

            //Sony's localization decisions be damned...
            InputManager.JapaneseButtonSwap = true;
            
            // What exactly does distinationRect represent?
            /*
             * The destinationRect defines the area of the screen 
             * that the rendertarget gets rendered to.
             * In the Draw() loop, the rendertarget is drawn with destinationRect as its
             * rectangle. This means that the "game window" can be any size within the
             * "program window." 
             * 
             * The below code centers it and sets it to fill the height of the screen
             * while scaling width to match the intended aspect ratio.
            */
            ScaleFactorX = (((int)ScreenResolution.Y / (int)AspectRatio.Y) * (int)AspectRatio.X);
            destinationRect.X = ((int)ScreenResolution.X / 2);
            destinationRect.Y = ((int)ScreenResolution.Y / 2);
            destinationRect.Height = (int)ScreenResolution.Y;
            destinationRect.Width = (ScaleFactorX);

            Camera.X = 0;
            Camera.Y = 0;
            mapDisplayDevice = new XnaDisplayDevice(
                this.Content, this.GraphicsDevice);
            
            viewport = new xTile.Dimensions.Rectangle(new Size ( (int)TempResolution.X, (int) TempResolution.Y) );
            ///For one-axis map wrapping
            viewportNorth = new xTile.Dimensions.Rectangle(new Size( (int)TempResolution.X, (int)TempResolution.Y) );
            viewportSouth = new xTile.Dimensions.Rectangle(new Size( (int)TempResolution.X, (int)TempResolution.Y) );
            viewportEast = new xTile.Dimensions.Rectangle(new Size( (int)TempResolution.X, (int)TempResolution.Y) );
            viewportWest = new xTile.Dimensions.Rectangle(new Size( (int)TempResolution.X, (int)TempResolution.Y) );

            renderTarget = new RenderTarget2D(GraphicsDevice, (int)TempResolution.X, (int)TempResolution.Y, false, SurfaceFormat.Color, 0);

            ///Initialize Objects
            //Characters
            mapCharacter = new MapCharacter[255];
            for (int i = 0; i < 255; i++)
                mapCharacter[i] = new MapCharacter(Content.Load<Texture2D>("Texture//Character//00"), 9, 8, 6, RandomSeed.Next());
            mapCharacter[0].Visible = true;
            mapCharacter[1].Visible = true;
            mapCharacter[1].IsWandering = true;

            for (int i = 2; i < 14; i++)
            {
                mapCharacter[i].Visible = true;
                mapCharacter[i].IsWandering = true;
            }


            //Maps
            GameMap.LoadTileSheets(mapDisplayDevice);

            GlobalVariables.TileMapWidth = (GameMap.Layers[0].LayerWidth);
            GlobalVariables.TileMapHeight = (GameMap.Layers[0].LayerHeight);
            GlobalVariables.TileMapResolutionX = (GlobalVariables.TileMapWidth * GlobalVariables.TileSize);
            GlobalVariables.TileMapResolutionY = (GlobalVariables.TileMapHeight * GlobalVariables.TileSize);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameMap = Content.Load<Map>("Map\\01");
            //GameMap.LoadTileSheets(mapDisplayDevice);
            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            GameMap.DisposeTileSheets(mapDisplayDevice);
            GameMap = null;
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            //Update Characters
            foreach (MapCharacter i in mapCharacter)
                if (i.Visible == true) { i.Update(); }


            if (InputManager.Select)
                this.Exit();
            if ((InputManager.Right) && ((!mapCharacter[0].isMoving) && (!InputManager.Up) && (!InputManager.Down)))
                mapCharacter[0].Move(2);
            if ((InputManager.Left) && ((!mapCharacter[0].isMoving) && (!InputManager.Up) && (!InputManager.Down)))
                mapCharacter[0].Move(3);
            if ((InputManager.Up) && (!mapCharacter[0].isMoving))
                mapCharacter[0].Move(0);
            if ((InputManager.Down) && (!mapCharacter[0].isMoving))
                mapCharacter[0].Move(1);
            if (InputManager.Cancel)
                mapCharacter[0].Running = true;
            if (!InputManager.Cancel)
            {
                mapCharacter[0].Running = false;
            }
            if (InputManager.Confirm)
            {
            }
            if (InputManager.Y)
            {
            }
            
            if ((InputManager.X))
            {
                mapCharacter[1].MoveFor(2, 1);
            }

            Camera.FollowCharacter(mapCharacter[0], ScreenCenter);
            
            GameMap.Update(gameTime.ElapsedGameTime.Milliseconds);

            //Update the viewport to current zoomed size
            viewport = new xTile.Dimensions.Rectangle(
                new Location(Camera.X, Camera.Y),
                new Size( (int)TempResolution.X, (int)TempResolution.Y));
            //Wrapping Viewports
            viewportNorth = new xTile.Dimensions.Rectangle(
                new Location(Camera.X, ( Camera.Y - GlobalVariables.TileMapResolutionY) ),
                new Size((int)TempResolution.X, (int)TempResolution.Y));
            viewportSouth = new xTile.Dimensions.Rectangle(
                new Location(Camera.X, (Camera.Y + GlobalVariables.TileMapResolutionY)),
                new Size((int)TempResolution.X, (int)TempResolution.Y));
            viewportEast = new xTile.Dimensions.Rectangle(
                new Location( (Camera.X + GlobalVariables.TileMapResolutionX), Camera.Y),
                new Size((int)TempResolution.X, (int)TempResolution.Y));
            viewportWest = new xTile.Dimensions.Rectangle(
                new Location((Camera.X - GlobalVariables.TileMapResolutionX), Camera.Y),
                new Size((int)TempResolution.X, (int)TempResolution.Y));

            //Update the RenderTarget2D
            renderTarget = new RenderTarget2D(GraphicsDevice, (int)TempResolution.X, (int)TempResolution.Y, false, SurfaceFormat.Color, 0);

            //Used for rotation origin, also screen positioning
            ScreenCenter.X = (TempResolution.X / 2);
            ScreenCenter.Y = (TempResolution.Y / 2);

            //Used for drawing sprites, to keep them in sync with the scrolled map
            viewportOffset.X = viewport.Location.X;
            viewportOffset.Y = viewport.Location.Y;

            base.Update(gameTime);

            Console.WriteLine(viewport.Location);
        }


        private void MapCharacterDraw(object sender, LayerEventArgs layerEventArgs)
        {
            SpriteBatch spriteBatch = mapDisplayDevice.SpriteBatchAlpha;
            foreach (MapCharacter i in mapCharacter)
                if (i.Visible == true) { i.Draw(spriteBatch, viewportOffset); }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            ///Objects to draw...
            //Map

            GameMap.Layers[1].BeforeDraw += MapCharacterDraw;
            switch (GlobalVariables.MapWrapType)
            {
                case 0:
                    GameMap.Draw(mapDisplayDevice, viewport, Location.Origin, false);
                    break;
                //Horizontal Wrap
                case 1:
                    GameMap.Draw(mapDisplayDevice, viewport);
                    GameMap.Draw(mapDisplayDevice, viewportEast);
                    GameMap.Draw(mapDisplayDevice, viewportWest);
                    break;
                //Vertical Wrap
                case 2:
                    GameMap.Draw(mapDisplayDevice, viewport);
                    GameMap.Draw(mapDisplayDevice, viewportNorth);
                    GameMap.Draw(mapDisplayDevice, viewportSouth);
                    break;
                //Full Wrap
                case 3:
                    GameMap.Draw(mapDisplayDevice, viewport, Location.Origin, true);
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                null,
                SamplerState.PointClamp,
                null,
                null);

            spriteBatch.Draw(
                renderTarget,
                destinationRect,
                null,
                Color.White,
                MapRotation,
                ScreenCenter,
                SpriteEffects.None,
                0);
            
            spriteBatch.End();
        }
    }
}
