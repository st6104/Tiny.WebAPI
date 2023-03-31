namespace Tiny.Api.Attributes.Conventions;

[AttributeUsage(AttributeTargets.Field)]
sealed class SuccessStatusCodeAttribute : Attribute
{
    public int StatusCode { get; }

    public SuccessStatusCodeAttribute(int statusCode)
    {
        StatusCode = statusCode;
    }
}
