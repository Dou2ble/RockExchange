using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace RockExchange;

public class Game
{
        private Board _board;
        
        public Game()
        {
                _board = new Board();
                while (!Raylib.WindowShouldClose())
                {
                        Raylib.BeginDrawing();

                        Update();
                        Draw();
        
                        Raylib.EndDrawing();
                }
        }

        private void Update()
        {
                _board.Update();

                if (Raylib.IsKeyPressed(KeyboardKey.One))
                {
                        _board = new Board();
                }
        }


        private void Draw()
        {
                Raylib.ClearBackground(Color.White);
                
                Raylib.DrawTexture(Assets.Instance.Backgrounds[Time.Day], 0, 0, Color.White);
                _board.Draw();
                Raylib.DrawText(_board.Score.ToString(), 12, 12, 20, Color.Black); 
        }
}