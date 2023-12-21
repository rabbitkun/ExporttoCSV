using System;
using System.IO;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
    
namespace ExPortGridviewToXML 
{
    public partial class Default : System.Web.UI.Page
    {
        private SqlConnection con;
        private SqlCommand com; 
        private string constr, query;
        private void connection()
        {
            constr = ConfigurationManager.ConnectionStrings["getconn"].ToString();
            con = new SqlConnection(constr);
            con.Open();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bindgrid();
            }
        }
        private void Bindgrid()
        {
            connection();
            query = "select *from Employee";//not recommended this i have written just for example,write stored procedure for security
            com = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            con.Close();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ExportGridToCSV();
        }
        private void ExportGridToCSV()
        {
            Bindgrid();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Vithal_Wadje.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            GridView1.AllowPaging = false;
            GridView1.DataBind();
            StringBuilder columnbind = new StringBuilder();
            for (int k = 0; k < GridView1.Columns.Count; k++)
            {
                columnbind.Append(GridView1.Columns[k].HeaderText + ',');
            }
            columnbind.Append("\r\n");
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                for (int k = 0; k < GridView1.Columns.Count; k++)
                {
                    columnbind.Append(GridView1.Rows[i].Cells[k].Text + ',');
                }
                columnbind.Append("\r\n");
            }
            Response.Output.Write(columnbind.ToString());
            Response.Flush();
            Response.End();
        }
    }
}
