using Godot;
using Godot.Collections;
using System;

public partial class Player3D : CharacterBody3D
{
	private Node3D Camera;

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;


	public Vector3I GetTilePosition(Vector3 position)
	{
		return new Vector3I(
			(int)Mathf.Snapped(position.X - 0.5f, 1),
			(int)Mathf.Snapped(position.Y - 0.5f, 1),
			(int)Mathf.Snapped(position.Z - 0.5f, 1)
		);
	}


    public override void _Ready()
    {
        base._Ready();
		Camera = GetNode<Node3D>("../CameraPivot");
    }


	public void OnUseItem(Dictionary target)
	{
		if (target.Count == 0)
		{
			return;
		}
		Node3D target_node = (Node3D)target["collider"];
		if (target_node is VoxelTerrain)
		{
			Vector3 offsetTargetPosition = (Vector3)target["position"] - (Vector3)target["normal"] * 0.001f;
			(target_node as Ground).BreakTile(GetTilePosition(offsetTargetPosition));
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 cameraDirection = Camera.Rotation;
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized().Rotated(Vector3.Up, cameraDirection.Y);
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
