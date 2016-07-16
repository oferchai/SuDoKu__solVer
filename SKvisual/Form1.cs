using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SK;

namespace SKvisual
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbLevel.SelectedIndex = 2;
            Form1_Load(null, null);

        }

        public AutoResetEvent MoveNext = new AutoResetEvent(false);

        public void UpdateMattrix()
        {
            foreach (var row in _solver.sk.Rows)
                foreach (var s in row.Value)
                {
                    DGV.Rows[s.RowId].Cells[s.ColId].Style.ForeColor = Color.Blue;
                    DGV.Rows[s.RowId].Cells[s.ColId].Style.BackColor = Color.Azure;

                    if (s.IsNumberSet)
                    {
                        DGV.Rows[s.RowId].Cells[s.ColId].Style.Font = new System.Drawing.Font("Calibri", 16.2F,
                            FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                        DGV.Rows[s.RowId].Cells[s.ColId].Value = s.Number.Value + "";

                    }
                    else
                    {
                        DGV.Rows[s.RowId].Cells[s.ColId].Value = s.Possible.Aggregate(string.Empty,
                            (c, n) => c + "," + n + "");
                        DGV.Rows[s.RowId].Cells[s.ColId].Style.Font = new Font("Arial", 8F, GraphicsUnit.Pixel);

                    }
                }

            DGV.Invoke((MethodInvoker)delegate
           {
               DGV.Refresh();
           });


        }

        const int cColWidth = 45;
        const int cRowHeigth = 45;
        const int cMaxCell = 9;
        const int cSidelength = cColWidth * cMaxCell + 3;

        DataGridView DGV;
        private SKMattrix _mattrix;
        private SKSolver _solver;
        private Task _t;


        private void Form1_Load(object sender, EventArgs e)
        {


            DGV = new DataGridView();
            DGV.Name = "DGV";
            DGV.AllowUserToResizeColumns = false;
            DGV.AllowUserToResizeRows = false;
            DGV.AllowUserToAddRows = false;
            DGV.RowHeadersVisible = false;
            DGV.ColumnHeadersVisible = false;
            DGV.GridColor = Color.DarkBlue;
            DGV.DefaultCellStyle.BackColor = Color.AliceBlue;
            DGV.ScrollBars = ScrollBars.None;
            DGV.Size = new Size(cSidelength, cSidelength);
            DGV.Location = new Point(50, 70);
            DGV.Font = new System.Drawing.Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            DGV.ForeColor = Color.DarkBlue;
            DGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //DGV.DataSource = dt;
            // add 81 cells
            for (int i = 0; i < cMaxCell; i++)
            {
                DataGridViewTextBoxColumn TxCol = new DataGridViewTextBoxColumn();
                TxCol.MaxInputLength = 1; // only one digit allowed in a cell
                DGV.Columns.Add(TxCol);
                DGV.Columns[i].Name = "Col " + (i + 1).ToString();
                DGV.Columns[i].Width = cColWidth;
                DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewRow row = new DataGridViewRow();
                row.Height = cRowHeigth;
                DGV.Rows.Add(row);
            }



            // mark the 9 square areas consisting of 9 cells
            DGV.Columns[2].DividerWidth = 2;
            DGV.Columns[5].DividerWidth = 2;
            DGV.Rows[2].DividerHeight = 2;
            DGV.Rows[5].DividerHeight = 2;

            Controls.Add(DGV);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_t != null && !_t.IsCompleted)
                return;

            _t = null;
            int level = cbLevel.SelectedIndex > 3 ? cbLevel.SelectedIndex + 100 : (cbLevel.SelectedIndex) + 4;
            _mattrix = Program.CreateMattrixRnd(level);
            _solver = new SKSolver(_mattrix, this);
            _solver.Highlight += Highlight;

            UpdateMattrix();

        }

        public void Highlight(object sender, EventArgs e)
        {
            var h = e as HighlightHandler;
            UpdateMattrix();
            foreach (var s in h.Singles)
            {
                DGV.Rows[s.RowId].Cells[s.ColId].Style.BackColor = Color.Bisque;
            }
            DGV.Rows[h.Single.RowId].Cells[h.Single.ColId].Style.ForeColor = Color.Red;
            DGV.Rows[h.Single.RowId].Cells[h.Single.ColId].Style.BackColor = Color.Yellow;

            //DGV.Rows[h.Single.RowId].Cells[h.Single.ColId].Style.Font = new System.Drawing.Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            if (DGV.InvokeRequired)
                DGV.Invoke((MethodInvoker)delegate
               {
                   DGV.Refresh();
               });

            if (AlgoDesc.InvokeRequired)
                AlgoDesc.Invoke((MethodInvoker)delegate { AlgoDesc.Text = h.Text; });
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (_t == null || _t.IsCompleted)
            {
                _t = new Task(() => _solver.SolveEx());
                _t.Start();
            }
            UpdateMattrix();
            MoveNext.Set();
        }

        

        private void ClearP_Click(object sender, EventArgs e)
        {
            if (_t == null || _t.IsCompleted)
            {
                _mattrix = new SKMattrix(Enumerable.Empty<SKSingle>());
                UpdateMattrix();
            }

        }

        private void UseUIPuzzle_Click(object sender, EventArgs e)
        {
            _mattrix = new SKMattrix(GetPuzzleFromUIGrid());
            _solver = new SKSolver(_mattrix, this);
            _solver.Highlight += Highlight;

            UpdateMattrix();


        }

        private IEnumerable<SKSingle> GetPuzzleFromUIGrid()
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    string val = (string)DGV.Rows[r].Cells[c].Value;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    int num;
                    if (!Int32.TryParse(new string(new[] { val[0] }), out num))
                        continue;
                    if (num > 0)
                        yield return new SKSingle(r, c, num);

                }

            }
        }

        private void AutoContinue_CheckedChanged(object sender, EventArgs e)
        {
           

        }

        public void PushBacktrace(SKSolver.BacktraceItem peek)
        {
            if( Backtrace.InvokeRequired)
                Backtrace.Invoke((MethodInvoker)delegate { Backtrace.Items.Add(peek.ToString()); });
        }

        public void BacktracePop()
        {
            if (Backtrace.InvokeRequired)
                Backtrace.Invoke((MethodInvoker)delegate { Backtrace.Items.RemoveAt(Backtrace.Items.Count-1); });
        }
    }
}
