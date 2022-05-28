using Microsoft.Xna.Framework.Graphics;

namespace juliasfinal.GameObjects
{
	interface SpelObjekt
	{
		int Size { get; }

		void Draw(SpriteBatch spriteBatch);
		//här ritas min sprite ut!
	}
}