using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace naptar
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;

        public static List<Event> events;

        static DateTime now = DateTime.Now;
        static int year = now.Year;
        static int month = now.Month;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            PositionTopRight();

            // Form a desktophoz kötése
            IntPtr desktopHandle = GetDesktopWindow();
            SetParent(this.Handle, desktopHandle);

            // Mindig a legalacsonyabb szinten tartás
            SetWindowPos(this.Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);

            // Átlátszó háttér beállítása (opcionális)
            this.BackColor = System.Drawing.Color.Lime;
            this.TransparencyKey = System.Drawing.Color.Lime;

            // Form stílusának beállítása
            this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;
            this.TopMost = false;
            this.ShowInTaskbar = false;


            InitializeEvents(); 
            DisplayDays();
            
        }
        public static void InitializeEvents()
        {
            events = SQLiteDataAccess.LoadEventsPartialParameters();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            //SetWindowPos(this.Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
            PositionTopRight() ;
        }

        private void PositionTopRight()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            // Form méretei
            int formWidth = this.Width;
            int formHeight = this.Height;

            // Pozíció kiszámítása a jobb felső sarokba
            int newX = screenWidth - formWidth;
            int newY = 0;

            // SetWindowPos használata konkrét koordinátákkal
            SetWindowPos(this.Handle, IntPtr.Zero, newX, newY, formWidth, formHeight, SWP_NOACTIVATE | SWP_SHOWWINDOW);
        }


        public void DisplayDays()
        {
            events.Clear();
            InitializeEvents();

            int daysInMonth = DateTime.DaysInMonth(year, month);

            DateTime startOfTheMonth = new DateTime(year, month, 1);
            int starterDay = Convert.ToInt32(startOfTheMonth.DayOfWeek.ToString("d"));
            if (starterDay == 0) { starterDay = 7; }

            string monthName = new CultureInfo("hu-HU").DateTimeFormat.GetMonthName(month);
            label8.Text = year.ToString() + " " + monthName;

            for (int i = 1; i < starterDay; i++)
            {
                UCBlank ucb = new UCBlank();
                dayContainer.Controls.Add(ucb);
            }

            string currentDate = string.Empty;
            for (int i = 1; i <= daysInMonth; i++)
            {
                UCDays ucd = new UCDays();
                ucd.dayEnumerator(i);
                dayContainer.Controls.Add(ucd);

                if (month < 10 && i < 10) { currentDate = year.ToString() + "-" + "0" + month + "-" + "0" + i; }
                else if (month < 10 && i >= 10) { currentDate = year.ToString() + "-" + "0" + month + "-" + i; }
                else if (month >= 10 && i < 10) { currentDate = year.ToString() + "-" + month + "-" + "0" + i; }
                else { currentDate = year.ToString() + "-" + month + "-" + i; }

                ucd.EventLabel = SQLiteDataAccess.LoadEventByDay(events, currentDate);
                

            }  
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dayContainer.Controls.Clear();

            month++;
            if (month > 12) 
            { 
                year++; 
                month = 1; 
            }
            DisplayDays();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            dayContainer.Controls.Clear();

            month--;
            if (month == 0)
            {
                year--;
                month = 12;
            }
            DisplayDays();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEventForm aef = new AddEventForm(this);
            aef.ShowDialog();

            string deleteFromDate = string.Empty;
            int monthMinusOne = DateTime.Now.Month - 1;
            int yearOfDelete = DateTime.Now.Year;

            if (monthMinusOne == 0)
            {
                yearOfDelete--; monthMinusOne = 12;
                deleteFromDate = yearOfDelete + "-" + monthMinusOne + "-25";
            }
            else if (monthMinusOne < 10)
            {
                deleteFromDate = yearOfDelete + "-0" + monthMinusOne + "-25";
            }
            else { deleteFromDate = yearOfDelete + "-" + monthMinusOne + "-25"; }

            SQLiteDataAccess.DeleteOldEvents(deleteFromDate);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            RemoveEventForm reform = new RemoveEventForm(this);
            reform.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            dayContainer.Controls.Clear();
            DisplayDays();
        }

        public void RefreshButtonClick()
        {
            btnRefresh.PerformClick();
        }

        
    }
}
