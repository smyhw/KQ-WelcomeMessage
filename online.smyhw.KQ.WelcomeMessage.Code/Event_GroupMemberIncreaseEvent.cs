using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace online.smyhw.KQ.WelcomeMessage.Code
{

    public class Sdata//静态资源池(划掉)
    {
        public static Hashtable MessageMap = new Hashtable();
        public static Boolean ISready = false;
        public static CQApi APIII;//当前KQ_API实例
        public static CQLog log;//当前KQ_API的日志实例
    }
    public class Event_GroupMemberIncrease : IGroupMemberIncrease
    {
        public void GroupMemberIncrease(object sender, CQGroupMemberIncreaseEventArgs e)
        {
            if (!Sdata.ISready) { Sdata.log.Info("失败", "群" + e.FromGroup.Id.ToString() + "初始化没有完成"); return; }//如果配置没有成功加载，则不处理消息
            String msg = (String)Sdata.MessageMap[e.FromGroup.Id.ToString()];
            if (msg == null) { Sdata.log.Info("失败", "群"+e.FromGroup.Id.ToString()+"没有对应的进群消息"); return; }
            CQCode cqat = e.FromQQ.CQCode_At();
            Sdata.APIII.SendGroupMessage(e.FromGroup, cqat,msg);
        }
    }
}
