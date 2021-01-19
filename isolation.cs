using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        static void Initialization() // Calls for user's information and intital structure for the game.
        {
            Write( "Enter your name [default Player A]: " );
            string name1 = ReadLine( );
            if( name1.Length == 0 ) name1 = "Player A";
            playerA = name1;
            
            Write( "Enter your name [default Player B]: " );
            string name2 = ReadLine( );
            if( name2.Length == 0 ) name2 = "Player B";
            playerB = name2;

            Write("Enter the number of rows [default 6]: ");
            string gameRows = ReadLine();
            if( gameRows.Length == 0 ) gameRows = "6";
            int rows = int.Parse(gameRows);
            if ( rows < 4 | rows > 26) 
            {
                throw new ArgumentOutOfRangeException(nameof(rows), 
                "The number of rows must be between 4 and 26.");
            }  
            boardRows = rows;
        
            Write("Enter the number of columns [default 8]: ");
            string gameCols = ReadLine();
            if( gameCols.Length == 0 ) gameCols = "8";
            int cols = int.Parse(gameCols);
            if ( cols < 4 | cols > 26) 
            {
                throw new ArgumentOutOfRangeException(nameof(cols), 
                "The number of columns must be between 4 and 26."); 
            }
            boardCols = cols;
            
            board = new bool[boardRows, boardCols];
            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardCols; c++)
                {
                    board[r,c] = true;
                }
            }
            
            Write( "Enter the first platform tile's row [default 2]: " );
            string ARow = ReadLine();
            if( ARow.Length == 0 ) ARow = "2";
            int theARow = int.Parse(ARow);
            platformARow = theARow;
            pawnARow = platformARow;
            
            Write( "Enter the first platform tile's column [default 0]: " );
            string ACol = ReadLine();
            if( ACol.Length == 0 ) ACol = "0";
            int theACol = int.Parse(ACol);
            platformACol = theACol;
            pawnACol = platformACol;
            
            Write( "Enter the second platform tile's row [default 3]: " );
            string BRow = ReadLine();
            if( BRow.Length == 0 ) BRow = "3";
            int theBRow = int.Parse(BRow);
            platformBRow = theBRow;
            pawnBRow = platformBRow;
            
            Write( "Enter the second platform tile's column [default 7]: " );
            string BCol = ReadLine();
            if(BCol.Length == 0 ) BCol = "7";
            int theBCol = int.Parse(BCol);
            platformBCol = theBCol;
            pawnBCol = platformBCol;
        }
        
        static bool[ , ] board = new bool[ boardRows, boardCols ]; 
        
        static string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};

        static string turn = "A";
        
        static string playerA;
        static string playerB;
        static int boardRows = 6;
        static int boardCols = 8;
        static int platformARow;
        static int platformACol;
        static int platformBRow;
        static int platformBCol;
        
        static string move;
        static int pawnARow;
        static int pawnACol;
        static int pawnBRow;
        static int pawnBCol;        
        
        static void Main() 
        {
            Initialization();
            DrawGameBoard();
            
            bool game = true;
            while (game)
            {
                if (turn == "A") WriteLine("It is {0}'s turn", playerA);
                else WriteLine("It is {0}'s turn", playerB);
                move = playerMoves();
                DrawGameBoard( ); 
                if (turn == "A") turn = "B";
                else turn = "A";
            }
        }
        
        static string playerMoves() // Calls for player's move and decides whether it is valid or not.
        {
            bool validMove = false;
            while (validMove == false)
            {
                Write( "Enter a move [abcd]: " );
                string nextMove = ReadLine();
                int nextRow = Array.IndexOf( letters, nextMove.Substring( 0, 1 ));
                int nextCol = Array.IndexOf( letters, nextMove.Substring( 1, 1 ));
                int removeRow = Array.IndexOf( letters, nextMove.Substring( 2, 1 ));
                int removeCol = Array.IndexOf( letters, nextMove.Substring( 3, 1 ));
                
                if( nextRow < 0 || nextRow >= boardRows
                    || nextCol < 0 || nextCol >= boardCols 
                    || nextRow == pawnARow && nextCol == pawnACol
                    || nextRow == pawnBRow && nextCol == pawnBCol
                    || board[ nextRow, nextCol ] == false
                    || turn == "A" && (int) Math.Abs( pawnARow - nextRow ) > 1
                    || turn == "A" && (int) Math.Abs( pawnACol - nextCol ) > 1
                    || turn == "B" && (int) Math.Abs( pawnBRow - nextRow ) > 1
                    || turn == "B" && (int) Math.Abs( pawnBCol - nextCol ) > 1 )
                {
                    WriteLine( "Your pawn can't move there." );
                }
                else if( removeRow < 0 || removeRow >= boardRows
                    || removeCol < 0 || removeCol >= boardCols 
                    || turn == "B" && removeRow == pawnARow && removeCol == pawnACol
                    || turn == "A" && removeRow == pawnBRow && removeCol == pawnBCol
                    || removeRow == nextRow && removeCol == nextCol
                    || removeRow == platformARow && removeCol == platformACol
                    || removeRow == platformBRow && removeCol == platformBCol
                    || board[ removeRow, removeCol ] == false )
                {
                    WriteLine( "You can't remove that tile." );
                }
                else 
                {
                    validMove = true;
                    move = nextMove;
                    if (turn == "A")
                    {
                        pawnARow = nextRow;
                        pawnACol = nextCol;
                    }
                    else
                    {
                        pawnBRow = nextRow;
                        pawnBCol = nextCol;
                    }
                    board [ removeRow, removeCol ] = false;
                }
            }
            return move;
        }
        
        static void DrawGameBoard( ) // Draws the game board based on previous information given.
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string bb = "\u25a0"; // block
            const string fb = "\u2588"; // left half block
            const string lh = "\u258c"; // left half block
            const string rh = "\u2590"; // right half block
            
            WriteLine( ); 
            
            // Draw top row of letters.   
            Write("   ");
            for( int r = 0; r < board.GetLength( 1 ); r ++ )
            {
                Write( "  {0} ", letters[ r ] );
            }    
            WriteLine();
            
            // Draw top board boundary.
            Write( "   " );
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write( tl );
                Write( "{0}{0}{0}", h );
                if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", tr ); 
                else                                Write( "{0}", hb );
            }
            WriteLine( );
            
            // Draw left row of letters.
            for( int r = 0; r < board.GetLength( 0 ); r ++ )
            {
                Write( " {0} ", letters[ r ] );
                
                // Draw row contents.
                for( int c = 0; c < board.GetLength( 1 ); c ++ )
                {
                    if ( c == 0 )                                       Write( v );
                    if( r == pawnARow & c == pawnACol )                 Write( " A {0}", v );
                    else if( r == pawnBRow & c == pawnBCol )            Write( " B {0}", v );
                    else if( r == platformARow & c == platformACol )    Write( " {0} {1}", bb, v );
                    else if( r == platformBRow & c == platformBCol )    Write( " {0} {1}", bb, v );
                    else if( board[r, c] == false )                     Write( "   {0}", v );
                    else                                                Write( "{0}{1}{2}{3}", rh, fb, lh, v );
                }
                WriteLine( );
                
                // Draw boundary after row.
                if( r != board.GetLength( 0 ) - 1 )
                { 
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( vr );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", vl ); 
                        else                                Write( "{0}", hv );
                    }
                    WriteLine( );
                }
                else
                {
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( bl );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", br ); 
                        else                                Write( "{0}", ha );
                    }
                    WriteLine( );
                }
            }
            WriteLine( );
        }
    }
}
