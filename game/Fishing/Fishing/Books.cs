using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Team Tranquil
 * GDAPS 2 Project
 * Class purpose: Create skill and spell books for the player to catch
 * 
 * Known issues:
 * 
 */

namespace Fishing
{
    internal class Books : Collectible
    {
        //Books are collectibles that increase the player's skill points, which can be used to upgrade the fishing rod

        //TODO: Add book effects(discuss with team how we may want to do this)
        //TODO cont: do we want our methods to add certain effects?

        /* FIELDS AND PROPERTIES */

        private string spell; // This field will be null if it is a skill book, a spell name if it is a spell book

        // For books that follow fish it will need a reference to said fish
        private Fish fish;

        public string Spell
        {
            get { return spell; }
        }

        // Paramaterized constructor for books (if it is a skill book the spell value will be null)
        public Books(string spell, int speed, Texture2D texture, int minDepth, int maxDepth, int windowWidth, int windowHeight, Random rng, Fish fish)
            : base(speed, texture, minDepth, maxDepth, windowWidth, windowHeight, rng)
        {
            if(spell == "skill")
            {
                this.spell = null;
            }
            else
            {
                this.spell = spell;
            }

            this.fish = fish;
        }

        public override void Update()
        {
            // If the book doesn't have a fish to follow
            if(fish == null)
            {
                base.Update();
            }
            else // If it needs to follow a fish
            {
                // Will stay at the same position as the fish + half the width or the height (to stay underneath it)
                position.X = fish.Position.X + fish.Position.Width/2;
                position.Y = fish.Position.Y + fish.Position.Height;

                if (fish.IsDead)
                {
                    this.isDead = true;
                }
            }
        }
    }
}
