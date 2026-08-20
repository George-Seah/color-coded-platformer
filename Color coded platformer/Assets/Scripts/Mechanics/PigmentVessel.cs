using UnityEngine;

public class PigmentVessel : MonoBehaviour
{
    public GameColor currentPigment = GameColor.None;

    public void AbsorbPigment(BaseColorPicker target)
    {
        if (target.colorState != GameColor.White && target.colorState != GameColor.Default)
        {
            currentPigment = target.colorState;
            target.colorState = GameColor.White;
            target.ApplyColor();
        }
    }

    public void InjectPigment(BaseColorPicker target)
    {
        if (currentPigment != GameColor.None)
        {
            target.colorState = currentPigment;
            target.ApplyColor();
            currentPigment = GameColor.None; 
        }
    }
}
