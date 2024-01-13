using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace RVP_3_Yurlova
{
    public partial class Form : System.Windows.Forms.Form
    {
        private BindingList<IBrandAvto> brands = new BindingList<IBrandAvto>();

        private BindingSource carBindingSource = new BindingSource();

        private List<Transport> reciveServerList = new List<Transport>();

        Socket socket = null;

        int countRecive = 0;

        public Form()
        {
            InitializeComponent();
            InitDataGridView();
            InitializeCarBrandsList();
            InitializeDataGridViewCars();
        }
        private void InitializeCarBrandsList()
        {
            brands = new BindingList<IBrandAvto>();

            // Добавим несколько примеров марок
            brands.Add(new CPassengerCar("BMW", "x5", 100, 200));
            brands.Add(new CPassengerCar("Lada", "granta", 45, 54));
            brands.Add(new CTruckCar("Volvo", "gfdgd", 123, 455));
            brands.Add(null);

            carBindingSource.DataSource = brands;
            

        }
        private void InitDataGridView()
        {
            dataGridViewCarBrands.AutoGenerateColumns = false;

            dataGridViewCarBrands.DataSource = carBindingSource;

            DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn.Name = "Type";
            dataGridViewComboBoxColumn.HeaderText = "Тип";
            dataGridViewComboBoxColumn.Items.Add("Passenger");
            dataGridViewComboBoxColumn.Items.Add("Truck");
            dataGridViewComboBoxColumn.Items.Add("Bus");

            dataGridViewCarBrands.Columns.Add(dataGridViewComboBoxColumn);
            dataGridViewCarBrands.Columns.Add("BrandName", "Бренд");
            dataGridViewCarBrands.Columns.Add("ModelName", "Модель");
            dataGridViewCarBrands.Columns.Add("Horsepower", "Лошадиные силы");
            dataGridViewCarBrands.Columns.Add("MaxSpeed", "Максимальная скорость");

            foreach (DataGridViewColumn column in dataGridViewCarBrands.Columns)
            {
                column.DataPropertyName = column.Name;
            }

            

        }


        private void UpdateComboBoxColumn()
        {

            int columnInd = dataGridViewCarBrands.Columns["Type"].Index;
            for (int i = 0; i < brands.Count - 1; i++)
            {
                IBrandAvto brand = brands[i];

                DataGridViewRow row = dataGridViewCarBrands.Rows[brands.IndexOf(brand)];
                if (brand is CPassengerCar) row.Cells[columnInd].Value = "Passenger";
                if (brand is CTruckCar) row.Cells[columnInd].Value = "Truck";
                if (brand is BusCar) row.Cells[columnInd].Value = "Bus";


            }
        }
        private void UpdateColorRow()
        {
            int columnInd = dataGridViewCarBrands.Columns["Type"].Index;
            for (int i = 0; i < brands.Count - 1; i++)
            {
                IBrandAvto brand = brands[i];
                DataGridViewRow row = dataGridViewCarBrands.Rows[brands.IndexOf(brand)];
                if (brand is CPassengerCar) row.DefaultCellStyle.BackColor = Color.Pink;
                if (brand is CTruckCar) row.DefaultCellStyle.BackColor = Color.Orange;
                if (brand is BusCar) row.DefaultCellStyle.BackColor = Color.Blue;


            }
        }
        private void CreateNewBrandRow(DataGridViewCellEventArgs e)
        {
            int comboBoxIndex = dataGridViewCarBrands.Columns["Type"].Index;
            if (e.ColumnIndex == comboBoxIndex)
            {
                string valueOfComboBox = dataGridViewCarBrands.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();
                brands.Remove(null);
                if (valueOfComboBox == "Passenger") brands.Add(new CPassengerCar());
                if (valueOfComboBox == "Truck") brands.Add(new CTruckCar());
                if (valueOfComboBox == "Bus") brands.Add(new BusCar());
            }

            brands.Add(null);
            UpdateComboBoxColumn();

            EditBrandRow(e);
        }

        private void EditBrandRow(DataGridViewCellEventArgs e)
        {
            int comboBoxIndex = dataGridViewCarBrands.Columns["Type"].Index;

            if (e.ColumnIndex == comboBoxIndex)
            {
                string valueOfComboBox = dataGridViewCarBrands.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();
                IBrandAvto editBrand = brands[e.RowIndex];

                if (valueOfComboBox == "Passenger") brands[e.RowIndex] = new CPassengerCar(editBrand.BrandName, editBrand.ModelName, editBrand.Horsepower, editBrand.MaxSpeed);
                if (valueOfComboBox == "Truck") brands[e.RowIndex] = new CTruckCar(editBrand.BrandName, editBrand.ModelName, editBrand.Horsepower, editBrand.MaxSpeed);
                if (valueOfComboBox == "Bus") brands[e.RowIndex] = new BusCar(editBrand.BrandName, editBrand.ModelName, editBrand.Horsepower, editBrand.MaxSpeed);
                Loader.RemoveBrand(brands[e.RowIndex].BrandName);


            }
            else
            {
                string value = dataGridViewCarBrands.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                switch (e.ColumnIndex)
                {
                    case 1:
                        {
                            brands[e.RowIndex].BrandName = value;
                            break;
                        }
                    case 2:
                        {
                            brands[e.RowIndex].ModelName = value;
                            break;
                        }
                    case 3:
                        {
                            brands[e.RowIndex].Horsepower = int.Parse(value);
                            break;
                        }
                    case 4:
                        {
                            brands[e.RowIndex].MaxSpeed = int.Parse(value);
                            break;
                        }
                }

            }
        }

        private void dataGridViewCarBrands_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridViewCarBrands.Rows.Count - 1 && e.ColumnIndex == dataGridViewCarBrands.Columns["Type"].Index)
            {
                CreateNewBrandRow(e);
            }
            else if (e.RowIndex == dataGridViewCarBrands.Rows.Count - 1 && e.ColumnIndex != dataGridViewCarBrands.Columns["Type"].Index)
            {
                MessageBox.Show("Выберите значение в поле Type!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                brands.Remove(null);
                brands.Add(null);
            }
            else
            {

                EditBrandRow(e);
            }
            UpdateComboBoxColumn();
            UpdateColorRow();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            UpdateComboBoxColumn();
            UpdateColorRow();
        }

        private void dataGridViewCarBrands_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Неверный ввод!", "DataError");
            dataGridViewCarBrands.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
        }

        private void SerializeXml(string fileName)
        {

            List<CBrandAvto> data = new List<CBrandAvto>();
            foreach (IBrandAvto brand in brands)
            {
                if (brand != null) { data.Add(brand as CBrandAvto); }
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CBrandAvto>), new Type[]
                {
                    typeof(CPassengerCar),
                    typeof(CTruckCar),
                    typeof(BusCar),
                });
                FileStream writer = new FileStream(fileName, FileMode.Create);//поток

                serializer.Serialize(writer, data);
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeserializeXml(string fileName)
        {
            brands.Clear();
            List<CBrandAvto> data = new List<CBrandAvto>();


            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CBrandAvto>), new Type[]
                {
                    typeof(CPassengerCar),
                    typeof(CTruckCar),
                    typeof(BusCar),

                });
                
                FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate);
                data = serializer.Deserialize(writer) as List<CBrandAvto>;
                writer.Close();
                foreach(CBrandAvto brand in data)
                {
                    if(brand is CPassengerCar)
                    {
                        
                        brands.Add(new CPassengerCar(brand.BrandName,brand.ModelName,brand.Horsepower,brand.MaxSpeed));
                    }
                    else if(brand is CTruckCar)
                    {
                        brands.Add(new CTruckCar(brand.BrandName, brand.ModelName, brand.Horsepower, brand.MaxSpeed));
                    }
                    else if (brand is BusCar)
                    {
                        brands.Add(new BusCar(brand.BrandName, brand.ModelName, brand.Horsepower, brand.MaxSpeed));
                    }
                }

                brands.Add(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        //private void buttonLoad_Click(object sender, EventArgs e)
        //{
        //    using(OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {
        //        openFileDialog.Filter = "XML Files|*.xml";
        //        if(openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            DeserializeXml(openFileDialog.FileName);
        //        }
        //    }
            
        //    UpdateColorRow();
        //    UpdateComboBoxColumn();
            
        //}

        //private void buttonSave_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {
        //        openFileDialog.Filter = "XML Files|*.xml";
        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            SerializeXml(openFileDialog.FileName);
        //        }
        //    }
            
        //    MessageBox.Show("Сохранено!");
        //}

        //private void buttonExit_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        private void InitializeDataGridViewCars()
        {
            dataGridViewCars.Visible = false;
            dataGridViewCars.AutoGenerateColumns = false;
            dataGridViewCars.AllowUserToAddRows = false;
            dataGridViewCars.Columns.Add("", "");
            dataGridViewCars.Columns.Add("", "");
            dataGridViewCars.Columns.Add("", "");

            
        }

        private void AddTextColumnForTrucks()
        {
            dataGridViewCars.Columns.Add("RegistrationNumber", "Регистрационный номер");
            dataGridViewCars.Columns.Add("WheelCount", "Количество Колес");
            dataGridViewCars.Columns.Add("BodyVolume", "Обьем");

            dataGridViewCars.Tag = "Truck";
        }

        private void AddTextColumnForCars()
        {
            dataGridViewCars.Columns.Add("RegistrationNumber", "Регистрационный номер");
            dataGridViewCars.Columns.Add("NamedMultimedia", "Название мультимедиа");
            dataGridViewCars.Columns.Add("AirbagCount", "Количество подушек безопасности");

            dataGridViewCars.Tag = "Passenger";
        }
        private void AddTextColumnForBus()
        {
            dataGridViewCars.Columns.Add("RegistrationNumber", "Регистрационный номер");
            dataGridViewCars.Columns.Add("CountSeats", "Количество сидений");
            dataGridViewCars.Columns.Add("MaxPeoples", "Максимальное кол-во пассажиров");

            dataGridViewCars.Tag = "Bus";
        }
        private void ClearDataGridViewCars()
        {
            dataGridViewCars.Visible = false;
            dataGridViewCars.Rows.Clear();
            dataGridViewCars.Columns.Clear();
        }

        private void dataGridViewCarBrands_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dataGridViewCarBrands.Enabled = false;
            
            ClearDataGridViewCars();

            if (e.RowIndex >= 0 && dataGridViewCarBrands.SelectedRows.Count == 1 && e.RowIndex != dataGridViewCarBrands.RowCount - 1)
            {
                //ShowTableCars(e);
                SocketConnect();

                
                int indexSelectedRow = IndexSectedRow();
                int comboBoxIndex = dataGridViewCarBrands.Columns["Type"].Index;
                string valueOfComboBox = dataGridViewCarBrands.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();

                string brandName = brands[indexSelectedRow].BrandName;

                SocketSendMessage(brandName, valueOfComboBox);

                SynchronizationContext syncContext = SynchronizationContext.Current;
                Task task = СonnectRecive(syncContext, e);
            }
            else if (e.RowIndex == dataGridViewCarBrands.RowCount - 1)
            {
                dataGridViewCarBrands.Enabled = true;
                
            }
            else
            {
                ClearDataGridViewCars();
            }
        }

        private int IndexSectedRow()
        {
            int indexSelectedRow = -1;
            foreach (DataGridViewRow row in dataGridViewCarBrands.SelectedRows)
            {
                indexSelectedRow = row.Index;
            }
            return indexSelectedRow;
        }

        private void ShowTableCars(DataGridViewCellMouseEventArgs e)
        {
            dataGridViewCars.Visible = true;
           


            int comboBoxIndex = dataGridViewCarBrands.Columns["Type"].Index;
            string valueOfComboBox = dataGridViewCarBrands.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();

            int indexSelectedRow = IndexSectedRow();
            string brandName = brands[indexSelectedRow].BrandName;


            if (valueOfComboBox == "Passenger")
            {
                AddTextColumnForCars();
                
            }
            if (valueOfComboBox == "Truck")
            {
                AddTextColumnForTrucks();
                
            }
            if (valueOfComboBox == "Bus")
            {
                AddTextColumnForBus();

            }


            LoadData(brandName, valueOfComboBox);
        }

        private async void LoadData(string brandName, string type)
        {
            ProgressBarForm progressBarForm = new ProgressBarForm();
            progressBarForm.BrandName = brandName;
            progressBarForm.Show();

            await Loader.Load(brandName, type);

            progressBarForm.Close();

            if (type == "Passenger") LoadCars();
            if (type == "Truck") LoadTrucks();
            if (type == "Bus") LoadBus();

            dataGridViewCarBrands.Enabled = true;
            
        }

        private void LoadCars()
        {
            int indexSelectedRow = IndexSectedRow();

            CPassengerCar brand = (CPassengerCar)brands[indexSelectedRow];
            string brandName = brand.BrandName;

            List<Transport> dataCars = Loader.GetData(brandName);

            if (dataCars.Count > 0)
            {
                foreach (CCar car in dataCars)
                {
                    dataGridViewCars.Rows.Add(car.RegistNum, car.MultiMedia, car.AirbagCount.ToString());
                }
            }
        }

        private void LoadTrucks()
        {
            int indexSelectedRow = IndexSectedRow();
            
            CTruckCar brand = (CTruckCar)brands[indexSelectedRow];
            string brandName = brand.BrandName;

            List<Transport> dataTrucks = Loader.GetData(brandName);
            
            if (dataTrucks.Count > 0)
            {
                foreach (CTruck truck in dataTrucks)
                {
                    dataGridViewCars.Rows.Add(truck.RegistNum, truck.WheelCount.ToString(), truck.BodyVolume.ToString());
                }
            }
        }
        private void LoadBus()
        {
            int indexSelectedRow = IndexSectedRow();

            BusCar brand = (BusCar)brands[indexSelectedRow];
            string brandName = brand.BrandName;

            List<Transport> dataBus = Loader.GetData(brandName);

            if (dataBus.Count > 0)
            {
                foreach (CBus bus in dataBus)
                {
                    dataGridViewCars.Rows.Add(bus.RegistNum, bus.CountSeats.ToString(), bus.MaxPeoples.ToString());
                }
            }
        }



        private void LoadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DeserializeXml(openFileDialog.FileName);
                }
            }

            UpdateColorRow();
            UpdateComboBoxColumn();
        }

        private void SaveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SerializeXml(openFileDialog.FileName);
                }
            }

            MessageBox.Show("Сохранено!");
        }

        private void ExitToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SocketConnect()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipEndPoint);
                //MessageBox.Show($"Подключение к {socket.RemoteEndPoint} установлено");
            }
            catch (SocketException)
            {
                MessageBox.Show("Не удалось установить подключение с сервером");
            }
        }
        private void SocketSendMessage(string brandName, string type)
        {
            byte[] buf = new byte[1024];

            string xmlData = StringXMLData(brandName, type);
            byte[] msg = Encoding.ASCII.GetBytes(xmlData);

            // Отправляем данные через сокет
            int bytesSent = socket.Send(msg);
        }

        private string StringXMLData(string brandName, string type)
        {
            List<string> dataList = new List<string>() { brandName, type };
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            XmlDocument xmlDocument = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())//поток
            {
                serializer.Serialize(ms, dataList);//серилизуем данные в поток

                ms.Position = 0; // переходим в начало потока
                // Загружаем данные из потока в XML документ
                xmlDocument.Load(ms);
            }

            string xmlData;
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlDocument.Save(stringWriter);
                // Получаем строку с XML данными
                xmlData = stringWriter.ToString();
            }
            return xmlData;
        }
        private void ReceiveModels(string xmlData)
        {
            // Создание XML-документа и загрузка данных
            XmlSerializer serializer = new XmlSerializer(typeof(List<Transport>), new Type[]
               {
                    typeof(Transport),
                    typeof(CTruck),
                    typeof(CCar),
                    typeof(CBus)
               });
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            using (StringReader stringReader = new StringReader(xmlDocument.OuterXml))
            {
                reciveServerList = (List<Transport>)serializer.Deserialize(stringReader);
            }
        }
        private async Task СonnectRecive(SynchronizationContext syncContext, DataGridViewCellMouseEventArgs e)
        {
            dataGridViewCarBrands.Enabled = false;
            
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Visible = true;
            label1.Visible = true;
            await Task.Run(() =>
            {
                byte[] bytes = new byte[4096];

                int indexSelectedRow = IndexSectedRow();
                int comboBoxIndex = dataGridViewCarBrands.Columns["Type"].Index;
                string valueOfComboBox = dataGridViewCarBrands.Rows[e.RowIndex].Cells[comboBoxIndex].Value.ToString();

                string brandName = brands[indexSelectedRow].BrandName;

              
                try
                {
                    string xmlData;
                    while (true)
                    {
                        int bytesRec = socket.Receive(bytes);

                        if (bytesRec > 0)
                        {
                            // Конвертация полученных байтов в строку XML

                            xmlData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            ReceiveModels(xmlData);
                            Control control = new Control();
                            syncContext.Post(_ =>
                            {
                                ClearDataGridViewCars();
                                CreateCarTable();
                            }, null);

                        }
                        else break;
                    }


                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();




                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show("ArgumentNullException : " + ane.ToString(), "ArgumentNullException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("SocketException : {0}" + se.ToString(), "SocketException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception en)
                {
                    MessageBox.Show("Unexpected exception : {0}" + en.ToString(), "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            });
            dataGridViewCars.Visible = true;
            progressBar1.Value = 100;
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            label1.Visible = false;
            dataGridViewCarBrands.Enabled = true;
           
        }

        private void CreateCarTable()
        {
            progressBar1.Value = (int)(((double)reciveServerList.Count / 21) * 100);

            
            int comboBoxIndex = dataGridViewCarBrands.Columns["Type"].Index;
            int indexSelectedRow = IndexSectedRow();
            string valueOfComboBox = dataGridViewCarBrands.Rows[indexSelectedRow].Cells[comboBoxIndex].Value.ToString();


            string brandName = brands[indexSelectedRow].BrandName;



           

            if (valueOfComboBox == "Passenger")
            {
                AddTextColumnForCars();
                
            }
            if (valueOfComboBox == "Truck")
            {
                AddTextColumnForTrucks();
                
            }
            if (valueOfComboBox == "Bus")
            {
                AddTextColumnForBus();
                
            }

            if (reciveServerList.Count > 0)
            {
                foreach (Transport vehicles in reciveServerList)
                {
                    if (vehicles is CCar)
                    {
                        CCar car = (CCar)vehicles;
                        dataGridViewCars.Rows.Add(car.RegistNum, car.MultiMedia, car.AirbagCount.ToString());
                    }
                    if (vehicles is CTruck)
                    {
                        CTruck truck = (CTruck)vehicles;
                        dataGridViewCars.Rows.Add(truck.RegistNum, truck.WheelCount.ToString(), truck.BodyVolume.ToString());
                    }
                    if (vehicles is CBus)
                    {
                        CBus bus = (CBus)vehicles;
                        dataGridViewCars.Rows.Add(bus.RegistNum, bus.CountSeats.ToString(), bus.MaxPeoples.ToString());
                    }

                }
            }
        }
    }

    
}


