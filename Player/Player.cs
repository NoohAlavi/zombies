using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public Vector2 Velocity = new Vector2();
    [Export] public float MovementSpeed = 200f;
    [Export] public float Gravity = 25f;
    [Export] public float JumpForce = 800f;
    [Export] public float Health = 500f;
    [Export] public float Ammo = 50f;
    [Export] public float Clips = 2f;
    [Export] public float Energy = 100f;

    public bool IsGameOver = false;
    public bool IsInputModeController = false;

    private AnimatedSprite _animatedSprite;
    private PackedScene _bulletScene;
    private TextureProgress _healthBar;
    private TextureProgress _energyBar;
    private CPUParticles2D _bloodParticles;
    private GameOver _gameOver;
    private AudioStreamPlayer2D _shootSound;
    private Label _ammoLabel;

    private float _scale = 3f;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _healthBar = GetNode<TextureProgress>("/root/World/HUD/HealthBar");
        _energyBar = GetNode<TextureProgress>("/root/World/HUD/EnergyBar");
        _bloodParticles = GetNode<CPUParticles2D>("BloodParticles");
        _gameOver = GetNode<GameOver>("/root/World/HUD/GameOver");
        _shootSound = GetNode<AudioStreamPlayer2D>("ShootSound");
        _ammoLabel = GetNode<Label>("/root/World/HUD/AmmoLabel");

        _bulletScene = GD.Load<PackedScene>("res://Bullet/Bullet.tscn");

        if (Input.GetConnectedJoypads().Count > 0f)
        {
            IsInputModeController = true;
        }
    }

    public override void _Process(float delta)
    {
        _healthBar.Value = Health;
        _energyBar.Value = Energy;

        if (Health <= 0f || Position.y > 1800f)
        {
            MovementSpeed = 0f;
            IsGameOver = true;
            _gameOver.Show();
        }

        _ammoLabel.Text = "Ammo: " + Ammo + "/" + Clips;
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

        if (Input.IsActionPressed("Sprint") && Math.Abs(Velocity.x) > 0f && Energy > 0f)
        {
            MovementSpeed = 400f;
            Energy -= 2f;
        }
        else
        {
            MovementSpeed = 200f;
            Energy += .5f;
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

        if (Input.IsActionJustPressed("Reload"))
        {
            Reload();
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
        Vector2 dir = new Vector2();
        dir.x = Input.GetJoyAxis(0, 2);
        dir.y = Input.GetJoyAxis(0, 3);
        if (Ammo > 0f)
        {
            Bullet bullet = _bulletScene.Instance() as Bullet;
            GetNode("/root/World/BulletHolder").AddChild(bullet);
            bullet.Position = Position;
            // if (!IsInputModeController)
            // {
            bullet.LookAt(GetGlobalMousePosition());
            bullet.Direction = Position.DirectionTo(GetGlobalMousePosition());
            // }
            // else
            // {
            // bullet.LookAt(dir);
            // bullet.Direction = dir;
            // }
            _shootSound.Play();
            Ammo--;
        }
    }

    private void Reload()
    {
        if (Clips > 0f)
        {
            Ammo = 50f;
            Clips--;
        }
    }
}
