using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CNoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(CNoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case CNoiseSettings.FilterType.Simple:
                return new CSimpleNoiseFilter(settings.stdNoiseSettings);
            case CNoiseSettings.FilterType.Ridgid:
                return new CRidgidNoiseFilter(settings.ridgidNoiseSettings);
            case CNoiseSettings.FilterType.Hilly:
                return new CHillyNoiseFilter(settings.stdNoiseSettings);
            case CNoiseSettings.FilterType.Brain:
                return new CBrainNoiseFilter(settings.stdNoiseSettings);
            default:
                break;
        }
        return null;
    }
}
