using System;
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

        // Window width
        int windowWidth;

        //Fish information
        private string name;
        private bool isMythical;

        // Fish direction (for use with some book handling) (true is right, false is left)
        private bool direction;

        //Texture for special effect on siren
        Texture2D sirenEffectTexture;

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

        public bool Direction
        {
            get { return direction; }

            //Get-only property
        }

        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor, pass neccessary parameters to parent class
        public Fish(string name, int speed, Texture2D texture, Texture2D sirenEffectTexture, int minDepth, int maxDepth, int windowWidth, int windowHeight, Random rng, Background bg)
            : base(speed, texture, minDepth, maxDepth, windowWidth, windowHeight, rng, bg)
        {
            //Initialize name field
            this.name = name;

            //Initialize siren effect
            this.sirenEffectTexture = sirenEffectTexture;

            //If the fish is mythical, set isMythical to true
            if (name == "siren")
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
                if (sender.Position.Y + sender.texture.Height > position.Y
                    && sender.Position.Y < position.Y)
                {
                    //The fish is affected by the siren power
                    affectedByPower = true;

                    //Slow the fish down if swimming away from the siren,
                    //or speed it up if swimming towards it
                    if (sender.Position.X > position.X)
                    {
                        position.X += speed - 1;
                    }
                    else
                    {
                        position.X -= speed - 1;
                    }
                }

                //If the fish's position intersects that of the siren
                if (position.Intersects(sender.Position))
                {
                    //The fish should be removed from play
                    isDead = true;
                }

                if (sender.isDead)
                {
                    affectedByPower = false;
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

                direction = true;
            }

            //If the fish is going to the left, draw it flipped
            else
            {
                _spriteBatch.Draw(texture, new Vector2(position.X, position.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);

                direction = false;
            }

            //If the fish is a siren
            if (name == "siren")
            {
                //Add the special siren effects for the width of the whole window
                for (int i = 1; i < windowWidth; i++)
                {
                    //Draw the effects going to the right
                    _spriteBatch.Draw(sirenEffectTexture, new Vector2(position.X + 200 * i, position.Y),
                        new Rectangle(0, 0, sirenEffectTexture.Width, sirenEffectTexture.Height),
                        Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

                    //Draw the effects going to the left
                    _spriteBatch.Draw(sirenEffectTexture, new Vector2(position.X - 200 * i, position.Y),
                        new Rectangle(0, 0, sirenEffectTexture.Width, sirenEffectTexture.Height),
                        Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);

                }
            }
        }
    }
}
