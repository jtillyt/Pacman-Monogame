using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
	public class Player
	{
		private readonly int _speed = 150;

		private Vector2 _position;
		private Dir _nextDirection = Dir.None;

		private static Rectangle lastRect = new Rectangle(1467, 3, 39, 39);

		public static int RadiusOffSet = 19;
		public static Rectangle[] deathAnimRect = new Rectangle[11];

		public static Rectangle[] rectsDown = new Rectangle[3];
		public static Rectangle[] rectsUp = new Rectangle[3];
		public static Rectangle[] rectsLeft = new Rectangle[3];
		public static Rectangle[] rectsRight = new Rectangle[3];

		bool canMove = true;
		float canMoveTimer = 0;
		readonly float TimerThreshold = 0.2f;

		Vector2 previousTile;
		Vector2 currentTile;

		public Player(int tileX, int tileY, Tile[,] tileArray)
		{
			_position = tileArray[tileX, tileY].Position;
			_position.X += 14;
			currentTile = new Vector2(tileX, tileY);
			previousTile = new Vector2(tileX - 1, tileY);

			rectsDown[0] = new Rectangle(1371, 147, 39, 39);
			rectsDown[1] = new Rectangle(1419, 147, 39, 39);
			rectsDown[2] = lastRect;

			rectsUp[0] = new Rectangle(1371, 99, 39, 39);
			rectsUp[1] = new Rectangle(1419, 99, 39, 39);
			rectsUp[2] = lastRect;

			rectsLeft[0] = new Rectangle(1371, 51, 39, 39);
			rectsLeft[1] = new Rectangle(1419, 51, 39, 39);
			rectsLeft[2] = lastRect;

			rectsRight[0] = new Rectangle(1371, 3, 39, 39);
			rectsRight[1] = new Rectangle(1419, 3, 39, 39);
			rectsRight[2] = lastRect;

			deathAnimRect[0] = new Rectangle(1515, 3, 39, 39);
			deathAnimRect[1] = new Rectangle(1563, 3, 39, 39);
			deathAnimRect[2] = new Rectangle(1611, 3, 39, 39);
			deathAnimRect[3] = new Rectangle(1659, 3, 39, 39);
			deathAnimRect[4] = new Rectangle(1707, 6, 39, 39);
			deathAnimRect[5] = new Rectangle(1755, 9, 39, 39);
			deathAnimRect[6] = new Rectangle(1803, 12, 39, 39);
			deathAnimRect[7] = new Rectangle(1851, 12, 39, 39);
			deathAnimRect[8] = new Rectangle(1899, 12, 39, 39);
			deathAnimRect[9] = new Rectangle(1947, 9, 39, 39);
			deathAnimRect[10] = new Rectangle(1995, 15, 39, 39);

			PlayerAnim = new SpriteAnimation(0.08f, rectsRight, 2);
		}

		public int ExtraLives { get; set; } = 0;

		public Vector2 CurrentTile
		{
			get { return currentTile; }
			set { currentTile = value; }
		}

		public Dir Direction { get; set; } = Dir.Right;

		public SpriteAnimation PlayerAnim { get; }

		public Vector2 PreviousTile
		{
			get { return previousTile; }
		}

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public void setX(float newX)
		{
			_position.X = newX;
		}

		public void setY(float newY)
		{
			_position.Y = newY;
		}

		public void Update(GameTime gameTime, Tile[,] tileArray)
		{
			GamePadState gState = GamePad.GetState(PlayerIndex.One);
			KeyboardState kState = Keyboard.GetState();

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!canMove)
			{
				canMoveTimer += dt;
				if (canMoveTimer >= TimerThreshold)
				{
					canMove = true;
					canMoveTimer = 0;
				}
			}
			PlayerAnim.Update(gameTime);

			if (kState.IsKeyDown(Keys.D) || gState.DPad.Right == ButtonState.Pressed)
			{
				_nextDirection = Dir.Right;
			}
			if (kState.IsKeyDown(Keys.A) || gState.DPad.Left == ButtonState.Pressed)
			{
				_nextDirection = Dir.Left;
			}
			if (kState.IsKeyDown(Keys.W) || gState.DPad.Up == ButtonState.Pressed)
			{
				_nextDirection = Dir.Up;
			}
			if (kState.IsKeyDown(Keys.S) || gState.DPad.Down == ButtonState.Pressed)
			{
				_nextDirection = Dir.Down;
			}

			if (canMove)
			{
				switch (_nextDirection)
				{
					case Dir.Right:
						if (PacmanGame._gameController.IsNextTileAvailable(_nextDirection, currentTile))
						{
							canMove = false;
							Direction = _nextDirection;
							_position.Y = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y + 1;
							_nextDirection = Dir.None;
						}
						break;
					case Dir.Left:
						if (PacmanGame._gameController.IsNextTileAvailable(_nextDirection, currentTile))
						{
							canMove = false;
							Direction = _nextDirection;
							_position.Y = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y + 1;
							_nextDirection = Dir.None;
						}
						break;
					case Dir.Down:
						if (PacmanGame._gameController.IsNextTileAvailable(_nextDirection, currentTile))
						{
							canMove = false;
							Direction = _nextDirection;
							_position.X = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 2;
							_nextDirection = Dir.None;
						}
						break;
					case Dir.Up:
						if (PacmanGame._gameController.IsNextTileAvailable(_nextDirection, currentTile))
						{
							canMove = false;
							Direction = _nextDirection;
							_position.X = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 2;
							_nextDirection = Dir.None;
						}
						break;
				}
			}

			if (!PacmanGame._gameController.IsNextTileAvailable(Direction, currentTile))
				Direction = Dir.None;

			switch (Direction)
			{
				case Dir.Right:
					if (PacmanGame._gameController.IsNextTileAvailable(Dir.Right, currentTile))
					{
						_position.X += _speed * dt;
						PlayerAnim.setSourceRects(rectsRight);
					}
					break;
				case Dir.Left:
					if (PacmanGame._gameController.IsNextTileAvailable(Dir.Left, currentTile))
					{
						_position.X -= _speed * dt;
						PlayerAnim.setSourceRects(rectsLeft);
					}
					break;
				case Dir.Down:
					if (PacmanGame._gameController.IsNextTileAvailable(Dir.Down, currentTile))
					{
						_position.Y += _speed * dt;
						PlayerAnim.setSourceRects(rectsDown);
					}
					break;
				case Dir.Up:
					if (PacmanGame._gameController.IsNextTileAvailable(Dir.Up, currentTile))
					{
						_position.Y -= _speed * dt;
						PlayerAnim.setSourceRects(rectsUp);
					}
					break;
				case Dir.None:
					Vector2 p = tileArray[(int)currentTile.X, (int)currentTile.Y].Position;
					_position = new Vector2(p.X + 2, p.Y + 1);
					MySounds.munchInstance.Stop();
					break;
			}
		}

		public void EatSnack(int listPosition)
		{
			PacmanGame.score += PacmanGame._gameController.SnackList[listPosition].scoreGain;

			if (PacmanGame._gameController.SnackList[listPosition].snackType == Snack.SnackType.Big)
			{
				PacmanGame._gameController.EatenBigSnack = true;
				MySounds.eat_fruit.Play();
			}

			PacmanGame._gameController.SnackList.RemoveAt(listPosition);
			MySounds.munchInstance.Play();
		}

		public void Draw(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
		{
			PlayerAnim.Draw(spriteBatch, spriteSheet, new Vector2(_position.X - RadiusOffSet / 2, _position.Y - RadiusOffSet / 2 + 1));
		}

		public int CheckForTeleportPos(Tile[,] tileArray)
		{
			if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { 0, 14 }))
			{
				if (_position.X < -30)
				{
					return 1;
				}
			}
			else if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { GameController.NumberOfTilesX - 1, 14 }))
			{
				if (_position.X > tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 30)
				{
					return 2;
				}
			}
			return 0;
		}

		public void Teleport(Vector2 pos, Vector2 tilePos)
		{
			_position = pos;
			previousTile = currentTile;
			currentTile = tilePos;
		}

		public void UpdatePlayerTilePosition(GameController controller)
		{
			var tileArray = controller.TileArray;

			tileArray[(int)previousTile.X, (int)previousTile.Y].tileType = Tile.TileType.None;
			tileArray[(int)currentTile.X, (int)currentTile.Y].tileType = Tile.TileType.Player;

			if (CheckForTeleportPos(tileArray) == 1)
			{
				if (Direction == Dir.Left)
					Teleport(new Vector2(PacmanGame.WindowWidth + 30, _position.Y), new Vector2(GameController.NumberOfTilesX - 1, 14));
			}
			else if (CheckForTeleportPos(tileArray) == 2)
			{
				if (Direction == Dir.Right)
					Teleport(new Vector2(-30, _position.Y), new Vector2(0, 14));
			}

			for (int x = 0; x < tileArray.GetLength(0); x++)
			{
				for (int y = 0; y < tileArray.GetLength(1); y++)
				{
					if (PacmanGame._gameController.CheckTileType(new Vector2(x, y), Tile.TileType.Player))
					{
						int snackListPos = PacmanGame._gameController.FindSnackListPosition(tileArray[x, y].Position);
						if (snackListPos != -1)
						{
							EatSnack(snackListPos);
						}
					}

					float tilePosX = tileArray[x, y].Position.X;
					float tilePosY = tileArray[x, y].Position.Y;

					float nextTilePosX = tileArray[x, y].Position.X + controller.TileWidth;
					float nextTilePosY = tileArray[x, y].Position.Y + controller.TileHeight;

					float pacmanPosOffSetX = RadiusOffSet / 2;
					float pacmanPosOffSetY = RadiusOffSet / 2;

					switch (Direction)
					{
						case Dir.Right:
							nextTilePosX = tileArray[x, y].Position.X + controller.TileWidth;
							break;
						case Dir.Left:
							nextTilePosX = tileArray[x, y].Position.X - controller.TileWidth;
							pacmanPosOffSetX *= -1;
							break;
						case Dir.Down:
							nextTilePosY = tileArray[x, y].Position.Y + controller.TileHeight;
							break;
						case Dir.Up:
							nextTilePosY = tileArray[x, y].Position.Y - controller.TileHeight;
							pacmanPosOffSetY *= -1;
							break;
					}

					float pacmanPosX = _position.X + pacmanPosOffSetX;
					float pacmanPosY = _position.Y + pacmanPosOffSetY;

					if (Direction == Dir.Right || Direction == Dir.Down)
					{
						if (pacmanPosX >= tilePosX && pacmanPosX < nextTilePosX)
						{
							if (pacmanPosY >= tilePosY && pacmanPosY < nextTilePosY)
							{
								previousTile = currentTile;
								currentTile = new Vector2(x, y);
								if (PacmanGame._gameController.CheckTileType(currentTile, Tile.TileType.None))
								{
									MySounds.munchInstance.Stop();
								}
							}
						}
					}
					else if (Direction == Dir.Left)
					{
						if (pacmanPosX <= tilePosX && pacmanPosX > nextTilePosX)
						{
							if (pacmanPosY >= tilePosY && pacmanPosY < nextTilePosY)
							{
								previousTile = currentTile;
								currentTile = new Vector2(x, y);
								if (PacmanGame._gameController.CheckTileType(currentTile, Tile.TileType.None))
								{
									MySounds.munchInstance.Stop();
								}
							}
						}
					}
					else if (Direction == Dir.Up)
					{
						if (pacmanPosX >= tilePosX && pacmanPosX < nextTilePosX)
						{
							if (pacmanPosY <= tilePosY && pacmanPosY > nextTilePosY)
							{
								previousTile = currentTile;
								currentTile = new Vector2(x, y);
								if (PacmanGame._gameController.CheckTileType(currentTile, Tile.TileType.None))
								{
									MySounds.munchInstance.Stop();
								}
							}
						}
					}
				}
			}
		}

		public void debugPacmanPosition(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(PacmanGame.debuggingDot, _position, Color.White);
		}
	}
}
