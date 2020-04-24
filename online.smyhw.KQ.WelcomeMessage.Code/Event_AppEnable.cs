using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace online.smyhw.KQ.WelcomeMessage.Code
{
    public class Event_AppEnable : IAppEnable
    {
        //这个方法将在程序运行时执行
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            e.CQLog.Info("初始化","WelcomeMessage插件加载...");
            Sdata.ISready = false;
            Sdata.MessageMap = new Hashtable();
            Sdata.APIII = e.CQApi;
            Sdata.log = e.CQLog;

            //加载配置文件
            if (!File.Exists("./WelcomeMessage.txt"))
            {
                Sdata.log.Warning("初始化", "未找到配置文件，创建之...");
                File.Create("./WelcomeMessage.txt").Dispose();
                System.IO.StreamWriter config_file_ = new System.IO.StreamWriter("./WelcomeMessage.txt");
                config_file_.WriteLine("#修改为群号和对应的欢迎信息");
                config_file_.WriteLine("#例如：");
                config_file_.WriteLine("123456789 = 欢迎进群！");
                config_file_.Close();
                Sdata.log.Warning("初始化","配置文件创建完毕，请修改配置文件并重新启用应用！");
                return;
            }

            //读取配置文件
            System.IO.StreamReader config_file = new System.IO.StreamReader("./WelcomeMessage.txt");
            while (true)
            {
                string line_text = config_file.ReadLine();
                if (line_text == null) { break; }
                if (line_text.StartsWith("#")) { continue; }
                string[] temp1 = line_text.Split('=');
                String qqGroup = temp1[0].Trim();
                String temp2 = line_text.Substring(line_text.IndexOf("=")+1);
                String WelcomeMsg = temp2.Trim();
                Sdata.log.Info("初始化", "配置项目{QQ="+qqGroup+";msg="+WelcomeMsg+"}");
                Sdata.MessageMap.Add(qqGroup, WelcomeMsg);
            }
            config_file.Close();

            Sdata.ISready = true;
        }
    }
}
