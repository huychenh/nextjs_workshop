namespace CarStore.AppContracts.Common
{
    public record ResultDto<T>(T Data, bool IsError = false, string ErrorMessage = default!) where T : notnull;
    public record ListResultDto<T>(List<T> Items, long TotalItems, int Page, int PageSize) where T : notnull;
}

