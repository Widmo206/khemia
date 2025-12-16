using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;

	enum MovementState
	{
		Stationary,
		Walking,
		Running,
	}

	private enum Direction
	{
		Left,
		Right,
	}

	MovementState movementState = MovementState.Stationary;
	Direction facingDirection = Direction.Right;


	AnimatedSprite2D MainSprite;


	public override void _Ready()
	{
		MainSprite = GetNode<AnimatedSprite2D>("MainSprite");
	}


	public override void _PhysicsProcess(double _delta)
	{
		Vector2 velocity = Velocity;


		Vector2 direction = new Vector2(
			Input.GetAxis("move-west",  "move-east"),	// left - right
			Input.GetAxis("move-north", "move-south")	//   up - down
		).Normalized();

		if (direction.X != 0)
		{
			facingDirection = (direction.X > 0 ? Direction.Right : Direction.Left);
		}

		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed); // speed decay??
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed); // speed decay??
		}

		Velocity = velocity;
		MoveAndSlide();
	}


	public override void _Process(double _delta)
	{
		MainSprite.FlipH = (facingDirection == Direction.Right ? false : true);
	}
}
