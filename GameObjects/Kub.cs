using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using juliasfinal.Verktyg;

namespace juliasfinal.GameObjects
{
	class Kub : SpelObjekt
	{
		private readonly Random random;
		public (int X, int Y) Position { get; private set; }

		public int Size => Constants.RECT_SIZE;

		public Texture2D Texture { get; set; }

		public Kub() => random = new Random();

		public void SetupTexture(GraphicsDevice graphics)
		{
			Texture = new Texture2D(graphics, 1, 1);
			Texture.SetData(new[] { Color.White });
		}

		public void ResetToRandomPosition(List<Rectangle> tail)
		{//Här nedan ska jag se till att kuben hamnar på random ställen på spelskärmen  
			var rectangles = new List<(int posX, int posY)>();

			for (var areaX = 0; areaX < Constants.WINDOW_SIZE; ++areaX)
			{
				for (var areaY = 0; areaY < Constants.WINDOW_SIZE; ++areaY)
				{
					var (x, y) = (areaX / Size * Size, areaY / Size * Size);

					if (tail.Any(t => t.X == x && t.Y == y))
						continue;

					if (x == Position.X && y == Position.Y)
						continue;

					rectangles.Add((x, y));
				}
			}

			Position = rectangles[random.Next(rectangles.Count)];
		}    

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, new Rectangle(Position.X, Position.Y, Size, Size), Color.LightCoral);
		}
	}
}