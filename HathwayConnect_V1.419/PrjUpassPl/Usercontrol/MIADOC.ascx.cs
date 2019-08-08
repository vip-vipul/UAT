using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrjUpassPl.Usercontrol
{
    public partial class MIADOC : System.Web.UI.UserControl
    {
        


        public string _lblMIAday
        {
            get { return lblMIAday.Text; }
            set { lblMIAday.Text = value; }
        }

        public string _lblMIAmonth
        {
            get { return lblMIAmonth.Text; }
            set { lblMIAmonth.Text = value; }


        }

        public string _lblMIAlcoName
        {
            get { return lblMIAlcoName.Text; }
            set { lblMIAlcoName.Text = value; }
        }
        public string _lblMIAlcoaddress
        {
            get { return lblMIAlcoaddress.Text; }
            set { lblMIAlcoaddress.Text = value; }
        }
        public string _lblMIAlcodate
        {
            get { return lblMIAlcodate.Text; }
            set { lblMIAlcodate.Text = value; }
        }
        public string _lblMIAlcoheadOffice
        {
            get { return lblMIAlcoheadOffice.Text; }
            set { lblMIAlcoheadOffice.Text = value; }
        }
        public string _lblMIAlcoarea
        {
            get { return lblMIAlcoarea.Text; }
            set { lblMIAlcoarea.Text = value; }
        }

        public string _lblmiamsoregisno
        {
            get { return lblmiamsoregisno.Text; }
            set { lblmiamsoregisno.Text = value; }
        }

        public string _lblmiamsoregisdate
        {
            get { return lblmiamsoregisdate.Text; }
            set { lblmiamsoregisdate.Text = value; }
        }

        public string _lblmiamsoarea
        {
            get { return lblmiamsoarea.Text; }
            set { lblmiamsoarea.Text = value; }
        }

        public string _lbllcoregisno
        {
            get { return lbllcoregisno.Text; }
            set { lbllcoregisno.Text = value; }
        }

        public string _lbllcoterritory
        {
            get { return lbllcoterritory.Text; }
            set { lbllcoterritory.Text = value; }
        }

        public string _lbllconame
        {
            get { return lbllconame.Text; }
            set { lbllconame.Text = value; }
        }

        public string _lblcompanyname
        {
            get { return lblcompanyname.Text; }
            set { lblcompanyname.Text = value; }
        }

        public string _lblip
        {
            get { return lblip.Text; }
            set { lblip.Text = value; }
        }

        public string _lbldatetime
        {
            get { return lbldatetime.Text; }
            set { lbldatetime.Text = value; }
        } 

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}