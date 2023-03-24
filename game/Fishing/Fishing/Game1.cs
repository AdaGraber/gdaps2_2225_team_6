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

        //Textures
        List<Texture2D> fishTextures;
        Texture2D rodTexture;

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

            //Initialize Random object
            rng = new Random();

            //Initialize the list of fish textures
            fishTextures = new List<Texture2D>();

            //Initialize menu
            menu = new Menu(windowWidth, windowHeight);

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

            //Loads fishing rod texture
            rodTexture = Content.Load<Texture2D>("rodPH");

            //initializes fishing rod
            fishingRod = new FishingRod(rodTexture, 100, new Vector2(windowWidth / 2, 0)); //TODO: Update the depth and position to the starting depth and position we want, values are just placeholder

            //Initialize the CollectibleManager
            collectibleManager = new CollectibleManager(rng, windowWidth, windowHeight,
                fishTextures, fishingRod);

            /* MENU TEXTURES */

            //load texture for menu button
            Button menuButton = new Button(Content.Load<Texture2D>("menuButton"), Content.Load<SpriteFont>("Font"))
            {
                Position = new Vector2(10, 10),
                Text = "",
            };
            menuButton.Click += MenuButtonClick;


            buttonList = new List<Button>()
            {
                menuButton,
            };

            menu.Load(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach(Button btn in buttonList)
            {
                btn.Update(gameTime);
            }

            menu.Update(gameTime);

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


            //buttons
            foreach (Button btn in buttonList)
            {
                btn.Draw(gameTime, _spriteBatch);
            }

            if (menu.Open)
            {
                menu.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void MenuButtonClick(object sender, System.EventArgs e)
        {
            if(menu.Open)
            {
                menu.Open = false;
            }
            else
            {
                menu.Open = true;
            }
        }
    }
}