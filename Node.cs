﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Pacman
{
	public class Node
	{
		public int gCost;
		public int hCost;

		public Node parent;
		public Direction ignoreDirection = Direction.None;

		public void setIgnoreDirection(Direction currentDir)
		{
			switch (currentDir)
			{
				case Direction.Right:
					ignoreDirection = Direction.Left;
					break;
				case Direction.Left:
					ignoreDirection = Direction.Right;
					break;
				case Direction.Down:
					ignoreDirection = Direction.Up;
					break;
				case Direction.Up:
					ignoreDirection = Direction.Down;
					break;
			}
		}

		public void setParent(Node parent)
		{
			this.parent = parent;
		}

		public int fCost
		{
			get
			{
				return gCost + hCost;
			}
		}

		public bool isWalkable;

		public Vector2 pos;
		readonly Tile tile;

		public Node(Vector2 pos, Tile[,] tileArray)
		{
			if (pos.X < 0 || pos.Y < 0 || pos.X >= GameController.NumberOfTilesX || pos.Y >= GameController.NumberOfTilesY)
			{
				this.pos = new Vector2(-100, -100);
			}
			else
			{
				this.pos = pos;
				tile = tileArray[(int)pos.X, (int)pos.Y];
				if (tile.tileType == Tile.TileType.Wall)
				{
					isWalkable = false;
				}
				else
				{
					isWalkable = true;
				}
			}

		}

		public Node Copy(Tile[,] tileArray)
		{
			Node node = new Node(pos, tileArray);

			node.hCost = hCost;
			node.gCost = gCost;
			node.ignoreDirection = ignoreDirection;
			if (parent != null)
			{
				node.setParent(parent.Copy(tileArray));
			}

			return node;
		}

		public List<Node> getNeighbours(Tile[,] tileArray)
		{
			List<Node> neighbours = new List<Node>();

			if (ignoreDirection != Direction.Left)
			{
				Node left = new Node(new Vector2(pos.X - 1, pos.Y), tileArray);
				if (left.pos != new Vector2(-100, -100))
					neighbours.Add(left);
			}

			if (ignoreDirection != Direction.Right)
			{
				Node right = new Node(new Vector2(pos.X + 1, pos.Y), tileArray);
				if (right.pos != new Vector2(-100, -100))
					neighbours.Add(right);
			}

			if (ignoreDirection != Direction.Up)
			{
				Node up = new Node(new Vector2(pos.X, pos.Y - 1), tileArray);
				if (up.pos != new Vector2(-100, -100))
					neighbours.Add(up);
			}

			if (ignoreDirection != Direction.Down)
			{
				Node down = new Node(new Vector2(pos.X, pos.Y + 1), tileArray);
				if (down.pos != new Vector2(-100, -100))
					neighbours.Add(down);
			}

			return neighbours;
		}
	}
}
