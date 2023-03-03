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
 * Class purpose: Create fish for the player to catch
 * 
 * Known issues:
 * - very incomplete
 * - implementation that exists is very messy and disorganized -- it will be improved later
 */

namespace Fishing
{
    internal class Fish : Collectible
    {
        /* FIELDS AND PROPERTIES */

        //If I had to guess, I'd say all of the fields and methods so far will likely be shared in some form by Book
        //and moved to Collectible eventually

        private Random rng;

        private string name;
        private int speed;

        private Vector2 position;
        private Texture2D texture;

        //I'm not sure if this implementation for depth is going to stick,\
        //but it can easily be changed if need be
        private int minDepth;
        private int maxDepth;

        //Not very many properties yet until I know what information the rest of the code will need from Fish

        public string Name
        {
            get { return name; }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public Fish(string name, int speed, Texture2D texture, int minDepth, int maxDepth,
            int windowWidth, int windowHeight, Random rng)
        {
            // Initialize fields
            this.name = name;
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
        /// Updates the fish.
        /// </summary>
        public void Update()
        {
            //Move the fish horizontally across the screen
            position.X += speed;

            //TODO: Remove fish if it leaves the screen
        }

        /// <summary>
        /// Draws the fish.
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
                    new Rectangle(0,0, texture.Width, texture.Height),
                    Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.FlipHorizontally, 0);
            }

            //TODO: Add animation
        }

    }
}
