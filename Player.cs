using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
	public class Player
	{
		private Vector2 position;
		private Dir nextDirection = Dir.None;
		private readonly int speed = 150;
		public static int radiusOffSet = 19;
		public static Rectangle[] deathAnimRect = new Rectangle[11];
		private static Rectangle lastRect = new Rectangle(1467, 3, 39, 39);
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
			position = tileArray[tileX, tileY].Position;
			position.X += 14;
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
			get { return position; }
			set { position = value; }
		}

		public void setX(float newX)
		{
			position.X = newX;
		}

		public void setY(float newY)
		{
			position.Y = newY;
		}

		public void Update(GameTime gameTime, Tile[,] tileArray)
		{
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

			if (kState.IsKeyDown(Keys.D))
			{
				nextDirection = Dir.Right;
			}
			if (kState.IsKeyDown(Keys.A))
			{
				nextDirection = Dir.Left;
			}
			if (kState.IsKeyDown(Keys.W))
			{
				nextDirection = Dir.Up;
			}
			if (kState.IsKeyDown(Keys.S))
			{
				nextDirection = Dir.Down;
			}

			if (canMove)
			{
				switch (nextDirection)
				{
					case Dir.Right:
						if (Game1.gameController.IsNextTileAvailable(nextDirection, currentTile))
						{
							canMove = false;
							Direction = nextDirection;
							position.Y = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y + 1;
							nextDirection = Dir.None;
						}
						break;
					case Dir.Left:
						if (Game1.gameController.IsNextTileAvailable(nextDirection, currentTile))
						{
							canMove = false;
							Direction = nextDirection;
							position.Y = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.Y + 1;
							nextDirection = Dir.None;
						}
						break;
					case Dir.Down:
						if (Game1.gameController.IsNextTileAvailable(nextDirection, currentTile))
						{
							canMove = false;
							Direction = nextDirection;
							position.X = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 2;
							nextDirection = Dir.None;
						}
						break;
					case Dir.Up:
						if (Game1.gameController.IsNextTileAvailable(nextDirection, currentTile))
						{
							canMove = false;
							Direction = nextDirection;
							position.X = tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 2;
							nextDirection = Dir.None;
						}
						break;
				}
			}

			if (!Game1.gameController.IsNextTileAvailable(Direction, currentTile))
				Direction = Dir.None;

			switch (Direction)
			{
				case Dir.Right:
					if (Game1.gameController.IsNextTileAvailable(Dir.Right, currentTile))
					{
						position.X += speed * dt;
						PlayerAnim.setSourceRects(rectsRight);
					}
					break;
				case Dir.Left:
					if (Game1.gameController.IsNextTileAvailable(Dir.Left, currentTile))
					{
						position.X -= speed * dt;
						PlayerAnim.setSourceRects(rectsLeft);
					}
					break;
				case Dir.Down:
					if (Game1.gameController.IsNextTileAvailable(Dir.Down, currentTile))
					{
						position.Y += speed * dt;
						PlayerAnim.setSourceRects(rectsDown);
					}
					break;
				case Dir.Up:
					if (Game1.gameController.IsNextTileAvailable(Dir.Up, currentTile))
					{
						position.Y -= speed * dt;
						PlayerAnim.setSourceRects(rectsUp);
					}
					break;
				case Dir.None:
					Vector2 p = tileArray[(int)currentTile.X, (int)currentTile.Y].Position;
					position = new Vector2(p.X + 2, p.Y + 1);
					MySounds.munchInstance.Stop();
					break;
			}
		}

		public void eatSnack(int listPosition)
		{
			Game1.score += Game1.gameController.SnackList[listPosition].scoreGain;

			if (Game1.gameController.SnackList[listPosition].snackType == Snack.SnackType.Big)
			{
				Game1.gameController.EatenBigSnack = true;
				MySounds.eat_fruit.Play();
			}

			Game1.gameController.SnackList.RemoveAt(listPosition);
			MySounds.munchInstance.Play();
		}

		public void Draw(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
		{
			PlayerAnim.Draw(spriteBatch, spriteSheet, new Vector2(position.X - radiusOffSet / 2, position.Y - radiusOffSet / 2 + 1));
		}

		public int checkForTeleportPos(Tile[,] tileArray)
		{
			if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { 0, 14 }))
			{
				if (position.X < -30)
				{
					return 1;
				}
			}
			else if (new int[2] { (int)currentTile.X, (int)currentTile.Y }.SequenceEqual(new int[2] { Controller.NumberOfTilesX - 1, 14 }))
			{
				if (position.X > tileArray[(int)currentTile.X, (int)currentTile.Y].Position.X + 30)
				{
					return 2;
				}
			}
			return 0;
		}

		public void teleport(Vector2 pos, Vector2 tilePos)
		{
			position = pos;
			previousTile = currentTile;
			currentTile = tilePos;
		}

		public void UpdatePlayerTilePosition(Controller controller)
		{
			var tileArray = controller.TileArray;

			tileArray[(int)previousTile.X, (int)previousTile.Y].tileType = Tile.TileType.None;
			tileArray[(int)currentTile.X, (int)currentTile.Y].tileType = Tile.TileType.Player;

			if (checkForTeleportPos(tileArray) == 1)
			{
				if (Direction == Dir.Left)
					teleport(new Vector2(Game1.windowWidth + 30, position.Y), new Vector2(Controller.NumberOfTilesX - 1, 14));
			}
			else if (checkForTeleportPos(tileArray) == 2)
			{
				if (Direction == Dir.Right)
					teleport(new Vector2(-30, position.Y), new Vector2(0, 14));
			}

			for (int x = 0; x < tileArray.GetLength(0); x++)
			{
				for (int y = 0; y < tileArray.GetLength(1); y++)
				{
					if (Game1.gameController.CheckTileType(new Vector2(x, y), Tile.TileType.Player))
					{
						int snackListPos = Game1.gameController.FindSnackListPosition(tileArray[x, y].Position);
						if (snackListPos != -1)
						{
							eatSnack(snackListPos);
						}
					}

					float tilePosX = tileArray[x, y].Position.X;
					float tilePosY = tileArray[x, y].Position.Y;

					float nextTilePosX = tileArray[x, y].Position.X + controller.TileWidth;
					float nextTilePosY = tileArray[x, y].Position.Y + controller.TileHeight;

					float pacmanPosOffSetX = radiusOffSet / 2;
					float pacmanPosOffSetY = radiusOffSet / 2;

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

					float pacmanPosX = position.X + pacmanPosOffSetX;
					float pacmanPosY = position.Y + pacmanPosOffSetY;

					if (Direction == Dir.Right || Direction == Dir.Down)
					{
						if (pacmanPosX >= tilePosX && pacmanPosX < nextTilePosX)
						{
							if (pacmanPosY >= tilePosY && pacmanPosY < nextTilePosY)
							{
								previousTile = currentTile;
								currentTile = new Vector2(x, y);
								if (Game1.gameController.CheckTileType(currentTile, Tile.TileType.None))
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
								if (Game1.gameController.CheckTileType(currentTile, Tile.TileType.None))
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
								if (Game1.gameController.CheckTileType(currentTile, Tile.TileType.None))
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
			spriteBatch.Draw(Game1.debuggingDot, position, Color.White);
		}
	}
}
