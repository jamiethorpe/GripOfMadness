using Godot;

namespace Diablo2d.scripts;

public partial class Player : CharacterBody2D
{
	private string _currentAnimation = "idle_south";
	private bool _isAttacking;
	private string _lastDirection = "south"; // Default direction

	private Vector2 _movementPosition = Vector2.Zero;
	private float _speed = 300.0f;
	[Export] public AnimatedSprite2D AnimatedSprite;
	public AttackComponent AttackComponent;

	public override void _Ready()
	{
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AttackComponent = GetNode<AttackComponent>("AttackComponent");
		_movementPosition = Position;
		AnimatedSprite.Connect("animation_finished", Callable.From(OnAnimationFinished));

		// Get the EnemyManager and connect to the EnemySpawned signal
		var enemyManager = GetNode<EnemyManager>("/root/Game/EnemyManager");
		enemyManager.Connect(EnemyManager.SignalName.EnemySpawned, new Callable(this, nameof(OnEnemySpawned)));
		// Connect to already existing enemies if any
		// Connect to already existing enemies if any
		foreach (var child in enemyManager.GetChildren())
			if (child is Enemy enemy)
				OnEnemySpawned(enemy);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("right_click"))
			HandleAttack();
		// else if (Input.IsActionPressed("left_click"))
		// {
		// 	// Check for an Area2D at the mouse position
		// 	var query = new PhysicsPointQueryParameters2D
		// 	{
		// 		Position = GetViewport().GetMousePosition(),
		// 		CollideWithAreas = true
		// 	};
		//
		// 	var objectsUnderClick = GetWorld2D().DirectSpaceState.IntersectPoint(query);
		// 	GD.Print(objectsUnderClick);
		// 	foreach (var target in objectsUnderClick)
		// 	{
		// 		if (target is Area2D)
		// 		{
		// 			Console.WriteLine('Found target: ' + target);;
		// 		}
		// 	}
		// }
		else if (!_isAttacking) HandleMovement();
	}

	private void OnEnemySpawned(Enemy enemy)
	{
		GD.Print("Enemy spawn event triggered in Player");
		// Connect the new enemy's signal
		var hitbox = enemy.GetNode<HitboxComponent>("HitboxComponent");
		GD.Print($"Connecting signal for HitboxComponent with Test value: {hitbox.test}");
		hitbox.Connect(HitboxComponent.SignalName.EnemyClicked, new Callable(this, nameof(OnEnemyClicked)));
		if (hitbox.IsConnected(HitboxComponent.SignalName.EnemyClicked, new Callable(this, nameof(OnEnemyClicked))))
			GD.Print($"Signal 'EnemyClicked' connected to {hitbox.test} successfully.");
		else
			GD.Print("Failed to connect signal 'EnemyClicked'.");
		// enemy.TreeExiting += () => OnEnemyExited(enemy);
	}

	private void OnEnemyExited(Enemy enemy)
	{
		GD.Print("Enemy exited event triggered in Player");
		// Explicitly disconnect signals if needed (usually not necessary since QueueFree handles it)
		enemy.GetNode<HitboxComponent>("HitboxComponent")
			.Disconnect(HitboxComponent.SignalName.EnemyClicked, new Callable(this, nameof(OnEnemyClicked)));
	}

	private void OnEnemyClicked(HitboxComponent hitbox)
	{
		GD.Print("yep");
		GD.Print($"Enemy clicked: {hitbox.Name}");
		// Handle the enemy being clicked, e.g., start an attack
	}

	private void HandleAttack()
	{
		var attackClickPosition = GetGlobalMousePosition();
		var attackDirectionVector = attackClickPosition - Position;
		var attackAngle = Mathf.Atan2(attackDirectionVector.Y, attackDirectionVector.X);
		var attackDirection = GetDirection(attackAngle);
		_lastDirection = attackDirection;
		var animationToPlay = "one_hand_overhead_attack_" + attackDirection;
		_currentAnimation = animationToPlay;
		_isAttacking = true;
		AnimatedSprite.Play(animationToPlay);
		Velocity = Vector2.Zero; // Stop movement during attack
		_movementPosition = Position; // Clear target position to stop running after attack
	}

	private void HandleMovement()
	{
		if (Input.IsActionPressed("left_click")) _movementPosition = GetGlobalMousePosition();

		var directionVector = _movementPosition - Position;
		var angle = Mathf.Atan2(directionVector.Y, directionVector.X);

		if (Position.DistanceSquaredTo(_movementPosition) > 10)
		{
			var targetPosition = directionVector.Normalized();
			Velocity = targetPosition * _speed;
			var direction = GetDirection(angle);
			_lastDirection = direction;
			var animationToPlay = "run_" + direction;
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
			Velocity = Vector2.Zero; // Ensure velocity is zero when idling
		}
	}

	private void OnAnimationFinished()
	{
		if (_isAttacking)
		{
			_isAttacking = false;
			AnimatedSprite.Play("idle_" + _lastDirection);
		}
	}

	private string GetDirection(float angle)
	{
		if (angle >= -Mathf.Pi / 8 && angle < Mathf.Pi / 8)
			return "east";
		if (angle >= Mathf.Pi / 8 && angle < 3 * Mathf.Pi / 8)
			return "south_east";
		if (angle >= 3 * Mathf.Pi / 8 && angle < 5 * Mathf.Pi / 8)
			return "south";
		if (angle >= 5 * Mathf.Pi / 8 && angle < 7 * Mathf.Pi / 8)
			return "south_west";
		if (angle >= -3 * Mathf.Pi / 8 && angle < -Mathf.Pi / 8)
			return "north_east";
		if (angle >= -5 * Mathf.Pi / 8 && angle < -3 * Mathf.Pi / 8)
			return "north";
		if (angle >= -7 * Mathf.Pi / 8 && angle < -5 * Mathf.Pi / 8)
			return "north_west";
		return "west";
	}
}
