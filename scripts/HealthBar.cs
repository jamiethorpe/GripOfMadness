using Godot;
using System;

namespace Diablo2d.scripts;

public partial class HealthBar : ProgressBar
{
	public void SetValue(Enemy enemy)
	{
		if (enemy == null)
		{
			return;
		}
		
		Value = Math.Floor((float)enemy.HealthComponent.Health / enemy.HealthComponent.MaxHealth * 100);
	}
}
