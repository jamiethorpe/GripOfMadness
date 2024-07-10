using Godot;

namespace Diablo2d.scripts;

public partial class HitboxComponent : Area2D
{
    [Signal]
    public delegate void EnemyClickedEventHandler(HitboxComponent hitbox);

    public Enemy Enemy;

    [Export] public HealthComponent HealthComponent;
    [Export] public bool IsAttackable = true;
    [Export] public string test = "yes";

    public override void _Ready()
    {
        Enemy = GetParent<Enemy>();
    }

    public void Damage(AttackComponent attack)
    {
        if (!IsAttackable) return;

        HealthComponent.Damage(attack);
    }

    public void _input_event(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && IsAttackable)
        {
            GD.Print("Clicked me!");
            EmitSignal(nameof(EnemyClicked), this);
        }
    }
}