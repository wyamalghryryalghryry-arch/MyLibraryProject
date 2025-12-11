using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLibraryProject
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
        
                // اسم مستخدم وكلمة مرور ثابتة للتجربة
                if (txtUsername.Text == "AWN" && txtPassword.Text == "1234567")
                {
                    // إذا كانت البيانات صحيحة، قم بإخفاء نافذة تسجيل الدخول الحالية
                    this.Hide();

                    // قم بإنشاء وفتح نافذة المكتبة الرئيسية (Form1)
                    Form1 mainForm = new Form1();
                    mainForm.Show();
                }
                else
                {
                    // إذا كانت البيانات خاطئة، أظهر رسالة خطأ
                    MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة.", "خطأ في الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }

