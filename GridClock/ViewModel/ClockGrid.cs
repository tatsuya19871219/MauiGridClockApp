using Microsoft.Maui.Controls.Shapes;

namespace GridClock.ViewModel
{
    public class ClockGrid
    {
        Grid _parent;
        List<Grid> _subGrids;

        Color _baseColor = Colors.Black;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public int Counts => Rows * Columns;

        public ClockGrid(Grid parent, int height)
        {
            _parent = parent;

            SetHeight(height);
        }

        public void SetBaseColor(Color baseColor) => _baseColor = baseColor;

        public void SetHeight(int height) => _parent.HeightRequest = height;

        public void SetGridSize(int rows, int columns)
        {
            Rows = rows; Columns = columns;

            _parent.WidthRequest = _parent.HeightRequest / rows * columns;

            for (int i = 0; i < rows; i++) _parent.AddRowDefinition(new RowDefinition());
            for (int j = 0; j < columns; j++) _parent.AddColumnDefinition(new ColumnDefinition());

            // test
            //for (int k = 0; k < rows * columns; k++) _parent.Add(new Rectangle { Fill = Colors.Red }, k%columns, k/columns);

        }

        public void Initialize()
        {
            _parent.Clear();
            _subGrids = new List<Grid>();


            for (int k = 0; k < Counts; k++)
            {
                Grid grid = new Grid();
                grid.HeightRequest = _parent.HeightRequest / 3;

                for (int i = 0; i < 3; i++) grid.AddRowDefinition(new RowDefinition());
                for (int j = 0; j < 3; j++) grid.AddColumnDefinition(new ColumnDefinition());

                _subGrids.Add(grid);

                _parent.Add(_subGrids[k], k % Columns, k / Columns);
            }
        }

        public void Refresh()
        {
            for (int k = 0; k < Counts; k++) RefreshAt(k);
        }

        private void RefreshAt(int k)
        {
            _subGrids[k].Clear();
        }

        public void FillAt(int k, int sub)
        {
            _subGrids[k].Add(new Rectangle { Fill = _baseColor }, sub % 3, sub / 3);
        }

        public void FillAll(int k)
        {
            RefreshAt(k);

            Rectangle rect = new Rectangle { Fill = _baseColor };
            Grid.SetRowSpan(rect, 3);
            Grid.SetColumnSpan(rect, 3);

            _subGrids[k].Add(rect);
        }

    }
}