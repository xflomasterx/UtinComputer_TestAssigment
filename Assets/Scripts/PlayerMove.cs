using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float moveTime = 2f;
    bool isMoving = false;
    bool isDoorOpening = false;
    Vector3 startPos;
    Vector3 targetPos;
    float startTime;
    public void StartMove()
    {
        isMoving = true;
        startPos = transform.position;
        targetPos = new Vector3(Manager.target.position.x,transform.position.y, Manager.target.position.z);
        startTime = Time.time ;
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            this.transform.position = Vector3.Lerp(startPos, targetPos, (Time.time - startTime) / moveTime);
            if ((Time.time - startTime) >= moveTime)
            {
                Manager.gameManager.WinFinal();
                isMoving = false;
            }
            if (!isDoorOpening)
                if (Vector3.Distance(this.transform.position, targetPos) <= 5f+Manager.target.localScale.x)
                {
                    Manager.target.GetComponent<Animator>().Play("Open") ;
                    isDoorOpening = true;
                }
        }
    }
   
}
