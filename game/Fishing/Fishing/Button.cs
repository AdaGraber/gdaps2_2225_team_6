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
        public ColorWriteChannels PenColor { get; private set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }
        public string Text { get; set; }


    }
}
