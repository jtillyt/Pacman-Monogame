using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{

	public class PacmanGame : Game
	{
		public static GameController _gameController;

		private Rectangle _backgroundRect = new Rectangle(684, 0, 672, 744);
		private readonly GraphicsDeviceManager _graphics;

		private SpriteBatch _spriteBatch;

		public static Texture2D GeneralSprites1;
		public static Texture2D GeneralSprites2;

		public static Texture2D debuggingDot;
		public static Texture2D debugLineX;
		public static Texture2D debugLineY;
		public static Texture2D playerDebugLineX;
		public static Texture2D playerDebugLineY;
		public static Texture2D pathfindingDebugLineX;
		public static Texture2D pathfindingDebugLineY;

		public static SpriteSheet spriteSheet1;
		public static SpriteSheet spriteSheet2;

		public static float gamePauseTimer;
		public static float gameStartSongLength;
		public static Text Text;

		private Inky _inky;
		private Blinky _blinky;
		private Clyde _clyde;
		private Pinky _pinky;

		Player Pacman;

		public static SpriteAnimation pacmanDeathAnimation;

		public static bool hasPassedInitialSong = false;
		public static bool hasPauseJustEnded;

		public static int score;

		public PacmanGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		public int ScoreOffSet { get; } = 27;
		public int WindowHeight => 744 + ScoreOffSet;
		public int WindowWidth { get; } = 672;

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = WindowWidth;
			_graphics.PreferredBackBufferHeight = WindowHeight;
			_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			MySounds.credit = Content.Load<SoundEffect>("Sounds/credit");
			MySounds.death_1 = Content.Load<SoundEffect>("Sounds/death_1");
			MySounds.death_2 = Content.Load<SoundEffect>("Sounds/death_2");
			MySounds.eat_fruit = Content.Load<SoundEffect>("Sounds/eat_fruit");
			MySounds.eat_ghost = Content.Load<SoundEffect>("Sounds/eat_ghost");
			MySounds.extend = Content.Load<SoundEffect>("Sounds/extend");
			MySounds.game_start = Content.Load<SoundEffect>("Sounds/game_start");
			MySounds.intermission = Content.Load<SoundEffect>("Sounds/intermission");

			MySounds.munch = Content.Load<SoundEffect>("Sounds/munch");
			MySounds.munchInstance = MySounds.munch.CreateInstance();
			MySounds.munchInstance.Volume = 0.35f;
			MySounds.munchInstance.IsLooped = true;

			MySounds.power_pellet = Content.Load<SoundEffect>("Sounds/power_pellet");
			MySounds.power_pellet_instance = MySounds.power_pellet.CreateInstance();
			MySounds.power_pellet_instance.IsLooped = true;

			MySounds.retreating = Content.Load<SoundEffect>("Sounds/retreating");
			MySounds.retreatingInstance = MySounds.retreating.CreateInstance();
			MySounds.retreatingInstance.IsLooped = true;

			MySounds.siren_1 = Content.Load<SoundEffect>("Sounds/siren_1");
			MySounds.siren_1_instance = MySounds.siren_1.CreateInstance();
			MySounds.siren_1_instance.Volume = 0.8f;
			MySounds.siren_1_instance.IsLooped = true;

			MySounds.siren_2 = Content.Load<SoundEffect>("Sounds/siren_2");
			MySounds.siren_3 = Content.Load<SoundEffect>("Sounds/siren_3");
			MySounds.siren_4 = Content.Load<SoundEffect>("Sounds/siren_4");
			MySounds.siren_5 = Content.Load<SoundEffect>("Sounds/siren_5");

			GeneralSprites1 = Content.Load<Texture2D>("SpriteSheets/GeneralSprites1");
			GeneralSprites2 = Content.Load<Texture2D>("SpriteSheets/GeneralSprites2");
			debuggingDot = Content.Load<Texture2D>("SpriteSheets/debuggingDot");
			debugLineX = Content.Load<Texture2D>("SpriteSheets/debugLineX");
			debugLineY = Content.Load<Texture2D>("SpriteSheets/debugLineY");
			playerDebugLineX = Content.Load<Texture2D>("SpriteSheets/playerDebugLineX");
			playerDebugLineY = Content.Load<Texture2D>("SpriteSheets/playerDebugLineY");
			pathfindingDebugLineX = Content.Load<Texture2D>("SpriteSheets/pathfindingDebugLineX");
			pathfindingDebugLineY = Content.Load<Texture2D>("SpriteSheets/pathfindingDebugLineY");
			spriteSheet1 = new SpriteSheet(GeneralSprites1);
			spriteSheet2 = new SpriteSheet(GeneralSprites2);

			Menu.setPacmanLogo = Content.Load<Texture2D>("SpriteSheets/pac-man-logo");
			Menu.setBasicFont = Content.Load<SpriteFont>("simpleFont");
			GameOver.setBasicFont = Content.Load<SpriteFont>("simpleFont");

			Text = new Text(new SpriteSheet(Content.Load<Texture2D>("Spritesheets/TextSprites")));

			_gameController = new GameController();
			_gameController.CreateGrid();

			_inky = new Inky(11, 14, _gameController.TileArray);
			_blinky = new Blinky(13, 11, _gameController.TileArray);
			_pinky = new Pinky(13, 14, _gameController.TileArray);
			_clyde = new Clyde(15, 14, _gameController.TileArray);

			Pacman = new Player(13, 23, _gameController.TileArray);

			pacmanDeathAnimation = new SpriteAnimation(0.278f, Player.deathAnimRect, 0, false, false);

			gameStartSongLength = 4.23f;
			gamePauseTimer += gameStartSongLength;
		}

		protected override void Update(GameTime gameTime)
		{
			//var caps = GamePad.GetCapabilities(0);

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (_gameController.CurrentGameState == GameController.GameState.GameOver)
			{
				GameOver.Update();
				base.Update(gameTime);
				return;
			}

			if (_gameController.CurrentGameState == GameController.GameState.Menu)
			{
				Menu.Update(gameTime);
				base.Update(gameTime);
				return;
			}

			// checks for game over
			if (Pacman.ExtraLives < 0 && !pacmanDeathAnimation.IsPlaying)
			{
				_gameController.GameOver(_inky, _blinky, _pinky, _clyde, Pacman);
			}

			// checks if game is paused, if true returns
			if (gamePauseTimer > 0)
			{
				gamePauseTimer -= dt;
				hasPassedInitialSong = true;

				pacmanDeathAnimation.Update(gameTime);

				MySounds.siren_1_instance.Stop();
				hasPauseJustEnded = true;

				base.Update(gameTime);
				return;
			}

			if (hasPauseJustEnded)
			{
				MySounds.siren_1_instance.Play();
				hasPauseJustEnded = false;
			}

			Pacman.UpdatePlayerTilePosition(_gameController);
			Pacman.Update(gameTime, _gameController.TileArray);

			_gameController.UpdateGhosts(_inky,
										_blinky,
										_pinky,
										_clyde,
										gameTime,
										Pacman,
										_blinky.CurrentTile);

			if (_gameController.StartPacmanDeathAnim)
			{
				_gameController.StartPacmanDeathAnim = false;
				pacmanDeathAnimation.start();
			}

			if (_gameController.SnackList.Count == 0)
			{
				_gameController.Win(_inky, _blinky, _pinky, _clyde, Pacman);
				gamePauseTimer = 3f;
				base.Update(gameTime);
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin();

			if (_gameController.CurrentGameState == GameController.GameState.Normal)
			{
				spriteSheet1.drawSprite(_spriteBatch, _backgroundRect, new Vector2(0, ScoreOffSet));
				Text.draw(_spriteBatch, "score - " + score, new Vector2(3, 3), 24, Text.Color.White);
				if (Pacman.ExtraLives != -1)
					Text.draw(_spriteBatch, "lives " + Pacman.ExtraLives, new Vector2(500, 3), 24, Text.Color.White);
				else
					Text.draw(_spriteBatch, "lives 0", new Vector2(500, 3), 24, Text.Color.White);

				foreach (Snack snack in _gameController.SnackList)
				{
					snack.Draw(_spriteBatch, _gameController);
				}
				if (!pacmanDeathAnimation.IsPlaying)
					Pacman.Draw(_spriteBatch, spriteSheet1);
				if (hasPassedInitialSong || score == 0)
					if (!pacmanDeathAnimation.IsPlaying)
						_gameController.DrawGhosts(_inky, _blinky, _pinky, _clyde, _spriteBatch, spriteSheet1);

				pacmanDeathAnimation.Draw(_spriteBatch, spriteSheet1, _gameController.PacmanDeathPosition);

				//gameController.drawGridDebugger(_spriteBatch);

				//gameController.drawPathFindingDebugger(_spriteBatch, inky.PathToPacMan);
				//gameController.drawPathFindingDebugger(_spriteBatch, blinky.PathToPacMan);
				//gameController.drawPathFindingDebugger(_spriteBatch, pinky.PathToPacMan);
				//gameController.drawPathFindingDebugger(_spriteBatch, clyde.PathToPacMan);

				//gameController.drawPacmanGridDebugger(_spriteBatch);
				//Pacman.debugPacmanPosition(_spriteBatch);
			}

			else if (_gameController.CurrentGameState == GameController.GameState.GameOver)
			{
				GameOver.Draw(_spriteBatch, Text);
			}

			else if (_gameController.CurrentGameState == GameController.GameState.Menu)
			{
				Menu.Draw(_spriteBatch);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
