using Godot;
using System;

namespace Diablo2d.scripts;

public partial class EnemyDetails : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private HealthBar _healthBar;
	[Export] private Label _name;
	
	public override void _Ready()
	{
		Hide();
	}

	public void Show(Enemy enemy)
	{
		_name.Text = enemy.Name;
		_healthBar.SetValue(enemy);
		Show();
	}
	
	public void UpdateHealth(Enemy enemy)
	{
		_healthBar.SetValue(enemy);
	}
}
