using System;
using System.Collections.Generic;
using Godot;

namespace Diablo2d.Utils;

public static class Directions
{
    public static readonly string North = "north";
    public static readonly string East = "east";
    public static readonly string South = "south";
    public static readonly string West = "west";
    public static readonly string NorthEast = "north_east";
    public static readonly string SouthEast = "south_east";
    public static readonly string NorthWest = "north_west";
    public static readonly string SouthWest = "south_west";
    
    public static List<string> AllDirections = new()
    {
        North,
        East,
        South,
        West,
        NorthEast,
        SouthEast,
        NorthWest,
        SouthWest
    };
    
    public static string GetRandomDirection()
    {
        var random = new Random();
        var index = random.Next(AllDirections.Count);
        return AllDirections[index];
    }

    public static string GetCardinalDirection(float angle)
    {
        string direction = angle switch
        {
            < Mathf.Pi / 8 and >= -Mathf.Pi / 8 => East,
            >= Mathf.Pi / 8 and < 3 * Mathf.Pi / 8 => SouthEast,
            < 5 * Mathf.Pi / 8 and >= 3 * Mathf.Pi / 8 => South,
            < 7 * Mathf.Pi / 8 and >= 5 * Mathf.Pi / 8 => SouthWest,
            < -Mathf.Pi / 8 and >= -3 * Mathf.Pi / 8 => NorthEast,
            < -3 * Mathf.Pi / 8 and >= -5 * Mathf.Pi / 8 => North,
            < -5 * Mathf.Pi / 8 and >= -7 * Mathf.Pi / 8 => NorthWest,
            _ => West
        };

        return direction;
    }
}
