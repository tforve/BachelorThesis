using UnityEngine;

/*  Update scale of mash if radius is changed
 *  maybe need to recalculate everything on the fly? 
 */

[ExecuteInEditMode]
public class PlanetRender : MonoBehaviour
{
    public float updatedRadius;             // get value of PlanetBody.radius. Used to change mesh scale    
    bool settingsChanged;                   // check if settings in Inspector changed.

    void Update()
    {
        if(settingsChanged)
        {
            updatedRadius = GetComponentInParent<PlanetBody>().radius;
            this.transform.localScale = new Vector3(updatedRadius, updatedRadius, updatedRadius);
        }
    }

    void OnValidate()
    {
        settingsChanged = true;
    }


}
