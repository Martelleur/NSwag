using MyNamespace;
using open.api.tool.Api;

// ====================================
// Valid and invalid user name to check
// ====================================
var userNameValid = "robert123";
var userNameInvalid = "robert.123";


// ===================
// USING OPEN API TOOL
// ===================
try
{
    Console.WriteLine("""
 
    ===================
    USING OPEN API TOOL
    ===================
    """);
    var tempApi = new TestControllerVersionTwoApi("http://localhost:5099");
    Console.WriteLine($"tempApi.Configuration.BasePath: {tempApi.Configuration.BasePath}");

    // Use correct user name unPatterned
    Console.WriteLine($"\nTry unPattern pinging valid user name {userNameValid}...");
    var resTempApiValidUsernam_unPatterned = await tempApi.PingUserNameV2Async(userNameValid);
    Console.WriteLine($"Ping user name: {resTempApiValidUsernam_unPatterned.UserName}");
    Console.WriteLine(resTempApiValidUsernam_unPatterned.UserName);

    // Fail at server
    // Use incorrect user name unPatterned
    Console.WriteLine($"\nTry unPattern pinging invalid user name {userNameInvalid}...");
    var resTempApiInvalidUsernam_unPatterned = await tempApi.PingUserNameV2Async(userNameInvalid);
    Console.WriteLine($"Ping user name: {resTempApiInvalidUsernam_unPatterned.UserName}");
    Console.WriteLine(resTempApiInvalidUsernam_unPatterned.UserName);
}
catch (Exception e) { Console.WriteLine(e.StackTrace); }


//===================
//USING NSWAG
//===================
try
{
    Console.WriteLine("""
 
    ===================
    USING NSWAG
    ===================
    """);
    var httpClient = new HttpClient();
    var client = new Client("http://localhost:5099/", httpClient);

    // Use correct user name unPatterned
    Console.WriteLine($"\nTry pinging valid user name {userNameValid}...");
    var resSuccess_unPatterned = await client.PingUserNameV2Async(userNameValid);
    Console.WriteLine($"Ping user name: {resSuccess_unPatterned.UserName}");
    Console.WriteLine(resSuccess_unPatterned.UserName);

    // Fail at client
    // Use incorrect user name
    Console.WriteLine($"\nTry pinging invalid user name {userNameInvalid}...");
    var resFailure_unPatterned = await client.PingUserNameV2Async(userNameInvalid);
    Console.WriteLine($"Ping user name: {resFailure_unPatterned.UserName}");
    Console.WriteLine(resFailure_unPatterned.UserName);
}
catch (Exception e) { Console.WriteLine(e.StackTrace); }
