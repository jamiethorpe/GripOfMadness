using Diablo2d.Utils;
using Godot;

namespace Diablo2d.scripts;

public partial class AgroArea : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect(Area2D.SignalName.AreaEntered, new Callable(this, nameof(OnAgroAreaEntered)));
    }

    public void OnAgroAreaEntered(Area2D area)
    {
        if (!area.IsInGroup(Groups.Enemies))
            return;

        var enemy = area.GetParent<Enemy>();
        enemy.Activate();
    }
}