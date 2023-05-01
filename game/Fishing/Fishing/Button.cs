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

        Color mainColor = Color.White;
        Color hoverColor = Color.Gray;
        Color outlineColor;

        private SpriteFont font;
        private Texture2D texture;
        private Texture2D outline;

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
        //overload for colors
        public Button(GraphicsDevice graphicsDevice, Texture2D texture, SpriteFont font, Color mainColor, Color hoverColor)
        {
            this.texture = texture;
            this.font = font;

            this.mainColor = mainColor;
            this.hoverColor = hoverColor;

            PenColor = Color.Black;
        }
        public Button(GraphicsDevice graphicsDevice, Texture2D texture, SpriteFont font, Color mainColor, Color hoverColor, Color outlineColor)
        {
            this.texture = texture;
            this.font = font;

            this.mainColor = mainColor;
            this.hoverColor = hoverColor;
            this.outlineColor = outlineColor;

            outline = new Texture2D(graphicsDevice, 1, 1);
            outline.SetData(new[] { outlineColor });

            PenColor = Color.Black;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = mainColor;

            if(isHovering)
            {
                color = hoverColor;
            }

            if(outline != null)
            {
                spriteBatch.Draw(outline, new Rectangle((int)Position.X - 3, (int)Position.Y - 3, Rectangle.Width + 6, Rectangle.Height + 6), Color.White);
            }
            spriteBatch.Draw(texture, Rectangle, color);
            
            if(!string.IsNullOrEmpty(Text))
            {
                float x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                float y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);

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
