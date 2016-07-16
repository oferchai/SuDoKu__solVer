using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SKvisual;

namespace SK
{
    public class HighlightHandler : EventArgs
    {
        public HighlightHandler()
        {
            Singles = Enumerable.Empty<SKSingle>();
        }

        public SKSingle Single;
        public IEnumerable<SKSingle> Singles;
        public string Text;
    }
    public class SKSolver
    {
        public readonly SKMattrix sk;
        private readonly Form1 _frm;

        public SKSolver(SKMattrix sk, Form1 frm)
        {
            this.sk = sk;
            _frm = frm;
        }

        public event EventHandler Highlight;

        public void Raise(SKSingle single, string text)
        {
            if (Highlight != null)
            {
                Highlight(this, new HighlightHandler { Single = single, Text = text });
            }
        }

        public void RaiseEx(SKSingle single, string text, IEnumerable<SKSingle> singles)
        {
            if (Highlight != null)
            {
                Highlight(this, new HighlightHandler { Single = single, Text = text, Singles = singles });
            }
        }

        // Solver with a retry algorithm, make arbitrary solutions decisions and continue! 
        // TBD: need to support backtrace functionality
        public void SolveEx()
        {
            Solve();
            string desc = string.Empty;
            for (int i = 0; i < 50; i++)
                if (!sk.IsSolved && sk.ValidateMattrix(ref desc))
                {

                    var firstUnsolvedSingle = sk.AllSingles.Where(s => !s.IsNumberSet && s.Possible.Count==2).First();
                    Raise(firstUnsolvedSingle, "Puzzle have more than one solution, setting this cell to:"
                        + firstUnsolvedSingle.Possible.First());
                    //Wait();
                    firstUnsolvedSingle.SetNumber(firstUnsolvedSingle.Possible.First());
                    Solve();
                }

            if(!sk.ValidateMattrix(ref desc))
                Raise(sk.AllSingles.First(),desc);

        }
        public void Solve()
        {
            //Wait();

            bool changed;


            do
            {
                changed = false;

                changed |= NoBrainer(changed);


                changed |= IsMatrixChanged("UniqueRectabgle",
                    SimpleSKAlgo.UniqueRegtangle(sk, RaiseEx, NoWait), sk);


                changed |= IsMatrixChanged("HiddenVectorInCol",
                    SimpleSKAlgo.HiddenVectorInCol(sk, RaiseEx, NoWait), sk);

                changed |= NoBrainer(changed);

                changed |= IsMatrixChanged("HiddenVectorInRow",
                    SimpleSKAlgo.HiddenVectorInRow(sk, RaiseEx, NoWait), sk);

                changed |= NoBrainer(changed);

                changed |= IsMatrixChanged("NakedPairInCube",
                    SimpleSKAlgo.HiddenVectorInCube(sk, RaiseEx, NoWait), sk);




                string desc = string.Empty;
                if (changed && !sk.ValidateMattrix(ref desc))
                {
                    Raise(sk.AllSingles.First(), desc);
                    Wait();
                    break;
                }

            } while (!sk.IsSolved && changed);

            if ( !sk.IsSolved)
            {
                //IsMatrixChanged("UniqueRectabgle",
                //    SimpleSKAlgo.UniqueRegtangle(sk, RaiseEx, Wait), sk);

                Raise(sk.AllSingles.First(), "Cannot solve puzzle :-(");
                //Wait();
            }
            else
                Raise(sk.AllSingles.First(), "puzzle is SOLVED! :-)");




        }

        private bool NoBrainer(bool changed)
        {
            changed |= IsMatrixChanged("SinglePossiableNumber",
                SimpleSKAlgo.SinglePossiableNumber(sk, Raise, NoWait), sk);

            changed |= IsMatrixChanged("SingleNumberInCube",
                SimpleSKAlgo.SingleNumberInCube(sk, Raise, NoWait), sk);

            changed |= IsMatrixChanged("SingleNumberInRow",
                SimpleSKAlgo.SingleNumberInRow(sk, Raise, NoWait), sk);
            return changed;
        }

        private static bool IsMatrixChanged(string algoName, IEnumerable<SKSingle> changedSingles, SKMattrix sk)
        {
            bool mattrixChanged = changedSingles.Count() > 0;


            return mattrixChanged;
        }

        private void Wait()
        {
            if (_frm != null)
                _frm.MoveNext.WaitOne();

        }
        private void NoWait()
        {

            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }
    }
}