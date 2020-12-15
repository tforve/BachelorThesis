using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CNoiseFilterFactory
{
    public static CINoiseFilter CreateNoiseFilter(CNoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case CNoiseSettings.FilterType.Simple:
                return new CSimpleNoiseFilter(settings.stdNoiseSettings);
            case CNoiseSettings.FilterType.Ridgid:
                return new CRidgidNoiseFilter(settings.ridgidNoiseSettings);
            default:
                break;
        }
        return null;
    }
}
