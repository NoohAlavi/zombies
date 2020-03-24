using Godot;
using System;

public class GameOver : Control
{
    private Button _retryButton;

    public override void _Ready()
    {
        _retryButton = GetNode<Button>("RetryButton");
        _retryButton.Connect("pressed", this, "OnRetryButtonPressed");
    }

    public void OnRetryButtonPressed()
    {
        GetTree().ChangeScene("res://MainMenu/MainMenu.tscn");
    }
}
