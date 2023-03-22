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
        private void simpleButton1_Click(object sender, EventArgs e)
        {

            try
            {
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
                if(dtTeam==null)
                {
                    dtTeam = new DataTable("SoftViet");
                    if (!dtTeam.Columns.Contains("A1")) dtTeam.Columns.Add("A1", typeof(string));
                    if (!dtTeam.Columns.Contains("A2")) dtTeam.Columns.Add("A2", typeof(string));
                    if (!dtTeam.Columns.Contains("A3")) dtTeam.Columns.Add("A3", typeof(string));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RandomAndAddToTable(string players, DataTable dtTeam, ref int stt)
        {
            List<string> lstPlayer = GetListFromString(players);
            int len = lstPlayer.Count / 3 + (lstPlayer.Count % 3 > 0 ? 1 : 0);
            for (int i = 0; i < len; i++)
            {
                DataRow drNew = dtTeam.NewRow();
                dtTeam.Rows.Add(drNew);
            }
            while (lstPlayer.Count>0)
            {
                int rowIndex = 0;
                if (stt>3)
                {
                    rowIndex = stt % 3 == 0 ? stt / 3 - 1 : stt / 3;
                }
                string player = GetRandomAddRemove(ref lstPlayer);
                int colIndex = stt % 3 == 0 ? 3 : stt % 3;
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
