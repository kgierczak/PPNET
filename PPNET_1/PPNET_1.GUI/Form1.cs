namespace PPNET_1.GUI;

public partial class Form1 : Form
{
    private TextBox txtSeed;
    private TextBox txtNumberOfItems;
    private TextBox txtCapacity;
    private Button btnSolve;
    private TextBox txtProblem;
    private TextBox txtResult;
    private Label lblSeed;
    private Label lblNumberOfItems;
    private Label lblCapacity;
    private Label lblProblem;
    private Label lblResult;

    public Form1()
    {
        InitializeComponent();
        InitializeCustomControls();
    }

    private void InitializeCustomControls()
    {
        this.Text = "Knapsack Problem";
        this.Size = new Size(800, 600);

        lblSeed = new Label { Text = "Ziarno (seed):", Location = new Point(20, 20), AutoSize = true };
        txtSeed = new TextBox { Location = new Point(200, 20), Width = 150 };

        lblNumberOfItems = new Label { Text = "Liczba przedmiotów:", Location = new Point(20, 50), AutoSize = true };
        txtNumberOfItems = new TextBox { Location = new Point(200, 50), Width = 150 };

        lblCapacity = new Label { Text = "Pojemność:", Location = new Point(20, 80), AutoSize = true };
        txtCapacity = new TextBox { Location = new Point(200, 80), Width = 150 };

        btnSolve = new Button { Text = "Generuj i rozwiąż", Location = new Point(370, 20), Width = 150, Height = 80 };
        btnSolve.Click += BtnSolve_Click;

        lblProblem = new Label { Text = "Wygenerowany Problem:", Location = new Point(20, 120), AutoSize = true };
        txtProblem = new TextBox 
        { 
            Location = new Point(20, 150), 
            Width = 740, 
            Height = 150, 
            Multiline = true, 
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 9)
        };

        lblResult = new Label { Text = "Rozwiązanie:", Location = new Point(20, 320), AutoSize = true };
        txtResult = new TextBox 
        { 
            Location = new Point(20, 350), 
            Width = 740, 
            Height = 150, 
            Multiline = true, 
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 9)
        };

        this.Controls.Add(lblSeed);
        this.Controls.Add(txtSeed);
        this.Controls.Add(lblNumberOfItems);
        this.Controls.Add(txtNumberOfItems);
        this.Controls.Add(lblCapacity);
        this.Controls.Add(txtCapacity);
        this.Controls.Add(btnSolve);
        this.Controls.Add(lblProblem);
        this.Controls.Add(txtProblem);
        this.Controls.Add(lblResult);
        this.Controls.Add(txtResult);
    }

    private void BtnSolve_Click(object sender, EventArgs e)
    {
        try
        {
            txtSeed.BackColor = Color.White;
            txtNumberOfItems.BackColor = Color.White;
            txtCapacity.BackColor = Color.White;

            if (!int.TryParse(txtSeed.Text, out int seed))
            {
                txtSeed.BackColor = Color.LightPink;
                MessageBox.Show("Proszę wprowadzić prawidłową liczbę całkowitą dla ziarna.", "Nieprawidłowe dane", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtNumberOfItems.Text, out int numberOfItems) || numberOfItems <= 0)
            {
                txtNumberOfItems.BackColor = Color.LightPink;
                MessageBox.Show("Proszę wprowadzić liczbę dodatnią dla liczby przedmiotów.", "Nieprawidłowe dane", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity) || capacity < 0)
            {
                txtCapacity.BackColor = Color.LightPink;
                MessageBox.Show("Proszę wprowadzić nieujemną liczbę dla pojemności.", "Nieprawidłowe dane", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PPNET_1.Problem problem = new PPNET_1.Problem(numberOfItems, seed);
            txtProblem.Text = problem.ToString();

            PPNET_1.Result result = problem.Solve(capacity);
            txtResult.Text = $"Pojemność: {capacity}\n\n{result.ToString()}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

