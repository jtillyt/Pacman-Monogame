using System;

namespace Pacman
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using var game = new PacmanGame();
			game.Run();
		}
	}
}
