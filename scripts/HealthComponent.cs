using Godot;

namespace Diablo2d.scripts;

[GlobalClass]
public partial class HealthComponent : Resource, IAttackable
{
    [Export] public int Health { get; set; }
    [Export] public int MaxHealth { get; set; }
    private IKillable _killable;

    public HealthComponent()
    {
        Health = MaxHealth; 
    }
    
    public void Initialize(IKillable killable)
    {
        _killable = killable;
        Health = MaxHealth;
    }

    public void Damage(AttackComponent attack)
    {
        Health -= attack.AttackDamage;
        if (Health <= 0) _killable.Die();
    }
}