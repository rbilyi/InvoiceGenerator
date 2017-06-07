using System.ComponentModel.DataAnnotations.Schema;

namespace Nakladna.CommonData
{
    public partial class Customer
    {
        public override bool Equals(object obj)
        {
            return obj is Customer && Name.Equals(((Customer)obj).Name, System.StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class SpecialPrice
    {
        [NotMapped]
        public string CustomerName
        {
            get
            {
                return Customer.Name;
            }
        }
    }

    public partial class GoodType
    {
        [NotMapped]
        public bool HasReturn
        {
            get
            {
                return ReturnColumn.HasValue;
            }

        }

        [NotMapped]
        public string ColumnName
        {
            get
            {
                return Utils.Excell.GetExcelColumnName(ColumnInDocument);
            }
            set
            {
                ColumnInDocument = Utils.Excell.GetColumnNumber(value);
            }
        }

        [NotMapped]
        public string ReturnColumnName
        {
            get
            {

                return ReturnColumn.HasValue
                    ? Utils.Excell.GetExcelColumnName(ReturnColumn.Value)
                    : string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ReturnColumn = null;
                }
                else
                {
                    ReturnColumn = Utils.Excell.GetColumnNumber(value);
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
