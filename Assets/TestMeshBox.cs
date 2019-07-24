﻿using Geometry_Algorithm;
using LinearAlgebra.VectorAlgebra;
using Mathd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshBox : MonoBehaviour {

    // Use this for initialization
    VoxTriFace voxTriFace;

    public List<GameObject> goList;
    public List<GameObject> meshGoList = new List<GameObject>();

    void Start () {
        VoxSpace voxSpace = new VoxSpace();
        voxTriFace = new VoxTriFace(voxSpace);
        VoxBoxViewer voxBoxViewer = new VoxBoxViewer();

        if (goList == null)
            return;

        Transform tf;

        System.DateTime startTime, endTime;
        System.TimeSpan time;
        startTime = System.DateTime.Now;

        int count = 0;


        for (int j = 0; j < goList[0].transform.childCount; j++)
        {
            tf = goList[0].transform.GetChild(j);
            MeshFilter mf = tf.GetComponent<MeshFilter>();
            Vector3[] vectors = mf.mesh.vertices;
            Vector3[] normals = mf.mesh.normals;
            int[] idxs = mf.mesh.triangles;
            Vector3[] vects = new Vector3[3];
            Vector[] vectxs = new Vector[3];
          
            for (int i = 0; i < idxs.Length; i += 3)
            {
                vects[0] = vectors[idxs[i]];
                vects[1] = vectors[idxs[i + 1]];
                vects[2] = vectors[idxs[i + 2]];

                Vector3 v = mf.transform.localToWorldMatrix.MultiplyPoint(vects[0]);
                vectxs[0] = new Vector(new double[] { v.x, v.y, v.z, 1 }, VectorType.Column);

                v = mf.transform.localToWorldMatrix.MultiplyPoint(vects[1]);
                vectxs[1] = new Vector(new double[] { v.x, v.y, v.z, 1 }, VectorType.Column);

                v = mf.transform.localToWorldMatrix.MultiplyPoint(vects[2]);
                vectxs[2] = new Vector(new double[] { v.x, v.y, v.z, 1 }, VectorType.Column);

                voxTriFace.TransTriFaceWorldVertexToVoxSpace(vectxs);
                // voxBoxViewer.AppendVoxBoxs(voxTriFace.voxBoxList.ToArray(), voxSpace);

                count += voxTriFace.voxBoxList.Count;
            }
        }

        endTime = System.DateTime.Now;
        time = endTime - startTime;
        int runTime = time.Milliseconds;

        Debug.Log("用时:" + runTime + "毫秒");
        Debug.Log("voxel数量:" + count + "个");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}