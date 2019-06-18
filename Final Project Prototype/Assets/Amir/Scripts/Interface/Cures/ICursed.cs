public interface ICursed
{
    #region Properties
    bool IsOnceOnly { get; set; }
    bool IsStartCurse { get; set; }
    int Weight { get; set; }
    #endregion Properties

    #region Methods
    void StartCurse();
    #endregion Methods
}