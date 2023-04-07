using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/* Team Tranquil
 * GDAPS 2 Project
 * Class purpose: Parent class for items that can be picked up by the player, specifically Fish and Books
 * 
 * Known issues:
 * 
 */

namespace Fishing
{
    //Delegate for mythical fishes' powers
    internal delegate void AffectedByPower(Fish sender, int index);

    internal class CollectibleManager
    {
        /* FIELDS AND PROPERTIES */

        //Event for handling mythical powers
        public event AffectedByPower UsingPower;

        //Random object
        Random rng;

        //Window width and height
        int windowWidth;
        int windowHeight;

        //Dictionary to hold the data for individual fish species
        Dictionary<string, int[]> fishSpecies = new Dictionary<string, int[]>();

        //List of collectibles
        List<Collectible> collectibles = new List<Collectible>();

        //List of known spells
        List<string> spells = new List<string>();

        //List of textures
        List<Texture2D> fishTextures = new List<Texture2D>();

        // Texture for the books
        Texture2D bookTexture;

        //Reference to fishing rod
        FishingRod fishingRod;

        //Variables for file reading
        StreamReader input = null;
        string fishData = "../../../Content/FishData.txt";

        // TEMPORARY variable to hold skill points until skills are properly implemented
        private int skillPoints = 0;

        // Property for the spells so the player can have access (not sure if needed yet)
        public List<string> Spells
        {
            get => spells;
        }

        // Property for the SP so the player can "see" when the number of points increases
        public int SkillPoints
        {
            get => skillPoints;
        }

        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public CollectibleManager(Random rng, int windowWidth, int windowHeight, List<Texture2D> fishTextures, Texture2D bookTexture, FishingRod fishingRod)
        {
            //Initialize given values
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.rng = rng;
            this.fishTextures = fishTextures;
            this.bookTexture = bookTexture;
            this.fishingRod = fishingRod;

            //Give skillPoints a starting value of 0
            skillPoints = 0;

            //Read the fish data
            ReadFishData();
        }

        /// <summary>
        /// Updates the collectibles.
        /// </summary>
        public void Update()
        { 

            // Spawn the fish
            SpawnFish();

            //Check each collectible
            for (int i = 0; i < collectibles.Count; i++)
            {
                //Update each collectible
                collectibles[i].Update();

                //Check to see if the player has caught a collectible
                if (
                    //If the collectible is caught already
                    collectibles[i].IsCaught
                    //OR if the collectible is overlapping the fishing rod
                    || (collectibles[i].Position.Intersects(fishingRod.Rect)
                    //and the fishing rod does not already have an item
                    && !fishingRod.HasItem
                    //And if the collectible is either a book
                    && ((collectibles[i] is Books)
                    //or a fish whose required skill points are lower than the player's skill points
                    || (collectibles[i] is Fish && skillPoints >= fishSpecies[((Fish)collectibles[i]).Name][4]))))
                {
                    //If the above conditions are met,
                    //set the collectible's position to that of the fishing rod
                    collectibles[i].FollowFishingRod(fishingRod.Rect);

                    //And tell the fishing rod it has an item
                    fishingRod.HasItem = true;
                }

                //If the collectible is a fish and is mythical
                if (collectibles[i] is Fish && ((Fish)collectibles[i]).IsMythical)
                {
                    //Have the fish use its power
                    if (UsingPower != null)
                    {
                        UsingPower((Fish)collectibles[i], i);
                    }
                }

                //Check to see if the collectible has left the screen on the left or right
                if (collectibles[i].Position.X == windowWidth
                    || collectibles[i].Position.X == 0 - collectibles[i].Position.Width)
                {
                    //The collectible should be destroyed
                    collectibles[i].IsDead = true;
                }

                //Check if the collectible is caught and the player made it to the top of the screen with it
                if (collectibles[i].IsCaught && fishingRod.Rect.Y == 0)
                {
                    //If the collectible is a fish
                    if (collectibles[i] is Fish)
                    {
                        //Cast the collectible to a fish
                        Fish currentFish = (Fish)collectibles[i];

                        //For readability purposes, get a reference to the int array for the species of that fish
                        int[] fishData = fishSpecies[currentFish.Name];

                        //Check if the species of fish has never been caught before
                        if (fishData[fishData.Count() - 1] == 0)
                        {
                            //If so, set the fish's caught value to true
                            fishData[fishData.Count() - 1] = 1;
                        }

                        //Otherwise, do nothing
                    }

                    //If the collectible is a book
                    else
                    {
                        // Create a temporary instance of the book collectible
                        Books tempBook = (Books)collectibles[i];

                        if (tempBook.Spell != null) // If the book actually has a spell
                        {
                            spells.Add(tempBook.Spell);
                        }
                        else // If there is no spell then it is a skill book
                        {
                            skillPoints++;
                        }
                    }

                    //Make the fishing rod no longer have an item
                    fishingRod.HasItem = false;

                    //The collectible should be destroyed
                    collectibles[i].IsDead = true;
                }

                //If the collectible is dead
                if (collectibles[i].IsDead)
                {
                    //Remove the caught collectible from the list of collectibles
                    collectibles.RemoveAt(i);

                    //Decrement i since a collectible was removed
                    i--;
                }
            }
        }

        /// <summary>
        /// Reads the data for the fish species from a text file.
        /// </summary>
        public void ReadFishData()
        {
            try
            {
                string line = null;
                string[] lines;

                input = new StreamReader(fishData);

                //Loop through every line in the file
                while ((line = input.ReadLine()) != null)
                {
                    //String for use as dictionary key
                    string key;

                    //Split the line into an array
                    lines = line.Split(',');

                    //Set the key equal to the first piece of data in the file
                    key = lines[0];

                    //Create a new integer array for holding the remaining data
                    int[] fishData = new int[lines.Count() + 1];

                    //Search the entire array, minus the no-longer-useful first piece of data
                    for (int i = 1; i < lines.Count(); i++)
                    {
                        //Put that data into the fish array
                        fishData[i - 1] = int.Parse(lines[i]);
                    }

                    //Add a final value to track whether or not the fish has been caught before --
                    //1 if caught and 0 if not
                    //TODO: Add this value to file if saving is implemented
                    fishData[lines.Count()] = 0;

                    //Add the fish to the list of species
                    fishSpecies.Add(key, fishData);
                }
            }

            //Catch any exceptions
            catch (Exception e)
            {
                //Print the error to the debugger for now
                Console.WriteLine(e.Message);
            }

            //If the file was opened, close it
            if (input != null)
            {
                input.Close();
            }
        }

        /// <summary>
        /// Has a random chance of spawning each species of fish every frame.
        /// </summary>
        public void SpawnFish()
        {
            int fishTextureIndex = 0;

            //Check each fish in the dictionary
            foreach (KeyValuePair<string, int[]> n in fishSpecies)
            {
                //Set the texture of the fish to the texture stored at the proper index
                Texture2D fishTexture = fishTextures[fishTextureIndex];

                //TODO: Will add implementation for depth later

                //Get the chance of the fish spawning
                int spawnChance = n.Value[0];

                //Give the fish a spawnChance-in-1000 chance of spawning -- the higher the spawnChance,
                //the more likely it is to spawn
                if (rng.Next(1001) <= spawnChance)
                {
                    //Create a new fish using the data in the array in the dictionary
                    Fish newFish = new Fish(n.Key, n.Value[1], fishTexture, n.Value[2], n.Value[3],
                        windowWidth, windowHeight, rng);

                    if (!newFish.IsMythical)
                    {
                        UsingPower += newFish.PowerEffect;
                    }

                    // Add that fish to the list
                    collectibles.Add(newFish);
                }

                //Add one to the index
                fishTextureIndex++;
            }
        }

        /// <summary>
        /// Draw the collectibles.
        /// </summary>
        /// <param name="_spriteBatch">The SpriteBatch.</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            //Draw every fish
            foreach (Collectible n in collectibles)
            {
                n.Draw(_spriteBatch);
            }
        }
    }
}
