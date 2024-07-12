using Godot;

namespace Diablo2d.scripts;

public partial class Enemy : CharacterBody2D
{
    // When inactive, the enemy will not move or attack
    private bool _isActive;

    [Export] public string DisplayName = "Default Enemy";
    [Export] public HealthComponent HealthComponent;
    public HitboxComponent HitboxComponent;

    public override void _Ready()
    {
        HealthComponent = GetNode<HealthComponent>("HealthComponent");
    }

    public void Activate()
    {
        GD.Print("Activating");
        _isActive = true;
        GD.Print(_isActive);
    }
}