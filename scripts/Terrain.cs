using Godot;
using System;

namespace Diablo2d.scripts;
public partial class Terrain : TileMap
{
	[Signal]
	public delegate void TerrainClickedEventHandler(Vector2 position);

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
		{
			EmitSignal(nameof(TerrainClicked), GetGlobalMousePosition());
		}
	}
}
