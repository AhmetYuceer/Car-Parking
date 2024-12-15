using System;
using System.Collections.Generic;
using UnityEngine;
using Interface;
using Enums;

public class LightController : MonoBehaviour , IMoveable
{
    [SerializeField] private Material _redMaterial , _greenMaterial;
    [SerializeField] private Light _redPointLight, _greenPointLight;
    [SerializeField] private MeshRenderer _lightRenderer;
    
    [SerializeField] private List<LightModData> _lights = new List<LightModData>();
    
    [SerializeField] private Direction _carDirection;
    [SerializeField] private List<CarController> _cars = new List<CarController>();
    
    private LightMod _currentLightMod;

    private void Start()
    {
        _currentLightMod = LightMod.Red;
        _redPointLight.gameObject.SetActive(true);
        _greenPointLight.gameObject.SetActive(false);
    }

    public void Move(Direction direction)
    {
        foreach (var lightMod in _lights)
        {
            if (lightMod.Direction == direction)
            {
                SwitchLight(lightMod.LightMod);
                break;
            }
        }
    }

    private void SwitchLight(LightMod lightMod)
    {
        if (lightMod == _currentLightMod)
            return;

        switch (lightMod)
        {
            case LightMod.Red:
                _redPointLight.gameObject.SetActive(true);
                _greenPointLight.gameObject.SetActive(false);
                _lightRenderer.material = _redMaterial;
                break;
            case LightMod.Green:
                _redPointLight.gameObject.SetActive(false);
                _greenPointLight.gameObject.SetActive(true);
                _lightRenderer.material = _greenMaterial;
                break;
        }
        _currentLightMod = lightMod;

        if (_currentLightMod == LightMod.Green)
        {
            foreach (var car in _cars)
                car.Move(_carDirection);
        }
        
        GameManager.Instance.Move(1);
    }
}

[Serializable]
public class LightModData
{
    public Direction Direction;
    public LightMod LightMod;
}
