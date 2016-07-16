using System;
using System.Linq;
using SK;

namespace SKvisual
{


    public static class SKCreator
    {

        public static SKMattrix CreateRandomMattrix()
        {
            string desc = string.Empty;

            var res = new SKMattrix(Enumerable.Empty<SKSingle>());
            var r = new Random();
            while (!res.IsSolved)
            {
                var unresolvedSingles = res.AllSingles.Where(s => !s.IsNumberSet);
                if (unresolvedSingles.Count() < 10)
                    break;
                var singleLocation = r.Next(0, unresolvedSingles.Count() - 1);

                var single = unresolvedSingles.ElementAt(singleLocation);

                var validNumbers = single.Possible.ToList();

                do
                {

                    
                    var skBackup = res.HardCopy();
                    
                    single = res.AllSingles.Where(s => !s.IsNumberSet).ElementAt(singleLocation);

                    int randomNumber = validNumbers.ElementAt(r.Next(0, validNumbers.Count - 1));
                    single.SetNumber(randomNumber);

                    if (!res.ValidateMattrix(ref desc))
                    {
                        res = skBackup;
                    }
                    else
                    {
                        break;
                    }
                } while (true);


            }

            res = res.HardCopy();
            if (!res.ValidateMattrix(ref desc))
            {
                return null;
            }

            var resolver = new SKSolver(res, null);
            resolver.Solve();
            return resolver.sk;

        }


        public static SKMattrix CreateRandomMattrixRec(SKMattrix sk)
        {
            string desc = string.Empty;

            var r = new Random();
            
            if (!sk.IsSolved)
            {
                var backup = sk.HardCopy();


                var unresolvedSingles = sk.AllSingles.Where(s => !s.IsNumberSet);
                var singleLocation = r.Next(0, unresolvedSingles.Count() - 1);

                var single = unresolvedSingles.ElementAt(singleLocation);

                var validNumbers = single.Possible.ToList();


                int randomNumber = validNumbers.ElementAt(r.Next(0, validNumbers.Count - 1));

                single.SetNumber(randomNumber);
                bool continueSetNumbers = true;
                while (continueSetNumbers)
                {

                    continueSetNumbers = false;
                    foreach (var s in sk.AllSingles.Where(s => !s.IsNumberSet && s.Possible.Count == 1))
                    {
                        s.SetNumber(s.Possible.First());
                        continueSetNumbers = true;
                    }
                }

                if (sk.ValidateMattrix(ref desc))
                    CreateRandomMattrixRec(sk);
                else
                    CreateRandomMattrixRec(backup);




            }

            return sk;

        }

        public static SKMattrix GenerateSeedMattrix()
        {
            //SKSingle[] singles = new[]
            //{
            //    new SKSingle(0, 8, 9),
            //    new SKSingle(1, 0, 9),
            //    new SKSingle(1, 1, 3),
            //    new SKSingle(1, 5, 6),
            //    new SKSingle(1, 6, 5),
            //    new SKSingle(2, 1, 8),
            //    new SKSingle(2, 2, 1),
            //    new SKSingle(2, 5, 7),
            //    new SKSingle(2, 7, 6),
            //    new SKSingle(3, 2, 5),
            //    new SKSingle(3, 4, 6),
            //    new SKSingle(3, 6, 7),
            //    new SKSingle(3, 7, 4),
            //    new SKSingle(4, 1, 6),
            //    new SKSingle(4, 4, 7),
            //    new SKSingle(4, 5, 4),
            //    new SKSingle(5, 2, 4),
            //    new SKSingle(6, 0, 7),
            //    new SKSingle(6, 3, 9),
            //    new SKSingle(6, 5, 2),
            //    new SKSingle(6, 6, 6),
            //    new SKSingle(7, 0, 4),
            //    new SKSingle(7, 4, 1),
            //    new SKSingle(7, 6, 9),
            //    new SKSingle(7, 7, 8),
            //    new SKSingle(8, 1, 9),
            //    new SKSingle(8, 2, 8),
            //    new SKSingle(8, 7, 2),

            //};

            //SKMattrix sk = new SKMattrix(singles);
            //var solver = new SKSolver(sk, null);
            //solver.Solve();
            SKSingle[] singles1 = new[]
            {
                new SKSingle(0, 0, 2),
                new SKSingle(0, 1, 4),
                new SKSingle(0, 2, 6),
                new SKSingle(0, 3, 1),
                new SKSingle(0, 4, 5),
                new SKSingle(0, 5, 8),
                new SKSingle(0, 6, 3),
                new SKSingle(0, 7, 7),
                new SKSingle(0, 8, 9),

                new SKSingle(1, 0, 9),
                new SKSingle(1, 1, 3),
                new SKSingle(1, 2, 7),
                new SKSingle(1, 3, 4),
                new SKSingle(1, 4, 2),
                new SKSingle(1, 5, 6),
                new SKSingle(1, 6, 5),
                new SKSingle(1, 7, 1),
                new SKSingle(1, 8, 8),

                new SKSingle(2, 0, 5),
                new SKSingle(2, 1, 8),
                new SKSingle(2, 2, 1),
                new SKSingle(2, 3, 3),
                new SKSingle(2, 4, 9),
                new SKSingle(2, 5, 7),
                new SKSingle(2, 6, 4),
                new SKSingle(2, 7, 6),
                new SKSingle(2, 8, 2),

                new SKSingle(3, 0, 3),
                new SKSingle(3, 1, 2),
                new SKSingle(3, 2, 5),
                new SKSingle(3, 3, 8),
                new SKSingle(3, 4, 6),
                new SKSingle(3, 5, 9),
                new SKSingle(3, 6, 7),
                new SKSingle(3, 7, 4),
                new SKSingle(3, 8, 1),

                new SKSingle(4, 0, 1),
                new SKSingle(4, 1, 6),
                new SKSingle(4, 2, 9),
                new SKSingle(4, 3, 2),
                new SKSingle(4, 4, 7),
                new SKSingle(4, 5, 4),
                new SKSingle(4, 6, 8),
                new SKSingle(4, 7, 3),
                new SKSingle(4, 8, 5),

                new SKSingle(5, 0, 8),
                new SKSingle(5, 1, 7),
                new SKSingle(5, 2, 4),
                new SKSingle(5, 3, 5),
                new SKSingle(5, 4, 3),
                new SKSingle(5, 5, 1),
                new SKSingle(5, 6, 2),
                new SKSingle(5, 7, 9),
                new SKSingle(5, 8, 6),

                new SKSingle(6, 0, 7),
                new SKSingle(6, 1, 1),
                new SKSingle(6, 2, 3),
                new SKSingle(6, 3, 9),
                new SKSingle(6, 4, 8),
                new SKSingle(6, 5, 2),
                new SKSingle(6, 6, 6),
                new SKSingle(6, 7, 5),
                new SKSingle(6, 8, 4),

                new SKSingle(7, 0, 4),
                new SKSingle(7, 1, 5),
                new SKSingle(7, 2, 2),
                new SKSingle(7, 3, 6),
                new SKSingle(7, 4, 1),
                new SKSingle(7, 5, 3),
                new SKSingle(7, 6, 9),
                new SKSingle(7, 7, 8),
                new SKSingle(7, 8, 7),

                new SKSingle(8, 0, 6),
                new SKSingle(8, 1, 9),
                new SKSingle(8, 2, 8),
                new SKSingle(8, 3, 7),
                new SKSingle(8, 4, 4),
                new SKSingle(8, 5, 5),
                new SKSingle(8, 6, 1),
                new SKSingle(8, 7, 2),
                new SKSingle(8, 8, 3),


            };
            SKMattrix sk = new SKMattrix(singles1);            
            return sk;
        }
        public static SKMattrix CreateRandomMattrixShuffleRow(SKMattrix sk, int a, int b)
        {
            foreach (var s in sk.Rows[a])
            {
                s.RowId = b; 
            }
            foreach (var s in sk.Rows[b])
            {
                s.RowId = a;
            }
            return new SKMattrix(sk.AllSingles);
            
        }

        public static SKMattrix CreateRandomMattrixShuffleCol(SKMattrix sk, int a, int b)
        {
            foreach (var s in sk.Cols[a])
            {
                s.ColId = b;
            }
            foreach (var s in sk.Cols[b])
            {
                s.ColId = a;
            }
            return new SKMattrix(sk.AllSingles);
        }

        public static SKMattrix CreateRandomMattrixShuffle(SKMattrix sk, decimal difficultyLevel)
        {
            string desc = string.Empty;
            if(!sk.ValidateMattrix(ref desc))
                throw new Exception("Puzzle not valid!");
            
                

            var r = new Random();
            int group, a;
            for (int i = 0; i < 100; i++)
            {
                switch (r.Next(0, 1))
                {
                    case 0:
                        group = r.Next(0, 2);
                        a = r.Next(0, 2);

                        sk = CreateRandomMattrixShuffleCol(sk, (group*3) + a, (group*3) + (a + 1)%3);
                        break;
                    case 1:
                        group = r.Next(0, 2);
                        a = r.Next(0, 2);

                        sk = CreateRandomMattrixShuffleRow(sk, (group*3) + a, (group*3) + (a + 1)%3);
                        break;

                }
            }



            for (int i = 0; i < difficultyLevel*10; i++)
            {
                do
                {
                    int loc = r.Next(0, 80);
                    if (sk.AllSingles.ElementAt(loc).Number != -1)
                    {
                        sk.AllSingles.ElementAt(loc).Number = -1;
                        break;
                    }
                } while (true);
            }


            var newPuzzle =  new SKMattrix(sk.AllSingles.Where(s=>s.Number!=-1));
            if (!newPuzzle.ValidateMattrix(ref desc))
                throw new Exception("Puzzle not valid!");
            return newPuzzle;


        }
        
    }
}