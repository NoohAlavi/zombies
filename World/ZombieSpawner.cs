using Godot;
using System;

public class ZombieSpawner : Node2D
{
    private Timer _spawnTimer;
    private PackedScene _zombieScene;

    public override void _Ready()
    {
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.Connect("timeout", this, "OnSpawnTimerTimeout");

        _zombieScene = GD.Load<PackedScene>("res://Zombie/Zombie.tscn");
    }

    public void OnSpawnTimerTimeout()
    {
        Zombie zombie = _zombieScene.Instance() as Zombie;
        zombie.Position = new Vector2(GD.Randi() % 2200, 300);

        GetNode("ZombieHolder").AddChild(zombie);
    }
}
