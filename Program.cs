using System.Runtime.InteropServices;
using Raylib_cs;

namespace RockExchange;

class Program
{
    static void Main(string[] args)
    {
            Raylib.InitWindow(1280, 720, "Rock Exchange");

            Image icon = Raylib.LoadImage("assets/textures/lime-rock.png");
            unsafe
            {
                Raylib.ImageFormat(&icon, PixelFormat.UncompressedR8G8B8);
            }
            Raylib.SetWindowIcon(icon);
            Raylib.UnloadImage(icon);
            
            Raylib.SetTargetFPS(60);

            Game game = new Game();
    
            Raylib.CloseWindow();
    }
}