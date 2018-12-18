using System;
using System.Collections.Generic;
using System.Linq;

namespace B18_Ex02
{
     public class CheckersGame
     {
          private Team m_Player1;
          private Team m_Player2;
          private Board m_GameBoard;
          private Team m_ActiveTeam = new Team();
          private Team m_InactiveTeam = new Team();
          private eGameMode m_GameMode;
          private eGameStatus m_GameStatus;

          public Team ActiveTeam
          {
               get { return m_ActiveTeam; }
               set { m_ActiveTeam = value; }
          }

          public Team InactiveTeam
          {
               get { return m_InactiveTeam; }
               set { m_InactiveTeam = value; }
          }

          public Board Board
          {
               get { return m_GameBoard; }

               set { m_GameBoard = value; }
          }

          public eGameMode Mode
          {
               get { return m_GameMode; }
               set { m_GameMode = value; }
          }

          public eGameStatus Status
          {
               get { return m_GameStatus; }
               set { m_GameStatus = value; }
          }

          public CheckersGame(string i_player1Name, string i_player2Name, int i_gameBoardSize, eGameMode i_gameMode)
          {
               m_GameStatus = eGameStatus.ActiveGame;
               m_GameMode = i_gameMode;
               m_GameBoard = new Board(i_gameBoardSize);
               InitializeTeams(i_player1Name, i_player2Name, i_gameMode);
          }

          public void InitializeTeams(string i_player1Name, string i_player2Name, eGameMode i_gameMode)
          {
               m_Player1 = new Team(i_player1Name, Team.eTeamType.User, Team.eDirectionOfMovement.Up, Team.eTeamSign.X);
               if (i_gameMode == eGameMode.VersusAnotherPlayer)
               {
                    m_Player2 = new Team(i_player2Name, Team.eTeamType.User, Team.eDirectionOfMovement.Down, Team.eTeamSign.O);
               }
               else
               {
                    m_Player2 = new Team(i_player2Name, Team.eTeamType.Computer, Team.eDirectionOfMovement.Down, Team.eTeamSign.O);
               }
          }

          public void AssignMenToTeams()
          {
               for (int i = 0; i < (m_GameBoard.BoardSize / 2) - 1; i++)
               {
                    for (int j = 0; j < m_GameBoard.BoardSize; j++)
                    {
                         if (m_GameBoard.GetSquare(i, j).SquareColor == Square.eSquareColor.Black)
                         {
                              m_Player2.AssignManToSquare(m_GameBoard.GetSquare(i, j));
                         }
                    }
               }

               for (int i = (m_GameBoard.BoardSize / 2) + 1; i < m_GameBoard.BoardSize; i++)
               {
                    for (int j = 0; j < m_GameBoard.BoardSize; j++)
                    {
                         if (m_GameBoard.GetSquare(i, j).SquareColor == Square.eSquareColor.Black)
                         {
                              m_Player1.AssignManToSquare(m_GameBoard.GetSquare(i, j));
                         }
                    }
               }
          }

          public void CreateNewRound()
          {
               m_GameBoard.ClearBoard();
               m_GameStatus = eGameStatus.InRound;
               m_ActiveTeam = m_Player1;
               m_InactiveTeam = m_Player2;
               RestoreTeams();
          }

          public void RestoreTeams()
          {
               m_ActiveTeam.isLeadingTeam = true;
               m_InactiveTeam.isLeadingTeam = true;
               m_ActiveTeam.LastMoveExecuted = null;
               m_InactiveTeam.LastMoveExecuted = null;
               m_ActiveTeam.DisposeMen();
               m_InactiveTeam.DisposeMen();
               AssignMenToTeams();
               UpdateMovesInTeams();
          }

          public void UpdateMovesInTeams()
          {
               m_ActiveTeam.PrepareTeamMovesForNewTurn();
               m_InactiveTeam.PrepareTeamMovesForNewTurn();
          }

          public void CrownNewKings()
          {
               int relevantLineForCrown;
               if (m_ActiveTeam.Direction == Team.eDirectionOfMovement.Up)
               {
                    relevantLineForCrown = 0;
               }
               else
               {
                    relevantLineForCrown = m_GameBoard.BoardSize - 1;
               }

               m_ActiveTeam.CrownTeamKings(relevantLineForCrown);
          }

          public void MakeAMoveProcess(Move i_executingMove)
          {
               if (i_executingMove.MoveOption == Move.eMoveOption.Quit)
               {
                    m_GameStatus = eGameStatus.RoundEnd;
               }
               else
               {
                    i_executingMove.ExecuteMove();
                    m_ActiveTeam.LastMoveExecuted = i_executingMove;
                    CrownNewKings();
                    UpdateLeaderTeam();
                    UpdateMovesInTeams();
               }

               if (IsEndOfRound())
               {
                    CalculateScore();
               }
          }

          public void UpdateLeaderTeam()
          {
               if (m_ActiveTeam.CalculateTeamRank() > m_InactiveTeam.CalculateTeamRank())
               {
                    m_ActiveTeam.isLeadingTeam = true;
                    m_InactiveTeam.isLeadingTeam = false;
               }
               else if (m_ActiveTeam.CalculateTeamRank() == m_InactiveTeam.CalculateTeamRank())
               {
                    m_ActiveTeam.isLeadingTeam = true;
                    m_InactiveTeam.isLeadingTeam = true;
               }
               else
               {
                    m_ActiveTeam.isLeadingTeam = false;
                    m_InactiveTeam.isLeadingTeam = true;
               }
          }

          public void CalculateScore()
          {
               if (m_ActiveTeam.isLeadingTeam == true && m_InactiveTeam.isLeadingTeam == true)
               {
                    m_ActiveTeam.CalculateTeamScore(m_ActiveTeam.CalculateTeamRank(), m_InactiveTeam.CalculateTeamRank());
                    m_InactiveTeam.CalculateTeamScore(m_InactiveTeam.CalculateTeamRank(), m_ActiveTeam.CalculateTeamRank());
               }
               else if (m_ActiveTeam.isLeadingTeam == true)
               {
                    m_ActiveTeam.CalculateTeamScore(m_ActiveTeam.CalculateTeamRank(), m_InactiveTeam.CalculateTeamRank());
               }
               else
               {
                    m_InactiveTeam.CalculateTeamScore(m_InactiveTeam.CalculateTeamRank(), m_ActiveTeam.CalculateTeamRank());
               }
          }

          public void SwapActiveTeam()
          {
               if (m_ActiveTeam == m_Player1)
               {
                    m_ActiveTeam = m_Player2;
                    m_InactiveTeam = m_Player1;
               }
               else
               {
                    m_ActiveTeam = m_Player1;
                    m_InactiveTeam = m_Player2;
               }
          }

          public bool IsProgressiveMoveAvailable(Move i_requestedMove)
          {
               bool isProgressiveMoveAvailable = false;
               if (i_requestedMove.MoveOption == Move.eMoveOption.Attack)
               {
                    foreach (Move activeMove in i_requestedMove.DestinationSquare.CurrentMan.Team.AttackMoves)
                    {
                         if (i_requestedMove.DestinationSquare.Position.x == activeMove.SourceSquare.Position.x &&
                              i_requestedMove.DestinationSquare.Position.y == activeMove.SourceSquare.Position.y)
                         {
                              isProgressiveMoveAvailable = true;
                         }
                    }
               }

               return isProgressiveMoveAvailable;
          }

          public Move GenerateMoveRequest()
          {
               Random randomMove = new Random();
               Move generatedMove = new Move();
               if (m_ActiveTeam.AttackMoves.Count > 0)
               {
                    generatedMove = m_ActiveTeam.AttackMoves[randomMove.Next(0, m_ActiveTeam.AttackMoves.Count)];
               }
               else
               {
                    if (m_ActiveTeam.RegularMoves.Count > 0)
                    {
                         generatedMove = m_ActiveTeam.RegularMoves[randomMove.Next(0, m_ActiveTeam.RegularMoves.Count)];
                    }
               }

               return generatedMove;
          }

          public void GenerateProgressiveAttack(ref Move io_executedMove)
          {
               List<Move> relevantMoves = new List<Move>();
               foreach (Move move in io_executedMove.DestinationSquare.CurrentMan.Team.AttackMoves)
               {
                    if (move.SourceSquare.Position.x == io_executedMove.DestinationSquare.Position.x &&
                         move.SourceSquare.Position.y == io_executedMove.DestinationSquare.Position.y)
                    {
                         relevantMoves.Add(move);
                    }
               }

               Random randomMove = new Random();
               io_executedMove = relevantMoves[randomMove.Next(0, relevantMoves.Count)];
          }

          public bool IsEndOfRound()
          {
               bool isEndOfRound = false;
               if (m_ActiveTeam.AttackMoves.Count + m_ActiveTeam.RegularMoves.Count == 0
                    && m_InactiveTeam.AttackMoves.Count + m_InactiveTeam.RegularMoves.Count == 0)
               {
                    isEndOfRound = true;
                    m_GameStatus = eGameStatus.RoundEndWithDraw;
               }
               else if (m_ActiveTeam.AttackMoves.Count + m_ActiveTeam.RegularMoves.Count == 0
                    || m_InactiveTeam.AttackMoves.Count + m_InactiveTeam.RegularMoves.Count == 0)
               {
                    isEndOfRound = true;
                    m_GameStatus = eGameStatus.RoundEnd;
               }

               return isEndOfRound;
          }

          public enum eGameStatus
          {
               InRound,
               StartingNewRound = 1,
               GameEnd = 2,
               ActiveGame = 3,
               RoundEnd,
               RoundEndWithDraw
          }

          public enum eGameMode
          {
               VersusAnotherPlayer = 1,
               VersusComputer = 2
          }

          public enum ePossibleDirections
          {
               UpLeft,
               UpRight,
               DownLeft,
               DownRight
          }
     }
}
