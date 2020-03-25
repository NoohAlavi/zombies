using Godot;
using System;

public class AmmoBox : Area2D
{
    public override void _Ready()
    {
        Connect("body_entered", this, "OnBodyEntered");
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if (body is Player)
        {
            Player player = body as Player;
            player.Clips++;
            QueueFree();
        }
    }
}
