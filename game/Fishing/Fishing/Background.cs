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
        private Color backgroundColor;
        private Texture2D texture;

        //Window dimensions
        private int windowWidth;
        private int windowHeight;

        //Fishing rod reference
        private FishingRod fishingRod;


        public Rectangle Position
        {
            get { return position; }
        }

        //Gets the X value the player is at on the background --
        //is calculated based on background color
        public int X
        {
            get
            {
                //0 is minimum
                //329 is maximum (178 = 0)

                return backgroundColor.R + backgroundColor.G + backgroundColor.B - 178 + 600; //TODO: this is messy
                //507 is maximum

                //178 is minimum

            }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        public Background (Texture2D texture, int windowWidth, int windowHeight)
        {
            this.texture = texture;

            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            position = new Rectangle(0, 0, windowWidth, 1000);
            backgroundColor = new Color(87, 165, 255);

        }

        /// <summary>
        /// Updates the background color of the screen. Commented out due to incompleteness.
        /// </summary>
        public void Update(FishingRod fishingRod)
        {
            //If the fishing rod is at the bottom of the window, is not below its max depth,
            //and is going down
            if (fishingRod.Rect.Y >= windowHeight - fishingRod.Rect.Height
                && fishingRod.CurrentDepth < fishingRod.MaxDepth
                && fishingRod.PlayerDirection == Direction.Down)
            {
                //As long as the background red color is not too dark, subtract from it
                if (backgroundColor.R > 0)
                {
                    backgroundColor.R--;
                }

                //As long as the background green color is not too dark, subtract from it
                if (backgroundColor.G > 78)
                {
                    backgroundColor.G--;
                }

                //As long as the background blue color is not too dark, subtract from it
                if (backgroundColor.B > 100)
                {
                    backgroundColor.B--;
                }
            }

            //If the fishing rod is at the top of the window, its current depth is not above zero,
            //and is going up
            else if (fishingRod.Rect.Y <= 0
                && fishingRod.CurrentDepth > 0
                && fishingRod.PlayerDirection == Direction.Up)
            {
                //As long as the background red color is not too bright, add to it
                if (backgroundColor.R < 87)
                {
                    backgroundColor.R++;
                }

                //As long as the background green color is not too bright, add to it
                if (backgroundColor.G < 165)
                {
                    backgroundColor.G++;
                }

                //As long as the background blue color is not too bright, add to it
                if (backgroundColor.B < 255)
                {
                    backgroundColor.B++;
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, position, backgroundColor);
        }
    }
}
