using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Man
     {
          private const char k_UpDirectionKingSign = 'K';
          private const char k_UpDirectionManSign = 'X';
          private const char k_DownDirectionKingSign = 'U';
          private const char k_DownDirectionManSign = 'O';
          private Team m_ManTeam;
          private Square m_CurrentPosition;
          private bool m_IsKing;
          private Team.eDirectionOfMovement m_ManDirection;
          private Team.eTeamSign m_ManSign;
          
          public bool IsKing
          {
               get { return m_IsKing; }
               set { m_IsKing = value; }
          }

          public Team Team
          {
               get { return m_ManTeam; }
               set { m_ManTeam = value; }
          }

          public Team.eDirectionOfMovement Direction
          {
               get { return m_ManDirection; }
               set { m_ManDirection = value; }
          }

          public Square CurrentPosition
          {
               get { return m_CurrentPosition; }
               set { m_CurrentPosition = value; }
          }

          public char Sign
          {
               get
               {
                    char signOutput;
                    if (m_ManSign == Team.eTeamSign.X)
                    {
                         if (m_IsKing == true)
                         {
                              signOutput = k_UpDirectionKingSign;
                         }
                         else
                         {
                              signOutput = k_UpDirectionManSign;
                         }
                    }
                    else
                    {
                         if (m_IsKing == true)
                         {
                              signOutput = k_DownDirectionKingSign;
                         }
                         else
                         {
                              signOutput = k_DownDirectionManSign;
                         }
                    }

                    return signOutput;
               }
          }

          public Man(Team i_manTeam, Square i_manPosition, Team.eDirectionOfMovement i_manDirection)
          {
               m_ManTeam = i_manTeam;
               m_CurrentPosition = i_manPosition;
               m_ManDirection = i_manDirection;
               m_ManSign = i_manTeam.Sign;
               m_IsKing = false;
          }

          public void Crown()
          {
               m_IsKing = true;
          }
     }
}
