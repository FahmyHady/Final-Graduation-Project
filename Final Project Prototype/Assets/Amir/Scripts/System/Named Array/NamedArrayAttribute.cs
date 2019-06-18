using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class NamedArrayAttribute : PropertyAttribute
{
    #region Fields
    public string[] names;
    #endregion Fields

    #region Constructors
    public NamedArrayAttribute(string[] names) { this.names = names; }
    #endregion Constructors
}