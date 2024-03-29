﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Security;
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
        private Random rng;

        //Background
        private Background bg;

        //Window width and height
        private int windowWidth;
        private int windowHeight;

        //Dictionary to hold the data for individual fish species
        private Dictionary<string, int[]> fishSpecies = new Dictionary<string, int[]>();

        // Dictionary to hold the different books (skill / spells)
        private Dictionary<string, int[]> books = new Dictionary<string, int[]>();

        //List of collectibles
        private List<Collectible> collectibles = new List<Collectible>();

        //List of textures
        private List<Texture2D> fishTextures = new List<Texture2D>();

        //Texture for siren effect
        private Texture2D sirenEffectTexture;

        // Texture for the books
        private Texture2D bookTexture;

        //Reference to fishing rod
        private FishingRod fishingRod;

        //Variables for file reading/
        private StreamReader input = null;
        private string fishData = "../../../Content/FishData.txt";
        private string bookData = "../../../Content/BookData.txt";

        // Skill points
        private int skillPoints = 0;

        // Property for the SP so the player can "see" when the number of points increases
        public int SkillPoints
        {
            get => skillPoints;

            set { skillPoints = value; }
        }
        // Property to get the dictionary over to the achievments menu
        public Dictionary<string, int[]> FishSpecies
        {
            get => fishSpecies;
        }
        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public CollectibleManager(Random rng, Background bg, int windowWidth, int windowHeight, List<Texture2D> fishTextures, Texture2D sirenEffectTexture, Texture2D bookTexture, FishingRod fishingRod)
        {
            //Initialize given values
            this.rng = rng;
            this.bg = bg;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.fishTextures = fishTextures;
            this.sirenEffectTexture = sirenEffectTexture;
            this.bookTexture = bookTexture;
            this.fishingRod = fishingRod;

            //Give skillPoints a starting value of 0
            skillPoints = 4;

            //Read the fish data
            ReadData(fishData);
            ReadData(bookData);
        }

        /// <summary>
        /// Updates the collectibles.
        /// </summary>
        public void Update()
        {

            // Spawn the fish
            SpawnFish();

            // Spawn books
            SpawnBooks();

            //Check each collectible
            for (int i = 0; i < collectibles.Count; i++)
            {
                //Update each collectible
                collectibles[i].Update();

                Books tempBook = null;
                // If the collectible is a book it will need a reference so the spell can be checked
                if (collectibles[i] is Books)
                {
                    tempBook = (Books)collectibles[i];
                }

                //Check to see if the player has caught a collectible
                if (
                    //If the collectible is caught already
                    (collectibles[i].IsCaught && !collectibles[i].AffectedByPower)
                    //OR if the collectible is overlapping the fishing rod's catch radius
                    || (
                        collectibles[i].Position.Intersects(fishingRod.CatchRadius)
                        //and the fishing rod does not already have an item
                        && !fishingRod.HasItem
                        //and the collectible is not affected by the siren's power
                        && !collectibles[i].AffectedByPower
                        //And if the collectible is either a book
                        && (
                            (collectibles[i] is Books
                            //and isn't the siren call spell book (isn't in itself catchable)
                            && tempBook.Spell != "sirencall")
                            //or if the collectible is a fish
                            || (collectibles[i] is Fish
                            //whose required level is lower than the player's level
                                && fishingRod.Level >= fishSpecies[((Fish)collectibles[i]).Name][4]
                                //and either the fish is not a siren
                                && (((Fish)collectibles[i]).Name != "siren"
                                    //or the player has the mute spell
                                    || fishingRod.Spells.Contains("mute")
                                   )
                                )
                            )
                        )
                    )
                {
                    //If the above conditions are met,
                    //set the collectible's position to that of the fishing rod
                    collectibles[i].FollowFishingRod(fishingRod.Rect);

                    //And tell the fishing rod it has an item
                    fishingRod.HasItem = true;
                }

                //If the collectible is a fish, is mythical,
                //and the player does not have the mute spell
                if (collectibles[i] is Fish
                    && ((Fish)collectibles[i]).IsMythical
                    && !fishingRod.Spells.Contains("mute"))
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

                //Check if the collectible is caught and the player made it to the top of the level with it
                if (collectibles[i].IsCaught && fishingRod.Rect.Y <= 5)
                {
                    //If the collectible is a fish
                    if (collectibles[i] is Fish)
                    {
                        //Cast the collectible to a fish
                        Fish currentFish = (Fish)collectibles[i];

                        //For readability purposes, get a reference to the int array for the species of that fish
                        int[] fishData = fishSpecies[currentFish.Name];

                        //Give the player exp
                        fishingRod.TotalExp += fishData[5];

                        //Check if the species of fish has never been caught before
                        if (fishData[fishData.Count() - 1] == 0)
                        {
                            //If so, set the fish's caught value to true
                            fishData[fishData.Count() - 1] = 1;

                            // If the caught fish is a siren, the player gains the siren call spell
                            if (currentFish.Name == "siren")
                            {
                                fishingRod.Spells.Add("sirencall");
                                books.Remove("sirencall");
                            }
                        }
                        //Otherwise, do nothing
                    }

                    //If the collectible is a book
                    else
                    {
                        // A temporary instance of the book collectible was created earlier for testing purposes,
                        // so no need to create a new one
                        if (tempBook.Spell != null) // If the book actually has a spell
                        {
                            fishingRod.Spells.Add(tempBook.Spell);
                            // Can only get a spell once, so once obtained the book will no longer spawn
                            books.Remove(tempBook.Spell);
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

                //If the collectible is being affected by a siren and is caught
                if (collectibles[i].AffectedByPower && collectibles[i].IsCaught)
                {
                    //The fishing rod no longer has the item
                    fishingRod.HasItem = false;
                    collectibles[i].IsCaught = false;
                }

                //Update the collectible's knowledge of the player level
                collectibles[i].PlayerLevel = fishingRod.Level;

                //If the collectible is dead
                if (collectibles[i].IsDead)
                {
                    //If the collectible is caught
                    if (collectibles[i].IsCaught)
                    {
                        //Then the fishing rod no longer has the collectible
                        fishingRod.HasItem = false;
                    }

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
        public void ReadData(string data)
        {
            try
            {
                //Variables for file reading
                string line = null;
                string[] lines;

                input = new StreamReader(data);

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
                    int[] collectibleData = new int[lines.Count() + 1];

                    //Search the entire array, minus the no-longer-useful first piece of data
                    for (int i = 1; i < lines.Count(); i++)
                    {
                        //Put that data into the array
                        collectibleData[i - 1] = int.Parse(lines[i]);
                    }

                    // If we are reading the fish data
                    if (data == fishData)
                    {
                        //Add a final value to track whether or not the fish has been caught before --
                        //1 if caught and 0 if not
                        collectibleData[lines.Count()] = 0;

                        //Add the fish to the list of species
                        fishSpecies.Add(key, collectibleData);
                    }
                    else // If we are reading the book data
                    {
                        books.Add(key, collectibleData);
                    }
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

                //Get the chance of the fish spawning
                int spawnChance = n.Value[0];

                //Give the fish a spawnChance-in-1000 chance of spawning -- the higher the spawnChance,
                //the more likely it is to spawn
                if (rng.Next(1001) <= spawnChance)
                {
                    //Create a new fish using the data in the array in the dictionary
                    Fish newFish = new Fish(n.Key, n.Value[1], n.Value[4], fishTexture, sirenEffectTexture, n.Value[2], n.Value[3],
                        windowWidth, windowHeight, rng, bg);

                    if (!newFish.IsMythical)
                    {
                        UsingPower += newFish.PowerEffect;
                    }

                    if (newFish.Name == "siren" && !fishingRod.Spells.Contains("sirencall"))
                    {
                        // Create a new book that will follow the siren, and give the siren call spell if teh player doesn't have it already
                        Books sirenCall = new Books("sirencall", books["sirencall"][1], bookTexture, books["sirencall"][2], books["sirencall"][3],
                            windowWidth, windowHeight, rng, bg, newFish);

                        collectibles.Add(sirenCall);
                    }

                    // Add that fish to the list
                    collectibles.Add(newFish);
                }

                //Add one to the index
                fishTextureIndex++;
            }
        }

        /// <summary>
        /// Random chance to spawn different books each frame
        /// </summary>
        public void SpawnBooks()
        {
            int bookTextureIndex = 0;

            //Check each fish in the dictionary
            foreach (KeyValuePair<string, int[]> n in books)
            {
                //Get the chance of the fish spawning
                int spawnChance = n.Value[0];

                //Give the fish a spawnChance-in-1000 chance of spawning -- the higher the spawnChance,
                //the more likely it is to spawn
                if (rng.Next(10001) <= spawnChance)
                {
                    //Create a new book using the data in the array in the dictionary
                    Books newBook = new Books(n.Key, n.Value[1], bookTexture, n.Value[2], n.Value[3],
                        windowWidth, windowHeight, rng, bg, null);

                    // Add that book to the list
                    collectibles.Add(newBook);
                }

                //Add one to the index
                bookTextureIndex++;
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

                //As long as the sirens should not be muted
                if (n is Fish && ((Fish)n).Name == "siren" && !fishingRod.Spells.Contains("mute"))
                {
                    //Draw their effect
                    ((Fish)n).DrawSiren(_spriteBatch);
                }
            }
        }
    }
}
