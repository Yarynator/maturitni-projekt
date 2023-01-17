using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, Entity
{

    [SerializeField] private MoveAction moveAction;

    private bool isWalking;
    private bool isWaiting;

    private float timer;

    private List<GridPosition> gridPositionMap;
    private int positionIndex;

    private Vector2 endPosition;

    private void Update()
    {
        if (!isWalking && !isWaiting)
        {
            isWalking = true;

            List<NPCMoveMapSO> npcMoveMapList = SceneInfo.Instance.GetNPCMoveMapList();
            List<NPCMoveMapSO> validNPCMoveLists = new List<NPCMoveMapSO>();
            GridPosition actualPosition = LevelGrid.Instance.GetGridPosition(transform.position);

            foreach(NPCMoveMapSO npcMoveMap in npcMoveMapList)
            {
                if (npcMoveMap.moveMap[0] == actualPosition || npcMoveMap.moveMap[npcMoveMap.moveMap.Count - 1] == actualPosition)
                {
                    validNPCMoveLists.Add(npcMoveMap);
                }
            }

            int randomIndex = Random.Range(0, validNPCMoveLists.Count);

            List<GridPosition> choosedMap = validNPCMoveLists[randomIndex].moveMap;

            gridPositionMap = new List<GridPosition>();
            positionIndex = 0;
            if(choosedMap[0] == actualPosition)
            {
                foreach (GridPosition gridPosition in choosedMap)
                {
                    gridPositionMap.Add(gridPosition);
                }
            }
            else
            {
                for (int i = choosedMap.Count - 1; i >= 0; i--)
                {
                    gridPositionMap.Add(choosedMap[i]);
                }
            }

            GetComponent<Animator>().SetBool("isMoving", true);
            //moveAction.TakeAction(gridPositionMap[positionIndex], NextPosition, true, 1f);
            endPosition = LevelGrid.Instance.GetWorldPosition(gridPositionMap[positionIndex]);
        }

        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                isWaiting = false;
            }
        }

        if (isWalking)
        {

            Vector2 dir = endPosition - new Vector2(transform.position.x, transform.position.y);

            transform.position += new Vector3(dir.x, dir.y).normalized * Time.deltaTime;

            if(dir.x < 0)
            {
                if(transform.localScale.x < 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = -scale.x;
                    transform.localScale = scale;
                }
            }
            else
            {
                if (transform.localScale.x > 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = -scale.x;
                    transform.localScale = scale;
                }
            }

            if (Vector2.Distance(transform.position, endPosition) < .1f)
            {
                NextPosition();
            }
        }
    }

    private void NextPosition()
    {
        positionIndex++;

        if (positionIndex >= gridPositionMap.Count)
        {
            GetComponent<Animator>().SetBool("isMoving", false);
            timer = Random.Range(2f, 8f);
            isWaiting = true;
            isWalking = false;
        }
        else
        {
            endPosition = LevelGrid.Instance.GetWorldPosition(gridPositionMap[positionIndex]);
        }
    }

    public bool canAttack()
    {
        return false;
    }

    public void Damage(int damage, Entity sender)
    {

    }

    public Health GetHealth()
    {
        return null;
    }

    public int GetMaxMoveDistance()
    {
        return 7;
    }

    public bool IsEnemy()
    {
        return false;
    }

    public bool CanBeAttacked()
    {
        return false;
    }
}
