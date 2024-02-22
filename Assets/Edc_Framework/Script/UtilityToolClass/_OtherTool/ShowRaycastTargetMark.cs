using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRaycastTargetMark : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Vector3[] boxSize = new Vector3[4];
        foreach (var item in FindObjectsOfType<MaskableGraphic>())
        {
            if (item.raycastTarget)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < 4; i++)
                {
                    item.rectTransform.GetWorldCorners(boxSize);
                    Gizmos.DrawLine(boxSize[i], boxSize[(i + 1) % 4]);
                }
            }
        }    
    }
}
