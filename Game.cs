using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using Raylib_cs;

namespace RockExchange;

public class Game
{
        private Board _board;
        private Menu _menu = new Menu();
        private GameMode _gameMode = GameMode.Menu;

        private GameMode gameMode
        {
                get { return _gameMode; }
                set
                {
                        _board = value switch
                        {
                                GameMode.Game => new Board(),
                                _ => _board
                        };
                        
                        _gameMode = value;
                }
        }
        
        public Game()
        {
                _board = new Board();
                while (!Raylib.WindowShouldClose())
                {
                        Raylib.BeginDrawing();

                        Raylib.ClearBackground(Color.White);
                        Raylib.DrawTexture(Assets.Instance.Background, 0, 0, Color.White);
                        
                        switch (_gameMode)
                        {
                                case GameMode.Menu: 
                                        GameMode? mode = _menu.Draw();
                                        if (mode != null)
                                        {
                                                gameMode = mode.Value;
                                        }

                                        break;
                                case GameMode.Game: 
                                        Update();
                                        Draw();
                                        break;
                        }
        
                        Raylib.EndDrawing();
                }
        }

        private void Update()
        {
                if (Raylib.IsKeyReleased(KeyboardKey.Escape))
                {
                        _gameMode = GameMode.Menu;
                }
                _board.Update();
        }

        private void DrawUI(Vector2 pos)
        {
                float offset = Raylib.MeasureTextEx(Assets.Instance.FontBold, "Exchanges: \t", Assets.FontSize, 0f).X;
                
                Raylib.DrawTextEx(Assets.Instance.FontBold, "Score:", pos, Assets.FontSize, 0f, Color.White);
                Raylib.DrawTextEx(Assets.Instance.Font, _board.Score.ToString(), pos with { X = pos.X + offset }, Assets.FontSize, 0f, Color.White);
                
                Raylib.DrawTextEx(Assets.Instance.FontBold, $"Exchanges:", new Vector2(pos.X, pos.Y+Assets.FontSize), Assets.FontSize, 0f, Color.White);
                Raylib.DrawTextEx(Assets.Instance.Font, _board.Exchanges.ToString(), new Vector2(pos.X + offset, pos.Y + Assets.FontSize), Assets.FontSize, 0f, Color.White);
                
                Raylib.DrawTextEx(Assets.Instance.FontBold, "Average:", new Vector2(pos.X, pos.Y+Assets.FontSize*2), Assets.FontSize, 0f, Color.White);
                Raylib.DrawTextEx(Assets.Instance.Font, Math.Round((float)_board.Score/_board.Exchanges).ToString(CultureInfo.InvariantCulture), new Vector2(pos.X + offset, pos.Y + Assets.FontSize*2), Assets.FontSize, 0f, Color.White);
        }
        private void Draw()
        {
                _board.Draw();
                DrawUI(new Vector2(55, 85));
        }
}