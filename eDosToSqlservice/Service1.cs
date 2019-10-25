using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eDosToSqlservice
{
    public partial class Service1 : ServiceBase
    {
        public bool _isRunning;
        //public Thread thread;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //  Model model = new Model();
            //thread = new Thread(Run);
            //thread.Start();

            this._isRunning = true;
            Model model = new Model();
            model.Process();


        }

        //void Run()
        //{
        //    Model model = new Model();
        //    while (true)
        //    {
        //        //if (model == null)
        //        //{
        //        //    model = new Model();
        //        //}
        //        model.Process();
        //        Thread.Sleep(1000);
        //    }
        //}

        protected override void OnStop()
        {
            //thread.Abort();

            this._isRunning = false;
        }
    }
}
