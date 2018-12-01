namespace Dau.ORM
{
    public class ColumnInfo
    {
        public ColumnInfo()
        {

        }

        public ColumnInfo(string COLUMN_NAME, string Operator, params string[] Operator_values)
        {
            this.COLUMN_NAME = COLUMN_NAME;
            this.Operator = Operator;
            this.Operator_values = Operator_values;
        }


        public string Operator { get; set; }
        public string COLUMN_NAME { get; set; }

        public string COLUMN_VALUE { get; set; }

        public string[] Operator_values { get; set; }
    }
}