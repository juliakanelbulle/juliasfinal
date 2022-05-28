using Microsoft.Xna.Framework.Input;

namespace juliasfinal.Verktyg
{
	static class Tangentbord
	{
		static KeyboardState currentState;

		static KeyboardState previousState;
		public static KeyboardState GetState()
		{
			previousState = currentState;
			currentState = Keyboard.GetState();

			return currentState;
		}

		public static bool PreviouslyKeyUp(Keys key) => previousState.IsKeyUp(key);
		public static bool IsKeyPress(Keys key) => currentState.IsKeyDown(key) && previousState.IsKeyUp(key);
	}
}