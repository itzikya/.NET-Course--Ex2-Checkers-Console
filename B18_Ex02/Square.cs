using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public class Square
     {
          public struct SquarePosition
          {
               private int m_SquareLine;
               private int m_SquareColumm;

               public SquarePosition(int i_squareLine, int i_squareColumn)
               {
                    m_SquareLine = i_squareLine;
                    m_SquareColumm = i_squareColumn;
               }

               public int x
               {
                    get { return m_SquareColumm; }
                    set { m_SquareColumm = value; }
               }

               public int y
               {
                    get { return m_SquareLine; }
                    set { m_SquareLine = value; }
               }
          }

          public class SquareNeighbours
          {
               private Square m_UpRightSquare;
               private Square m_UpLeftSquare;
               private Square m_DownRightSquare;
               private Square m_DownLeftSquare;

               public Square UpRight
               {
                    get { return m_UpRightSquare; }
                    set { m_UpRightSquare = value; }
               }

               public Square UpLeft
               {
                    get { return m_UpLeftSquare; }
                    set { m_UpLeftSquare = value; }
               }

               public Square DownRight
               {
                    get { return m_DownRightSquare; }
                    set { m_DownRightSquare = value; }
               }

               public Square DownLeft
               {
                    get { return m_DownLeftSquare; }
                    set { m_DownLeftSquare = value; }
               }
          }

          private SquarePosition m_SquarePosition;
          private Man m_CurrentMan;
          private SquareNeighbours m_SquareNeighbours = new SquareNeighbours();
          private eSquareColor m_SquareColor;

          public SquarePosition Position
          {
               get { return m_SquarePosition; }
               set { m_SquarePosition = value; }
          }

          public eSquareColor SquareColor
          {
               get { return m_SquareColor; }
               set { m_SquareColor = value; }
          }

          public Man CurrentMan
          {
               get { return m_CurrentMan; }
               set { m_CurrentMan = value; }
          }

          public SquareNeighbours Neighbours
          {
               get { return m_SquareNeighbours; }
               set { m_SquareNeighbours = value; }
          }

          public Square()
          {
          }

          public Square(int i_squareLine, int i_squareColumm)
          {
               m_SquarePosition = new SquarePosition(i_squareLine, i_squareColumm);
               if ((m_SquarePosition.x + m_SquarePosition.y) % 2 == 0)
               {
                    m_SquareColor = eSquareColor.White;
               }
               else
               {
                    m_SquareColor = eSquareColor.Black;
               }
          }

          public void AssignNeighbour(Square i_neighbourSquare, CheckersGame.ePossibleDirections i_squareDirection)
          {
               if (i_squareDirection == CheckersGame.ePossibleDirections.UpLeft)
               {
                    m_SquareNeighbours.UpLeft = new Square();
                    m_SquareNeighbours.UpLeft = i_neighbourSquare;
               }
               else if (i_squareDirection == CheckersGame.ePossibleDirections.UpRight)
               {
                    m_SquareNeighbours.UpRight = new Square();
                    m_SquareNeighbours.UpRight = i_neighbourSquare;
               }
               else if (i_squareDirection == CheckersGame.ePossibleDirections.DownLeft)
               {
                    m_SquareNeighbours.DownLeft = new Square();
                    m_SquareNeighbours.DownLeft = i_neighbourSquare;
               }
               else
               {
                    m_SquareNeighbours.DownRight = new Square();
                    m_SquareNeighbours.DownRight = i_neighbourSquare;
               }
          }

          public enum eSquareColor
          {
               Black, White
          }
     }
}
