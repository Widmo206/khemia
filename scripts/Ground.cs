using Godot;
using System;

public partial class Ground : VoxelTerrain
{
	private VoxelTool breakTool;
	private VoxelTool placeTool;


    public override void _Ready()
    {
        breakTool = GetVoxelTool();
		breakTool.Mode = VoxelTool.ModeEnum.Remove;
		placeTool = GetVoxelTool();
		placeTool.Mode = VoxelTool.ModeEnum.Set;
    }


	public void BreakTile(Vector3I position)
	{
		breakTool.DoPoint(position);
	}
}
