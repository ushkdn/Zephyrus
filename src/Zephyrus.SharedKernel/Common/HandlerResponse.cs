namespace Zephyrus.SharedKernel.Common;

public record HandlerResponse<T>(T? Data, string? Message, bool Success);