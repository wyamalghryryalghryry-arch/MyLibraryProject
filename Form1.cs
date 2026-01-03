using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing; // مهم لإضافة الشفافية

namespace MyLibraryProject // تأكد أن هذا هو اسم مشروعك
{
    public partial class Form1 : Form
    {
        // !!! انتبه: هذا السطر هو سر الاتصال بقاعدة البيانات
        string connectionString = @"Data Source=الشتاء;Initial Catalog=BookManagerDB;Integrated Security=True";


        public Form1()
        {
            InitializeComponent();
        }

        // === هذه الوظيفة تعمل أول ما يفتح البرنامج ===
        private void Form1_Load(object sender, EventArgs e)
        {
            // تحميل الكتب من قاعدة البيانات
            LoadBooks();

            // لمسة جمالية: جعل خلفية العناوين شفافة
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
        }
        // === وظائف مساعدة ===

        private void LoadBooks()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل الاتصال بقاعدة البيانات. تأكد من أن اسم السيرفر صحيح وأن خدمة SQL Server تعمل.\n" + ex.Message, "خطأ اتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearTextBoxes()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtYear.Clear();
            txtGenre.Clear();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            // التأكد من أن الحقول ليست فارغة
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text) || string.IsNullOrWhiteSpace(txtYear.Text))
            {
                MessageBox.Show("الرجاء ملء حقول العنوان والمؤلف والسنة على الأقل.", "حقول فارغة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Books (Title, Author, Year, Genre) VALUES (@Title, @Author, @Year, @Genre)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                        cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                        cmd.Parameters.AddWithValue("@Year", int.Parse(txtYear.Text));
                        cmd.Parameters.AddWithValue("@Genre", txtGenre.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("تمت إضافة الكتاب بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBooks(); // تحديث الجدول
                        ClearTextBoxes(); // تنظيف مربعات النص
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("الرجاء إدخال سنة النشر كأرقام فقط.", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // رسالة تأكيد قبل الحذف
                var confirmResult = MessageBox.Show("هل أنت متأكد من أنك تريد حذف هذا الكتاب؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookID"].Value);

                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            string query = "DELETE FROM Books WHERE BookID = @BookID";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@BookID", bookId);
                                con.Open();
                                cmd.ExecuteNonQuery();

                                MessageBox.Show("تم حذف الكتاب بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadBooks(); // تحديث الجدول
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("حدث خطأ أثناء الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("الرجاء تحديد كتاب من الجدول لحذفه أولاً.", "لم يتم التحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void editbutton1_Click(object sender, EventArgs e)
        {
           
            {
                // التأكد من أن المستخدم اختار صفاً
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    try
                    {
                        // --- الجزء الأول: تحديث الجدول الظاهر أمام المستخدم (كما فعلنا سابقاً) ---
                        DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                        selectedRow.Cells["Title"].Value = txtTitle.Text;
                        selectedRow.Cells["Author"].Value = txtAuthor.Text;
                        selectedRow.Cells["Year"].Value = txtYear.Text;
                        selectedRow.Cells["Genre"].Value = txtGenre.Text;

                        // --- الجزء الثاني (الجديد والمهم): حفظ التغييرات في قاعدة البيانات ---

                        // 1. الحصول على الرقم التعريفي للكتاب الذي نريد تعديله
                        int bookID = Convert.ToInt32(selectedRow.Cells["BookID"].Value); // تأكد من أن اسم العمود "BookID" صحيح

                        // 2. تعريف جملة الاتصال بقاعدة البيانات (استبدلها بجملة الاتصال الخاصة بك)
                        string connectionString = "Data Source=الشتاء;Initial Catalog=BookManagerDB;Integrated Security=True;";

                        // 3. كتابة أمر التحديث (UPDATE)
                        string query = "UPDATE Books SET Title = @Title, Author = @Author, Year = @Year, Genre = @Genre WHERE BookID = @BookID";
                        // استبدل "Books" باسم جدول الكتب الفعلي في قاعدة بياناتك

                        // 4. تنفيذ الأمر
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // إضافة الباراميترات لمنع SQL Injection
                                command.Parameters.AddWithValue("@Title", txtTitle.Text);
                                command.Parameters.AddWithValue("@Author", txtAuthor.Text);
                                command.Parameters.AddWithValue("@Year", Convert.ToInt32(txtYear.Text)); // تحويل السنة إلى رقم
                                command.Parameters.AddWithValue("@Genre", txtGenre.Text);
                                command.Parameters.AddWithValue("@BookID", bookID);

                                connection.Open(); // فتح الاتصال
                                command.ExecuteNonQuery(); // تنفيذ أمر التحديث
                                connection.Close(); // إغلاق الاتصال
                            }
                        }

                        // 5. عرض رسالة النجاح النهائية
                        MessageBox.Show("تم تحديث البيانات بنجاح في الجدول وقاعدة البيانات.");
                    }
                    catch (Exception ex)
                    {
                        // عرض أي خطأ يحدث أثناء العملية
                        MessageBox.Show("حدث خطأ أثناء حفظ التعديلات: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("يرجى تحديد الكتاب الذي تريد تعديله أولاً.");
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // نتأكد أن المستخدم اختار سطراً كاملاً
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // نأخذ السطر الذي تم اختياره
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // نملأ مربعات النص بالبيانات من خلايا هذا السطر
                // (نستخدم الأسماء الصحيحة من مشروعك)
                txtTitle.Text = selectedRow.Cells["Title"].Value.ToString();
                txtAuthor.Text = selectedRow.Cells["Author"].Value.ToString();
                txtYear.Text = selectedRow.Cells["Year"].Value.ToString();
                txtGenre.Text = selectedRow.Cells["Genre"].Value.ToString();
            }
        }
    }
}

      

     

    

