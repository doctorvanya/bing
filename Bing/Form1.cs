using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Bing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Picture Deserialize()
        {
            WebClient web = new WebClient();
            string source =
                web.DownloadString(
                        new Uri("http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US",
                            UriKind.Absolute));
            XmlSerializer xml = new XmlSerializer(typeof(Picture));
            Picture result = xml.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(source))) as Picture;
            return (result);
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            //Deserialize info from bing
            Picture picture = Deserialize();
            string sURL = "https://www.bing.com/" + picture.items.Path;
            //show image
            WebRequest req = WebRequest.Create(sURL);
            WebResponse res = req.GetResponse();
            Stream imgStream = res.GetResponseStream();
            Image img1 = Image.FromStream(imgStream);
            pictureBox1.Image = new Bitmap(img1, pictureBox1.Size);

            label1.Text = picture.items.Name;
            
        }

        public void SendMail(string destinationMail)
        {
            Picture picture = Deserialize();
            string sURL = "https://www.bing.com/" + picture.items.Path;
            string htmlBody = "<html><body><h1>Picture</h1><br><img src=\""+ sURL + " \" width=\"500px\" height=\"300px\"></body></html>";
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

            MailMessage msg = new MailMessage();

            msg.AlternateViews.Add(avHtml);
            msg.From = new MailAddress("admin@gmail.com");
            msg.To.Add(destinationMail);
            msg.Subject = "Bing image";
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("donikanton7@gmail.com", "okijuhyg1234");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                MessageBox.Show("Mail has been successfully sent!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail Has error" + ex.Message);
               
            }
            finally
            {
                msg.Dispose();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please, enter the mail");
            }
            else
            {
                SendMail(textBox1.Text);
            }
        }
    }
}
