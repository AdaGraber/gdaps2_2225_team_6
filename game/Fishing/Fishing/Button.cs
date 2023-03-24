using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing
{
    internal class Button
    {
        /* FIELDS AND PROPERTIES */

        private MouseState mState;
        private MouseState prevMState;

        private SpriteFont font;
        private Texture2D texture;

        private bool isHovering;

        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColor { get; private set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }
        public string Text { get; set; }

        /* CONSTRUCTOR AND METHODS */
        public Button(Texture2D texture, SpriteFont font)
        {
            this.texture = texture;
            this.font = font;

            PenColor = Color.Black;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle rect)
        {
            Color color = Color.White;

            if(isHovering)
            {
                color = Color.Gray;
            }
            
            if(rect != null)
            {
                spriteBatch.Draw(texture, rect, color);
            }
            else
            {
                spriteBatch.Draw(texture, Rectangle, color);
            }
            

            if(!string.IsNullOrEmpty(Text))
            {
                float x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                float y = (Rectangle.Y + (Rectangle.Width / 2)) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x,y), PenColor);
            }
        }

        public void Update(GameTime gameTime)
        {
            prevMState = mState;
            mState = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(mState.X, mState.Y, 1, 1);

            isHovering = false;

            if(mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if(mState.LeftButton == ButtonState.Released && prevMState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

    }
}
