#Example OAuth Client

You will need to do the following things to get the project working:

*web.config changes*

- add your consumer key
- add you consumer secret

Run a build through Visual Studio so the Nuget packages can be restored.

Running the web site click the "Authorize" button.  After going through the validation process on our site you will come back to your page and the access token will be displayed in a label on the bottom of the page.

NOTE: if using the access token in a querystring do not forget to urlenocde it first.