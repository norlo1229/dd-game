using Assets.src.actors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public List<Character> CharacterTurnList;
    public int Turn;
    public int SelectedCharacterIndex;
    public TurnState TurnState;
    public Character selectedCharacter;

    public void Awake()
    {
        CharacterTurnList = new List<Character>();
        Turn = 1;
        SelectedCharacterIndex = 0;
        TurnState = TurnState.Initialize;
    }

    private void InitializeTurns()
    {
        CharacterTurnList.Clear();

        var characters = GameObject.FindObjectsOfType<Character>().ToList();

        Debug.Log($"Found {characters.Count} characters");

        foreach(Character c in characters)
        {
            c.Initiative = Dice.Roll(Constants.DiceSides.Twenty, 1);
            Debug.Log($"{c.CharacterData.Name} rolled a {c.Initiative} initative");
            MovementUtility.OccupyTile(c, c.Position);
        }

        CharacterTurnList = characters.OrderByDescending(c => c.Initiative).ToList();

        TurnState = TurnState.SelectActiveCharacter;
    }

    public void Update()
    {
        switch (TurnState)
        {
            case TurnState.Initialize:
                InitializeTurns();
                break;
            case TurnState.SelectActiveCharacter:
                GetNextCharacter();
                break;
            case TurnState.PerformActions:
                UpdateActions();
                break;
            case TurnState.EndCharacterTurn:
                EndCharacterTurn();
                break;
        }
    }

    private void UpdateActions()
    {
        if (selectedCharacter.GetComponent<AIControllerBase>())
        {
            selectedCharacter.GetComponent<AIControllerBase>().Execute();
        }
    }

    private void GetNextCharacter()
    {
        if(CharacterTurnList.Count == 0)
        {
            return;
        }

        selectedCharacter = CharacterTurnList[SelectedCharacterIndex];
        selectedCharacter.AvailableMovement = selectedCharacter.CharacterData.MovementSpeed;
        selectedCharacter.EndTurn += SelectedCharacter_EndTurn;

        Debug.Log($"It is now {selectedCharacter.CharacterData.Name}'s turn");

        var aiController = selectedCharacter.gameObject.GetComponent<AIControllerBase>();

        if (aiController != null)
        {
            Debug.Log($"{selectedCharacter.CharacterData.Name} is thinking...");
        }
        else
        {
            ActorController.SetTarget(selectedCharacter);
        }

        TurnState = TurnState.PerformActions;
    }

    private void SelectedCharacter_EndTurn(object sender, System.EventArgs e)
    {
        TurnState = TurnState.EndCharacterTurn;
    }

    private void EndCharacterTurn()
    {
        ActorController.SetTarget(null);
        Debug.Log($"{selectedCharacter.CharacterData.Name} ends its turn");
        TurnState = TurnState.EndCharacterTurn;
        selectedCharacter.EndTurn -= SelectedCharacter_EndTurn;
        SelectedCharacterIndex++;

        if(SelectedCharacterIndex > CharacterTurnList.Count - 1)
        {
            Turn++;
            SelectedCharacterIndex = 0;
        }

        TurnState = TurnState.SelectActiveCharacter;
    }
}

public enum TurnState
{
    Initialize,
    SelectActiveCharacter,
    PerformActions,
    EndCharacterTurn
}