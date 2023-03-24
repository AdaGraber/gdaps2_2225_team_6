using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //What the player controls in order to catch fish and books

        

        //FIELDS and Properties
        private Texture2D rodDesign;
        private int depth;
        private Vector2 pos;
        private Rectangle rect;
        private Direction direction;
        private KeyboardState prevKbState;
        private KeyboardState kbState;
        private int minDepth;
        private int maxDepth;
        private bool hasItem;

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
        public int Depth { get { return depth; } }
       
        //constructor TODO: Implement the rod's rectangle to print
        public FishingRod(Texture2D rodDesign, int depth, Vector2 pos)
        { 
            this.rodDesign = rodDesign;
            this.depth = depth;
            this.pos = pos;

            hasItem = false;

            rect = new Rectangle((int)pos.X, (int)pos.Y, 20, 40); //very temporary fix for compiler errors
        }

        public void Catch(Collectible itemCaught)
        {
            if (hasItem == true && rect.Y == 0)
            {
                if (itemCaught is Books)
                {
                    
                }

                if (itemCaught is Fish)
                {
                    //should remove fish from play, which is difficult for fishingRod to do

                    // if the item caught has not been caught before, update the list
                }
            }
            hasItem = true;
        }

        //TODO: Finish fishing rod update
        public void Update()
        {
            //adds kbstate when updated
            kbState = Keyboard.GetState();

            //Determines state switching
            switch(direction)
            {
                case Direction.Up:
                    if (kbState.IsKeyUp(Keys.Up))
                    {
                        direction = Direction.Stationary;
                    }
                    break;
                case Direction.Down:
                    if (kbState.IsKeyUp(Keys.Down))
                    {
                        direction = Direction.Stationary;
                    }
                    break;
                case Direction.Left:
                    if (kbState.IsKeyUp(Keys.Left))
                    { 
                        direction = Direction.Stationary; 
                    }
                    break;
                case Direction.Right:
                    if (kbState.IsKeyUp(Keys.Right))
                    {
                        direction = Direction.Stationary;
                    }
                    break;
                case Direction.Ascent:
                    if(kbState.IsKeyUp(Keys.Space) && !prevKbState.IsKeyUp(Keys.Space))
                    {
                        direction = Direction.Stationary;
                    }
                    break;
                case Direction.Stationary:
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        direction = Direction.Ascent;
                    }
                    if (kbState.IsKeyDown(Keys.Down))
                    {
                        direction = Direction.Down;
                    }
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        direction = Direction.Up;
                    }
                    if (kbState.IsKeyDown(Keys.Left))
                    {
                        direction = Direction.Left;
                    }
                    if (kbState.IsKeyDown(Keys.Right))
                    {
                        direction = Direction.Right;
                    }
                    break;
            }

            //after everything in update is finished, update prevkbstate
            prevKbState = kbState;
            
        }

        //Updates fishing rod's actions based on what may be seen in 
        public void Draw()
        {
            //switch to update movement based on the state
            //TODO: For future sprint, verify maximum window that fishing rod can reach, values are placeholder for now
            //Additionally, maybe add a "float" mechanic when stationary to make the fishing rod float in the water
            switch (direction)
            {
                case Direction.Stationary:

                    break;
                case Direction.Up:
                    pos.Y--;
                    if(pos.Y < 0)
                    {
                        pos.Y = 0;
                        depth--;
                    }
                    break;
                case Direction.Down:
                    pos.Y++;
                    if(pos.Y > 200)
                    {
                        pos.Y = 200;
                        depth++;
                    }
                    break;
                case Direction.Left:
                    pos.X--;
                    if(pos.X < 0)
                    {
                        pos.X = 0;
                    }
                    break;
                case Direction.Right:
                    pos.X++;
                    if(pos.X > 200)
                    {
                        pos.X = 200;
                    }
                    break;
                case Direction.Ascent:
                    pos.Y -= 5;
                    if(pos.Y < 0)
                    {
                        pos.Y = 0;
                        depth -= 5;
                    }
                    break;
            }
        }
    }
}
