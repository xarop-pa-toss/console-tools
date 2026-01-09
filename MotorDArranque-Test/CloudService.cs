internal class CloudService
{
    public string Name;
    public string Id;
    public decimal HourlyRate;

    public CloudService(string Name, string Id, decimal HourlyRate)
    {
        this.Name = Name;
        this.Id = Id;
        this.HourlyRate = HourlyRate;
    }
}