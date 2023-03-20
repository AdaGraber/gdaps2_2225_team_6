using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Fishing
{
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
        List<Fish> fishes = new List<Fish>();

        //List of textures
        List<Texture2D> fishTextures = new List<Texture2D>();

        //Variables for file reading
        StreamReader input = null;
        string fishData = "../../../Content/FishData.txt";

        
        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public CollectibleManager(Random rng, int windowWidth, int windowHeight, List<Texture2D> fishTextures)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.rng = rng;
            this.fishTextures = fishTextures;

            //Not sure if you're supposed to call methods in constructors or not -- may change
            ReadFishData();
        }

        /// <summary>
        /// Updates the collectibles.
        /// </summary>
        public void Update()
        { 
            SpawnFish();

            //Update each fish
            foreach (Fish n in fishes)
            {
                n.Update();
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
                    int[] fishData = new int[lines.Count() - 1];

                    //Search the entire array, minus the no-longer-useful first piece of data
                    for (int i = 1; i < lines.Count(); i++)
                    {
                        //Put that data into the fish array
                        fishData[i - 1] = int.Parse(lines[i]);
                    }

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
                    fishes.Add(new Fish(n.Key, n.Value[1], fishTexture, n.Value[2], n.Value[3],
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
            foreach (Fish n in fishes)
            {
                n.Draw(_spriteBatch);
            }
        }
    }
}
