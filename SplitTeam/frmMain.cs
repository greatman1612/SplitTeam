using SoftVietProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplitTeam
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        Random r = new Random();
        DataTable dtTeam;
        int TeamNumber = 3;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                TeamNumber = Misc.ObjInt(speTeamNumber.EditValue);
                InitGrid();
                dtTeam.Rows.Clear();
                int stt = 1;
                
                RandomAndAddToTable(memGoalKeeper.Text, dtTeam, ref stt);
                RandomAndAddToTable(memSeedPlayer.Text, dtTeam , ref stt);
                RandomAndAddToTable(memWeakPlayer.Text, dtTeam, ref stt);
                RandomAndAddToTable(memSubstitutePlayer.Text, dtTeam, ref stt);
                grdTeam.DataSource = dtTeam;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                InitGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitGrid()
        {
            if (dtTeam == null)
            {
                dtTeam = new DataTable("SoftViet");
            }
            for (int i = 1; i <= TeamNumber; i++)
            {
                if (grdvTeam.Columns[$"A{i}"] != null) grdvTeam.Columns[$"A{i}"].Visible = true;
                if (!dtTeam.Columns.Contains($"A{i}")) dtTeam.Columns.Add($"A{i}", typeof(string));
            }
            for (int i = TeamNumber + 1; i <= 8; i++)
            {
                if (grdvTeam.Columns[$"A{i}"] != null) grdvTeam.Columns[$"A{i}"].Visible = false;
            }
        }
        private void RandomAndAddToTable(string players, DataTable dtTeam, ref int stt)
        {
            List<string> lstPlayer = GetListFromString(players);
            int len = lstPlayer.Count / TeamNumber + (lstPlayer.Count % TeamNumber > 0 ? 1 : 0);
            for (int i = 0; i < len; i++)
            {
                DataRow drNew = dtTeam.NewRow();
                dtTeam.Rows.Add(drNew);
            }
            while (lstPlayer.Count>0)
            {
                int rowIndex = 0;
                if (stt>TeamNumber)
                {
                    rowIndex = stt % TeamNumber == 0 ? stt / TeamNumber - 1 : stt / TeamNumber;
                }
                string player = GetRandomAddRemove(ref lstPlayer);
                int colIndex = stt % TeamNumber == 0 ? TeamNumber : stt % TeamNumber;
                dtTeam.Rows[rowIndex][$"A{colIndex}"] = player;
                stt++;
            }
        }
        private string GetRandomAddRemove(ref List<string> lstPlayers)
        {
            if (lstPlayers == null || lstPlayers.Count == 0) return string.Empty;
            int index = r.Next(lstPlayers.Count);
            string res = lstPlayers[index];
            lstPlayers.RemoveAt(index);
            return res;
        }
        private List<string> GetListFromString(string text)
        {
            List<string> lst = Misc.SplitToList(text, "\r\n");
            for (int i = lst.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(lst[i])) lst.RemoveAt(i);
            }
            return lst;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            try
            {
                if (dtTeam != null)
                {
                    dtTeam.Rows.Clear();
                    grdTeam.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
