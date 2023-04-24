using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/* Team Tranquil
 * GDAPS 2 Project
 * Class purpose: Create the user-controlled fishing rod
 * 
 * Known issues: Rod fishing line is unaligned
 * 
 */

namespace Fishing
{
    //enum of direction of fishing rod
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Ascent,
        Stationary
    }

    //enum of keybinds states
    enum Keybinds
    {
        WASD,
        Arrows

    }

    internal class FishingRod
    {
        /* FIELDS AND PROPERTIES */

        //Background
        Background bg;

        //Texture and position rectangle
        private Texture2D rodDesign;
        private Rectangle rect;
        private Rectangle catchRadius; // Will change with spells
        private Texture2D rodWire;

        // SKILL TREE STUFF
        private int skillPoints;
        private int totalExp;
        private int maxSpeed;
        private int maxDepth;

        //Direction enum and keyboard state
        private Direction direction;
        private Keybinds keybinds;
        private KeyboardState kbState;
        private KeyboardState prevKBState;

        //Dimensions of the window
        private int windowWidth;
        private int windowHeight;

        //Whether or not the fishing rod has an item
        private bool hasItem;

        //Variable for fishing rod's current depth
        private int currentDepth;

        // Timer variables to be used for spells
        private float sirenCallUptime;
        private float sirenCallCooldown;

        // SKILL TREE PROPERTIES
        //--------------------------------------------------
        public int SkillPoints 
        {
            get => skillPoints;
        }

        public int TotalExp
        {
            get => totalExp;

            set { totalExp = value; }
        }

        public int Level
        {
            get { return (int)Math.Sqrt(totalExp / 10); }
        }

        public int MaxSpeed
        {
            get => maxSpeed;
        }

        public int MaxDepth
        {
            get => maxDepth;
        }

        public Direction PlayerDirection
        {
            get { return direction; }
        }

        public int CurrentDepth
        {
            get => currentDepth;
        }
        //---------------------------------------------------
        //Keybinds Property
        public Keybinds Keybinds 
        { 
            get { return keybinds; } 
            set { keybinds = value; }
        }
        //---------------------------------------------------

        // List of known spells
        List<string> spells = new List<string>();

        // Property for the spells for access in the collectible manager
        public List<string> Spells
        {
            get => spells;
        }

        public Rectangle Rect
        {
            get { return rect; }
            //Get-only property
        }

        public Rectangle CatchRadius
        {
            get => catchRadius;
        }

        public bool HasItem
        {
            get { return hasItem; }
            set { hasItem = value; }
        }

        //TODO: Add any other properties or fields needed

        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public FishingRod(Texture2D rodDesign, Texture2D rodLine, int maxDepth, int x, int y, int windowWidth, int windowHeight, Background bg)
        {
            this.bg = bg;

            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.rodDesign = rodDesign;
            this.maxDepth = maxDepth;
            this.rodWire = rodLine;

            skillPoints = 0;
            totalExp = 0;

            // Initialize currentDepth
            currentDepth = y;

            //Set the rectangle at the given x and y position and with the width and height of the texture
            rect = new Rectangle(x, y, 50, 50);
            catchRadius = rect;

            // TEMPORARY give the player the siren call spell since it cannot be obtained in game currently
            spells.Add("sirencall");
        }

        /// <summary>
        /// Updates the fishing rod and controls movement.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //Updates the keyboard state
            kbState = Keyboard.GetState();

            //Determines state switching

            //Ascent
            //movement becomes based on keybind preset, this case is WASD preset
            if(keybinds == Keybinds.WASD)
            {
                if (kbState.IsKeyDown(Keys.Space))
                {
                    direction = Direction.Ascent;
                }

                //Down
                else if (kbState.IsKeyDown(Keys.S))
                {
                    direction = Direction.Down;
                }

                //Up
                else if (kbState.IsKeyDown(Keys.W))
                {
                    direction = Direction.Up;
                }

                //Left
                else if (kbState.IsKeyDown(Keys.A))
                {
                    direction = Direction.Left;
                }

                //Right
                else if (kbState.IsKeyDown(Keys.D))
                {
                    direction = Direction.Right;
                }

                //Stationary
                else
                {
                    direction = Direction.Stationary;
                }
            }

            //Moves based if keybinds preset is arrow keys
            if(keybinds == Keybinds.Arrows)
            {
                if (kbState.IsKeyDown(Keys.Space))
                {
                    direction = Direction.Ascent;
                }

                //Down
                else if (kbState.IsKeyDown(Keys.Down))
                {
                    direction = Direction.Down;
                }

                //Up
                else if (kbState.IsKeyDown(Keys.Up))
                {
                    direction = Direction.Up;
                }

                //Left
                else if (kbState.IsKeyDown(Keys.Left))
                {
                    direction = Direction.Left;
                }

                //Right
                else if (kbState.IsKeyDown(Keys.Right))
                {
                    direction = Direction.Right;
                }

                //Stationary
                else
                {
                    direction = Direction.Stationary;
                }
            }
            

            //Switch to update movement based on the state
            //TODO: For future sprint, verify maximum window that fishing rod can reach, values are placeholder for now
            //Additionally, maybe add a "float" mechanic when stationary to make the fishing rod float in the water
            switch (direction)
            {
                //Ascent
                case Direction.Ascent:
                    if (rect.Y > 0)
                    {
                        if (currentDepth <= windowHeight / 2 || rect.Y > windowHeight / 2)
                        {
                            rect.Y -= 5;
                        }

                        currentDepth -= 5;
                    }
                    break;

                //Up
                case Direction.Up:

                    //As long as the y position is less than 0
                    if (rect.Y > 0)
                    {
                        //Only allow the player to leave the center of the window if they're close to the top
                        if (currentDepth <= windowHeight / 2 || rect.Y > windowHeight / 2)
                        {
                            //If the player is holding down shift, double their speed
                            if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                            {
                                rect.Y -= 2;
                            }

                            //Otherwise, move up normally
                            else
                            {
                                rect.Y--;
                            }
                        }

                        //Update the current depth separately here, since currentDepth can go
                        //deeper than the y value
                        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                        {
                            currentDepth -= 2;
                        }

                        else
                        {
                            currentDepth--;
                        }

                    }
                    break;

                //Down
                case Direction.Down:

                    if (
                        //As long as the player isn't going off the edge of the screen
                        rect.Y < windowHeight - rect.Height
                        //And isn't centered, or is near the bottom of the level
                        && (rect.Y < windowHeight / 2 || currentDepth >= bg.TextureHeight + bg.Position.Y - windowHeight / 2))
                    {
                        //If the shift key is being held down
                        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                        {
                            //Move down at a higher rate than normal
                            rect.Y += 3;
                        }
                        else
                        {
                            //Otherwise, move down at a regular rate
                            rect.Y++;
                        }
                    }

                    //Update the current depth separately here, since currentDepth can go
                    //deeper than the y value
                    if (
                        //If the current depth does not exceed max depth
                        currentDepth < maxDepth
                        //And the current depth does not exceed bottom of level
                        && currentDepth < bg.TextureHeight + bg.Position.Y
                        //And the shift key is being held down
                        && (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift)))
                    {
                        //Add to the current depth at a faster rate than usual
                        currentDepth += 3;
                    }

                    //Otherwise
                    else if (
                        //If the current depth does not exceed max depth
                        currentDepth < maxDepth
                        //And the current depth does not exceed bottom of level
                        && currentDepth < bg.TextureHeight + bg.Position.Y)
                    {
                        //Add to the current depth
                        currentDepth++;
                    }

                    break;

                //Left
                case Direction.Left:
                    if (rect.X > 0)
                    {
                        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                        {
                            rect.X -= 3;
                        }
                        else
                        {
                            rect.X--;
                        }
                        
                    }
                    
                    break;

                //Right
                case Direction.Right:
                    if (rect.X < windowWidth)
                    {
                        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                        {
                            rect.X += 3;
                        }
                        else
                        {
                            rect.X++;
                        }
                        
                    }
                  
                    break;

                //Stationary/anything else -- do nothing
            }

            // Have the catch radius rectangle follow the player based on the midpoints
            Point rectMiddle = new Point(rect.X + rect.Width/2, rect.Y + rect.Height/2);
            catchRadius.X = rectMiddle.X - catchRadius.Width/2;
            catchRadius.Y = rectMiddle.Y - catchRadius.Height/2;

            // Handling for spells ----------------------------------------------------------------------
            
            // Siren call (currently bound to E)
            if(kbState.IsKeyUp(Keys.E) && prevKBState.IsKeyDown(Keys.E) 
                // Check if the player has the spell and it is not currently in use
                && spells.Contains("sirencall") && sirenCallUptime == 0
                // Make sure the spell isn't on cooldown
                && sirenCallCooldown <= 0)
            {
                // Triple the player's catch radius for 5 seconds
                catchRadius.Width *= 3;
                catchRadius.Height *= 3;
                sirenCallUptime = 5f;
            }

            // If the spell is currently in use, decrease the time left based on the game time in seconds
            if(sirenCallUptime > 0)
            {
                sirenCallUptime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            // If the spell has either just ended or is inactive
            if(sirenCallUptime <= 0)
            {
                sirenCallUptime = 0; // Just in case it somehow becomes less than 0

                // If the catch radius is larger than the player sprite (aka the spell has just ended) then change the radius back to normal
                if(catchRadius.Width != rect.Width)
                {
                    // Reset the catch radius
                    catchRadius.Width = rect.Width;
                    catchRadius.Height = rect.Height;

                    // Reset the cooldown to 15 seconds (can be modified)
                    sirenCallCooldown = 15f;
                }

                // If the spell is inactive then the cooldown for said spell will hac to ne incremented assuming the cooldown isn't over
                if(sirenCallCooldown > 0)
                {
                    sirenCallCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            // Update the previous keyboard state
            prevKBState = kbState;
        }

        /// <summary>
        /// Draws the fishing rod.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(rodDesign, rect, Color.White);
            _spriteBatch.Draw(rodWire, new Rectangle(rect.X, rect.Y-(rect.Y/2), 1000,100), Color.White); //TODO: This shows sometimes but not always, find the right values for this to work
        }

        /// <summary>
        /// Checks for a change in skill points by accepting an int value
        /// </summary>
        public void skillPointChange(int value)
        {
            // If the passed value (from the collectible manager) is higher
            if(skillPoints < value)
            {
                // Set the new numebr of skill points to be the passed value
                skillPoints = value;

                // TODO: Prompt the user to use the new skill point

            }
        }
    }
}