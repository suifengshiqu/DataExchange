using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataExchange.Client;
using Timer = System.Windows.Forms.Timer;

namespace DataExchange.Remote
{
    class FileSendThread
    {
        public FileTransferMessage _file;
        public IServerSide _proxy;
        public void SendFile()
        {
            try
            {
                _proxy.TransferFile(_file);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (_file.FileData != null)
                {
                    _file.FileData.Close();
                }
            }
        }
    }
    public partial class FrmControlPanel : Form
    {
        private ServiceHost _serviceHost = null;
        private Timer _timer = null;
        public FrmControlPanel()
        {
            InitializeComponent();
        }

        private IServerSide _proxy;
        private void btnStart_Click(object sender, EventArgs e)
        {
            //_serviceHost = new ServiceHost(typeof(DataExchange));
            //this._serviceHost.Open();
            //_timer = new Timer {Interval = 10000};
            //_timer.Tick += _timer_Tick;
            //_timer.Start();
            txtLog.Text += "服务已启动\r\n";

            FileTransferMessage file = null;
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.TransferMode = TransferMode.Streamed;
                binding.SendTimeout = new TimeSpan(0, 30, 0);
                binding.Security.Mode = SecurityMode.None;
                txtLog.Text += "获取文件信息\r\n";
                var get = WcfChannelFactory.ExecuteMetod<IServerSide>("net.tcp://zkjc-lb:4533/ServerSide.svc/basic", "GetInfo");
                file = get as FileTransferMessage;
                txtLog.Text += string.Format("文件信息:{0}\r\n", file.FileName);
                txtLog.Text += "推送文件\r\n";
                //利用通道创建客户端代理
                _proxy = ChannelFactory<IServerSide>.CreateChannel(binding, new EndpointAddress("net.tcp://zkjc-lb:4533/ServerSide.svc/basic"));
                IContextChannel obj = _proxy as IContextChannel;
                file = _proxy.GetInfo();
                FileSendThread sendThread = new FileSendThread();
                sendThread._file = file;
                sendThread._proxy = _proxy;
                Thread threadRead = new Thread(new ThreadStart(sendThread.SendFile));
                threadRead.Start();
                txtLog.Text += "推送完成\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var get = WcfChannelFactory.ExecuteMetod<IServerSide>("http://127.0.0.1:8098/ServerSide.svc", "GetData");
                txtLog.Text += get + "\thttp://127.0.0.1:8098/ServerSide.svc\r\n";
                var setMsg = WcfChannelFactory.ExecuteMetod<IServerSide>("http://127.0.0.1:8010/ServerSide.svc", "WriteMethod", get);
                txtLog.Text += setMsg + "\thttp://127.0.0.1:8010/ServerSide.svc\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseService();
            txtLog.Text += "服务已停止\r\n";
        }

        void CloseService()
        {
            var disposable = this._serviceHost as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }

        private void FrmControlPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseService();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = string.Empty;
        }
    }
}
