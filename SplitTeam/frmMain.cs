using SoftVietProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        string playerValueLink = "https://docs.google.com/spreadsheets/d/1KfUK-hjuOzh4kaF7Kk5jtcI9Sz2IdUdY82uz88DWCUk/export?format=csv&id=1KfUK-hjuOzh4kaF7Kk5jtcI9Sz2IdUdY82uz88DWCUk&gid=0";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                TeamNumber = Misc.ObjInt(speTeamNumber.EditValue);
                InitGrid();
                dtTeam.Rows.Clear();
                int stt = 1;
                
                RandomAndAddToTable(memGoalKeeper.Text, dtTeam, ref stt);
                RandomAndAddToTable(memSeedPlayer_Stricker.Text, dtTeam , ref stt);
                RandomAndAddToTable(memWeakPlayer_Striker.Text, dtTeam, ref stt);
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
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;
        private void Sleep(int Interval)
        {
            Thread.Sleep(Interval);
        }
        public string DownloadGoogle(string url)
        {
            string strResponse = "";
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    Stream objStream;
                    StreamReader objSR;
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                    System.Net.HttpWebRequest wrquest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    System.Net.HttpWebResponse getresponse = null;
                    getresponse = (System.Net.HttpWebResponse)wrquest.GetResponse();

                    objStream = getresponse.GetResponseStream();
                    objSR = new StreamReader(objStream, encode, true);
                    strResponse = objSR.ReadToEnd().Trim();

                    objSR.Close();
                    objStream.Close();
                    return strResponse;
                }
                catch (Exception e)
                {
                    if (i == NumberOfRetries)
                        throw;
                    Sleep(DelayOnRetry);
                }
            }
            return strResponse;
        }
        private void btnPlayerClassification_Click(object sender, EventArgs e)
        {
            try
            {
                string playerData= DownloadGoogle(playerValueLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
