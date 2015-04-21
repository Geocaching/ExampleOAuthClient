<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExampleOauthClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <p>
        <span class="bold">Instructions:</span></p>
    <p>
        <ol>
            <li>Click the "Get Authorization" Button. This will take you to the Geocaching Staging
                site where you will be asked to sign in to Geocaching.com.</li>
            <li>Sign in using a Premium Geocaching Account. If you are already signed in to Geocaching.com,
                this step will automatically be skipped.</li>
            <li>You will be presented with the option to allow or deny access to this test application
                to make Geocaching API calls on your behalf. Select "Allow Access".</li>
            <li>You will be returned to the test client and now will be able to make an API call
                by clicking the "Make API Call" button. A list of Geocache logs will be returned
                if successful, and "not authorized" will be returned if you have not properly authorized
                your account.</li>
        </ol>
    </p><br />
    <fieldset title="Authorization">
        <p>
            <span class="bold">Step One</span>: Click the "Get Authorization" Button.</p>
        <asp:Button ID="uxAuthorizationButton" runat="server" Text="Get Authorization" OnClick="uxAuthorizationButton_Click" />
        <br />
        <blockquote>
            The following parameters are being used for the OAuth authorization process (handshake)
            and API call:<br />
            <br />
            <span class="bold">Endpoints:</span><br />
            <asp:Label ID="uxRequestTokenLabel" runat="server" />&nbsp;<span class="bold"><asp:Label
                ID="uxRequestTokenEndpoint" runat="server" /></span><br />
            <asp:Label ID="uxUserAuthorizationLabel" runat="server" />&nbsp;<span class="bold"><asp:Label
                ID="uxUserAuthorizationEndpoint" runat="server" /></span><br />
            <asp:Label ID="uxAccessTokenLabel" runat="server" />&nbsp;<span class="bold"><asp:Label
                ID="uxAccessTokenEndpoint" runat="server" /></span><br />
            <br />
            <asp:Label ID="uxConsumerKeyLabel" runat="server" Text="Consumer Key:" />
            <span class="bold">
                <asp:Label ID="uxConsumerKey" runat="server" /></span>
            <br />
            <asp:Label ID="uxConsumerSecretLabel" runat="server" Text="Consumer Secret:" />
            <span class="bold">
                <asp:Label ID="uxConsumerSecret" runat="server" /></span>
            <br />
        </blockquote>
        <asp:Label class="success" ID="uxAuthorizationLabel" runat="server" />
    </fieldset>
    <br />    
<%--    <fieldset title="ApiCall">
        <p>
            <span class="bold">Step Two:</span> Call the Api</p>
        <p class="description">
            If you call the Api before you get authorization, you will get a "not authorized"
            error. If the call is successful, you will get a list of Geocache logs.</p>
        <asp:Button ID="uxCallApi" runat="server" OnClick="uxCallApi_Click" Text="Make Api Call" /><br /><br />
    </fieldset>--%>
    <asp:Label ID="uxResults" runat="server" />
    </form>
</body>
</html>
