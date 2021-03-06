﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public SKMattrix sk;
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

        public class BacktraceItem
        {
            public SKMattrix Mattrix;
            public SKSingle Single;
            public int GuessId;

            public string ToString()
            {
                return Single.ToString() + " Set to " + Single.Possible.ElementAt(GuessId);
            }
        }
        // Solver with a retry algorithm, make arbitrary solutions decisions and continue! 
        // TBD: need to support backtrace functionality

            // some more comments
        public void SolveEx()
        {
            // create the backtrace stack 
            var backtrace = new Stack<BacktraceItem>();
            string desc = string.Empty;
            do
            {
                // attempt to solve the puzzle
                Solve();

                if (sk.IsSolved)
                    return;

                // if unable to solve the puzzle, is puzzle still valid ?
                if (sk.ValidateMattrix(ref desc))
                {
                    // get the first branch single, and branch
                    var firstUnsolvedSingle = sk.AllSingles.Where(s => !s.IsNumberSet && s.Possible.Count <= 3).OrderBy(s=>s.Possible.Count).FirstOrDefault();
                    if (firstUnsolvedSingle == null)
                    {
                        Raise(sk.AllSingles.First(), "Cannot branch , no single with 3 or less candidates ");
                        Wait();
                        return;
                    }
                    Raise(firstUnsolvedSingle, "Branching puzzle, setting this cell to:"
                                               + firstUnsolvedSingle.Possible.First());
                    Wait();

                    var newMattrix = sk.HardCopy();
                    var newSingle =
                        newMattrix.AllSingles.Where(
                            s => s.RowId == firstUnsolvedSingle.RowId && s.ColId == firstUnsolvedSingle.ColId).Single();


                    // push branch to stack
                    backtrace.Push(new BacktraceItem
                    {
                        GuessId = 0,
                        Mattrix = newMattrix,
                        Single = newSingle
                    });
                    if(_frm!=null) _frm.PushBacktrace(backtrace.Peek());
                    firstUnsolvedSingle.SetNumber(firstUnsolvedSingle.Possible.First());

                }
                else
                {
                    // try backtracing ... 
                    BacktraceItem backtraceItem = null;

                    do
                    {
                        // get top branch from stack
                        backtraceItem = backtrace.Pop();
                        if (_frm != null)  _frm.BacktracePop();
                        // if stack is empty. Abort 
                        if (backtraceItem == null)
                        {
                            Raise(sk.AllSingles.First(), "Puzzel in UNSOLVABLE");

                            Wait();
                            return;
                        }

                        if (backtraceItem.Single.Possible.Count() - 1 > backtraceItem.GuessId)
                        {
                            sk = backtraceItem.Mattrix;
                            var newMattrix = sk.HardCopy();
                            var newSingle =
                                newMattrix.AllSingles.Where(
                                    s => s.RowId == backtraceItem.Single.RowId && s.ColId == backtraceItem.Single.ColId).Single();

                            backtrace.Push(new BacktraceItem
                            {
                                GuessId = backtraceItem.GuessId + 1,
                                Single = newSingle,
                                Mattrix = newMattrix
                            });
                            if (_frm != null)  _frm.PushBacktrace(backtrace.Peek());

                            Raise(backtraceItem.Single, "BackTracing puzzle, setting this cell to:" +
                                                        backtraceItem.Single.Possible.ElementAt(backtraceItem.GuessId + 1));
                            Wait();
                            sk.AllSingles.Where(
                                    s => s.RowId == backtraceItem.Single.RowId && s.ColId == backtraceItem.Single.ColId).Single().SetNumber(
                                backtraceItem.Single.Possible.ElementAt(backtraceItem.GuessId + 1));
                            if (!sk.ValidateMattrix(ref desc))
                                backtraceItem = null;
                        }
                        else
                        {
                            backtraceItem = null;
                        }
                    } while (backtraceItem == null);


                }
            } while (true);

            //}
            //Solve();
            //string desc = string.Empty;
            //for (int i = 0; i < 50; i++)
            //    if (!sk.IsSolved && sk.ValidateMattrix(ref desc))
            //    {

            //        var firstUnsolvedSingle = sk.AllSingles.Where(s => !s.IsNumberSet && s.Possible.Count==2).First();
            //        Raise(firstUnsolvedSingle, "Puzzle have more than one solution, setting this cell to:"
            //            + firstUnsolvedSingle.Possible.First());
            //        Wait();
            //        firstUnsolvedSingle.SetNumber(firstUnsolvedSingle.Possible.First());
            //        Solve();
            //    }

            //if(!sk.ValidateMattrix(ref desc))
            //    Raise(sk.AllSingles.First(),desc);

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
                    AdvanceSKAlgo.UniqueRegtangle(sk, RaiseEx, NoWait), sk);


                changed |= IsMatrixChanged("HiddenVectorInCol",
                    SimpleSKAlgo.HiddenVectorInCol(sk, RaiseEx, NoWait), sk);

                changed |= NoBrainer(changed);

                changed |= IsMatrixChanged("HiddenVectorInRow",
                    SimpleSKAlgo.HiddenVectorInRow(sk, RaiseEx, NoWait), sk);

                changed |= NoBrainer(changed);

                changed |= IsMatrixChanged("NakedPairInCube",
                    SimpleSKAlgo.HiddenVectorInCube(sk, RaiseEx, NoWait), sk);

                changed |= IsMatrixChanged("SwordfishAlgo",
                    AdvanceSKAlgo.SwordfishAlgo(sk, RaiseEx, Wait), sk);


                changed |= IsMatrixChanged("XWngPattern",
                                    AdvanceSKAlgo.XWingAlgo(sk, RaiseEx, Wait), sk);

                changed |= NoBrainer(changed);




                string desc = string.Empty;
                if (changed && !sk.ValidateMattrix(ref desc))
                {
                    Raise(sk.AllSingles.First(), desc);
                    Wait();
                    break;
                }

            } while (!sk.IsSolved && changed);

            if(!sk.IsSolved)
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

        private bool IsMatrixChanged(string algoName, IEnumerable<SKSingle> changedSingles, SKMattrix sk)
        {
            bool mattrixChanged = changedSingles.Count() > 0;

            if(mattrixChanged)
                RaiseEx(changedSingles.First(), algoName, changedSingles);

            return mattrixChanged;
        }

        private void Wait()
        {
            if (_frm != null && !_frm.AutoContinue.Checked)
                _frm.MoveNext.WaitOne();

        }
        private void NoWait()
        {

            Thread.Sleep(TimeSpan.FromMilliseconds(50));
        }
    }
}