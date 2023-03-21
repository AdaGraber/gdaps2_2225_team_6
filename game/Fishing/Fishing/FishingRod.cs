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

        //TODO: Add any other properties or fields needed
        public int Depth { get { return depth; } }
       
        //constructor TODO: Implement the rod's rectangle to print
        public FishingRod(Texture2D rodDesign, int depth, Vector2 pos)
        { 
            this.rodDesign = rodDesign;
            this.depth = depth;
            this.pos = pos;

            rect = new Rectangle((int)pos.X, (int)pos.Y, 20, 40); //very temporary fix for compiler errors
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
    }
}
