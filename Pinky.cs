using Microsoft.Xna.Framework;

namespace Pacman
{
	public class Pinky : Enemy
	{
		public Pinky(int tileX, int tileY, Tile[,] tileArray) : base(tileX, tileY, tileArray)
		{
			ScatterTargetTile = new Vector2(1, 2);
			type = GhostType.Pinky;

			rectsDown[0] = new Rectangle(1659, 243, 42, 42);
			rectsDown[1] = new Rectangle(1707, 243, 42, 42);

			rectsUp[0] = new Rectangle(1563, 243, 42, 42);
			rectsUp[1] = new Rectangle(1611, 243, 42, 42);

			rectsLeft[0] = new Rectangle(1467, 243, 42, 42);
			rectsLeft[1] = new Rectangle(1515, 243, 42, 42);

			rectsRight[0] = new Rectangle(1371, 243, 42, 42);
			rectsRight[1] = new Rectangle(1419, 243, 42, 42);

			enemyAnim = new SpriteAnimation(0.08f, rectsDown);
		}

		public override Vector2 getChaseTargetPosition(Vector2 playerTilePos, Direction playerDir, Tile[,] tileArray)
		{
			Vector2 pos = playerTilePos;
			Direction PlayerDir = playerDir;

			if (PlayerDir == Direction.None)
			{
				PlayerDir = playerLastDir;
			}

			switch (PlayerDir)
			{
				case Direction.Right:
					pos = new Vector2(playerTilePos.X + 4, playerTilePos.Y);
					playerLastDir = Direction.Right;
					break;
				case Direction.Left:
					pos = new Vector2(playerTilePos.X - 4, playerTilePos.Y);
					playerLastDir = Direction.Left;
					break;
				case Direction.Down:
					pos = new Vector2(playerTilePos.X, playerTilePos.Y + 4);
					playerLastDir = Direction.Down;
					break;
				case Direction.Up:
					pos = new Vector2(playerTilePos.X, playerTilePos.Y - 4);
					playerLastDir = Direction.Up;
					break;
			}
			if (pos.X < 0 || pos.Y < 0 || pos.X > GameController.NumberOfTilesX - 1 || pos.Y > GameController.NumberOfTilesY - 1)
			{
				return playerTilePos;
			}
			if (tileArray[(int)pos.X, (int)pos.Y].tileType == Tile.TileType.Wall)
			{
				return playerTilePos;
			}
			return pos;
		}
	}
}
