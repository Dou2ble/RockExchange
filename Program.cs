using System.Runtime.InteropServices;
using Raylib_cs;

namespace RockExchange;

class Program
{
    static void Main(string[] args)
    {
            Raylib.InitWindow(1280, 720, "Rock Exchange");

            // Load window icon
            Image icon = Raylib.LoadImage("assets/textures/icon.png");
            unsafe
            {
                Raylib.ImageFormat(&icon, PixelFormat.UncompressedR8G8B8);
            }
            Raylib.SetWindowIcon(icon);
            Raylib.UnloadImage(icon);
            
            // Don't close the game using escape
            Raylib.SetExitKey(KeyboardKey.Null);
            
            Raylib.SetTargetFPS(60);

            Game game = new Game();
    
            Raylib.CloseWindow();
    }
}