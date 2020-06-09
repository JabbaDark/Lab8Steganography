using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using System.Windows;
using System.Drawing.Imaging;

namespace Stegan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int seed = Convert.ToInt32(KeyMes.Text);
                using (var image = new Bitmap(@"C:\Code\SteganMain.png"))
                {
                    MesLength.Text = Mes.Text.Length.ToString();
                    var r = new Random(seed);
                    var bytes = Encoding.UTF8.GetBytes(Mes.Text);
                    var bits = new BitArray(bytes);
                    foreach (bool bit in bits)
                    {
                        int x = r.Next(0, image.Width), y = r.Next(0, image.Height);
                        var pixel = image.GetPixel(x, y);
                        byte R;
                        if (bit) R = (byte)(pixel.R | 1);
                        else R = (byte)(pixel.R & 254);
                        image.SetPixel(x, y, Color.FromArgb(R, pixel.G, pixel.B));
                    }
                    image.Save($@"C:\Code\SteganCode.png", ImageFormat.Png);
                    Mes.Text = string.Empty;
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int seed = Convert.ToInt32(KeyMes.Text);
            int length = Convert.ToInt32(MesLength.Text);
                using (var image = new Bitmap($@"C:\Code\SteganCode.png"))
                {
                    var r = new Random(seed);
                    var bits = new List<bool>();
                    var bytes = new List<byte>();
                    for (int i = 0; i < length * 8; i++)
                    {
                        int x = r.Next(0, image.Width), y = r.Next(0, image.Height);
                        var pixel = image.GetPixel(x, y);
                        bits.Add((pixel.R & 1) != 0);
                    }
                    int c = 0;
                    byte b = 0;
                    foreach (var bit in bits)
                    {
                        b |= (byte)(Convert.ToByte(bit) << c);
                        if (c == 7)
                        {
                            bytes.Add(b);
                            c = 0;
                            b = 0;
                        }
                        else c++;
                    }
                    Mes.Text = Encoding.UTF8.GetString(bytes.ToArray());
                }
        }
    }
}
