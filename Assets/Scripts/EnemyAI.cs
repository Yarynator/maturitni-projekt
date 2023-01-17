using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    public static EnemyAI Instance { get; private set;}



    private Enemy enemy;
    private State state;

    private float timer;
    private float timerMax;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("EnemyAI");
            return;
        }
        Instance = this;

        state = State.WaitingForEnemyTurn;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer += Time.deltaTime;
                if (timer >= timerMax)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        state = State.WaitingForEnemyTurn;
                        BattleManager.Instance.NextRound();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    public void SetTurn(Enemy enemy)
    {
        this.enemy = enemy;
        timer = 0;
        timerMax = 1.5f;
        state = State.TakingTurn;
    }

    private void SetStateTakingTurn()
    {
        timer = 0;
        timerMax = 1f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        BaseAction[] baseActionArray = enemy.GetComponents<BaseAction>();

        foreach(BaseAction baseAction in baseActionArray)
        {
            /*Debug.Log(baseAction.GetName());
            Debug.Log(baseAction.GetBestEnemyAIAction());*/
            /*if(!(baseAction.GetActionType() == ActionType.Attack && BattleManager.Instance.CanUseAttack()))
            {
                continue;
            }
            else if(!(baseAction.GetActionType() == ActionType.Magic && BattleManager.Instance.CanUseMagic()))
            {
                continue;
            }
            else if(!(baseAction.GetActionType() == ActionType.Move && BattleManager.Instance.GetStepsRemains() <= 0))
            {
                continue;
            }*/

            switch (baseAction.GetActionType())
            {
                case ActionType.Move:
                    if (BattleManager.Instance.GetStepsRemains() == 0)
                        continue;
                    if (baseAction.GetBestEnemyAIAction().actionValue == 0)
                        continue;
                    break;
                case ActionType.Attack:
                    if (!BattleManager.Instance.CanUseAttack())
                        continue;
                    break;
                case ActionType.Magic:
                    if (!BattleManager.Instance.CanUseMagic())
                        continue;
                    break;
            }

            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null)
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyActionComplete);
            if(bestBaseAction.GetActionType() == ActionType.Attack)
            {
                BattleManager.Instance.UseAttack();
            }
            else if (bestBaseAction.GetActionType() == ActionType.Magic)
            {
                BattleManager.Instance.UseMagic();
            }
            return true;
        }

        return false;
    }

}
