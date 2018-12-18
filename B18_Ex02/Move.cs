using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Move
     {
          private Square m_SourceSquare;
          private Square m_CapturedSquare;
          private Square m_DestinationSquare;
          private eMoveOption m_MoveOption;

          public eMoveOption MoveOption
          {
               get { return m_MoveOption; }
               set { m_MoveOption = value; }
          }

          public Move()
          {
               m_SourceSquare = new Square();
               m_DestinationSquare = new Square();
          }

          public Move(Square i_sourceSquare, Square i_destinationSquare)
          {
               m_SourceSquare = i_sourceSquare;
               m_DestinationSquare = i_destinationSquare;
               m_MoveOption = eMoveOption.Move;
          }

          public Move(Square i_sourceSquare, Square i_capturedSquare, Square i_destinationSquare, eMoveOption i_moveOption)
          {
               m_SourceSquare = i_sourceSquare;
               m_CapturedSquare = i_capturedSquare;
               m_DestinationSquare = i_destinationSquare;
               m_MoveOption = i_moveOption;
          }

          public Move(eMoveOption i_moveOption)
          {
               m_MoveOption = i_moveOption;
          }

          public Square SourceSquare
          {
               get { return m_SourceSquare; }
               set { m_SourceSquare = value; }
          }

          public Square CapturedSquare
          {
               get { return m_CapturedSquare; }
               set { m_CapturedSquare = value; }
          }

          public Square DestinationSquare
          {
               get { return m_DestinationSquare; }
               set { m_DestinationSquare = value; }
          }

          public new string ToString()
          {
               const string seperatorInMoveString = ">";
               char sourceLengthLetter = (char)(m_SourceSquare.Position.x + (int)'A');
               char sourceWidthLetter = (char)(m_SourceSquare.Position.y + (int)'a');
               char destinationLengthLetter = (char)(m_DestinationSquare.Position.x + (int)'A');
               char destinationWidthLetter = (char)(m_DestinationSquare.Position.y + (int)'a');
               string stringOfMove = string.Format(
                    "{1}{2}{0}{3}{4}",
                    seperatorInMoveString,
                    sourceLengthLetter.ToString(),
                    sourceWidthLetter.ToString(),
                    destinationLengthLetter.ToString(),
                    destinationWidthLetter.ToString());

               return stringOfMove;
          }

          public Move Parse(Move.eMoveOption moveOption)
          {
               Move requestedPlayerMove = new Move(moveOption);

               return requestedPlayerMove;
          }

          public Move Parse(string i_userInput, Team i_activeTeam)
          {
               string[] splittedInput = i_userInput.Split('>');
               Square sourceSquare = new Square((int)splittedInput[0][1] - 'a', (int)splittedInput[0][0] - 'A');
               Square destinationSquare = new Square((int)splittedInput[1][1] - 'a', (int)splittedInput[1][0] - 'A');
               Move requestedPlayerMove = new Move(sourceSquare, destinationSquare);

               foreach (Move attackMove in i_activeTeam.AttackMoves)
               {
                    if (attackMove.SourceSquare.Position.x == sourceSquare.Position.x &&
                         attackMove.SourceSquare.Position.y == sourceSquare.Position.y &&
                         attackMove.DestinationSquare.Position.x == destinationSquare.Position.x &&
                         attackMove.DestinationSquare.Position.y == destinationSquare.Position.y)
                    {
                         requestedPlayerMove = attackMove;
                         requestedPlayerMove.MoveOption = eMoveOption.Attack;
                    }
               }

               foreach (Move regularMove in i_activeTeam.RegularMoves)
               {
                    if (regularMove.SourceSquare.Position.y == sourceSquare.Position.x &&
                         regularMove.SourceSquare.Position.y == sourceSquare.Position.y &&
                         regularMove.DestinationSquare.Position.x == destinationSquare.Position.x &&
                         regularMove.DestinationSquare.Position.y == destinationSquare.Position.y)
                    {
                         requestedPlayerMove = regularMove;
                         requestedPlayerMove.MoveOption = eMoveOption.Move;
                    }
               }

               return requestedPlayerMove;
          }

          public bool TryParse(string i_userInput, ref Move i_requestedMove, Team i_activeTeam)
          {
               bool canConvertInputToMove = false;
               i_requestedMove = i_requestedMove.Parse(i_userInput, i_activeTeam);
               if (i_requestedMove.IsLegalMove(ref i_requestedMove, i_activeTeam))
               {
                    canConvertInputToMove = true;
               }

               return canConvertInputToMove;
          }

          public bool TryParse(Move.eMoveOption moveOption, ref Move i_requestedMove, Team i_activeTeam)
          {
               bool canConvertInputToMove = false;
               if (i_activeTeam.IsTeamCanQuit())
               {
                    i_requestedMove = i_requestedMove.Parse(Move.eMoveOption.Quit);
                    canConvertInputToMove = true;
               }

               return canConvertInputToMove;
          }

          public bool IsLegalMove(ref Move i_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalMove = false;

               isLegalMove = IsAttackMove(i_userRequestForMove, i_activeTeam);
               if (isLegalMove == false && i_activeTeam.AttackMoves.Count == 0)
               {
                    isLegalMove = CheckRegularMoves(ref i_userRequestForMove, i_activeTeam);
               }

               return isLegalMove;
          }

          public bool IsAttackMove(Move io_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalAttackMove = false;
               foreach (Move availableMove in i_activeTeam.AttackMoves)
               {
                    if (IsMoveMatchToMoveInMovesList(ref io_userRequestForMove, availableMove))
                    {
                         isLegalAttackMove = true;
                    }
               }

               return isLegalAttackMove;
          }

          public bool CheckRegularMoves(ref Move io_userRequestForMove, Team i_activeTeam)
          {
               bool isLegalRegularMove = false;
               foreach (Move availableMove in i_activeTeam.RegularMoves)
               {
                    if (IsMoveMatchToMoveInMovesList(ref io_userRequestForMove, availableMove))
                    {
                         isLegalRegularMove = true;
                    }
               }

               return isLegalRegularMove;
          }

          public bool IsMoveMatchToMoveInMovesList(ref Move i_userRequestForMove, Move i_availableMove)
          {
               bool isMoveMatchToMoveInMovesList = false;
               if (i_availableMove.m_DestinationSquare.Position.x == i_userRequestForMove.m_DestinationSquare.Position.x &&
                    i_availableMove.m_DestinationSquare.Position.y == i_userRequestForMove.m_DestinationSquare.Position.y &&
                    i_availableMove.m_SourceSquare.Position.x == i_userRequestForMove.m_SourceSquare.Position.x &&
                   i_availableMove.m_SourceSquare.Position.y == i_userRequestForMove.m_SourceSquare.Position.y)
               {
                    i_userRequestForMove = i_availableMove;
                    isMoveMatchToMoveInMovesList = true;
               }

               return isMoveMatchToMoveInMovesList;
          }

          public void ExecuteMove()
          {
               m_DestinationSquare.CurrentMan = m_SourceSquare.CurrentMan;
               m_SourceSquare.CurrentMan.CurrentPosition = m_DestinationSquare;
               m_SourceSquare.CurrentMan = null;
               if (m_MoveOption == eMoveOption.Attack)
               {
                    m_CapturedSquare.CurrentMan.Team.ArmyOfMen.Remove(m_CapturedSquare.CurrentMan);
                    m_CapturedSquare.CurrentMan = null;
               }
          }

          public enum eMoveOption
          {
               Quit,
               Move,
               Attack,
          }
     }
}
