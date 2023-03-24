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

        Texture2D saveBtnTexture;

        SpriteFont menuHeader;

        private bool open = false;

        private int windowWidth;
        private int windowHeight;
        private List<Button> buttonList;

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
            saveBtnTexture = new Texture2D(graphicsDevice, 1, 1);
            saveBtnTexture.SetData(new[] { Color.Bisque });

            //button handler
            Button saveButton = new Button(saveBtnTexture, content.Load<SpriteFont>("Font"))
            {
                Rectangle = new Rectangle(0, 0, 500, 20),
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
            foreach(Button btn in buttonList)
            {
                btn.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(outerShade, new Rectangle(0, 0, windowWidth, windowHeight), Color.White * 0.5f);

            spriteBatch.Draw(menuOutline, new Rectangle((windowWidth / 2 - 307), (windowHeight / 2 - 207), 614, 414), Color.White);
            spriteBatch.Draw(menuFrame, new Rectangle((windowWidth/2 - 300), (windowHeight/2 - 200), 600, 400), Color.White);

            foreach(Button btn in buttonList)
            {
                btn.Draw(gameTime, spriteBatch);
            }

            spriteBatch.DrawString(menuHeader, "Menu", new Vector2((windowWidth / 2 - menuHeader.MeasureString("Menu").X / 2), (windowHeight / 2 - 180)), Color.Black);
           
        }

        private void SaveButtonClick(object sender, System.EventArgs e)
        {

        }
    }
}
