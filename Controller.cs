using Raylib_cs;

namespace RockExchange;

public static class Controller
{
    public static Direction? GetDirection()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.W) || Raylib.IsKeyPressed(KeyboardKey.Up))
        {
            return Direction.Up; 
        }
        if (Raylib.IsKeyPressed(KeyboardKey.A) || Raylib.IsKeyPressed(KeyboardKey.Left))
        {
            return Direction.Left; 
        }
        if (Raylib.IsKeyPressed(KeyboardKey.S) || Raylib.IsKeyPressed(KeyboardKey.Down))
        {
            return Direction.Down; 
        }
        if (Raylib.IsKeyPressed(KeyboardKey.D) || Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            return Direction.Right; 
        }

        return null;
    }
}