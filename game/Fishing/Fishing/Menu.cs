using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;

namespace Fishing
{
    internal class Menu
    {
        /* FIELDS AND PROPERTIES */

        //menu textures
        Texture2D outerShade;
        Texture2D menuFrame;
        Texture2D menuOutline;

        //menu buttons
        Button menuButton;
        Button saveButton;
        Button statsButton;
        Button achieveButton;
        Button quitButton;
        Button backButton;

        //header
        SpriteFont menuHeader;

        private int windowWidth;
        private int windowHeight;

        private List<Button> buttonList;
        private List<Button> mainButtonList;
        private List<Button> statsButtonList;
        private List<Button> achievementsButtonList;
        private MenuState currentState;

        private bool open = false;

        public bool Open
        {
            get { return open; }
        }

        public List<Button> Buttons { get { return buttonList; } } 


        /* CONSTRUCTORS AND METHODS */
        
        //Parameterized constructor
        public Menu(int windowWidth, int windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            currentState = MenuState.Closed;
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            //background shade
            outerShade = new Texture2D(graphicsDevice, 1, 1);
            outerShade.SetData(new[] { Color.Gray });

            //frame
            menuFrame = new Texture2D(graphicsDevice, 1, 1);
            menuFrame.SetData(new[] { Color.BurlyWood });

            //frame outline
            menuOutline = new Texture2D(graphicsDevice, 1, 1);
            menuOutline.SetData(new[] { Color.Chocolate });

            //menu header
            menuHeader = content.Load<SpriteFont>("Header");

            /* BUTTON HANDLER */
            menuButton = new Button(content.Load<Texture2D>("menuButton"), content.Load<SpriteFont>("Font"))
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
            saveButton.Position = new Vector2(windowWidth / 2 - saveButton.Rectangle.Width / 2, (windowHeight / 2 - saveButton.Rectangle.Height / 2)-100);
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
                Text = "Achievement",
            };
            achieveButton.Position = new Vector2(statsButton.Rectangle.Right+30, (windowHeight / 2 - saveButton.Rectangle.Height / 2) - 50);
            achieveButton.Click += AchievementButtonClick;

            //quit button
            quitButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Quit",
            };
            quitButton.Position = new Vector2(windowWidth / 2 - quitButton.Rectangle.Width / 2, (windowHeight / 2 - quitButton.Rectangle.Height / 2) + 100);
            quitButton.Click += QuitButtonClick;

            //back button
            backButton = new Button(graphicsDevice, content.Load<Texture2D>("longButton"), content.Load<SpriteFont>("Font"), Color.Bisque, Color.Peru, Color.Peru)
            {
                Text = "Back to Main Menu",
            };
            backButton.Position = new Vector2(windowWidth / 2 - backButton.Rectangle.Width / 2, (windowHeight / 2 - backButton.Rectangle.Height / 2) + 100);
            backButton.Click += BackButtonClick;

            mainButtonList = new List<Button>()
            {
                saveButton,
                statsButton,
                achieveButton,
                quitButton,
            };

            statsButtonList = new List<Button>()
            {
                backButton,
            };

            achievementsButtonList = new List<Button>()
            {
                backButton,
            };
        }

        public void Update(GameTime gameTime)
        {
            if(currentState != MenuState.Closed)
            {
                List<Button> updateButton;
                switch (currentState)
                {
                    case MenuState.Stats:
                        updateButton = statsButtonList;
                        break;

                    case MenuState.Achievements:
                        updateButton = achievementsButtonList;
                        break;

                    default:
                        updateButton = mainButtonList;
                        break;
                }

                foreach (Button btn in updateButton)
                {
                    btn.Update(gameTime);
                }
            }
            
            menuButton.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string headerText = "";
            if(currentState != MenuState.Closed)
            {
                spriteBatch.Draw(outerShade, new Rectangle(0, 0, windowWidth, windowHeight), Color.White * 0.5f);
                spriteBatch.Draw(menuOutline, new Rectangle((windowWidth / 2 - 307), (windowHeight / 2 - 207), 614, 414), Color.White);
                spriteBatch.Draw(menuFrame, new Rectangle((windowWidth / 2 - 300), (windowHeight / 2 - 200), 600, 400), Color.White);
                

                if (currentState == MenuState.Main)
                {
                    headerText = "Menu";
                    for (int i = 0; i < mainButtonList.Count; i++)
                    {
                        mainButtonList[i].Draw(gameTime, spriteBatch);
                    }
                }
                else if (currentState == MenuState.Stats)
                {
                    headerText = "Stats";
                    for (int i = 0; i < statsButtonList.Count; i++)
                    {
                        statsButtonList[i].Draw(gameTime, spriteBatch);
                    }
                }
                else if (currentState == MenuState.Achievements)
                {
                    headerText = "Achievements";
                    for (int i = 0; i < achievementsButtonList.Count; i++)
                    {
                        achievementsButtonList[i].Draw(gameTime, spriteBatch);
                    }
                }

                spriteBatch.DrawString(menuHeader, headerText, new Vector2((windowWidth / 2 - menuHeader.MeasureString(headerText).X / 2), (windowHeight / 2 - 180)), Color.Black);
            }  
            menuButton.Draw(gameTime, spriteBatch);
        }

        private void MenuButtonClick(object sender, System.EventArgs e)
        {
            if (currentState == MenuState.Closed)
            {
                currentState = MenuState.Main;
            }
            else
            {
                currentState = MenuState.Closed;
            }
        }
        private void SaveButtonClick(object sender, System.EventArgs e)
        {
            saveButton.Text = "Saved!";
        }
        private void StatsButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Stats;
        }
        private void AchievementButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Achievements;
        }
        private void QuitButtonClick(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }
        private void BackButtonClick(object sender, System.EventArgs e)
        {
            currentState = MenuState.Main;
            
        }
    }
}
