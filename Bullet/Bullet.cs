using Godot;
using System;

public class Bullet : Area2D
{
    [Export] public Vector2 Direction;
    public float Speed = 1000f;

    private Timer _despawnTimer;

    public override void _Ready()
    {
        _despawnTimer = GetNode<Timer>("DespawnTimer");
        _despawnTimer.Connect("timeout", this, "_OnDespawnTimerTimeout");

        Connect("body_entered", this, "OnBulletBodyEntered");
    }

    public override void _PhysicsProcess(float delta)
    {
        Position += Direction * Speed * delta;
    }

    public void _OnDespawnTimerTimeout()
    {
        QueueFree();
    }

    public void OnBulletBodyEntered(PhysicsBody2D body)
    {
        if (body is Zombie)
        {
            Zombie zombie = body as Zombie;
            zombie.Health--;
            zombie.State = "Aggressive";
            zombie.ShowBlood();
            QueueFree();
        }
    }
}
