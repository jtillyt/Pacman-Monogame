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
			KeyboardState kState = Keyboard.GetState();
			if (kState.IsKeyDown(Keys.Space))
			{
				_pacmanGame.GameController.CurrentGameState = GameController.GameState.Menu;
			}
		}

		public void Draw(SpriteBatch spriteBatch, Text text)
		{
			text.draw(spriteBatch, "game over!", new Vector2(100, 321), 48, Text.Color.Red, 2f);
			spriteBatch.DrawString(BasicFont, "PRESS SPACE TO GO TO MENU", BasicFontPos, Color.Red);
		}
	}
}
