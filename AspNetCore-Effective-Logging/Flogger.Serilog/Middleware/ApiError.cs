namespace Flogger.Serilog.Middleware
{
    public class ApiError
    {
        public string Id { get; internal set; }
        public short Status { get; internal set; }
        public string Code { get; set; }
        public string Links { get; set; }
        public string Title { get; internal set; }
        public string Detail { get; set; }
    }
}
