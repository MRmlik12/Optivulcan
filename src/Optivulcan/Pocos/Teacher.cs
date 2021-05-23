namespace Optivulcan.Pocos
{
    public class Teacher
    {
        public string Initials { get; set; }
        public string Href { get; set; }

        public override string ToString() => Initials;
    }
}