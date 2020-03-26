using Godot;
using System;

public class World : Node2D
{

    public float Score;

    private Timer _spawnTimer;
    private Timer _scoreTimer;
    private Label _killsLabel;
    private Label _scoreText;
    private PackedScene _zombieScene;
    private Player _player;
    private Timer _ammoBoxSpawnTimer;
    private Timer _medKitSpawnTimer;
    private PackedScene _ammoBoxScene;
    private PackedScene _medKitScene;

    public override void _Ready()
    {
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.Connect("timeout", this, "OnSpawnTimerTimeout");

        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _scoreTimer.Connect("timeout", this, "OnScoreTimerTimeout");

        _ammoBoxSpawnTimer = GetNode<Timer>("AmmoBoxSpawnTimer");
        _ammoBoxSpawnTimer.Connect("timeout", this, "SpawnAmmoBox");

        _medKitSpawnTimer = GetNode<Timer>("MedKitSpawnTimer");
        _medKitSpawnTimer.Connect("timeout", this, "SpawnMedKit");

        _scoreText = GetNode<Label>("HUD/ScoreLabel");
        _killsLabel = GetNode<Label>("HUD/KillsLabel");

        _player = GetNode<Player>("Player");

        _zombieScene = ResourceLoader.Load<PackedScene>("res://Zombie/Zombie.tscn");
        _ammoBoxScene = ResourceLoader.Load<PackedScene>("res://AmmoBox/AmmoBox.tscn");
        _medKitScene = ResourceLoader.Load<PackedScene>("res://MedKit/MedKit.tscn");

        GD.Randomize();
    }

    public override void _Process(float delta)
    {
        _scoreText.Text = "Time: " + Score.ToString();
        _killsLabel.Text = "Kills: " + _player.Kills.ToString();
        if (_player.IsGameOver)
        {
            _scoreTimer.Stop();
        }
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

    public void OnScoreTimerTimeout() => Score++;

    public void SpawnAmmoBox()
    {
        AmmoBox ammoBox = _ammoBoxScene.Instance() as AmmoBox;
        GetNode("AmmoBoxHolder").AddChild(ammoBox);
        ammoBox.Position = new Vector2(GD.Randi() % 2200, 400);
    }

    public void SpawnMedKit()
    {
        MedKit medKit = _medKitScene.Instance() as MedKit;
        GetNode("MedKitHolder").AddChild(medKit);
        medKit.Position = new Vector2(GD.Randi() % 2200, 400);
    }
}
