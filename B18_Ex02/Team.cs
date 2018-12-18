using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Team
     {
          private const int k_ManRank = 1;
          private const int k_KingRank = 4;
          private string m_TeamName;
          private eTeamType m_TeamType;
          private eTeamSign m_TeamSign;
          private eDirectionOfMovement m_TeamDirectionOfMovement;
          private int m_TeamScore;
          private Move m_LastMoveExecuted;
          private bool m_IsLeadingTeam;
          private LinkedList<Man> m_ArmyOfMen = new LinkedList<Man>();
          private List<Move> m_AttackMoves = new List<Move>();
          private List<Move> m_RegularMoves = new List<Move>();

          public string Name
          {
               get { return m_TeamName; }
               set { m_TeamName = value; }
          }

          public Move LastMoveExecuted
          {
               get { return m_LastMoveExecuted; }
               set { m_LastMoveExecuted = value; }
          }

          public eDirectionOfMovement Direction
          {
               get { return m_TeamDirectionOfMovement; }
               set { m_TeamDirectionOfMovement = value; }
          }

          public eTeamSign Sign
          {
               get { return m_TeamSign; }
               set { m_TeamSign = value; }
          }

          public eTeamType Type
          {
               get { return m_TeamType; }
               set { m_TeamType = value; }
          }

          public LinkedList<Man> ArmyOfMen
          {
               get { return m_ArmyOfMen; }
               set { m_ArmyOfMen = value; }
          }

          public List<Move> AttackMoves
          {
               get { return m_AttackMoves; }
               set { m_AttackMoves = value; }
          }

          public List<Move> RegularMoves
          {
               get { return m_RegularMoves; }
               set { m_RegularMoves = value; }
          }

          public int Score
          {
               get { return m_TeamScore; }
               set { m_TeamScore = value; }
          }

          public bool isLeadingTeam
          {
               get { return m_IsLeadingTeam; }
               set { m_IsLeadingTeam = value; }
          }

          public Team()
          {    
          }

          public Team(string i_playerName, eTeamType i_teamType, eDirectionOfMovement i_teamDirection, eTeamSign i_teamSign)
          {
               m_TeamName = i_playerName;
               m_TeamType = i_teamType;
               m_TeamDirectionOfMovement = i_teamDirection;
               m_TeamSign = i_teamSign;
               m_TeamScore = 0;
          }

          public void DisposeMen()
          {
               m_ArmyOfMen.Clear();
          }

          public void AssignManToSquare(Square i_manSquare)
          {
               Man recruitedMan = new Man(this, i_manSquare, m_TeamDirectionOfMovement);
               m_ArmyOfMen.AddLast(recruitedMan);
               i_manSquare.CurrentMan = recruitedMan;
          }

          public void PrepareTeamMovesForNewTurn()
          {
               UpdateAttackMoves();
               UpdateRegularMoves();
          }

          public void CrownTeamKings(int i_relevantLineForCrown)
          {
               foreach (Man man in m_ArmyOfMen)
               {
                    if (man.CurrentPosition.Position.y == i_relevantLineForCrown)
                    {
                         man.Crown();
                    }
               }
          }

          public void UpdateAttackMoves()
          {
               m_AttackMoves.Clear();
               foreach (Man man in m_ArmyOfMen)
               {
                    UpdateAttackMoveForSquare(man.CurrentPosition);
               }
          }

          public void UpdateAttackMoveForSquare(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.CurrentMan.IsKing)
               {
                    UpdateUpAttacks(i_squareToBeUpdated);
                    UpdateDownAttacks(i_squareToBeUpdated);
               }
               else if (i_squareToBeUpdated.CurrentMan.Direction == Team.eDirectionOfMovement.Up)
               {
                    UpdateUpAttacks(i_squareToBeUpdated);
               }
               else
               {
                    UpdateDownAttacks(i_squareToBeUpdated);
               }
          }

          public void UpdateUpAttacks(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.Neighbours.UpLeft != null)
               {
                    Square UpLeftSquare = new Square();
                    UpLeftSquare = i_squareToBeUpdated.Neighbours.UpLeft;
                    if (UpLeftSquare.CurrentMan != null)
                    {
                         if (UpLeftSquare.CurrentMan.Team != i_squareToBeUpdated.CurrentMan.Team)
                         {
                              if (UpLeftSquare.Neighbours.UpLeft != null)
                              {
                                   if (UpLeftSquare.Neighbours.UpLeft.CurrentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, UpLeftSquare, UpLeftSquare.Neighbours.UpLeft, Move.eMoveOption.Attack);
                                        m_AttackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }

               if (i_squareToBeUpdated.Neighbours.UpRight != null)
               {
                    Square upRightSquare = new Square();
                    upRightSquare = i_squareToBeUpdated.Neighbours.UpRight;
                    if (upRightSquare.CurrentMan != null)
                    {
                         if (upRightSquare.CurrentMan.Team != i_squareToBeUpdated.CurrentMan.Team)
                         {
                              if (upRightSquare.Neighbours.UpRight != null)
                              {
                                   if (upRightSquare.Neighbours.UpRight.CurrentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, upRightSquare, upRightSquare.Neighbours.UpRight, Move.eMoveOption.Attack);
                                        m_AttackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }
          }

          public void UpdateDownAttacks(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.Neighbours.DownLeft != null)
               {
                    Square downLeftSquare = new Square();
                    downLeftSquare = i_squareToBeUpdated.Neighbours.DownLeft;
                    if (downLeftSquare.CurrentMan != null)
                    {
                         if (downLeftSquare.CurrentMan.Team != i_squareToBeUpdated.CurrentMan.Team)
                         {
                              if (downLeftSquare.Neighbours.DownLeft != null)
                              {
                                   if (downLeftSquare.Neighbours.DownLeft.CurrentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, downLeftSquare, downLeftSquare.Neighbours.DownLeft, Move.eMoveOption.Attack);
                                        m_AttackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }

               if (i_squareToBeUpdated.Neighbours.DownRight != null)
               {
                    Square downRightSquare = new Square();
                    downRightSquare = i_squareToBeUpdated.Neighbours.DownRight;
                    if (downRightSquare.CurrentMan != null)
                    {
                         if (downRightSquare.CurrentMan.Team != i_squareToBeUpdated.CurrentMan.Team)
                         {
                              if (downRightSquare.Neighbours.DownRight != null)
                              {
                                   if (downRightSquare.Neighbours.DownRight.CurrentMan == null)
                                   {
                                        Move addedMoveToAttackList = new Move(i_squareToBeUpdated, downRightSquare, downRightSquare.Neighbours.DownRight, Move.eMoveOption.Attack);
                                        m_AttackMoves.Add(addedMoveToAttackList);
                                   }
                              }
                         }
                    }
               }
          }

          public void UpdateRegularMoves()
          {
               m_RegularMoves.Clear();
               foreach (Man man in m_ArmyOfMen)
               {
                    UpdateRegularMoveForSquare(man.CurrentPosition);
               }
          }

          public void UpdateRegularMoveForSquare(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.CurrentMan.IsKing)
               {
                    UpdateUpMoves(i_squareToBeUpdated);
                    UpdateDownMoves(i_squareToBeUpdated);
               }
               else if (i_squareToBeUpdated.CurrentMan.Direction == Team.eDirectionOfMovement.Up)
               {
                    UpdateUpMoves(i_squareToBeUpdated);
               }
               else
               {
                    UpdateDownMoves(i_squareToBeUpdated);
               }
          }

          public void UpdateUpMoves(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.Neighbours.UpLeft != null)
               {
                    if (i_squareToBeUpdated.Neighbours.UpLeft.CurrentMan == null)
                    {
                         Square UpLeftSquare = new Square();
                         UpLeftSquare = i_squareToBeUpdated.Neighbours.UpLeft;
                         Move addedMoveToMoveList = new Move(i_squareToBeUpdated, UpLeftSquare);
                         m_RegularMoves.Add(addedMoveToMoveList);
                    }
               }

               if (i_squareToBeUpdated.Neighbours.UpRight != null)
               {
                    if (i_squareToBeUpdated.Neighbours.UpRight.CurrentMan == null)
                    {
                         Square upRightSquare = new Square();
                         upRightSquare = i_squareToBeUpdated.Neighbours.UpRight;
                         Move addedMoveToMoveList = new Move(i_squareToBeUpdated, upRightSquare);
                         m_RegularMoves.Add(addedMoveToMoveList);
                    }
               }
          }

          public void UpdateDownMoves(Square i_squareToBeUpdated)
          {
               if (i_squareToBeUpdated.Neighbours.DownLeft != null)
               {
                    if (i_squareToBeUpdated.Neighbours.DownLeft.CurrentMan == null)
                    {
                         Square downLeftSquare = new Square();
                         downLeftSquare = i_squareToBeUpdated.Neighbours.DownLeft;
                         Move addedMoveToMoveList = new Move(i_squareToBeUpdated, downLeftSquare);
                         m_RegularMoves.Add(addedMoveToMoveList);
                    }
               }

               if (i_squareToBeUpdated.Neighbours.DownRight != null)
               {
                    if (i_squareToBeUpdated.Neighbours.DownRight.CurrentMan == null)
                    {
                         Square downRightSquare = new Square();
                         downRightSquare = i_squareToBeUpdated.Neighbours.DownRight;
                         Move addedMoveToMoveList = new Move(i_squareToBeUpdated, downRightSquare);
                         m_RegularMoves.Add(addedMoveToMoveList);
                    }
               }
          }

          public int CalculateTeamRank()
          {
               int teamRank = 0;
               foreach (Man man in m_ArmyOfMen)
               {
                    if (man.IsKing == true)
                    {
                         teamRank += k_KingRank;
                    }
                    else
                    {
                         teamRank += k_ManRank;
                    }
               }

               return teamRank;
          }

          public void CalculateTeamScore(int i_teamRank, int i_opponentRank)
          {
               m_TeamScore += i_teamRank - i_opponentRank;
          }

          public bool IsTeamCanQuit()
          {
               return m_IsLeadingTeam == false ? true : false;
          }

          public enum eTeamType
          {
               User,
               Computer
          }

          public enum eTeamSign
          {
               X,
               O
          }

          public enum eDirectionOfMovement
          {
               Up,
               Down,
          }
     }
}
