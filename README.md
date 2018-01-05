AppLinks
----------
AppLinks is a Zebble implementation of Rivets. It is an open source and cross platform for linking mobile applications and web application. Also, it available on NuGet and implemented for Android and IOS platform.

### What is/are AppLinks? 
App Links are a defined set of metadata that can be advertised within html of web pages that specify how to deep link to content inside of a Mobile app.

App Links are about the ***discovery*** of ways to link between Mobile and Web.

- **Mobile Deep Linking from the Web** - Web pages can advertise special <metadata ... /> tags within a normal web page, which specify how to deep link to content inside of a particular Mobile app.
- **Mobile to Mobile Linking** - Mobile apps can resolve meta-data from Web links into links for other mobile apps.
### How to use AppLinks in Zebble?
To using AppLinks in your Zebble application you can use below code to navigate to an URL which can be a web page or an application with specific URL and Identifier:

```csharp
var result = await AppLinks.Navigate(url);
```

The result is a type of “NavigationResult” in Zebble and it can be one of below options:

0)	Failed
1)	Web
2)	App

If the result is 0 or Failed, this means the metadata on the webpage are wrong or the application or webpage not found. Web result means the URL is refer to a webpage and the App result is refer to an application.

### Handling Incoming AppLinks

To handling the incoming links you do not need to use different code in all platforms but you need to make some changes in all platform to configure your application or web link. You can use below code to receive all incoming requests with parameters:

```csharp
AppLinks.OnAppLinkReceived.Handle(parameters =>
{
     //Do something with parameters (call some page or something else)
});
```

##### Android:

Firstly, you need to add this attribute to MainActivity class of Zebble Android application like below:

```csharp
[IntentFilter(new[] { Android.Content.Intent.ActionView },
Categories = new[] { Android.Content.Intent.CategoryBrowsable, Android.Content.Intent.CategoryDefault },
DataScheme = "http or https", DataHost = "web address or *", 
AutoVerify = true)]
```

Then you can use “OnAppLinkReceived” event of Zebble AppLinks in anywhere you want like page initialization to receive incoming requests.
 
##### IOS:

In IOS platform you need to set the scheme and application URL in the “Info.plist” file like below:

```xml
<plist version="1.0">
  <dict>
	…
    <key>CFBundleURLTypes</key>
    <array>
      <dict>
        <key>CFBundleURLName</key>
        <string>com.companyname.example</string>
        <key>CFBundleURLTypes</key>
        <string>Viewer</string>
        <key>CFBundleURLSchemes</key>
        <array>
          <string>example</string>
        </array>
      </dict>
    </array>
  </dict>
</plist>
```

After that configuration you can use “OnAppLinkReceived” event like Android platform.
 
### App Link Specification

To create and advertise your application in web site and open the application you can use below meta-tags for each platform:
```html
<html>
<head>
	<!-- iOS -->
	<meta property="al:ios:url" content="example://products?id=widget" />
	<meta property="al:ios:app_name" content="Example Store" />
	<meta property="al:ios:app_store_id" content="12345" />
	
	<!-- Android -->
	<meta property="al:android:package" content="com.example" />
	<meta property="al:android:url" content="example://products?id=widget" />
	<meta property="al:android:app_name" content="Example Store" />
</head>
</html>
```