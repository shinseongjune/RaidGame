using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueSetter : MonoBehaviour
{
    Slider slider;
    public Stats targetStats;
    public Stat.Type targetType;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        switch (targetType)
        {
            case Stat.Type.MaxHP:
                slider.value = targetStats.HP / targetStats[(int)Stat.Type.MaxHP].Current;
                break;
            case Stat.Type.MaxMP:
                slider.value = targetStats.MP / targetStats[(int)Stat.Type.MaxMP].Current;
                break;
            case Stat.Type.Might:
                break;
            case Stat.Type.Agility:
                break;
            case Stat.Type.Dignity:
                break;
            case Stat.Type.Willpower:
                break;
        }
    }
}
