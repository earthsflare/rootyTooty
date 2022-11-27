using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldFormat : MonoBehaviour
{
    [SerializeField] protected FieldFormat above = null;
    [SerializeField] protected FieldFormat below = null;

    public abstract void FormatFields();
    public abstract float GetBottom();
}
