namespace Optivulcan.Pocos
{
    public class Classroom
    {
        public string ClassroomNumber { get; set; }
        public string Href { get; set; }

        public override string ToString() => ClassroomNumber;
    }
}