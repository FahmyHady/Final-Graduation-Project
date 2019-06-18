using UnityEngine;

public class RandomPlaceObj : MonoBehaviour
{
    #region Fields
    [SerializeField] private int amount;
    private RandomPlaceIndecator place;
    #endregion Fields

    #region Properties
    public int Amount { get => amount; set => amount = value; }
    public RandomPlaceIndecator Place { get => place; set => CheckPlace(value); }
    #endregion Properties

    #region Methods
    private void CheckPlace(RandomPlaceIndecator randomPlace)
    {
        if (place != null)
        {
            place.IsFree = true;
            place.REvent.Raise();
            place = null;
        }
        if (randomPlace != null)
        {
            place = randomPlace;
            place.IsFree = false;
        }
    }
    #endregion Methods
}