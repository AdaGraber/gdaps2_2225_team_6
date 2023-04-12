using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing
{
    internal class Background
    {
        int windowWidth;
        int windowHeight;

        private Rectangle background;
        private Texture2D backgroundTexture;
        private Color backgroundColor;

        private FishingRod fishingRod;

        public Background(int windowWidth, int windowHeight, GraphicsDevice graphicsDevice, FishingRod fishingRod, Texture2D backgroundTexture)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            this.backgroundTexture = backgroundTexture;

            background = new Rectangle(0, 0, windowWidth, windowHeight);

            this.fishingRod = fishingRod;

            backgroundColor = Color.CornflowerBlue;
        }

        public void Update()
        {
            if (fishingRod.Rect.Y >= windowHeight - fishingRod.Rect.Height
                && fishingRod.CurrentDepth < fishingRod.MaxDepth
                && fishingRod.PlayerDirection == Direction.Down)
            {
                backgroundColor.A--;
            }

            else if (fishingRod.Rect.Y <= 0
                && fishingRod.CurrentDepth > 0
                && fishingRod.PlayerDirection == Direction.Up)
            {
                backgroundColor.A++;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(backgroundTexture, background, backgroundColor);
        }
    }
}
