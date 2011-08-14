using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using xTile;
using xTile.Dimensions;
using xTile.Display;

namespace RPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        //Microsoft.Xna.Framework.Rectangle destinationRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);

        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        public byte TileSize = 16;
        Vector2 DesignResolution = new Vector2(256, 192);
        Vector2 ScreenResolution = new Vector2(1280, 720);
        Vector2 TempResolution;
        Vector2 ScreenCenter;
        Vector2 AspectRatio;
        int ScaleFactorX = 0;
        float MapRotation = 0;
        public Vector2 viewportOffset;

        MapCharacter[] mapCharacter;
        Map GameMap;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            AspectRatio.X = (DesignResolution.X / TileSize);
            AspectRatio.Y = (DesignResolution.Y / TileSize);
            TempResolution.X = DesignResolution.X;
            TempResolution.Y = DesignResolution.Y;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ScaleFactorX = (((int)ScreenResolution.Y / (int)AspectRatio.Y) * (int)AspectRatio.X);
            //destinationRect.X = ((int)ScreenResolution.X / 2);
            //destinationRect.Y = ((int)ScreenResolution.Y / 2);
            //destinationRect.Height = (int)ScreenResolution.Y;
            //destinationRect.Width = (ScaleFactorX);

            Camera.X = 0;
            Camera.Y = 0;
            mapDisplayDevice = new XnaDisplayDevice(
                this.Content, this.GraphicsDevice);

            viewport = new xTile.Dimensions.Rectangle(new Size((int)TempResolution.X, (int)TempResolution.Y));
            //viewportL = new xTile.Dimensions.Rectangle(new Size( (int)TempResolution.X, (int)TempResolution.Y) );
            //viewportR = new xTile.Dimensions.Rectangle(new Size( (int)TempResolution.X, (int)TempResolution.Y) );
            renderTarget = new RenderTarget2D(GraphicsDevice, (int)TempResolution.X, (int)TempResolution.Y, false, SurfaceFormat.Color, 0);

            ///Initialize Objects
            //Characters
            mapCharacter = new MapCharacter[255];
            for (int i = 0; i < 255; i++)
                mapCharacter[i] = new MapCharacter(Content.Load<Texture2D>("Texture//Character//00"), 9, 0, 0);
            mapCharacter[0].Visible = true;
            mapCharacter[1].Visible = true;
            //Maps
            GameMap.LoadTileSheets(mapDisplayDevice);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameMap = Content.Load<Map>("Map\\00");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
            if (InputManager.Y)
            {
                mapCharacter[0].Running = true;
                DiceBag.Shake(20);
                Console.WriteLine(DiceBag.RollDice);
            }
            if (!InputManager.Y)
            {
                mapCharacter[0].Running = false;
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
                new Size((int)TempResolution.X, (int)TempResolution.Y));

            renderTarget = new RenderTarget2D(GraphicsDevice, (int)TempResolution.X, (int)TempResolution.Y, false, SurfaceFormat.Color, 0);

            //Used for rotation origin, also screen positioning
            ScreenCenter.X = (TempResolution.X / 2);
            ScreenCenter.Y = (TempResolution.Y / 2);

            //Used for drawing sprites, to keep them in sync with the scrolled map
            viewportOffset.X = viewport.Location.X;
            viewportOffset.Y = viewport.Location.Y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            ///Objects to draw...
            //Map
            GameMap.Draw(mapDisplayDevice, viewport, Location.Origin, (true));


            //MapCharacters
            //TO-DO: Hook an event handler to the BeforeDraw of the top map layer to draw the characters on the map instead of drawing them after the map
            foreach (MapCharacter i in mapCharacter)
                if (i.Visible == true) { i.Draw(spriteBatch, viewportOffset); }

            base.Draw(gameTime);
            //GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(
                renderTarget,
                GraphicsDevice.Viewport.Bounds,
                Color.Wheat);

            spriteBatch.End();
        }
    }
}
