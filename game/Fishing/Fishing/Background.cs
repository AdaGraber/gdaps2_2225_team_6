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
        private int windowHeight;

        public Rectangle Position
        {
            get { return position; }
        }

        public int TextureHeight
        {
            //Note: Texture height is currently 1080, which is the maximum height of the level
            get { return texture.Height; }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        public Background (Texture2D texture, int windowWidth, int windowHeight)
        {
            this.texture = texture;
            this.windowHeight = windowHeight;

            position = new Rectangle(0, 0, windowWidth, texture.Height);
        }

        /// <summary>
        /// Moves the background in accordance with player movement.
        /// </summary>
        public void Update(FishingRod fishingRod)
        {
            //As long as the fishing rod isn't at the very top of the level
            if (fishingRod.CurrentDepth > windowHeight / 2)
            {
                //Set the position to the negative of the current depth
                //(negative because background needs to move up as player moves down and vice versa)
                position.Y = -fishingRod.CurrentDepth + windowHeight / 2;
            }

            //If the fishing rod IS at the top of the level, keep the position at y = 0
            else
            {
                position.Y = 0;
            }
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
