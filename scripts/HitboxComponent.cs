using Godot;

namespace Diablo2d.scripts;

public partial class HitboxComponent : Area2D
{
    [Signal]
    public delegate void EnemyClickedEventHandler(HitboxComponent hitbox);

    [Export] public HealthComponent HealthComponent;

    [Export] public bool IsAttackable = true;
    [Export] public string test = "yes";

    public void Damage(AttackComponent attack)
    {
        if (!IsAttackable) return;

        HealthComponent.Damage(attack);
    }

    public void _input_event(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && IsAttackable)
        {
            GD.Print("You clicked me!");
            GD.Print("Emitting signal from instance with Test: " + test);
            EmitSignal(nameof(EnemyClicked), this);
            GD.Print("Signal emitted.");
        }
    }
}