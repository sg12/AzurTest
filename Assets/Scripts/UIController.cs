using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text labelN;
    [SerializeField] private Slider sliderN;
    [Space]
    [SerializeField] private Text labelV;
    [SerializeField] private Slider sliderV;
    [Space]
    [SerializeField] private Text labelT;
    [SerializeField] private Slider sliderT;
    [Space]
    [SerializeField] private Text labelCamera;
    [Space]
    [SerializeField] private AnimalFactory animalFactory;

    public static bool IsRotatingByDragOnScreen = true;

    public void SetScaleFloor()
    {
        animalFactory.SetScaleFloor((int)sliderN.value);
        labelN.text = ($"N={sliderN.value}");
    }

    public void SetSpeed()
    {
        animalFactory.SetSpeedValue(sliderV.value);
        labelV.text = ($"V={sliderV.value}");
    }

    public void SetTimeScale()
    {
        animalFactory.SetTimeScale((int)sliderT.value);
        labelT.text = ($"T={sliderT.value}");
    }

    public void SwitchCameraAction()
    {
        IsRotatingByDragOnScreen = !IsRotatingByDragOnScreen;
        if (IsRotatingByDragOnScreen)
            labelCamera.text = "Camera rotate my dragging screen (vertical)";
        else
            labelCamera.text = "Camera zoom my dragging on screen";
    }

    public void SaveGameState()
    {
        animalFactory.SaveFactoryState();
    }

    public void LoadGameState()
    {
        animalFactory.LoadFactoryState();
    }

    public void ResetGameState()
    {
        animalFactory.RestartGame();
    }
}
