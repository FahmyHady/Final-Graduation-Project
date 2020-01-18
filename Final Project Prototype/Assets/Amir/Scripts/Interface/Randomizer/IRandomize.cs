using UnityEngine;

public interface IRandomize
{
    #region Properties
    bool HasManyUses { get; set; }
    #endregion Properties

    #region Methods
    void Assign(ref GameObject @object);
    #endregion Methods
}