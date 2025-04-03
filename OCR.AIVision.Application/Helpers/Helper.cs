namespace OCR.AIVision.Application.Helpers;

public static class Helper
{
    public static string GetErrorMessage(Exception exception) => string.Concat(exception.Message, " ", (exception.InnerException != null ? exception.InnerException.Message : string.Empty));
}