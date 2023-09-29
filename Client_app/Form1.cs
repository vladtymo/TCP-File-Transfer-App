using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Shared;
using System.Text.Json;

namespace Client_app
{
    public partial class Form1 : Form
    {
        public string FilePath { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        // вибираємо файл для відправки
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                // зберігаємо шлях вибраного файла
                FilePath = fileDialog.FileName;
                fileNameLabel.Text = FilePath;
            }
        }

        // відправляємо вибраний файл на сервер
        private void button1_Click(object sender, EventArgs e)
        {
            // перевіряємо чи існує вибраний файл
            if (string.IsNullOrWhiteSpace(FilePath)) return;

            // створюємо кінцеву точку відправки
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipTextBox.Text), int.Parse(portTextBox.Text));
            TcpClient client = new TcpClient();

            try
            {
                // виконуємо підключення
                client.Connect(endPoint);

                // transfering data using NetworkStream from TcpClient
                //NetworkStream ns = client.GetStream();

                //ns.Write(); // send data to remote end point
                //ns.Read();  // recieve data from remote end point

                // створюємо клас, який містить інформацію про файл
                FileTransferInfo info = new FileTransferInfo();
                info.Name = Path.GetFileName(FilePath);

                // створюємо потік файла
                //using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                //{
                //    // зчитуємо файл в масив байтів
                //    byte[] fileData = new byte[fs.Length];
                //    fs.Read(fileData, 0, fileData.Length);

                //    // встановлюємо вміст файла
                //    info.Data = fileData;
                //}
                info.Data = File.ReadAllBytes(FilePath);

                // на цьому етапі в нас є повністю сформований клас,
                // який містить ім'я та вміст вибраного файла

                // відправка даних
                //BinaryFormatter serializer = new BinaryFormatter();
                using (NetworkStream stream = client.GetStream())
                {
                    // серіалізуємо об'єкт класа
                    // та відправляємо його на сервер
                    JsonSerializer.Serialize(stream, info);
                    //serializer.Serialize(stream, info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}