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
        Texture2D saveBtnOutline;

        //header
        SpriteFont menuHeader;

        private bool open = false;

        private int windowWidth;
        private int windowHeight;
        private List<Button> buttonList;
        private List<Texture2D> buttonOutlines;

        public List<Button> Buttons { get { return buttonList; } }
        public bool Open { get; set; }

        public Menu(int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            buttonOutlines= new List<Texture2D>();
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
            saveBtnTexture = new Texture2D(graphicsDevice, 1, 1);
            saveBtnTexture.SetData(new[] { Color.Bisque });
            saveBtnOutline = new Texture2D(graphicsDevice, 1, 1);
            saveBtnOutline.SetData(new[] { Color.Peru });
            buttonOutlines.Add(saveBtnOutline);


            //button handler
            saveButton = new Button(saveBtnTexture, content.Load<SpriteFont>("Font"))
            {
                Text = "Save Game",
            };
            saveButton.Position = new Vector2(windowWidth / 2 - saveButton.Rectangle.Width / 2, (windowHeight / 2 - saveButton.Rectangle.Height / 2)-100);

            saveButton.Click += SaveButtonClick;


            buttonList = new List<Button>()
            {
                saveButton,
            };
        }
        public void Update(GameTime gameTime)
        {
            saveButton.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(outerShade, new Rectangle(0, 0, windowWidth, windowHeight), Color.White * 0.5f);

            spriteBatch.Draw(menuOutline, new Rectangle((windowWidth / 2 - 307), (windowHeight / 2 - 207), 614, 414), Color.White);
            spriteBatch.Draw(menuFrame, new Rectangle((windowWidth/2 - 300), (windowHeight/2 - 200), 600, 400), Color.White);

            int sizeX = 500;
            int sizeY = 40;

            foreach (Texture2D outline in buttonOutlines)
            {
                sizeX = 508;
                sizeY = 48;
                spriteBatch.Draw(outline, new Rectangle(windowWidth / 2 - sizeX / 2, (windowHeight / 2 - sizeY / 2) - 100, sizeX, sizeY), Color.White);
            }

            foreach(Button btn in buttonList)
            {
                sizeX = 500;
                sizeY = 40;
                btn.Draw(gameTime, spriteBatch, new Rectangle(windowWidth / 2 - sizeX / 2, (windowHeight / 2 - sizeY / 2) - 100, sizeX, sizeY));
            }

            spriteBatch.DrawString(menuHeader, "Menu", new Vector2((windowWidth / 2 - menuHeader.MeasureString("Menu").X / 2), (windowHeight / 2 - 180)), Color.Black);
           
        }

        private void SaveButtonClick(object sender, System.EventArgs e)
        {
            saveButton.Text = "Saved!";
        }
    }
}
