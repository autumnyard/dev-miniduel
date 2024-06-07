using AutumnYard.Miniduel;
using System;
using System.Collections.Generic;

namespace MiniduelRunner
{
    internal class Program
    {
        static int Main(string[] args)
        {
            List<Action> asd = new List<Action>()
            {
                TestPlay_AllAttack,
                TestPlay_AllDefense,
                TestPlay_AllParry,
                TestPlay_Normal1,
                TestPlay_Normal2,
            };

            foreach (var test in asd)
            {
                test.Invoke();
                Console.WriteLine();
                Console.WriteLine();
            }

            // results should be:
            // 3 - 3, 1
            // 0 - 0, 1
            // 0 - 0, 0
            // 3 - 2, 0
            // 1 - 2, 1

            Console.ReadKey();
            return 0;
        }

        private static void TestPlay_AllAttack()
        {
            Game game = new Game();
            TestAdd(game, 0, 0, EPiece.Attack);
            TestAdd(game, 0, 1, EPiece.Attack);
            TestAdd(game, 0, 2, EPiece.Attack);
            TestAdd(game, 1, 0, EPiece.Attack);
            TestAdd(game, 1, 1, EPiece.Attack);
            TestAdd(game, 1, 2, EPiece.Attack);
            TestStartDuel(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestFinishRound(game);
        }

        private static void TestPlay_AllDefense()
        {
            Game game = new Game();
            TestAdd(game, 0, 0, EPiece.Defense);
            TestAdd(game, 0, 1, EPiece.Defense);
            TestAdd(game, 0, 2, EPiece.Defense);
            TestAdd(game, 1, 0, EPiece.Defense);
            TestAdd(game, 1, 1, EPiece.Defense);
            TestAdd(game, 1, 2, EPiece.Defense);
            TestStartDuel(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestFinishRound(game);
        }

        private static void TestPlay_AllParry()
        {
            Game game = new Game();
            TestAdd(game, 0, 0, EPiece.Parry);
            TestAdd(game, 0, 1, EPiece.Parry);
            TestAdd(game, 0, 2, EPiece.Parry);
            TestAdd(game, 1, 0, EPiece.Parry);
            TestAdd(game, 1, 1, EPiece.Parry);
            TestAdd(game, 1, 2, EPiece.Parry);
            TestStartDuel(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestFinishRound(game);
        }

        private static void TestPlay_Normal1()
        {
            Game game = new Game();
            TestAdd(game, 0, 0, EPiece.Attack);
            TestAdd(game, 0, 1, EPiece.Attack);
            TestAdd(game, 0, 2, EPiece.Parry);
            TestAdd(game, 1, 0, EPiece.Defense);
            TestAdd(game, 1, 1, EPiece.Attack);
            TestAdd(game, 1, 2, EPiece.Attack);
            TestStartDuel(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestFinishRound(game);
        }

        private static void TestPlay_Normal2()
        {
            Game game = new Game();
            TestAdd(game, 0, 0, EPiece.Defense);
            TestAdd(game, 0, 1, EPiece.Parry);
            TestAdd(game, 0, 2, EPiece.Attack);
            TestAdd(game, 1, 0, EPiece.Attack);
            TestAdd(game, 1, 1, EPiece.Defense);
            TestAdd(game, 1, 2, EPiece.Parry);
            TestStartDuel(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestPlayNextRound(game);
            TestFinishRound(game);
        }

        private static void TestAdd(Game game, int player, int location, EPiece piece)
        {
            bool result = game.SetPiece(player, location, piece);
            //Console.WriteLine($" - trying to place in [{player},{location}]. Result: {result}");
        }

        private static void TestStartDuel(Game game)
        {
            //Console.WriteLine($" - trying to duel. Result: {game.StartDuel()}");
            game.StartDuel();
        }

        private static void TestPlayNextRound(Game game)
        {
            //Console.WriteLine($"");
            //Console.WriteLine($" - playing next round...");
            game.PlayNextFight();
        }

        private static void TestFinishRound(Game game)
        {
            //Console.WriteLine($"");
            //Console.WriteLine($" - playing next round...");
            game.FinishRound();
        }
    }
}
