using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
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

        //Random object
        protected Random rng;

        //Background
        Background bg;

        //Rectangle and texture
        protected Rectangle position;
        protected Texture2D texture;

        //Stats
        protected int speed;
        protected int spawnDepth;
        protected int minDepth;
        protected int maxDepth;
        protected bool isCaught;

        //Boolean to allow collectible to recognize if it should be removed from play
        protected bool isDead;

        //Whether or not the collectible is being affected by a mythical power
        protected bool affectedByPower;

        public Rectangle Position
        {
            get { return position; }

            set { value = position; }
        }

        public bool IsCaught
        {
            get { return isCaught; }

            set { isCaught = value; }
        }

        public bool IsDead
        {
            get { return isDead; }

            //Should not be able to bring a dead collectible back to life
            set { isDead = true; }
        }

        public bool AffectedByPower
        {
            get { return affectedByPower; }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public Collectible(int speed, Texture2D texture, int minDepth, int maxDepth,
            int windowWidth, int windowHeight, Random rng, Background bg)
        {
            // Initializing fields
            this.bg = bg;
            this.rng = rng;
            this.texture = texture;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;

            spawnDepth = rng.Next(minDepth, maxDepth + 1);

            //Give the collectible a 50-50 chance of moving left or right
            if (rng.Next(2) == 0)
            {
                //Set the speed equal to the given speed
                this.speed = speed;

                //Set the collectible's position just out of sight at a random y location within its min and max depth range
                position = new Rectangle(-texture.Width, spawnDepth, texture.Width, texture.Height);
            }
            else
            {
                //Set the speed equal to the negative of the given speed, so that it travels right to left
                this.speed = -speed;

                //Set the collectible's position just out of sight at a random y location within its min and max depth range
                position = new Rectangle(windowWidth, spawnDepth, texture.Width, texture.Height);
            }
        }

        /// <summary>
        /// Catches the fish.
        /// </summary>
        /// <param name="fishingRodPosition">The position of the fishing rod.</param>
        public void FollowFishingRod(Rectangle fishingRodPosition)
        {
            //Set position equal to that of the fishing rod
            position = fishingRodPosition;

            //Set isCaught to true
            isCaught = true;
        }

        /// <summary>
        /// Updates the collectible.
        /// </summary>
        public virtual void Update()
        {
            //If the fishing rod does not have the fish
            if (!isCaught)
            {
                //Move the collectible horizontally across the screen
                position.X += speed;

                //Move the collectible with the background
                position.Y = spawnDepth + bg.Position.Y;
            }

            //If the fish is caught
            else
            {
                //Update depth so if the fish is stolen by a siren, it will stay at the same depth
                spawnDepth = position.Y - bg.Position.Y;
            }
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
