using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
	public class Menu
	{
		private readonly Rectangle _pacmanLogoPos = new Rectangle(13, 40, 4530 / 7, 1184 / 7);
		private readonly Vector2 _lineOne = new Vector2(150, 400);
		private readonly PacmanGame _pacmanGame;
		private SpriteFont _basicFont;
		private Texture2D _pacmanLogo;

		public Menu(PacmanGame pacmanGame)
		{
			_pacmanGame = pacmanGame;
		}

		public SpriteFont BasicFont
		{
			set { _basicFont = value; }
		}

		public Texture2D PacmanLogo
		{
			set { _pacmanLogo = value; }
		}

		public void Update(GameTime gameTime)
		{
			GamePadState gState = GamePad.GetState(PlayerIndex.One);

			KeyboardState kState = Keyboard.GetState();

			if (kState.IsKeyDown(Keys.Enter) || gState.IsButtonDown(Buttons.Start))
			{
				_pacmanGame.GameController.CurrentGameState = GameController.GameState.Normal;
				MySounds.game_start.Play();
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(_basicFont, "PRESS ENTER OR START", _lineOne, Color.Red);
			spriteBatch.Draw(_pacmanLogo, _pacmanLogoPos, Color.White);
		}
	}
}
