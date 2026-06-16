using Godot;
using System;
using System.Data.Common;

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


	public void PlaceTile(Vector3I position, ulong id)
	{
		placeTool.Value = id;
		placeTool.DoPoint(position);
	}
}
