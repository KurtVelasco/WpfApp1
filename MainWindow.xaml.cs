using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinalTerm_Project_EMS
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        private string LastName;
        private string FirstName;
        private string Email;
        private string HomeAddress;
        private string Contact;
        private string GetEmail;
        private string Password;
        private string MiddleName;
        private int Department;
        private int Position;
        private int Status;
        private int ScheduleType;


        private DateTime? Birthday;
        private DateTime? EmployedOn;

        public MainWindow()
        {
            InitializeComponent();
        }

        //
        //Ignore the Database since just look through the WPF insertion
        //All those red are because of the lack of database so
        EmployeeDatabaseDataContext db = new EmployeeDatabaseDataContext(Properties.Settings.Default.MockEMSDatabaseConnectionString);

        //Example of using the Insertion of Input including the output of the ID

        //    CREATE PROCEDURE InsertDataAndGetID
        //    @Param1 VARCHAR(50),
        //    @Param2 INT,
        //    @NewID INT OUTPUT
        //AS
        //BEGIN
        //    SET NOCOUNT ON;

        //    INSERT INTO YourTable(Column1, Column2)
        //    VALUES(@Param1, @Param2);

        //        SET @NewID = SCOPE_IDENTITY();
        //        END

        private void Button_AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            //Since middle name is optional just add N/A on the tex
            if (Textbox_MiddleName.Text.Length < 1)
            {
                Textbox_MiddleName.Text = "N/A";
            }

            //Here make sure to cheeck the each field if theres an input, use the bool isValid to check
            // and go through each element
            bool isValid = true;           

            if (isValid)
            {
                MessageBoxResult ms = MessageBox.Show("Add Employee to the Database?", "Add Employee?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ms == MessageBoxResult.Yes)
                {
                    AddEmployeeDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please input all fields", "Missing field", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void AddEmployeeDatabase()
        {
            //Get each element from each text, and store them through each
            //variable,


            LastName = Textbox_LastName.Text;
            FirstName = Textbox_FirstName.Text;
            Email = Textbox_Email.Text;
            HomeAddress = Textbox_HomeAddress.Text;
            Contact = Textbox_Contact.Text;
            Password = Textbox_Password.Text;
            MiddleName = Textbox_MiddleName.Text;
            Department = ((KeyValuePair<int, string>)Combobox_Department.SelectedItem).Key;
            Position = ((KeyValuePair<int, string>)Combobox_Position.SelectedItem).Key;
            ScheduleType = ((KeyValuePair<int, string>)Combobox_ScheduleType.SelectedItem).Key;
            Birthday = DatePicker_Birthday.SelectedDate;
            EmployedOn = DatePicker_EmployedOn.SelectedDate;


            //Have a usp to check if you have information that could be duplicates
            //Im not sure what you guys have data that should be duplicate but in any case just have an if statment
            List<USP_CHECK_DUPLICATE_EMAILSResult> checkEmployee = db.USP_CHECK_DUPLICATE_EMAILS(Textbox_Email.Text).ToList();
            if (checkEmployee.Count > 0)
            {
                MessageBox.Show("Email already exist in the database", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                //In this case in the USP, the input should be similar to the selected input here in the code behind
                //also make sure its on order
                db.USP_INSERT_EMPLOYEE(FirstName, MiddleName, LastName, Birthday, Contact, Email, HomeAddress, Department, Position, 1, ScheduleType, Password, EmployedOn);
                MessageBox.Show("Added Employee with the Email: " + Email, "Added Employee" + LastName + ", " + FirstName, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


    }
}