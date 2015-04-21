using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace ExampleOauthClient
{
    public partial class Default : System.Web.UI.Page
    {
        private string consumerKey = string.Empty;
        private string consumerSecret = string.Empty;

        private string requestTokenEndpoint = string.Empty;
        private string userAuthorizationEndpoint = string.Empty;
        private string accessTokenEndpoint = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Make sure all config files are set up

            // ConsumerKey is the unique key Groundspeak provides to the Consumer
            if (ConfigurationManager.AppSettings["ConsumerKey"] == null)
            {
                throw new ArgumentException("there is no ConsumerKey setting in the web.config");
            }

            // ConsumerSecret is a second piece of identification to help verify the Consumer's identity
            if (ConfigurationManager.AppSettings["ConsumerSecret"] == null)
            {
                throw new ArgumentException("there is no ConsumerSecret setting in the web.config");
            }

            // The Url from which we can ask for and receive a Request Token
            if (ConfigurationManager.AppSettings["OAuthRequestTokenEndpoint"] == null)
            {
                throw new ArgumentException("there is no Request EndPoint setting in the web.config");
            }

            // The Url from which we can authorize our Request Token
            if (ConfigurationManager.AppSettings["OAuthUserAuthorizationEndpoint"] == null)
            {
                throw new ArgumentException("there is no User Authorization EndPoint setting in the web.config");
            }

            // The Url from which we can ask for and receive an Access Token
            if (ConfigurationManager.AppSettings["OAuthAccessTokenEndpoint"] == null)
            {
                throw new ArgumentException("there is no Access Token EndPoint setting in the web.config");
            }

            // get endpoints from web.config
            requestTokenEndpoint = ConfigurationManager.AppSettings["OAuthRequestTokenEndpoint"].ToString();
            userAuthorizationEndpoint = ConfigurationManager.AppSettings["OAuthUserAuthorizationEndpoint"].ToString();
            accessTokenEndpoint = ConfigurationManager.AppSettings["OAuthAccessTokenEndpoint"].ToString();

            // get consumer key and secret from web.config
            consumerKey = ConfigurationManager.AppSettings["ConsumerKey"].ToString();
            consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"].ToString();

            if (!IsPostBack)
            {
                PopulateControls();
            }
        }

        private void PopulateControls()
        {
            // add values to page controls

            this.uxRequestTokenLabel.Text = "Request Token Endpoint:";
            this.uxUserAuthorizationLabel.Text = "User Authorization Endpoint:";
            this.uxAccessTokenLabel.Text = "User Authorization Endpoint:";

            this.uxConsumerKey.Text = consumerKey;
            this.uxConsumerSecret.Text = consumerSecret;
            this.uxRequestTokenEndpoint.Text = requestTokenEndpoint;
            this.uxUserAuthorizationEndpoint.Text = userAuthorizationEndpoint;
            this.uxAccessTokenEndpoint.Text = accessTokenEndpoint;

            if (Session["WcfTokenManager"] != null)
            {
                WebConsumer consumer = this.CreateConsumer();
                var accessTokenMessage = consumer.ProcessUserAuthorization();
                if (accessTokenMessage != null)
                {
                    Session["WcfAccessToken"] = accessTokenMessage.AccessToken;
                    this.uxAuthorizationLabel.Text = "Authorized!  Access token: " + accessTokenMessage.AccessToken;

                }
            }
        }

        protected void uxAuthorizationButton_Click(object sender, EventArgs e)
        {
            WebConsumer consumer = this.CreateConsumer();
            UriBuilder callback = new UriBuilder(Request.Url); // the groundspeak oauth handler will call back to this page

            callback.Query = null;
            // At this time Groundspeak is not distinguishing permissions for its API. "All" is the only setting, but you must include the "scope" parameter

            //string scope = "All";
            //string locale = "fr-FR";
            //var requestParams = new Dictionary<string, string> {
            //    { "scope", scope}, {"locale", locale }
            //};

            string scope = "All";
            var requestParams = new Dictionary<string, string> {
                { "scope", scope}
            };

            try
            {
                // send initial request to and receive response from service provider
                // must send consumersecret and consumerkey in this call

                var response = consumer.PrepareRequestUserAuthorization(callback.Uri, requestParams, null);

                // immediately send response back to service provider to request 
                consumer.Channel.Send(response);
            }
            catch (Exception ex)
            {
                uxResults.Text = "<br />" + ex.Message + "<br /><br />";
            }
        }

        private WebConsumer CreateConsumer()
        {
            MessageReceivingEndpoint oauthRequestTokenEndpoint;
            MessageReceivingEndpoint oauthUserAuthorizationEndpoint;
            MessageReceivingEndpoint oauthAccessTokenEndpoint;

            // use Post Requests and appropriate endpoints
            oauthRequestTokenEndpoint = new MessageReceivingEndpoint(new Uri(requestTokenEndpoint), HttpDeliveryMethods.PostRequest);
            oauthUserAuthorizationEndpoint = new MessageReceivingEndpoint(new Uri(userAuthorizationEndpoint), HttpDeliveryMethods.PostRequest);
            oauthAccessTokenEndpoint = new MessageReceivingEndpoint(new Uri(accessTokenEndpoint), HttpDeliveryMethods.PostRequest);

            // in memory token manager should not be used in production. an actual database should be used instead to remember a user's tokens
            var tokenManager = Session["WcfTokenManager"] as InMemoryTokenManager;
            if (tokenManager == null)
            {
                tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);
                Session["WcfTokenManager"] = tokenManager;
            }

            // set up web consumer
            WebConsumer consumer = new WebConsumer(
                new ServiceProviderDescription
                {
                    RequestTokenEndpoint = oauthRequestTokenEndpoint,
                    UserAuthorizationEndpoint = oauthUserAuthorizationEndpoint,
                    AccessTokenEndpoint = oauthAccessTokenEndpoint,
                    TamperProtectionElements = new DotNetOpenAuth.Messaging.ITamperProtectionChannelBindingElement[] {
					    new HmacSha1SigningBindingElement(),
				    },
                },
                tokenManager);

            return consumer;
        }
    }
}