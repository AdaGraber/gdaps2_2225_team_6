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
 * 
 */

namespace Fishing
{
    internal class Fish : Collectible
    {
        /* FIELDS AND PROPERTIES */

        private string name;

        //Not very many properties yet until I know what information the rest of the code will need from Fish

        public string Name
        {
            get { return name; }

            //Get-only property
        }


        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor, pass neccessary parameters to parent class
        public Fish(string name, int speed, Texture2D texture, int minDepth, int maxDepth, int windowWidth, int windowHeight, Random rng) 
            : base(speed, texture, minDepth, maxDepth, windowWidth, windowHeight, rng)
        {
            // Initialize name field
            this.name = name;
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
