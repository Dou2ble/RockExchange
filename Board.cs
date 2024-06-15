using Raylib_cs;
using System.Numerics;

namespace RockExchange;

public class Board
{
        private const int BoardSize = 7;
        private const int TileSize = 64;
        private readonly Vector2 _boardOffset = new Vector2(256, 64);
        private bool _checked;
        public bool Alive = true;
        public uint Exchanges = 0;
        public uint Score { get; private set; }
        private Rock?[,] _rocks = new Rock?[BoardSize, BoardSize];
        private Tuple<int, int> _hoveredRock = new Tuple<int, int>(0, 0);
        private Tuple<int, int>? _selectedRock = null;

        public Board()
        {
                while (true) {
                        // Initialize the game board
                        for (int x = 0; x < BoardSize; x++)
                        {
                                for (int y = 0; y < BoardSize; y++)
                                {
                                        _rocks[x, y] = new Rock();
                                }
                        }

                        if (FindConnection() == null && !NoMoreMoves())
                        {
                                      break;
                        }
                }
        }

        private bool NoMoreMoves()
        {
                if (FindConnection() != null)
                {
                        return false;
                }
                
                // directions
                Array allDirections = Enum.GetValues(typeof(Direction));
                
                for (int x = 0; x < BoardSize; x++)
                {
                        for (int y = 0; y < BoardSize; y++)
                        {
                                Tuple<int, int> current = new Tuple<int, int>(x, y);

                                foreach (Direction direction in allDirections)
                                {
                                        Tuple<int, int> exchangee = direction switch
                                        {
                                                Direction.Down => new Tuple<int, int>(x, y - 1),
                                                Direction.Left => new Tuple<int, int>(x - 1, y),
                                                Direction.Right => new Tuple<int, int>(x + 1, y),
                                                Direction.Up => new Tuple<int, int>(x, y + 1),
                                                _ => throw new ArgumentOutOfRangeException()
                                        };

                                        if (Exchange(current, exchangee, false, false))
                                        {
                                                Exchange(exchangee, current, true, false);
                                                return false;
                                        }
                                }
                        }
                }

                return true;
        }

        private List<Tuple<int, int>> BoardDfs(Tuple<int, int> pos)
        {
                List<Tuple<int, int>> visited = new List<Tuple<int, int>>();
                List<Tuple<int, int>> stack = new List<Tuple<int, int>>();
                
                visited.Add(pos);
                stack.Add(pos);

                // TODO: make this code cleaner
                while (stack.Count > 0)
                {
                        Tuple<int, int> current = stack[^1];
                        stack.RemoveAt(stack.Count - 1);
                        
                        if (_rocks[current.Item1, current.Item2] == null)
                        {
                                continue;
                        }
                        
                        // Check UP
                        if (current.Item2 > 0)
                        {
                                if (!visited.Contains(new Tuple<int, int>(current.Item1, current.Item2 - 1)) && _rocks[current.Item1, current.Item2 - 1] != null &&
                                    _rocks[current.Item1, current.Item2].Kind == _rocks[current.Item1, current.Item2 - 1].Kind)
                                {
                                        visited.Add(new Tuple<int, int>(current.Item1, current.Item2 - 1));
                                        stack.Add(new Tuple<int, int>(current.Item1, current.Item2 - 1));
                                }
                        }
                        // Check LEFT
                        if (current.Item1 > 0)
                        {
                                if (!visited.Contains(new Tuple<int, int>(current.Item1 - 1, current.Item2)) && _rocks[current.Item1 - 1, current.Item2] != null &&
                                    _rocks[current.Item1, current.Item2].Kind == _rocks[current.Item1 - 1, current.Item2].Kind)
                                {
                                        visited.Add(new Tuple<int, int>(current.Item1 - 1, current.Item2));
                                        stack.Add(new Tuple<int, int>(current.Item1 - 1, current.Item2));
                                }
                        }
                        // Check DOWN
                        if (current.Item2 < BoardSize - 1)
                        {
                                if (!visited.Contains(new Tuple<int, int>(current.Item1, current.Item2 + 1)) && _rocks[current.Item1, current.Item2 + 1] != null &&
                                    _rocks[current.Item1, current.Item2].Kind == _rocks[current.Item1, current.Item2 + 1].Kind)
                                {
                                        visited.Add(new Tuple<int, int>(current.Item1, current.Item2 + 1));
                                        stack.Add(new Tuple<int, int>(current.Item1, current.Item2 + 1));
                                }
                        }
                        // Check RIGHT
                        if (current.Item1 < BoardSize - 1)
                        {
                                if (!visited.Contains(new Tuple<int, int>(current.Item1 + 1, current.Item2)) && _rocks[current.Item1 + 1, current.Item2] != null &&
                                    _rocks[current.Item1, current.Item2].Kind == _rocks[current.Item1 + 1, current.Item2].Kind)
                                {
                                        visited.Add(new Tuple<int, int>(current.Item1 + 1, current.Item2));
                                        stack.Add(new Tuple<int, int>(current.Item1 + 1, current.Item2));
                                }
                        }
                }

                return visited;
        }

        private List<Tuple<int, int>>? FindConnection()
        {
                for (int x = 0; x < BoardSize; x++)
                {
                        for (int y = 0; y < BoardSize; y++)
                        {
                                Tuple<int, int> tuple = new Tuple<int, int>(x, y);
                                List<Tuple<int, int>> visited = BoardDfs(tuple);

                                // if the visited list is less than 3 then it is impossible for it to have a three in a row
                                if (visited.Count < 3)
                                {
                                        continue;
                                }
                                
                                // check for a three in a row
                                foreach (Tuple<int, int> pos in visited)
                                {
                                        if (visited.Contains(new Tuple<int, int>(pos.Item1 - 1, pos.Item2)) && visited.Contains(new Tuple<int, int>(pos.Item1 + 1, pos.Item2)) ||
                                            visited.Contains(new Tuple<int, int>(pos.Item1, pos.Item2 - 1)) && visited.Contains(new Tuple<int, int>(pos.Item1, pos.Item2 + 1)))
                                        {
                                                return visited;
                                        }
                                }
                        }
                }
                         
                
                return null;
        }

        // Swaps two different rocks. If the swap was successful, returns true, otherwise false
        private bool Exchange(Tuple<int, int> first, Tuple<int, int> second, bool force = false, bool count = true)
        {
                // check that the tiles are in bounds
                if (first.Item1 < 0 || first.Item1 >= BoardSize || first.Item2 < 0 || first.Item2 >= BoardSize ||
                    second.Item1 < 0 || second.Item1 >= BoardSize || second.Item2 < 0 || second.Item2 >= BoardSize)
                {
                        return false;
                }
                
                // If the tiles aren't adjacent or the same, return
                if (!force && Math.Abs(first.Item1 - second.Item1) + Math.Abs(first.Item2 - second.Item2) != 1)
                {
                        return false;
                } 
                
                // Swap the tiles using deconstruction
                (_rocks[first.Item1, first.Item2], _rocks[second.Item1, second.Item2]) = (_rocks[second.Item1, second.Item2], _rocks[first.Item1, first.Item2]);
                
                // if the swap doesn't create a connection, swap back
                if (!force && FindConnection() == null)
                {
                        // swap back the tiles using deconstruction
                        (_rocks[first.Item1, first.Item2], _rocks[second.Item1, second.Item2]) = (_rocks[second.Item1, second.Item2], _rocks[first.Item1, first.Item2]);
                        return false;
                }

                if (count)
                {
                        Exchanges++;    
                }
                
                return true;
        }

        private void PickTile(Tuple<int, int> tile)
        {
                if (_selectedRock == null)
                {
                        _selectedRock = tile;
                }
                else
                {
                        if (Exchange(_selectedRock, tile))
                        {
                                _checked = false;
                        }
                        _selectedRock = null;
                }
        }

        private void Gravity()
        {
                // move the rocks down
                while (true)
                {
                        bool nullRocks = false; 
                        
                        for (int x = 0; x < BoardSize; x++)
                        {
                                for (int y = 0; y < BoardSize; y++)
                                {
                                        if (_rocks[x, y] != null)
                                        {
                                                continue;
                                        }

                                        nullRocks = true;
                                        // if the rock is at the top of the board we should simply spawn a new one
                                        if (y == 0)
                                        {
                                                _rocks[x, y] = new Rock();
                                                continue;
                                        }
                                        
                                        for (int i = y - 1; i >= 0; i--)
                                        {
                                                if (_rocks[x, i] != null)
                                                {
                                                        _rocks[x, y] = _rocks[x, i];
                                                        _rocks[x, i] = null;
                                                }
                                        }
                                }
                        } 
                        
                        if (!nullRocks)
                        {
                                break;
                        }
                }
        }

        private void RemoveConnection()
        {
                List<Tuple<int, int>>? connection = FindConnection();
                if (connection != null)
                {
                        // // Handle special rocks
                        // switch (_rocks[connection[0].Item1, connection[0].Item2].Kind)
                        // {
                        //         case RockKind.Coin:
                        //                 Score += 50;
                        //                 break;
                        // }
                        
                        // Remove every rock in the connection
                        foreach (Tuple<int, int> tuple in connection)
                        {
                                _rocks[tuple.Item1, tuple.Item2] = null;
                        }
                        
                        Score += (uint)connection.Count * 10;
                }
        }

        private void Check()
        {
                while (true)
                {
                        Rock?[,] before = _rocks;

                        RemoveConnection();
                        Gravity();

                        if (NoMoreMoves())
                        {
                                Alive = false;
                        }

                        if (before == _rocks)
                        {
                                break;
                        }
                }
        }

        public void Update()
        {
                Vector2 mousePosition = Raylib.GetMousePosition();
                
                Vector2 mouseBoardPosition = (mousePosition - _boardOffset) / TileSize;

                if (!_checked)
                {
                        Check();
                }
                
                // if the boardPosition is in the board bounds we should use mouse controls
                if (mouseBoardPosition.X >= 0 && mouseBoardPosition.X < BoardSize &&
                    mouseBoardPosition.Y >= 0 && mouseBoardPosition.Y < BoardSize)
                {
                        _hoveredRock = new Tuple<int, int>((int)mouseBoardPosition.X, (int)mouseBoardPosition.Y);

                        if (Raylib.IsMouseButtonPressed(MouseButton.Left) ||
                            Raylib.IsMouseButtonPressed(MouseButton.Middle) ||
                            Raylib.IsMouseButtonPressed(MouseButton.Middle))
                        {
                                PickTile(_hoveredRock);
                        }
                } 
                else
                {
                        // use keyboard controls 
                        _hoveredRock = Controller.GetDirection() switch
                        {
                                Direction.Up => new Tuple<int, int>(_hoveredRock.Item1,
                                        Math.Max(0, _hoveredRock.Item2 - 1)),
                                Direction.Down => new Tuple<int, int>(_hoveredRock.Item1,
                                        Math.Min(BoardSize - 1, _hoveredRock.Item2 + 1)),
                                Direction.Left => new Tuple<int, int>(Math.Max(0, _hoveredRock.Item1 - 1),
                                        _hoveredRock.Item2),
                                Direction.Right => new Tuple<int, int>(Math.Min(BoardSize - 1, _hoveredRock.Item1 + 1),
                                        _hoveredRock.Item2),
                                _ => _hoveredRock
                        };
                }
                
                if (Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                        PickTile(_hoveredRock);
                }
        }

        private void DrawBackground()
        {
                Raylib.DrawRectangleRounded(
                        new Rectangle(_boardOffset.X, _boardOffset.Y, BoardSize * TileSize, BoardSize * TileSize), 0.05f, 4,
                        GUI.BackgroundColor);

                for (int x = 0; x < BoardSize; x++)
                {
                        for (int y = 0; y < BoardSize; y++)
                        {
                                if ((x + y * BoardSize) % 2 == 0)
                                {
                                        Raylib.DrawRectangleRounded(new Rectangle(_boardOffset.X + x*TileSize, _boardOffset.Y + y*TileSize, TileSize, TileSize), 0.3f, 4, GUI.BackgroundColorAlt);
                                }
                        }
                }
        }

        private void DrawRocks()
        {
                for (int x = 0; x < BoardSize; x++)
                {
                        for (int y = 0; y < BoardSize; y++)
                        {
                                if (_rocks[x, y] == null)
                                {
                                        continue;
                                } 
                                
                                Vector2 position = _boardOffset;
                                position.X += x * TileSize;
                                position.Y += y * TileSize;

                                Vector2 circlePosition = position;
                                circlePosition.X += (float)TileSize / 2;
                                circlePosition.Y += (float)TileSize / 2;

                                Color? hoverColor = null;
                                if (_hoveredRock.Item1 == x && _hoveredRock.Item2 == y)
                                {
                                        hoverColor = Color.Orange;
                                }

                                if (_selectedRock != null)
                                {
                                        if (_selectedRock.Item1 == x && _selectedRock.Item2 == y)
                                        {
                                                hoverColor = Color.Red;
                                        }
                                }

                                if (hoverColor != null)
                                {
                                        Raylib.DrawTextureEx(Assets.Instance.HoverRocks[_rocks[x, y].Kind], position, 0,
                                                1f / Assets.RockSize * TileSize, hoverColor.Value);
                                }
                                
                                Raylib.DrawTextureEx(Assets.Instance.Rocks[_rocks[x, y].Kind], position, 0, 1f/Assets.RockSize*TileSize, Color.White);
                        }
                }
        }

        public void Draw()
        {
                DrawBackground(); 
                DrawRocks();
        }
}