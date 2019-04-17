namespace TagsCloud.ConsoleClient
{
    class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var container = new ContainerBuilder().Build();
            var ui = container.Resolve<IUserInterface>();
            ui.Run(args);
        }
    }
}