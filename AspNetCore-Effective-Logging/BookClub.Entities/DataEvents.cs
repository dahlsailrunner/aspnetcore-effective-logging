using Microsoft.Extensions.Logging;

namespace BookClub.Entities
{
    public class DataEvents
    {
        public static EventId GetMany = new EventId(10001, "GetManyFromProc");
    }
}
