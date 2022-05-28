using Microsoft.Xna.Framework.Graphics;

namespace juliasfinal.GameObjects
{
	interface IGameObject
	{
		int Size { get; }

		void Draw(SpriteBatch spriteBatch);
	}
}