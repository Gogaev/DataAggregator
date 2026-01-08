namespace DataAggregator.Application.Services.Abstractions
{
    public interface IAuxiliaryClientCodeGenerator
    {
        string Generate(string firstName, string lastName, string organizationName);
    }
}
