using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
	public class SpriteSheet
	{
		private readonly Texture2D spriteSheet;

		public SpriteSheet(Texture2D newSpriteSheet)
		{
			spriteSheet = newSpriteSheet;
		}

		public void drawSprite(SpriteBatch spriteBatch, Rectangle sourceRectangle, Vector2 position)
		{
			spriteBatch.Draw(spriteSheet, position, sourceRectangle, Color.White);
		}

		public void drawSprite(SpriteBatch spriteBatch, Rectangle sourceRectangle, Vector2 position, float scale)
		{
			spriteBatch.Draw(spriteSheet, position, sourceRectangle, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
		}
	}
}
