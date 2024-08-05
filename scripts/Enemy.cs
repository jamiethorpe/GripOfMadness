using Diablo2d.scripts.Enemies;
using Godot;
using Diablo2d.Utils;

namespace Diablo2d.scripts;

public partial class Enemy : CharacterBody2D, IKillable
{
    // When inactive, the enemy will not move or attack
    private bool _isActive;
    private string _lastDirection = Directions.South; // Default direction
    private bool _isDead;
    
    private AnimatedSprite2D _animatedSprite;
    private string _currentAnimation = Animations.IdleSouth;
    private HealthBar _healthBar;

    [Export] public string DisplayName = "Default Enemy";
    [Export] public HealthComponent HealthComponent;
    public HitboxComponent HitboxComponent;
    
    [Signal]
    public delegate void EnemyDiedEventHandler(Enemy enemy);

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite.Play(_currentAnimation);
        
        HealthComponent = GetNode<HealthComponent>("HealthComponent");
        HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
        
    }

    public void Activate()
    {
        _isActive = true; 
    }

    public void Die()
    {
        // play death animation
        _isDead = true;
        _animatedSprite.Play("die_" + _lastDirection);
        HitboxComponent.QueueFree();
        HealthComponent.QueueFree();
        GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
        GetNode<EnemyDetails>("/root/Game/UI/EnemyDetails").Hide();


        // TODO: replace it with a "Corpse" scene using the same position and the final "death" frame from the animation
    }

    public bool IsDead()
    {
        return _isDead;
    }
    
}