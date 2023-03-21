using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Fishing
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Window dimensions
        private int windowWidth;
        private int windowHeight;

        //Random object
        private Random rng;

        //List of fish textures
        List<Texture2D> fishTextures;
        Texture2D rodTexture;

        //Collectible manager
        CollectibleManager collectibleManager;

        //Fishing rod class setup
        FishingRod fishingRod;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initialize window width and height
            windowWidth = _graphics.GraphicsDevice.Viewport.Width;
            windowHeight = _graphics.GraphicsDevice.Viewport.Height;

            //Initialize Random object
            rng = new Random();

            //Initialize the list of fish textures
            fishTextures = new List<Texture2D>();

            //initialize rod texture
            rodTexture = new Texture2D(); //TODO: Verify if this works or if there is a better way to do this.

            //Initializes rod texture
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load the fish textures and add them to the list of textures
            fishTextures.Add(Content.Load<Texture2D>("fish_placeholder"));
            fishTextures.Add(Content.Load<Texture2D>("fish_placeholder2"));

            //Loads fishing rod texture
            rodTexture.Add(Content.Load<Texture2D>("rodPH"));
            //Initialize the CollectibleManager
            collectibleManager = new CollectibleManager(rng, windowWidth, windowHeight,
                fishTextures);

            //initializes fishing rod
            fishingRod = new FishingRod(rodTexture, depth, pos) //TODO: Update the depth and position to the starting depth and position we want, values are just placeholder
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            collectibleManager.Update();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // TODO: Add your drawing code here

            //Draw the fish in the Collectible Manager
            collectibleManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}