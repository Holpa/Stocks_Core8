namespace api
{
    public class APIendPoints
    {
        public static IResult GetHelloMessage(HttpContext context)
        {
            return Results.Ok("Hello World End Point");
        }

    }
}