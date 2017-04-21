

using System;
using System.Text;

namespace FYP_Chess_Engine {
    class Program {

        public static String[,] Board = new String[8, 8]; 

        public static String g_PlayerColour;
        public static String g_WhoPlays;
        public static String g_WhichColorPlays;
        public static String g_movingPiece;

        //Used for log purposes
        public static int number_of_moves_analysed;

        // User choices
        public static int user_choice;

        // Variable to store temporarily the piece that is moving
        public static String g_tempMovingPiece;
        public static String g_tempMovingPieceKingCheck;

        // Variables to show if king is in check
        public static bool whiteKingCheck;
        public static bool blackKingCheck;

        // Initial coordinates of the two kings
        // (see CheckForWhiteCheck and CheckForBlackCheck functions)
        public static int startingWhiteKingColumn;
        public static int startingWhiteKingRank;
        public static int startingBlKingColumn;
        public static int startingBlKingRank;

        // Variables to show where the kings are in the chessboard
        public static int whiteKingColumn;
        public static int whiteKingRank;
        public static int blKingColumn;
        public static int blKingRank;

        // Variables to help find if a king is under check.
        // (see CheckForWhiteCheck and CheckForBlackCheck functions)
        public static bool dangerFromRight;
        public static bool dangerFromLeft;
        public static bool dangerFromUp;
        public static bool dangerFromDown;
        public static bool dangerFromUpRight;
        public static bool dangerFromDownRight;
        public static bool dangerFromUpLeft;
        public static bool dangerFromDownLeft;

        public static bool dangerForPiece;

        // Chessboards used for the computer thought
        public static String[,] BoardMove0 = new String[8, 8]; // Statement table representing the chessboard
        public static String[,] BoardMoveAfter = new String[8, 8]; // Analysis table representing the board
        public static String[,] BoardThinking = new String[8, 8]; // Thinking table representing the board
        public static String[,] BoardCMCheck = new String[8, 8]; // Checkmate Check table representing the board

        // These arrays will hold the Minimax analysis nodes data
        // Dimension ,1: For the score
        // Dimension ,2: For the parent
        // Dimensions 3-6: For the initial move starting/ finishing columns-ranks (only for the 0-level array)
        public static int[,] NodesAnalysis0 = new int[1000000, 6];
        public static int[,] NodesAnalysis1 = new int[1000000, 2];
        public static int[,] NodesAnalysis2 = new int[1000000, 2];

        //public static int Nodes_Total_count;
        public static int nodeLevel_0_count;
        public static int nodeLevel_1_count;
        public static int nodeLevel_2_count;

        // Is it possible to eat back the piece that was moved by the computer
        public static bool possibilityToEatBack;

        // Variable to show if promotion of a pawn occured
        public static bool promotionOccured = false;

        // Variable to show if castling occured
        public static bool castlingOccured = false;

        // Variables for 'For' loops
        public static int i;
        public static int j;

        // Variable for the correctness of the move
        public static bool g_Correctness;

        // Variables to check the legality of the move
        public static bool g_Legal;
        public static bool exitLegalCheck = false;
        //Loops
        public static int h;
        public static int p;
        public static int howToMoveRank;
        public static int howToMoveColumn;

        // Has the user entered a wrong column?
        public static bool g_WrongColumn;

        // Coordinates of the starting square of the move
        public static String g_StartingColumn;
        public static int g_StartingRank;
        public static String g_FinishingColumn;
        public static int g_FinishingRank;

        //Variables used for move scoring
        public static int valueOfHumanMovingPiece = 0;
        public static int valueOfMovingPiece = 0;

        // Variable for en passant moves
        public static bool enpassantOccured;

        // Coordinates of the square Where the player can perform en passant
        public static int enpassantPossibleTargetRank;
        public static int enpassantPossibleTargetColumn;

        // If player eats a piece, then make the square a preferred target
        public static int humanLastMoveTargetColumn;
        public static int humanLastMoveTargetRow;

        // Column number inserted by the user (used for calculation)
        public static int g_StartingColumnNumber;
        public static int g_FinishingColumnNumber;

        //Variables used for AI
        public static int currentMoveScore;
        public static int bestMoveStartingColumnNumber;
        public static int bestMoveFinishingColumnNumber;
        public static int bestMoveStartingRank;
        public static int bestMoveFinishingRank;
        public static int moveAnalysed;
        public static bool stopAnalyzing;
        public static int thinkingDepth;
        public static int g_StartingColumnNumber_AI;
        public static int g_FinishingColumnNumber_AI;
        public static int g_StartingRank_AI;
        public static int g_FinishingRank_AI;
        public static bool First_Call;
        public static String whoIsAnalysed;
        public static String MovingPiece_AI;

        // For writing the computer move
        public static String AIStartingColumnText;
        public static String AIFinishingColumnText;

        // Move number
        public static int move;

        // Variable to show if a move is found for the AI to do
        public static bool Best_Move_Found;

        // Variables to store the scores of positions during the analysis
        public static int tempScoreMove_0;
        public static int tempScoreMove_1_Human;
        public static int tempScoreMove_2;


        //Creating Multiple boards for danger checks
        //0 = Not dangerous square
        //1 = Dangerous square
        public static int[,] BoardDangerSquares = new int[8, 8];
        public static int[,] numberOfDefenders = new int[8, 8];
        public static int[,] numberOfAttackers = new int[8, 8];
        public static int[,] valueOfDefenders = new int[8, 8];
        public static int[,] valueOfAttackers = new int[8, 8];

        static void Main(string[] args) {

            Console.WriteLine("FYP_Chess_Engine by Joshua Nsubuga | Student Number: N3088835");
            Console.WriteLine("");
        
            bool exit_game = false;
            bool sideChosen = false;

            // Setup startup position
            startingPosition(); // Moved here to allow for displaying of board if the player chooses white, otherwise pieces arent set and a null reference is thrown because the board is empty

            do {
                // Setup game
                Console.WriteLine("Which side would you like to play?");
                Console.WriteLine("Please enter either 'w' or 'b' and press enter...");
                String user_choice = Console.ReadLine();

                /*if(user_choice.CompareTo("w") == 0)
                  {
                      g_PlayerColour = "White";
                      g_WhoPlays = "Human";                      //Swapped for switch case to handle invalid inputs when selecting sides (null reference exception thrown otherwise)
                  } else if(user_choice.CompareTo("b") == 0)
                  {
                      g_PlayerColour = "Black";
                      g_WhoPlays = "AI";
                  }
                */

                switch(user_choice) {

                case "w":
                    g_PlayerColour = "White";
                    g_WhoPlays = "Human";
                    sideChosen = true;
                    displayBoard(Board); // Allows player to see board before making first move (was not shown before)
                    break;
                case "b":
                    g_PlayerColour = "Black";
                    g_WhoPlays = "AI";
                    sideChosen = true;
                    break;
                default:
                    Console.WriteLine("Invalid Choice.");
                    break;

                }
            } while(sideChosen == false);
            
            thinkingDepth = 2;


            do {

                if(g_WhoPlays.CompareTo("AI") == 0) {
                    // Call AI Thought function

                    moveAnalysed = 0;
                    stopAnalyzing = false;
                    First_Call = true;
                    Best_Move_Found = false;
                    whoIsAnalysed = "AI";

                    // CHECK DANGER - Start
                    #region checkDanger
                    // Find the dangerous squares in the chessboard, where if the AI
                    // moves its piece, it will almost certainly lose it

                    for(i = 0; i <= 7; i++) {
                        for(j = 0; j <= 7; j++) {
                            BoardDangerSquares[i, j] = 0;
                        }
                    }

                    // Initialize variables for finding the dangerous squares
                    for(int di = 0; di <= 7; di++) {
                        for(int dj = 0; dj <= 7; dj++) {
                            numberOfAttackers[di, dj] = 0;
                            numberOfDefenders[di, dj] = 0;
                            valueOfAttackers[di, dj] = 0;
                            valueOfDefenders[di, dj] = 0;
                        }
                    }
                    
                    // Scans for current pieces under threat
                    //findAttackers(Board);
                    findDefenders(Board);

                    #endregion checkDanger
                    // CHECK DANGER - End


                    computerMove(Board);

                } else if(g_WhoPlays.CompareTo("Human") == 0) {

                    // Player enters his move
                    Console.WriteLine("");
                    Console.Write("Starting column (A to H)...");
                    g_StartingColumn = Console.ReadLine().ToUpper(); //Converts text to uppercase for If statements (conversion done later)

                    Console.Write("Starting rank (1 to 8).....");
                    g_StartingRank = Int32.Parse(Console.ReadLine()); //Converts input to integers for If statements

                    Console.Write("Finishing column (A to H)...");
                    g_FinishingColumn = Console.ReadLine().ToUpper(); //Converts text to uppercase for If statements (conversion done later)

                    Console.Write("Finishing rank (1 to 8).....");
                    g_FinishingRank = Int32.Parse(Console.ReadLine()); //Converts input to integers for If statements

                    // Show the move entered
                    String displayedMove = String.Concat("Your move: ", g_StartingColumn, g_StartingRank.ToString(), " -> ");
                    displayedMove = String.Concat(displayedMove, g_FinishingColumn, g_FinishingRank.ToString());
                    Console.WriteLine(displayedMove);

                    Console.Clear();
                    Console.WriteLine("");
                    Console.WriteLine("Thinking...");

                    // Check the move entered by the Human for correctness and legality
                    enterMove();
                }

            } while(exit_game == false);

        }

        // Setup the starting positions
        public static void startingPosition() {
            for(int a = 0; a <= 7; a++) {
                for(int b = 0; b <= 7; b++) {
                    Board[(a), (b)] = "";
                }
            }

            Board[(0), (0)] = "White Rook";
            Board[(0), (1)] = "White Pawn";
            Board[(0), (6)] = "Black Pawn";
            Board[(0), (7)] = "Black Rook";
            Board[(1), (0)] = "White Knight";
            Board[(1), (1)] = "White Pawn";
            Board[(1), (6)] = "Black Pawn";
            Board[(1), (7)] = "Black Knight";
            Board[(2), (0)] = "White Bishop";
            Board[(2), (1)] = "White Pawn";
            Board[(2), (6)] = "Black Pawn";
            Board[(2), (7)] = "Black Bishop";
            Board[(3), (0)] = "White Queen";
            Board[(3), (1)] = "White Pawn";
            Board[(3), (6)] = "Black Pawn";
            Board[(3), (7)] = "Black Queen";
            Board[(4), (0)] = "White King";
            Board[(4), (1)] = "White Pawn";
            Board[(4), (6)] = "Black Pawn";
            Board[(4), (7)] = "Black King";
            Board[(5), (0)] = "White Bishop";
            Board[(5), (1)] = "White Pawn";
            Board[(5), (6)] = "Black Pawn";
            Board[(5), (7)] = "Black Bishop";
            Board[(6), (0)] = "White Knight";
            Board[(6), (1)] = "White Pawn";
            Board[(6), (6)] = "Black Pawn";
            Board[(6), (7)] = "Black Knight";
            Board[(7), (0)] = "White Rook";
            Board[(7), (1)] = "White Pawn";
            Board[(7), (6)] = "Black Pawn";
            Board[(7), (7)] = "Black Rook";

            g_WhichColorPlays = "White";
        }

        public static void enterMove() {
            // Validate the move the Human opponent entered

            // Store the move entered by the Human player

            if(g_StartingColumn.CompareTo("A") == 0)
                g_StartingColumnNumber = 1;
            else if(g_StartingColumn.CompareTo("B") == 0)
                g_StartingColumnNumber = 2;
            else if(g_StartingColumn.CompareTo("C") == 0)
                g_StartingColumnNumber = 3;
            else if(g_StartingColumn.CompareTo("D") == 0)
                g_StartingColumnNumber = 4;
            else if(g_StartingColumn.CompareTo("E") == 0)
                g_StartingColumnNumber = 5;
            else if(g_StartingColumn.CompareTo("F") == 0)
                g_StartingColumnNumber = 6;
            else if(g_StartingColumn.CompareTo("G") == 0)
                g_StartingColumnNumber = 7;
            else if(g_StartingColumn.CompareTo("H") == 0)
                g_StartingColumnNumber = 8;


            if(g_FinishingColumn.CompareTo("A") == 0)
                g_FinishingColumnNumber = 1;
            else if(g_FinishingColumn.CompareTo("B") == 0)
                g_FinishingColumnNumber = 2;
            else if(g_FinishingColumn.CompareTo("C") == 0)
                g_FinishingColumnNumber = 3;
            else if(g_FinishingColumn.CompareTo("D") == 0)
                g_FinishingColumnNumber = 4;
            else if(g_FinishingColumn.CompareTo("E") == 0)
                g_FinishingColumnNumber = 5;
            else if(g_FinishingColumn.CompareTo("F") == 0)
                g_FinishingColumnNumber = 6;
            else if(g_FinishingColumn.CompareTo("G") == 0)
                g_FinishingColumnNumber = 7;
            else if(g_FinishingColumn.CompareTo("H") == 0)
                g_FinishingColumnNumber = 8;


            // Is it his turn?
            if(g_WhoPlays.CompareTo("Human") == 0)   // If it is the computers turn then void

            // Is the column entered valid?

            // If user has entered a valid column (eg " e " or " f ") and moves a piece of the their colour then proceed
            // Its necessary to make and check whether the user has written a valid row ( i.e. a number from 1 to 8), because this is managed by the variables and g_StartingRank g_FinishingRank (which are integers taking values ​​from 1 to 8)

            if(((g_WhoPlays.CompareTo("Human") == 0)) && (((g_WhichColorPlays.CompareTo("White") == 0) && ((Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White Pawn") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White Rook") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White Knight") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White Bishop") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White Queen") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White King") == 0))) || ((g_WhichColorPlays.CompareTo("Black") == 0) && ((Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black Pawn") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black Rook") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black Knight") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black Bishop") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black Queen") == 0) || (Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black King") == 0))))) {

                g_WrongColumn = false;
                g_movingPiece = Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)];
            } else {
                g_WrongColumn = true;
            }

            // Check correctness of move entered
            g_Correctness = correctnessCheck(Board, 0, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);

            // Check legality of move entered (only if it is correct to save time)
            if(g_Correctness == true)
                g_Legal = legalityCheck(Board, 0, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);

            // Check if the Human's king is in check even after his move
            // Temporarily move the piece the user wants to move
            Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)] = "";
            g_tempMovingPiece = Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)];
            Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)] = g_movingPiece;

            // Check if White King is still under check
            whiteKingCheck = checkForWhiteCheck(Board);

            if((g_WhichColorPlays.CompareTo("White") == 0) && (whiteKingCheck == true))
                g_Legal = false;

            // Check if Black King is still under check
            blackKingCheck = checkForBlackCheck(Board);

            if((g_WhichColorPlays.CompareTo("Black") == 0) && (blackKingCheck == true))
                g_Legal = false;

            // Restore all pieces to the initial state
            Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)] = g_movingPiece;
            Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)] = g_tempMovingPiece;


            // Check if Human has castled
            #region checkCastling

            // White castling

            // Small castling
            if((g_PlayerColour.CompareTo("White") == 0) && (g_StartingColumnNumber == 5) && (g_FinishingColumnNumber == 7) && (g_StartingRank == 1) && (g_FinishingRank == 1)) {
                if((Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White King") == 0) && (Board[(7), (0)].CompareTo("White Rook") == 0) && (Board[(5), (0)].CompareTo("") == 0) && (Board[(6), (0)].CompareTo("") == 0)) {
                    g_Correctness = true;
                    g_Legal = true;
                    castlingOccured = true;
                }
            }

            // Big castling
            if((g_PlayerColour.CompareTo("White") == 0) && (g_StartingColumnNumber == 5) && (g_FinishingColumnNumber == 3) && (g_StartingRank == 1) && (g_FinishingRank == 1)) {
                if((Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("White King") == 0) && (Board[(0), (0)].CompareTo("White Rook") == 0) && (Board[(1), (0)].CompareTo("") == 0) && (Board[(2), (0)].CompareTo("") == 0) && (Board[(3), (0)].CompareTo("") == 0)) {
                    g_Correctness = true;
                    g_Legal = true;
                    castlingOccured = true;
                }
            }

            // Black castling

            // Small castling
            if((g_PlayerColour.CompareTo("Black") == 0) && (g_StartingColumnNumber == 5) && (g_FinishingColumnNumber == 7) && (g_StartingRank == 8) && (g_FinishingRank == 8)) {
                if((Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black King") == 0) && (Board[(7), (7)].CompareTo("Black Rook") == 0) && (Board[(5), (7)].CompareTo("") == 0) && (Board[(6), (7)].CompareTo("") == 0)) {
                    g_Correctness = true;
                    g_Legal = true;
                    castlingOccured = true;
                }
            }

            // Big castling
            if((g_PlayerColour.CompareTo("Black") == 0) && (g_StartingColumnNumber == 5) && (g_FinishingColumnNumber == 3) && (g_StartingRank == 8) && (g_FinishingRank == 8)) {
                if((Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)].CompareTo("Black King") == 0) && (Board[(0), (7)].CompareTo("Black Rook") == 0) && (Board[(1), (7)].CompareTo("") == 0) && (Board[(2), (7)].CompareTo("") == 0) && (Board[(3), (7)].CompareTo("") == 0)) {
                    g_Correctness = true;
                    g_Legal = true;
                    castlingOccured = true;
                }
            }
            #endregion checkCastling

            // Redraw the chessboard
            if((g_Correctness == true) && (g_Legal == true)) {
                if((g_movingPiece.CompareTo("White Rook") == 0) || (g_movingPiece.CompareTo("Black Rook") == 0))
                    valueOfHumanMovingPiece = 5;
                if((g_movingPiece.CompareTo("White Knight") == 0) || (g_movingPiece.CompareTo("Black Knight") == 0))
                    valueOfHumanMovingPiece = 3;
                if((g_movingPiece.CompareTo("White Bishop") == 0) || (g_movingPiece.CompareTo("Black Bishop") == 0))
                    valueOfHumanMovingPiece = 3;
                if((g_movingPiece.CompareTo("White Queen") == 0) || (g_movingPiece.CompareTo("Black Queen") == 0))
                    valueOfHumanMovingPiece = 9;
                if((g_movingPiece.CompareTo("White King") == 0) || (g_movingPiece.CompareTo("Black King") == 0))
                    valueOfHumanMovingPiece = 119;
                if((g_movingPiece.CompareTo("White Pawn") == 0) || (g_movingPiece.CompareTo("Black Pawn") == 0))
                    valueOfHumanMovingPiece = 1;

                // Game moves increase by 1 move only when the player plays, so as to avoid increasing the game moves every half-move
                // Erase initial square
                Board[(g_StartingColumnNumber - 1), (g_StartingRank - 1)] = "";

                humanLastMoveTargetColumn = -1;
                humanLastMoveTargetRow = -1;
                if(Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)].CompareTo("") != 0) {
                    humanLastMoveTargetColumn = g_FinishingColumnNumber;
                    humanLastMoveTargetRow = g_FinishingRank;
                }

                // Go to destination square
                Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)] = g_movingPiece;


                // Check for en passant
                #region checkEnPassant
                if(enpassantOccured == true)            {
                    if(g_PlayerColour.CompareTo("White") == 0)
                        Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1 - 1)] = "";
                    else if(g_PlayerColour.CompareTo("Black") == 0)
                        Board[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1 + 1)] = "";
                }

                // Record possible square when the next one playing will be able to perform en passant
                if((g_StartingRank == 2) && (g_FinishingRank == 4)) {
                    enpassantPossibleTargetRank = g_FinishingRank - 1;
                    enpassantPossibleTargetColumn = g_FinishingColumnNumber;
                } else if((g_StartingRank == 7) && (g_FinishingRank == 5)) {
                    enpassantPossibleTargetRank = g_FinishingRank + 1;
                    enpassantPossibleTargetColumn = g_FinishingColumnNumber;
                } else {
                    // Invalid value for enpassant move (enpassant not possible in the next move)
                    enpassantPossibleTargetRank = -9;
                    enpassantPossibleTargetColumn = -9;
                }
                #endregion checkEnPassant

                // Check if castling occured (so as to move the rook next to the moving king)
                #region castlingOccured
                if(castlingOccured == true) {
                    if(g_PlayerColour.CompareTo("White") == 0) {
                        if(Board[(6), (0)].CompareTo("White King") == 0) {
                            Board[(5), (0)] = "White Rook";
                            Board[(7), (0)] = "";
                            //Console.WriteLine( "Small Castle: White" );
                        } else if(Board[(2), (0)].CompareTo("White King") == 0) {
                            Board[(3), (0)] = "White Rook";
                            Board[(0), (0)] = "";
                            //Console.WriteLine( "Big Castle: White" );
                        }
                    } else if(g_PlayerColour.CompareTo("Black") == 0) {
                        if(Board[(6), (7)].CompareTo("Black King") == 0) {
                            Board[(5), (7)] = "Black Rook";
                            Board[(7), (7)] = "";
                            //Console.WriteLine( "Small Castle: Black" );
                        } else if(Board[(2), (7)].CompareTo("Black King") == 0) {
                            Board[(3), (7)] = "Black Rook";
                            Board[(0), (7)] = "";
                            //Console.WriteLine( "Big Castle: Black" );
                        }
                    }

                    // Restore the castlingOccured variable to false, so we avoid false castlings in the future
                    castlingOccured = false;
                }
                #endregion castlingOccured

                // Does a pawn need promotion?
                pawnPromotion();

                g_WhoPlays = "AI";

                // It is the other color's turn to play
                if(g_WhichColorPlays.CompareTo("White") == 0)
                    g_WhichColorPlays = "Black";
                else if(g_WhichColorPlays.CompareTo("Black") == 0)
                    g_WhichColorPlays = "White";

                // Restore variable values to initial values
                g_StartingColumn = "";
                g_FinishingColumn = "";
                g_StartingRank = 1;
                g_FinishingRank = 1;

                // "Check" Message
                whiteKingCheck = checkForWhiteCheck(Board);
                blackKingCheck = checkForBlackCheck(Board);

                if((whiteKingCheck == true) || (blackKingCheck == true)) {
                    Console.WriteLine("CHECK!");
                }


                //// If it is the turn of the AI to play, then call the respective AI Thought function
                //if(g_WhoPlays.CompareTo("AI") == 0) {
                //    moveAnalysed = 0;
                //    stopAnalyzing = false;
                //    First_Call = true;
                //    Best_Move_Found = false;
                //    whoIsAnalysed = "AI";
                //}

            } else {
                Console.WriteLine("Invalid move");
                displayBoard(Board);
            }


        }

        //STUDY
        /* FUNCTION TO CHECK THE CORRECTNESS OF THE MOVE
           (i.e. a Bishop can only move in diagonals, rooks in lines and columns etc)
           The correctness "checkForDanger" mode differs from the normal mode in that it does not make all the validations
           (since it is used to check for "Dangerous" squares in the chessboard and not to actually judge the correctness of an actual move) */
        public static bool correctnessCheck(string[,] correctnessBoard, int checkForDanger, int startRank, int startColumn, int finishRank, int finishColumn, String MovingPiece_2) {

            // Don't need to check for the existence of a piece in the destination square (legal check does that)
            bool correctness;
            correctness = false;
            enpassantOccured = false;
            if(MovingPiece_2 != null) {
                if((g_WhoPlays.CompareTo("Human") == 0) && (g_WrongColumn == false) && (MovingPiece_2.CompareTo("") == 1)) {
                    // If the user has written a valid column, has chosen to move a piece (not initiated a blank square) and its his turn to play, then analyse the correctness of the move

                    // Rook
                    if((MovingPiece_2.CompareTo("White Rook") == 0) || (MovingPiece_2.CompareTo("Black Rook") == 0)) {
                    if((finishColumn != startColumn) && (finishRank == startRank))       // Κίνηση σε στήλη
                        correctness = true;
                    else if((finishRank != startRank) && (finishColumn == startColumn))  // Κίνηση σε γραμμή
                        correctness = true;
                    }

                    // Knight
                    if((MovingPiece_2.CompareTo("White Knight") == 0) || (MovingPiece_2.CompareTo("Black Knight") == 0)) {
                        if((finishColumn == (startColumn + 1)) && (finishRank == (startRank + 2)))
                            correctness = true;
                        else if((finishColumn == (startColumn + 2)) && (finishRank == (startRank - 1)))
                            correctness = true;
                        else if((finishColumn == (startColumn + 1)) && (finishRank == (startRank - 2)))
                            correctness = true;
                        else if((finishColumn == (startColumn - 1)) && (finishRank == (startRank - 2)))
                            correctness = true;
                        else if((finishColumn == (startColumn - 2)) && (finishRank == (startRank - 1)))
                            correctness = true;
                        else if((finishColumn == (startColumn - 2)) && (finishRank == (startRank + 1)))
                            correctness = true;
                        else if((finishColumn == (startColumn - 1)) && (finishRank == (startRank + 2)))
                            correctness = true;
                        else if((finishColumn == (startColumn + 2)) && (finishRank == (startRank + 1)))
                            correctness = true;
                    }

                    // Bishop
                    if((MovingPiece_2.CompareTo("White Bishop") == 0) || (MovingPiece_2.CompareTo("Black Bishop") == 0)) {
                        if(((Math.Abs(finishColumn - startColumn)) == (Math.Abs(finishRank - startRank))) && (finishColumn != startColumn) && (finishRank != startRank))
                            correctness = true;
                    }

                    // Queen
                    if((MovingPiece_2.CompareTo("White Queen") == 0) || (MovingPiece_2.CompareTo("Black Queen") == 0)) {
                        if((finishColumn != startColumn) && (finishRank == startRank))       // Move to column
                            correctness = true;
                        else if((finishRank != startRank) && (finishColumn == startColumn))  // Move to column
                            correctness = true;
                        if(((Math.Abs(finishColumn - startColumn)) == (Math.Abs(finishRank - startRank))) && (finishColumn != startColumn) && (finishRank != startRank))
                            correctness = true;
                    }

                    // King
                    if((MovingPiece_2.CompareTo("White King") == 0) || (MovingPiece_2.CompareTo("Black King") == 0)) {
                        // move in rows and columns
                        if((finishColumn == (startColumn + 1)) && (finishRank == startRank))
                            correctness = true;
                        else if((finishColumn == (startColumn - 1)) && (finishRank == startRank))
                            correctness = true;
                        else if((finishRank == (startRank + 1)) && (finishColumn == startColumn))
                            correctness = true;
                        else if((finishRank == (startRank - 1)) && (finishColumn == startColumn))
                            correctness = true;

                        // move in diagonals
                        else if((finishColumn == (startColumn + 1)) && (finishRank == (startRank + 1)))
                            correctness = true;
                        else if((finishColumn == (startColumn + 1)) && (finishRank == (startRank - 1)))
                            correctness = true;
                        else if((finishColumn == (startColumn - 1)) && (finishRank == (startRank - 1)))
                            correctness = true;
                        else if((finishColumn == (startColumn - 1)) && (finishRank == (startRank + 1)))
                            correctness = true;
                    }

                    // White Pawn
                    if(MovingPiece_2.CompareTo("White Pawn") == 0) {
                        // move forward
                        if((finishRank == (startRank + 1)) && (finishColumn == startColumn))
                            correctness = true;
                        // move forward for 2 squares
                        else if((finishRank == (startRank + 2)) && (finishColumn == startColumn) && (startRank == 2))
                            correctness = true;

                        else if((finishRank == (startRank + 1)) && ((finishColumn == (startColumn - 1)) || (finishColumn == (startColumn + 1)))) {
                            if(checkForDanger == 0) {
                                // eat forward to the left
                                if((finishRank == (startRank + 1)) && (finishColumn == (startColumn - 1)) && ((correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Pawn") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Rook") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Knight") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Bishop") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Queen") == 0)))
                                    correctness = true;

                                // eat forward to the right
                                if((finishRank == (startRank + 1)) && (finishColumn == (startColumn + 1)) && ((correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Pawn") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Rook") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Knight") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Bishop") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("Black Queen") == 0)))
                                    correctness = true;
                            } else if(checkForDanger == 1) {
                                correctness = true;
                            }
                        }

                        // En Passant eat forward to the left
                        else if((finishRank == (startRank + 1)) && (finishColumn == (startColumn - 1))) {
                            if(checkForDanger == 0) {
                                if((finishRank == 6) && (correctnessBoard[(finishColumn - 1), (4)].CompareTo("Black Pawn") == 0)) {
                                    correctness = true;
                                    enpassantOccured = true;
                                    correctnessBoard[(finishColumn - 1), (finishRank - 1 - 1)] = "";
                                } else {
                                    correctness = false;
                                    enpassantOccured = false;
                                }
                            }
                        }

                        // En Passant eat forward to the right
                        else if((finishRank == (startRank + 1)) && (finishColumn == (startColumn + 1))) {
                            if(checkForDanger == 0) {
                                if((finishRank == 6) && (correctnessBoard[(finishColumn - 1), (4)].CompareTo("Black Pawn") == 0)) {
                                    correctness = true;
                                    enpassantOccured = true;
                                    correctnessBoard[(finishColumn - 1), (finishRank - 1 - 1)] = "";
                                } else {
                                    correctness = false;
                                    enpassantOccured = false;
                                }
                            }
                        }
                    }


                    // Black Pawn
                    if(MovingPiece_2.CompareTo("Black Pawn") == 0) {
                        // move forward
                        if((finishRank == (startRank - 1)) && (finishColumn == startColumn))
                            correctness = true;
                        // move forward for 2 squares
                        else if((finishRank == (startRank - 2)) && (finishColumn == startColumn) && (startRank == 7))
                            correctness = true;

                        else if((finishRank == (startRank - 1)) && ((finishColumn == (startColumn + 1)) || (finishColumn == (startColumn - 1)))) {
                            if(checkForDanger == 0) {
                                // eat forward to the left
                                if((finishRank == (startRank - 1)) && (finishColumn == (startColumn + 1)) && ((correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Pawn") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Rook") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Knight") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Bishop") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Queen") == 0)))
                                    correctness = true;

                                // eat forward to the right
                                if((finishRank == (startRank - 1)) && (finishColumn == (startColumn - 1)) && ((correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Pawn") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Rook") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Knight") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Bishop") == 0) || (correctnessBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("White Queen") == 0)))
                                    correctness = true;
                            } else if(checkForDanger == 1) {
                                // eat forward to the left
                                if((finishRank == (startRank - 1)) && (finishColumn == (startColumn + 1)))
                                    correctness = true;

                                // eat forward to the right
                                if((finishRank == (startRank - 1)) && (finishColumn == (startColumn - 1)))
                                    correctness = true;
                            }
                        }

                        // En Passant eat forward to the left
                        else if((finishRank == (startRank - 1)) && (finishColumn == (startColumn + 1))) {
                            if(checkForDanger == 0) {
                                if((finishRank == 3) && (correctnessBoard[(finishColumn - 1), (3)].CompareTo("White Pawn") == 0)) {
                                    correctness = true;
                                    enpassantOccured = true;
                                    correctnessBoard[(finishColumn - 1), (finishRank + 1 - 1)] = "";
                                } else {
                                    correctness = false;
                                    enpassantOccured = false;
                                }
                            }
                        }

                        // En Passant eat forward to the right
                        else if((finishRank == (startRank - 1)) && (finishColumn == (startColumn - 1))) {
                            if(checkForDanger == 0) {
                                if((finishRank == 3) && (correctnessBoard[(finishColumn - 1), (3)].CompareTo("White Pawn") == 0)) {
                                    correctness = true;
                                    enpassantOccured = true;
                                    correctnessBoard[(finishColumn - 1), (finishRank + 1 - 1)] = "";
                                } else {
                                    correctness = false;
                                    enpassantOccured = false;
                                }
                            }
                        }
                    }
                }
            }
            return correctness;
        }

        //STUDY
        public static bool legalityCheck(string[,] legalBoard, int checkForDanger, int startRank, int startColumn, int finishRank, int finishColumn, String MovingPiece_2) {

            bool legal;

           // Check the legality of the move e.g. the user has chosen to move a tower from a2 to a5 , but the a4 is a pawn , then legal is then set to false and returned by the function

            legal = true;

            if(((finishRank - 1) > 7) || ((finishRank - 1) < 0) || ((finishColumn - 1) > 7) || ((finishColumn - 1) < 0))
                legal = false;

            // if a piece of the same colour is in the destination square
            if(checkForDanger == 0) {
                if((MovingPiece_2.CompareTo("White King") == 0) || (MovingPiece_2.CompareTo("White Queen") == 0) || (MovingPiece_2.CompareTo("White Rook") == 0) || (MovingPiece_2.CompareTo("White Knight") == 0) || (MovingPiece_2.CompareTo("White Bishop") == 0) || (MovingPiece_2.CompareTo("White Pawn") == 0)) {
                    if((legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("White King") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("White Queen") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("White Rook") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("White Knight") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("White Bishop") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("White Pawn") == 0)) {
                    legal = false;
                    }
                } else if((MovingPiece_2.CompareTo("Black King") == 0) || (MovingPiece_2.CompareTo("Black Queen") == 0) || (MovingPiece_2.CompareTo("Black Rook") == 0) || (MovingPiece_2.CompareTo("Black Knight") == 0) || (MovingPiece_2.CompareTo("Black Bishop") == 0) || (MovingPiece_2.CompareTo("Black Pawn") == 0)) {
                    if((legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("Black King") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("Black Queen") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("Black Rook") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("Black Knight") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("Black Bishop") == 0) || (legalBoard[((finishColumn - 1)), ((finishRank - 1))].CompareTo("Black Pawn") == 0))
                        legal = false;
                }
            }

            if(MovingPiece_2.CompareTo("White King") == 0) {
                if(checkForDanger == 0) {

                    // White King

                    // is the king threatened in the destination square?
                    // temporarily move king
                    legalBoard[(startColumn - 1), (startRank - 1)] = "";
                    g_tempMovingPieceKingCheck = legalBoard[(finishColumn - 1), (finishRank - 1)];
                    legalBoard[(finishColumn - 1), (finishRank - 1)] = "White King";

                    whiteKingCheck = checkForWhiteCheck(legalBoard);

                    if(whiteKingCheck == true)
                        legal = false;

                    // restore pieces
                    legalBoard[(startColumn - 1), (startRank - 1)] = "White King";
                    legalBoard[(finishColumn - 1), (finishRank - 1)] = g_tempMovingPieceKingCheck;
                }
            } else if(MovingPiece_2.CompareTo("Black King") == 0) {
                if(checkForDanger == 0) {

                    // Black King

                    // is the BK threatened in the destination square?
                    // temporarily move king
                    legalBoard[(startColumn - 1), (startRank - 1)] = "";
                    g_tempMovingPieceKingCheck = legalBoard[(finishColumn - 1), (finishRank - 1)];
                    legalBoard[(finishColumn - 1), (finishRank - 1)] = "Black King";

                    blackKingCheck = checkForBlackCheck(legalBoard);

                    if(blackKingCheck == true)
                        legal = false;

                    // restore pieces
                    legalBoard[(startColumn - 1), (startRank - 1)] = "Black King";
                    legalBoard[(finishColumn - 1), (finishRank - 1)] = g_tempMovingPieceKingCheck;
                }
            } else if(MovingPiece_2.CompareTo("White Pawn") == 0) {
                if(checkForDanger == 0) {

                    // White Pawn

                    // move forward
                    if((finishRank == (startRank + 1)) && (finishColumn == startColumn)) {
                        if(legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 1) {
                            legal = false;
                        }
                    }

                    // move forward for 2 squares
                    else if((finishRank == (startRank + 2)) && (finishColumn == startColumn)) {
                        if(startRank == 2) {
                            if((legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 1) || (legalBoard[(finishColumn - 1), (finishRank - 1 - 1)].CompareTo("") == 1))
                                legal = false;
                        }
                    }

                    // eat forward to the right
                    else if((finishRank == (startRank + 1)) && (finishColumn == startColumn + 1)) {
                        if(enpassantOccured == false) {
                            if(legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 0)
                                legal = false;
                        }
                    }

                    // eat forward to the left
                    else if((finishRank == (startRank + 1)) && (finishColumn == startColumn - 1)) {
                        if(enpassantOccured == false) {
                            if(legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 0)
                                legal = false;
                        }
                    }
                }
            } else if(MovingPiece_2.CompareTo("Black Pawn") == 0) {
                if(checkForDanger == 0) {
                    // Black Pawn

                    // move forward
                    if((finishRank == (startRank - 1)) && (finishColumn == startColumn)) {
                        if(legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 1) //If occupied
                            legal = false;
                    }

                    // move forward for 2 squares
                    else if((finishRank == (startRank - 2)) && (finishColumn == startColumn)) {
                        if(startRank == 7) {
                            if((legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 1) || (legalBoard[(finishColumn - 1), (finishRank + 1 - 1)].CompareTo("") == 1)) // Dont need + - in actualitty
                                legal = false;
                        }
                    }

                    // eat forward to the right
                    else if((finishRank == (startRank - 1)) && (finishColumn == startColumn + 1)) {
                        if(enpassantOccured == false) {
                            if(legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 0)
                                legal = false;
                        }
                    }

                    // eat forward to the left
                    else if((finishRank == (startRank - 1)) && (finishColumn == startColumn - 1)) {
                        if(enpassantOccured == false) {
                            if(legalBoard[(finishColumn - 1), (finishRank - 1)].CompareTo("") == 0)
                                legal = false;
                        }
                    }
                }
            } else if((MovingPiece_2.CompareTo("White Rook") == 0) || (MovingPiece_2.CompareTo("White Queen") == 0) || (MovingPiece_2.CompareTo("White Bishop") == 0) || (MovingPiece_2.CompareTo("Black Rook") == 0) || (MovingPiece_2.CompareTo("Black Queen") == 0) || (MovingPiece_2.CompareTo("Black Bishop") == 0)) {
                h = 0;
                p = 0;
                howToMoveRank = 0;
                howToMoveColumn = 0;

                if(((finishRank - 1) > (startRank - 1)) || ((finishRank - 1) < (startRank - 1)))
                    howToMoveRank = ((finishRank - 1) - (startRank - 1)) / System.Math.Abs((finishRank - 1) - (startRank - 1));

                if(((finishColumn - 1) > (startColumn - 1)) || ((finishColumn - 1) < (startColumn - 1)))
                    howToMoveColumn = ((finishColumn - 1) - (startColumn - 1)) / System.Math.Abs((finishColumn - 1) - (startColumn - 1));

                exitLegalCheck = false;

                do {
                    h = h + howToMoveRank;
                    p = p + howToMoveColumn;

                    if((((startRank - 1) + h) == (finishRank - 1)) && ((((startColumn - 1) + p)) == (finishColumn - 1)))
                        exitLegalCheck = true;

                    if((startColumn - 1 + p) < 0)
                        exitLegalCheck = true;
                    else if((startRank - 1 + h) < 0)
                        exitLegalCheck = true;
                    else if((startColumn - 1 + p) > 7)
                        exitLegalCheck = true;
                    else if((startRank - 1 + h) > 7)
                        exitLegalCheck = true;

                    // if a piece exists between the initial and the destination square, then the move is illegal
                    if(exitLegalCheck == false) {
                        if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("White Rook") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("White Knight") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("White Bishop") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("White Queen") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("White King") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("White Pawn") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        }

                        if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("Black Rook") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("Black Knight") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("Black Bishop") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("Black Queen") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("Black King") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        } else if(legalBoard[(startColumn - 1 + p), (startRank - 1 + h)].CompareTo("Black Pawn") == 0) {
                            legal = false;
                            exitLegalCheck = true;
                        }
                    }
                } while(exitLegalCheck == false);
            }
            return legal;
        }

        //STUDY
        public static bool checkForWhiteCheck(string[,] WCBoard) {
            // Check if the WK is under check

            bool kingCheck;

            /* Find the coordinates of the king
               If a king is found, then record that square's coordinates into whiteKingColumn and whiteKingRank
               I write (i + 1) instead of i and (j + 1) instead of j because the first element of BCBoard table [(8), (8)] starts at BCBoard[(0),(0)] not BCBoard[(1),(1)] */

            for(i = 0; i <= 7; i++) {
                for(j = 0; j <= 7; j++) {
                    if(WCBoard[(i), (j)].CompareTo("White King") == 0) {
                        whiteKingColumn = (i + 1);
                        whiteKingRank = (j + 1);
                    }
                }
            }

            // Now I check whether the white king is in check
            kingCheck = false;

            // First check if there is a risk for the white king from the right, 'WCBoard [(8), (8)],(whiteKingColumn + 1)' <= 8 in the 'if' statements makes sure the king stays on the board. At first, dangerFromRight = true. However if there is a white piece to the right of the white king, then the king is not in check to its right since it is protected by a piece of the same colour, therefore we set dangerFromRight = false and threats from the right stop (I added the condition (dangerFromRight == true) to the "If they do this" test)
            // But if there is no white piece right of the king to protect him, then the king is likely to be threatened from his right, so I continue the watch.
            // Note : The check is made possible by the opposing tower or queen

            // Check if there is a risk for the white king from the right (from queen or bishop)
            dangerFromRight = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((whiteKingColumn + tick) <= 8) && (dangerFromRight == true)) {
                    if((WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("White Queen") == 0))
                        dangerFromRight = false;
                    else if((WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank - 1)].CompareTo("Black King") == 0))
                        dangerFromRight = false;
                }
            }


            // Check if there is a risk for the white king from the left (from queen or bishop)
            dangerFromLeft = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((whiteKingColumn - tick) >= 1) && (dangerFromLeft == true)) {
                    if((WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("White Queen") == 0))
                        dangerFromLeft = false;
                    else if((WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - 1)].CompareTo("Black King") == 0))
                        dangerFromLeft = false;
                }
            }


            // Check if there is a risk for the white king from above (from queen or bishop)
            dangerFromUp = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((whiteKingRank + tick) <= 8) && (dangerFromUp == true)) {
                    if((WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("White Queen") == 0))
                        dangerFromUp = false;
                    else if((WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank + tick - 1)].CompareTo("Black King") == 0))
                        dangerFromUp = false;
                }
            }


            // Check if there is a risk for the white king from below (from queen or bishop)
            dangerFromDown = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((whiteKingRank - tick) >= 1) && (dangerFromDown == true)) {
                    if((WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("White Queen") == 0))
                        dangerFromDown = false;
                    else if((WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn - 1), (whiteKingRank - tick - 1)].CompareTo("Black King") == 0))
                        dangerFromDown = false;
                }
            }


            // Check if there is a risk for the white king from up-right (from queen or bishop)
            dangerFromUpRight = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((whiteKingColumn + tick) <= 8) && ((whiteKingRank + tick) <= 8) && (dangerFromUpRight == true)) {
                    if((WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("White Queen") == 0))
                        dangerFromUpRight = false;
                    else if((WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn + tick - 1), (whiteKingRank + tick - 1)].CompareTo("Black King") == 0))
                        dangerFromUpRight = false;
                }
            }


            // Check if there is a risk for the white king from down-left (from queen or bishop)
            dangerFromDownLeft = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((whiteKingColumn - tick) >= 1) && ((whiteKingRank - tick) >= 1) && (dangerFromDownLeft == true)) {
                    if((WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("White Queen") == 0))
                        dangerFromDownLeft = false;
                    else if((WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn - tick - 1), (whiteKingRank - tick - 1)].CompareTo("Black King") == 0))
                        dangerFromDownLeft = false;
                }
            }

            // Check if there is a risk for the white king from down-right (from queen or bishop)
            dangerFromDownRight = true;

            for(int pos = 1; pos <= 7; pos++) {
                if(((whiteKingColumn + pos) <= 8) && ((whiteKingRank - pos) >= 1) && (dangerFromDownRight == true)) {
                    if((WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("White Queen") == 0))
                        dangerFromDownRight = false;
                    else if((WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn + pos - 1), (whiteKingRank - pos - 1)].CompareTo("Black King") == 0))
                        dangerFromDownRight = false;
                }
            }


            // Check if there is a risk for the white king from up-left (from queen or bishop)
            dangerFromUpLeft = true;

            for(int pos = 1; pos <= 7; pos++) {
                if(((whiteKingColumn - pos) >= 1) && ((whiteKingRank + pos) <= 8) && (dangerFromUpLeft == true)) {
                    if((WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("Black Bishop") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("Black Queen") == 0))
                        kingCheck = true;
                    else if((WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("White Pawn") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("White Rook") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("White Knight") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("White Bishop") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("White Queen") == 0))
                        dangerFromUpLeft = false;
                    else if((WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("Black Pawn") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("Black Rook") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("Black Knight") == 0) || (WCBoard[(whiteKingColumn - pos - 1), (whiteKingRank + pos - 1)].CompareTo("Black King") == 0))
                        dangerFromUpLeft = false;
                }
            }



            // Check if the white king is threatened by pawn
            if(((whiteKingColumn + 1) <= 8) && ((whiteKingRank + 1) <= 8)) {
                if(WCBoard[(whiteKingColumn + 1 - 1), (whiteKingRank + 1 - 1)].CompareTo("Black Pawn") == 0) {
                    kingCheck = true;
                }
            }


            if(((whiteKingColumn - 1) >= 1) && ((whiteKingRank + 1) <= 8)) {
                if(WCBoard[(whiteKingColumn - 1 - 1), (whiteKingRank + 1 - 1)].CompareTo("Black Pawn") == 0) {
                    kingCheck = true;
                }
            }


            // Check if the white king is threatened by knight
            if(((whiteKingColumn + 1) <= 8) && ((whiteKingRank + 2) <= 8))
                if(WCBoard[(whiteKingColumn + 1 - 1), (whiteKingRank + 2 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn + 2) <= 8) && ((whiteKingRank - 1) >= 1))
                if(WCBoard[(whiteKingColumn + 2 - 1), (whiteKingRank - 1 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn + 1) <= 8) && ((whiteKingRank - 2) >= 1))
                if(WCBoard[(whiteKingColumn + 1 - 1), (whiteKingRank - 2 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn - 1) >= 1) && ((whiteKingRank - 2) >= 1))
                if(WCBoard[(whiteKingColumn - 1 - 1), (whiteKingRank - 2 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn - 2) >= 1) && ((whiteKingRank - 1) >= 1))
                if(WCBoard[(whiteKingColumn - 2 - 1), (whiteKingRank - 1 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn - 2) >= 1) && ((whiteKingRank + 1) <= 8))
                if(WCBoard[(whiteKingColumn - 2 - 1), (whiteKingRank + 1 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn - 1) >= 1) && ((whiteKingRank + 2) <= 8))
                if(WCBoard[(whiteKingColumn - 1 - 1), (whiteKingRank + 2 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            if(((whiteKingColumn + 2) <= 8) && ((whiteKingRank + 1) <= 8))
                if(WCBoard[(whiteKingColumn + 2 - 1), (whiteKingRank + 1 - 1)].CompareTo("Black Knight") == 0)
                    kingCheck = true;

            return kingCheck;
        }
        //FIX
        public static bool checkForWhiteMate(string[,] WMBoard) {
            // Check if the WK is under checkmate

            bool mate;

            // Variable that is used to control whether there is mate
            /* Specifically, the function checks if king is initially in check and if there is, check whether this
             * can be avoided by moving the threatened king in a nearby square . */
            // This variable captures whether there remains a possibility of a mate on the chessboard.

            bool dangerForMate;

            // Check whether there is a mate  on the white king
            mate = false;
            dangerForMate = true;    /* First, there is a good possibility to have a mate on the chessboard, 
            so I define it true until proven false */
            // If after checking all possible moves the king can move in an adjacent block and avoid check, the variable gets returns false.


            // Find the initial coordinates of the King
            for(i = 0; i <= 7; i++) {
                for(j = 0; j <= 7; j++) {
                    if(WMBoard[(i), (j)].CompareTo("White King") == 0) {
                        startingWhiteKingColumn = (i + 1);
                        startingWhiteKingRank = (j + 1);
                    }
                }
            }


            // Check if the white king is checkmate
            if(g_WhichColorPlays.CompareTo("White") == 0) {

                // Check if there is check at the moment
                whiteKingCheck = checkForWhiteCheck(WMBoard);

                if(whiteKingCheck == false)
                    dangerForMate = false;

                // Check whether the king will remain in check if it tries to escape up
                if(startingWhiteKingRank < 8) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1 + 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingRank - 1 + 1) <= 7)) {

                     /* Temporarily moving the king upwards to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1 + 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1 + 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape up-right
                if((startingWhiteKingColumn < 8) && (startingWhiteKingRank < 8)) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1 + 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingRank - 1 + 1) <= 7) && ((startingWhiteKingColumn - 1 + 1) <= 7)) {

                        /* Temporarily moving the king up-right to check whether it remains in check 
                           The verification is done only if there is no other piece of the same colour in the square to block it
                           and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1 + 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1 + 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape right
                if(startingWhiteKingColumn < 8) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingColumn - 1 + 1) <= 7)) {

                     /* Temporarily moving the king right to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape down-right
                if((startingWhiteKingColumn < 8) && (startingWhiteKingRank > 1)) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1 - 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingRank - 1 - 1) >= 0) && ((startingWhiteKingColumn - 1 + 1) <= 7)) {

                     /* Temporarily moving the king down-right to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1 - 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1 + 1), (startingWhiteKingRank - 1 - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape down
                if(startingWhiteKingRank > 1) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1 - 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingRank - 1 - 1) >= 0)) {

                     /* Temporarily moving the king down to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1 - 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1 - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape down-left
                if((startingWhiteKingColumn > 1) && (startingWhiteKingRank > 1)) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1 - 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingRank - 1 - 1) >= 0) && ((startingWhiteKingColumn - 1 - 1) >= 0)) {

                     /* Temporarily moving the king down-left to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1 - 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1 - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape left
                if(startingWhiteKingColumn > 1) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingColumn - 1 - 1) >= 0)) {

                     /* Temporarily moving the king left to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape up-left
                if((startingWhiteKingColumn > 1) && (startingWhiteKingRank < 8)) {

                    g_movingPiece = WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)];
                    g_tempMovingPiece = WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1 + 1)];

                    if((g_tempMovingPiece.CompareTo("White Queen") == 1) && (g_tempMovingPiece.CompareTo("White Rook") == 1) && (g_tempMovingPiece.CompareTo("White Knight") == 1) && (g_tempMovingPiece.CompareTo("White Bishop") == 1) && (g_tempMovingPiece.CompareTo("White Pawn") == 1) && (dangerForMate == true) && ((startingWhiteKingRank - 1 + 1) <= 7) && ((startingWhiteKingColumn - 1 - 1) >= 0)) {

                     /* Temporarily moving the king up-left to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = "";
                        WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1 + 1)] = g_movingPiece;
                        whiteKingCheck = checkForWhiteCheck(WMBoard);

                        if(whiteKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        WMBoard[(startingWhiteKingColumn - 1), (startingWhiteKingRank - 1)] = g_movingPiece;
                        WMBoard[(startingWhiteKingColumn - 1 - 1), (startingWhiteKingRank - 1 + 1)] = g_tempMovingPiece;

                    }

                }

                if(dangerForMate == true)
                    mate = true;

            }

            return mate;
        }

        //STUDY
        public static bool checkForBlackCheck(string[,] BCBoard) {
            // Check if the BK is under threat

            bool kingCheck;

            /* Find the coordinates of the king
               If a king is found, then record that square's coordinates into blKingColumn and blKingRank
               I write (i + 1) instead of i and (j + 1) instead of j because the first element of BCBoard table [(8), (8)] starts at BCBoard[(0),(0)] not BCBoard[(1),(1)] */
            for(i = 0; i <= 7; i++) {
                for(j = 0; j <= 7; j++) {
                    if(BCBoard[(i), (j)].CompareTo("Black King") == 0) {
                        blKingColumn = (i + 1);
                        blKingRank = (j + 1);
                    }
                }
            }

            // Now I check whether the black king is in check
            kingCheck = false;

            // First check if there is a risk for the white king from the right, 'BCBoard [(8), (8)],(blKingColumn + 1)' <= 8 in the 'if' statements makes sure the king stays on the board. At first, dangerFromRight = true. However if there is a white piece to the right of the white king, then the king is not in check to its right since it is protected by a piece of the same colour, therefore we set dangerFromRight = false and threats from the right stop (I added the condition (dangerFromRight == true) to the "If they do this" test)
            // But if there is no black piece right of the king to protect him, then the king is likely to be threatened from his right, so I continue the watch
            // Note : The check is made possible by the opposing tower or queen

            // Check if there is a risk for the black king from the right (from queen or bishop)
            dangerFromRight = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingColumn + tick) <= 8) && (dangerFromRight == true)) {
                    if((BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("Black Queen") == 0))
                        dangerFromRight = false;
                    else if((BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - 1)].CompareTo("White King") == 0))
                        dangerFromRight = false;
                }
            }



            // Check if there is a risk for the black king from the left (from queen or bishop)
            dangerFromLeft = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingColumn - tick) >= 1) && (dangerFromLeft == true)) {
                    if((BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("Black Queen") == 0))
                        dangerFromLeft = false;
                    else if((BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - 1)].CompareTo("White King") == 0))
                        dangerFromLeft = false;
                }
            }



            // Check if there is a risk for the black king from above (from queen or bishop)
            dangerFromUp = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingRank + tick) <= 8) && (dangerFromUp == true)) {
                    if((BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("Black Queen") == 0))
                        dangerFromUp = false;
                    else if((BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank + tick - 1)].CompareTo("White King") == 0))
                        dangerFromUp = false;
                }
            }

            // Check if there is a risk for the black king from below (from queen or bishop)
            dangerFromDown = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingRank - tick) >= 1) && (dangerFromDown == true)) {
                    if((BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("Black Queen") == 0))
                        dangerFromDown = false;
                    else if((BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn - 1), (blKingRank - tick - 1)].CompareTo("White King") == 0))
                        dangerFromDown = false;
                }
            }



            // Check if there is a risk for the black king from the up-right (from queen or bishop)
            dangerFromUpRight = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingColumn + tick) <= 8) && ((blKingRank + tick) <= 8) && (dangerFromUpRight == true)) {
                    if((BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("Black Queen") == 0))
                        dangerFromUpRight = false;
                    else if((BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank + tick - 1)].CompareTo("White King") == 0))
                        dangerFromUpRight = false;
                }
            }



            // Check if there is a risk for the black king from the bottom-left (from queen or bishop)
            dangerFromDownLeft = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingColumn - tick) >= 1) && ((blKingRank - tick) >= 1) && (dangerFromDownLeft == true)) {
                    if((BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("Black Queen") == 0))
                        dangerFromDownLeft = false;
                    else if((BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank - tick - 1)].CompareTo("White King") == 0))
                        dangerFromDownLeft = false;
                }
            }


            
            // Check if there is a risk for the black king from the bottom-right (from queen or bishop)
            dangerFromDownRight = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingColumn + tick) <= 8) && ((blKingRank - tick) >= 1) && (dangerFromDownRight == true)) {
                    if((BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("Black Queen") == 0))
                        dangerFromDownRight = false;
                    else if((BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn + tick - 1), (blKingRank - tick - 1)].CompareTo("White King") == 0))
                        dangerFromDownRight = false;
                }
            }

            // Check if there is a risk for the black king from the top-left (from queen or bishop)
            dangerFromUpLeft = true;

            for(int tick = 1; tick <= 7; tick++) {
                if(((blKingColumn - tick) >= 1) && ((blKingRank + tick) <= 8) && (dangerFromUpLeft == true)) {
                    if((BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("White Bishop") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("White Queen") == 0))
                        kingCheck = true;
                    else if((BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("Black Pawn") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("Black Rook") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("Black Knight") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("Black Bishop") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("Black Queen") == 0))
                        dangerFromUpLeft = false;
                    else if((BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("White Pawn") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("White Rook") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("White Knight") == 0) || (BCBoard[(blKingColumn - tick - 1), (blKingRank + tick - 1)].CompareTo("White King") == 0))
                        dangerFromUpLeft = false;
                }
            }

            // Check if the black king is threatened by pawn
            if(((blKingColumn + 1) <= 8) && ((blKingRank - 1) >= 1)) {
                if(BCBoard[(blKingColumn + 1 - 1), (blKingRank - 1 - 1)].CompareTo("White Pawn") == 0) {
                    kingCheck = true;
                }
            }


            if(((blKingColumn - 1) >= 1) && ((blKingRank - 1) >= 1)) {
                if(BCBoard[(blKingColumn - 1 - 1), (blKingRank - 1 - 1)].CompareTo("White Pawn") == 0) {
                    kingCheck = true;
                }
            }

            // Check if the black king is threatened by knight
            if(((blKingColumn + 1) <= 8) && ((blKingRank + 2) <= 8))
                if(BCBoard[(blKingColumn + 1 - 1), (blKingRank + 2 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn + 2) <= 8) && ((blKingRank - 1) >= 1))
                if(BCBoard[(blKingColumn + 2 - 1), (blKingRank - 1 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn + 1) <= 8) && ((blKingRank - 2) >= 1))
                if(BCBoard[(blKingColumn + 1 - 1), (blKingRank - 2 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn - 1) >= 1) && ((blKingRank - 2) >= 1))
                if(BCBoard[(blKingColumn - 1 - 1), (blKingRank - 2 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn - 2) >= 1) && ((blKingRank - 1) >= 1))
                if(BCBoard[(blKingColumn - 2 - 1), (blKingRank - 1 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn - 2) >= 1) && ((blKingRank + 1) <= 8))
                if(BCBoard[(blKingColumn - 2 - 1), (blKingRank + 1 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn - 1) >= 1) && ((blKingRank + 2) <= 8))
                if(BCBoard[(blKingColumn - 1 - 1), (blKingRank + 2 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            if(((blKingColumn + 2) <= 8) && ((blKingRank + 1) <= 8))
                if(BCBoard[(blKingColumn + 2 - 1), (blKingRank + 1 - 1)].CompareTo("White Knight") == 0)
                    kingCheck = true;

            return kingCheck;
        }
        //FIX
        public static bool checkForBlackMate(string[,] BMBoard) {
            // Check if the BK is under checkmate

            bool mate;

            // Variable that is used to control whether there is mate
            /* Specifically, the function checks if king is initially in check and if there is, check whether this
             * can be avoided by moving the threatened king in a nearby square . */
            // This variable captures whether there remains a possibility of a mate on the chessboard.

            bool dangerForMate;

            // Check whether there is a mate  on the black king
            mate = false;
            dangerForMate = true;    /* First, there is a good possibility to have a mate on the chessboard, 
                so I define it true until proven false */
            // If after checking all possible moves the king can move in an adjacent block and avoid check, the variable gets returns false.


            // Find the initial coordinates of the King
            for(i = 0; i <= 7; i++) {
                for(j = 0; j <= 7; j++) {
                    if(BMBoard[(i), (j)].CompareTo("Black King") == 0) {
                        startingBlKingColumn = (i + 1);
                        startingBlKingRank = (j + 1);
                    }
                }
            }


            // Check if the white king is checkmate
            if(g_WhichColorPlays.CompareTo("Black") == 0) {

                // Check if there is check at the moment
                blackKingCheck = checkForBlackCheck(BMBoard);

                if(blackKingCheck == false)
                    dangerForMate = false;

                // Check whether the king will remain in check if it tries to escape up
                if(startingBlKingRank < 8) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1 + 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingRank - 1 + 1) <= 7)) {

                     /* Temporarily moving the king upwards to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1 + 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1 + 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape up-right
                if((startingBlKingColumn < 8) && (startingBlKingRank < 8)) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1 + 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingRank - 1 + 1) <= 7) && ((startingBlKingColumn - 1 + 1) <= 7)) {

                     /* Temporarily moving the king up-right to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1 + 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1 + 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape right
                if(startingBlKingColumn < 8) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingColumn - 1 + 1) <= 7)) {

                     /* Temporarily moving the king right to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape down-right
                if((startingBlKingColumn < 8) && (startingBlKingRank > 1)) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1 - 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingRank - 1 - 1) >= 0) && ((startingBlKingColumn - 1 + 1) <= 7)) {

                     /* Temporarily moving the king down-right to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1 - 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1 + 1), (startingBlKingRank - 1 - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape down
                if(startingBlKingRank > 1) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1 - 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingRank - 1 - 1) >= 0)) {

                     /* Temporarily moving the king down to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1 - 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1 - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape down-left
                if((startingBlKingColumn > 1) && (startingBlKingRank > 1)) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1 - 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingRank - 1 - 1) >= 0) && ((startingBlKingColumn - 1 - 1) >= 0)) {

                     /* Temporarily moving the king down-left to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1 - 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1 - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape left
                if(startingBlKingColumn > 1) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingColumn - 1 - 1) >= 0)) {

                     /* Temporarily moving the king left to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1)] = g_tempMovingPiece;

                    }

                }


                // Check whether the king will remain in check if it tries to escape up-left
                if((startingBlKingColumn > 1) && (startingBlKingRank < 8)) {

                    g_movingPiece = BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)];
                    g_tempMovingPiece = BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1 + 1)];

                    if((g_tempMovingPiece.CompareTo("Black Queen") == 1) && (g_tempMovingPiece.CompareTo("Black Rook") == 1) && (g_tempMovingPiece.CompareTo("Black Knight") == 1) && (g_tempMovingPiece.CompareTo("Black Bishop") == 1) && (g_tempMovingPiece.CompareTo("Black Pawn") == 1) && (dangerForMate == true) && ((startingBlKingRank - 1 + 1) <= 7) && ((startingBlKingColumn - 1 - 1) >= 0)) {

                     /* Temporarily moving the king up-left to check whether it remains in check 
                        The verification is done only if there is no other piece of the same colour in the square to block it
                        and the king doesnt leave the chessboard through movement */
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = "";
                        BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1 + 1)] = g_movingPiece;
                        blackKingCheck = checkForBlackCheck(BMBoard);

                        if(blackKingCheck == false)
                            dangerForMate = false;

                        // Reset the board to the state it was in before the king moved temporarily for checks
                        BMBoard[(startingBlKingColumn - 1), (startingBlKingRank - 1)] = g_movingPiece;
                        BMBoard[(startingBlKingColumn - 1 - 1), (startingBlKingRank - 1 + 1)] = g_tempMovingPiece;

                    }

                }

                if(dangerForMate == true)
                    mate = true;

            }

            return mate;
        }

        public static void pawnPromotion() {
            for(i = 0; i <= 7; i++) {
                if((Board[(i), (0)].CompareTo("Black Pawn") == 0) && (g_WhoPlays.CompareTo("Human") == 0)) {
        
                    // promote pawn
                    Console.WriteLine("Promote to: 1. Queen, 2. Rook, 3. Knight, 4. Bishop? ");
                    user_choice = Int32.Parse(Console.ReadLine());

                    switch(user_choice) {
                        case 1:
                            Board[(i), (0)] = "Black Queen";
                            break;
                        case 2:
                            Board[(i), (0)] = "Black Rook";
                            break;
                        case 3:
                            Board[(i), (0)] = "Black Knight";
                            break;
                        case 4:
                            Board[(i), (0)] = "Black Bishop";
                            break;
                    };

                }

                if((Board[(i), (7)].CompareTo("White Pawn") == 0) && (g_WhoPlays.CompareTo("Human") == 0)) {

                    // promote pawn
                    Console.WriteLine("Promote to: 1. Queen, 2. Rook, 3. Knight, 4. Bishop? ");
                    user_choice = Int32.Parse(Console.ReadLine());

                    switch(user_choice) {
                        case 1:
                            Board[(i), (7)] = "White Queen";
                            break;

                        case 2:
                            Board[(i), (7)] = "White Rook";
                            break;

                        case 3:
                            Board[(i), (7)] = "White Knight";
                            break;

                        case 4:
                            Board[(i), (7)] = "White Bishop";
                            break;
                    };
                }
            }
        }

        //STUDY
        public static void computerMove(string[,] boardThinkingInit) {
            /* Uncomment to see the computer thinking */
            #region WriteLog
            //Console.WriteLine("");
            //Console.WriteLine("CompMove -- Entered ComputerMove");
            //Console.WriteLine(string.Concat("CompMove -- Depth analysed: ", moveAnalysed.ToString()));
            //Console.WriteLine(string.Concat("CompMove -- Number of moves analysed: ", number_of_moves_analysed.ToString()));
            //Console.WriteLine(string.Concat("CompMove -- Move analysed: ", g_StartingColumnNumber_AI.ToString(), g_StartingRank_AI.ToString(), " -> ", g_FinishingColumnNumber_AI.ToString(), g_FinishingRank_AI.ToString()));
            //Console.WriteLine(string.Concat("CompMove -- Number of Nodes 0: ", nodeLevel_0_count.ToString()));
            //Console.WriteLine(string.Concat("CompMove -- Number of Nodes 1: ", nodeLevel_1_count.ToString()));
            //Console.WriteLine(string.Concat("CompMove -- Number of Nodes 2: ", nodeLevel_2_count.ToString()));
            //Console.WriteLine("");
            #endregion WriteLog

            int iii;
            int jjj;
            String MovingPiece0;
            String g_tempMovingPiece0;
            int g_StartingColumnNumber0;
            int g_FinishingColumnNumber0;
            int g_StartingRank0;
            int g_FinishingRank0;
            // If there is a possibility to eat back what was eaten then go for it
            possibilityToEatBack = false;

            #region InitializeNodes
            // START [MiniMax algorithm]
            nodeLevel_0_count = 0;
            nodeLevel_1_count = 0;
            nodeLevel_2_count = 0;

            for(int dimension1 = 0; dimension1 < 1000000; dimension1++) {
                for(int dimension2 = 0; dimension2 < 6; dimension2++) {
                    NodesAnalysis0[dimension1, dimension2] = 0;
                }
            }

            for(int dimension1 = 0; dimension1 < 1000000; dimension1++) {
                for(int dimension2 = 0; dimension2 < 2; dimension2++) {
                    NodesAnalysis1[dimension1, dimension2] = 0;
                    NodesAnalysis2[dimension1, dimension2] = 0;
                }
            }
            #endregion InitializeNodes

            #region StoreInitialPosition
            // Store the initial position in the chessboard
            for(iii = 0; iii <= 7; iii++) {
                for(jjj = 0; jjj <= 7; jjj++) {
                    BoardThinking[iii, jjj] = boardThinkingInit[(iii), (jjj)];
                    BoardMove0[(iii), (jjj)] = boardThinkingInit[(iii), (jjj)];
                }
            }
            #endregion StoreInitialPosition

            // Check for dangerous squares
            // Also find number and value of attackers and defenders for each square of the chessboard: will be used below to determine if the move is stupid
            #region DangerousSquares
            dangerForPiece = false;
            
            //Clear board for evaluation
            for(int o1 = 0; o1 <= 7; o1++) {
                for(int p1 = 0; p1 <= 7; p1++) {
                    BoardDangerSquares[(o1), (p1)] = 0;
                }
            }

            // Find attackers-defenders
            findAttackers(BoardThinking);
            findDefenders(BoardThinking);

            // Determine dangerous squares
            for(int o1 = 0; o1 <= 7; o1++) {
                for(int p1 = 0; p1 <= 7; p1++) {
                    if(numberOfAttackers[o1, p1] > numberOfDefenders[o1, p1])
                        BoardDangerSquares[(o1), (p1)] = 1;
                }
            }
            #endregion DangerousSquares


            //---------------------------------------
            // Check all possible moves
            //---------------------------------------
            for(iii = 0; iii <= 7; iii++) {
                for(jjj = 0; jjj <= 7; jjj++) {
                    if(((whoIsAnalysed.CompareTo("AI") == 0) && ((((BoardThinking[(iii), (jjj)].CompareTo("White King") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Queen") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Rook") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Knight") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Bishop") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Pawn") == 0)) && (g_PlayerColour.CompareTo("Black") == 0)) || (((BoardThinking[(iii), (jjj)].CompareTo("Black King") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Queen") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Rook") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Knight") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Bishop") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Pawn") == 0)) && (g_PlayerColour.CompareTo("White") == 0)))) || ((whoIsAnalysed.CompareTo("Human") == 0) && ((((BoardThinking[(iii), (jjj)].CompareTo("White King") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Queen") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Rook") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Knight") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Bishop") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("White Pawn") == 0)) && (g_PlayerColour.CompareTo("White") == 0)) || (((BoardThinking[(iii), (jjj)].CompareTo("Black King") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Queen") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Rook") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Knight") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Bishop") == 0) || (BoardThinking[(iii), (jjj)].CompareTo("Black Pawn") == 0)) && (g_PlayerColour.CompareTo("Black") == 0))))) {

                        for(int w = 0; w <= 7; w++) {
                            for(int r = 0; r <= 7; r++) {

                                g_movingPiece = BoardThinking[(iii), (jjj)];
                                g_StartingColumnNumber = iii + 1;
                                g_FinishingColumnNumber = w + 1;
                                g_StartingRank = jjj + 1;
                                g_FinishingRank = r + 1;

                                // Store temporary move data in local variables, to use them in the Undo of the move  at the end of this function (the g_movingPiece, g_StartingColumnNumber, etc variables are changed by next functions as well, so using them leads to problems)
                                MovingPiece0 = g_movingPiece;
                                g_StartingColumnNumber0 = g_StartingColumnNumber;
                                g_FinishingColumnNumber0 = g_FinishingColumnNumber;
                                g_StartingRank0 = g_StartingRank;
                                g_FinishingRank0 = g_FinishingRank;
                                g_tempMovingPiece0 = BoardThinking[(g_FinishingColumnNumber0 - 1), (g_FinishingRank0 - 1)];

                                // Check for stupid moves in the start of the game
                                String doNotMakeStupidMove = "N";
                                #region CheckStupidMove
                                if(move < 5) {
                                    if((g_movingPiece.CompareTo("White Queen") == 0) || (g_movingPiece.CompareTo("Black Queen") == 0) ||
                                            (g_movingPiece.CompareTo("White Rook") == 0) || (g_movingPiece.CompareTo("Black Rook") == 0)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if(((g_movingPiece.CompareTo("White Knight") == 0) || (g_movingPiece.CompareTo("Black Knight") == 0))
                                              && (g_FinishingColumnNumber == 1)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if(((g_movingPiece.CompareTo("White Knight") == 0) || (g_movingPiece.CompareTo("Black Knight") == 0))
                                              && (g_FinishingColumnNumber == 8)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("White Knight") == 0) && (g_FinishingRank == 2) && (g_FinishingColumnNumber == 4)
                                              && (BoardThinking[(2), (0)].CompareTo("White Bishop") == 0)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("White Knight") == 0) && (g_FinishingRank == 2) && (g_FinishingColumnNumber == 5)
                                              && (BoardThinking[(5), (0)].CompareTo("White Bishop") == 0)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("Black Knight") == 0) && (g_FinishingRank == 7) && (g_FinishingColumnNumber == 4)
                                              && (BoardThinking[(2), (7)].CompareTo("Black Bishop") == 0)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("Black Knight") == 0) && (g_FinishingRank == 7) && (g_FinishingColumnNumber == 5)
                                              && (BoardThinking[(5), (7)].CompareTo("Black Bishop") == 0)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("White Pawn") == 0) && ((g_StartingColumnNumber == 1) || (g_StartingColumnNumber == 2))) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("White Pawn") == 0) && ((g_StartingColumnNumber == 7) || (g_StartingColumnNumber == 8))) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("Black Pawn") == 0) && ((g_StartingColumnNumber == 1) || (g_StartingColumnNumber == 2))) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("Black Pawn") == 0) && ((g_StartingColumnNumber == 7) || (g_StartingColumnNumber == 8))) {
                                        doNotMakeStupidMove = "Y";
                                    } else if((g_movingPiece.CompareTo("White King") == 0) || (g_movingPiece.CompareTo("Black King") == 0)) {
                                        doNotMakeStupidMove = "Y";
                                    } else if(((g_movingPiece.CompareTo("White Bishop") == 0) || (g_movingPiece.CompareTo("Black Bishop") == 0))
                                          && ((g_FinishingRank == 3) || (g_FinishingRank == 5) || (g_FinishingRank == 6))) {
                                        doNotMakeStupidMove = "Y";
                                    }
                                }
                                #endregion CheckStupidMove

                                // Store the value of the moving piece
                                if((g_movingPiece.CompareTo("White Rook") == 0) || (g_movingPiece.CompareTo("Black Rook") == 0))
                                    valueOfMovingPiece = 5;
                                if((g_movingPiece.CompareTo("White Knight") == 0) || (g_movingPiece.CompareTo("Black Knight") == 0))
                                    valueOfMovingPiece = 3;
                                if((g_movingPiece.CompareTo("White Bishop") == 0) || (g_movingPiece.CompareTo("Black Bishop") == 0))
                                    valueOfMovingPiece = 3;
                                if((g_movingPiece.CompareTo("White Queen") == 0) || (g_movingPiece.CompareTo("Black Queen") == 0))
                                    valueOfMovingPiece = 9;
                                if((g_movingPiece.CompareTo("White King") == 0) || (g_movingPiece.CompareTo("Black King") == 0))
                                    valueOfMovingPiece = 15;
                                if((g_movingPiece.CompareTo("White Pawn") == 0) || (g_movingPiece.CompareTo("Black Pawn") == 0))
                                    valueOfMovingPiece = 1;

                                // If a piece of lower value attacks the square where the computer moves, then stupid move
                                if((numberOfAttackers[w, r] == 1) && (valueOfAttackers[w, r] < valueOfMovingPiece))
                                    doNotMakeStupidMove = "Y";


                                //If the move is not stupid or the destination square is not dangerous then do the move and check it
                                if((doNotMakeStupidMove.CompareTo("N") == 0) && (BoardDangerSquares[w, r] == 0)) {
                                    
                                    /* The core of thinking: Here the computer checks the moves
                                       Validity and legality of the move will be checked in CheckMove
                                       (plus some additional checks for possible mate etc) */
                                    checkMove(BoardThinking, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);

                                    // If everything is OK, then do the move and measure it's score
                                    if((g_Correctness == true) && (g_Legal == true)) {
                                        // Do the move
                                        g_tempMovingPiece = BoardThinking[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)];
                                        BoardThinking[(g_StartingColumnNumber - 1), (g_StartingRank - 1)] = "";
                                        BoardThinking[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)] = g_movingPiece;

                                        // Check the score after the computer move
                                        if(moveAnalysed == 0) {
                                            nodeLevel_0_count++;
                                            tempScoreMove_0 = countScore(BoardThinking);
                                        }

                                        //Check if you can eat back the Human piece that moved
                                        if((g_FinishingColumnNumber == humanLastMoveTargetColumn)
                                             && (g_FinishingRank == humanLastMoveTargetRow)
                                             && (valueOfMovingPiece <= valueOfHumanMovingPiece)) {
                                            bestMoveStartingColumnNumber = g_StartingColumnNumber;
                                            bestMoveStartingRank = g_StartingRank;
                                            bestMoveFinishingColumnNumber = g_FinishingColumnNumber;
                                            bestMoveFinishingRank = g_FinishingRank;

                                            possibilityToEatBack = true;
                                        }


                                        // If you can eat back the Human piece, go for it and don't analyse
                                        if((moveAnalysed < thinkingDepth) && (possibilityToEatBack == false)) {
                                            moveAnalysed = moveAnalysed + 1;

                                            //Tried to remove and pass over Board to Analyse_Move_1_HumanMove,
                                            //but the result changed. Must check it. It should be the same.
                                            //The problem is probably that BoardThinking is used/referenced somewhere else in the program and its values are distorted later on.
                                            for(i = 0; i <= 7; i++) {
                                                for(j = 0; j <= 7; j++) {
                                                    BoardMoveAfter[(i), (j)] = BoardThinking[(i), (j)];
                                                }
                                            }

                                            whoIsAnalysed = "Human";

                                            // Check Human moves (to find the best possible move the player can make)
                                            // to the move currently analysed by the AI Thought process)
                                            // I will always have to call the next level of thought at this point
                                            analyseMove1HumanMove(BoardMoveAfter);
                                        }

                                        // Undo the move
                                        BoardThinking[(g_StartingColumnNumber0 - 1), (g_StartingRank0 - 1)] = MovingPiece0;
                                        BoardThinking[(g_FinishingColumnNumber0 - 1), (g_FinishingRank0 - 1)] = g_tempMovingPiece0;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if(((whiteKingCheck == true) || (blackKingCheck == true)) && (Best_Move_Found == false)) {
                Console.WriteLine("Checkmate!");
            }

            // Do the best move found
            // Analyse only if possibility to eat back is not true
            if(possibilityToEatBack == false) {
                // [MiniMax algorithm]
                // Find the node 1 move with the best score via the MiniMax algorithm.
                int counter0, counter1, counter2;

                // ------------------------------------------------------
                // NodesAnalysis
                // ------------------------------------------------------
                // Nodes structure...
                // [ccc, xxx, 0]: Score of node No. ccc at level xxx
                // [ccc, xxx, 1]: Parent of node No. ccc at level xxx-1
                // ------------------------------------------------------

                int parentNodeAnalysed = -999;

                for(counter2 = 1; counter2 <= nodeLevel_2_count; counter2++) {
                    if(Int32.Parse(NodesAnalysis2[counter2, 1].ToString()) != parentNodeAnalysed) {
                        parentNodeAnalysed = Int32.Parse(NodesAnalysis2[counter2, 1].ToString());
                        NodesAnalysis1[Int32.Parse(NodesAnalysis2[counter2, 1].ToString()), 0] = NodesAnalysis2[counter2, 0];
                    }

                    if(NodesAnalysis2[counter2, 0] >= NodesAnalysis1[Int32.Parse(NodesAnalysis2[counter2, 1].ToString()), 0])
                        NodesAnalysis1[Int32.Parse(NodesAnalysis2[counter2, 1].ToString()), 0] = NodesAnalysis2[counter2, 0];
                }

                // Now the node0 level is filled with the score data
                // this is line 1 in the shape at http://upload.wikimedia.org/wikipedia/commons/thumb/6/6f/Minimax.svg/300px-Minimax.svg.png

                parentNodeAnalysed = -999;

                for(counter1 = 1; counter1 <= nodeLevel_1_count; counter1++) {
                    if(Int32.Parse(NodesAnalysis1[counter1, 1].ToString()) != parentNodeAnalysed) {
                        parentNodeAnalysed = Int32.Parse(NodesAnalysis1[counter1, 1].ToString());
                        NodesAnalysis0[Int32.Parse(NodesAnalysis1[counter1, 1].ToString()), 0] = NodesAnalysis1[counter1, 0];
                    }

                    if(NodesAnalysis1[counter1, 0] <= NodesAnalysis0[Int32.Parse(NodesAnalysis1[counter1, 1].ToString()), 0])
                        NodesAnalysis0[Int32.Parse(NodesAnalysis1[counter1, 1].ToString()), 0] = NodesAnalysis1[counter1, 0];
                }

                // Choose the biggest score at the Node0 level
                // Check example at http://en.wikipedia.org/wiki/Minimax#Example_2
                // This is line 0 at the shape at http://upload.wikimedia.org/wikipedia/commons/thumb/6/6f/Minimax.svg/300px-Minimax.svg.png

                // Initialize the score with the first score and move found
                double temp_score = NodesAnalysis0[1, 0];
                bestMoveStartingColumnNumber = Int32.Parse(NodesAnalysis0[1, 2].ToString());
                bestMoveStartingRank = Int32.Parse(NodesAnalysis0[1, 4].ToString());
                bestMoveFinishingColumnNumber = Int32.Parse(NodesAnalysis0[1, 3].ToString());
                bestMoveFinishingRank = Int32.Parse(NodesAnalysis0[1, 5].ToString());

                for(counter0 = 1; counter0 <= nodeLevel_0_count; counter0++) {
                    if(NodesAnalysis0[counter0, 0] > temp_score) {
                        temp_score = NodesAnalysis0[counter0, 0];

                        bestMoveStartingColumnNumber = Int32.Parse(NodesAnalysis0[counter0, 2].ToString());
                        bestMoveStartingRank = Int32.Parse(NodesAnalysis0[counter0, 4].ToString());
                        bestMoveFinishingColumnNumber = Int32.Parse(NodesAnalysis0[counter0, 3].ToString());
                        bestMoveFinishingRank = Int32.Parse(NodesAnalysis0[counter0, 5].ToString());
                    }
                }
            }

            //If no move found => Resign. If best move found => OK. Go do it.
            if(bestMoveStartingColumnNumber > 0) {
                g_movingPiece = Board[(bestMoveStartingColumnNumber - 1), (bestMoveStartingRank - 1)];
                Board[(bestMoveStartingColumnNumber - 1), (bestMoveStartingRank - 1)] = "";

                if(bestMoveStartingColumnNumber == 1)
                    AIStartingColumnText = "a";
                else if(bestMoveStartingColumnNumber == 2)
                    AIStartingColumnText = "b";
                else if(bestMoveStartingColumnNumber == 3)
                    AIStartingColumnText = "c";
                else if(bestMoveStartingColumnNumber == 4)
                    AIStartingColumnText = "d";
                else if(bestMoveStartingColumnNumber == 5)
                    AIStartingColumnText = "e";
                else if(bestMoveStartingColumnNumber == 6)
                    AIStartingColumnText = "f";
                else if(bestMoveStartingColumnNumber == 7)
                    AIStartingColumnText = "g";
                else if(bestMoveStartingColumnNumber == 8)
                    AIStartingColumnText = "h";

                if(bestMoveFinishingColumnNumber == 1)
                    AIFinishingColumnText = "a";
                else if(bestMoveFinishingColumnNumber == 2)
                    AIFinishingColumnText = "b";
                else if(bestMoveFinishingColumnNumber == 3)
                    AIFinishingColumnText = "c";
                else if(bestMoveFinishingColumnNumber == 4)
                    AIFinishingColumnText = "d";
                else if(bestMoveFinishingColumnNumber == 5)
                    AIFinishingColumnText = "e";
                else if(bestMoveFinishingColumnNumber == 6)
                    AIFinishingColumnText = "f";
                else if(bestMoveFinishingColumnNumber == 7)
                    AIFinishingColumnText = "g";
                else if(bestMoveFinishingColumnNumber == 8)
                    AIFinishingColumnText = "h";

                // Position the piece to the square of destination

                Board[(bestMoveFinishingColumnNumber - 1), (bestMoveFinishingRank - 1)] = g_movingPiece;

                // Is there a pawn to promote? AI Automatically chooses Queen for promotion
                if((g_movingPiece.CompareTo("White Pawn") == 0) || (g_movingPiece.CompareTo("Black Pawn") == 0)) {
                        if(bestMoveFinishingRank == 8) {
                            Board[(bestMoveFinishingColumnNumber - 1), (bestMoveFinishingRank - 1)] = "White Queen";
                            Console.WriteLine("Queen!");
                        } else if(bestMoveFinishingRank == 1) {
                            Board[(bestMoveFinishingColumnNumber - 1), (bestMoveFinishingRank - 1)] = "Black Queen";
                            Console.WriteLine("Queen!");
                        }
                }

                // Show AI move
                Console.WriteLine(String.Concat("My move: ", AIStartingColumnText, bestMoveStartingRank.ToString(), " -> ", AIFinishingColumnText, bestMoveFinishingRank.ToString()));
                displayBoard(Board);

                // If the computer plays with white, then we increase the number of moves (in if statement)
                // Now it is the other color's turn to play
                if(g_PlayerColour.CompareTo("Black") == 0) {
                    g_WhichColorPlays = "Black";
                    move += 1;
                } else if(g_PlayerColour.CompareTo("White") == 0)
                    g_WhichColorPlays = "White";

                // Now it is the Human's turn to play
                g_WhoPlays = "Human";

                }
                //If no move found => Resign
            else {
                Console.WriteLine("I resign");
            }
        }

        public static void displayBoard(string[,] drawBoard) {
            bool BoardColour = true;
            //False = Grey True = Dark Grey
            int RowCounter = 7;
            while(RowCounter > -1) {
                for(int ColumnCounter = 0; ColumnCounter <= 7; ColumnCounter++) {
                    Console.BackgroundColor = setBoardColour(ref BoardColour);
                    setPieceColour(drawBoard[ColumnCounter, RowCounter]);
                    Console.Write(" ");
                    Console.Write(getKey(drawBoard[ColumnCounter, RowCounter]));
                    Console.Write(" ");
                }
                Console.WriteLine("");
                Console.BackgroundColor = setBoardColour(ref BoardColour);
                //I do this to switch the colour at the end of the row
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                RowCounter -= 1;
            }
            // Return to the default letters colour
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static ConsoleColor setBoardColour(ref bool boardColour) {
            if(boardColour == false) {
                boardColour = true;
                return ConsoleColor.DarkGray;
            } else {
                boardColour = false;
                return ConsoleColor.Gray;
            }
        }

        public static void setPieceColour(string piece) {
            if(piece.CompareTo("") == 0) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            } else {
                if(piece.Substring(0, 5).CompareTo("White") == 0) { //Distinguishing between Black and White pieces
                    Console.ForegroundColor = ConsoleColor.White;
                } else {
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
        }

        public static string getKey(string piece) {
            if(piece.CompareTo("") == 0)
                return " ";
            if(piece.CompareTo("White Pawn") == 0)
                return "o";
            if(piece.CompareTo("White Rook") == 0)
                return "R";
            if(piece.CompareTo("White Bishop") == 0)
                return "B";
            if(piece.CompareTo("White King") == 0)
                return "K";
            if(piece.CompareTo("White Knight") == 0)
                return "N";
            if(piece.CompareTo("White Queen") == 0)
                return "Q";
            if(piece.CompareTo("Black Pawn") == 0)
                return "o";
            if(piece.CompareTo("Black Rook") == 0)
                return "R";
            if(piece.CompareTo("Black Bishop") == 0)
                return "B";
            if(piece.CompareTo("Black King") == 0)
                return "K";
            if(piece.CompareTo("Black Knight") == 0)
                return "N";
            if(piece.CompareTo("Black Queen") == 0)
                return "Q";
            return piece;
        }

        //STUDY
        public static void analyseMove1HumanMove(string[,] boardHumanThinking_2) {
            /* Uncomment to see the computer thinking */
            #region WriteLog
        Console.WriteLine("");
        Console.WriteLine("HumanMove -- Entered HumanMove Template 2");
        Console.WriteLine(string.Concat("HumanMove -- Depth analysed: ", moveAnalysed.ToString()));
        Console.WriteLine(string.Concat("HumanMove -- Number of moves analysed: ", number_of_moves_analysed.ToString()));
        Console.WriteLine(string.Concat("HumanMove -- Move analysed: ", g_StartingColumnNumber_AI.ToString(), g_StartingRank_AI.ToString(), " -> ", g_FinishingColumnNumber_AI.ToString(), g_FinishingRank_AI.ToString()));
        Console.WriteLine(string.Concat("HumanMove -- Number of Nodes 0: ", nodeLevel_0_count.ToString()));
        Console.WriteLine(string.Concat("HumanMove -- Number of Nodes 1: ", nodeLevel_1_count.ToString()));
        Console.WriteLine(string.Concat("HumanMove -- Number of Nodes 2: ", nodeLevel_2_count.ToString()));
        Console.WriteLine("");
            #endregion WriteLog

            /* Scan chessboard -> Find a human player piece -> Move to all possible squares
               Check correctness and legality of move -> If all OK then measure the move's score
               Do the best move and use the ComputerMove function to further analyse the next move (more depth) */
            int p;
            int q;
            String movingPiece1;
            String g_tempMoveData1;
            int startingColumnNumber1;
            int finishingColumnNumber1;
            int startingRank1;
            int finishingRank1;

            // Check all possible moves
            for(p = 0; p <= 7; p++) {
                for(q = 0; q <= 7; q++) {
                    //If the human is playing black AND the square contains a black piece OR human is playing white AND square contains a white piece
                    if(((whoIsAnalysed.CompareTo("Human") == 0) && ((((boardHumanThinking_2[(p), (q)].CompareTo("Black King") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("Black Queen") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("Black Rook") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("Black Knight") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("Black Bishop") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("Black Pawn") == 0)) && (g_PlayerColour.CompareTo("Black") == 0)) || (((boardHumanThinking_2[(p), (q)].CompareTo("White King") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("White Queen") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("White Rook") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("White Knight") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("White Bishop") == 0) || (boardHumanThinking_2[(p), (q)].CompareTo("White Pawn") == 0)) && (g_PlayerColour.CompareTo("White") == 0))))) {
                        for(int w = 0; w <= 7; w++) {
                            for(int r = 0; r <= 7; r++) {
                                g_movingPiece = boardHumanThinking_2[(p), (q)];
                                g_StartingColumnNumber = p + 1;
                                g_FinishingColumnNumber = w + 1;
                                g_StartingRank = q + 1;
                                g_FinishingRank = r + 1;

                                /* Store temporary move data in local variables, so I can use them in the Undo of the move
                                   at the end of this function (the g_movingPiece, g_StartingColumnNumber, etc variables are
                                   changed by next functions as well, so using them leads to problems) */
                                movingPiece1 = g_movingPiece;
                                startingColumnNumber1 = g_StartingColumnNumber;
                                finishingColumnNumber1 = g_FinishingColumnNumber;
                                startingRank1 = g_StartingRank;
                                finishingRank1 = g_FinishingRank;
                                g_tempMoveData1 = boardHumanThinking_2[(finishingColumnNumber1 - 1), (finishingRank1 - 1)];

                                // Check the move
                                /* Necessary values for variables for the correctness check and
                                   legality check functions to function properly */
                                g_WhoPlays = "Human";
                                g_WrongColumn = false;
                                g_movingPiece = boardHumanThinking_2[(g_StartingColumnNumber - 1), (g_StartingRank - 1)]; // Same statement as above after search - not needed (specifies move to check)
                                g_Correctness = correctnessCheck(boardHumanThinking_2, 0, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);
                                g_Legal = legalityCheck(boardHumanThinking_2, 0, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);
                                // Restore normal value of g_WhoPlays
                                g_WhoPlays = "AI";

                                // If all is ok, then do the move and measure it
                                if((g_Correctness == true) && (g_Legal == true)) {
                                    // Do the move
                                    g_tempMovingPiece = boardHumanThinking_2[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)];
                                    boardHumanThinking_2[(g_StartingColumnNumber - 1), (g_StartingRank - 1)] = "";
                                    boardHumanThinking_2[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)] = g_movingPiece;

                                    // Measure score AFTER the move
                                    if(moveAnalysed == 1) {
                                        nodeLevel_1_count++;
                                        tempScoreMove_1_Human = countScore(boardHumanThinking_2);
                                    }

                                    if(moveAnalysed < thinkingDepth) {
                                        // Call ComputerMove for the AI throught process to continue
                                        moveAnalysed += 1;

                                        whoIsAnalysed = "AI";

                                        for(i = 0; i <= 7; i++) {
                                            for(j = 0; j <= 7; j++) {
                                                BoardMoveAfter[(i), (j)] = boardHumanThinking_2[(i), (j)];
                                            }
                                        }

                                        if(moveAnalysed == 2)
                                            analyseMove2ComputerMove(BoardMoveAfter);
                                    }

                                    // Undo the move
                                    boardHumanThinking_2[(startingColumnNumber1 - 1), (startingRank1 - 1)] = movingPiece1;
                                    boardHumanThinking_2[(finishingColumnNumber1 - 1), (finishingRank1 - 1)] = g_tempMoveData1;
                                }
                            }
                        }
                    }
                }
            }

            moveAnalysed = moveAnalysed - 1;
            whoIsAnalysed = "AI";
        }

        //STUDY
        public static void analyseMove2ComputerMove(string[,] boardThinkingAI_2) {
        /* Uncomment to see the computer thinking */
        #region WriteLog
        //Console.WriteLine("");
        //Console.WriteLine("CompMove2 -- Entered ComputerMove Template 2");
        //Console.WriteLine(string.Concat("CompMove2 -- Depth analysed: ", moveAnalysed.ToString()));
        //Console.WriteLine(string.Concat("CompMove2 -- Number of moves analysed: ", number_of_moves_analysed.ToString()));
        //Console.WriteLine(string.Concat("CompMove2 -- Move analysed: ", g_StartingColumnNumber_AI.ToString(), g_StartingRank_AI.ToString(), " -> ", g_FinishingColumnNumber_AI.ToString(), g_FinishingRank_AI.ToString()));
        //Console.WriteLine(string.Concat("CompMove2 -- Number of Nodes 0: ", nodeLevel_0_count.ToString()));
        //Console.WriteLine(string.Concat("CompMove2 -- Number of Nodes 1: ", nodeLevel_1_count.ToString()));
        //Console.WriteLine(string.Concat("CompMove2 -- Number of Nodes 2: ", nodeLevel_2_count.ToString()));
        //Console.WriteLine("");
        #endregion WriteLog

        // Declaration of variables used in the "for" loop (the variables i and j can't be used because they are global and there is a problem with returning ComputerMove from CheckMove)

        int p2;
        int q2;
        String movingPiece2;
        String g_tempMoveData2;
        int g_StartingColumnNumber2;
        int g_FinishingColumnNumber2;
        int g_StartingRank2;
        int g_FinishingRank2;

        /* Scanning the board : Where there is an AI piece, 
         * They will count ALL the possible moves in any direction, regardless of correctness or legality
         * Then with the help of the legality correctness check functions will be checked
         * If the movement is correct and legal, the piece will be
         * temporarily moved on the board and the score of the new position will be evaluated */

        // NOTE : All columns and horizontal unit are increased by 1
        // This is because it must be converted from the 0-7 measurement (used in the following "for i next" and at the Board table notation) to the 1-8 measurement which is used in the starting/finishingColumn/Rank variables throughout the rest of the program 

        for(p2 = 0; p2 <= 7; p2++) {
            for(q2 = 0; q2 <= 7; q2++) {

                if(((whoIsAnalysed.CompareTo("AI") == 0) && ((((boardThinkingAI_2[(p2), (q2)].CompareTo("White King") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("White Queen") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("White Rook") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("White Knight") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("White Bishop") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("White Pawn") == 0)) && (g_PlayerColour.CompareTo("Black") == 0)) || (((boardThinkingAI_2[(p2), (q2)].CompareTo("Black King") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("Black Queen") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("Black Rook") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("Black Knight") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("Black Bishop") == 0) || (boardThinkingAI_2[(p2), (q2)].CompareTo("Black Pawn") == 0)) && (g_PlayerColour.CompareTo("White") == 0))))) {

                    for(int w = 0; w <= 7; w++) {
                        for(int r = 0; r <= 7; r++) {
                            g_movingPiece = boardThinkingAI_2[(p2), (q2)];
                            g_StartingColumnNumber = p2 + 1;
                            g_FinishingColumnNumber = w + 1;
                            g_StartingRank = q2 + 1;
                            g_FinishingRank = r + 1;

                            // Store temporary move data in local variables, so we can undo the move at the end of this function (the MovingPiece, g_StartingColumnNumber, etc variables are
                            // changed by next functions as well, so using them leads to problems)
                            movingPiece2 = g_movingPiece;
                            g_StartingColumnNumber2 = g_StartingColumnNumber;
                            g_FinishingColumnNumber2 = g_FinishingColumnNumber;
                            g_StartingRank2 = g_StartingRank;
                            g_FinishingRank2 = g_FinishingRank;
                            g_tempMoveData2 = boardThinkingAI_2[(g_FinishingColumnNumber2 - 1), (g_FinishingRank2 - 1)];

                            // Check validity and legality
                            // Necessary values for variables for the correctness check and the legality check function to work properly
                            g_WhoPlays = "Human";
                            g_WrongColumn = false;
                            g_Correctness = correctnessCheck(boardThinkingAI_2, 0, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);
                            g_Legal = legalityCheck(boardThinkingAI_2, 0, g_StartingRank, g_StartingColumnNumber, g_FinishingRank, g_FinishingColumnNumber, g_movingPiece);
                            // restore normal value of g_WhoPlays
                            g_WhoPlays = "AI";

                            number_of_moves_analysed++;

                                // If all ok, then do the move and measure it
                                if((g_Correctness == true) && (g_Legal == true)) {

                                    // Do the move
                                    g_tempMovingPiece = boardThinkingAI_2[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)];
                                    boardThinkingAI_2[(g_StartingColumnNumber - 1), (g_StartingRank - 1)] = "";
                                    boardThinkingAI_2[(g_FinishingColumnNumber - 1), (g_FinishingRank - 1)] = g_movingPiece;

                                    // Check the score after the computer move
                                    if(moveAnalysed == 2) {
                                        nodeLevel_2_count++;
                                        tempScoreMove_2 = countScore(boardThinkingAI_2);
                                    }

                                    if(moveAnalysed == thinkingDepth) {
                                        // [MiniMax algorithm]
                                        // Record the node in the Nodes Analysis array (to use with MiniMax algorithm)
                                        NodesAnalysis0[nodeLevel_0_count, 0] = tempScoreMove_0;
                                        NodesAnalysis1[nodeLevel_1_count, 0] = tempScoreMove_1_Human;
                                        NodesAnalysis2[nodeLevel_2_count, 0] = tempScoreMove_2;

                                        // Store the parents (number of the node of the upper level)
                                        NodesAnalysis0[nodeLevel_0_count, 1] = 0;
                                        NodesAnalysis1[nodeLevel_1_count, 1] = nodeLevel_0_count;
                                        NodesAnalysis2[nodeLevel_2_count, 1] = nodeLevel_1_count;
                                    }

                                    // Undo the move
                                    boardThinkingAI_2[(g_StartingColumnNumber2 - 1), (g_StartingRank2 - 1)] = movingPiece2;
                                    boardThinkingAI_2[(g_FinishingColumnNumber2 - 1), (g_FinishingRank2 - 1)] = g_tempMoveData2;

                                }
                            }
                        }
                    }
                }
            }

            moveAnalysed = moveAnalysed - 1;
            whoIsAnalysed = "Human";
        }

        // AI Thought Process:
        // Depth 0 (moveAnalysed = 0): First half move analysed - First AI half move analysed
        // Depth 1 (moveAnalysed = 1): Second half move analysed - First Human half move analysed
        // Depth 2 (moveAnalysed = 2): Third half move analysed - Second AI half move analysed

        public static int countScore(string[,] CSBoard) {
            // Wh pieces: positive score
            // Bl pieces: negative score

            currentMoveScore = 0;

            for(i = 0; i <= 7; i++) {
                for(j = 0; j <= 7; j++) {
                    if(CSBoard[(i), (j)].CompareTo("White Pawn") == 0)
                        currentMoveScore = currentMoveScore + 1;
                    else if(CSBoard[(i), (j)].CompareTo("White Rook") == 0) {
                        currentMoveScore = currentMoveScore + 5;
                    } else if(CSBoard[(i), (j)].CompareTo("White Knight") == 0) {
                        currentMoveScore = currentMoveScore + 3;
                    } else if(CSBoard[(i), (j)].CompareTo("White Bishop") == 0) {
                        currentMoveScore = currentMoveScore + 3;
                    } else if(CSBoard[(i), (j)].CompareTo("White Queen") == 0) {
                        currentMoveScore = currentMoveScore + 9;
                    } else if(CSBoard[(i), (j)].CompareTo("White King") == 0)
                        currentMoveScore = currentMoveScore + 15;
                    else if(CSBoard[(i), (j)].CompareTo("Black Pawn") == 0)
                        currentMoveScore = currentMoveScore - 1;
                    else if(CSBoard[(i), (j)].CompareTo("Black Rook") == 0) {
                        currentMoveScore = currentMoveScore - 5;
                    } else if(CSBoard[(i), (j)].CompareTo("Black Knight") == 0) {
                        currentMoveScore = currentMoveScore - 3;
                    } else if(CSBoard[(i), (j)].CompareTo("Black Bishop") == 0) {
                        currentMoveScore = currentMoveScore - 3;
                    } else if(CSBoard[(i), (j)].CompareTo("Black Queen") == 0) {
                        currentMoveScore = currentMoveScore - 9;
                    } else if(CSBoard[(i), (j)].CompareTo("Black King") == 0)
                        currentMoveScore = currentMoveScore - 15;

                }
            }

            return currentMoveScore;
        }

        public static void findAttackers(string[,] BoardAttackers) {
            String MovingPiece_Attack;
            int g_StartingRank_Attack;
            int g_StartingColumnNumber_Attack;
            int g_FinishingRank_Attack;
            int g_FinishingColumnNumber_Attack;

            // Scan the chessboard, for each AI piece analyse all possible destinations in the chessboard. Check correctness and legality of the move analysed. If correct and legal, then do the move
            // In all column and rank numbers I add + 1, because I must transform them from the 0-7 measure system of the array table to the 1-8 measure system of the chessboard
            //If the piece is white and the human is white
            for(int iii2 = 0; iii2 <= 7; iii2++) { // scan
                for(int jjj2 = 0; jjj2 <= 7; jjj2++) {
                    if((((BoardAttackers[(iii2), (jjj2)].CompareTo("White King") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("White Queen") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("White Rook") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("White Knight") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("White Bishop") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("White Pawn") == 0)) && (g_PlayerColour.CompareTo("White") == 0)) || (((BoardAttackers[(iii2), (jjj2)].CompareTo("Black King") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("Black Queen") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("Black Rook") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("Black Knight") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("Black Bishop") == 0) || (BoardAttackers[(iii2), (jjj2)].CompareTo("Black Pawn") == 0)) && (g_PlayerColour.CompareTo("Black") == 0))) { // filter

                        MovingPiece_Attack = BoardAttackers[(iii2), (jjj2)];
                        g_StartingColumnNumber_Attack = iii2 + 1;
                        g_StartingRank_Attack = jjj2 + 1;

                        // find squares where the Human opponent can hit
                        for(int w2 = 0; w2 <= 7; w2++) { // scan
                            for(int r2 = 0; r2 <= 7; r2++) {
                                g_FinishingColumnNumber_Attack = w2 + 1;
                                g_FinishingRank_Attack = r2 + 1;

                                // check the move - filter
                                g_WhoPlays = "Human";
                                g_WrongColumn = false;
                                //Pass in the piece in question, its position, a destination, and deterimine the direction of movement base on going through all legal moves and matching the ones that fit
                                g_Correctness = correctnessCheck(Board, 1, g_StartingRank_Attack, g_StartingColumnNumber_Attack, g_FinishingRank_Attack, g_FinishingColumnNumber_Attack, MovingPiece_Attack);
                                if(g_Correctness == true) {
                                    g_Legal = legalityCheck(Board, 1, g_StartingRank_Attack, g_StartingColumnNumber_Attack, g_FinishingRank_Attack, g_FinishingColumnNumber_Attack, MovingPiece_Attack);
                                }
                                // restore normal value of g_WhoPlays
                                g_WhoPlays = "AI";

                                // If a pawn is moving, then take into account only moves of eating other pieces and not forward moves
                                if((MovingPiece_Attack.CompareTo("White Pawn") == 0) || (MovingPiece_Attack.CompareTo("Black Pawn") == 0)) {
                                    if(g_FinishingColumnNumber_Attack == g_StartingColumnNumber_Attack) {
                                        g_Correctness = false;
                                    }
                                }

                                if((g_Correctness == true) && (g_Legal == true)) {
                                    // Attacker on that square found
                                    numberOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = numberOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 1;
        
                                    // Calculate the value (total value) of the attackers
                                    if((MovingPiece_Attack.CompareTo("White Rook") == 0) || (MovingPiece_Attack.CompareTo("Black Rook") == 0))
                                        valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 5;
                                    else if((MovingPiece_Attack.CompareTo("White Bishop") == 0) || (MovingPiece_Attack.CompareTo("Black Bishop") == 0))
                                        valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 3;
                                    else if((MovingPiece_Attack.CompareTo("White Knight") == 0) || (MovingPiece_Attack.CompareTo("Black Knight") == 0))
                                        valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 3;
                                    else if((MovingPiece_Attack.CompareTo("White Queen") == 0) || (MovingPiece_Attack.CompareTo("Black Queen") == 0))
                                        valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 9;
                                    else if((MovingPiece_Attack.CompareTo("White Pawn") == 0) || (MovingPiece_Attack.CompareTo("Black Pawn") == 0))
                                        valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfAttackers[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void findDefenders(string[,] BoardDefenders) {
            String MovingPiece_Defend;
            int g_StartingRank_Attack;
            int g_StartingColumnNumber_Attack;
            int g_FinishingRank_Attack;
            int g_FinishingColumnNumber_Attack;

            // Find squares that are also protected by an AI piece
            // If protected, then the square is not really dangerous

            // Initialize all variables used to find exceptions in the non-dangerous squares.
            // Exceptions: If the Human can hit a square and the computer defends it with its pieces, then the
            // square is not dangerous. However, if the computer has only one piece to defend that square, then
            // it cannot move that specific piece to that square (because then the square would have no defenders and
            // would become again a dangerous square)

            for(int iii3 = 0; iii3 <= 7; iii3++) {
                for(int jjj3 = 0; jjj3 <= 7; jjj3++) {
                    if((((BoardDefenders[(iii3), (jjj3)].CompareTo("White King") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("White Queen") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("White Rook") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("White Knight") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("White Bishop") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("White Pawn") == 0)) && (g_PlayerColour.CompareTo("Black") == 0)) || (((BoardDefenders[(iii3), (jjj3)].CompareTo("Black King") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("Black Queen") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("Black Rook") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("Black Knight") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("Black Bishop") == 0) || (BoardDefenders[(iii3), (jjj3)].CompareTo("Black Pawn") == 0)) && (g_PlayerColour.CompareTo("White") == 0))) {

                        MovingPiece_Defend = BoardDefenders[(iii3), (jjj3)];
                        g_StartingColumnNumber_Attack = iii3 + 1;
                        g_StartingRank_Attack = jjj3 + 1;

                        for(int w1 = 0; w1 <= 7; w1++) {
                            for(int r1 = 0; r1 <= 7; r1++) {

                                g_FinishingColumnNumber_Attack = w1 + 1;
                                g_FinishingRank_Attack = r1 + 1;

                                // create g_WhoPlays and g_WrongColumn variables , to properly execute a correctness and legality check
                                g_WhoPlays = "Human";
                                g_WrongColumn = false;
                                g_Correctness = correctnessCheck(BoardDefenders, 1, g_StartingRank_Attack, g_StartingColumnNumber_Attack, g_FinishingRank_Attack, g_FinishingColumnNumber_Attack, MovingPiece_Defend);
                                if(g_Correctness == true) {
                                g_Legal = legalityCheck(BoardDefenders, 1, g_StartingRank_Attack, g_StartingColumnNumber_Attack, g_FinishingRank_Attack, g_FinishingColumnNumber_Attack, MovingPiece_Defend);
                                }
                                // restore normal value of g_WhoPlays
                                g_WhoPlays = "AI";

                                // You can count for all moves that "defend" a square except the move of a pawn forward
                                if((MovingPiece_Defend.CompareTo("White Pawn") == 0) || (MovingPiece_Defend.CompareTo("Black Pawn") == 0)) {
                                    if(g_FinishingColumnNumber_Attack == g_StartingColumnNumber_Attack) {
                                        g_Correctness = false;
                                    }
                                }

                                g_WhoPlays = "AI";
                                if((g_Correctness == true) && (g_Legal == true)) {
                                    // A new defender for that square is found
                                    numberOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = numberOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 1;

                                    // Calculate the value (total value) of the defenders
                                    if((MovingPiece_Defend.CompareTo("White Rook") == 0) || (MovingPiece_Defend.CompareTo("Black Rook") == 0))
                                        valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 5;
                                    else if((MovingPiece_Defend.CompareTo("White Bishop") == 0) || (MovingPiece_Defend.CompareTo("Black Bishop") == 0))
                                        valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 3;
                                    else if((MovingPiece_Defend.CompareTo("White Knight") == 0) || (MovingPiece_Defend.CompareTo("Black Knight") == 0))
                                        valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 3;
                                    else if((MovingPiece_Defend.CompareTo("White Queen") == 0) || (MovingPiece_Defend.CompareTo("Black Queen") == 0))
                                        valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 9;
                                    else if((MovingPiece_Defend.CompareTo("White Pawn") == 0) || (MovingPiece_Defend.CompareTo("Black Pawn") == 0))
                                        valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 1;
                                    else if((MovingPiece_Defend.CompareTo("White King") == 0) || (MovingPiece_Defend.CompareTo("Black King") == 0))
                                        valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] = valueOfDefenders[(g_FinishingColumnNumber_Attack - 1), (g_FinishingRank_Attack - 1)] + 15;
                                }
                            }
                        }
                    }
                }
            }
        }

        //STUDY
        public static void checkMove(string[,] CMBoard, int g_StartingRankCM, int g_StartingColumnNumberCM, int g_FinishingRankCM, int g_FinishingColumnNumberCM, String MovingPieceCM) {

            String tempCM;

            for(int iii = 0; iii <= 7; iii++) {
                for(int jjj = 0; jjj <= 7; jjj++) {
                    BoardCMCheck[iii, jjj] = CMBoard[(iii), (jjj)];
                }
            }

            g_WhoPlays = "Human";
            g_WrongColumn = false;

            // Check correctness of move
            g_Correctness = correctnessCheck(CMBoard, 0, g_StartingRankCM, g_StartingColumnNumberCM, g_FinishingRankCM, g_FinishingColumnNumberCM, MovingPieceCM);
            // if move is correct, then check the legality also
            if(g_Correctness == true)
                g_Legal = legalityCheck(CMBoard, 0, g_StartingRankCM, g_StartingColumnNumberCM, g_FinishingRankCM, g_FinishingColumnNumberCM, MovingPieceCM);

            // Restore the normal value of the g_WhoPlays
            g_WhoPlays = "AI";

            // Temporarily move the piece to see if the king will continue to be under check
            #region CheckCheck

            BoardCMCheck[(g_StartingColumnNumberCM - 1), (g_StartingRankCM - 1)] = "";
            tempCM = BoardCMCheck[(g_FinishingColumnNumberCM - 1), (g_FinishingRankCM - 1)];
            //Does check persist in the target location?
            BoardCMCheck[(g_FinishingColumnNumberCM - 1), (g_FinishingRankCM - 1)] = MovingPieceCM;


            // Is the white king still under check?
            whiteKingCheck = checkForWhiteCheck(CMBoard);

            if((g_WhichColorPlays.CompareTo("White") == 0) && (whiteKingCheck == true)) {
                g_Legal = false;
            }


            // is the black king under check?
            blackKingCheck = checkForBlackCheck(CMBoard);

            if((g_WhichColorPlays.CompareTo("Black") == 0) && (blackKingCheck == true)) {
                g_Legal = false;
            }


            // restore pieces to their initial positions
            CMBoard[(g_StartingColumnNumberCM - 1), (g_StartingRankCM - 1)] = MovingPieceCM;
            #endregion CheckCheck

            if(((g_Correctness == true) && (g_Legal == true)) && (moveAnalysed == 0)) {
                // Store the move to ***_AI variables (because after continuous calls of ComputerMove the initial move under analysis will be lost...)

                MovingPiece_AI = g_movingPiece;
                g_StartingColumnNumber_AI = g_StartingColumnNumber;
                g_FinishingColumnNumber_AI = g_FinishingColumnNumber;
                g_StartingRank_AI = g_StartingRank;
                g_FinishingRank_AI = g_FinishingRank;

                // Store the initial move coordinates (at the node 0 level)
                NodesAnalysis0[nodeLevel_0_count, 2] = g_StartingColumnNumber_AI;
                NodesAnalysis0[nodeLevel_0_count, 3] = g_FinishingColumnNumber_AI;
                NodesAnalysis0[nodeLevel_0_count, 4] = g_StartingRank_AI;
                NodesAnalysis0[nodeLevel_0_count, 5] = g_FinishingRank_AI;
            }

        }
    }
}
