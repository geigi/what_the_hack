using UnityEngine;

namespace World
{
    public class TouchClickController: MonoBehaviour
    {
        //Change me to change the touch phase used.
        TouchPhase touchPhase = TouchPhase.Ended;
        public Camera Camera;

        void Update() {
            //We check if we have more than one touch happening.
            //We also check if the first touches phase is Ended (that the finger was lifted)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase || Input.GetMouseButtonDown(0))
            {
                Vector3 touchClickPos;
                if (Input.GetMouseButtonDown(0))
                {
                    touchClickPos = Input.mousePosition;
                }
                else
                {
                    touchClickPos = Input.GetTouch(0).position;
                }
                //We transform the touch position into word space from screen space and store it.
                var touchPosWorld = Camera.ScreenToWorldPoint(touchClickPos);
 
                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
 
                //We now raycast with this information. If we have hit something we can process it.
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.transform.forward);
 
                if (hitInformation.collider != null) {
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    //touchedObject should be the object someone touched.
                    Debug.Log("Touched " + touchedObject.transform.name);
                }
            }
        }
    }
}