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

        public string Spell
        {
            get { return spell; }
        }

        // Paramaterized constructor for books (if it is a skill book the spell value will be null)
        public Books(string spell, int speed, Texture2D texture, int minDepth, int maxDepth, int windowWidth, int windowHeight, Random rng)
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
        }
    }
}
