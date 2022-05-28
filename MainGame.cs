using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using juliasfinal.GameObjects;
using juliasfinal.Verktyg;
using juliasfinal.Verktyg.Saves;

namespace juliasfinal
{
	class MainGame : Game
	{
		private readonly GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private readonly Snake snake;
		private readonly Kub kub;

		private SpriteFont scoreFont;
		private SpriteFont gameOverFont;

		private Hiscore hiscore;
		private bool started;
		private int score;
		private bool drawGrid;
		//Deklarerat 
		public MainGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferHeight = Constants.WINDOW_SIZE;
			graphics.PreferredBackBufferWidth = Constants.WINDOW_SIZE;

			IsMouseVisible = true;

			snake = new Snake();
			kub = new Kub();
		}

		protected override void Initialize()
		{
			snake.SetStartingPosition();
			kub.ResetToRandomPosition(snake.Tail);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			hiscore = SparatResultat.Load();

			scoreFont = Content.Load<SpriteFont>("ScoreFont");
			gameOverFont = Content.Load<SpriteFont>("GameOverFont");

			snake.SetupTexture(GraphicsDevice);
			kub.SetupTexture(GraphicsDevice);
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			if (score > hiscore.PersonalBest)
			{
				hiscore.PersonalBest = score;
				SparatResultat.Save(hiscore);
			}

			base.OnExiting(sender, args);
		}

		protected override void Update(GameTime gameTime)
		{
			var state = Tangentbord.GetState();

			if (Tangentbord.IsKeyPress(Keys.Escape))
			{
				Exit();
				return;
			}

			if (!started && Tangentbord.IsKeyPress(Keys.Space))
			{
				started = true;
			}

			if (Tangentbord.IsKeyPress(Keys.G))
			{
				drawGrid = !drawGrid;
			}

			snake.Update(gameTime);

			if (!snake.IsAlive && Tangentbord.IsKeyPress(Keys.Space))
			{
				snake.ResetSnake();
				score = 0;
			}

			snake.HandleUserInput(state);

			if (snake.CollisionWithKub(kub))
			{
				kub.ResetToRandomPosition(snake.Tail);
				snake.ExtendTail();
				score++;
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

			if (started)
			{
				snake.Draw(spriteBatch);
				kub.Draw(spriteBatch);

				spriteBatch.DrawString(scoreFont, $"Current Score: {score}", new Vector2(10, 10), Color.Pink);
				spriteBatch.DrawString(scoreFont, $"Highscore: {hiscore.PersonalBest}", new Vector2(10, 26), Color.Pink);

				if (!snake.IsAlive)
				{
					spriteBatch.DrawCenteredString(gameOverFont, $"Game over\nPress Speace to restart.", Color.Pink);
				}
				else
				{
					if (drawGrid)
					{
						spriteBatch.DrawGrid();
					}
				} 
			}
			else
			{
				spriteBatch.DrawCenteredStringWithOffset(gameOverFont, "Klicka pa mellanslag for att starta.", Color.Pink, (0, -100));
				spriteBatch.DrawCenteredStringWithOffset(scoreFont, string.Join("\n", new[]
				{
					"           GOOD LUCK",
					"-------------------------------------",
					"",
					"",
					"",
					"",
					"",
					"", //här blev det lite knasigt men min dator får spel annars
					""
				}), Color.Pink, (0, 70));
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}