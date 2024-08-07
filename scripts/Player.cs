using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public AnimatedSprite2D animatedSprite2D;
	public const float Speed = 300.0f;
	public const float RollSpeed = 500.0f;
	public const float JumpVelocity = -400.0f;
	public bool isDefending = false;
	public bool isOnCutscene = false;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private void _ChangeSpriteDirection(float direction)
	{
		if (direction > 0 || this.isOnCutscene)
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
		if (this.isOnCutscene)
		{
			if (IsOnFloor()) animatedSprite2D.Play("idle");
			return;
		}

		if (Input.IsActionPressed("attack"))
		{
			animatedSprite2D.Play("attack");
			isDefending = false;
			return;
		}

		if (Input.IsActionPressed("defense"))
		{
			animatedSprite2D.Play("defense");
			isDefending = true;
			return;
		}

		isDefending = false;

		if (IsOnFloor())
		{
			if (direction == 0)
			{
				animatedSprite2D.Play("idle");
				return;
			}


			if (Input.IsActionPressed("roll"))
			{
				animatedSprite2D.Play("roll");
				return;
			}

			animatedSprite2D.Play("run");
			return;
		}

		animatedSprite2D.Play("jump");
	}

	private Vector2 _GetPlayerVelocity(float direction, double delta)
	{
		Vector2 velocity = Velocity;
		bool isOnFloor = IsOnFloor();
		bool isMoving = direction != 0;
		float movementSpeed = Speed;

		if (!isOnFloor) velocity.Y += gravity * (float)delta;

		if (isOnCutscene)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, movementSpeed);
			return velocity;
		}

		if (Input.IsActionPressed("defense") || Input.IsActionPressed("attack"))
		{
			if (isOnFloor) velocity.X = 0;
			return velocity;
		}

		if (Input.IsActionJustPressed("jump") && isOnFloor)
		{
			velocity.Y = JumpVelocity;
		}
		else if (Input.IsActionPressed("roll") && isOnFloor)
		{
			movementSpeed = RollSpeed;
		}

		velocity.X = isMoving ? (direction * movementSpeed) : Mathf.MoveToward(Velocity.X, 0, movementSpeed);

		return velocity;
	}

	public override void _PhysicsProcess(double delta)
	{
		float direction = Input.GetAxis("move_left", "move_right");
		this._ChangeSpriteDirection(direction);
		this._ChangeSpriteAnimation(direction);
		Velocity = this._GetPlayerVelocity(direction, delta);
		MoveAndSlide();
	}

	public void startCutsceneMode()
	{
		this.isOnCutscene = true;
	}
}
