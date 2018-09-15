using UnityEngine;

public enum ProjectionEnumerator
{

    NoProjection,
    OrthographicProjection,
    PerspectiveProjection

}

public class CameraTransformation : Transformation
{
    [SerializeField] private float focalLength = 1f;

    public ProjectionEnumerator projectionDropdown;

    public override Matrix4x4 Matrix
    {
        get
        {
            switch (projectionDropdown)
            {
                case ProjectionEnumerator.NoProjection:
                {
                    Matrix4x4 matrix = new Matrix4x4();
                    matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
                    matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
                    matrix.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
                    matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                    return matrix;
                }
                case ProjectionEnumerator.OrthographicProjection:
                {
                    Matrix4x4 matrix = new Matrix4x4();
                    matrix.SetRow(0, new Vector4(focalLength, 0f, 0f, 0f));
                    matrix.SetRow(1, new Vector4(0f, focalLength, 0f, 0f));
                    matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
                    matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                    return matrix;
                }
                case ProjectionEnumerator.PerspectiveProjection:
                {
                    Matrix4x4 matrix = new Matrix4x4();
                    matrix.SetRow(0, new Vector4(focalLength, 0f, 0f, 0f));
                    matrix.SetRow(1, new Vector4(0f, focalLength, 0f, 0f));
                    matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
                    matrix.SetRow(3, new Vector4(0f, 0f, 1f, 0f));
                    return matrix;
                }
                default:
                {
                    Matrix4x4 matrix = new Matrix4x4();
                    matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
                    matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
                    matrix.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
                    matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                    return matrix;
                }

            }
            //if (isOrthographic)
            //{
            //    Matrix4x4 matrix = new Matrix4x4();
            //    matrix.SetRow(0, new Vector4(focalLength, 0f, 0f, 0f));
            //    matrix.SetRow(1, new Vector4(0f, focalLength, 0f, 0f));
            //    matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
            //    matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            //    return matrix;
            //}
            //else 
            //{
            //    Matrix4x4 matrix = new Matrix4x4();
            //    matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
            //    matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
            //    matrix.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
            //    matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            //    return matrix;
            //}
        }
    }
}