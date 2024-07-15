using Godot;

namespace Diablo2d.scripts;

public partial class AttackComponent : Node
{
	[Export] public int AttackDamage;
	[Export] public float Range;

	public void Attack(IAttackable target)
	{
		target.Damage(this);
	}
}
