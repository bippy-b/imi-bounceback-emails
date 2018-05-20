using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security;
using System.Security.Principal;
using System.Security.Util;
using System.DirectoryServices;


namespace imi_bounceback_emails
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string myuser = "";
            myuser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            lblIdentity.Text = myuser;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string myuser = "";
            myuser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            string ldapAddress = "";
            string ccSubmitter = "";

            ////Get the proper domain
            if (myuser.StartsWith("ICON_US"))
            {
                ldapAddress = "LDAP://am.iconcr.com";
            }
            if (myuser.StartsWith("ICON-EU"))
            {
                ldapAddress = "LDAP://eu.iconcr.com";
            }
            if (myuser.StartsWith("ICONCR"))
            {
                ldapAddress = "LDAP://iconcr.com";
            }
            if (myuser.StartsWith("BEACONBIO"))
            {
                ldapAddress = "LDAP://beacon.iconcr.com";
            }
            if (ldapAddress.Length == 0)
            {
                ldapAddress = "LDAP://iconcr.com";
            }

            DirectoryEntry de = new DirectoryEntry(ldapAddress,"icon_us\\svc_itg","Whodoesntlikecake?");
            DirectorySearcher ds = new DirectorySearcher(de);

            string[] usr = myuser.Split('\\');

            ds.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(samaccountname=" + usr[1] + "))";
            ds.SearchScope = SearchScope.Subtree;
            SearchResult rs = ds.FindOne();

            if (rs.GetDirectoryEntry().Properties["mail"].Value != null)
            {
                ccSubmitter = rs.GetDirectoryEntry().Properties["mail"].Value.ToString();
            }

            lblIdentity.Text = "[" + myuser + "] - " + usr[1];
            lblStatus.Text = "";

            if (TextBox1.Text.Length == 0)
            {
                //do nothing
            }
            else
            {
                if (ccSubmitter.Length == 0)
                {
                    ccSubmitter = "imisupport@iconplc.com";
                }


                SqlConnection conn = null;
		        SqlDataReader rdr  = null;

		        // typically obtained from user
		        // input, but we take a short cut
		        string bouncebackemail = TextBox1.Text;


		        try
		        {
			        // create and open a connection object
			        conn = new
                    SqlConnection("Server=IMI-MIRACLS-DB;DataBase=MIRA_v2p1_PROD;User ID=MIRA_RO;PWD=k4@hUwrE3RaCRa8A");
                    //SqlConnection("Server=IMI-SQL-02;DataBase=Security;Integrated Security=SSPI");
			        conn.Open();

			        // 1. create a command object identifying
			        // the stored procedure
			        SqlCommand cmd  = new SqlCommand("usp_bounce_back_notification", conn);

			        // 2. set the command object so it knows
			        // to execute a stored procedure
			        cmd.CommandType = CommandType.StoredProcedure;

			        // 3. add parameter to command, which
			        // will be passed to the stored procedure
			        cmd.Parameters.Add(new SqlParameter("@bouncebackemail", bouncebackemail));
                    cmd.Parameters.Add(new SqlParameter("@userlogin", bouncebackemail));
                    cmd.Parameters.Add(new SqlParameter("@ccemail", ccSubmitter));
                    cmd.Parameters.Add(new SqlParameter("@whoami", myuser));

			        // execute the command
			        rdr = cmd.ExecuteReader();


		        }
		        finally
		        {
			        if (conn != null)
			        {
				        conn.Close();
			        }
			        if (rdr != null)
			        {
				        rdr.Close();
			        }
		        }

                //let the user know something has happened
                lblStatus.Text = "An email has been sent for " + TextBox1.Text;
            }
	}
}
        }
    
