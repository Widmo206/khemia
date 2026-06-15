using Godot;
using Godot.Collections;

public partial class TileSelector : MeshInstance3D
{
	public enum Mode
	{
		Break = -1,
		Place = 1,
	}


	public Mode mode = Mode.Break;


	public Vector3 GetTileCenteredPosition(Vector3 current)
	{
		return new Vector3(
			Mathf.Snapped(current.X - 0.5f, 1) + 0.5f,
			Mathf.Snapped(current.Y - 0.5f, 1) + 0.5f,
			Mathf.Snapped(current.Z - 0.5f, 1) + 0.5f
		);
	}


	public void OnCameraRaycast(Dictionary data)
	{
		Node3D collider = (Node3D)data["collider"];
		if (collider is VoxelTerrain){
			Vector3 targetPosition = (Vector3)data["position"] + (int)mode * (Vector3)data["normal"] * 0.001f; // is this jank?
			Position = GetTileCenteredPosition(targetPosition);
		}
	}
}
