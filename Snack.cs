using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
	public class Snack
	{
		public enum SnackType { Small, Big };
		public SnackType snackType;
		private Vector2 gridPosition;
		private readonly int[] gridTile;
		public int scoreGain;
		private Rectangle smallSnackRect = new Rectangle(33, 33, 6, 6);
		private Rectangle bigSnackRect = new Rectangle(24, 72, 24, 24);
		private readonly int radiusOffSet;
		private int timerBigSnack = 20;

		public Vector2 Position
		{
			get { return gridPosition; }
		}

		public Snack(SnackType newSnackType, Vector2 newPosition, int[] newGridTile)
		{
			snackType = newSnackType;
			if (newSnackType == SnackType.Big)
			{
				scoreGain = 50;
				radiusOffSet = 12;
			}
			else
			{
				scoreGain = 10;
				radiusOffSet = 3;
			}

			gridPosition = newPosition;
			gridTile = newGridTile;
		}

		public void Draw(SpriteBatch spriteBatch, Controller controller)
		{
			if (snackType == SnackType.Small)
				Game1.spriteSheet1.drawSprite(spriteBatch, smallSnackRect, new Vector2(gridPosition.X + controller.TileWidth / 2 - radiusOffSet, gridPosition.Y + controller.TileHeight / 2 - radiusOffSet));
			else
			{
				if (timerBigSnack >= 10 || Game1.gamePauseTimer > 0)
					Game1.spriteSheet1.drawSprite(spriteBatch, bigSnackRect, new Vector2(gridPosition.X + controller.TileWidth / 2 - radiusOffSet, gridPosition.Y + controller.TileHeight / 2 - radiusOffSet));
				timerBigSnack -= 1;
				if (timerBigSnack < 0)
				{
					timerBigSnack = 20;
				}
			}
		}
	}
}
