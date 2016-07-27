using System;
using System.Linq;

namespace Nakladna.Gdoc
{
    public class Rocker
    {
        public string site { get; set; }
        public string strippedSite { get; set; }
        public string url { get; set; }
        public string twitter { get; set; }
    }

    public class SpreadSheet
    {
        public static void Go()
        {
            ////////////////////////////////////////////////////////////////////////////
            // STEP 1: Configure how to perform OAuth 2.0
            ////////////////////////////////////////////////////////////////////////////

            // TODO: Update the following information with that obtained from
            // https://code.google.com/apis/console. After registering
            // your application, these will be provided for you.

            string CLIENT_ID = "345296030463-604sndjpto5n8inaonllv89aga05aq7n.apps.googleusercontent.com";

            // This is the OAuth 2.0 Client Secret retrieved
            // above.  Be sure to store this value securely.  Leaking this
            // value would enable others to act on behalf of your application!
            string CLIENT_SECRET = "N-zSNIP1mkKrutG2yaBVPsY6";

            // Space separated list of scopes for which to request access.
            string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

            // This is the Redirect URI for installed applications.
            // If you are building a web application, you have to set your
            // Redirect URI at https://code.google.com/apis/console.
            string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

            ////////////////////////////////////////////////////////////////////////////
            // STEP 2: Set up the OAuth 2.0 object
            ////////////////////////////////////////////////////////////////////////////

            // OAuth2Parameters holds all the parameters related to OAuth 2.0.
            OAuth2Parameters parameters = new OAuth2Parameters();

            // Set your OAuth 2.0 Client Id (which you can register at
            // https://code.google.com/apis/console).
            parameters.ClientId = CLIENT_ID;

            // Set your OAuth 2.0 Client Secret, which can be obtained at
            // https://code.google.com/apis/console.
            parameters.ClientSecret = CLIENT_SECRET;

            // Set your Redirect URI, which can be registered at
            // https://code.google.com/apis/console.
            parameters.RedirectUri = REDIRECT_URI;

            ////////////////////////////////////////////////////////////////////////////
            // STEP 3: Get the Authorization URL
            ////////////////////////////////////////////////////////////////////////////

            // Set the scope for this particular service.
            parameters.Scope = SCOPE;

            // Get the authorization url.  The user of your application must visit
            // this url in order to authorize with Google.  If you are building a
            // browser-based application, you can redirect the user to the authorization
            // url.
            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Console.WriteLine(authorizationUrl);
            Console.WriteLine("Please visit the URL above to authorize your OAuth "
              + "request token.  Once that is complete, type in your access code to "
              + "continue...");
            parameters.AccessCode = Console.ReadLine();

            ////////////////////////////////////////////////////////////////////////////
            // STEP 4: Get the Access Token
            ////////////////////////////////////////////////////////////////////////////

            // Once the user authorizes with Google, the request token can be exchanged
            // for a long-lived access token.  If you are building a browser-based
            // application, you should parse the incoming request token from the url and
            // set it in OAuthParameters before calling GetAccessToken().
            OAuthUtil.GetAccessToken(parameters);
            string accessToken = parameters.AccessToken;
            Console.WriteLine("OAuth Access Token: " + accessToken);

            ////////////////////////////////////////////////////////////////////////////
            // STEP 5: Make an OAuth authorized request to Google
            ////////////////////////////////////////////////////////////////////////////

            // Initialize the variables needed to make the request
            GOAuth2RequestFactory requestFactory =
                new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.RequestFactory = requestFactory;

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            SpreadsheetQuery query = new SpreadsheetQuery();

            // Make a request to the API and get all spreadsheets.
            SpreadsheetFeed feed = service.Query(query);

            if (feed.Entries.Count == 0)
            {
                // TODO: There were no spreadsheets, act accordingly.
            }

            // Iterate through all of the spreadsheets returned
            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                // Print the title of this spreadsheet to the screen
                Console.WriteLine(entry.Title.Text);
            }

            // TODO: Choose a spreadsheet more intelligently based on your
            // app's needs.
            SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
            Console.WriteLine(spreadsheet.Title.Text);

            // Make a request to the API to fetch information about all
            // worksheets in the spreadsheet.
            WorksheetFeed wsFeed = spreadsheet.Worksheets;

            // Iterate through each worksheet in the spreadsheet.
            foreach (WorksheetEntry entry in wsFeed.Entries)
            {
                // Get the worksheet's title, row count, and column count.
                //string title = entry.Title.Text;
                //int rowCount = entry.Rows;
                //int colCount = entry.Cols;

                // Print the fetched information to the screen for this worksheet.
                //Console.WriteLine(title + "- rows:" + rowCount + " cols: " + colCount);
            }

            // Make the request to Google
            // See other portions of this guide for code to put here...
            //   UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //new ClientSecrets
            //{
            //    ClientId = "345296030463-604sndjpto5n8inaonllv89aga05aq7n.apps.googleusercontent.com",
            //    ClientSecret = "N-zSNIP1mkKrutG2yaBVPsY6",
            //},
            //new[] { DriveService.Scope.Drive }, "funofpl@gmail.com",
            //CancellationToken.None).Result;

            //   var service = new DriveService(new BaseClientService.Initializer()
            //   {
            //       HttpClientInitializer = credential,
            //       ApplicationName = "docsync-1156",
            //   });
            //   //var response = service.Files.Get("MySheet").Execute();
            //foreach (var item in response.)
            //{
            //    Console.WriteLine(item.Title);
            //}
        }
    }
}
