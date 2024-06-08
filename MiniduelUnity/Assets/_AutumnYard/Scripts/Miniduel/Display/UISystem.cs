using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UISystem : MonoBehaviour, IBoardEventsListener
    {
        [SerializeField] private TextMeshProUGUI _roundStateLabel;

        [SerializeField] private UIPreparing _preparing;
        [SerializeField] private UIDueling _dueling;
        [SerializeField] private UIFinished _finished;

        [SerializeField] private DisplayBoard _board;
        [SerializeField] private DisplayResults _results;

        #region IBoardEventsListener

        public void OnStartedRound(Round round)
        {
            Debug.Log("OnStartedRound");
            SetState(ERoundState.Preparation);
            EnterPreparingState(round);
        }

        public void OnSettedPiece(Round round, int player, int location, EPiece piece)
        {
            Debug.Log("OnSettedPiece");
            _board.OnSettedPiece(round, player, location, piece);
        }

        public void OnUnsettedPiece(Round round, int player, int location)
        {
            Debug.Log("OnUnsettedPiece");
            _board.OnSettedPiece(round, player, location, EPiece.None);
        }

        public void OnStartedDuel(Round round)
        {
            Debug.Log("OnStartedDuel");
            SetState(ERoundState.Dueling);
            EnterDuelingState(round);
        }

        public void OnPlayedNextFight(Round round, List<FightResult> fightResult)
        {
            Debug.Log("OnPlayedNextFight");
            RefreshDuelingState(round);
        }

        public void OnFinished(Round round)
        {
            Debug.Log("OnFinished");
            SetState(ERoundState.Finished);
            EnterFinishedState(round);
        }

        #endregion // IBoardEventsListener


        private void EnterPreparingState(Round round)
        {
            _results.Set(null);
            _results.Hide();
        }

        private void EnterDuelingState(Round round)
        {
            DisplayResults.DTO dto = new DisplayResults.DTO()
            {
                points1 = "Player1: 0 points",
                points2 = "Player2: 0 points",
                offense = "offense in Player1",
                winner = string.Empty,
            };
            _results.Set(dto);
            _results.Show();
        }

        private void RefreshDuelingState(Round round)
        {
            FightResultDTO fight = round.GetFightResult;
            RoundResult result = RoundOperations.GetRoundResult(fight.fightResults);
            DisplayResults.DTO dto = new DisplayResults.DTO()
            {
                points1 = $"Player1: {result.points1} points",
                points2 = $"Player2: {result.points2} points",
                offense = "Offense in " + (!result.offense ? "Player 1" : "Player 2"),
                winner = string.Empty,
            };
            _results.Set(dto);
        }

        private void EnterFinishedState(Round round)
        {
            RoundFinishedDTO finished = round.GetRoundFinished;
            DisplayResults.DTO dto = new DisplayResults.DTO()
            {
                points1 = $"Player1: {finished.points1} points",
                points2 = $"Player2: {finished.points2} points",
                offense = "Offense in " + (!finished.offense ? "Player 1" : "Player 2"),
                winner = "Winner is" + (finished.winner == 0 ? "Player 1" : "Player 2") + "!!",
            };
            _results.Set(dto);
        }


        private void SetState(ERoundState newState)
        {
            _preparing.gameObject.SetActive(newState == ERoundState.Preparation);
            _dueling.gameObject.SetActive(newState == ERoundState.Dueling);
            _finished.gameObject.SetActive(newState == ERoundState.Finished);
            _roundStateLabel.text = newState.ToString().ToLower();
        }
    }
}
