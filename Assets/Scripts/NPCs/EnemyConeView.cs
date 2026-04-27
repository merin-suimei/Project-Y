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

    [SerializeField] private float meshResolution = 0.5f;
    [SerializeField] private float edgeResolveIterations = 3f;
    [SerializeField] private float edgeDistanceTreshold = 0.5f;
    [SerializeField] private float viewHeightOffset = 4.5f;
    [SerializeField] private float middleLine = 0.81f;
    [SerializeField] private float obstacleInset = 0.02f;
    [SerializeField] public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    private readonly List<Vector3> _viewPoints = new();
    private readonly List<Vector3> _bottomLocal = new();
    private readonly List<Vector3> _viewPointsTop = new();
    private readonly List<Vector3> _vertices = new();
    private readonly List<int> _triangles = new();

    [Header("Edge Lines")]
    [SerializeField] private Material edgeLineMaterial;
    public Material EdgeLineMaterial => edgeLineMaterial;
    [SerializeField] private float edgeLineWidth = 0.05f;
    [Header("Bottom triangle")]
    [SerializeField] private bool drawLeftBottomRay = false;
    [SerializeField] private bool drawRightBottomRay = false;
    [SerializeField] private bool drawBottomArc = false;
    [Header("Top arc and center line")]
    [SerializeField] private bool drawTopArc = false;
    [SerializeField] private bool drawCenterLine = false;
    [Header("Left side")]
    [Tooltip("Edge is used if the raycast encountered an obstacle")]
    [SerializeField] private bool drawLeftEdge = false;
    [SerializeField] private bool drawLeftTopRay = false;
    [Header("Right side")]
    [Tooltip("Edge is used if the raycast encountered an obstacle")]
    [SerializeField] private bool drawRightEdge = false;
    [SerializeField] private bool drawRightTopRay = false;

    private LineRenderer bottomArcLine;
    private LineRenderer topArcLine;
    private LineRenderer leftEdgeLine;
    private LineRenderer rightEdgeLine;
    private LineRenderer leftBottomRayLine;
    private LineRenderer rightBottomRayLine;
    private LineRenderer leftTopRayLine;
    private LineRenderer rightTopRayLine;
    private LineRenderer centerSpineLine;

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

        CreateAllEdgeLines();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        _viewPoints.Clear();
        _bottomLocal.Clear();
        _viewPointsTop.Clear();
        _vertices.Clear();
        _triangles.Clear();

        float fullAngle = detectionSemiconeAngle * 2f;
        int stepCount = Mathf.RoundToInt(fullAngle * meshResolution);
        float stepAngleSize = fullAngle / stepCount;
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
                        _viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        _viewPoints.Add(edge.pointB);
                    }
                }
            }

            _viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        // �������� ��� ����� �� middleLine ��� ������ �����
        for (int i = 0; i < _viewPoints.Count; i++)
        {
            Vector3 bottomViewPoint = _viewPoints[i] - Vector3.up * middleLine;
            _viewPoints[i] = bottomViewPoint;

        }

        //List<Vector3> bottomLocal = new List<Vector3>(viewPoints.Count);
        for (int i = 0; i < _viewPoints.Count; i++)
        {
            _bottomLocal.Add(transform.InverseTransformPoint(_viewPoints[i]));
        }

        // ������� �����
        int vertexCountTop = _viewPoints.Count + 1;
        Vector3[] verticesTop = new Vector3[vertexCountTop];
        int[] trianglesTop = new int[(vertexCountTop - 2) * 3];

        verticesTop[0] = Vector3.up * viewHeightOffset;
        // ������ ������ �����
        for (int i = 0; i < _viewPoints.Count; i++)
        {
            Vector3 localPoint = _bottomLocal[i];
            Vector2 flatPoint = new Vector2(localPoint.x, localPoint.z);
            float distanceFromOrigin = flatPoint.magnitude;
            float t = Mathf.Clamp01(distanceFromOrigin / detectionRange);
            float topY = Mathf.Lerp(viewHeightOffset, 0f, t);
            Vector3 topPoint = new Vector3(localPoint.x, topY, localPoint.z);
            _viewPointsTop.Add(topPoint);
        }

        for (int i = 0; i < vertexCountTop - 1; i++)
        {
            verticesTop[i + 1] = _viewPointsTop[i];

            if (i < vertexCountTop - 2)
            {
                trianglesTop[i * 3] = 0;
                trianglesTop[i * 3 + 1] = i + 1;
                trianglesTop[i * 3 + 2] = i + 2;
            }
        }

        // ������ �����
        int vertexCountBottom = _bottomLocal.Count + 1;
        Vector3[] verticesBottom = new Vector3[vertexCountBottom];
        int[] trianglesBottom = new int[(vertexCountBottom - 2) * 3];

        verticesBottom[0] = Vector3.zero;
        for (int i = 0; i < vertexCountBottom - 1; i++)
        {
            verticesBottom[i + 1] = _bottomLocal[i];

            if (i < vertexCountBottom - 2)
            {
                trianglesBottom[i * 3] = 0;
                trianglesBottom[i * 3 + 1] = i + 2;
                trianglesBottom[i * 3 + 2] = i + 1;
            }
        }

        // ���������� �����
        int sideSegmentCount = _viewPoints.Count - 1;

        Vector3[] verticesSides = new Vector3[sideSegmentCount * 4];
        int[] trianglesSides = new int[sideSegmentCount * 6];

        for (int i = 0; i < sideSegmentCount; i++)
        {
            int v = i * 4;
            int t = i * 6;

            Vector3 b0 = _bottomLocal[i];
            Vector3 b1 = _bottomLocal[i + 1];
            Vector3 top0 = _viewPointsTop[i];
            Vector3 top1 = _viewPointsTop[i + 1];

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
        verticesLeftCap[1] = _bottomLocal[0];
        verticesLeftCap[2] = _viewPointsTop[0];
        verticesLeftCap[3] = Vector3.up * viewHeightOffset;

        trianglesLeftCap[0] = 0;
        trianglesLeftCap[1] = 1;
        trianglesLeftCap[2] = 2;

        trianglesLeftCap[3] = 0;
        trianglesLeftCap[4] = 2;
        trianglesLeftCap[5] = 3;

        // ������
        int last = _viewPoints.Count - 1;

        Vector3[] verticesRightCap = new Vector3[4];
        int[] trianglesRightCap = new int[6];

        verticesRightCap[0] = Vector3.zero;
        verticesRightCap[1] = Vector3.up * viewHeightOffset;
        verticesRightCap[2] = _viewPointsTop[last];
        verticesRightCap[3] = _bottomLocal[last];

        trianglesRightCap[0] = 0;
        trianglesRightCap[1] = 1;
        trianglesRightCap[2] = 2;

        trianglesRightCap[3] = 0;
        trianglesRightCap[4] = 2;
        trianglesRightCap[5] = 3;

        // ����������� ���� ������
        //List<Vector3> vertices = new List<Vector3>();
        //List<int> triangles = new List<int>();
        _vertices.AddRange(verticesBottom);
        for (int i = 0; i < trianglesBottom.Length; i++)
        {
            _triangles.Add(trianglesBottom[i]);
        }
        int topStart = _vertices.Count;
        _vertices.AddRange(verticesTop);
        for (int i = 0; i < trianglesTop.Length; i++)
        {
            _triangles.Add(trianglesTop[i] + topStart);
        }
        int leftStart = _vertices.Count;
        _vertices.AddRange(verticesLeftCap);
        for (int i = 0; i < trianglesLeftCap.Length; i++)
        {
            _triangles.Add(trianglesLeftCap[i] + leftStart);
        }
        int sideStart = _vertices.Count;
        _vertices.AddRange(verticesSides);
        for (int i = 0; i < trianglesSides.Length; i++)
        {
            _triangles.Add(trianglesSides[i] + sideStart);
        }
        int rightStart = _vertices.Count;
        _vertices.AddRange(verticesRightCap);
        for (int i = 0; i < trianglesRightCap.Length; i++)
        {
            _triangles.Add(trianglesRightCap[i] + rightStart);
        }


        viewMesh.Clear();
        viewMesh.vertices = _vertices.ToArray();
        viewMesh.triangles = _triangles.ToArray();
        viewMesh.RecalculateNormals();
        viewMesh.RecalculateBounds();

        UpdateEdgeLines(_bottomLocal, _viewPointsTop);
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

    // lines
    private void CreateAllEdgeLines()
    {
        bottomArcLine = CreateEdgeLine("BottomArcLine");
        topArcLine = CreateEdgeLine("TopArcLine");
        leftEdgeLine = CreateEdgeLine("LeftEdgeLine");
        rightEdgeLine = CreateEdgeLine("RightEdgeLine");
        leftBottomRayLine = CreateEdgeLine("LeftBottomRayLine");
        rightBottomRayLine = CreateEdgeLine("RightBottomRayLine");
        leftTopRayLine = CreateEdgeLine("LeftTopRayLine");
        rightTopRayLine = CreateEdgeLine("RightTopRayLine");
        centerSpineLine = CreateEdgeLine("CenterSpineLine");
    }
    private LineRenderer CreateEdgeLine(string lineName)
    {
        GameObject lineObject = new GameObject(lineName);
        lineObject.transform.SetParent(transform, false);

        LineRenderer lr = lineObject.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.alignment = LineAlignment.View;
        lr.widthMultiplier = edgeLineWidth;
        lr.material = edgeLineMaterial;
        lr.positionCount = 0;
        lr.numCornerVertices = 2;
        lr.numCapVertices = 2;

        return lr;
    }

    private void DisableAllLines()
    {
        bottomArcLine.enabled = false;
        topArcLine.enabled = false;
        leftEdgeLine.enabled = false;
        rightEdgeLine.enabled = false;
        leftBottomRayLine.enabled = false;
        rightBottomRayLine.enabled = false;
        leftTopRayLine.enabled = false;
        rightTopRayLine.enabled = false;
        centerSpineLine.enabled = false;
    }

    private void SetOptionalSegment(LineRenderer lr, bool shouldDraw, Vector3 a, Vector3 b)
    {
        if (!shouldDraw)
        {
            lr.enabled = false;
            return;
        }

        SetTwoPointLine(lr, a, b);
    }

    private void SetOptionalPolyline(LineRenderer lr, bool shouldDraw, List<Vector3> points)
    {
        if (!shouldDraw || points == null || points.Count < 2)
        {
            lr.enabled = false;
            return;
        }

        SetPolyline(lr, points);
    }

    private void SetTwoPointLine(LineRenderer lr, Vector3 a, Vector3 b)
    {
        lr.enabled = true;
        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
    }

    private void SetPolyline(LineRenderer lr, List<Vector3> points)
    {
        if (points == null || points.Count < 2)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;
        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }

    private void UpdateEdgeLines(List<Vector3> bottomLocal, List<Vector3> topLocal)
    {
        if (bottomLocal == null || topLocal == null || bottomLocal.Count < 2 || topLocal.Count < 2)
        {
            DisableAllLines();
            return;
        }

        int last = bottomLocal.Count - 1;

        Vector3 bottomApex = Vector3.zero;
        Vector3 topApex = Vector3.up * viewHeightOffset;

        SetOptionalPolyline(bottomArcLine, drawBottomArc, bottomLocal);
        SetOptionalPolyline(topArcLine, drawTopArc, topLocal);

        SetOptionalSegment(leftEdgeLine, drawLeftEdge, bottomLocal[0], topLocal[0]);
        SetOptionalSegment(rightEdgeLine, drawRightEdge, bottomLocal[last], topLocal[last]);

        SetOptionalSegment(leftBottomRayLine, drawLeftBottomRay, bottomApex, bottomLocal[0]);
        SetOptionalSegment(rightBottomRayLine, drawRightBottomRay, bottomApex, bottomLocal[last]);

        SetOptionalSegment(leftTopRayLine, drawLeftTopRay, topApex, topLocal[0]);
        SetOptionalSegment(rightTopRayLine, drawRightTopRay, topApex, topLocal[last]);

        SetOptionalSegment(centerSpineLine, drawCenterLine, bottomApex, topApex);
    }

    public void SetLinesColor(Color color)
    {
        ApplyLineColor(bottomArcLine, color);
        ApplyLineColor(topArcLine, color);
        ApplyLineColor(leftEdgeLine, color);
        ApplyLineColor(rightEdgeLine, color);
        ApplyLineColor(leftBottomRayLine, color);
        ApplyLineColor(rightBottomRayLine, color);
        ApplyLineColor(leftTopRayLine, color);
        ApplyLineColor(rightTopRayLine, color);
        ApplyLineColor(centerSpineLine, color);
    }

    private void ApplyLineColor(LineRenderer lr, Color color)
    {
        if (lr == null) return;

        Material mat = lr.material;
        if (mat == null) return;

        if (mat.HasProperty("_Color"))
            mat.SetColor("_Color", color);

        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);

        if (mat.HasProperty("_EmissionColor"))
            mat.SetColor("_EmissionColor", color);
    }

}
