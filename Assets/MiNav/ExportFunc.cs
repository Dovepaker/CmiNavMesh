﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MINAV
{
    public class ExportFunc
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SimpleVector3c
        {
            public float x, y, z;
        };

        [DllImport("MiNavMeshPlus")]
        public static extern IntPtr CreateVoxelSpace();

        [DllImport("MiNavMeshPlus")]
        public static extern void DisposeVoxelSpace(IntPtr voxelSpace);

        [DllImport("MiNavMeshPlus")]
        public static extern void SetCellSize(IntPtr voxelSpace, float cellSize, float cellHeight);

        [DllImport("MiNavMeshPlus")]
        public static extern float GetCellSize(IntPtr voxelSpace);

        [DllImport("MiNavMeshPlus")]
        public static extern float GetCellHeight(IntPtr voxelSpace);

        [DllImport("MiNavMeshPlus")]
        public static extern void CreateSpaceGrids(IntPtr voxelSpace);

        [DllImport("MiNavMeshPlus")]
        public static extern void CreateVoxels(IntPtr voxelSpace, IntPtr solidSpanGroup);

        [DllImport("MiNavMeshPlus")]
        public static extern void FreeSolidSpanGridsMemory(IntPtr voxelSpace);

        [DllImport("MiNavMeshPlus")]
        public static extern void TransModelVertexs(
            IntPtr voxelSpace,
            SimpleVector3c vert0,
            SimpleVector3c vert1,
            SimpleVector3c vert2);

        [DllImport("MiNavMeshPlus")]
        public static extern IntPtr CreateSolidSpanGroup(IntPtr voxSpace);

        [DllImport("MiNavMeshPlus")]
        public static extern IntPtr GetSolidSpanGrids(IntPtr solidSpanGroup);

        [DllImport("MiNavMeshPlus")]
        public static extern int GetGridCount(IntPtr solidSpanGroup);
    }
}
