using Godot;
using System;

public partial class CameraPivot : Node3D
{
	[Export] public Node3D target;
	[Export] public float verticalOffset = 1f;
	[Export] public float zoomSpeed      = 1f;
	[Export] public float zoomMin        = 2f;
	[Export] public float zoomMax        = 30f;
	[Export] public float sensitivityX   = 1f;
	[Export] public float sensitivityY   = 1f;

	private float targetZoom = 10f;

	private Camera3D Camera;
	private RayCast3D CollisionChecker;


	private void UpdateCameraZoom()
	{
		float zoomLevel;
		CollisionChecker.ForceRaycastUpdate();
		if (CollisionChecker.IsColliding())
		{
			zoomLevel = (CollisionChecker.GetCollisionPoint() - Position).Length() - 2*Camera.Near;
		}
		else
		{
			zoomLevel = targetZoom;
		}
		Camera.Position = new Vector3(0, 0, zoomLevel);
	}


	public override void _Ready()
	{
		Camera			 = GetNode<Camera3D>("3PCamera");
		CollisionChecker = GetNode<RayCast3D>("CollisionChecker");
	}

    
    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion motion = @event as InputEventMouseMotion;
			Vector3 rotation = Rotation;
			if (Input.IsActionPressed("rotate_camera"))
			{
				
				Vector2 mouseMovement = motion.ScreenRelative;
				rotation += new Vector3(mouseMovement.Y * -0.005f * sensitivityY, mouseMovement.X * -0.005f * sensitivityX, 0);
				rotation.X = Mathf.Clamp(rotation.X, -Mathf.Pi, Mathf.Pi);
				Rotation = rotation;
				UpdateCameraZoom();
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}
    }


    public override void _Process(double delta)
    {
		float zoomDirection = 0f; // workaround because GetAxis didn't work for some reason
		if (Input.IsActionJustPressed("zoom_in"))
		{
			zoomDirection = -1f;
		}
		if (Input.IsActionJustPressed("zoom_out"))
		{
			zoomDirection += 1f;
		}
		if (zoomDirection != 0f)
		{
			targetZoom += zoomSpeed * zoomDirection;
			targetZoom = Mathf.Clamp(targetZoom, zoomMin, zoomMax);
			GD.Print(targetZoom);
		}
		CollisionChecker.TargetPosition = new Vector3(0, 0, targetZoom);
		UpdateCameraZoom();
		

		Vector3 position = Position;
		Vector3 toTarget = target.Position + new Vector3(0, verticalOffset, 0) - position;
		position += 0.5f * toTarget;
		Position = position;
    }
}
