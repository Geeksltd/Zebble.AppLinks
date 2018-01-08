AppLinks
----------
AppLinks is a Zebble implementation of facebook AppLinks. AppLinks is an open source and cross-platform library for linking mobile and web application. It is available on NuGet and implemented for Android, iOS and UWP platforms.

### What is AppLinks? 
App Links are a defined set of metadata that can be advertised within HTML of web pages that specify how to deep link to content inside of a Mobile app.

- **Mobile Deep Linking from the Web**: Web pages can advertise special <metadata ... /> tags within a normal web page, which specify how to deep link to content inside of a particular Mobile app.
- **Mobile to Mobile Linking**: Mobile apps can resolve meta-data from Web links into links for other mobile apps.
### How to use AppLinks in Zebble?
To use AppLinks in your Zebble application you can use below code to navigate to an URL which can be a web page or an application with specific URL and Identifier:

```csharp
var result = await AppLinks.Navigate(url);
```

The result is a type of “NavigationResult” in Zebble and it can be one of below options:

0)    Failed
1)    Web
2)    App

If the result is 0 or Failed, this means the metadata on the webpage is wrong or the application or webpage could not be found. Web result means the URL is referred to a webpage and the App result is refer to an application.

### Handling Incoming AppLinks

To handle the incoming links you do not need to use different code in all platforms but you need to make some changes in all platform to configure your application or web link. You can use below code to receive all incoming requests with parameters:

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

After that configuration, you can use “OnAppLinkReceived” event like Android platform.

##### UWP:

In UWP platform you need to add a protocol extension into the UWP manifest file and set a name for your application like below:

```xml
<Package ... >
    ...
    ...
    <Applications>
        <Application Id="App" ... >
            <Extensions>
            <uap:Extension Category="windows.protocol">
              <uap:Protocol Name="com.example" />
            </uap:Extension>
          </Extensions>
        </Application>
        ...
    </Applications>
    ..
</Package>
```
 
After that, you can navigate to an URL like other platforms and receive an incoming request from other applications by using "OnAppLinkReceived" event.

### App Link Specification

To create and advertise your application in website and open the application you can use below meta-tags for each platform:
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
    
    <!-- Windows -->
    <meta property="al:windows_phone:url" content="com.example://products?id=widget" />
    <meta property="al:windows_phone:app_name" content="Example Store" />
    <meta property="al:windows_phone:app_id" content="a14e93aa-27c7-df11-a844-00237de2db9f" />
</head>
</html>
```