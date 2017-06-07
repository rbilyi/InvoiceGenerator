namespace Nakladna.CommonData
{
    public class SheetItem
    {
        private string text;

        public int Index { get; private set; }
        public SheetItem(int index, string text)
        {
            Index = index;
            this.text = text;
        }

        public override string ToString()
        {
            return Index + " " + text;
        }
    }
}
