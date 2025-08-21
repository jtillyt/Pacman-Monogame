using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
	public class GameOver
	{
		private readonly PacmanGame _pacmanGame;

		public GameOver(PacmanGame pacmanGame)
		{
			_pacmanGame = pacmanGame;
		}

		private Vector2 BasicFontPos { get; } = new Vector2(93, 400);

		public SpriteFont BasicFont { get; set; }

		public void Update()
		{
			GamePadState gState = GamePad.GetState(PlayerIndex.One);
			KeyboardState kState = Keyboard.GetState();

			if (kState.IsKeyDown(Keys.Space) || gState.IsButtonDown(Buttons.Start))
			{
				_pacmanGame.GameController.CurrentGameState = GameController.GameState.Menu;
			}
		}

		public void Draw(SpriteBatch spriteBatch, Text text)
		{
			text.draw(spriteBatch, "Game Over!", new Vector2(100, 321), 48, Text.Color.Red, 2f);
			spriteBatch.DrawString(BasicFont, "Press SPACE or START", BasicFontPos, Color.Red);
		}
	}
}
