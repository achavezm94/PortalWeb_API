namespace PortalWeb_API.Methods
{
    public class HoraActual
    {
        public DateTime HoraActualProceso()
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            DateTime timeUtc = DateTime.UtcNow;
            //DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
        }
    }
}
