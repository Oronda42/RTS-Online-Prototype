using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    #region Properties

    /// <summary>
    /// Reference to the camera (To be assigned manually)
    /// </summary>
    [SerializeField]
    private Camera camera;

    /// <summary>
    /// Original position when initialized
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// Bottom left bound
    /// </summary>
    public Vector3 BottomLeftBound { get { return bottomLeftBound; } set { bottomLeftBound = value; } }
    [SerializeField]
    private Vector3 topRightBound;

    /// <summary>
    /// Top right bound
    /// </summary>
    public Vector3 TopRightBound { get { return topRightBound; } set { topRightBound = value; } }
    [SerializeField]
    private Vector3 bottomLeftBound;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Temporary
        //Init(Vector3.zero, Vector3.one * -10, Vector3.one * 10);
    }

    // Update is called once per frame
    void Update()
    {
        StuckInBounds();
    }

    #region Implementation

    /// <summary>
    /// Initialize the camera
    /// </summary>
    /// <param name="pBottomLeftBounds"></param>
    /// <param name="pTopRightBounds"></param>
    public void Init(Vector3 pPosition, Vector3 pBottomLeftBounds, Vector3 pTopRightBounds)
    {
        originalPosition = pPosition;
        BottomLeftBound = pBottomLeftBounds;
        TopRightBound = pTopRightBounds;

        camera.transform.position = originalPosition;
    }

    /// <summary>
    /// Stuck the camera in bounds
    /// </summary>
    private void StuckInBounds()
    {
        ////////////////////
        /// x axis
        if ( (camera.transform.position.x - (camera.orthographicSize * camera.aspect)) < bottomLeftBound.x )
            camera.transform.position = new Vector3(
                bottomLeftBound.x + (camera.orthographicSize * camera.aspect),
                camera.transform.position.y, camera.transform.position.z);

        if ( (camera.transform.position.x + (camera.orthographicSize * camera.aspect)) > topRightBound.x)
            camera.transform.position = new Vector3(
                topRightBound.x - (camera.orthographicSize * camera.aspect),
                camera.transform.position.y, camera.transform.position.z);

        ////////////////////
        /// y axis
        if ((camera.transform.position.y - camera.orthographicSize) < bottomLeftBound.y)
            camera.transform.position = new Vector3(
                camera.transform.position.x,
                bottomLeftBound.y + camera.orthographicSize,
                camera.transform.position.z);

        if ((camera.transform.position.y + camera.orthographicSize) > topRightBound.y)
            camera.transform.position = new Vector3(
                camera.transform.position.x,
                topRightBound.y - camera.orthographicSize,
                camera.transform.position.z);

    }
    #endregion
}
