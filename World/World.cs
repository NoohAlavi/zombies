using Godot;
using System;

public class World : Node2D
{
    private Timer _spawnTimer;
    private Timer _scoreTimer;
    private Label _scoreText;
    private PackedScene _zombieScene;
    private Player _player;

    public float Score;

    public override void _Ready()
    {
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.Connect("timeout", this, "OnSpawnTimerTimeout");

        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _scoreTimer.Connect("timeout", this, "OnScoreTimerTimeout");

        _scoreText = GetNode<Label>("HUD/ScoreLabel");

        _player = GetNode<Player>("Player");

        _zombieScene = GD.Load<PackedScene>("res://Zombie/Zombie.tscn");
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
            zombie.Position = new Vector2(GD.Randi() % 2200, 400);

            GetNode("ZombieHolder").AddChild(zombie);
        }
    }

    public void OnScoreTimerTimeout()
    {
        Score++;
    }
}
