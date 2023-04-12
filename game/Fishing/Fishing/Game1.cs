using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Fishing
{
    //Game state enum
    enum MenuState
    {
        Closed,
        Main,
        Stats,
        Achievements
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Game state
        MenuState menuState;

        //Window dimensions
        private int windowWidth;
        private int windowHeight;

        //Background
        Color backgroundColor;

        //Random object
        private Random rng;

        //Textures
        Texture2D backgroundTexture;
        List<Texture2D> fishTextures;
        Texture2D rodTexture;
        Texture2D bookTexture;

        //Collectible manager
        CollectibleManager collectibleManager;

        //Fishing rod class setup
        FishingRod fishingRod;

        //buttons
        private List<Button> buttonList;

        //Menu
        Menu menu;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            //Initialize window width and height
            windowWidth = _graphics.GraphicsDevice.Viewport.Width;
            windowHeight = _graphics.GraphicsDevice.Viewport.Height;

            backgroundColor = new Color(87, 165, 255);

            //Initialize Random object
            rng = new Random();

            //Initialize the list of fish textures
            fishTextures = new List<Texture2D>();

            //Initialize menu
            menu = new Menu(windowWidth, windowHeight);

            //Set the initial menuState to be closed
            menuState = MenuState.Closed;

            //Initializes rod texture
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load the fish textures and add them to the list of textures
            fishTextures.Add(Content.Load<Texture2D>("angelfish"));
            fishTextures.Add(Content.Load<Texture2D>("aquafish"));
            fishTextures.Add(Content.Load<Texture2D>("flamefish"));
            fishTextures.Add(Content.Load<Texture2D>("siren"));

            //Loads fishing rod texture
            rodTexture = Content.Load<Texture2D>("rodPH");

            // Load book texture
            bookTexture = Content.Load<Texture2D>("book");

            //initializes fishing rod
            fishingRod = new FishingRod(rodTexture, 1000, windowWidth / 2, 1, windowWidth, windowHeight); //TODO: Update the depth and position to the starting depth and position we want, values are just placeholder

            //Initialize the CollectibleManager
            collectibleManager = new CollectibleManager(rng, windowWidth, windowHeight,
                fishTextures, bookTexture, fishingRod);

            //Initialize the background
            //bg = new Background(windowWidth, windowHeight, GraphicsDevice, fishingRod, backgroundTexture);

            //load textures for menu
            menu.Load(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //If the game is running
            if (menuState == MenuState.Closed)
            {
                //Update the collectibles
                collectibleManager.Update();

                //Update the fishing rod
                fishingRod.Update(gameTime);

                // Check for a change in the number of skill points
                fishingRod.skillPointChange(collectibleManager.SkillPoints);
            }

            //Update the background color
            UpdateBackground();

            menu.Update(gameTime, fishingRod);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch.Begin();

            // TODO: Add your drawing code here

            //Draw the fishing rod
            fishingRod.Draw(_spriteBatch);

            //Draw the collectibles in the Collectible Manager
            collectibleManager.Draw(_spriteBatch);

            //draw menu
            menu.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates the background color of the screen.
        /// </summary>
        private void UpdateBackground()
        {
            if (fishingRod.Rect.Y >= windowHeight - fishingRod.Rect.Height
                && fishingRod.CurrentDepth < fishingRod.MaxDepth
                && fishingRod.PlayerDirection == Direction.Down)
            {
                if (backgroundColor.R > 10)
                {
                    backgroundColor.R--;
                }

                if (backgroundColor.G > 10)
                {
                    backgroundColor.G--;
                }

                if (backgroundColor.B > 20)
                {
                    backgroundColor.B--;
                }
            }

            else if (fishingRod.Rect.Y <= 0
                && fishingRod.CurrentDepth > 0
                && fishingRod.PlayerDirection == Direction.Up)
            {
                if (backgroundColor.R < 87)
                {
                    backgroundColor.R++;
                }

                if (backgroundColor.G < 165)
                {
                    backgroundColor.G++;
                }

                if (backgroundColor.B < 255)
                {
                    backgroundColor.B++;
                }
            }

        }
    }
}