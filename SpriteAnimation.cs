using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
	public class SpriteAnimation
	{
		private float timer = 0;
		private readonly float threshold;
		private readonly bool isLooped = true;

		public int AnimationIndex { get; private set; } = 0;

		public bool IsPlaying { get; set; }

		public SpriteAnimation(float newThreshold, Rectangle[] newSourceRectangles)
		{
			threshold = newThreshold;
			SourceRectangles = newSourceRectangles;
			IsPlaying = true;
		}

		public SpriteAnimation(float newThreshold, Rectangle[] newSourceRectangles, int startingAnimIndex)
		{
			threshold = newThreshold;
			SourceRectangles = newSourceRectangles;
			AnimationIndex = startingAnimIndex;
			IsPlaying = true;
		}

		public SpriteAnimation(float newThreshold, Rectangle[] newSourceRectangles, int startingAnimIndex, bool newIsLooped, bool newIsPlaying)
		{
			threshold = newThreshold;
			SourceRectangles = newSourceRectangles;
			AnimationIndex = startingAnimIndex;
			isLooped = newIsLooped;
			IsPlaying = newIsPlaying;
		}

		public void setAnimIndex(int newAnimIndex)
		{
			AnimationIndex = newAnimIndex;
		}

		public void setSourceRects(Rectangle[] newSourceRects)
		{
			if (newSourceRects.Length != SourceRectangles.Length)
				AnimationIndex = 0;
			SourceRectangles = newSourceRects;
		}

		public void start()
		{
			IsPlaying = true;
			AnimationIndex = 0;
		}

		public Rectangle[] SourceRectangles { get; private set; }

		public void Update(GameTime gameTime)
		{
			if (isLooped)
			{
				timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (timer > threshold)
				{
					timer -= threshold;
					if (AnimationIndex < SourceRectangles.Length - 1)
					{
						AnimationIndex++;
					}
					else
					{
						AnimationIndex = 0;
					}
				}
				return;
			}
			// if not looped, plays animation once and then stops (by setting isPlaying to false)
			else
			{
				if (IsPlaying)
				{
					timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (timer > threshold)
					{
						timer -= threshold;
						if (AnimationIndex < SourceRectangles.Length - 1)
						{
							AnimationIndex++;
						}
						else
						{
							IsPlaying = false;
						}
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, SpriteSheet spriteSheet, Vector2 position)
		{
			if (IsPlaying)
				spriteSheet.drawSprite(spriteBatch, SourceRectangles[AnimationIndex], position);
		}
	}
}
