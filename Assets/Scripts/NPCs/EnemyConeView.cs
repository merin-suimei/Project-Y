using System.Collections.Generic;
using UnityEngine;
using static Enemy;

[RequireComponent(typeof(Enemy))]
public class EnemyConeView : MonoBehaviour
{
    private Enemy enemy;
    private float detectionSemiconeAngle;
    private float detectionRange;
    private LayerMask raycastIgnore;

    [SerializeField] public float meshResolution = 0.5f;
    [SerializeField] public float edgeResolveIterations = 3f;
    [SerializeField] public float edgeDistanceTreshold = 0.5f;
    [SerializeField] public float viewHeightOffset = 4.5f;
    [SerializeField] public float middleLine = 0.81f;
    [SerializeField] private float obstacleInset = 0.02f;
    [SerializeField] public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        detectionSemiconeAngle = enemy.detectionSemiconeAngle;
        detectionRange = enemy.detectionRange;
        raycastIgnore = enemy.raycastIgnore;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        float fullAngle = detectionSemiconeAngle * 2f;
        int stepCount = Mathf.RoundToInt(fullAngle * meshResolution);
        float stepAngleSize = fullAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float localAngle = -detectionSemiconeAngle + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(localAngle);
            if (i > 0)
            {
                bool edgeDistTresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceTreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistTresholdExceeded))
                {
                    EdgeInfo edge = FindeEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        // �������� ��� ����� �� middleLine ��� ������ �����
        for (int i = 0; i < viewPoints.Count; i++)
        {
            Vector3 bottomViewPoint = viewPoints[i] - Vector3.up * middleLine;
            viewPoints[i] = bottomViewPoint;

        }

        // ������� �����
        int vertexCountTop = viewPoints.Count + 1;
        Vector3[] verticesTop = new Vector3[vertexCountTop];
        int[] trianglesTop = new int[(vertexCountTop - 2) * 3];

        verticesTop[0] = Vector3.up * viewHeightOffset;
        // ������ ������ �����
        List<Vector3> viewPointsTop = new List<Vector3>();
        for (int i = 0; i < viewPoints.Count; i++)
        {
            Vector3 localPoint = transform.InverseTransformPoint(viewPoints[i]);
            Vector2 flatPoint = new Vector2(localPoint.x, localPoint.z);
            float distanceFromOrigin = flatPoint.magnitude;
            float t = Mathf.Clamp01(distanceFromOrigin / detectionRange);
            float topY = Mathf.Lerp(viewHeightOffset, 0f, t);
            Vector3 topPoint = new Vector3(localPoint.x, topY, localPoint.z);
            viewPointsTop.Add(topPoint);
        }

        for (int i = 0; i < vertexCountTop - 1; i++)
        {
            verticesTop[i + 1] = viewPointsTop[i];

            if (i < vertexCountTop - 2)
            {
                trianglesTop[i * 3] = 0;
                trianglesTop[i * 3 + 1] = i + 1;
                trianglesTop[i * 3 + 2] = i + 2;
            }
        }

        // ������ �����
        int vertexCountBottom = viewPoints.Count + 1;
        Vector3[] verticesBottom = new Vector3[vertexCountBottom];
        int[] trianglesBottom = new int[(vertexCountBottom - 2) * 3];

        verticesBottom[0] = Vector3.zero;
        for (int i = 0; i < vertexCountBottom - 1; i++)
        {
            verticesBottom[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCountBottom - 2)
            {
                trianglesBottom[i * 3] = 0;
                trianglesBottom[i * 3 + 1] = i + 2;
                trianglesBottom[i * 3 + 2] = i + 1;
            }
        }

        // ���������� �����
        int sideSegmentCount = viewPoints.Count - 1;

        Vector3[] verticesSides = new Vector3[sideSegmentCount * 4];
        int[] trianglesSides = new int[sideSegmentCount * 6];

        for (int i = 0; i < sideSegmentCount; i++)
        {
            int v = i * 4;
            int t = i * 6;

            Vector3 b0 = transform.InverseTransformPoint(viewPoints[i]);
            Vector3 b1 = transform.InverseTransformPoint(viewPoints[i + 1]);
            Vector3 top0 = viewPointsTop[i];
            Vector3 top1 = viewPointsTop[i + 1];

            verticesSides[v + 0] = b0;
            verticesSides[v + 1] = b1;
            verticesSides[v + 2] = top0;
            verticesSides[v + 3] = top1;

            trianglesSides[t + 0] = v + 0;
            trianglesSides[t + 1] = v + 3;
            trianglesSides[t + 2] = v + 2;

            trianglesSides[t + 3] = v + 0;
            trianglesSides[t + 4] = v + 1;
            trianglesSides[t + 5] = v + 3;
        }

        // ������� �����
        // �����
        Vector3[] verticesLeftCap = new Vector3[4];
        int[] trianglesLeftCap = new int[6];

        verticesLeftCap[0] = Vector3.zero;
        verticesLeftCap[1] = transform.InverseTransformPoint(viewPoints[0]);
        verticesLeftCap[2] = viewPointsTop[0];
        verticesLeftCap[3] = Vector3.up * viewHeightOffset;

        trianglesLeftCap[0] = 0;
        trianglesLeftCap[1] = 1;
        trianglesLeftCap[2] = 2;

        trianglesLeftCap[3] = 0;
        trianglesLeftCap[4] = 2;
        trianglesLeftCap[5] = 3;

        // ������
        int last = viewPoints.Count - 1;

        Vector3[] verticesRightCap = new Vector3[4];
        int[] trianglesRightCap = new int[6];

        verticesRightCap[0] = Vector3.zero;
        verticesRightCap[1] = Vector3.up * viewHeightOffset;
        verticesRightCap[2] = viewPointsTop[last];
        verticesRightCap[3] = transform.InverseTransformPoint(viewPoints[last]);

        trianglesRightCap[0] = 0;
        trianglesRightCap[1] = 1;
        trianglesRightCap[2] = 2;

        trianglesRightCap[3] = 0;
        trianglesRightCap[4] = 2;
        trianglesRightCap[5] = 3;

        // ����������� ���� ������
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        vertices.AddRange(verticesBottom);
        for (int i = 0; i < trianglesBottom.Length; i++)
        {
            triangles.Add(trianglesBottom[i]);
        }
        int topStart = vertices.Count;
        vertices.AddRange(verticesTop);
        for (int i = 0; i < trianglesTop.Length; i++)
        {
            triangles.Add(trianglesTop[i] + topStart);
        }
        int leftStart = vertices.Count;
        vertices.AddRange(verticesLeftCap);
        for (int i = 0; i < trianglesLeftCap.Length; i++)
        {
            triangles.Add(trianglesLeftCap[i] + leftStart);
        }
        int sideStart = vertices.Count;
        vertices.AddRange(verticesSides);
        for (int i = 0; i < trianglesSides.Length; i++)
        {
            triangles.Add(trianglesSides[i] + sideStart);
        }
        int rightStart = vertices.Count;
        vertices.AddRange(verticesRightCap);
        for (int i = 0; i < trianglesRightCap.Length; i++)
        {
            triangles.Add(trianglesRightCap[i] + rightStart);
        }


        viewMesh.Clear();
        viewMesh.vertices = vertices.ToArray();
        viewMesh.triangles = triangles.ToArray();
        viewMesh.RecalculateNormals();
        viewMesh.RecalculateBounds();
    }

    ViewCastInfo ViewCast(float localAngle)
    {
        Quaternion rot = Quaternion.Euler(0f, localAngle, 0f);
        Vector3 dirFromAngle = transform.rotation * rot * Vector3.forward;
        Vector3 rayOrigin = transform.position + Vector3.up * middleLine;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, dirFromAngle, out hit, detectionRange, ~raycastIgnore))
        {
            float adjustedDistance = Mathf.Max(0f, hit.distance - obstacleInset);
            Vector3 adjustedPoint = rayOrigin + dirFromAngle * adjustedDistance;
            return new ViewCastInfo(true, adjustedPoint, adjustedDistance, localAngle);
        }
        else
        {
            return new ViewCastInfo(false, rayOrigin + dirFromAngle * detectionRange, detectionRange, localAngle);
        }
    }

    EdgeInfo FindeEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDistTresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceTreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistTresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
}
