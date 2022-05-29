using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using juliasfinal.Verktyg;

namespace juliasfinal.GameObjects
{
	class Snake : SpelObjekt
	{
		private int lastUpdate;
		const int INTERVAL_MILLISECONDS = 96;
		
		public List<Rectangle> Tail { get; }

		private (int X, int Y) position;
		private (int X, int Y) direction = (1, 0);

		private bool gridMode = true;

		public int Size => Constants.RECT_SIZE;

		public bool IsAlive { get; set; } = true;
	
		public Texture2D Texture { get; set; }

		public Snake() => Tail = new List<Rectangle>
		{
			new Rectangle(position.X, position.Y, Size, Size)
		};

		public void SetupTexture(GraphicsDevice graphics)
		{
			Texture = new Texture2D(graphics, 1, 1);
			Texture.SetData(new[] { Color.White });
		}

		private bool CanMove(Styra direction) => direction switch
		{
			Styra.Up => Tail.Count == 1 || (Tail.Count > 1 && this.direction.Y != 1),
			Styra.Down => Tail.Count == 1 || (Tail.Count > 1 && this.direction.Y != -1),
			Styra.Left => Tail.Count == 1 || (Tail.Count > 1 && this.direction.X != 1),
			Styra.Right => Tail.Count == 1 || (Tail.Count > 1 && this.direction.X != -1),
			_ => false
		}; //här anger jag hur min svans ska röra på sig beroende på tangenterna

		public void ResetSnake()
		{
			Tail.Clear();

			SetStartingPosition();

			ExtendTail();
			IsAlive = true;
		}

		public void HandleUserInput(KeyboardState state)
		{
			var pressedKeyCount = state.GetPressedKeyCount();
			if (pressedKeyCount != 1)
			{
				return;
			}

			var key = state.GetPressedKeys().First();
			if (!Tangentbord.PreviouslyKeyUp(key))
				return;

			var (x, y) = GetMoveDirection(key);

			// Försöker att flytta X till Y åt samma håll sammtidigt
			if ((x, y) == (0, 0) || (x, y) == (1, 1) || (x, y) == (-1, -1))
			{
				return;
			}

			direction = (x, y);
		}

		private (int X, int Y) GetMoveDirection(Keys key) => key switch
		{
			Keys.Up => CanMove(Styra.Up) ? (0, -1) : direction,
			Keys.Down => CanMove(Styra.Down) ? (0, 1) : direction,
			Keys.Left => CanMove(Styra.Left) ? (-1, 0) : direction,
			Keys.Right => CanMove(Styra.Right) ? (1, 0) : direction,
			_ => direction
		};

		public bool CollisionWithKub(Kub kub) => position == kub.Position;

		public void ExtendTail()
		{
			Tail.Add(new Rectangle(position.X, position.Y, Size, Size));
		}

		public void SetStartingPosition()
		{
			position.X = Constants.WINDOW_SIZE / 2;
			position.Y = Constants.WINDOW_SIZE / 2;
		}

		private bool CollisionWithTailPart(Rectangle tailPart) => position.X == tailPart.X && position.Y == tailPart.Y;

		public void Update(GameTime gameTime)
		{
			lastUpdate += gameTime.ElapsedGameTime.Milliseconds;

			if (IsAlive && lastUpdate >= INTERVAL_MILLISECONDS)
			{
				lastUpdate = 0;

				position.Y += direction.Y * Size;
				position.X += direction.X * Size;

				position.Y = position.Y == -Size ? Constants.WINDOW_SIZE : position.Y == Constants.WINDOW_SIZE ? 0 : position.Y;
				position.X = position.X == -Size ? Constants.WINDOW_SIZE : position.X == Constants.WINDOW_SIZE ? 0 : position.X;

				if (Tail.Count > 1)
				{
					for (var i = Tail.Count - 1; i > 0; --i)
					{
						if (CollisionWithTailPart(Tail[i]))
						{
							IsAlive = false;
						}

						Tail[i] = new Rectangle(Tail[i - 1].X, Tail[i - 1].Y, Size, Size);
					}
				}

				Tail[0] = new Rectangle(position.X, position.Y, Size, Size);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var part in Tail)
			{
				if (gridMode)
				{
					spriteBatch.Draw(Texture, new Rectangle(part.X - 1, part.Y - 1, Size + 2, Size + 2), Color.Black);
				}

				spriteBatch.Draw(Texture, part, IsAlive ? Color.Pink : Color.LightCoral);
			}
		}
	}
}