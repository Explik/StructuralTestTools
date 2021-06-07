using OleVanSanten.TestTools.MSTest;

namespace Lecture_3_Tests
{
    public static class TestHelper
    {
        static TestHelper()
        {
            // To force the inclusion of the Lecture 3 Exercises assembly
            // a class from this assembly is referenced 
            Lecture_3.Program.Main(new string[0]);
        }

        public static TestFactory Factory { get; } = TestFactory.CreateFromConfigurationFile("./../../../TestToolsConfig.xml");
    }
}
