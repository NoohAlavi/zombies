using Godot;
using System;

public class MedKit : Area2D
{
    public override void _Ready()
    {
        Connect("body_entered", this, "OnBodyEntered");
    }

    public void OnBodyEntered(PhysicsBody2D body)
    {
        if (body is Player)
        {
            Player p = body as Player;
            p.Health += 250f;
            if (p.Health > 500f)
            {
                p.Health = 500f;
            }
            QueueFree();
        }
    }
}
