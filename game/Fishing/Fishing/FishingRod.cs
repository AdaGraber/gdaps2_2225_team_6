using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
 * Known issues:
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
    internal class FishingRod
    {
        /* FIELDS AND PROPERTIES */

        //Texture and position rectangle
        private Texture2D rodDesign;
        private Rectangle rect;

        // SKILL TREE STUFF
        private int skillPoints;
        private int maxSpeed;
        private int maxDepth;

        //Direction enum and keyboard state
        private Direction direction;
        private KeyboardState kbState;

        //Width of the window
        private int windowWidth;

        //Whether or not the fishing rod has an item
        private bool hasItem;

        // SKILL TREE PROPERTIES
        //--------------------------------------------------

        public int Speed
        {
            get => maxSpeed;
        }

        public int Depth
        {
            get => maxDepth;
        }
        //---------------------------------------------------

        public Rectangle Rect
        {
            get { return rect; }
            //Get-only property
        }

        public bool HasItem
        {
            get { return hasItem; }
            set { hasItem = value; }
        }

        //TODO: Add any other properties or fields needed

        /* CONSTRUCTORS AND METHODS */

        //Parameterized constructor
        public FishingRod(Texture2D rodDesign, int maxDepth, int x, int y, int windowWidth)
        {
            this.windowWidth = windowWidth;
            this.rodDesign = rodDesign;
            this.maxDepth = maxDepth;

            skillPoints = 0;

            //Set the rectangle at the given x and y position and with the width and height of the texture
            rect = new Rectangle(x, y, rodDesign.Width, rodDesign.Height);
        }

        /// <summary>
        /// Updates the fishing rod and controls movement.
        /// </summary>
        public void Update()
        {
            //Updates the keyboard state
            kbState = Keyboard.GetState();

            //Determines state switching

            //Ascent
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

            //Switch to update movement based on the state
            //TODO: For future sprint, verify maximum window that fishing rod can reach, values are placeholder for now
            //Additionally, maybe add a "float" mechanic when stationary to make the fishing rod float in the water
            switch (direction)
            {
                //Ascent
                case Direction.Ascent:
                    if (rect.Y > 0)
                    {
                        rect.Y -= 4;
                    }
                    break;

                //Up
                case Direction.Up:
                    if (rect.Y > 0)
                    {
                        rect.Y--;
                    }
                    break;

                //Down
                case Direction.Down:
                    if (rect.Y < maxDepth)
                    {
                        rect.Y++;
                    }
                    break;

                //Left
                case Direction.Left:
                    if (rect.X > 0)
                    {
                        rect.X--;
                    }
                    break;

                //Right
                case Direction.Right:
                    if (rect.X < windowWidth)
                    {
                        rect.X++;
                    }

                    break;

                //Stationary/anything else -- do nothing
            }
        }

        /// <summary>
        /// Draws the fishing rod.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(rodDesign, rect, Color.White);
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