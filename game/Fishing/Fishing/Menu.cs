using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Fishing
{
    internal class Menu
    {
        //menu textures
        Texture2D outerShade;
        Texture2D menuFrame;
        Texture2D menuOutline;

        //save button
        Button saveButton;
        Texture2D saveBtnTexture;

        //stats button
        Button statsButton;
        Texture2D statsBtnTexture;

        //achievements button
        Button achieveButton;
        Texture2D achieveBtnTexture;

        //quit button
        Button quitButton;
        Texture2D quitBtnTexture;

        //header
        SpriteFont menuHeader;

        private bool open = false;

        private int windowWidth;
        private int windowHeight;
        private List<Button> buttonList;

        public List<Button> Buttons { get { return buttonList; } }
        public bool Open { get; set; }

        public Menu(int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            //background shade
            outerShade = new Texture2D(graphicsDevice, 1, 1);
            outerShade.SetData(new[] { Color.Gray });

            //frame
            menuFrame = new Texture2D(graphicsDevice, 1, 1);
            menuFrame.SetData(new[] { Color.BurlyWood });

            //frame outline
            menuOutline = new Texture2D(graphicsDevice, 1, 1);
            menuOutline.SetData(new[] { Color.Chocolate });

            //menu header
            menuHeader = content.Load<SpriteFont>("Header");

            //saveButton texture
            saveBtnTexture = content.Load<Texture2D>("longButton");

            //statsButton texture
            statsBtnTexture = content.Load<Texture2D>("shortButton");

            //achieveButton texture
            achieveBtnTexture = content.Load<Texture2D>("shortButton");

            //quit button
            quitBtnTexture = content.Load<Texture2D>("longButton");


            /* BUTTON HANDLER */
            //save button
            saveButton = new Button(graphicsDevice, saveBtnTexture, content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Save Game",
            };
            saveButton.Position = new Vector2(windowWidth / 2 - saveButton.Rectangle.Width / 2, (windowHeight / 2 - saveButton.Rectangle.Height / 2)-100);
            saveButton.Click += SaveButtonClick;

            //stats button
            statsButton = new Button(graphicsDevice, statsBtnTexture, content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Stats",
            };
            statsButton.Position = new Vector2(windowWidth / 2 - saveButton.Rectangle.Width / 2, (windowHeight / 2 - saveButton.Rectangle.Height / 2) - 50); //uses saveButton to cut in half while staying at the same point

            //achieve button
            achieveButton = new Button(graphicsDevice, achieveBtnTexture, content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Achievement",
            };
            achieveButton.Position = new Vector2(statsButton.Rectangle.Right+30, (windowHeight / 2 - saveButton.Rectangle.Height / 2) - 50);

            //quit button
            quitButton = new Button(graphicsDevice, quitBtnTexture, content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Quit",
            };
            quitButton.Position = new Vector2(windowWidth / 2 - quitButton.Rectangle.Width / 2, (windowHeight / 2 - quitButton.Rectangle.Height / 2) + 100);
            quitButton.Click += QuitButtonClick;

            buttonList = new List<Button>()
            {
                saveButton,
                statsButton,
                achieveButton,
                quitButton,
            };
        }
        public void Update(GameTime gameTime)
        {
            foreach(Button button in buttonList)
            {
                button.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(outerShade, new Rectangle(0, 0, windowWidth, windowHeight), Color.White * 0.5f);

            spriteBatch.Draw(menuOutline, new Rectangle((windowWidth / 2 - 307), (windowHeight / 2 - 207), 614, 414), Color.White);
            spriteBatch.Draw(menuFrame, new Rectangle((windowWidth/2 - 300), (windowHeight/2 - 200), 600, 400), Color.White);

            for(int i = 0 ; i < buttonList.Count ; i++)
            {     
                buttonList[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.DrawString(menuHeader, "Menu", new Vector2((windowWidth / 2 - menuHeader.MeasureString("Menu").X / 2), (windowHeight / 2 - 180)), Color.Black);
        }

        private void SaveButtonClick(object sender, System.EventArgs e)
        {
            saveButton.Text = "Saved!";
        }

        private void QuitButtonClick(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
