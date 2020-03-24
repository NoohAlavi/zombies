using Godot;
using System;

public class MainMenu : Control
{
    private Button _playButton;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("PlayButton");
        _playButton.Connect("pressed", this, "OnPlayButtonPressed");
    }

    public void OnPlayButtonPressed()
    {
        GetTree().ChangeScene("res://World/World.tscn");
    }
}
