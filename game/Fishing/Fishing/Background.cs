using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/* Team Tranquil
 * GDAPS 2 Project
 * Class purpose: Create a background and coordinate system for the level
 * 
 * Known issues:
 * 
 */

namespace Fishing
{
    internal class Background
    {
        /* FIELDS AND PROPERTIES */

        //Background information
        private Rectangle position;
        private Texture2D texture;

        public Rectangle Position
        {
            get { return position; }
        }

        public int TextureHeight
        {
            get { return texture.Height; }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        public Background (Texture2D texture, int windowWidth)
        {
            this.texture = texture;

            position = new Rectangle(0, 0, windowWidth, texture.Height);
        }

        /// <summary>
        /// Moves the background in accordance with player movement.
        /// </summary>
        public void Update(FishingRod fishingRod)
        {
            position.Y = -fishingRod.CurrentDepth;
        }

        /// <summary>
        /// Draws the background.
        /// </summary>
        /// <param name="_spriteBatch">The SpriteBatch.</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
