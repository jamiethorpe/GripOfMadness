using Godot;

namespace Diablo2d.scripts;

public partial class HealthComponent : Node2D, IAttackable
{
    public int Health;
    [Export] public int MaxHealth;
    private IKillable _killable;

    public override void _Ready()
    {
        Health = MaxHealth;
        _killable = GetParent<IKillable>();
    }

    public void Damage(AttackComponent attack)
    {
        Health -= attack.AttackDamage;
        if (Health <= 0) _killable.Die();
    }
}