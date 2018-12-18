using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace B18_Ex02
{
     public static class UserInterface
     {
          private const int k_MaximumUserNameSize = 20;
          private const int k_SmallBoardSize = 6;
          private const int k_MediumBoardSize = 8;
          private const int k_BigBoardSize = 10;
          private const string k_QuitRequest = "Q";

          public static void RunGame()
          {
               string player1Name;
               string player2Name;
               int gameBoardSize;
               CheckersGame.eGameMode gameMode;
               RunPreGameDialog(out player1Name, out player2Name, out gameBoardSize, out gameMode);
               CheckersGame game = new CheckersGame(player1Name, player2Name, gameBoardSize, gameMode);
               ManageGame(game);
          }

          public static void ManageGame(CheckersGame game)
          {
               while (game.Status == CheckersGame.eGameStatus.ActiveGame)
               {
                    game.CreateNewRound();
                    while (game.Status == CheckersGame.eGameStatus.InRound)
                    {
                         ManageRound(game);
                    }

                    if (game.Status == CheckersGame.eGameStatus.StartingNewRound)
                    {
                         game.Status = CheckersGame.eGameStatus.ActiveGame;
                    }
               }
          }

          public static void ManageRound(CheckersGame game)
          {
               PrintGameBoard(game.Board);
               PrintMoveInfo(game.ActiveTeam, game.InactiveTeam);
               System.Threading.Thread.Sleep(300);
               game.UpdateMovesInTeams();
               Move requestedMove = new Move();
               if (game.ActiveTeam.Type == Team.eTeamType.User)
               {
                    requestedMove = HandleUserMoveInput(game);
               }
               else
               {
                    requestedMove = game.GenerateMoveRequest();
               }

               if (game.Status == CheckersGame.eGameStatus.InRound)
               {
                    game.MakeAMoveProcess(requestedMove);
                    while (game.IsProgressiveMoveAvailable(requestedMove) && game.Status == CheckersGame.eGameStatus.InRound)
                    {
                         PrintGameBoard(game.Board);
                         PrintMoveInfo(game.ActiveTeam, game.InactiveTeam);
                         System.Threading.Thread.Sleep(100);
                         if (game.Mode == CheckersGame.eGameMode.VersusAnotherPlayer)
                         {
                              requestedMove = HandleUserProgressiveMoveInput(requestedMove, game.Status, game.ActiveTeam);
                         }
                         else
                         {
                              game.GenerateProgressiveAttack(ref requestedMove);
                         }

                         game.MakeAMoveProcess(requestedMove);
                    }
               }

               if (game.IsEndOfRound())
               {
                    System.Threading.Thread.Sleep(100);
                    HandleEndOfRound(game);
               }
               else
               {
                    game.SwapActiveTeam();
               }
          }

          public static void HandleEndOfRound(CheckersGame game)
          {
               Ex02.ConsoleUtils.Screen.Clear();
               CheckersGame.eGameStatus newStatusFromUser;
               if (game.Status == CheckersGame.eGameStatus.RoundEndWithDraw)
               {
                    RunAnotherRoundDialog(game.ActiveTeam, game.InactiveTeam, out newStatusFromUser);
               }
               else
               {
                    RunAnotherRoundDialog(game.ActiveTeam, out newStatusFromUser);
               }

               game.Status = newStatusFromUser;
          }

          public static void RunPreGameDialog(out string o_player1Name, out string o_player2Name, out int o_gameBoardSize, out CheckersGame.eGameMode o_gameMode)
          {
               PrintIntroduction();
               o_player1Name = GetUserName();
               o_gameBoardSize = GetRequestedBoardSize();
               o_gameMode = GetGameModeFromUser();
               if (o_gameMode == CheckersGame.eGameMode.VersusAnotherPlayer)
               {
                    o_player2Name = GetUserName();
               }
               else
               {
                    o_player2Name = "Computer";
               }
          }

          public static void PrintIntroduction()
          {
               Console.WriteLine("Hello, Let's play Checkers!");
          }

          public static void PrintIllegalInputMassage()
          {
               Console.WriteLine("Illegal input inserted. Please try again.");
          }

          public static string GetUserName()
          {
               Console.WriteLine("Please enter your name: ");
               string userInputForName = Console.ReadLine();
               while (!IsLegalUserNameInserted(userInputForName))
               {
                    PrintIllegalInputMassage();
                    userInputForName = Console.ReadLine();
               }

               return userInputForName;
          }

          public static bool IsLegalUserNameInserted(string i_insertedUserName)
          {
               return Regex.IsMatch(i_insertedUserName, "^[a-z,A-Z,0-9,!-/,:-@,[-`, {-~]+$") && i_insertedUserName.Length <= k_MaximumUserNameSize ? true : false;
          }

          public static int GetRequestedBoardSize()
          {
               int userChoiseForBoardSize;
               string boardSizeMassage = string.Format(@"Please enter preferred board size:
(6) 6x6 
(8) 8x8
(10) 10x10");
               Console.WriteLine(boardSizeMassage);
               string userInputForBoardSize = Console.ReadLine();

               while (!int.TryParse(userInputForBoardSize, out userChoiseForBoardSize) || !IsLegalBoardSizeInserted(userChoiseForBoardSize))
               {
                    PrintIllegalInputMassage();
                    userInputForBoardSize = Console.ReadLine();
               }

               return userChoiseForBoardSize;
          }

          public static bool IsLegalBoardSizeInserted(int i_insertedUserChoiseForBoardSize)
          {
               return (i_insertedUserChoiseForBoardSize == k_SmallBoardSize || i_insertedUserChoiseForBoardSize == k_MediumBoardSize || i_insertedUserChoiseForBoardSize == k_BigBoardSize) ? true : false;
          }

          public static CheckersGame.eGameMode GetGameModeFromUser()
          {
               string gameModeMassage = string.Format(@"Please enter prefered game mode:
(1) Versus Another Player
(2) Versus Computer");
               Console.WriteLine(gameModeMassage);
               string userInputForGameMode = Console.ReadLine();
               int userChoiseForGameMode;
               while (!int.TryParse(userInputForGameMode, out userChoiseForGameMode) || !IsLegalGameModeInserted(userChoiseForGameMode))
               {
                    PrintIllegalInputMassage();
                    userInputForGameMode = Console.ReadLine();
               }

               return (CheckersGame.eGameMode)userChoiseForGameMode;
          }

          public static bool IsLegalGameModeInserted(int i_insertedUserChoiseForGameMode)
          {
               return i_insertedUserChoiseForGameMode == 1 || i_insertedUserChoiseForGameMode == 2 ? true : false;
          }

          public static void PrintGameBoard(Board gameBoard)
          {
               Ex02.ConsoleUtils.Screen.Clear();
               for (int i = 0; i < gameBoard.BoardSize; i++)
               {
                    Console.Write("   ");
                    Console.Write((char)('A' + i));
               }

               Console.WriteLine("   ");
               printGameBoardSeperator(gameBoard.BoardSize);

               for (int i = 0; i < gameBoard.BoardSize; i++)
               {
                    Console.Write((char)('a' + i));
                    Console.Write('|');
                    for (int j = 0; j < gameBoard.BoardSize; j++)
                    {
                         Console.Write(" {0} |", gameBoard.GetSquareContent(i, j));
                    }

                    Console.WriteLine(' ');
                    printGameBoardSeperator(gameBoard.BoardSize);
               }
          }

          private static void printGameBoardSeperator(int i_gameBoardSize)
          {
               Console.Write(' ');
               for (int i = 0; i < (i_gameBoardSize * 4) + 2; i++)
               {
                    Console.Write('=');
               }

               Console.WriteLine(' ');
          }

          public static void PrintMoveInfo(Team i_activeTeam, Team i_inactiveTeam)
          {
               if (i_inactiveTeam.LastMoveExecuted != null)
               {
                    Console.WriteLine(string.Format("{0}'s move was ({1}): {2}", i_inactiveTeam.Name, i_inactiveTeam.Sign, i_inactiveTeam.LastMoveExecuted.ToString()));
               }

               Console.WriteLine(string.Format("{0}'s Turn ({1}):", i_activeTeam.Name, i_activeTeam.Sign));
          }

          public static Move HandleUserMoveInput(CheckersGame i_game)
          {
               Move returnedMoveRequest = new Move();
               string userInput = Console.ReadLine();

               while (IsLegalUserMoveInput(userInput, ref returnedMoveRequest, i_game.ActiveTeam) == false)
               {
                    PrintGameBoard(i_game.Board);
                    PrintMoveInfo(i_game.ActiveTeam, i_game.InactiveTeam);
                    PrintIllegalInputMassage();
                    userInput = Console.ReadLine();
               }

               return returnedMoveRequest;
          }

          public static Move HandleUserProgressiveMoveInput(Move i_previousMove, CheckersGame.eGameStatus i_gameStatus, Team i_activeTeam)
          {
               Move returnedMoveRequest = new Move();
               string userInput = Console.ReadLine();

               while (IsLegalUserMoveInput(userInput, ref returnedMoveRequest, i_activeTeam) == false ||
                    i_previousMove.DestinationSquare.Position.x != returnedMoveRequest.SourceSquare.Position.x ||
                    i_previousMove.DestinationSquare.Position.y != returnedMoveRequest.SourceSquare.Position.y)
               {
                    PrintIllegalInputMassage();
                    userInput = Console.ReadLine();
               }

               return returnedMoveRequest;
          }

          public static bool IsLegalUserMoveInput(string i_userInput, ref Move io_moveInput, Team i_activeTeam)
          {
               bool isLegalUserMoveInput = false;

               if (IsLegalQuitRequest(i_userInput))
               {
                    if (io_moveInput.TryParse(i_userInput, ref io_moveInput, i_activeTeam) == true)
                    {
                         isLegalUserMoveInput = true;
                    }
               }
               else if (IsLegalMoveInputFormat(i_userInput))
               {
                    if (io_moveInput.TryParse(i_userInput, ref io_moveInput, i_activeTeam) == true)
                    {
                         isLegalUserMoveInput = true;
                    }
               }

               return isLegalUserMoveInput;
          }

          public static bool IsLegalQuitRequest(string i_userInput)
          {
               return i_userInput == k_QuitRequest ? true : false;
          }

          public static bool IsLegalMoveInputFormat(string i_userInput)
          {
               string legalMovePattern = @"^[A-Z][a-z]>[A-Z][a-z]$";

               return Regex.IsMatch(i_userInput, legalMovePattern);
          }

          public static void RunAnotherRoundDialog(Team i_winningTeam, out CheckersGame.eGameStatus io_gameStatus)
          {
               PrintWinningTeamMassage(i_winningTeam);
               io_gameStatus = GetGameStatusFromUser();
          }

          public static void RunAnotherRoundDialog(Team i_firstTeam, Team i_secondTeam, out CheckersGame.eGameStatus io_gameStatus)
          {
               PrintDrawMassage(i_firstTeam, i_secondTeam);
               io_gameStatus = GetGameStatusFromUser();
          }

          public static void PrintWinningTeamMassage(Team i_winningTeam)
          {
               string winningTeamMassage = string.Format(@"The winner is {0}, with {1} points!", i_winningTeam.Name, i_winningTeam.Score);
               Console.WriteLine(winningTeamMassage);
          }

          public static void PrintDrawMassage(Team i_firstTeam, Team i_secondTeam)
          {
               string drawMassage = string.Format(
                    @"The game ended with draw. {0} with {1} points, {2} with {3} points.",
                    i_firstTeam.Name,
                    i_firstTeam.Score,
                    i_secondTeam.Name,
                    i_secondTeam.Score);
          }

          public static CheckersGame.eGameStatus GetGameStatusFromUser()
          {
               string gameStatusMassage = string.Format(@"What do you want to do now?
(1) Play Another Round!
(2) Exit");
               Console.WriteLine(gameStatusMassage);
               string userInputForGameStatus = Console.ReadLine();
               int userChoiseForGameStatus;
               while (!int.TryParse(userInputForGameStatus, out userChoiseForGameStatus) || !IsLegalStatusInserted(userChoiseForGameStatus))
               {
                    PrintIllegalInputMassage();
                    userInputForGameStatus = Console.ReadLine();
               }

               return (CheckersGame.eGameStatus)userChoiseForGameStatus;
          }

          public static bool IsLegalStatusInserted(int i_insertedUserChoiseForGameStatus)
          {
               return i_insertedUserChoiseForGameStatus == 1 || i_insertedUserChoiseForGameStatus == 2 ? true : false;
          }
     }
}
