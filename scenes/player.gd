extends CharacterBody2D

var speed = 300.0
var click_position = Vector2()
var last_direction = "south"  # Default direction
var current_animation = "idle_south"
var is_attacking = false

@onready var animated_sprite = $AnimatedSprite2D

func _ready():
	click_position = position
	animated_sprite.connect("animation_finished", Callable(self, "_on_AnimatedSprite_animation_finished"))

func _physics_process(_delta):
	if Input.is_action_just_pressed("right_click"):
		attack()
	elif not is_attacking:
		handle_movement()

func attack():
	var attack_click_position = get_global_mouse_position()
	var attack_direction_vector = attack_click_position - position
	var attack_angle = atan2(attack_direction_vector.y, attack_direction_vector.x)
	var attack_direction = get_direction(attack_angle)
	last_direction = attack_direction
	var animation_to_play = "one_hand_overhead_attack_" + attack_direction
	current_animation = animation_to_play
	is_attacking = true
	animated_sprite.play(animation_to_play)
	velocity = Vector2.ZERO  # Stop movement during attack
	click_position = position  # Clear target position to stop running after attack

func handle_movement():
	if Input.is_action_pressed("left_click"):
		click_position = get_global_mouse_position()

	var direction_vector = click_position - position
	var angle = atan2(direction_vector.y, direction_vector.x)

	if position.distance_squared_to(click_position) > 10:
		var target_position = direction_vector.normalized()
		velocity = target_position * speed
		var direction = get_direction(angle)
		last_direction = direction
		var animation_to_play = "run_" + direction
		if current_animation != animation_to_play:
			current_animation = animation_to_play
			animated_sprite.play(animation_to_play)
		move_and_slide()
	else:
		current_animation = "idle_" + last_direction
		animated_sprite.play(current_animation)
		velocity = Vector2.ZERO  # Ensure velocity is zero when idling

func _on_AnimatedSprite_animation_finished():
	if is_attacking:
		is_attacking = false
		animated_sprite.play("idle_" + last_direction)

func get_direction(angle):
	if angle >= -PI/8 and angle < PI/8:
		return "east"
	elif angle >= PI/8 and angle < 3*PI/8:
		return "south_east"
	elif angle >= 3*PI/8 and angle < 5*PI/8:
		return "south"
	elif angle >= 5*PI/8 and angle < 7*PI/8:
		return "south_west"
	elif angle >= -3*PI/8 and angle < -PI/8:
		return "north_east"
	elif angle >= -5*PI/8 and angle < -3*PI/8:
		return "north"
	elif angle >= -7*PI/8 and angle < -5*PI/8:
		return "north_west"
	else:
		return "west"
