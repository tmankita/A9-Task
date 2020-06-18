
namespace ex1
{
    class Program
    {
        /*
         * valid Date format: 2018-03-29Th07:35
         * Assume INPUT and OUTPUT exist on the local macine.
         */

        static void Main(string[] args)
        {
            Content content = new Content();
            content.createDB(@"/Users/tmankita/Desktop/INPUT.txt");
            content.writeToFile(@"/Users/tmankita/Desktop/OUTPUT.txt");

        }
    }
}
