using Godot;

namespace Diablo2d.scripts;

public partial class AttackComponent : Node
{
	public int AttackDamage;
	public int KnockbackForce;
	[Export] public float Range;

	public void Attack(IAttackable target)
	{
		target.Damage(this);
	}
}
