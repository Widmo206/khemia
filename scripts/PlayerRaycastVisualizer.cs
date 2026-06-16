using Godot;
using Godot.Collections;

public partial class PlayerRaycastVisualizer : MeshInstance3D
{
	public void OnCameraRaycast(Dictionary target)
	{
		if (target.Count > 0)
		{
			Position = (Vector3)target["position"];
			Visible = true;
		}
		else
		{
			Visible = false;
		}
	}
}
