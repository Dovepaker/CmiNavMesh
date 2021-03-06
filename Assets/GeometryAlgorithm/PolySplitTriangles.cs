﻿using Mathd;
using System.Collections.Generic;

namespace Geometry_Algorithm
{
    public class PolySplitTriangles
    {
        GeometryAlgorithm geoAlgor;

        //合法的三角形分割列表
        LinkedList<LinkedListNode<Vector3d>[]> legalTriNodeList = new LinkedList<LinkedListNode<Vector3d>[]>();

        public PolySplitTriangles(GeometryAlgorithm geoAlgor)
        {
            this.geoAlgor = geoAlgor;
        }

        public List<Vector3d[]> Split(Poly poly)
        {
            LinkedList<Vector3d> polyVertList = CreatePolyVertToList(poly);
            return Split(polyVertList, poly.faceNormal);
        }

        public List<Vector3d[]> Split(LinkedList<Vector3d> polyVertList, Vector3d polyFaceNormal)
        {
            if (polyVertList == null)
                return null;

            Vector3d dir;
            LinkedListNode<Vector3d>[] abc;
            List<Vector3d[]> triVertexList = new List<Vector3d[]>();
            legalTriNodeList.Clear();

            while (polyVertList.Count > 2)
            {
                for (var node = polyVertList.First; node != null; node = node.Next)
                {
                    //获取一个耳三角形
                    abc = GetTriNodes(polyVertList, node);

                    Vector3d ab = abc[1].Value - abc[0].Value;
                    Vector3d bc = abc[2].Value - abc[1].Value;
                    dir = Vector3d.Cross(ab, bc);
                    int ret = geoAlgor.CmpParallelVecDir(dir, polyFaceNormal);
                    if (ret == -1)
                        continue;

                    Vector3d ac = abc[2].Value - abc[0].Value;
                    Vector3d ba = abc[0].Value - abc[1].Value;
                    Vector3d ca = abc[0].Value - abc[2].Value;
                    Vector3d cb = abc[1].Value - abc[2].Value;
                    Vector3d aCross = Vector3d.Cross(ab, ac);
                    Vector3d bCross = Vector3d.Cross(bc, ba);
                    Vector3d cCross = Vector3d.Cross(ca, cb);

                    //
                    bool isInTri = false;
                    for (var node2 = polyVertList.First; node2 != null; node2 = node2.Next)
                    {
                        if (node2 == abc[0] || node2 == abc[1] || node2 == abc[2])
                            continue;

                        Vector3d pt = node2.Value;
                        isInTri = TestPointInTri(pt, abc[0].Value, abc[1].Value, abc[2].Value, aCross, bCross, cCross);
                        if (isInTri == true)
                            break;
                    }

                    if (isInTri == false)
                        legalTriNodeList.AddLast(abc);

                    if (polyVertList.Count == 3)
                        break;
                }

                LinkedListNode<Vector3d>[] nodes = GetLimitShortSideTri(legalTriNodeList);
                if (nodes != null)
                {
                    polyVertList.Remove(nodes[1]);
                    Vector3d[] vertexs = new Vector3d[3];
                    vertexs[0] = nodes[0].Value;
                    vertexs[1] = nodes[1].Value;
                    vertexs[2] = nodes[2].Value;
                    triVertexList.Add(vertexs);
                }
                else
                {
                    break;
                }

                legalTriNodeList.Clear();
            }

            return triVertexList;
        }

        LinkedListNode<Vector3d>[] GetLimitShortSideTri(LinkedList<LinkedListNode<Vector3d>[]> triList)
        {
            if (triList == null || triList.Count == 0)
                return null;

            LinkedListNode<Vector3d>[] triNodes;
            LinkedListNode<Vector3d>[] minSideTriNodes = triList.First.Value;
            Vector3d ca = minSideTriNodes[0].Value - minSideTriNodes[2].Value;
            double minSideLen = ca.sqrMagnitude;

            for (var node = triList.First.Next; node != null; node = node.Next)
            {
                triNodes = node.Value;
                ca = triNodes[0].Value - triNodes[2].Value;
                if (ca.sqrMagnitude < minSideLen)
                {
                    minSideLen = ca.sqrMagnitude;
                    minSideTriNodes = triNodes;
                }
            }

            return minSideTriNodes;
        }


        LinkedList<Vector3d> CreatePolyVertToList(Poly poly)
        {
            if (poly == null)
                return null;

            LinkedList<Vector3d> polyVertList = new LinkedList<Vector3d>();

            Vector3d[] triPts = poly.vertexsList[0];
            for (int i = 0; i < triPts.Length; i++)
            {
                polyVertList.AddLast(triPts[i]);
            }

            return polyVertList;
        }


        LinkedListNode<Vector3d>[] GetTriNodes(LinkedList<Vector3d> polyVertList, LinkedListNode<Vector3d> firstNode)
        {
            LinkedListNode<Vector3d>[] triNodes = new LinkedListNode<Vector3d>[3];

            triNodes[0] = firstNode;

            if (firstNode.Next == null)
            {
                triNodes[1] = polyVertList.First;
                triNodes[2] = polyVertList.First.Next;
            }
            else
            {
                triNodes[1] = firstNode.Next;

                if (firstNode.Next.Next == null)
                {
                    triNodes[2] = polyVertList.First;
                }
                else
                {
                    triNodes[2] = firstNode.Next.Next;
                }
            }

            return triNodes;
        }

        
        /// <summary>     
            /// 判断点是否在三角形内部的方法如下：
            ///如果D在三角形ABC内部，则D与C在直线AB的同一侧，D与B在直线AC的同一侧，D与A在直线BC的同一侧。
            ///同时满足这三个条件，则可测试出点D在三角形ABC内部。
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="triPtA"></param>
        /// <param name="triPtB"></param>
        /// <param name="triPtC"></param>
        /// <param name="aCross"></param>
        /// <param name="bCross"></param>
        /// <param name="cCross"></param>
        /// <returns></returns>
        bool TestPointInTri(Vector3d pt, 
            Vector3d triPtA, Vector3d triPtB, Vector3d triPtC,
            Vector3d aCross, Vector3d bCross, Vector3d cCross)
        {
            Vector3d ab = triPtB - triPtA;
            Vector3d ad = pt - triPtA;
            Vector3d s2 = Vector3d.Cross(ab, ad); 
            int ret = geoAlgor.CmpParallelVecDir(aCross, s2);
            if (ret == -1)
                return false;

            Vector3d bc = triPtC - triPtB;
            Vector3d bd = pt - triPtB;
            s2 = Vector3d.Cross(bc, bd);
            ret = geoAlgor.CmpParallelVecDir(bCross, s2);
            if (ret == -1)
                return false;

            Vector3d ca = triPtA - triPtC;
            Vector3d cd = pt - triPtC;
            s2 = Vector3d.Cross(ca, cd);
            ret = geoAlgor.CmpParallelVecDir(cCross, s2);
            if (ret == -1)
                return false;

            return true;
        }

    }
}
