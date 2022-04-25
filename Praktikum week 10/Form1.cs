using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Praktikum_week_10
{
    public partial class FormLatihan : Form
    {
        public FormLatihan()
        {
            InitializeComponent();
        }
        static string connectionString = "server=localhost;uid=root;pwd=;database=premier_league;";
        public MySqlConnection sqlConnect = new MySqlConnection(connectionString);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;
        public string sqlQuery;

        DataTable dtTeamAway = new DataTable();
        DataTable dtTeamHome = new DataTable();

        

        private void FormLatihan_Load(object sender, EventArgs e)
        {
            sqlQuery = "SELECT t.team_id as `ID Tim`, t.team_name as `Nama Tim`, m.manager_name as `Nama Manager`, IF(m2.manager_name IS NULL,'----',m2.manager_name) as `Nama Asisten Manager`,p.player_name as `Nama Kapten`, home_stadium as `Stadium`, capacity as `Kapasitas` FROM team t LEFT JOIN manager m2 ON  t.assmanager_id = m2.manager_id ,manager m, player p WHERE t.manager_id = m.manager_id AND t.captain_id = p.player_id ;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamHome);
            sqlAdapter.Fill(dtTeamAway);
            comboHome.DataSource = dtTeamHome;
            comboHome.DisplayMember = "Nama Tim";
            comboHome.ValueMember = "ID Tim";
            comboAway.DataSource = dtTeamAway;
            comboAway.DisplayMember = "Nama Tim";
        }

        private void comboHome_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int posisiIndex = comboHome.SelectedIndex;
            labHomeManager.Text = dtTeamHome.Rows[posisiIndex]["Nama Manager"].ToString();
            labHomeAssManager.Text = dtTeamHome.Rows[posisiIndex]["Nama Asisten Manager"].ToString();
            labHomeKapten.Text = dtTeamHome.Rows[posisiIndex]["Nama Kapten"].ToString();
            labStadium.Text = dtTeamHome.Rows[posisiIndex]["Stadium"].ToString();
            labCapacity.Text = dtTeamHome.Rows[posisiIndex]["Kapasitas"].ToString();
        }

        private void comboAway_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int posisiIndex = comboAway.SelectedIndex;
            labAwayManager.Text = dtTeamAway.Rows[posisiIndex]["Nama Manager"].ToString();
            labAwayAssManager.Text = dtTeamAway.Rows[posisiIndex]["Nama Asisten Manager"].ToString();
            labAwayKapten.Text = dtTeamAway.Rows[posisiIndex]["Nama Kapten"].ToString();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            sqlQuery = "select date_format(m.match_date, \"%e %M %Y\") as 'Tanggal', concat(m.goal_home, ' - ', m.goal_away) as Skor from `match` m where m.team_home = '" + comboHome.SelectedValue.ToString() + "' and m.team_away = '" + comboAway.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);

            DataTable TglnSkor = new DataTable();
            sqlAdapter.Fill(TglnSkor);
            lblTgl.Text = TglnSkor.Rows[0]["Tanggal"].ToString();
            lblSkor.Text = TglnSkor.Rows[0]["Skor"].ToString();

            sqlQuery = "select dm.minute as 'MINUTE', if (p.team_id = m.team_home, p.player_name, '') as 'Player Name 1', if (p.team_id != m.team_home, '', if (dm.type = 'CY', 'Yellow Card', if (dm.type = 'CR', 'Red Card', if (dm.type = 'GO', 'Goal',  if (dm.type = 'GP', 'Goal Penalty', if (dm.type = 'GW', 'Own Goal',  if (dm.type = 'PM', 'Penalty Miss', ''))))))) as 'Tipe 1', if (p.team_id = m.team_away, p.player_name, '') as 'Player Name 2',  if (p.team_id != m.team_away, '', if (dm.type = 'CY', 'Yellow Card', if (dm.type = 'CR', 'Red Card', if (dm.type = 'GO', 'Goal',  if (dm.type = 'GP', 'Goal Penalty', if (dm.type = 'GW', 'Own Goal', if (dm.type = 'PM', 'Penalty Miss', ''))))))) as 'Tipe 2' from dmatch dm, player p, `match` m where dm.match_id = m.match_id and p.player_id = dm.player_id; ";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);

            DataTable dgw = new DataTable();
            sqlAdapter.Fill(dgw);
            dgw1.DataSource = dgw;
        }
    }
}
