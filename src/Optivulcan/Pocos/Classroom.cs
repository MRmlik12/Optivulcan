namespace Optivulcan.Pocos
{
    public class Classroom
    {
        public string ClassroomNumber { get; set; }
        public string ClassroomHref { get; set; }

        public override string ToString() => ClassroomNumber;
    }
}