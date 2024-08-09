using Diablo2d.enums;
using Diablo2d.Utils;
using Godot;

namespace Diablo2d.scripts;

public partial class Player : CharacterBody2D
{
	private string _currentAnimation = Animations.IdleSouth;
	private PlayerAnimationType _currentAnimationType;
	
	private bool _isAttacking;
	private AttackComponent _attackComponent;

	private string _lastDirection = Directions.South; // Default direction
	private Vector2 _movementPosition = Vector2.Zero;
	private float _speed = 300.0f;
	private  NavigationAgent2D _navigationAgent;
	
	[Export] public AnimatedSprite2D AnimatedSprite;
	
	public Enemy TargetEnemy;
	
	[Export] public EnemyDetails EnemyDetails;

	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		
		_attackComponent = GetNode<AttackComponent>("AttackComponent");
		_movementPosition = Position;

		
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AnimatedSprite.Connect("animation_finished", Callable.From(OnAnimationFinished));

		var enemyManager = GetNode<EnemyManager>("/root/Game/EnemyManager");
		enemyManager.Connect(EnemyManager.SignalName.EnemySpawned, new Callable(this, nameof(OnEnemySpawned)));

		// Connect to already existing enemies if any
		foreach (var child in enemyManager.GetChildren())
		{
			if (child is not Enemy enemy)
			{
				continue;
			}
			
			OnEnemySpawned(enemy);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_isAttacking && (Input.IsActionPressed("left_click") || Input.IsActionJustPressed("left_click")))
		{ 
			_movementPosition = GetGlobalMousePosition();
		} else if (TargetEnemy is not null && _isAttacking)
		{
			var distance = Position.DistanceTo(TargetEnemy.Position);

			if (distance > _attackComponent.Range)
			{
				_movementPosition = 
					TargetEnemy.Position
					- (Position - TargetEnemy.Position).Normalized()
					* _attackComponent.Range;
				
				Move();
			}
			else
			{
				HandleAttack();
			}

			return;
		}
		
		if (_movementPosition != Position)
		{
			Move();
		}
	}

	private void OnEnemySpawned(Enemy enemy)
	{
		enemy.Connect(Enemy.SignalName.EnemyDied, new Callable(this, nameof(OnEnemyDied)));
		
		var hitbox = enemy.GetNode<HitboxComponent>("HitboxComponent");
		hitbox.Connect(HitboxComponent.SignalName.EnemyClicked, new Callable(this, nameof(OnEnemyClicked)));
		hitbox.Connect(HitboxComponent.SignalName.EnemyHovered, new Callable(this, nameof(OnEnemyHovered)));
		hitbox.Connect(HitboxComponent.SignalName.EnemyHoverRemoved, new Callable(this, nameof(OnEnemyHoverRemoved)));
	}

	private void OnEnemyDied(Enemy enemy)
	{
		_isAttacking = false;
		TargetEnemy = null;
	}

	// private void OnEnemyExited(Enemy enemy)
	// {
	// 	// Explicitly disconnect signals if needed (usually not necessary since QueueFree handles it)
	// 	enemy.GetNode<HitboxComponent>("HitboxComponent")
	// 		.Disconnect(HitboxComponent.SignalName.EnemyClicked, new Callable(this, nameof(OnEnemyClicked)));
	// }

	private void OnEnemyClicked(HitboxComponent hitbox)
	{
		_isAttacking = true;
		TargetEnemy = hitbox.Enemy;
	}
	
	private void OnEnemyHovered(HitboxComponent hitbox)
	{
		EnemyDetails.Show(hitbox.Enemy);
	}
	
	private void OnEnemyHoverRemoved(HitboxComponent hitbox)
	{
		EnemyDetails.Hide();
	}

	private void HandleAttack()
	{
		if (TargetEnemy.IsDead())
		{
			return;
		}
		
		Vector2 attackPosition = TargetEnemy?.Position ?? GetGlobalMousePosition();

		var attackDirectionVector = attackPosition - Position;
		var attackAngle = Mathf.Atan2(attackDirectionVector.Y, attackDirectionVector.X);
		var attackDirection = Directions.GetCardinalDirection(attackAngle);
		_lastDirection = attackDirection;
		var animationToPlay = "one_hand_overhead_attack_" + attackDirection;
		_currentAnimation = animationToPlay;
		_isAttacking = true;
		AnimatedSprite.Play(animationToPlay);
		Velocity = Vector2.Zero; // Stop movement during attack
		_movementPosition = Position; // Clear target position to stop running after attack
	}

	private void Move()
	{
		if (Position.DistanceSquaredTo(_movementPosition) > 10)
		{
			_navigationAgent.TargetPosition = _movementPosition;
			var directionVector = _navigationAgent.GetNextPathPosition() - Position;
			var angle = Mathf.Atan2(directionVector.Y, directionVector.X);
			Velocity = directionVector.Normalized() * _speed;
			var cardinalDirection = Directions.GetCardinalDirection(angle);
			_lastDirection = cardinalDirection;
			var animationToPlay = "run_" + cardinalDirection;
			if (_currentAnimation != animationToPlay)
			{
				_currentAnimation = animationToPlay;
				AnimatedSprite.Play(animationToPlay);
			}

			MoveAndSlide();
		}
		else
		{
			_currentAnimation = "idle_" + _lastDirection;
			AnimatedSprite.Play(_currentAnimation);
			Velocity = Vector2.Zero;
		}
	}

	private void OnAnimationFinished()
	{
		if (_isAttacking)
		{
			if (!TargetEnemy.IsDead())
			{
				_attackComponent.Attack(TargetEnemy.HealthComponent);
				EnemyDetails.UpdateHealth(TargetEnemy);
			}

			if (!Input.IsActionPressed("left_click"))
			{
				_isAttacking = false;

				_currentAnimation = "idle_" + _lastDirection;
				_currentAnimationType = PlayerAnimationType.Idle;
				AnimatedSprite.Play(_currentAnimation);
			}
			
		}
	}
}
