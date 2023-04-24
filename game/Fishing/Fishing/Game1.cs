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
        private MenuState menuState;

        //Window dimensions
        private int windowWidth;
        private int windowHeight;

        //Background
        private Background bg;

        //Random object
        private Random rng;

        //Textures
        private Texture2D backgroundTexture;
        private List<Texture2D> fishTextures;
        private Texture2D rodTexture;
        private Texture2D bookTexture;
        private Texture2D rodLineTexture;

        //Collectible manager
        private CollectibleManager collectibleManager;

        //Fishing rod class setup
        private FishingRod fishingRod;

        //buttons
        private List<Button> buttonList;

        //Menu
        private Menu menu;

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

            //Initialize background color

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

            //Initialize base keybinds
            fishingRod.Keybinds = Keybinds.WASD;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load background
            backgroundTexture = Content.Load<Texture2D>("backgroundTexture");

            //Load the fish textures and add them to the list of textures
            fishTextures.Add(Content.Load<Texture2D>("angelfish"));
            fishTextures.Add(Content.Load<Texture2D>("aquafish"));
            fishTextures.Add(Content.Load<Texture2D>("flamefish"));
            fishTextures.Add(Content.Load<Texture2D>("siren"));

            //Loads fishing rod and line texture
            rodTexture = Content.Load<Texture2D>("fishingHook");
            rodLineTexture = Content.Load<Texture2D>("rodLine");

            // Load book texture
            bookTexture = Content.Load<Texture2D>("book");

            //Initialize the background
            bg = new Background(backgroundTexture, windowWidth);

            //initializes fishing rod
            fishingRod = new FishingRod(rodTexture, rodLineTexture, 1000, windowWidth / 2, 1, windowWidth, windowHeight, bg); //TODO: Update the depth and position to the starting depth and position we want, values are just placeholder

            //Initialize the CollectibleManager
            collectibleManager = new CollectibleManager(rng, bg, windowWidth, windowHeight,
                fishTextures, bookTexture, fishingRod);

            //load textures for menu
            menu.Load(GraphicsDevice, Content, collectibleManager);
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
            bg.Update(fishingRod);

            menu.Update(gameTime, fishingRod);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            // TODO: Add your drawing code here

            //Draw the background
            bg.Draw(_spriteBatch);

            //Draw the fishing rod
            fishingRod.Draw(_spriteBatch);

            //Draw the collectibles in the Collectible Manager
            collectibleManager.Draw(_spriteBatch);

            //draw menu
            menu.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}