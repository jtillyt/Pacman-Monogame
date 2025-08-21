using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pacman
{
	public class GameController
	{
		public const int NumberOfTilesX = 28;
		public const int NumberOfTilesY = 31;

		private const float GhostInitialTimerLength = 2f;
		private const float GhostTimerChaserLength = 20f;
		private const float GhostTimerScatterLength = 15f;

		private readonly PacmanGame _pacmanGame;

		private readonly int[,] _mapDesign = new int[,] {
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1 },
			{ 1, 3, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 3, 1 },
			{ 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 5, 1, 1, 5, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 5, 1, 1, 5, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 1, 1, 1, 2, 2, 1, 1, 1, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 1, 2, 2, 2, 2, 2, 2, 1, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 1, 2, 2, 2, 2, 2, 2, 1, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 1, 2, 2, 2, 2, 2, 2, 1, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1 },
			{ 1, 3, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 5, 5, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 3, 1 },
			{ 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1 },
			{ 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
			{ 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
		};

		private float _ghostInitialTimer = 0f;
		public float _ghostTimerChaser = 0f;
		public float _ghostTimerScatter = 0f;

		public int TileHeight {get; init; }
		public int TileWidth {get; init; }

		public bool EatenBigSnack { get; set; } = false;

		public Enemy.EnemyState EnemiesState { get; set; } = Enemy.EnemyState.Scatter;

		public GameState CurrentGameState { get; set; } = GameState.Menu;

		public int GhostScoreMultiplier { get; set; } = 1;

		public Vector2 PacmanDeathPosition;

		public List<Snack> SnackList { get; set; } = new List<Snack>();

		public bool StartPacmanDeathAnim { get; set; } = false;

		public Tile[,] TileArray;

		public GameController(PacmanGame pacmanGame)
		{
			_pacmanGame = pacmanGame;

			TileWidth = _pacmanGame.WindowWidth / NumberOfTilesX;
			TileHeight = (_pacmanGame.WindowHeight - _pacmanGame.ScoreOffSet) / NumberOfTilesY;
			TileArray = new Tile[NumberOfTilesX, NumberOfTilesY];
		}

		public bool CheckTileType(Vector2 gridIndex, Tile.TileType tileType)
		{
			bool tile = false;
			if (TileArray[(int)gridIndex.X, (int)gridIndex.Y].tileType == tileType)
			{
				tile = true;
			}
			return tile;
		}

		public void CreateGrid() // creates grid that contains Tile objects, which represent 24x24 pixels squares in the game, all with types such as walls, snacks and etc.
		{
			for (int y = 0; y < NumberOfTilesY; y++)
			{
				for (int x = 0; x < NumberOfTilesX; x++)
				{
					if (_mapDesign[y, x] == 0) // small snack
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												Tile.TileType.Snack);
						TileArray[x, y].isEmpty = false;
						SnackList.Add(new Snack(Snack.SnackType.Small,
												new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												new int[] { x, y }));
					}
					else if (_mapDesign[y, x] == 1) // wall collider
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												Tile.TileType.Wall);
						TileArray[x, y].isEmpty = false;
					}
					else if (_mapDesign[y, x] == 2) //  ghost house
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												Tile.TileType.GhostHouse);
						TileArray[x, y].isEmpty = false;
					}
					else if (_mapDesign[y, x] == 3) // big snack
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												Tile.TileType.Snack);
						TileArray[x, y].isEmpty = false;
						SnackList.Add(new Snack(Snack.SnackType.Big,
												new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												new int[] { x, y }));
					}
					else if (_mapDesign[y, x] == 5) // empty
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet));
					}
				}
			}
		}

		// creates snacks again for when the player eats all snacks on the screen
		public void CreateSnacks()
		{
			for (int y = 0; y < NumberOfTilesY; y++)
			{
				for (int x = 0; x < NumberOfTilesX; x++)
				{
					if (_mapDesign[y, x] == 0) // small snack
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												Tile.TileType.Snack);
						TileArray[x, y].isEmpty = false;
						SnackList.Add(new Snack(Snack.SnackType.Small,
												new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												new int[] { x, y }));
					}
					else if (_mapDesign[y, x] == 3) // big snack
					{
						TileArray[x, y] = new Tile(new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												Tile.TileType.Snack);
						TileArray[x, y].isEmpty = false;
						SnackList.Add(new Snack(Snack.SnackType.Big,
												new Vector2(x * TileWidth, (y * TileHeight) + _pacmanGame.ScoreOffSet),
												new int[] { x, y }));
					}
				}
			}
		}

		public void DrawGhosts(Inky i, Blinky b, Pinky p, Clyde c, SpriteBatch spriteBatch, SpriteSheet spriteSheet)
		{
			i.Draw(spriteBatch, spriteSheet);
			b.Draw(spriteBatch, spriteSheet);
			p.Draw(spriteBatch, spriteSheet);
			c.Draw(spriteBatch, spriteSheet);
		}

		public void DrawGridDebugger(SpriteBatch spriteBatch)
		{
			for (int x = 0; x < NumberOfTilesX; x++)
			{
				for (int y = 0; y < NumberOfTilesY; y++)
				{
					Vector2 dotPosition = TileArray[x, y].Position;
					spriteBatch.Draw(PacmanGame.debugLineX, dotPosition, Color.White);
					spriteBatch.Draw(PacmanGame.debugLineY, dotPosition, Color.White);
				}
			}
		}

		public void DrawPacmanGridDebugger(SpriteBatch spriteBatch)
		{
			for (int x = 0; x < NumberOfTilesX; x++)
			{
				for (int y = 0; y < NumberOfTilesY; y++)
				{
					Vector2 dotPosition = TileArray[x, y].Position;
					if (TileArray[x, y].tileType == Tile.TileType.Player)
					{
						spriteBatch.Draw(PacmanGame.playerDebugLineX, dotPosition, Color.White);
						spriteBatch.Draw(PacmanGame.playerDebugLineY, dotPosition, Color.White);
						spriteBatch.Draw(PacmanGame.playerDebugLineX,
										new Vector2(dotPosition.X, dotPosition.Y + 24),
										Color.White);
						spriteBatch.Draw(PacmanGame.playerDebugLineY,
										new Vector2(dotPosition.X + 24, dotPosition.Y),
										Color.White);
					}
				}
			}
		}

		public void DrawPathFindingDebugger(SpriteBatch spriteBatch, List<Vector2> path)
		{
			if (path == null)
			{
				return;
			}

			foreach (Vector2 gridPos in path)
			{
				Vector2 pos = TileArray[(int)gridPos.X, (int)gridPos.Y].Position;
				spriteBatch.Draw(PacmanGame.pathfindingDebugLineX, pos, Color.White);
				spriteBatch.Draw(PacmanGame.pathfindingDebugLineY, pos, Color.White);
				spriteBatch.Draw(PacmanGame.pathfindingDebugLineX, new Vector2(pos.X, pos.Y + 24), Color.White);
				spriteBatch.Draw(PacmanGame.pathfindingDebugLineY, new Vector2(pos.X + 24, pos.Y), Color.White);
			}
		}

		public int FindSnackListPosition(Vector2 snackGridPos)
		{
			int listPosition = -1;
			foreach (Snack snack in SnackList)
			{
				if (snack.Position == snackGridPos)
				{
					listPosition = SnackList.IndexOf(snack);
				}
			}

			return listPosition;
		}

		public void GameOver(Inky i, Blinky b, Pinky p, Clyde c, Player pacman)
		{
			CurrentGameState = GameState.GameOver;

			PacmanGame.hasPassedInitialSong = false;
			PacmanGame.score = 0;
			PacmanGame.pacmanDeathAnimation.IsPlaying = false;
			PacmanGame.gamePauseTimer = PacmanGame.gameStartSongLength;
			pacman.ExtraLives = 4;

			CreateSnacks();
			resetGhosts(i, b, p, c);

			_ghostTimerChaser = 0;
			_ghostTimerScatter = 0;
			_ghostInitialTimer = 0;

			EatenBigSnack = false;

			pacman.Position = new Vector2(TileArray[13, 23].Position.X + 14, TileArray[13, 23].Position.Y);
			pacman.CurrentTile = new Vector2(13, 23);
			pacman.PlayerAnim.setSourceRects(Player.rectsRight);
			pacman.PlayerAnim.setAnimIndex(2);
			pacman.Direction = Direction.Right;

			MySounds.munchInstance.Stop();
			MySounds.power_pellet_instance.Stop();
			MySounds.retreatingInstance.Stop();
		}

		public bool IsNextTileAvailable(Direction dir, Vector2 tile)
		{ // tile != new int[2] {0, 14} && tile != new int[2] {numberOfTilesX-1 ,14}
			if (tile.Equals(new Vector2(0, 14)) || tile.Equals(new Vector2(NumberOfTilesX - 1, 14)))
			{
				if ((dir == Direction.Right) || (dir == Direction.Left))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				switch (dir)
				{
					case Direction.Right:
						if ((TileArray[((int)tile.X) + 1, (int)tile.Y].tileType == Tile.TileType.Wall) ||
							(TileArray[((int)tile.X) + 1, (int)tile.Y].tileType == Tile.TileType.GhostHouse))
						{
							return false;
						}
						break;

					case Direction.Left:
						if ((TileArray[((int)tile.X) - 1, (int)tile.Y].tileType == Tile.TileType.Wall) ||
							(TileArray[((int)tile.X) - 1, (int)tile.Y].tileType == Tile.TileType.GhostHouse))
						{
							return false;
						}
						break;

					case Direction.Down:
						if ((TileArray[(int)tile.X, ((int)tile.Y) + 1].tileType == Tile.TileType.Wall) ||
							(TileArray[(int)tile.X, ((int)tile.Y) + 1].tileType == Tile.TileType.GhostHouse))
						{
							return false;
						}
						break;

					case Direction.Up:
						if ((TileArray[(int)tile.X, ((int)tile.Y) - 1].tileType == Tile.TileType.Wall) ||
							(TileArray[(int)tile.X, ((int)tile.Y) - 1].tileType == Tile.TileType.GhostHouse))
						{
							return false;
						}
						break;
				}
				return true;
			}
		}

		public bool IsNextTileAvailableGhosts(Direction dir, Vector2 tile)
		{ // tile != new int[2] {0, 14} && tile != new int[2] {numberOfTilesX-1 ,14}
			if (tile.Equals(new Vector2(0, 14)) || tile.Equals(new Vector2(NumberOfTilesX - 1, 14)))
			{
				if ((dir == Direction.Right) || (dir == Direction.Left))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				switch (dir)
				{
					case Direction.Right:
						if (TileArray[((int)tile.X) + 1, (int)tile.Y].tileType == Tile.TileType.Wall)
						{
							return false;
						}
						break;

					case Direction.Left:
						if (TileArray[((int)tile.X) - 1, (int)tile.Y].tileType == Tile.TileType.Wall)
						{
							return false;
						}
						break;

					case Direction.Down:
						if (TileArray[(int)tile.X, ((int)tile.Y) + 1].tileType == Tile.TileType.Wall)
						{
							return false;
						}
						break;

					case Direction.Up:
						if (TileArray[(int)tile.X, ((int)tile.Y) - 1].tileType == Tile.TileType.Wall)
						{
							return false;
						}
						break;
				}
				return true;
			}
		}

		public void KillPacman(Inky i, Blinky b, Pinky p, Clyde c, Player pacman)
		{
			pacman.ExtraLives -= 1;
			StartPacmanDeathAnim = true;
			PacmanDeathPosition = new Vector2(pacman.Position.X - (Player.RadiusOffSet / 2),
											(pacman.Position.Y - (Player.RadiusOffSet / 2)) + 1);
			MySounds.death_1.Play(); //Length = 2.78
			PacmanGame.gamePauseTimer = 4f;

			resetGhosts(i, b, p, c);

			_ghostTimerChaser = 0;
			_ghostTimerScatter = 0;
			_ghostInitialTimer = 0;

			EatenBigSnack = false;

			pacman.Position = new Vector2(TileArray[13, 23].Position.X + 14, TileArray[13, 23].Position.Y);
			pacman.CurrentTile = new Vector2(13, 23);
			pacman.PlayerAnim.setSourceRects(Player.rectsRight);
			pacman.PlayerAnim.setAnimIndex(2);
			pacman.Direction = Direction.Right;

			MySounds.munchInstance.Stop();
			MySounds.power_pellet_instance.Stop();
			MySounds.retreatingInstance.Stop();
		}

		public void resetGhosts(Inky i, Blinky b, Pinky p, Clyde c)
		{
			_ghostInitialTimer = 0;

			SetGhostStates(i, b, p, c, Enemy.EnemyState.Scatter);

			i.EnemyAnim.setSourceRects(i.rectsUp);
			b.EnemyAnim.setSourceRects(b.rectsLeft);
			p.EnemyAnim.setSourceRects(p.rectsDown);
			c.EnemyAnim.setSourceRects(c.rectsUp);

			i.timerFrightened = 0;
			b.timerFrightened = 0;
			p.timerFrightened = 0;
			c.timerFrightened = 0;

			i.PathToPacMan = new List<Vector2>();
			b.PathToPacMan = new List<Vector2>();
			p.PathToPacMan = new List<Vector2>();
			c.PathToPacMan = new List<Vector2>();

			i.Position = new Vector2(TileArray[11, 14].Position.X + 12, TileArray[11, 14].Position.Y);
			b.Position = new Vector2(TileArray[13, 11].Position.X + 12, TileArray[13, 11].Position.Y);
			p.Position = new Vector2(TileArray[13, 14].Position.X + 12, TileArray[13, 14].Position.Y);
			c.Position = new Vector2(TileArray[15, 14].Position.X + 12, TileArray[15, 14].Position.Y);
		}

		public static Direction ReturnOppositeDir(Direction dir)
		{
			switch (dir)
			{
				case Direction.Up:
					return Direction.Down;

				case Direction.Down:
					return Direction.Up;

				case Direction.Right:
					return Direction.Left;

				case Direction.Left:
					return Direction.Right;
			}
			return Direction.None;
		}

		public void SetGhostStates(Inky i, Blinky b, Pinky p, Clyde c, Enemy.EnemyState eState)
		{
			if ((eState == Enemy.EnemyState.Chase) ||
				(eState == Enemy.EnemyState.Scatter) ||
				(eState == Enemy.EnemyState.Eaten))
			{
				i.speed = i.normalSpeed;
				b.speed = b.normalSpeed;
				p.speed = p.normalSpeed;
				c.speed = c.normalSpeed;
			}
			else
			{
				if (i.state != Enemy.EnemyState.Eaten)
				{
					i.speed = i.frightenedSpeed;
				}

				if (b.state != Enemy.EnemyState.Eaten)
				{
					b.speed = b.frightenedSpeed;
				}

				if (p.state != Enemy.EnemyState.Eaten)
				{
					p.speed = p.frightenedSpeed;
				}

				if (c.state != Enemy.EnemyState.Eaten)
				{
					c.speed = c.frightenedSpeed;
				}
			}
			if (i.state != Enemy.EnemyState.Eaten)
			{
				i.state = eState;
			}

			if (b.state != Enemy.EnemyState.Eaten)
			{
				b.state = eState;
			}

			if (p.state != Enemy.EnemyState.Eaten)
			{
				p.state = eState;
			}

			if (c.state != Enemy.EnemyState.Eaten)
			{
				c.state = eState;
			}
		}

		public void SwitchBetweenStates(Inky i, Blinky b, Pinky p, Clyde c, GameTime gameTime)
		{
			if ((i.state == Enemy.EnemyState.Frightened) ||
				(i.state == Enemy.EnemyState.Eaten) ||
				(b.state == Enemy.EnemyState.Frightened) ||
				(b.state == Enemy.EnemyState.Eaten) ||
				(c.state == Enemy.EnemyState.Frightened) ||
				(c.state == Enemy.EnemyState.Eaten) ||
				(p.state == Enemy.EnemyState.Frightened) ||
				(p.state == Enemy.EnemyState.Eaten))
			{
				return;
			}

			if (EnemiesState == Enemy.EnemyState.Scatter)
			{
				_ghostTimerScatter += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_ghostTimerScatter > GhostTimerScatterLength)
				{
					_ghostTimerScatter = 0;
					EnemiesState = Enemy.EnemyState.Chase;
					SetGhostStates(i, b, p, c, Enemy.EnemyState.Chase);
				}
			}
			else if (EnemiesState == Enemy.EnemyState.Chase)
			{
				_ghostTimerChaser += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_ghostTimerChaser > GhostTimerChaserLength)
				{
					_ghostTimerChaser = 0;
					EnemiesState = Enemy.EnemyState.Scatter;
					SetGhostStates(i, b, p, c, Enemy.EnemyState.Scatter);
				}
			}
		}

		public void UpdateGhosts(Inky i,
								Blinky b,
								Pinky p,
								Clyde c,
								GameTime gameTime,
								Player Pacman,
								Vector2 blinkyPos)
		{
			if (EatenBigSnack)
			{
				EatenBigSnack = false;
				SetGhostStates(i, b, p, c, Enemy.EnemyState.Frightened);
				MySounds.power_pellet_instance.Play();
			}

			if ((i.state != Enemy.EnemyState.Frightened) &&
				(b.state != Enemy.EnemyState.Frightened) &&
				(p.state != Enemy.EnemyState.Frightened) &&
				(c.state != Enemy.EnemyState.Frightened))
			{
				MySounds.power_pellet_instance.Stop();
				GhostScoreMultiplier = 1;
			}
			if ((i.state != Enemy.EnemyState.Eaten) &&
				(b.state != Enemy.EnemyState.Eaten) &&
				(p.state != Enemy.EnemyState.Eaten) &&
				(c.state != Enemy.EnemyState.Eaten))
			{
				MySounds.retreatingInstance.Stop();
			}

			if (_ghostInitialTimer < GhostInitialTimerLength)
			{
				_ghostInitialTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				c.EnemyAnim.Update(gameTime);
				i.EnemyAnim.Update(gameTime);
			}
			if ((_ghostInitialTimer > GhostInitialTimerLength / 2) && (_ghostInitialTimer < GhostInitialTimerLength))
			{
				i.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
			}
			else if (_ghostInitialTimer > GhostInitialTimerLength) // When Initial timer ends, starts the timers to switch from scatter to chaser
			{
				c.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
				i.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
				SwitchBetweenStates(i, b, p, c, gameTime);
			}

			p.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);
			b.Update(gameTime, this, Pacman.CurrentTile, Pacman.Direction, blinkyPos);

			if ((i.colliding == true) || (b.colliding == true) || (p.colliding == true) || (c.colliding == true))
			{
				// killPacman(i, b, p, c, Pacman);
				i.colliding = false;
				b.colliding = false;
				p.colliding = false;
				c.colliding = false;
			}
		}

		public void Win(Inky i, Blinky b, Pinky p, Clyde c, Player pacman)
		{
			CreateSnacks();
			resetGhosts(i, b, p, c);

			_ghostTimerChaser = 0;
			_ghostTimerScatter = 0;
			_ghostInitialTimer = 0;

			EatenBigSnack = false;

			pacman.Position = new Vector2(TileArray[13, 23].Position.X + 14, TileArray[13, 23].Position.Y);
			pacman.CurrentTile = new Vector2(13, 23);
			pacman.PlayerAnim.setSourceRects(Player.rectsRight);
			pacman.PlayerAnim.setAnimIndex(2);
			pacman.Direction = Direction.Right;

			MySounds.munchInstance.Stop();
			MySounds.power_pellet_instance.Stop();
			MySounds.retreatingInstance.Stop();
		}

		public enum GameState
		{
			Normal,
			GameOver,
			Menu
		}

;
	}
}