using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Team Tranquil
 * GDAPS 2 Project
 * Class purpose: Parent class for items that can be picked up by the player, specifically Fish and Books
 * 
 * Known issues:
 * 
 */

namespace Fishing
{
    internal class Collectible
    {
        /* FIELDS AND PROPERTIES */

        protected Random rng;

        protected int speed;

        protected Rectangle position;
        protected Texture2D texture;

        //I'm not sure if this implementation for depth is going to stick,
        //but it can easily be changed if need be
        protected int minDepth;
        protected int maxDepth;

        protected bool isCaught;

        public Rectangle Position
        {
            get { return position; }
        }

        public bool IsCaught
        {
            get { return isCaught; }
        }


        public Collectible (int speed, Texture2D texture, int minDepth, int maxDepth,
            int windowWidth, int windowHeight, Random rng)
        {
            // Initiallizing fields
            this.texture = texture;
            this.rng = rng;
            this.minDepth = minDepth;

            //Error handling in case maxDepth's value is invalid
            if (maxDepth > minDepth)
            {
                this.maxDepth = maxDepth;
            }
            //If it is, set the maxDepth to 10000 as a placeholder
            else
            {
                maxDepth = 10000;
            }

            //Give the collectible a 50-50 chance of moving left or right
            if (rng.Next(2) == 0)
            {
                //Set the speed equal to the given speed
                this.speed = speed;

                //Set the collectible's position just out of sight at a random y location within its min and max depth range
                position = new Rectangle(-texture.Width, rng.Next(minDepth, maxDepth + 1), texture.Width, texture.Height);
            }
            else
            {
                //Set the speed equal to the negative of the given speed, so that it travels right to left
                this.speed = -speed;

                //Set the collectible's position just out of sight at a random y location within its min and max depth range
                position = new Rectangle(windowWidth, rng.Next(minDepth, maxDepth + 1), texture.Width, texture.Height);
            }
        }

        /// <summary>
        /// Catches the fish by setting its position equal to that of the fishing rod.
        /// </summary>
        /// <param name="fishingRodPosition">The position of the fishing rod.</param>
        public void Catch(Rectangle fishingRodPosition)
        {
            position = fishingRodPosition;
            isCaught = true;
        }

        /// <summary>
        /// Updates the collectible.
        /// </summary>
        public void Update()
        {
            if (!isCaught)
            {
                //Move the collectible horizontally across the screen
                position.X += speed;
            }

            //TODO: Remove collectible if it leaves the screen
        }

        /// <summary>
        /// Draws the collectible.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch.</param>
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
