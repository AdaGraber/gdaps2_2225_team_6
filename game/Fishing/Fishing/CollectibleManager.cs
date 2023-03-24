using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

/* Team Tranquil
 * GDAPS 2 Project
 * Class purpose: Parent class for items that can be picked up by the player, specifically Fish and Books
 * 
 * Known issues:
 * 
 */

namespace Fishing
{
    public delegate void OnCollision();

    internal class CollectibleManager
    {
        /* FIELDS AND PROPERTIES */

        //Random object
        Random rng;

        //Window width and height
        int windowWidth;
        int windowHeight;

        //Dictionary to hold the data for individual fish species
        Dictionary<string, int[]> fishSpecies = new Dictionary<string, int[]>();

        //List of fish
        List<Fish> collectibles = new List<Fish>();

        //List of textures
        List<Texture2D> fishTextures = new List<Texture2D>();

        //Reference to fishing rod
        FishingRod fishingRod;

        //Variables for file reading
        StreamReader input = null;
        string fishData = "../../../Content/FishData.txt";

        
        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public CollectibleManager(Random rng, int windowWidth, int windowHeight, List<Texture2D> fishTextures, FishingRod fishingRod)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.rng = rng;
            this.fishTextures = fishTextures;
            this.fishingRod = fishingRod;

            //Read the fish data
            ReadFishData();
        }

        /// <summary>
        /// Updates the collectibles.
        /// </summary>
        public void Update()
        { 
            SpawnFish();

            //Check each collectible
            for (int i = 0; i < collectibles.Count; i++)
            {
                //Update each collectible
                collectibles[i].Update();

                //Check to see if the collectible has left the screen on the left or right
                if (collectibles[i].Position.X == windowWidth
                    || collectibles[i].Position.X == 0 - collectibles[i].Position.Width)
                {
                    //Remove the collectible from the list of collectibles to improve performance
                    collectibles.Remove(collectibles[i]);

                    //Decrement i since a collectible was removed
                    i--;

                }

                //Check to see if the player has caught a collectible
                if (collectibles[i].Position.Intersects(fishingRod.Rect) && !fishingRod.HasItem)
                {
                    //If so, tell the collectible to be caught
                    collectibles[i].Catch(fishingRod.Rect);

                    //Tell the fishing rod it has an item
                    fishingRod.HasItem = true;
                }

                //Check if the collectible is caught and the player made it to the top of the screen with it
                if (collectibles[i].IsCaught && fishingRod.Rect.Y == 0)
                {
                    //If the collectible is a fish
                    if (collectibles[i] is Fish)
                    {
                        //For readability purposes, get a reference to the int array for the species of that fish
                        int[] fishData = fishSpecies[collectibles[i].Name];

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

                    }

                    //Make the fishing rod no longer have an item
                    fishingRod.HasItem = false;

                    //Remove the caught collectible from the list of collectibles
                    collectibles.Remove(collectibles[i]);

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
                    collectibles.Add(new Fish(n.Key, n.Value[1], fishTexture, n.Value[2], n.Value[3],
                        windowWidth, windowHeight, rng));
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
