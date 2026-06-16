using Godot;
using Godot.Collections;

public partial class CameraPivot : Node3D
{
	[Export] public Node3D cameraTarget;
	[Export] public float verticalOffset    = 1f;
	[Export] public float zoomSpeed         = 1f;
	[Export] public float zoomMin           = 2f;
	[Export] public float zoomMax           = 30f;
	[Export] public float sensitivityX      = 1f;
	[Export] public float sensitivityY      = 1f;
	[Export] public float selectionDistance = 1000f;

	private float targetZoom = 10f;
	private Vector2? capturedMousePosition = null;

	private Camera3D Camera;
	private RayCast3D CollisionChecker;


	[Signal] public delegate void ObjectSelectedEventHandler(Dictionary target);
	[Signal] public delegate void PlayerUseItemEventHandler(Dictionary target);
	
	
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


	private Dictionary RaycastFromCursor()
	{
		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
		Vector2 mousePos = GetViewport().GetMousePosition();

		Vector3 origin = Camera.ProjectRayOrigin(mousePos);
		Vector3 end = origin + Camera.ProjectRayNormal(mousePos) * selectionDistance;
		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
		query.CollideWithAreas = true;

		return spaceState.IntersectRay(query);
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
				capturedMousePosition ??= GetViewport().GetMousePosition(); // assign if null
				Input.MouseMode = Input.MouseModeEnum.Captured;
				Vector2 mouseMovement = motion.ScreenRelative;
				rotation += new Vector3(mouseMovement.Y * -0.005f * sensitivityY, mouseMovement.X * -0.005f * sensitivityX, 0);
				rotation.X = Mathf.Clamp(rotation.X, -Mathf.Pi/2, Mathf.Pi/2);
				Rotation = rotation;
				UpdateCameraZoom();
			}
		}
    }


    public override void _PhysicsProcess(double delta)
    {
		Vector3 position = Position;
		Vector3 toTarget = cameraTarget.Position + new Vector3(0, verticalOffset, 0) - position;
		position += 0.5f * toTarget;
		Position = position;

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
		}
		CollisionChecker.TargetPosition = new Vector3(0, 0, targetZoom);
		UpdateCameraZoom();
		
		if (!Input.IsActionPressed("rotate_camera"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			if (capturedMousePosition != null)
			{	// right side should never execute but has to be there because the compiler is dumb
				GetViewport().WarpMouse(capturedMousePosition ?? new Vector2(0, 0));
				capturedMousePosition = null;
			}
			Dictionary target = RaycastFromCursor();
			if (target.Count > 0)
			{
				EmitSignal(SignalName.ObjectSelected, target);
			}
			if (Input.IsActionJustPressed("use_item"))
			{
				EmitSignal(SignalName.PlayerUseItem, target);
			}
		}
    }
}
