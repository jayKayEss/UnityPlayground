using System.Collections;
using UnityEngine;

public class StackLayout : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z,
    }

    public enum Alignment
    {
        Leading,
        Center,
        Trailing,
    }

    public Axis axis;
    public Alignment alignment = Alignment.Center;
    public float cellSize = 1f;

    private void Reset()
    {
        LayoutChildren();
    }

    void OnValidate()
    {
        LayoutChildren();
    }

    void LayoutChildren()
    {
        StopAllCoroutines();
        StartCoroutine(LayoutChildrenRoutine());
    }

    IEnumerator LayoutChildrenRoutine()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = ChildPosition(i);
            yield return null;
        }
    }

    private Vector3 ChildPosition(int i)
    {
        return axis switch
        {
            Axis.X => new(ChildOffset(i), 0f, 0f),
            Axis.Y => new(0f, ChildOffset(i), 0f),
            Axis.Z => new(0f, 0f, ChildOffset(i)),
            _ => Vector3.zero,
        };
    }

    private float ChildOffset(int i)
    {
        return alignment switch
        {
            Alignment.Leading => i * cellSize,
            Alignment.Center => (float)(transform.childCount - 1) / 2f * cellSize - i * cellSize,
            Alignment.Trailing => -i * cellSize,
            _ => 0f,
        };
    }
}
