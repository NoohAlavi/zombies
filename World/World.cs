using Godot;
using System;

public class World : Node2D
{
    private Timer _spawnTimer;
    private Timer _scoreTimer;
    private Label _scoreText;
    private PackedScene _zombieScene;
    private Player _player;
    private Timer _ammoBoxSpawnTimer;
    private PackedScene _ammoBoxScene;

    public float Score;

    public override void _Ready()
    {
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.Connect("timeout", this, "OnSpawnTimerTimeout");

        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _scoreTimer.Connect("timeout", this, "OnScoreTimerTimeout");

        _ammoBoxSpawnTimer = GetNode<Timer>("AmmoBoxSpawnTimer");
        _ammoBoxSpawnTimer.Connect("timeout", this, "SpawnAmmoBox");

        _scoreText = GetNode<Label>("HUD/ScoreLabel");

        _player = GetNode<Player>("Player");

        _zombieScene = GD.Load<PackedScene>("res://Zombie/Zombie.tscn");
        _ammoBoxScene = GD.Load<PackedScene>("res://AmmoBox/AmmoBox.tscn");
    }

    public override void _Process(float delta)
    {
        _scoreText.Text = "Score: " + Score.ToString();
    }

    public void OnSpawnTimerTimeout()
    {
        if (!_player.IsGameOver)
        {
            Zombie zombie = _zombieScene.Instance() as Zombie;

            GetNode("ZombieHolder").AddChild(zombie);
            zombie.Position = new Vector2(GD.Randi() % 2200, 400);
        }
    }

    public void OnScoreTimerTimeout()
    {
        Score++;
    }

    public void SpawnAmmoBox()
    {
        AmmoBox ammoBox = _ammoBoxScene.Instance() as AmmoBox;
        GetNode("AmmoBoxHolder").AddChild(ammoBox);
        ammoBox.Position = new Vector2(GD.Randi() % 2200, 400);
    }
}
