﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
 */

namespace Fishing
{
    internal class Fish : Collectible
    {
        /* FIELDS AND PROPERTIES */

        int windowWidth;

        private string name;
        private bool isMythical;

        public string Name
        {
            get { return name; }

            //Get-only property
        }

        public bool IsMythical
        {
            get { return isMythical; }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor, pass neccessary parameters to parent class
        public Fish(string name, int speed, Texture2D texture, int minDepth, int maxDepth, int windowWidth, int windowHeight, Random rng) 
            : base(speed, texture, minDepth, maxDepth, windowWidth, windowHeight, rng)
        {
            //Initialize name field
            this.name = name;

            //If the fish is mythical, set isMythical to true
            if (name == "siren" /*|| add any other mythical creature names here*/)
            {
                this.isMythical = true;
            }

            //Otherwise, set isMythical to false
            else
            {
                this.isMythical = false;
            }

            //Initialize the window width
            this.windowWidth = windowWidth;
        }

        /// <summary>
        /// Causes a mythical fish's powers to affect fish.
        /// </summary>
        /// <param name="sender">The mythical fish that used its powers.</param>
        public void PowerEffect(Fish sender, int index)
        {
            //If the one using the power is a siren
            if (sender.Name == "siren")
            {
                //If the fish is within range of the siren
                if (sender.Position.Y > position.Y - 5 && sender.Position.Y < position.Y + 5)
                {
                    //The fish is no longer caught
                    isCaught = false;

                    //Move the fish towards the siren
                    if (sender.Position.X > position.X)
                    {
                        position.X++;
                    }
                    else if (sender.Position.X < position.X)
                    {
                        position.X--;
                    }
                }
            }
        }

        public override void Update()
        {
            //If the fish isn't mythical
            if (!isMythical)
            {
                //Update like normal
                base.Update();
            }

            //If the fish is a siren
            else if (name == "siren")
            {
                //Move only if not in the correct position on the edge of the screen
                if (position.X < 20)
                {
                    position.X++;
                }
                else if (position.X > windowWidth - 20)
                {
                    position.X--;
                }

            }
        }

        /// <summary>
        /// Override of Collectible's Draw that draws the fish.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch.</param>
        public override void Draw(SpriteBatch _spriteBatch)
        {
            //If the fish is going to the right, draw it normally
            if (speed > 0)
            {
                _spriteBatch.Draw(texture, new Vector2(position.X, position.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }

            //If the fish is going to the left, draw it flipped
            else
            {
                _spriteBatch.Draw(texture, new Vector2(position.X, position.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
            }

            //TODO: Add animation

        }
    }
}
