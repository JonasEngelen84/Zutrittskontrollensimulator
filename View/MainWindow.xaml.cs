using System;
using OBS_Zutrittskontrollen_Simulation.Models;
using OBS_Zutrittskontrolle.Models;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace OBS_Zutrittskontrollen_Simulator.View
{
    public partial class MainWindow : Window
    {
        // Saves Transponder from database
        private List<Transponder> TransponderList { get; set; }
        // Saves Terminal from database
        private List<Terminal> TerminalList { get; set; }
        // Saves Access from database
        private List<Access> AccessList { get; set; }
        // Saves Results from checking Access
        private List<Result> ResultList { get; set; }

        // Saves current Transponder
        private Transponder CurrentTransponder { get; set; }
        // Saves current Terminal
        private Terminal CurrentTerminal { get; set; }
        // Saves last chosen Transponder
        private Button lastTransponder;
        // Saves last chosen Terminal
        private Button lastTerminal;


        public MainWindow()
        {
            InitializeComponent();

            TerminalList = new();
            TransponderList = new();
            AccessList = new();
            ResultList = new();

            FillTransponderList();
            FillTerminalList();
            FillAccessList();

            // Set current date
            SetDate.SelectedDate = DateTime.Now;
        }

        // Get Data from Transponder and Employee Table
        public void FillTransponderList()
        {
            DataTable table = Database.GetTable("SELECT * FROM Transponder " +
                "INNER JOIN Employee ON (Transponder.Transponder_Id = Employee.Transponder_Id)");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                TransponderList.Add(new Transponder(
                    (int)table.Rows[i]["Transponder_Id"],
                    table.Rows[i]["Forename"].ToString(),
                    table.Rows[i]["Surname"].ToString(),
                    (int)table.Rows[i]["Employee_Id"]));
            }
        }

        // Get Data from Terminal Table
        public void FillTerminalList()
        {
            DataTable table = Database.GetTable("SELECT * FROM Terminal");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                TerminalList.Add(new Terminal(
                    (int)table.Rows[i]["Terminal_Id"],
                    (int)table.Rows[i]["Terminal_Type"],
                    table.Rows[i]["Terminal_Place"].ToString()));
            }
        }

        // Get Data from Access Table
        public void FillAccessList()
        {
            DataTable table = Database.GetTable("SELECT * FROM Access");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                AccessList.Add(new Access(
                    (int)table.Rows[i]["Transponder_Id"],
                    (int)table.Rows[i]["Terminal_Id"],
                    (DateTime)table.Rows[i]["AccessFrom"],
                    (DateTime)table.Rows[i]["AccessUntil"]));
            }
        }


        // Load Transponder with CheckBox in TransponderChoice-ListBox
        // Load Terminals with CheckBox in TerminalChoice-Listbox
        private void LoadTransponderAndTerminals(object sender, RoutedEventArgs e)
        {
            TerminalChoice.Items.Clear();
            TransponderChoice.Items.Clear();
            CheckBox checkAllTransponder = new();
            CheckBox checkAllTerminals = new();

            // Add CheckAllTransponder-Checkbox to TransponderChoice-ListBox
            checkAllTransponder.Content = "Alle markieren";
            checkAllTransponder.Checked += CheckAllTransponder;
            checkAllTransponder.Unchecked += UncheckAllTransponder;
            _ = TransponderChoice.Items.Add(checkAllTransponder);

            // Add Transponder-Checkboxes to TransponderChoice-ListBox
            foreach (Transponder x in TransponderList)
            {
                x.Checkbox.Content = x.Transponder_ID;
                x.Checkbox.Margin = new Thickness(15, 0, 0, 0);
                x.Checkbox.Checked += CheckSingleTransponder;
                x.Checkbox.Unchecked += UncheckSingleTransponder;
                x.Checkbox.IsChecked = false;
                _ = TransponderChoice.Items.Add(x.Checkbox);
            }

            // Add CheckAllTerminals-CheckBox to TerminalChoice-Listbox
            checkAllTerminals.Content = "Alle markieren";
            checkAllTerminals.Checked += CheckAllTerminals;
            checkAllTerminals.Unchecked += UncheckAllTerminals;
            _ = TerminalChoice.Items.Add(checkAllTerminals);

            // Add Terminal-Checkboxes to TerminalChoice-Listbox
            foreach (Terminal x in TerminalList)
            {
                x.Checkbox.Content = x.Terminal_ID;
                x.Checkbox.Margin = new Thickness(15, 0, 0, 0);
                x.Checkbox.Checked += CheckSingleTerminal;
                x.Checkbox.Unchecked += UncheckSingleTerminal;
                x.Checkbox.IsChecked = false;
                _ = TerminalChoice.Items.Add(x.Checkbox);
            }

            // Display Load-Menue
            LoadMenu.Visibility = Visibility.Visible;
        }


        // Set Image to checked Transponder / checked Terminals, add them to selectedTransponderList/selectedTerminalist
        // and bind these Lists to TransponderDisplay/TerminalDisplay for display
        private void AcceptSelection(object sender, RoutedEventArgs e)
        {
            CurrentTransponder = null;
            lastTransponder = null;
            CurrentTerminal = null;
            lastTransponder = null;

            List<Transponder> selectedTransponderList = new();

            foreach (Transponder x in TransponderList)
            {
                if ((bool)x.Checkbox.IsChecked)
                {
                    //x.Image = (BitmapImage)Application.Current.FindResource("Transponder");

                    BitmapImage bitmapImage = new();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(@"/Transponder.png", UriKind.RelativeOrAbsolute);
                    bitmapImage.EndInit();
                    x.Image = bitmapImage;
                    selectedTransponderList.Add(x);
                }
            }
            // Bind selectedTransponderList to TransponderDisplay
            TransponderDisplay.ItemsSource = selectedTransponderList;


            List<Terminal> SelectedTerminalList = new();

            foreach (Terminal x in TerminalList)
            {
                if ((bool)x.Checkbox.IsChecked)
                {
                    //x.Image = x.TerminalType.Equals(1)
                    //    ? (BitmapImage)Application.Current.FindResource("Terminal1")
                    //    : (BitmapImage)Application.Current.FindResource("Terminal2");

                    BitmapImage bitmapImage = new();
                    bitmapImage.BeginInit();

                    bitmapImage.UriSource = x.Terminal_Type == 1
                        ? new Uri(@"/Terminal1.png", UriKind.RelativeOrAbsolute)
                        : new Uri(@"/Terminal2.png", UriKind.RelativeOrAbsolute);

                    bitmapImage.EndInit();
                    x.Image = bitmapImage;
                    SelectedTerminalList.Add(x);
                }
            }
            // Bind SelectedTerminalList to TerminalDisplay
            TerminalDisplay.ItemsSource = SelectedTerminalList;

            LoadMenu.Visibility = Visibility.Hidden;
        }


        // Unmark the selected transponder, check for access if necessary and output the result
        private void SelectedTransponderClick(object sender, RoutedEventArgs e)
        {
            // If a transponder was already marked, this transponder is unmarked
            if (lastTransponder != null)
            {
                lastTransponder.Background = Brushes.White;
            }

            // Save selected transponder
            lastTransponder = sender as Button;
            CurrentTransponder = (Transponder)(sender as Button).DataContext;

            // Mark selected transponder
            (sender as Button).Background = Brushes.LightBlue;

            // If a terminal has already been selected => check for access and output the result
            if (CurrentTerminal != null)
            {
                AddResult();
            }
        }

        // Unmark the selected terminal, check for access if necessary and output the result
        private void SelectedTerminalClick(object sender, RoutedEventArgs e)
        {
            // If a terminal was already marked, this transponder is unmarked
            if (lastTerminal != null)
            {
                lastTerminal.Background = Brushes.White;
            }

            // Save selected terminal
            lastTerminal = sender as Button;
            CurrentTerminal = (Terminal)(sender as Button).DataContext;

            // Mark selected terminal
            (sender as Button).Background = Brushes.LightBlue;

            // If a transponder has already been selected => check for access and output the result
            if (CurrentTransponder != null)
            {
                AddResult();
            }
        }


        // Check correct time format of user input and add result to ResultIssue by correct format
        private void AddResult()
        {
            DateTime checkTime = new();
            bool acceptCheckTime = false;

            try
            {
                // Set CheckTime
                checkTime = new(SetDate.SelectedDate.Value.Year, SetDate.SelectedDate.Value.Month,
                    SetDate.SelectedDate.Value.Day, int.Parse(Hours.Text.ToString()),
                    int.Parse(Minutes.Text.ToString()), int.Parse(Seconds.Text.ToString()));

                acceptCheckTime = true;
            }
            catch
            {
                string messageBoxText = "Das Zeitformat wurde nicht korrekt angegeben!";
                string caption = "Falsches Zeitformat";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                _ = MessageBox.Show(messageBoxText, caption, button, icon);

                lastTransponder.Background = Brushes.White;
                lastTerminal.Background = Brushes.White;
                CurrentTransponder = null;
                lastTransponder = null;
                CurrentTerminal = null;
                lastTerminal = null;

            }

            // if correct time format, add result to ResultList and bind list to ResultIssue
            if (acceptCheckTime)
            {
                DateTime Time = new(0001, 01, 01, int.Parse(Hours.Text.ToString()),
                int.Parse(Minutes.Text.ToString()), int.Parse(Seconds.Text.ToString()));
                DateTime date = SetDate.SelectedDate.Value;

                ResultList.Add(new Result(CurrentTransponder.Transponder_ID, CurrentTerminal.Terminal_ID,
                    CurrentTerminal.Terminal_Place, date, Time, CheckAccess(CurrentTransponder, CurrentTerminal, checkTime),
                    CurrentTransponder.Employee_ID, CurrentTransponder.Surname, CurrentTransponder.Forename));

                ResultIssue.ItemsSource = null;
                ResultIssue.ItemsSource = ResultList;
            }
        }


        // Check for access
        private bool CheckAccess(Transponder transponder, Terminal terminal, DateTime checkTime)
        {
            bool access = false;

            try
            {
                foreach (Access x in AccessList)
                {
                    if (transponder.Transponder_ID == x.Transponder_ID && terminal.Terminal_ID == x.Terminal_ID
                        && checkTime >= x.AccessFrom && checkTime <= x.AccessUntil)
                    {
                        access = true;
                    }
                }
            }
            catch
            {
                string messageBoxText = "Zu dem ausgewählten Transponder und Terminal" +
                    "liegen keine Zutritts-Daten in der Datenbank vor!";
                string caption = "Keine Einträge in der Datenbank";
                _ = MessageBox.Show(messageBoxText, caption);
            }

            // If access = true => mark transponder and terminal green, else => mark them red
            if (access)
            {
                lastTransponder.Background = Brushes.Green;
                lastTerminal.Background = Brushes.Green;
            }
            else
            {
                lastTransponder.Background = Brushes.Red;
                lastTerminal.Background = Brushes.Red;
            }

            return access;
        }


        // Delete ResultIssue only when ResultList.Count > 0
        private void DeleteResultIssue(object sender, RoutedEventArgs e)
        {
            if (ResultList.Count > 0)
            {
                // Show warning-MessageBox befor delete ResultIssue
                string messageBoxText = "Die gesamte Resultat-Liste löschen?";
                string caption = "Resultat-Liste löschen";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult dialog = MessageBox.Show(messageBoxText, caption, button, icon);

                if (dialog == MessageBoxResult.Yes)
                {
                    if (lastTransponder != null)
                    {
                        lastTransponder.Background = Brushes.White;
                    }

                    if (lastTerminal != null)
                    {
                        lastTerminal.Background = Brushes.White;
                    }

                    ResultList.Clear();
                    ResultIssue.ItemsSource = null;
                    CurrentTransponder = null;
                    lastTransponder = null;
                    CurrentTerminal = null;
                    lastTerminal = null;
                }
            }
        }


        // Cancel LoadMenue
        private void CancelLoadMenue(object sender, RoutedEventArgs e)
        {
            LoadMenu.Visibility = Visibility.Hidden;
        }


        // Check all Transponder
        private void CheckAllTransponder(object sender, RoutedEventArgs e)
        {
            foreach (Transponder x in TransponderList)
            {
                x.Checkbox.IsChecked = true;
            }
        }

        // Uncheck all Transponder
        private void UncheckAllTransponder(object sender, RoutedEventArgs e)
        {
            foreach (Transponder x in TransponderList)
            {
                x.Checkbox.IsChecked = false;
            }
        }

        // Check single Transponder
        private void CheckSingleTransponder(object sender, RoutedEventArgs e)
        {
            foreach (Transponder x in TransponderList)
            {
                if (x.Transponder_ID == (int)((CheckBox)sender).Content)
                {
                    x.Checkbox.IsChecked = true;
                }
            }
        }

        // Uncheck single Transponder
        private void UncheckSingleTransponder(object sender, RoutedEventArgs e)
        {
            foreach (Transponder x in TransponderList)
            {
                if (x.Transponder_ID == (int)((CheckBox)sender).Content)
                {
                    x.Checkbox.IsChecked = false;
                }
            }
        }

        // Check all Terminals
        private void CheckAllTerminals(object sender, RoutedEventArgs e)
        {
            foreach (Terminal x in TerminalList)
            {
                x.Checkbox.IsChecked = true;
            }
        }

        // Uncheck all Terminals
        private void UncheckAllTerminals(object sender, RoutedEventArgs e)
        {
            foreach (Terminal x in TerminalList)
            {
                x.Checkbox.IsChecked = false;
            }
        }

        // Check single Terminal
        private void CheckSingleTerminal(object sender, RoutedEventArgs e)
        {
            foreach (Terminal x in TerminalList)
            {
                if (x.Terminal_ID == (int)((CheckBox)sender).Content)
                {
                    x.Checkbox.IsChecked = true;
                }
            }
        }

        // Uncheck single Terminal
        private void UncheckSingleTerminal(object sender, RoutedEventArgs e)
        {
            foreach (Terminal x in TerminalList)
            {
                if (x.Terminal_ID == (int)((CheckBox)sender).Content)
                {
                    x.Checkbox.IsChecked = false;
                }
            }
        }

        //private static void ExportResultsToPdf(List<Result> resList, string filename)
        //{
        //    // Create PDF Document
        //    PdfDocument document = new();
        //    PdfPage page = document.AddPage();
        //    XGraphics gfx = XGraphics.FromPdfPage(page);
        //    XFont font = new("Arial", 9, XFontStyle.Regular);


        //    // Y Coordinate. This variable is incremented to move the rows.
        //    int yCord = 0;

        //    // Run through results and add rows to the PDF.
        //    foreach (Result res in resList)
        //    {
        //        yCord += 10;
        //        string resString = res.TransponderID.ToString() + " " + res.TerminalID.ToString() + ", "
        //            + res.TerminalPlace + ", " + res.Date.ToString() + ", " + res.Time.ToString() + ", "
        //            + res.AccessIssue + ", " + res.EmployeeID.ToString() + ", " + res.Forename + ", " + res.Surname;

        //        gfx.DrawString(resString, font, XBrushes.Black, new XRect(0, yCord, page.Width, page.Height), XStringFormats.TopLeft);
        //    }

        //    //Save PDF File
        //    document.Save(filename);
        //}

        private void SaveResultList(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string filename = saveFileDialog.FileName;
                //ExportResultsToPdf(ResultList, filename);
            }
        }

    }
}
