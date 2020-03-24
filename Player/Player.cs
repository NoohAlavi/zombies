using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public Vector2 Velocity = new Vector2();
    [Export] public float MovementSpeed = 200f;
    [Export] public float Gravity = 25f;
    [Export] public float JumpForce = 800f;
    [Export] public float Health = 500f;

    public bool IsGameOver = false;

    private AnimatedSprite _animatedSprite;
    private PackedScene _bulletScene;
    private TextureProgress _healthBar;
    private CPUParticles2D _bloodParticles;
    private GameOver _gameOver;

    private float _scale = 3f;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _healthBar = GetNode<TextureProgress>("/root/World/HUD/HealthBar");
        _bloodParticles = GetNode<CPUParticles2D>("BloodParticles");
        _bulletScene = GD.Load<PackedScene>("res://Bullet/Bullet.tscn");
        _gameOver = GetNode<GameOver>("/root/World/HUD/GameOver");
    }

    public override void _Process(float delta)
    {
        _healthBar.Value = Health;
        if (Health <= 0f)
        {
            MovementSpeed = 0f;
            IsGameOver = true;
            _gameOver.Show();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity.y += Gravity;
        Velocity = MoveAndSlide(Velocity, Vector2.Up);

        if (IsOnFloor())
        {
            if (Math.Abs(Velocity.x) > 0)
            {
                _animatedSprite.Play("Run");
            }
            else
            {
                _animatedSprite.Play("Idle");
            }
        }

        if (Velocity.x > 0)
        {
            _animatedSprite.FlipH = false;
        }
        if (Velocity.x < 0)
        {
            _animatedSprite.FlipH = true;
        }
    }

    public override void _Input(InputEvent @event)
    {
        Velocity.x = (Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft")) * MovementSpeed;

        if (Input.IsActionPressed("Jump") && IsOnFloor())
        {
            Velocity.y = -JumpForce;
            _animatedSprite.Play("Jump");
        }

        if (Input.IsActionJustPressed("Fire"))
        {
            Shoot();
        }
    }

    public async void ShowBlood()
    {
        _bloodParticles.Show();
        await ToSignal(GetTree().CreateTimer(.5f), "timeout");
        _bloodParticles.Hide();
    }

    private void Shoot()
    {
        Bullet bullet = _bulletScene.Instance() as Bullet;
        GetNode("/root/World/BulletHolder").AddChild(bullet);
        bullet.Direction = Position.DirectionTo(GetGlobalMousePosition());
        bullet.Position = Position;
        bullet.LookAt(GetGlobalMousePosition());
    }
}
