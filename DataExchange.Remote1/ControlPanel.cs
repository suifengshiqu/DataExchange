using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataExchange.Remote
{
    public partial class ControlPanel : Form
    {
        private ServiceHost _serviceHost = null;
        public ControlPanel()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _serviceHost = new ServiceHost(typeof(DataExchange));
            this._serviceHost.Open();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseService();
        }

        void CloseService()
        {
            var disposable = this._serviceHost as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        private void ControlPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseService();
        }
    }
}
