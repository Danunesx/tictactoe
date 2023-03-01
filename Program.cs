using System;

namespace tictactoe
{
    class Program
    {
        static void Main(string[] args)
        {   
            string player, pc;
            string[] board = new string[10];
            int start;
            bool init = false, isFirst = true;

            StartBoard(board);
            Console.WriteLine("Escolha seu caractere (X ou O): ");
            player = Console.ReadLine();

            pc = PcSymbol(player);
            start = StartGame();

            while(!init){
                init = programChampionVerify(board);
                if(init) break;
                if(start == 1){
                    playerRound(player, board);
                    init = programChampionVerify(board);
                    if(init) break;
                    pcRound(pc, board, isFirst);
                } else {
                    pcRound(pc, board, isFirst);
                    init = programChampionVerify(board);
                    if(init) break;
                    playerRound(player, board);
                } 
                isFirst = false;
            } 
        }

        static bool validPosition(int position, string[] board){
            if(board[position] != " ")
                return false;
            else return true;
        }

        static void playerRound(string player, string[] board){
            int position;

            do {
                Console.WriteLine("Escolha uma posição de 1-9: ");
                position = Convert.ToInt16(Console.ReadLine());
                if(validPosition(position, board) == false)
                    Console.WriteLine("Posição Inválida");
            } while(!validPosition(position, board));
            
            board[position] = player;
            board[0] = Convert.ToString(position);
    
        }

        static int randomArray(string[] board){
            int randomPosition;
            int[] array = new int[4] {1, 3, 7, 9};

            Random randomNumber = new Random();
            
            do{
                randomPosition = randomNumber.Next(0,4);
            } while(!validPosition(randomPosition, board));

            return array[randomPosition];
        }

        static int randomFullArray(string[] board){
            int randomPosition;

            Random randomNumber = new Random();
            
            do{
                randomPosition = randomNumber.Next(0, 10);
            } while(!validPosition(randomPosition, board));

            return randomPosition;
        }

        static void Put(string[] board, int jump, string pc, int i){
            
            if(i - jump > 0) {
                i -= jump;
                Console.WriteLine(i);
            }

            while(i < board.Length){
                if(board[i] == " "){
                    board[i] = pc;
                    break;
                }
                i += jump;
            }
            
            if(i >= board.Length){
                board[randomFullArray(board)] = pc;
            }
        }

        static void pcRound(string pc, string[] board, bool isFirst){
            
            Console.WriteLine("\nVez do computador");
            
            if(board[0] == " "){ // certo
                board[5] = pc; // primeira jogada do pc
            } 
            else {
                int lastPos = Convert.ToInt16(board[0]);

                if(isFirst){ // se for a primeira rodade
                    if(lastPos % 2 == 0){
                        if(lastPos == 8 || lastPos == 2)
                            board[lastPos - 1] = pc;
                        else
                            board[lastPos + 3] = pc;
                    }
                    else if ((lastPos % 2 != 0) && lastPos != 5) {
                        board[5] = pc;
                    }
                    else {
                        board[randomArray(board)] = pc;
                    }
                }
                else {
                    int Pos;

                    for(Pos = 1; Pos < board.Length; Pos++){
                        if(board[Pos] == board[lastPos]){
                            //if(Pos % 3 == 0) Pos++; // bug pode ter em 4-2
                            if(Pos == lastPos) Pos++;
                            
                            int i;
                            if(Pos > lastPos) i = lastPos;
                            else i = Pos;

                            /*
                                verifica diagonais
                                se 5 é uma jogada do user
                                se a ultima jogada foi nas pontas
                                se a ultima jogada for diferente de 5
                            */
                            if((board[5] != pc && board[5] != " ") && (lastPos % 2 != 0) && (lastPos != 5)){
                                if(validPosition(board.Length - lastPos, board)){
                                    board[board.Length - lastPos] = pc;
                                } else {
                                    //ataca
                                }
                                break;
                            }
                            // colunas
                            else if(Math.Abs(Pos - lastPos) % 3 == 0){
                                Put(board, 3, pc, i);
                                break;
                            }
                            //linhas
                            else if (Math.Abs(Pos - lastPos) == 1 || Math.Abs(Pos - lastPos) == 2){
                                if((i != 6) && (i != 2)){
                                    Put(board, 1, pc, i);
                                break;
                                } else{
                                    if(validPosition(5, board)) 
                                        board[5] = pc;
                                    else 
                                        board[randomFullArray(board)] = pc;
                                    
                                    break;
                                }
                            }
                            else {
                                board[randomFullArray(board)] = pc;
                                break;
                            }
                        }
                    }
                }
                
            }
            
        }

        static bool programChampionVerify(string[] board){
            bool verify = ChampionVerify(board);
            bool draw =  DrawVerify(board);

            if(verify){
                Console.WriteLine("\njogador " +board[0]+ " ganhou!");
                PrintBoard(board);
                return verify;
            }
            else if(draw){
                Console.WriteLine("\nO jogo acabou em empate");
                PrintBoard(board);
                return draw;
            }
            
            PrintBoard(board);
            return false;
        }

        static bool DrawVerify(string[] board){
            bool isDraw = true;

            for(int i = 1; i < board.Length; i++){
                if(board[i] == " "){
                    isDraw = false;
                    break;
                }
            }

            return isDraw;
        }

        static bool ChampionVerify(string[] board){
            
            for(int i = 1; i < board.Length; i++){
                if(board[i] != " "){
                    // linhas
                    if((i - 1) % 3 == 0){
                        if((board[i] == board[i + 1]) && (board[i] == board[i + 2])){
                            board[0] = board[i];
                            return true;
                        }
                    }
                    // colunas
                    if(i < 4){
                        if((board[i] == board[i + 3]) && (board[i] == board[i + 6])){
                            board[0] = board[i];
                            return true;
                        }
                    }
                }
            }

            //diagonais
            if (board[5] != " "){
                if((board[5] == board[1]) && (board[5] == board[9])){
                    board[0] = board[5];
                    return true;
                }
                if((board[5] == board[3]) && (board[5] == board[7])){
                    board[0] = board[5];
                    return true;
                }
            } 
            
            return false;
        }

        static int StartGame(){
            int start;

            Random randomNumber = new Random();
            start = randomNumber.Next(1,2);

            return start;
        }

        static string PcSymbol(string player){
            string pc;

            if(player == "X"){
                pc = "O";
            } else {
                pc = "X";
            }

            return pc;
        }

        static void StartBoard(string[] board){
            for(int i = 0; i < board.Length; i++){
                board[i] = " ";
            }
        }

        static void PrintBoard(string[] board){

            Console.WriteLine(" " +board[7]+ " | " +board[8]+ " | " +board[9]);
            Console.WriteLine("---+---+---");
            Console.WriteLine(" " +board[4]+ " | " +board[5]+ " | " +board[6]);
            Console.WriteLine("---+---+---");
            Console.WriteLine(" " +board[1]+ " | " +board[2]+ " | " +board[3]);

        }
    }
}
