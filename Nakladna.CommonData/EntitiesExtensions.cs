namespace Nakladna.CommonData
{
    public partial class Customer
    {
        public override bool Equals(object obj)
        {
            return obj is Customer && Name == ((Customer)obj).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17;
        }
    }
}
