using Godot;

namespace Diablo2d.scripts;

public partial class HealthComponent : Node2D
{
    public int Health;
    [Export] public int MaxHealth;

    public override void _Ready()
    {
        Health = MaxHealth;
    }

    public void Damage(AttackComponent attack)
    {
        Health -= attack.AttackDamage;
        if (Health <= 0) GetParent().QueueFree();
    }
}