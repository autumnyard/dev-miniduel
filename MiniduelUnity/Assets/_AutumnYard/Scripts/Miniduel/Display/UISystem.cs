using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public sealed class UISystem : MonoBehaviour, IBoardEventsListener
    {
        [SerializeField] private UIPreparing _preparing;
        [SerializeField] private UIDueling _dueling;
        [SerializeField] private UIFinished _finished;

        [Header("TEST")]
        [SerializeField] private TextMeshProUGUI _roundStateLabel;

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

            var dto = GetUIPreparingDTO(round, player, location);
            _preparing.Set(dto);
        }

        public void OnUnsettedPiece(Round round, int player, int location)
        {
            Debug.Log("OnUnsettedPiece");

            var dto = GetUIPreparingDTO(round, player, location);
            _preparing.Set(dto);
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


        #region States

        private void SetState(ERoundState newState)
        {
            _preparing.gameObject.SetActive(newState == ERoundState.Preparation);
            _dueling.gameObject.SetActive(newState == ERoundState.Dueling);
            _finished.gameObject.SetActive(newState == ERoundState.Finished);
            _roundStateLabel.text = newState.ToString().ToLower();
        }

        private void EnterPreparingState(Round round)
        {
            var dto = GetUIPreparingDTO(round);
            _preparing.Set(dto);
        }

        private void EnterDuelingState(Round round)
        {
            UIDueling.DTO dto = GetUIDuelingBeginDTO(round);
            _dueling.Set(dto);
        }

        private void RefreshDuelingState(Round round)
        {
            UIDueling.DTO dto = GetUIDuelingUpdateDTO(round);
            _dueling.Set(dto);
        }

        private void EnterFinishedState(Round round)
        {
            UIFinished.DTO dto = GetUIFinishedDTO(round);
            _finished.Set(dto);
        }

        #endregion // States


        #region DTOs

        private static UIPreparing.DTO GetUIPreparingDTO(Round round, int player, int location)
        {
            var board = round.GetBoard;
            DisplayBoard.DTO boardDTO = new DisplayBoard.DTO()
            {
                players = board.players,
                locations = board.fights,
                board = board.board,
                newPiecePlayer = player,
                newPieceLocation = location,
            };
            UIPreparing.DTO dto = new UIPreparing.DTO()
            {
                board = boardDTO,
            };
            return dto;
        }

        private static UIPreparing.DTO GetUIPreparingDTO(Round round)
        {
            var board = round.GetBoard;
            DisplayBoard.DTO boardDTO = new DisplayBoard.DTO()
            {
                players = board.players,
                locations = board.fights,
                board = board.board,
            };
            UIPreparing.DTO dto = new UIPreparing.DTO()
            {
                board = boardDTO,
            };
            return dto;
        }

        private static UIDueling.DTO GetUIDuelingBeginDTO(Round round)
        {
            DisplayResults.DTO resultsDTO = new DisplayResults.DTO()
            {
                points1 = 0,
                points2 = 0,
            };
            var board = round.GetBoard;
            DisplayBoard.DTO boardDTO = new DisplayBoard.DTO()
            {
                players = board.players,
                locations = board.fights,
                board = board.board,
            };

            UIDueling.DTO dto = new UIDueling.DTO()
            {
                results = resultsDTO,
                board = boardDTO,
            };
            return dto;
        }

        private static UIDueling.DTO GetUIDuelingUpdateDTO(Round round)
        {
            FightResultDTO fight = round.GetFightResult;
            RoundResult result = RoundOperations.GetRoundResult(fight.fightResults);
            DisplayResults.DTO results = new DisplayResults.DTO()
            {
                points1 = result.points1,
                points2 = result.points2,
                offense = result.offense,
            };

            var board = round.GetBoard;
            DisplayBoard.DTO boardDto = new DisplayBoard.DTO()
            {
                players = board.players,
                locations = board.fights,
                board = board.board,
            };

            UIDueling.DTO dto = new UIDueling.DTO()
            {
                results = results,
                board = boardDto,
            };
            return dto;
        }

        private static UIFinished.DTO GetUIFinishedDTO(Round round)
        {
            RoundFinishedDTO finished = round.GetRoundFinished;
            DisplayResults.DTO resultsDTO = new DisplayResults.DTO()
            {
                points1 = finished.points1,
                points2 = finished.points2,
                offense = finished.offense,
                winner = finished.winner,
                hasFinished = true,
            };

            var board = round.GetBoard;
            DisplayBoard.DTO boardDTO = new DisplayBoard.DTO()
            {
                players = board.players,
                locations = board.fights,
                board = board.board,
            };

            UIFinished.DTO dto = new UIFinished.DTO()
            {
                results = resultsDTO,
                board = boardDTO,
            };
            return dto;
        }

        #endregion // DTOs
    }
}
