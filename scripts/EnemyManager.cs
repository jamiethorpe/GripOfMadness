using System.Collections.Generic;
using Godot;

namespace Diablo2d.scripts;

public partial class EnemyManager : Node
{
    [Signal]
    public delegate void EnemySpawnedEventHandler(HitboxComponent enemy);

    private readonly List<Enemy> _enemies = new();

    public override void _Ready()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        foreach (var child in GetChildren())
        {
            if (child is not Enemy enemy)
            {
                continue;
            }
            
            _enemies.Add(enemy);
            EmitSignal(SignalName.EnemySpawned, enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (!_enemies.Contains(enemy)) return;

        _enemies.Remove(enemy);
        enemy.QueueFree();
    }
}