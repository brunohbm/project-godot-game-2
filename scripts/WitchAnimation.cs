using Godot;
using System;

public partial class WitchAnimation : Area2D
{
	[Export]
	public AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _onBodyEntered(Node area)
	{
		if (area.IsInGroup("Player"))
		{
			if (area is Player player) player.startCutsceneMode();
			animationPlayer.Play("witch_appearance");
		}
	}
}
