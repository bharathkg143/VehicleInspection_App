namespace VIR_WebApp.CustomMiddlewares
{
    public static class DeviceDetectionMiddlewareExtensions
    {
        public static  IApplicationBuilder UseMobileDeviceDetection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DeviceDetectionMiddleware>();
        }
    }
}
