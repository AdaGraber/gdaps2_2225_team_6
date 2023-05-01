using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;
using System.IO;

namespace Fishing
{
    internal class Menu
    {
        /* FIELDS AND PROPERTIES */

        //menu textures
        Texture2D outerShade;
        Texture2D menuFrame;
        Texture2D menuOutline;
        Texture2D depthFrame; //display score
        Texture2D depthOutline;

        //menu buttons
        Button menuButton;
        Button saveButton;
        Button statsButton;
        Button achieveButton;
        Button howToPlayButton;
        Button controlsButton;
        Button quitButton;
        Button backButton;

        //achievements buttons
        Button rightArrowButton;
        Button leftArrowButton;

        //control buttons
        Button WASDButton;
        Button ArrowsButton;

        //stats menu
        Texture2D statsFrame;
        Texture2D statsOutline;
        int level = 0; //actual stats
        int maxDepth = 0;
        int maxSpeed = 0;
        int currentDepth;

        //achievements menu
        Texture2D flavorFrame;
        Texture2D flavorOutline;
        Texture2D clipFrame;
        Texture2D arrow;
        Rectangle clipRect; //rect for clip frame
        RasterizerState rsEnabled = new RasterizerState() { ScissorTestEnable = true }; //scissoring enabled
        //RasterizerState rsDisabled = new RasterizerState() { ScissorTestEnable = false }; //scissoring disabled

        //fonts
        SpriteFont menuHeader;
        SpriteFont menuFooter;

        private int windowWidth;
        private int windowHeight;

        private List<Button> mainButtonList;
        private List<Button> statsButtonList;
        private List<Button> achievementsButtonList;
        private List<Button> howToPlayButtonList;
        private List<Button> controlsButtonList;
        private MenuState currentState;

        //reference to fishing rod
        FishingRod fishingRod;
        
        //fish achievements
        Dictionary<string, int[]> fishSpecies; // copy CollectibleManager reference of dict to menu for display purposes
        List<Texture2D> fishTextures = new List<Texture2D>();
        int offsetX; //for displaying the fish
        int currentFish = 0;

        public MenuState CurrentState
        {
            get => currentState;
        }

        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public Menu(int windowWidth, int windowHeight, FishingRod fishingRod)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            this.fishingRod = fishingRod;

            currentState = MenuState.Closed;
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content, CollectibleManager collectibleManager)
        {
            //background shade
            outerShade = new Texture2D(graphicsDevice, 1, 1); //initialize a 1x1 texture
            outerShade.SetData(new[] { Color.Gray }); //color the texture

            //frames
            menuFrame = new Texture2D(graphicsDevice, 1, 1);
            menuFrame.SetData(new[] { Color.BurlyWood });
            //depth frame
            depthFrame = new Texture2D(graphicsDevice, 1, 1);
            depthFrame.SetData(new[] { Color.BurlyWood });
            //stats frame
            statsFrame = new Texture2D(graphicsDevice, 1, 1);
            statsFrame.SetData(new[] { Color.Bisque });
            //achievements frame
            flavorFrame = new Texture2D(graphicsDevice, 1, 1);
            flavorFrame.SetData(new[] { Color.Bisque });
            clipFrame = new Texture2D(graphicsDevice, 1, 1);
            clipFrame.SetData(new[] { Color.Bisque });
            arrow = content.Load<Texture2D>("ARROW");
            clipRect = new Rectangle((windowWidth / 2 - 249), (windowHeight / 2 - 120), 498, 144);

            //frame outlines
            menuOutline = new Texture2D(graphicsDevice, 1, 1);
            menuOutline.SetData(new[] { Color.Chocolate });
            //depth frame outline
            depthOutline = new Texture2D(graphicsDevice, 1, 1);
            depthOutline.SetData(new[] { Color.Chocolate });
            //stats frame
            statsOutline = new Texture2D(graphicsDevice, 1, 1);
            statsOutline.SetData(new[] { Color.Peru });
            //achievements frame
            flavorOutline = new Texture2D(graphicsDevice, 1, 1);
            flavorOutline.SetData(new[] { Color.Peru });


            //menu fonts
            menuHeader = content.Load<SpriteFont>("Header");
            menuFooter = content.Load<SpriteFont>("Foot");

            /* BUTTON HANDLER */
            menuButton = new Button(content.Load<Texture2D>("menuButton"), content.Load<SpriteFont>("Font")) //creates new button and loads texture and pos
            {
                Position = new Vector2(10, 10),
                Text = "",
            };
            menuButton.Click += MenuButtonClick;

            //save button
            saveButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Save Game",
            };
            saveButton.Position = new Vector2(windowWidth / 2 - saveButton.Rectangle.Width / 2, (windowHeight / 2 - saveButton.Rectangle.Height / 2) - 100);
            saveButton.Click += SaveButtonClick;

            //stats button
            statsButton = new Button(graphicsDevice, content.Load<Texture2D>("shortButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Stats",
            };
            statsButton.Position = new Vector2(windowWidth / 2 - saveButton.Rectangle.Width / 2, (windowHeight / 2 - saveButton.Rectangle.Height / 2) - 50); //uses saveButton to cut in half while staying at the same point
            statsButton.Click += StatsButtonClick;

            //achieve button
            achieveButton = new Button(graphicsDevice, content.Load<Texture2D>("shortButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Achievements",
            };
            achieveButton.Position = new Vector2(statsButton.Rectangle.Right + 30, (windowHeight / 2 - saveButton.Rectangle.Height / 2) - 50);
            achieveButton.Click += AchievementButtonClick;

            //quit button
            quitButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Quit",
            };
            quitButton.Position = new Vector2(windowWidth / 2 - quitButton.Rectangle.Width / 2, (windowHeight / 2 - quitButton.Rectangle.Height / 2) + 150);
            quitButton.Click += QuitButtonClick;

            //back button
            backButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Back to Main Menu",
            };
            backButton.Position = new Vector2(windowWidth / 2 - backButton.Rectangle.Width / 2, (windowHeight / 2 - backButton.Rectangle.Height / 2) + 150);
            backButton.Click += BackButtonClick;

            //right arrow button (for achievements)
            rightArrowButton = new Button(graphicsDevice, content.Load<Texture2D>("ARROW"), content.Load<SpriteFont>("Font"), Color.Peru, Color.SaddleBrown)
            {
                Text = "",
            };
            rightArrowButton.Position = new Vector2(clipRect.Right + 10, (clipRect.Center.Y - arrow.Height / 2));
            rightArrowButton.Click += RightArrowButtonClick;
            //left
            leftArrowButton = new Button(graphicsDevice, content.Load<Texture2D>("leftArrow"), content.Load<SpriteFont>("Font"), Color.Peru, Color.SaddleBrown)
            {
                Text = "",
            };
            leftArrowButton.Position = new Vector2(clipRect.Left - 40, (clipRect.Center.Y - arrow.Height / 2));
            leftArrowButton.Click += LeftArrowButtonClick;

            //howtoplay
            howToPlayButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "How To Play",
            };
            howToPlayButton.Position = new Vector2(windowWidth / 2 - backButton.Rectangle.Width / 2, (windowHeight / 2 - backButton.Rectangle.Height / 2) + 100);
            howToPlayButton.Click += HowToPlayButtonClick;

            //controls
            controlsButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Controls",
            };
            controlsButton.Position = new Vector2(windowWidth / 2 - backButton.Rectangle.Width / 2, (windowHeight / 2 - backButton.Rectangle.Height / 2) + 50);
            controlsButton.Click += ControlsButtonClick;
            //wasd
            WASDButton = new Button(graphicsDevice, content.Load<Texture2D>("squareButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "WASD\nUp - W\nDown - S\nLeft - A\nRight - D\nAscend - Space",
            };
            WASDButton.Position = new Vector2((windowWidth / 2 - WASDButton.Rectangle.Width / 2) - 130, (windowHeight / 2 - WASDButton.Rectangle.Height / 2));
            WASDButton.Click += WASDButtonClick;
            //arrows
            ArrowsButton = new Button(graphicsDevice, content.Load<Texture2D>("squareButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Arrows\nUp - Up\nDown - Arrow\nLeft - Left\nRight - Right\nAscend - Space",
            };
            ArrowsButton.Position = new Vector2((windowWidth / 2 - WASDButton.Rectangle.Width / 2) + 130, (windowHeight / 2 - WASDButton.Rectangle.Height / 2));
            ArrowsButton.Click += ArrowsButtonClick;


            //button lists
            mainButtonList = new List<Button>() //for the main menu
            {
                saveButton,
                statsButton,
                achieveButton,
                howToPlayButton,
                controlsButton,
                quitButton,
            };

            statsButtonList = new List<Button>() //stats menu
            {
                backButton,
            };

            achievementsButtonList = new List<Button>() //achievements menu
            {
                backButton,
                rightArrowButton,
                leftArrowButton,
            };

            howToPlayButtonList = new List<Button>() //instructions menu
            {
                backButton,
            };

            controlsButtonList = new List<Button>() //controls
            {
                WASDButton,
                ArrowsButton,
                backButton,
            };

            fishSpecies = collectibleManager.FishSpecies; //Reference existing FileIo dict
            foreach(KeyValuePair<string, int[]> pair in fishSpecies)
            {
                fishTextures.Add(content.Load<Texture2D>(pair.Key));
            }
        }

        public void Update(GameTime gameTime, FishingRod fishingRod)
        {
            if(currentState != MenuState.Closed) //menu is open
            {
                List<Button> updateButton; //general list to reference the desired list
                switch (currentState)
                {
                    case MenuState.Stats:
                        updateButton = statsButtonList;
                        level = fishingRod.Level; //skill level
                        maxDepth = fishingRod.MaxDepth; //depth
                        maxSpeed = fishingRod.MaxSpeed; //speed

                        break;

                    case MenuState.Achievements:
                        updateButton = achievementsButtonList;
                        break;

                    case MenuState.HowToPlay:
                        updateButton = howToPlayButtonList;
                        break;
                    case MenuState.Controls:
                        updateButton = controlsButtonList;
                        break;
                    default:
                        updateButton = mainButtonList;
                        break;
                }

                foreach (Button btn in updateButton)
                {
                    btn.Update(gameTime); //update buttons
                }
            }
            else
            {
                currentDepth = fishingRod.CurrentDepth;
            }
            
            menuButton.Update(gameTime); //hamburger
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string headerText = ""; //top text
            if(currentState != MenuState.Closed) //menu is open
            {
                //draw background shade, outline and menu frame
                spriteBatch.Draw(outerShade, new Rectangle(0, 0, windowWidth, windowHeight), Color.White * 0.5f);
                spriteBatch.Draw(menuOutline, new Rectangle((windowWidth / 2 - 307), (windowHeight / 2 - 207), 614, 414), Color.White);
                spriteBatch.Draw(menuFrame, new Rectangle((windowWidth / 2 - 300), (windowHeight / 2 - 200), 600, 400), Color.White);
                

                if (currentState == MenuState.Main) //main menu state
                {
                    headerText = "Menu";
                    for (int i = 0; i < mainButtonList.Count; i++)
                    {
                        mainButtonList[i].Draw(gameTime, spriteBatch); //load buttons for main menu
                    }
                }
                else if (currentState == MenuState.Stats) //stats menu state
                {
                    headerText = "Stats";
                    for (int i = 0; i < statsButtonList.Count; i++)
                    {
                        statsButtonList[i].Draw(gameTime, spriteBatch); //load buttons for stats menu
                    }

                    spriteBatch.Draw(statsOutline, new Rectangle((windowWidth / 2 - 252), (windowHeight / 2 - 100), 504, 150), Color.White);
                    spriteBatch.Draw(statsFrame, new Rectangle((windowWidth / 2 - 249), (windowHeight / 2 - 97), 498, 144), Color.White);

                    //stats text
                    string footText = String.Format("Level: {0}\nMax:Depth: {1}\nSpeed: {2}", level, maxDepth, maxSpeed);
                    spriteBatch.DrawString(menuFooter, footText, new Vector2((windowWidth / 2 - 239), (windowHeight / 2 - 87)), Color.Black);
                }
                else if (currentState == MenuState.Achievements) //achievements menu state
                {
                    headerText = "Achievements";
                    for (int i = 0; i < achievementsButtonList.Count; i++)
                    {
                        achievementsButtonList[i].Draw(gameTime, spriteBatch); //load buttons for achievements state
                    }

                    spriteBatch.End(); //END SPRITEBATCH
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rsEnabled); //got this from StackExchange, ENABLE SCISSORING/CLIPPING

                    Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle; //save current rect to restore later

                    spriteBatch.GraphicsDevice.ScissorRectangle = clipRect; //replace with the rect of the frame

                    
                    spriteBatch.Draw(clipFrame, clipRect, Color.White); //draw the frame to clip

                    //DRAW STUFF TO CLIP HERE 

                    //FISH TEXTURES
                    for (int i = 0; i < fishTextures.Count; i++)
                    {
                        spriteBatch.Draw(fishTextures[i], new Vector2((clipRect.Center.X - fishTextures[i].Width / 2) + (offsetX +100*i), clipRect.Center.Y - fishTextures[i].Height / 2), Color.White);
                    }

                    spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
                    spriteBatch.End(); //end clipping

                    spriteBatch.Begin(); //draw normally again

                    //arrows
                    //flavor frame
                    spriteBatch.Draw(flavorOutline, new Rectangle((windowWidth / 2 - 252), (windowHeight / 2 + 30), 504, 40), Color.White);
                    spriteBatch.Draw(flavorFrame, new Rectangle((windowWidth / 2 - 249), (windowHeight / 2 + 33), 498, 34), Color.White);

                    string flavorText = "";
                    string currentFishName = fishTextures[currentFish].Name;
                    int caught = fishSpecies[currentFishName][(fishSpecies[currentFishName].Count() - 1)];

                    string firstLetter = currentFishName.Substring(0, 1);
                    firstLetter = firstLetter.ToUpper();
                    currentFishName = currentFishName.Remove(0, 1);
                    currentFishName = currentFishName.Insert(0, firstLetter);
                    if (caught == 1) {
                        flavorText = currentFishName + " - Caught";
                    }
                    else if(caught == 0)
                    {
                        
                        flavorText = currentFishName + " - Not Caught";
                    }
                    
                    spriteBatch.DrawString(menuFooter, flavorText, new Vector2((windowWidth / 2 - menuFooter.MeasureString(flavorText).X / 2), (windowHeight / 2 + 38)), Color.Black);
                }
                else if (currentState == MenuState.HowToPlay)
                {
                    headerText = "How To Play";
                    for (int i = 0; i < howToPlayButtonList.Count; i++)
                    {
                        howToPlayButtonList[i].Draw(gameTime, spriteBatch); //load buttons for HowToPlay state
                    }

                    //use stats text box thing to house the instructions text
                    spriteBatch.Draw(statsOutline, new Rectangle((windowWidth / 2 - 252), (windowHeight / 2 - 100), 504, 165), Color.White);
                    spriteBatch.Draw(statsFrame, new Rectangle((windowWidth / 2 - 249), (windowHeight / 2 - 97), 498, 159), Color.White);

                    //howtoplay text
                    string footText = String.Format("Use WASD or arrow keys (check controls) in order \nto move the rod.\nCatching fish increases your score, catching books \nincreases your skill.\nHold space to quickly reel in the hook.\nHold shift to move the hook quickly in the water.");
                    spriteBatch.DrawString(menuFooter, footText, new Vector2((windowWidth / 2 - 239), (windowHeight / 2 - 87)), Color.Black);
                }
                else if(currentState == MenuState.Controls)
                {
                    headerText = "Controls";
                    for (int i = 0; i < controlsButtonList.Count; i++)
                    {
                       controlsButtonList[i].Draw(gameTime, spriteBatch); //load buttons for controls state
                    }
                }
                //header text
                spriteBatch.DrawString(menuHeader, headerText, new Vector2((windowWidth / 2 - menuHeader.MeasureString(headerText).X / 2), (windowHeight / 2 - 180)), Color.Black);
            }  
            else
            {
                spriteBatch.Draw(depthOutline, new Rectangle(10, windowHeight - 56, 136, 46), Color.White);
                spriteBatch.Draw(depthFrame, new Rectangle(13, windowHeight - 53, 130, 40), Color.White);
                spriteBatch.DrawString(menuFooter, "Depth: " + currentDepth, new Vector2(20, windowHeight - 45), Color.Black);
            }
            menuButton.Draw(gameTime, spriteBatch); //hamburger btn
        }

        private void MenuButtonClick(object sender, System.EventArgs e)
        {
            if (currentState == MenuState.Closed) //closed
            {
                currentState = MenuState.Main; //open menu
            }
            else
            {
                currentState = MenuState.Closed; //close menu

                // Reset save button text
                saveButton.Text = "Save Game";
            }
        }
        private void SaveButtonClick(object sender, System.EventArgs e)
        {
            StreamWriter output = null;

            try
            {
                //Initialize the StreamWriter
                output = new StreamWriter("../../../Content/Save.txt");

                //Add the fishing rod's stats to the first line
                output.WriteLine("{0},{1},{2},{3}",
                    fishingRod.TotalExp, fishingRod.MaxDepth, fishingRod.MaxSpeed,
                    fishingRod.SkillPoints);

                //Add the player's learned spells to the second line
                for (int i = 0; i < fishingRod.Spells.Count; i++)
                {
                    output.Write(fishingRod.Spells[i]);
                    
                    //As long as the spell being added is not the last one
                    if (i < fishingRod.Spells.Count - 1)
                    {
                        //Add a comma
                        output.Write(",");
                    }
                }

                output.WriteLine();

                //Add what fish have and haven't been caught
                for (int i = 0; i < fishSpecies.Count; i++)
                {
                    // Get the array for the species at i for readability
                    int[] fishData = fishSpecies.ElementAt(i).Value;

                    // Write whether or not the fish has been caught to the file
                    output.Write(fishData[fishData.Count() - 1]);

                    // As long as the fish being added is not the last one
                    if (i < fishSpecies.Count - 1)
                    {
                        // Add a comma
                        output.Write(",");
                    }
                }
            }

            catch (Exception savingException)
            {
                // Inform the user there was an error
                saveButton.Text = "Error saving: " + savingException.Message;
            }

            //If the file was opened, close it
            if (output != null)
            {
                output.Close();
            }

            saveButton.Text = "Saved!";
        }
        private void StatsButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Stats; //change menu to stats
        }
        private void AchievementButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Achievements; //change menu to achievements
        }
        private void QuitButtonClick(object sender, System.EventArgs e)
        {
            Environment.Exit(0); //quit
        }
        private void BackButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Main; //back to main menu 
        }
        private void RightArrowButtonClick(object sender, System.EventArgs e)
        {
            if(currentFish < fishTextures.Count-1)
            {
                offsetX -= 100;
                currentFish++;
            }
        }
        private void LeftArrowButtonClick(object sender, System.EventArgs e)
        {
            if(currentFish > 0)
            {
                offsetX += 100;
                currentFish--;
            }
        }
        private void HowToPlayButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.HowToPlay;
        }
        private void ControlsButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Controls;
        }
        private void WASDButtonClick(object sender, System.EventArgs e)
        {
            fishingRod.Keybinds = Keybinds.WASD;
        }
        private void ArrowsButtonClick(object sender, System.EventArgs e)
        {
            fishingRod.Keybinds = Keybinds.Arrows;
        }
    }
}
