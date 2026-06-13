using Godot;
using System;

public partial class CameraPivot : Node3D
{
	[Export] public Node3D target;
	[Export] public float zoomSpeed    = 1f;
	[Export] public float zoomMin      = 2f;
	[Export] public float zoomMax      = 30f;
	[Export] public float sensitivityX = 1f;
	[Export] public float sensitivityY = 1f;

	private Camera3D Camera;
	private RayCast3D CollisionChecker;

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
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		}
    }


    public override void _Process(double delta)
    {
		float zoom_direction = Input.GetAxis("zoom_in", "zoom_out");
		if (zoom_direction != 0f)
		{
			float new_zoom = Camera.Position.Z + zoomSpeed * zoom_direction;
			Mathf.Clamp(new_zoom, zoomMin, zoomMax);
			Camera.Position = new Vector3(0, 0, new_zoom);
		}

		Vector3 position = Position;
		Vector3 toTarget = target.Position - position;
		position += 0.5f * toTarget;
		Position = position;
    }


		/*
		if event is InputEventMouseMotion:
			if Input.is_action_pressed("camera_pivot"):
				Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
				var mouse_movement_x = event.relative.x
				var mouse_movement_y = event.relative.y
				rotation += Vector3(mouse_movement_y*-0.005*sensitivity_y, mouse_movement_x*-0.005*sensitivity_x, 0)
				rotation.x = clamp(rotation.x, -PI/2, PI/2)
			else:
				Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
		elif event is InputEventMouse:
			if Input.is_action_just_pressed("camera_zoom_in"):
				$Camera.position.z -= zoom_speed
			if Input.is_action_just_pressed("camera_zoom_out"):
				$Camera.position.z += zoom_speed
			$Camera.position.z = clamp($Camera.position.z, zoom_min, zoom_max)
		*/
}
