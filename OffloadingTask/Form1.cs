namespace OffloadingTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => ShowMessage("First Message.", 3000));
            thread.Start();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => ShowMessage("Second Message.", 5000));
            thread.Start();
        }

        private void ShowMessage(string message, int delay)
        {
            Thread.Sleep(delay);

            // Check if invoke is required
            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke(new Action(() => lblMessage.Text = message));
            }
            else
            {
                lblMessage.Text = message;
            }

            /*
                InvokeRequired: checks if you're on a different thread.

                Invoke: runs the given code on the UI thread.
             */
        }

    }
}
