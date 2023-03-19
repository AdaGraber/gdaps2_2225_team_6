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
    }
}
