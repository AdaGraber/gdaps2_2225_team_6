using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fishing
{
    internal class Collectible
    {
        //Parent class for items that can be picked up by the player, specifically Fish and Books

        /* FIELDS AND PROPERTIES */

        protected Random rng;

        protected int speed;

        protected Vector2 position;
        protected Texture2D texture;

        //I'm not sure if this implementation for depth is going to stick,\
        //but it can easily be changed if need be
        protected int minDepth;
        protected int maxDepth;

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

            //Give the fish a 50-50 chance of swimming left or right
            if (rng.Next(2) == 0)
            {
                //Set the speed equal to the given speed
                this.speed = speed;

                //Set the fish's position just out of sight at a random y location within its min and max depth range
                position = new Vector2(-texture.Width, rng.Next(minDepth, maxDepth + 1));
            }
            else
            {
                //Set the speed equal to the negative of the given speed, so that it travels right to left
                this.speed = -speed;

                //Set the fish's position just out of sight at a random y location within its min and max depth range
                position = new Vector2(windowWidth, rng.Next(minDepth, maxDepth + 1));
            }
        }

        /// <summary>
        /// Updates the collectible.
        /// </summary>
        public void Update()
        {
            //Move the fish horizontally across the screen
            position.X += speed;

            //TODO: Remove fish if it leaves the screen
        }

        /// <summary>
        /// Draws the collectible.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            //If the fish is going to the right, draw it normally
            if (speed > 0)
            {
                //This seems messy, but I can't find an overload of Draw() that doesn't have all this extraneous information
                _spriteBatch.Draw(texture, position,
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
            }

            //If the fish is going to the left, draw it flipped
            else
            {
                _spriteBatch.Draw(texture, position,
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.FlipHorizontally, 0);
            }

            //TODO: Add animation
        }
    }
}
