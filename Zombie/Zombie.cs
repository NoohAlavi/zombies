using Godot;
using System;

public class Zombie : KinematicBody2D
{
    [Export] public Vector2 Velocity = new Vector2();
    [Export] public float FollowSpeed = 200f;
    [Export] public float Gravity = 500f;
    [Export] public float Health = 5f;
    [Export] public string State = "Passive";

    private Player _player;
    private RayCast2D _rayCast;
    private Node2D _bloodParticlesHolder;
    private AnimatedSprite _animatedSprite;
    private CollisionShape2D _collisionShape;
    private Timer _showBloodTimer;
    private Timer _deathTimer;

    [Export] private float _rayCastLength = 50f;
    [Export] private float _scale = 3f;

    private bool _isDead = false;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/World/Player");
        _rayCast = GetNode<RayCast2D>("RayCast2D");
        _bloodParticlesHolder = GetNode<Node2D>("BloodParticlesHolder");
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _showBloodTimer = GetNode<Timer>("ShowBloodTimer");
        _showBloodTimer.Connect("timeout", this, "HideBlood");
        _deathTimer = GetNode<Timer>("DeathTimer");
        _deathTimer.Connect("timeout", this, "Destroy");
    }

    public override void _PhysicsProcess(float delta)
    {

        if (Health <= 0f)
        {
            Die();
        }

        if (Position.DistanceTo(_player.Position) <= 400)
        {
            State = "Aggressive";
        }

        if (State == "Aggressive" && !_isDead)
        {
            Velocity = Position.DirectionTo(_player.Position) * FollowSpeed;
            _animatedSprite.Play("Run");
        }
        else
        {
            Velocity.x = 0f;
        }

        Velocity.y += Gravity;
        Velocity = MoveAndSlide(Velocity, Vector2.Up);

        if (Velocity.x > 0f)
        {
            _bloodParticlesHolder.Scale = new Vector2(1f, 1f);
            _animatedSprite.FlipH = false;
        }
        if (Velocity.x < 0f)
        {
            _bloodParticlesHolder.Scale = new Vector2(-1f, 1f);
            _animatedSprite.FlipH = true;
        }

        _rayCast.CastTo = Position.DirectionTo(_player.Position) * _rayCastLength;

        if (_rayCast.IsColliding())
        {
            Node collider = _rayCast.GetCollider() as Node;
            if (collider is Player)
            {
                Player player = collider as Player;
                player.Health--;
                player.ShowBlood();
            }
        }
    }

    public void ShowBlood()
    {
        _bloodParticlesHolder.Show();
        _showBloodTimer.Start(.5f);
    }

    public void HideBlood()
    {
        _bloodParticlesHolder.Hide();
    }

    private void Die()
    {
        if (!_isDead)
        {
            _animatedSprite.Play("Death");
            _rayCast.Enabled = false;
            _isDead = true;
            _collisionShape.Disabled = true;
            Position = new Vector2(Position.x, 400f);
            Gravity = 0f;
            _player.Kills++;
        }
        // _deathTimer.Start();
    }

    public void Destroy()
    {
        QueueFree();
    }
}
