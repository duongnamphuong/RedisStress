﻿using RedisUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedisGUI1
{
    public partial class btnSpam : Form
    {
        private StackExchange.Redis.IServer server = null;
        private StackExchange.Redis.IDatabase cache = null;
        private int? NumberOfProducts = null;

        public btnSpam()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                server = RedisConnectorHelper.Connection.GetServer(ConfigurationManager.AppSettings["redisserver"]);
                cache = RedisConnectorHelper.Connection.GetDatabase();
                MessageBox.Show("Connect successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDeleteAllProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var ProductKeys = server.Keys(pattern: "Product*").ToList();
                var start = DateTime.Now;
                foreach (var key in ProductKeys)
                {
                    cache.KeyDelete(key);
                }
                var end = DateTime.Now;
                MessageBox.Show($"There are {ProductKeys.Count()} \"Product*\"-pattern keys. Deleted them in {(end - start).TotalMilliseconds} millisecs");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                var start = DateTime.Now;
                NumberOfProducts = int.Parse(txtNumberOfProducts.Text);
                for (int i = 1; i <= NumberOfProducts.Value; i++)
                {
                    cache.StringSet($"Product{i}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                }
                var end = DateTime.Now;
                MessageBox.Show($"Prepared {NumberOfProducts.Value} products in {(end - start).TotalMilliseconds} millisecs");
            }
            catch (Exception ex)
            {
                NumberOfProducts = null;
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rnd = new Random();
            try
            {
                int NumberOfHeartbeats = int.Parse(txtNumberOfHeartbeats.Text);
                var start = DateTime.Now;
                for (int i = 0; i < NumberOfHeartbeats; i++)
                {
                    string key = $"Product{rnd.Next(1, NumberOfProducts.Value + 1)}";
                    string value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                    cache.StringSet(key, value);
                }
                var end = DateTime.Now;
                MessageBox.Show($"Updating {NumberOfHeartbeats} heatbeats in {(end - start).TotalMilliseconds} millisecs");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
