using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public AnimatedSprite2D animatedSprite2D;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private void _ChangeSpriteDirection(float direction)
	{
		if (direction > 0)
		{
			animatedSprite2D.FlipH = false;
			return;
		}

		if (direction < 0)
		{
			animatedSprite2D.FlipH = true;
		}
	}

	private void _ChangeSpriteAnimation(float direction)
	{
		if (IsOnFloor())
		{
			if (direction == 0)
			{
				animatedSprite2D.Play("idle");
				return;
			}

			animatedSprite2D.Play("run");
			return;
		}

		animatedSprite2D.Play("jump");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float direction = Input.GetAxis("move_left", "move_right");
		this._ChangeSpriteDirection(direction);
		this._ChangeSpriteAnimation(direction);

		if (direction != 0)
		{
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}


		Velocity = velocity;
		MoveAndSlide();
	}
}
