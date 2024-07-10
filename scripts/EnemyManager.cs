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
        // var enemyScene = (PackedScene)ResourceLoader.Load("res://scenes/enemy.tscn");
        var enemy = GetNode<Enemy>("Skeleton/Enemy");
        // var hitbox = (HitboxComponent)enemy.GetNode("HitboxComponent");
        // AddChild(enemy);
        // _enemyHitboxes.Add(hitbox);
        _enemies.Add(enemy);

        GD.Print("Enemy spawned!");

        EmitSignal(SignalName.EnemySpawned, enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (!_enemies.Contains(enemy)) return;

        _enemies.Remove(enemy);
        enemy.QueueFree();
    }
}