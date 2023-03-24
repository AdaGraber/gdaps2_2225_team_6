using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Content;

namespace Fishing
{
    internal class Menu
    {
        //menu textures
        Texture2D outerShade;
        Texture2D menuFrame;
        Texture2D menuOutline;

        SpriteFont menuHeader;

        private bool open = false;

        private int windowWidth;
        private int windowHeight;

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
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(outerShade, new Rectangle(0, 0, windowWidth, windowHeight), Color.White * 0.5f);

            spriteBatch.Draw(menuOutline, new Rectangle((windowWidth / 2 - 307), (windowHeight / 2 - 207), 614, 414), Color.White);
            spriteBatch.Draw(menuFrame, new Rectangle((windowWidth/2 - 300), (windowHeight/2 - 200), 600, 400), Color.White);

            spriteBatch.DrawString(menuHeader, "Menu", new Vector2((windowWidth / 2 - menuHeader.MeasureString("Menu").X / 2), (windowHeight / 2 - 180)), Color.Black);
           
        }
    }
}
