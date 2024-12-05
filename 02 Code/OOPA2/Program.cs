namespace OOPA2
{

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            FrmLogIn loginForm = new FrmLogIn();

            // Subscribe to the FormClosed event
            loginForm.FormClosed += LoginForm_FormClosed;


            Application.Run(loginForm);
        }

        // Event handler for the FormClosed event
        private static void LoginForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit();
            // Configure the process start to be silent.
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c taskkill /IM OOPA2.exe /F",
                UseShellExecute = false,
                CreateNoWindow = true, // Prevent creating a window
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden // Hide the window
            };

            // Start the process with the configured settings.
            System.Diagnostics.Process.Start(startInfo);
            Application.Exit();
        }

    }
}
